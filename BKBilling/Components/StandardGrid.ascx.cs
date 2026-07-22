using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKBilling.Components
{
    public partial class StandardGrid : System.Web.UI.UserControl
    {
        public event EventHandler OnRebind;
        public event EventHandler<GridRowActionEventArgs> RowAction;

        // ---------- SESSION-BACKED STATE (survives F5 refresh) ----------
        // Each grid instance uses its own key so multiple grids on one page don't collide.
        private string SK(string suffix) => "SG_" + this.UniqueID + "_" + suffix;

        private T SessGet<T>(string suffix, T fallback)
        {
            var v = Session?[SK(suffix)];
            return v is T t ? t : fallback;
        }
        private void SessSet(string suffix, object value)
        {
            if (Session == null) return;
            if (value == null) Session.Remove(SK(suffix));
            else Session[SK(suffix)] = value;
        }

        // Cache the last-bound DataTable so Export uses filtered+sorted data
        private DataTable LastBound
        {
            get => ViewState["_LastDT"] as DataTable;
            set => ViewState["_LastDT"] = value;
        }

        // ViewState Safe Properties
        private List<GridColumnDef> ColConfigs
        {
            get => (List<GridColumnDef>)ViewState["_Cols"] ?? new List<GridColumnDef>();
            set => ViewState["_Cols"] = value;
        }

        // Column filters — mirrored to Session for refresh survival
        private Dictionary<string, FilterCriteria> ColFilters
        {
            get
            {
                var vs = (Dictionary<string, FilterCriteria>)ViewState["_Filt"];
                if (vs != null) return vs;
                var sess = SessGet<Dictionary<string, FilterCriteria>>("Filt", null);
                if (sess != null) { ViewState["_Filt"] = sess; return sess; }
                return new Dictionary<string, FilterCriteria>();
            }
            set
            {
                ViewState["_Filt"] = value;
                SessSet("Filt", value);
            }
        }

        private List<GridActionDef> ActionConfigs
        {
            get => (List<GridActionDef>)ViewState["_Acts"] ?? new List<GridActionDef>();
            set => ViewState["_Acts"] = value;
        }

        private string SortExp
        {
            get => (string)ViewState["_Sort"] ?? SessGet<string>("Sort", "");
            set { ViewState["_Sort"] = value; SessSet("Sort", value); }
        }
        private string SortDir
        {
            get => (string)ViewState["_Dir"] ?? SessGet<string>("Dir", "ASC");
            set { ViewState["_Dir"] = value; SessSet("Dir", value); }
        }
        public string KeyField { get => (string)ViewState["_Key"] ?? "Id"; set => ViewState["_Key"] = value; }

        public string DateColumn
        {
            get => (string)ViewState["_DateCol"];
            set => ViewState["_DateCol"] = value;
        }

        // ---------- LIFECYCLE ----------
        protected void Page_Load(object sender, EventArgs e)
        {
            // First load of a fresh page (F5 or navigation): rehydrate visible inputs from Session
            if (!Page.IsPostBack)
            {
                txtSearch.Text = SessGet<string>("Search", "");
                txtFrom.Text = SessGet<string>("From", "");
                txtTo.Text = SessGet<string>("To", "");
                var size = SessGet<string>("Size", "25");
                var item = ddlSize.Items.FindByValue(size);
                if (item != null) ddlSize.SelectedValue = size;
            }
            else
            {
                // Postback: user may have typed something new -> persist current input values
                SessSet("Search", txtSearch.Text);
                SessSet("From", txtFrom.Text);
                SessSet("To", txtTo.Text);
                SessSet("Size", ddlSize.SelectedValue);
            }
        }

        public void Configure(List<GridColumnDef> columns, List<GridActionDef> actions = null, string keyField = "Id")
        {
            this.ColConfigs = columns;
            this.ActionConfigs = actions ?? new List<GridActionDef>();
            this.KeyField = keyField;
            BuildColumns(this.ActionConfigs);
        }

        /// <summary>Clear all persisted state for THIS grid (search, dates, filters, sort, size).</summary>
        public void ResetState()
        {
            foreach (var k in new[] { "Filt", "Sort", "Dir", "Search", "From", "To", "Size" })
                Session?.Remove(SK(k));
            ViewState.Remove("_Filt");
            ViewState.Remove("_Sort");
            ViewState.Remove("_Dir");
            txtSearch.Text = ""; txtFrom.Text = ""; txtTo.Text = "";
            ddlSize.SelectedValue = "25";
        }

        private void BuildColumns(List<GridActionDef> actions)
        {
            gvInternal.Columns.Clear();
            foreach (var col in ColConfigs.Where(x => x.Visible))
            {
                TemplateField tf = new TemplateField
                {
                    HeaderText = col.HeaderText,
                    SortExpression = col.FieldKey,
                    HeaderTemplate = new ProHeaderTemplate(col, this)
                };
                if (!string.IsNullOrEmpty(col.Width)) tf.HeaderStyle.Width = Unit.Parse(col.Width);
                if (!string.IsNullOrEmpty(col.Align))
                    tf.ItemStyle.HorizontalAlign = (HorizontalAlign)Enum.Parse(typeof(HorizontalAlign), col.Align, true);
                tf.ItemTemplate = new ProItemTemplate(col.FieldKey, col.Format, col.Bold);
                gvInternal.Columns.Add(tf);
            }

            if (actions != null && actions.Count > 0)
            {
                TemplateField af = new TemplateField { HeaderText = "Action" };
                af.ItemStyle.Width = Unit.Pixel(80);
                af.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                af.ItemTemplate = new ActionTemplate(actions, KeyField);
                gvInternal.Columns.Add(af);
            }
        }

        public void BindData(DataTable dt)
        {
            // Fix: always ensure paging is on for normal binds (Export toggles it off)
            gvInternal.AllowPaging = true;

            DataTable filtered = ApplyFiltering(dt);
            litTotal.Text = (dt?.Rows.Count ?? 0).ToString();
            litVisible.Text = filtered.Rows.Count.ToString();

            if (!string.IsNullOrEmpty(SortExp))
            {
                DataView dv = filtered.DefaultView;
                dv.Sort = $"{SortExp} {SortDir}";
                filtered = dv.ToTable();
            }

            LastBound = filtered;
            gvInternal.DataSource = filtered;
            int size; if (!int.TryParse(ddlSize.SelectedValue, out size)) size = 25;
            gvInternal.PageSize = size;
            gvInternal.DataBind();
            RestoreUI();
        }

        private DataTable ApplyFiltering(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return dt ?? new DataTable();
            List<string> parts = new List<string>();

            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                string s = txtSearch.Text.Replace("'", "''");
                var search = dt.Columns.Cast<DataColumn>().Where(c => c.DataType == typeof(string))
                              .Select(c => $"[{c.ColumnName}] LIKE '%{s}%'");
                parts.Add("(" + string.Join(" OR ", search) + ")");
            }

            foreach (var f in ColFilters)
            {
                string v = f.Value.Value.Replace("'", "''");
                parts.Add(f.Value.Operator == "Equals"
                    ? $"[{f.Key}] = '{v}'"
                    : $"CONVERT([{f.Key}], System.String) LIKE '%{v}%'");
            }

            if (!string.IsNullOrEmpty(DateColumn))
            {
                if (!string.IsNullOrEmpty(txtFrom.Text)) parts.Add($"[{DateColumn}] >= #{txtFrom.Text}#");
                if (!string.IsNullOrEmpty(txtTo.Text)) parts.Add($"[{DateColumn}] <= #{txtTo.Text}#");
            }

            if (parts.Count == 0) return dt;
            try
            {
                DataRow[] rows = dt.Select(string.Join(" AND ", parts));
                return rows.Length > 0 ? rows.CopyToDataTable() : dt.Clone();
            }
            catch { return dt; }
        }

        public void SetFilter(string field, string op, string val)
        {
            var f = ColFilters;
            if (string.IsNullOrEmpty(val)) f.Remove(field);
            else f[field] = new FilterCriteria { Operator = op, Value = val };
            ColFilters = f;  // setter also writes to Session
            OnRebind?.Invoke(this, EventArgs.Empty);
        }

        private void RestoreUI()
        {
            if (gvInternal.HeaderRow == null) return;
            foreach (var f in ColFilters)
            {
                if (gvInternal.HeaderRow.FindControl("t_" + f.Key) is TextBox t) t.Text = f.Value.Value;
                if (gvInternal.HeaderRow.FindControl("o_" + f.Key) is DropDownList d) d.SelectedValue = f.Value.Operator;
            }
        }

        protected void Refresh_Click(object sender, EventArgs e)
        {
            SessSet("Search", txtSearch.Text);
            SessSet("From", txtFrom.Text);
            SessSet("To", txtTo.Text);
            SessSet("Size", ddlSize.SelectedValue);
            OnRebind?.Invoke(this, e);
        }
        protected void Prev_Click(object sender, EventArgs e) { if (gvInternal.PageIndex > 0) gvInternal.PageIndex--; OnRebind?.Invoke(this, e); }
        protected void Next_Click(object sender, EventArgs e) { gvInternal.PageIndex++; OnRebind?.Invoke(this, e); }

        protected void gvInternal_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDir = (SortExp == e.SortExpression && SortDir == "ASC") ? "DESC" : "ASC";
            SortExp = e.SortExpression;
            OnRebind?.Invoke(this, e);
        }

        protected void gvInternal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sort" || e.CommandName == "Page") return;
            RowAction?.Invoke(this, new GridRowActionEventArgs { ActionKey = e.CommandName, RowKey = e.CommandArgument.ToString() });
        }

        // Safe Excel export – uses filtered+sorted LastBound, no thread-abort, no design breakage.
        protected void Export_Click(object sender, EventArgs e)
        {
            DataTable src = LastBound;
            if (src == null)
            {
                OnRebind?.Invoke(this, e);
                src = LastBound;
                if (src == null) return;
            }

            var visibleCols = ColConfigs.Where(c => c.Visible).ToList();

            HttpResponse resp = HttpContext.Current.Response;
            resp.Clear();
            resp.Buffer = true;
            resp.AddHeader("content-disposition", "attachment;filename=Report.xls");
            resp.Charset = "";
            resp.ContentType = "application/vnd.ms-excel";

            using (var sw = new StringWriter())
            using (var hw = new HtmlTextWriter(sw))
            {
                var tbl = new Table { GridLines = GridLines.Both };
                var hdr = new TableRow();
                foreach (var c in visibleCols)
                    hdr.Cells.Add(new TableHeaderCell { Text = c.HeaderText, BackColor = System.Drawing.Color.LightGray });
                tbl.Rows.Add(hdr);

                foreach (DataRow r in src.Rows)
                {
                    var tr = new TableRow();
                    foreach (var c in visibleCols)
                    {
                        object v = r.Table.Columns.Contains(c.FieldKey) ? r[c.FieldKey] : "";
                        string txt = v == null || v == DBNull.Value ? "" :
                            (!string.IsNullOrEmpty(c.Format) ? string.Format(c.Format, v) : v.ToString());
                        tr.Cells.Add(new TableCell { Text = txt });
                    }
                    tbl.Rows.Add(tr);
                }

                tbl.RenderControl(hw);
                resp.Write(sw.ToString());
            }
            resp.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        private class ProHeaderTemplate : ITemplate
        {
            GridColumnDef _c; StandardGrid _p;
            public ProHeaderTemplate(GridColumnDef c, StandardGrid p) { _c = c; _p = p; }
            public void InstantiateIn(Control container)
            {
                var f = new Panel { CssClass = "header-flex" };
                f.Controls.Add(new LiteralControl($"<span>{_c.HeaderText}</span>"));
                var d = new Panel { CssClass = "menu-dots" };
                d.Attributes.Add("onclick", $"toggleProMenu(event, 'm_{_c.FieldKey}')");
                d.Controls.Add(new LiteralControl("<i class='fas fa-ellipsis-v'></i>"));
                f.Controls.Add(d);

                var menu = new Panel { ID = "m_" + _c.FieldKey, CssClass = "pro-flyout-menu" };
                menu.Controls.Add(CreateLink("Sort Ascending", "Sort", _c.FieldKey, "fas fa-sort-amount-down"));
                menu.Controls.Add(CreateLink("Sort Descending", "Sort", _c.FieldKey, "fas fa-sort-amount-up"));
                menu.Controls.Add(new LiteralControl("<div class='flyout-divider'></div>"));
                var filt = new Panel { CssClass = "menu-item" };
                filt.Attributes.Add("onclick", $"toggleProMenu(event, 'p_{_c.FieldKey}')");
                filt.Controls.Add(new LiteralControl("<i class='fas fa-filter'></i> Filter"));
                menu.Controls.Add(filt);
                f.Controls.Add(menu);

                var pop = new Panel { ID = $"p_{_c.FieldKey}", CssClass = "pro-filter-pop" };
                pop.Attributes.Add("onclick", "stopPopClose(event)");
                pop.Controls.Add(new LiteralControl($"<span class='filter-pop-label'>Is '{_c.HeaderText}':</span>"));
                var ddl = new DropDownList { ID = "o_" + _c.FieldKey, CssClass = "form-select filter-pop-input" };
                ddl.Items.Add(new ListItem("Contains", "Contains"));
                ddl.Items.Add(new ListItem("Exactly", "Equals"));
                pop.Controls.Add(ddl);
                var txt = new TextBox { ID = "t_" + _c.FieldKey, CssClass = "form-control filter-pop-input" };
                pop.Controls.Add(txt);
                var btns = new Panel { CssClass = "d-flex gap-2" };
                var bF = new Button { Text = "FILTER", CssClass = "btn btn-primary btn-sm flex-grow-1", CausesValidation = false };
                bF.Click += (s, e) => _p.SetFilter(_c.FieldKey, ddl.SelectedValue, txt.Text);
                var bC = new Button { Text = "CLEAR", CssClass = "btn btn-light btn-sm flex-grow-1", CausesValidation = false };
                bC.Click += (s, e) => { txt.Text = ""; _p.SetFilter(_c.FieldKey, "Contains", ""); };
                btns.Controls.Add(bF); btns.Controls.Add(bC); pop.Controls.Add(btns);
                f.Controls.Add(pop);

                container.Controls.Add(f);
            }
            private LinkButton CreateLink(string t, string c, string a, string i)
            {
                var l = new LinkButton { CommandName = c, CommandArgument = a, CssClass = "menu-item", CausesValidation = false };
                l.Controls.Add(new LiteralControl($"<i class='{i}'></i> {t}"));
                return l;
            }
        }

        private class ProItemTemplate : ITemplate
        {
            string _k, _fmt; bool _bold;
            public ProItemTemplate(string k, string fmt, bool bold) { _k = k; _fmt = fmt; _bold = bold; }
            public void InstantiateIn(Control c)
            {
                var lbl = new Label();
                lbl.DataBinding += (s, e) =>
                {
                    var l = (Label)s;
                    object v = DataBinder.Eval(((GridViewRow)l.NamingContainer).DataItem, _k);
                    if (v == null || v == DBNull.Value) { l.Text = ""; return; }
                    l.Text = string.IsNullOrEmpty(_fmt) ? v.ToString() : string.Format(_fmt, v);
                    if (_bold) l.Font.Bold = true;
                };
                c.Controls.Add(lbl);
            }
        }

        private class ActionTemplate : ITemplate
        {
            List<GridActionDef> _acts; string _key;
            public ActionTemplate(List<GridActionDef> acts, string key) { _acts = acts; _key = key; }
            public void InstantiateIn(Control c)
            {
                foreach (var a in _acts)
                {
                    var lb = new LinkButton
                    {
                        CommandName = a.Key,
                        ToolTip = a.Tooltip,
                        CssClass = "btn btn-sm btn-link p-1",
                        CausesValidation = false
                    };
                    lb.Controls.Add(new LiteralControl($"<i class='{a.Icon}'></i>"));
                    lb.DataBinding += (s, e) =>
                    {
                        var l = (LinkButton)s;
                        l.CommandArgument = DataBinder.Eval(((GridViewRow)l.NamingContainer).DataItem, _key)?.ToString();
                    };
                    c.Controls.Add(lb);
                    c.Controls.Add(new LiteralControl(" "));
                }
            }
        }
    }
}

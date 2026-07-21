using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BKBilling.Components
{
    public partial class StandardGrid : System.Web.UI.UserControl
    {
        public event EventHandler OnRebind;
        public event EventHandler OnAddClick;
        public event EventHandler<GridRowActionEventArgs> RowAction;

        public string SearchText => txtInternalSearch.Text.Trim();
        public string SortExpression { get => (string)ViewState["_sortExp"]; set => ViewState["_sortExp"] = value; }
        public string SortDirection { get => (string)ViewState["_sortDir"] ?? "ASC"; set => ViewState["_sortDir"] = value; }
        public string NewButtonText { set { litBtnText.Text = value; } }

        private List<GridColumnDef> ColumnConfigs
        {
            get => (List<GridColumnDef>)(ViewState["_colConfigs"] ?? new List<GridColumnDef>());
            set => ViewState["_colConfigs"] = value;
        }

        private Dictionary<string, ColumnFilterState> ColumnFilters
        {
            get => (Dictionary<string, ColumnFilterState>)(ViewState["_colFilters"]) ?? new Dictionary<string, ColumnFilterState>();
            set => ViewState["_colFilters"] = value;
        }

        private List<GridActionDef> _actions
        {
            get => (List<GridActionDef>)(ViewState["_gridActions"] ?? new List<GridActionDef>());
            set => ViewState["_gridActions"] = value;
        }

        private string _keyField
        {
            get => (string)(ViewState["_keyField"] ?? "Id");
            set => ViewState["_keyField"] = value;
        }

        public void Configure(List<GridColumnDef> columns, List<GridActionDef> actions = null, string keyField = "Id")
        {
            _keyField = keyField;
            _actions = actions ?? new List<GridActionDef>();

            if (!IsPostBack)
            {
                ColumnConfigs = columns;
                cblColumns.Items.Clear();
                foreach (var c in columns)
                {
                    ListItem li = new ListItem(c.HeaderText, c.FieldKey);
                    li.Selected = c.Visible;
                    cblColumns.Items.Add(li);
                }
            }
            BuildColumns();
        }

        private void BuildColumns()
        {
            gvInternal.Columns.Clear();
            foreach (var col in ColumnConfigs.Where(x => x.Visible))
            {
                TemplateField tf = new TemplateField { HeaderText = col.HeaderText, SortExpression = col.FieldKey };
                if (!string.IsNullOrEmpty(col.Width)) tf.HeaderStyle.Width = Unit.Parse(col.Width);

                tf.HeaderTemplate = new FilterableHeaderTemplate(col.HeaderText, col.FieldKey, this);

                if (!string.IsNullOrEmpty(col.CssClassFromField))
                    tf.ItemTemplate = new StatusColoredItemTemplate(col.FieldKey, col.CssClassFromField, col.CssClassPrefix, col.Format);
                else
                    tf.ItemTemplate = new BoundItemTemplate(col.FieldKey, col.Format, col.Bold);

                gvInternal.Columns.Add(tf);
            }

            if (_actions?.Count > 0)
            {
                var tf = new TemplateField { HeaderText = "Action", ItemStyle = { CssClass = "text-center", Width = Unit.Pixel(100) } };
                tf.ItemTemplate = new ActionButtonsTemplate(_actions, _keyField);
                gvInternal.Columns.Add(tf);
            }
        }

        public void BindData(DataTable data)
        {
            DataTable filtered = ApplyAdvancedFilters(data);
            litCount.Text = filtered.Rows.Count.ToString();

            if (!string.IsNullOrEmpty(SortExpression))
            {
                try
                {
                    DataView dv = filtered.DefaultView;
                    dv.Sort = $"{SortExpression} {SortDirection}";
                    filtered = dv.ToTable();
                }
                catch { }
            }

            gvInternal.DataSource = filtered;
            gvInternal.DataBind();
            RestoreFilterUI();
        }

        private DataTable ApplyAdvancedFilters(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return dt;

            List<string> parts = new List<string>();

            if (!string.IsNullOrEmpty(SearchText))
            {
                string safeSearch = SearchText.Replace("'", "''");
                var searchParts = dt.Columns.Cast<DataColumn>()
                    .Where(dc => dc.DataType == typeof(string))
                    .Select(dc => $"{dc.ColumnName} LIKE '%{safeSearch}%'");

                if (searchParts.Any())
                    parts.Add("(" + string.Join(" OR ", searchParts) + ")");
            }

            foreach (var filter in ColumnFilters)
            {
                string val = filter.Value.Value.Replace("'", "''");
                if (string.IsNullOrEmpty(val)) continue;

                if (filter.Value.Operator == "LIKE")
                    parts.Add($"{filter.Key} LIKE '%{val}%'");
                else
                    parts.Add($"{filter.Key} {filter.Value.Operator} '{val}'");
            }

            if (parts.Count == 0) return dt;

            try
            {
                DataRow[] rows = dt.Select(string.Join(" AND ", parts));
                return rows.Length > 0 ? rows.CopyToDataTable() : dt.Clone();
            }
            catch { return dt; }
        }

        protected void FilterControl_Changed(object sender, EventArgs e)
        {
            var filters = ColumnFilters;
            if (gvInternal.HeaderRow != null)
            {
                foreach (var col in ColumnConfigs.Where(x => x.Visible))
                {
                    TextBox txt = gvInternal.HeaderRow.FindControl("flt_" + col.FieldKey) as TextBox;
                    DropDownList ddl = gvInternal.HeaderRow.FindControl("op_" + col.FieldKey) as DropDownList;

                    if (txt != null && !string.IsNullOrEmpty(txt.Text))
                        filters[col.FieldKey] = new ColumnFilterState { Value = txt.Text, Operator = ddl.SelectedValue };
                    else
                        filters.Remove(col.FieldKey);
                }
            }
            ColumnFilters = filters;
            OnRebind?.Invoke(this, EventArgs.Empty);
        }

        private void RestoreFilterUI()
        {
            if (gvInternal.HeaderRow == null) return;
            foreach (var f in ColumnFilters)
            {
                if (gvInternal.HeaderRow.FindControl("flt_" + f.Key) is TextBox t) t.Text = f.Value.Value;
                if (gvInternal.HeaderRow.FindControl("op_" + f.Key) is DropDownList d) d.SelectedValue = f.Value.Operator;
            }
        }

        protected void cblColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            var configs = ColumnConfigs;
            foreach (ListItem item in cblColumns.Items)
            {
                var col = configs.FirstOrDefault(x => x.FieldKey == item.Value);
                if (col != null) col.Visible = item.Selected;
            }
            ColumnConfigs = configs;
            BuildColumns();
            OnRebind?.Invoke(this, EventArgs.Empty);
        }

        protected void btnAction_Click(object sender, EventArgs e) => OnRebind?.Invoke(this, e);
        protected void Add_Triggered(object sender, EventArgs e) => OnAddClick?.Invoke(this, e);
        protected void btnPrev_Click(object sender, EventArgs e) { if (gvInternal.PageIndex > 0) gvInternal.PageIndex--; OnRebind?.Invoke(this, e); }
        protected void btnNext_Click(object sender, EventArgs e) { gvInternal.PageIndex++; OnRebind?.Invoke(this, e); }
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e) { gvInternal.PageSize = int.Parse(ddlPageSize.SelectedValue); OnRebind?.Invoke(this, e); }

        protected void gvInternal_Sorting(object sender, GridViewSortEventArgs e)
        {
            SortDirection = (SortExpression == e.SortExpression && SortDirection == "ASC") ? "DESC" : "ASC";
            SortExpression = e.SortExpression;
            OnRebind?.Invoke(this, e);
        }

        protected void gvInternal_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sort" || e.CommandName == "Page") return;
            RowAction?.Invoke(this, new GridRowActionEventArgs { ActionKey = e.CommandName, RowKey = e.CommandArgument.ToString() });
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            gvInternal.AllowPaging = false;
            OnRebind?.Invoke(this, e);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=Export.xls");
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {
                    gvInternal.RenderControl(hw);
                    Response.Write(sw.ToString());
                }
            }
            Response.End();
        }

        // --- INTERNAL TEMPLATES ---

        private class FilterableHeaderTemplate : ITemplate
        {
            string _lab, _fld; StandardGrid _p;
            public FilterableHeaderTemplate(string l, string f, StandardGrid p) { _lab = l; _fld = f; _p = p; }

            public void InstantiateIn(Control container)
            {
                var wrapper = new Panel { CssClass = "header-wrapper" };
                wrapper.Controls.Add(new LiteralControl($"<span>{_lab}</span><span class='btn-header-menu' onclick='toggleHeaderMenu(event, this)'><i class='fas fa-ellipsis-v'></i></span>"));

                var menu = new Panel { CssClass = "column-context-menu" };
                menu.Controls.Add(CreateLink("fas fa-sort-alpha-down", "Sort Ascending", "Sort", _fld));
                menu.Controls.Add(CreateLink("fas fa-sort-alpha-up", "Sort Descending", "Sort", _fld));
                wrapper.Controls.Add(menu);
                container.Controls.Add(wrapper);

                var filterDiv = new Panel { CssClass = "filter-row" };
                filterDiv.Style.Add("display", "flex");
                filterDiv.Style.Add("gap", "2px");

                var ddlOp = new DropDownList { ID = "op_" + _fld, CssClass = "ddl-operator", AutoPostBack = true };
                ddlOp.Items.Add(new ListItem("★", "LIKE"));
                ddlOp.Items.Add(new ListItem("=", "="));
                ddlOp.Items.Add(new ListItem("!", "<>"));
                ddlOp.SelectedIndexChanged += _p.FilterControl_Changed;

                var txt = new TextBox { ID = "flt_" + _fld, CssClass = "header-filter", AutoPostBack = true };
                txt.Attributes["placeholder"] = "...";
                txt.TextChanged += _p.FilterControl_Changed;

                filterDiv.Controls.Add(ddlOp);
                filterDiv.Controls.Add(txt);
                container.Controls.Add(filterDiv);
            }

            private LinkButton CreateLink(string i, string t, string c, string a)
            {
                var b = new LinkButton { CommandName = c, CommandArgument = a, CssClass = "menu-item", CausesValidation = false };
                b.Controls.Add(new LiteralControl($"<i class='{i}'></i> {t}"));
                return b;
            }
        }

        private class BoundItemTemplate : ITemplate
        {
            string _f, _fmt; bool _b;
            public BoundItemTemplate(string f, string fmt, bool b) { _f = f; _fmt = fmt; _b = b; }
            public void InstantiateIn(Control c)
            {
                var l = new Literal();
                l.DataBinding += (s, e) => {
                    object v = DataBinder.Eval(((GridViewRow)l.NamingContainer).DataItem, _f);
                    l.Text = string.IsNullOrEmpty(_fmt) ? v?.ToString() : string.Format(_fmt, v);
                    if (_b) l.Text = $"<b>{l.Text}</b>";
                };
                c.Controls.Add(l);
            }
        }

        private class StatusColoredItemTemplate : ITemplate
        {
            string _f, _sf, _p, _fmt;
            public StatusColoredItemTemplate(string f, string sf, string p, string fmt) { _f = f; _sf = sf; _p = p; _fmt = fmt; }
            public void InstantiateIn(Control c)
            {
                var l = new Literal();
                l.DataBinding += (s, e) => {
                    var row = (GridViewRow)l.NamingContainer;
                    object v = DataBinder.Eval(row.DataItem, _f);
                    object sv = DataBinder.Eval(row.DataItem, _sf);
                    l.Text = $"<div class='{_p + sv?.ToString().ToLower()} ps-2'>{ (string.IsNullOrEmpty(_fmt) ? v?.ToString() : string.Format(_fmt, v)) }</div>";
                };
                c.Controls.Add(l);
            }
        }

        private class ActionButtonsTemplate : ITemplate
        {
            List<GridActionDef> _a; string _k;
            public ActionButtonsTemplate(List<GridActionDef> a, string k) { _a = a; _k = k; }
            public void InstantiateIn(Control c)
            {
                foreach (var act in _a)
                {
                    var b = new LinkButton { CommandName = act.Key, CssClass = act.CssClass, ToolTip = act.Tooltip, CausesValidation = false };
                    b.Controls.Add(new LiteralControl($"<i class='{act.Icon}'></i> {act.Label}"));
                    b.DataBinding += (s, e) => b.CommandArgument = DataBinder.Eval(((GridViewRow)b.NamingContainer).DataItem, _k).ToString();
                    c.Controls.Add(b);
                }
            }
        }
    }
}
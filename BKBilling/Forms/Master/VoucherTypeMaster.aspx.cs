using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class VoucherTypeMaster : System.Web.UI.Page
    {
        private string SortExpression { get => (string)ViewState["SortExp"] ?? "Voucher_Name"; set => ViewState["SortExp"] = value; }
        private string SortDirection { get => (string)ViewState["SortDir"] ?? "ASC"; set => ViewState["SortDir"] = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack)
            {
                ShowSearchMode();
                LoadList();
            }
        }

        private void LoadList()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    string sql = "SELECT * FROM VoucherType_Table WHERE Company_No = @cid";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    ApplyFiltersAndBind(dt);
                }
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        private void ApplyFiltersAndBind(DataTable dt)
        {
            List<string> filters = new List<string>();
            if (!string.IsNullOrEmpty(txtSearchAll.Text))
            {
                string s = txtSearchAll.Text.Trim().Replace("'", "''");
                filters.Add($"(Voucher_Name LIKE '%{s}%' OR Prefix LIKE '%{s}%' OR Suffix LIKE '%{s}%')");
            }

            if (gvVTypes.HeaderRow != null)
            {
                TextBox fName = (TextBox)gvVTypes.HeaderRow.FindControl("flt_name");
                if (fName != null && !string.IsNullOrEmpty(fName.Text))
                    filters.Add($"Voucher_Name LIKE '%{fName.Text.Trim().Replace("'", "''")}%'");
            }

            DataTable displayDt = dt;
            if (filters.Count > 0)
            {
                try
                {
                    DataRow[] rows = dt.Select(string.Join(" AND ", filters));
                    displayDt = rows.Length > 0 ? rows.CopyToDataTable() : dt.Clone();
                }
                catch { displayDt = dt.Clone(); }
            }

            DataView dv = displayDt.DefaultView;
            dv.Sort = $"{SortExpression} {SortDirection}";
            displayDt = dv.ToTable();

            gvVTypes.PageSize = int.Parse(ddlPageSize.SelectedValue);
            gvVTypes.DataSource = displayDt;
            gvVTypes.DataBind();
            litVisibleCount.Text = displayDt.Rows.Count.ToString();
        }

        private void LoadLedgerDropdowns()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                string sql = "SELECT Ledger_Sno, ledger_name FROM Ledger_Table WHERE Company_No = @cid AND ledger_Active = 1 ORDER BY ledger_name";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                BindDDL(ddlMainLedger, dt);
                BindDDL(ddlDiscountLedger, dt);
                BindDDL(ddlRoundOffLedger, dt);
            }
        }

        private void BindDDL(DropDownList ddl, DataTable dt)
        {
            ddl.DataSource = dt;
            ddl.DataTextField = "ledger_name";
            ddl.DataValueField = "Ledger_Sno";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("-- Select Ledger --", "0"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (hfViewMode.Value == "1") return;
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveVoucherType", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "UPDATE");
                        cmd.Parameters.AddWithValue("@VoucherType_Sno", hfVTypeID.Value);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@Prefix", txtPrefix.Text.Trim());
                        cmd.Parameters.AddWithValue("@Suffix", txtSuffix.Text.Trim());
                        cmd.Parameters.AddWithValue("@Padding_Width", ddlWidth.SelectedValue);
                        cmd.Parameters.AddWithValue("@Print_Title", txtPrintTitle.Text.Trim());

                        cmd.Parameters.AddWithValue("@Main_Ledger_Sno", ddlMainLedger.SelectedValue);
                        cmd.Parameters.AddWithValue("@Discount_Ledger_Sno", ddlDiscountLedger.SelectedValue);
                        cmd.Parameters.AddWithValue("@RoundOff_Ledger_Sno", ddlRoundOffLedger.SelectedValue);
                        cmd.Parameters.AddWithValue("@Is_Tax_Inclusive", chkTaxInclusive.Checked);
                        cmd.Parameters.AddWithValue("@IsActive", chkActive.Checked);

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();
                        Alert("Voucher settings updated successfully!", "success");
                        btnBack_Click(null, null);
                    }
                }
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        private void LoadForEdit(string id, bool isReadOnly)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM VoucherType_Table WHERE VoucherType_Sno=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                if (conn.State == ConnectionState.Closed) conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        hfVTypeID.Value = id;
                        litVName.Text = dr["Voucher_Name"].ToString();
                        txtPrintTitle.Text = dr["Print_Title"].ToString();
                        txtPrefix.Text = dr["Prefix"].ToString();
                        txtSuffix.Text = dr["Suffix"].ToString();
                        ddlWidth.SelectedValue = dr["Padding_Width"].ToString();

                        LoadLedgerDropdowns();
                        ddlMainLedger.SelectedValue = dr["Main_Ledger_Sno"].ToString() == "" ? "0" : dr["Main_Ledger_Sno"].ToString();
                        ddlDiscountLedger.SelectedValue = dr["Discount_Ledger_Sno"].ToString() == "" ? "0" : dr["Discount_Ledger_Sno"].ToString();
                        ddlRoundOffLedger.SelectedValue = dr["RoundOff_Ledger_Sno"].ToString() == "" ? "0" : dr["RoundOff_Ledger_Sno"].ToString();

                        chkActive.Checked = Convert.ToBoolean(dr["IsActive"]);
                        chkTaxInclusive.Checked = Convert.ToBoolean(dr["Is_Tax_Inclusive"]);

                        hfViewMode.Value = isReadOnly ? "1" : "0";
                        SetFormState(isReadOnly);
                        ShowAddMode();
                    }
                }
            }
        }

        private void SetFormState(bool isReadOnly)
        {
            txtPrintTitle.Enabled = txtPrefix.Enabled = txtSuffix.Enabled = ddlWidth.Enabled = !isReadOnly;
            ddlMainLedger.Enabled = ddlDiscountLedger.Enabled = ddlRoundOffLedger.Enabled = !isReadOnly;
            chkActive.Disabled = chkTaxInclusive.Disabled = isReadOnly;
            btnSave.Visible = !isReadOnly;
            if (isReadOnly) litVName.Text += " (View Mode)";
        }

        private void ShowSearchMode() { pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = true; pnlForm.Visible = phAddButtons.Visible = false; }
        private void ShowAddMode() { pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = false; phAddButtons.Visible = pnlForm.Visible = true; }

        protected void GridFilter_Changed(object sender, EventArgs e) { LoadList(); }
        protected void gvVTypes_Sorting(object sender, GridViewSortEventArgs e) { SortDirection = (SortExpression == e.SortExpression && SortDirection == "ASC") ? "DESC" : "ASC"; SortExpression = e.SortExpression; LoadList(); }
        protected void gvVTypes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord") LoadForEdit(e.CommandArgument.ToString(), false);
            else if (e.CommandName == "ViewRecord") LoadForEdit(e.CommandArgument.ToString(), true);
        }
        protected void btnBack_Click(object sender, EventArgs e) { ShowSearchMode(); LoadList(); }
        protected void Pager_Click(object sender, EventArgs e) { string c = ((LinkButton)sender).CommandArgument; if (c == "Prev" && gvVTypes.PageIndex > 0) gvVTypes.PageIndex--; else if (c == "Next") gvVTypes.PageIndex++; LoadList(); }
        private void Alert(string msg, string type) { ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "msg", $"showNotification('{msg.Replace("'", "\\'")}', '{type}');", true); }
    }
}
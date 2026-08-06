using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class CustomerMaster : System.Web.UI.Page
    {
        private const long CustomerGroupID = 1000000029;
        private string SortExpression { get => (string)ViewState["SortExp"] ?? "ledger_name"; set => ViewState["SortExp"] = value; }
        private string SortDirection { get => (string)ViewState["SortDir"] ?? "ASC"; set => ViewState["SortDir"] = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack)
            {
                LoadDropdowns();
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
                    using (SqlCommand cmd = new SqlCommand("sp_GetCustomerList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@LedgerGroup_no", CustomerGroupID);
                        cmd.Parameters.AddWithValue("@SearchText", txtSearchAll.Text.Trim());

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        ApplyFiltersAndBind(dt);
                    }
                }
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        private void ApplyFiltersAndBind(DataTable dt)
        {
            List<string> flyoutFilters = new List<string>();
            if (gvCustomers.HeaderRow != null)
            {
                TextBox fCode = (TextBox)gvCustomers.HeaderRow.FindControl("flt_code");
                if (fCode != null && !string.IsNullOrEmpty(fCode.Text))
                    flyoutFilters.Add($"ledger_code LIKE '%{fCode.Text.Trim().Replace("'", "''")}%'");

                TextBox fName = (TextBox)gvCustomers.HeaderRow.FindControl("flt_name");
                if (fName != null && !string.IsNullOrEmpty(fName.Text))
                    flyoutFilters.Add($"ledger_name LIKE '%{fName.Text.Trim().Replace("'", "''")}%'");
            }

            DataTable displayDt = dt;
            if (flyoutFilters.Count > 0)
            {
                try
                {
                    DataRow[] rows = dt.Select(string.Join(" AND ", flyoutFilters));
                    displayDt = rows.Length > 0 ? rows.CopyToDataTable() : dt.Clone();
                }
                catch { displayDt = dt.Clone(); }
            }

            DataView dv = displayDt.DefaultView;
            dv.Sort = $"{SortExpression} {SortDirection}";
            displayDt = dv.ToTable();

            gvCustomers.PageSize = int.Parse(ddlPageSize.SelectedValue);
            gvCustomers.DataSource = displayDt;
            gvCustomers.DataBind();
            litVisibleCount.Text = displayDt.Rows.Count.ToString();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (hfViewMode.Value == "1") return;
            if (string.IsNullOrWhiteSpace(txtCustName.Text) || string.IsNullOrWhiteSpace(txtCustCode.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            { Alert("Name, Code, and Mobile are required!", "error"); return; }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveCustomer", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfCustomerID.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@Ledger_Sno", isUpdate ? Convert.ToInt64(hfCustomerID.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ledger_name", txtCustName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@ledger_code", txtCustCode.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@Ledger_Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Active", chkActive.Checked);
                        cmd.Parameters.AddWithValue("@Area_no", ddlArea.SelectedValue);
                        cmd.Parameters.AddWithValue("@ledger_Add1", txtAdd1.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Add2", txtAdd2.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Add3", txtAdd3.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_ContactPerson", txtContactPerson.Text.Trim());
                        cmd.Parameters.AddWithValue("@Balance_Type", ddlBalType.SelectedValue);
                        cmd.Parameters.AddWithValue("@Credit_Limit", Convert.ToDecimal(string.IsNullOrEmpty(txtCreditLimit.Text) ? "0" : txtCreditLimit.Text));
                        cmd.Parameters.AddWithValue("@Credit_Days", Convert.ToInt32(string.IsNullOrEmpty(txtCreditDays.Text) ? "0" : txtCreditDays.Text));
                        cmd.Parameters.AddWithValue("@Ledger_GST", txtGST.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@GST_DealerType", ddlDealer.SelectedValue);
                        cmd.Parameters.AddWithValue("@GST_StateCode", ddlState.SelectedValue);
                        cmd.Parameters.AddWithValue("@Ledger_open", Convert.ToDecimal(string.IsNullOrEmpty(txtOpening.Text) ? "0" : txtOpening.Text));
                        cmd.Parameters.AddWithValue("@Ledger_remarks", txtRemarks.Text.Trim());

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();
                        Alert("Customer saved successfully!", "success");
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ledger_Table WHERE Ledger_Sno = @sno", conn);
                cmd.Parameters.AddWithValue("@sno", id);
                if (conn.State == ConnectionState.Closed) conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        hfCustomerID.Value = id;
                        txtCustName.Text = dr["ledger_name"].ToString();
                        txtCustCode.Text = dr["ledger_code"].ToString();
                        chkActive.Checked = Convert.ToBoolean(dr["ledger_Active"]);
                        ddlArea.SelectedValue = dr["Area_no"].ToString();
                        txtPhone.Text = dr["Ledger_Phone"].ToString();
                        txtEmail.Text = dr["Ledger_Email"].ToString();
                        txtAdd1.Text = dr["ledger_Add1"].ToString();
                        txtAdd2.Text = dr["ledger_Add2"].ToString();
                        txtAdd3.Text = dr["ledger_Add3"].ToString();
                        txtGST.Text = dr["Ledger_GST"].ToString();
                        ddlState.SelectedValue = dr["GST_StateCode"].ToString();
                        ddlDealer.SelectedValue = dr["GST_DealerType"].ToString();
                        txtOpening.Text = dr["Ledger_open"].ToString();
                        ddlBalType.SelectedValue = dr["Balance_Type"].ToString();
                        txtCreditLimit.Text = dr["Credit_Limit"].ToString();
                        txtCreditDays.Text = dr["Credit_Days"].ToString();
                        txtRemarks.Text = dr["Ledger_remarks"].ToString();
                        txtContactPerson.Text = dr["Ledger_ContactPerson"].ToString();

                        hfViewMode.Value = isReadOnly ? "1" : "0";
                        SetFormState(isReadOnly);
                        ShowAddMode();
                    }
                }
            }
        }

        private void SetFormState(bool isReadOnly)
        {
            txtCustName.Enabled = txtCustCode.Enabled = ddlArea.Enabled = txtCreditLimit.Enabled =
            txtCreditDays.Enabled = chkActive.Enabled = txtContactPerson.Enabled = txtPhone.Enabled =
            txtEmail.Enabled = txtAdd1.Enabled = txtAdd2.Enabled = txtAdd3.Enabled = txtGST.Enabled =
            ddlState.Enabled = ddlDealer.Enabled = txtOpening.Enabled = ddlBalType.Enabled =
            txtRemarks.Enabled = !isReadOnly;
            btnSave.Visible = !isReadOnly;
        }

        private void ShowSearchMode() { pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = true; pnlForm.Visible = phAddButtons.Visible = false; litTitle.Text = "Customer Directory"; }
        private void ShowAddMode() { pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = false; pnlForm.Visible = phAddButtons.Visible = true; litTitle.Text = hfViewMode.Value == "1" ? "View Customer" : (hfCustomerID.Value == "" ? "New Customer Setup" : "Edit Customer"); }

        protected void GridFilter_Changed(object sender, EventArgs e) { LoadList(); }
        protected void gvCustomers_Sorting(object sender, GridViewSortEventArgs e) { SortDirection = (SortExpression == e.SortExpression && SortDirection == "ASC") ? "DESC" : "ASC"; SortExpression = e.SortExpression; LoadList(); }
        protected void gvCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord") LoadForEdit(e.CommandArgument.ToString(), false);
            else if (e.CommandName == "ViewRecord") LoadForEdit(e.CommandArgument.ToString(), true);
        }
        protected void btnOpenCreate_Click(object sender, EventArgs e) { hfCustomerID.Value = ""; hfViewMode.Value = "0"; ClearInputs(); SetFormState(false); ShowAddMode(); }
        protected void btnBack_Click(object sender, EventArgs e) { ShowSearchMode(); LoadList(); }
        protected void Pager_Click(object sender, EventArgs e) { string c = ((LinkButton)sender).CommandArgument; if (c == "Prev" && gvCustomers.PageIndex > 0) gvCustomers.PageIndex--; else if (c == "Next") gvCustomers.PageIndex++; LoadList(); }

        private void LoadDropdowns()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAreaDropdown", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable(); da.Fill(dt);
                ddlArea.DataSource = dt; ddlArea.DataTextField = "Area_Name"; ddlArea.DataValueField = "Area_Sno"; ddlArea.DataBind();
                ddlArea.Items.Insert(0, new ListItem("-- Select Area --", "0"));

                ddlState.Items.Clear();
                ddlState.Items.Add(new ListItem("Tamil Nadu (33)", "33"));
                ddlState.Items.Add(new ListItem("Maharashtra (27)", "27"));
                ddlState.Items.Insert(0, new ListItem("-- Select State --", "0"));
            }
        }

        private void ClearInputs() { txtCustName.Text = txtCustCode.Text = txtContactPerson.Text = txtPhone.Text = txtEmail.Text = txtAdd1.Text = txtAdd2.Text = txtAdd3.Text = txtGST.Text = txtRemarks.Text = ""; txtOpening.Text = "0.00"; txtCreditLimit.Text = "0"; txtCreditDays.Text = "0"; chkActive.Checked = true; ddlArea.SelectedIndex = ddlState.SelectedIndex = ddlDealer.SelectedIndex = ddlBalType.SelectedIndex = 0; }
        private void Alert(string msg, string type) { ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "msg", $"showNotification('{msg.Replace("'", "\\'")}', '{type}');", true); }
    }
}
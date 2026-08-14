using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class JobWorkMaster : System.Web.UI.Page
    {
        private string SortExpression { get => (string)ViewState["SortExp"] ?? "ledger_name"; set => ViewState["SortExp"] = value; }
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
                    using (SqlCommand cmd = new SqlCommand("sp_GetJobWorkerList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
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
            litTotalCount.Text = dt.Rows.Count.ToString();
            List<string> filters = new List<string>();

            if (gvWorkers.HeaderRow != null)
            {
                TextBox fCode = (TextBox)gvWorkers.HeaderRow.FindControl("flt_code");
                if (fCode != null && !string.IsNullOrEmpty(fCode.Text))
                    filters.Add($"ledger_code LIKE '%{fCode.Text.Trim().Replace("'", "''")}%'");

                TextBox fName = (TextBox)gvWorkers.HeaderRow.FindControl("flt_name");
                if (fName != null && !string.IsNullOrEmpty(fName.Text))
                    filters.Add($"ledger_name LIKE '%{fName.Text.Trim().Replace("'", "''")}%'");
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

            gvWorkers.PageSize = int.Parse(ddlPageSize.SelectedValue);
            gvWorkers.DataSource = displayDt;
            gvWorkers.DataBind();
            litVisibleCount.Text = displayDt.Rows.Count.ToString();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (hfViewMode.Value == "1") return;
            if (string.IsNullOrWhiteSpace(txtWorkerName.Text) || string.IsNullOrWhiteSpace(txtWorkerCode.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            { Alert("Name, ID, and Mobile are required!", "error"); return; }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveJobWorker", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfJobWorkerID.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@Ledger_Sno", isUpdate ? Convert.ToInt64(hfJobWorkerID.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ledger_name", txtWorkerName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@ledger_code", txtWorkerCode.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@Ledger_Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Active", chkActive.Checked);
                        cmd.Parameters.AddWithValue("@Area_no", ddlArea.SelectedValue);
                        cmd.Parameters.AddWithValue("@ledger_Add1", txtAdd1.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Add2", txtAdd2.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_open", Convert.ToDecimal(string.IsNullOrEmpty(txtOpening.Text) ? "0" : txtOpening.Text));
                        cmd.Parameters.AddWithValue("@Ledger_remarks", txtRemarks.Text.Trim());

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("Worker details saved successfully!", "success");
                btnBack_Click(null, null);
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
                        hfJobWorkerID.Value = id;
                        txtWorkerName.Text = dr["ledger_name"].ToString();
                        txtWorkerCode.Text = dr["ledger_code"].ToString();
                        chkActive.Checked = Convert.ToBoolean(dr["ledger_Active"]);
                        LoadDropdowns();
                        ddlArea.SelectedValue = dr["Area_no"].ToString();
                        txtPhone.Text = dr["Ledger_Phone"].ToString();
                        txtEmail.Text = dr["Ledger_Email"].ToString();
                        txtAdd1.Text = dr["ledger_Add1"].ToString();
                        txtAdd2.Text = dr["ledger_Add2"].ToString();
                        txtOpening.Text = dr["Ledger_open"].ToString();
                        txtRemarks.Text = dr["Ledger_remarks"].ToString();

                        hfViewMode.Value = isReadOnly ? "1" : "0";
                        SetFormState(isReadOnly);
                        ShowAddMode();
                    }
                }
            }
        }

        private void SetFormState(bool isReadOnly)
        {
            txtWorkerName.Enabled = txtWorkerCode.Enabled = txtPhone.Enabled = ddlArea.Enabled =
            txtAdd1.Enabled = txtAdd2.Enabled = txtOpening.Enabled = txtEmail.Enabled =
            txtRemarks.Enabled = !isReadOnly;
            chkActive.Disabled = isReadOnly;
            btnSave.Visible = !isReadOnly;
            if (isReadOnly) litTitle.Text = "View Worker Details";
        }

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
            }
        }

        private void ShowSearchMode() { pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = true; pnlForm.Visible = phAddButtons.Visible = false; litTitle.Text = "Labour Directory"; }
        private void ShowAddMode() { pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = false; pnlForm.Visible = phAddButtons.Visible = true; litTitle.Text = hfViewMode.Value == "1" ? "View Worker" : (hfJobWorkerID.Value == "" ? "New Labour Registration" : "Modify Worker Details"); }

        protected void GridFilter_Changed(object sender, EventArgs e) { LoadList(); }
        protected void gvWorkers_Sorting(object sender, GridViewSortEventArgs e) { SortDirection = (SortExpression == e.SortExpression && SortDirection == "ASC") ? "DESC" : "ASC"; SortExpression = e.SortExpression; LoadList(); }
        protected void gvWorkers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord") LoadForEdit(e.CommandArgument.ToString(), false);
            else if (e.CommandName == "ViewRecord") LoadForEdit(e.CommandArgument.ToString(), true);
        }
        protected void btnOpenCreate_Click(object sender, EventArgs e) { hfJobWorkerID.Value = ""; hfViewMode.Value = "0"; ClearInputs(); LoadDropdowns(); SetFormState(false); ShowAddMode(); }
        protected void btnBack_Click(object sender, EventArgs e) { ShowSearchMode(); LoadList(); }
        protected void Pager_Click(object sender, EventArgs e) { string c = ((LinkButton)sender).CommandArgument; if (c == "Prev" && gvWorkers.PageIndex > 0) gvWorkers.PageIndex--; else if (c == "Next") gvWorkers.PageIndex++; LoadList(); }

        private void ClearInputs() { txtWorkerName.Text = txtWorkerCode.Text = txtPhone.Text = txtEmail.Text = txtAdd1.Text = txtAdd2.Text = txtRemarks.Text = ""; txtOpening.Text = "0.00"; chkActive.Checked = true; }
        private void Alert(string msg, string type) { ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "msg", $"showNotification('{msg.Replace("'", "\\'")}', '{type}');", true); }
    }
}
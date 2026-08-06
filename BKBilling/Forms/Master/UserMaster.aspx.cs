using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class UserMaster : System.Web.UI.Page
    {
        private string SortExpression { get => (string)ViewState["SortExp"] ?? "Username"; set => ViewState["SortExp"] = value; }
        private string SortDirection { get => (string)ViewState["SortDir"] ?? "ASC"; set => ViewState["SortDir"] = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) { Response.Redirect("~/Login.aspx"); return; }

            if (!IsPostBack)
            {
                if (Request.QueryString["mode"] == "profile")
                {
                    if (Session["UserID"] != null)
                    {
                        string currentUserId = Session["UserID"].ToString();
                        hfProfileMode.Value = "1";
                        LoadUserForEdit(currentUserId);

                        litTitle.Text = "My Profile Settings";
                        btnBack.Visible = false;
                    }
                    else
                    {
                        Response.Redirect("~/Login.aspx");
                    }
                }
                else
                {
                    ShowSearchMode();
                    LoadList();
                }
            }
        }

        private void LoadList()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetUserList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);

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
    List<string> filters = new List<string>();

    // 1. GLOBAL SEARCH (Top Search Bar)
    if (!string.IsNullOrEmpty(txtSearchAll.Text))
    {
        string s = txtSearchAll.Text.Trim().Replace("'", "''");
        filters.Add($"(Username LIKE '%{s}%' OR FullName LIKE '%{s}%' OR Role LIKE '%{s}%' OR Phone LIKE '%{s}%')");
    }

    // 2. COLUMN-SPECIFIC FLYOUT FILTERS
    if (gvUsers.HeaderRow != null)
    {
        // Login ID Filter
        TextBox fUser = (TextBox)gvUsers.HeaderRow.FindControl("flt_user");
        if (fUser != null && !string.IsNullOrEmpty(fUser.Text))
            filters.Add($"Username LIKE '%{fUser.Text.Trim().Replace("'", "''")}%'");

        // Full Name Filter
        TextBox fName = (TextBox)gvUsers.HeaderRow.FindControl("flt_name");
        if (fName != null && !string.IsNullOrEmpty(fName.Text))
            filters.Add($"FullName LIKE '%{fName.Text.Trim().Replace("'", "''")}%'");

        // Role Filter (Dropdown)
        DropDownList fRole = (DropDownList)gvUsers.HeaderRow.FindControl("flt_role");
        if (fRole != null && !string.IsNullOrEmpty(fRole.SelectedValue))
            filters.Add($"Role = '{fRole.SelectedValue}'");

        // Join Date Filter
        TextBox fDate = (TextBox)gvUsers.HeaderRow.FindControl("flt_date");
        if (fDate != null && !string.IsNullOrEmpty(fDate.Text))
            filters.Add($"Convert(Join_Date, 'System.String') LIKE '%{fDate.Text.Trim().Replace("'", "''")}%'");

        // Status Filter (Dropdown - Boolean logic)
        DropDownList fStatus = (DropDownList)gvUsers.HeaderRow.FindControl("flt_status");
        if (fStatus != null && !string.IsNullOrEmpty(fStatus.SelectedValue))
            filters.Add($"IsActive = {fStatus.SelectedValue}");
    }

    DataTable displayDt = dt;
    if (filters.Count > 0)
    {
        try
        {
            // Combine all filters with AND
            DataRow[] rows = dt.Select(string.Join(" AND ", filters));
            displayDt = rows.Length > 0 ? rows.CopyToDataTable() : dt.Clone();
        }
        catch { displayDt = dt.Clone(); }
    }

    // 3. SORTING LOGIC (All Columns)
    DataView dv = displayDt.DefaultView;
    dv.Sort = $"{SortExpression} {SortDirection}";
    displayDt = dv.ToTable();

    // 4. BINDING
    gvUsers.PageSize = int.Parse(ddlPageSize.SelectedValue);
    gvUsers.DataSource = displayDt;
    gvUsers.DataBind();

    // Update Counts
    litVisibleCount.Text = displayDt.Rows.Count.ToString();
}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtFullName.Text))
            { Alert("Required fields missing", "error"); return; }

            bool isUpd = !string.IsNullOrEmpty(hfUserSno.Value);
            if (!isUpd && string.IsNullOrWhiteSpace(txtPass.Text)) { Alert("Password required for new user", "error"); return; }
            if (!string.IsNullOrWhiteSpace(txtPass.Text) && txtPass.Text != txtConfirm.Text) { Alert("Passwords mismatch", "error"); return; }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", isUpd ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@User_Sno", isUpd ? Convert.ToInt64(hfUserSno.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Role", ddlRole.SelectedValue);
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address_1", txtAdd1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address_2", txtAdd2.Text.Trim());
                        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);
                        cmd.Parameters.AddWithValue("@Join_Date", string.IsNullOrEmpty(txtJoinDate.Text) ? (object)DBNull.Value : txtJoinDate.Text);

                        // Use original pass if not changed during update
                        string passHash = string.IsNullOrWhiteSpace(txtPass.Text) ? "" : SecurityHelper.ComputeHash(txtPass.Text);
                        cmd.Parameters.AddWithValue("@Password", passHash);

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("User updated successfully", "success");
                btnBack_Click(null, null);
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        private void LoadUserForEdit(string id)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetUserByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User_Sno", id);
                if (conn.State == ConnectionState.Closed) conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        hfUserSno.Value = id;
                        txtUsername.Text = dr["Username"].ToString();
                        txtFullName.Text = dr["FullName"].ToString();
                        ddlRole.SelectedValue = dr["Role"].ToString();
                        txtPhone.Text = dr["Phone"].ToString();
                        txtEmail.Text = dr["Email"].ToString();
                        txtAdd1.Text = dr["Address_1"].ToString();
                        txtAdd2.Text = dr["Address_2"].ToString();
                        chkIsActive.Checked = Convert.ToBoolean(dr["IsActive"]);
                        if (dr["Join_Date"] != DBNull.Value)
                            txtJoinDate.Text = Convert.ToDateTime(dr["Join_Date"]).ToString("yyyy-MM-dd");

                        txtPass.Text = txtConfirm.Text = "";

                        ShowAddMode();

                        bool isReadOnly = hfViewMode.Value == "1";
                        SetFormState(isReadOnly);
                    }
                }
            }
        }

        private void SetFormState(bool isReadOnly)
        {
            txtUsername.Enabled = !isReadOnly;
            txtFullName.Enabled = !isReadOnly;
            ddlRole.Enabled = !isReadOnly;
            txtPhone.Enabled = !isReadOnly;
            txtEmail.Enabled = !isReadOnly;
            txtAdd1.Enabled = !isReadOnly;
            txtAdd2.Enabled = !isReadOnly;
            txtJoinDate.Enabled = !isReadOnly;
            txtPass.Enabled = !isReadOnly;
            txtConfirm.Enabled = !isReadOnly;
            chkIsActive.Enabled = !isReadOnly;

            btnSave.Visible = !isReadOnly;

            if (isReadOnly)
                litTitle.Text = "View User Details";
        }

        private void ShowSearchMode()
        {
            pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = true;
            pnlForm.Visible = phAddButtons.Visible = false;
            litTitle.Text = "User Management";
        }

        private void ShowAddMode()
        {
            pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = false;
            pnlForm.Visible = phAddButtons.Visible = true;
            litTitle.Text = hfUserSno.Value == "" ? "Create User" : "Update User";
        }

        protected void GridFilter_Changed(object sender, EventArgs e) { LoadList(); }
        protected void gvUsers_Sorting(object sender, GridViewSortEventArgs e) { SortDirection = (SortExpression == e.SortExpression && SortDirection == "ASC") ? "DESC" : "ASC"; SortExpression = e.SortExpression; LoadList(); }
        protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord")
            {
                hfViewMode.Value = "0";
                LoadUserForEdit(e.CommandArgument.ToString());
            }
            else if (e.CommandName == "ViewRecord")
            {
                hfViewMode.Value = "1";
                LoadUserForEdit(e.CommandArgument.ToString());
            }
        }
        protected void btnOpenCreate_Click(object sender, EventArgs e)
        {
            hfUserSno.Value = "";
            hfViewMode.Value = "0";
            ClearInputs();
            SetFormState(false);
            ShowAddMode();
        }
        protected void btnBack_Click(object sender, EventArgs e) { ShowSearchMode(); LoadList(); }
        protected void Pager_Click(object sender, EventArgs e) { string c = ((LinkButton)sender).CommandArgument; if (c == "Prev" && gvUsers.PageIndex > 0) gvUsers.PageIndex--; else if (c == "Next") gvUsers.PageIndex++; LoadList(); }

        private void ClearInputs()
        {
            txtUsername.Text = txtFullName.Text = txtPhone.Text = txtEmail.Text = txtAdd1.Text = txtAdd2.Text = txtPass.Text = txtConfirm.Text = "";
            txtJoinDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            chkIsActive.Checked = true;
        }

        private void Alert(string msg, string type)
        {
            string script = $"showNotification('{msg.Replace("'", "\\'")}', '{type}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", script, true);
        }
    }
}
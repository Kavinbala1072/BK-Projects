using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class LedgerGroup : System.Web.UI.Page
    {
        private string SortExpression { get => (string)ViewState["SortExp"] ?? "LedgerGroup_Name"; set => ViewState["SortExp"] = value; }
        private string SortDirection { get => (string)ViewState["SortDir"] ?? "ASC"; set => ViewState["SortDir"] = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) { Response.Redirect("~/Login.aspx"); return; }
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
                    using (SqlCommand cmd = new SqlCommand("sp_GetLedgerGroupList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@SearchText", txtSearchAll.Text.Trim());

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
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

            if (gvGroups.HeaderRow != null)
            {
                TextBox fName = (TextBox)gvGroups.HeaderRow.FindControl("flt_name");
                if (fName != null && !string.IsNullOrEmpty(fName.Text))
                    filters.Add($"LedgerGroup_Name LIKE '%{fName.Text.Trim().Replace("'", "''")}%'");
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

            gvGroups.PageSize = int.Parse(ddlPageSize.SelectedValue);
            gvGroups.DataSource = displayDt;
            gvGroups.DataBind();
            litVisibleCount.Text = displayDt.Rows.Count.ToString();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (hfViewMode.Value == "1") return;
            if (string.IsNullOrWhiteSpace(txtGroupName.Text)) { Alert("Group Name is required", "error"); return; }
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveLedgerGroup", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfGroupId.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@LedgerGroup_Sno", isUpdate ? Convert.ToInt32(hfGroupId.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@LedgerGroup_Name", txtGroupName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@Ledgergroup_Under", ddlParentGroup.SelectedValue);
                        cmd.Parameters.AddWithValue("@Nature", ddlNature.SelectedValue);
                        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("Group registration saved successfully!", "success");
                btnBack_Click(null, null);
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        private void LoadForEdit(string id, bool isReadOnly)
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetLedgerGroupByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LedgerGroup_Sno", id);
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                hfGroupId.Value = id;
                                txtGroupName.Text = dr["LedgerGroup_Name"].ToString();
                                chkIsActive.Checked = Convert.ToBoolean(dr["IsActive"]);

                                BindParentDropdown();
                                ddlParentGroup.SelectedValue = dr["Ledgergroup_Under"].ToString();
                                ddlNature.SelectedValue = dr["Nature"].ToString();
                                ddlNature.Enabled = (ddlParentGroup.SelectedValue == "0");

                                hfViewMode.Value = isReadOnly ? "1" : "0";
                                SetFormState(isReadOnly);
                                ShowAddMode();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        private void SetFormState(bool isReadOnly)
        {
            txtGroupName.Enabled = ddlParentGroup.Enabled = ddlNature.Enabled = !isReadOnly;
            chkIsActive.Disabled = isReadOnly;
            btnSave.Visible = !isReadOnly;
            if (isReadOnly) litTitle.Text = "View Group Details";
        }

        protected void ddlParentGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlParentGroup.SelectedValue == "0") { ddlNature.Enabled = true; }
            else
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetLedgerGroupByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@LedgerGroup_Sno", ddlParentGroup.SelectedValue);
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read()) { ddlNature.SelectedValue = dr["Nature"].ToString(); ddlNature.Enabled = false; }
                        }
                    }
                }
            }
        }

        private void BindParentDropdown()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                string sql = "SELECT LedgerGroup_Sno, LedgerGroup_Name FROM LedgerGroup_Table WHERE Company_No = @cid AND IsActive = 1 ORDER BY LedgerGroup_Name";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                ddlParentGroup.DataSource = dt;
                ddlParentGroup.DataTextField = "LedgerGroup_Name";
                ddlParentGroup.DataValueField = "LedgerGroup_Sno";
                ddlParentGroup.DataBind();
                ddlParentGroup.Items.Insert(0, new ListItem("PRIMARY (ROOT)", "0"));
            }
        }

        private void ShowSearchMode() { pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = true; pnlForm.Visible = phAddButtons.Visible = false; litTitle.Text = "Group Directory"; }
        private void ShowAddMode() { pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = false; pnlForm.Visible = phAddButtons.Visible = true; litTitle.Text = hfViewMode.Value == "1" ? "View Group" : (hfGroupId.Value == "" ? "New Group Setup" : "Edit Group"); }

        protected void GridFilter_Changed(object sender, EventArgs e) { LoadList(); }
        protected void gvGroups_Sorting(object sender, GridViewSortEventArgs e) { SortDirection = (SortExpression == e.SortExpression && SortDirection == "ASC") ? "DESC" : "ASC"; SortExpression = e.SortExpression; LoadList(); }
        protected void gvGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditGroup") LoadForEdit(e.CommandArgument.ToString(), false);
            else if (e.CommandName == "ViewRecord") LoadForEdit(e.CommandArgument.ToString(), true);
        }
        protected void btnOpenCreate_Click(object sender, EventArgs e) { hfGroupId.Value = ""; hfViewMode.Value = "0"; ClearForm(); BindParentDropdown(); SetFormState(false); ShowAddMode(); }
        protected void btnBack_Click(object sender, EventArgs e) { ShowSearchMode(); LoadList(); }
        protected void Pager_Click(object sender, EventArgs e) { string c = ((LinkButton)sender).CommandArgument; if (c == "Prev" && gvGroups.PageIndex > 0) gvGroups.PageIndex--; else if (c == "Next") gvGroups.PageIndex++; LoadList(); }

        private void ClearForm() { txtGroupName.Text = ""; hfGroupId.Value = ""; chkIsActive.Checked = true; ddlNature.Enabled = true; ddlNature.SelectedIndex = 0; }
        private void Alert(string msg, string type) { ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "msg", $"showNotification('{msg.Replace("'", "\\'")}', '{type}');", true); }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class AreaMaster : System.Web.UI.Page
    {
        private string SortExpression { get => (string)ViewState["SortExp"] ?? "Area_Name"; set => ViewState["SortExp"] = value; }
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
                    using (SqlCommand cmd = new SqlCommand("sp_GetAreaList", conn))
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
            List<string> flyoutFilters = new List<string>();

            if (gvArea.HeaderRow != null)
            {
                TextBox fName = (TextBox)gvArea.HeaderRow.FindControl("flt_name");
                if (fName != null && !string.IsNullOrEmpty(fName.Text))
                    flyoutFilters.Add($"Area_Name LIKE '%{fName.Text.Trim().Replace("'", "''")}%'");
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

            gvArea.PageSize = int.Parse(ddlPageSize.SelectedValue);
            gvArea.DataSource = displayDt;
            gvArea.DataBind();
            litVisibleCount.Text = displayDt.Rows.Count.ToString();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAreaName.Text)) { Alert("Name is required!", "error"); return; }
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveArea", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfAreaId.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@Area_Sno", isUpdate ? Convert.ToInt32(hfAreaId.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Area_Name", txtAreaName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@Area_Under", ddlAreaUnder.SelectedValue);
                        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("Area settings updated successfully!", "success");
                btnBack_Click(null, null);
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        private void BindParentArea()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                string sql = "SELECT Area_Sno, Area_Name FROM Area_table WHERE Company_No = @cid AND IsActive = 1 ORDER BY Area_Name";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                ddlAreaUnder.DataSource = dt;
                ddlAreaUnder.DataTextField = "Area_Name";
                ddlAreaUnder.DataValueField = "Area_Sno";
                ddlAreaUnder.DataBind();
                ddlAreaUnder.Items.Insert(0, new ListItem("PRIMARY (ROOT)", "0"));
            }
        }

        private void LoadAreaForEdit(string id)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAreaByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Area_Sno", id);
                if (conn.State == ConnectionState.Closed) conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        hfAreaId.Value = id;
                        txtAreaName.Text = dr["Area_Name"].ToString();
                        chkIsActive.Checked = Convert.ToBoolean(dr["IsActive"]);
                        BindParentArea();
                        ddlAreaUnder.SelectedValue = dr["Area_Under"].ToString();
                        ShowAddMode();
                    }
                }
            }
        }

        private void ShowSearchMode()
        {
            pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = true;
            pnlForm.Visible = phAddButtons.Visible = false;
            litTitle.Text = "Area Directory";
        }

        private void ShowAddMode()
        {
            pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = false;
            pnlForm.Visible = phAddButtons.Visible = true;
            litTitle.Text = hfAreaId.Value == "" ? "Create New Area" : "Modify Area Details";
        }

        protected void GridFilter_Changed(object sender, EventArgs e) { LoadList(); }
        protected void gvArea_Sorting(object sender, GridViewSortEventArgs e) { SortDirection = (SortExpression == e.SortExpression && SortDirection == "ASC") ? "DESC" : "ASC"; SortExpression = e.SortExpression; LoadList(); }
        protected void gvArea_RowCommand(object sender, GridViewCommandEventArgs e) { if (e.CommandName == "EditArea") LoadAreaForEdit(e.CommandArgument.ToString()); }
        protected void btnOpenCreate_Click(object sender, EventArgs e) { hfAreaId.Value = ""; ClearForm(); BindParentArea(); ShowAddMode(); }
        protected void btnBack_Click(object sender, EventArgs e) { ShowSearchMode(); LoadList(); }
        protected void Pager_Click(object sender, EventArgs e) { string c = ((LinkButton)sender).CommandArgument; if (c == "Prev" && gvArea.PageIndex > 0) gvArea.PageIndex--; else if (c == "Next") gvArea.PageIndex++; LoadList(); }
        
        private void ClearForm() { txtAreaName.Text = ""; hfAreaId.Value = ""; chkIsActive.Checked = true; }
        private void Alert(string msg, string type) { ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "msg", $"showNotification('{msg.Replace("'", "\\'")}', '{type}');", true); }
    }
}
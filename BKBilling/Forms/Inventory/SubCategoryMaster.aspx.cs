using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Inventory
{
    public partial class SubCategoryMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack) LoadList();
        }

        private void LoadDropdowns()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                // Fetch active Main Categories from ItemGroup_Table
                SqlDataAdapter da = new SqlDataAdapter("SELECT ItemGroup_Sno, ItemGroup_Name FROM ItemGroup_Table WHERE Company_No = @cid AND IsActive = 1 ORDER BY ItemGroup_Name", conn);
                da.SelectCommand.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                DataTable dt = new DataTable(); da.Fill(dt);
                ddlMainCategory.DataSource = dt; ddlMainCategory.DataTextField = "ItemGroup_Name"; ddlMainCategory.DataValueField = "ItemGroup_Sno"; ddlMainCategory.DataBind();
                ddlMainCategory.Items.Insert(0, new ListItem("-- Select Main Category --", "0"));
            }
        }

        private void LoadList()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetSubCategoryList", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                    cmd.Parameters.AddWithValue("@SearchText", string.IsNullOrEmpty(txtSearch.Text) ? (object)DBNull.Value : txtSearch.Text.Trim());
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable(); da.Fill(dt);
                    gvSubCategories.DataSource = dt; gvSubCategories.DataBind();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlMainCategory.SelectedValue == "0" || string.IsNullOrWhiteSpace(txtSubCatName.Text)) { Alert("Main Category and Name are required", "error"); return; }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveSubCategory", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfSubCatSno.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@SubCat_Sno", isUpdate ? Convert.ToInt32(hfSubCatSno.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? 0);
                        cmd.Parameters.AddWithValue("@Category_No", ddlMainCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@SubCat_Name", txtSubCatName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@IsActive", chkActive.Checked);

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();

                        btnBack_Click(null, null);
                        Alert("Sub-Category saved successfully!", "success");
                    }
                }
            }
            catch (SqlException ex) { Alert(ex.Message, "error"); }
            catch (Exception ex) { Alert("Save Error: " + ex.Message, "error"); }
        }

        protected void gvSubCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM ItemSubCategory_Table WHERE SubCat_Sno=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfSubCatSno.Value = id.ToString();
                        LoadDropdowns();
                        ddlMainCategory.SelectedValue = dr["Category_No"].ToString();
                        txtSubCatName.Text = dr["SubCat_Name"].ToString();
                        chkActive.Checked = Convert.ToBoolean(dr["IsActive"]);
                        pnlList.Visible = false; pnlForm.Visible = true;
                    }
                }
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e) { LoadList(); }
        protected void btnOpenCreate_Click(object sender, EventArgs e) { hfSubCatSno.Value = ""; txtSubCatName.Text = ""; LoadDropdowns(); chkActive.Checked = true; pnlList.Visible = false; pnlForm.Visible = true; }
        protected void btnBack_Click(object sender, EventArgs e) { pnlList.Visible = true; pnlForm.Visible = false; LoadList(); }
        private void Alert(string msg, string type) { ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", $"showNotification('{msg.Replace("'", "\\'")}', '{type}');", true); }
    }
}
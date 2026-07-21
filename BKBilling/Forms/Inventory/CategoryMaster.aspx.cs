using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Inventory
{
    public partial class CategoryMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) { Response.Redirect("~/Login.aspx"); return; }
            if (!IsPostBack) { BindGrid(); }
        }

        private void DebugLog(string spName)
        {
            string script = $"console.info('SQL EXECUTION: {spName}');";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "debug_" + Guid.NewGuid(), script, true);
        }

        private void BindParentDropdown()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    string sql = "SELECT ItemGroup_Sno, ItemGroup_Name FROM ItemGroup_Table WHERE Company_No = @cid AND IsActive = 1 ORDER BY ItemGroup_Name";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adp.Fill(dt);

                    ddlParentCategory.DataSource = dt;
                    ddlParentCategory.DataTextField = "ItemGroup_Name";
                    ddlParentCategory.DataValueField = "ItemGroup_Sno";
                    ddlParentCategory.DataBind();
                    ddlParentCategory.Items.Insert(0, new ListItem("PRIMARY", "0"));
                }
            }
            catch (Exception ex) { Alert("Dropdown Error: " + ex.Message, "error"); }
        }

        private void BindGrid(string search = "")
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_GetCategoryList");
                    using (SqlCommand cmd = new SqlCommand("sp_GetCategoryList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@SearchText", string.IsNullOrEmpty(search) ? (object)DBNull.Value : search);

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        gvCategories.DataSource = dt;
                        gvCategories.DataBind();
                    }
                }
            }
            catch (Exception ex) { Alert("Load Error: " + ex.Message, "error"); }
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e)
        {
            ClearForm();
            BindParentDropdown();
            pnlList.Visible = false;
            pnlForm.Visible = true;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlList.Visible = true;
            pnlForm.Visible = false;
            BindGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text)) { Alert("Name is required!", "error"); return; }
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_SaveCategory");
                    using (SqlCommand cmd = new SqlCommand("sp_SaveCategory", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfCategorySno.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@ItemGroup_Sno", isUpdate ? Convert.ToInt32(hfCategorySno.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? 0);
                        cmd.Parameters.AddWithValue("@ItemGroup_Name", txtCategoryName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@ItemGroup_Under", ddlParentCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("Category details saved successfully!", "success");
                btnBack_Click(null, null);
            }
            catch (SqlException ex) { Alert(ex.Message, "error"); }
            catch (Exception ex) { Alert("Save Error: " + ex.Message, "error"); }
        }

        protected void gvCategories_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrEmpty(e.CommandArgument.ToString())) return;
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRecord")
            {
                try
                {
                    using (SqlConnection conn = DbHelper.GetConnection())
                    {
                        DebugLog("sp_GetCategoryByID");
                        using (SqlCommand cmd = new SqlCommand("sp_GetCategoryByID", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ItemGroup_Sno", id);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    hfCategorySno.Value = dr["ItemGroup_Sno"].ToString();
                                    txtCategoryName.Text = dr["ItemGroup_Name"].ToString();
                                    chkIsActive.Checked = Convert.ToBoolean(dr["IsActive"]);

                                    BindParentDropdown();
                                    ddlParentCategory.SelectedValue = dr["ItemGroup_Under"].ToString();

                                    pnlList.Visible = false;
                                    pnlForm.Visible = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { Alert("Load Error: " + ex.Message, "error"); }
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e) => BindGrid(txtSearch.Text.Trim());

        private void ClearForm()
        {
            txtCategoryName.Text = "";
            hfCategorySno.Value = "";
            chkIsActive.Checked = true;
        }

        private void Alert(string msg, string type)
        {
            string clean = msg.Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
            string script = $"showNotification('{clean}', '{type}');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertMessage", script, true);
        }
    }
}
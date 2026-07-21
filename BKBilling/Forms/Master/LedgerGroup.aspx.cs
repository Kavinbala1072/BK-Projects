using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class LedgerGroup : System.Web.UI.Page
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
                ddlParentGroup.Items.Insert(0, new ListItem("PRIMARY", "0"));
            }
        }

        protected void ddlParentGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlParentGroup.SelectedValue == "0")
            {
                ddlNature.Enabled = true;
            }
            else
            {
                try
                {
                    using (SqlConnection conn = DbHelper.GetConnection())
                    {
                        DebugLog("sp_GetLedgerGroupByID");
                        using (SqlCommand cmd = new SqlCommand("sp_GetLedgerGroupByID", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@LedgerGroup_Sno", ddlParentGroup.SelectedValue);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    ddlNature.SelectedValue = dr["Nature"].ToString();
                                    ddlNature.Enabled = false; // Lock nature to match parent
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { Alert("Nature Load Error: " + ex.Message, "error"); }
            }
        }

        private void BindGrid(string search = "")
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_GetLedgerGroupList");
                    using (SqlCommand cmd = new SqlCommand("sp_GetLedgerGroupList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@SearchText", string.IsNullOrEmpty(search) ? (object)DBNull.Value : search);

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        gvGroups.DataSource = dt;
                        gvGroups.DataBind();
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
            if (string.IsNullOrEmpty(txtGroupName.Text)) { Alert("Group Name is required", "error"); return; }
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_SaveLedgerGroup");
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

                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("Group registration saved successfully!", "success");
                btnBack_Click(null, null);
            }
            catch (SqlException ex) { Alert(ex.Message, "error"); }
            catch (Exception ex) { Alert("Save Error: " + ex.Message, "error"); }
        }

        protected void gvGroups_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrEmpty(e.CommandArgument.ToString())) return;
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditGroup")
            {
                try
                {
                    using (SqlConnection conn = DbHelper.GetConnection())
                    {
                        DebugLog("sp_GetLedgerGroupByID");
                        using (SqlCommand cmd = new SqlCommand("sp_GetLedgerGroupByID", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@LedgerGroup_Sno", id);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    hfGroupId.Value = dr["LedgerGroup_Sno"].ToString();
                                    txtGroupName.Text = dr["LedgerGroup_Name"].ToString();
                                    chkIsActive.Checked = Convert.ToBoolean(dr["IsActive"]);

                                    BindParentDropdown();
                                    ddlParentGroup.SelectedValue = dr["Ledgergroup_Under"].ToString();
                                    ddlNature.SelectedValue = dr["Nature"].ToString();

                                    ddlNature.Enabled = (ddlParentGroup.SelectedValue == "0");

                                    pnlList.Visible = false;
                                    pnlForm.Visible = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { Alert("Edit Load Error: " + ex.Message, "error"); }
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e) => BindGrid(txtSearch.Text.Trim());

        private void ClearForm()
        {
            txtGroupName.Text = "";
            hfGroupId.Value = "";
            chkIsActive.Checked = true;
            ddlNature.Enabled = true;
        }

        private void Alert(string msg, string type)
        {
            string script = $"showNotification('{msg.Replace("'", "\\'")}', '{type}');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", script, true);
        }
    }
}
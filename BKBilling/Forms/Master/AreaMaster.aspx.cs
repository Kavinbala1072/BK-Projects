using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class AreaMaster : System.Web.UI.Page
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

        private void BindParentArea()
        {
            try
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
                    ddlAreaUnder.Items.Insert(0, new ListItem("PRIMARY", "0"));
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
                    DebugLog("sp_GetAreaList");
                    using (SqlCommand cmd = new SqlCommand("sp_GetAreaList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@SearchText", string.IsNullOrEmpty(search) ? (object)DBNull.Value : search);

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        gvArea.DataSource = dt;
                        gvArea.DataBind();
                    }
                }
            }
            catch (Exception ex) { Alert("Load Error: " + ex.Message, "error"); }
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e)
        {
            ClearForm();
            BindParentArea();
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
            if (string.IsNullOrWhiteSpace(txtAreaName.Text)) { Alert("Name is required!", "error"); return; }
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_SaveArea");
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

                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("Data saved successfully!", "success");
                btnBack_Click(null, null);
            }
            catch (SqlException ex) { Alert(ex.Message, "error"); }
            catch (Exception ex) { Alert("System Error: " + ex.Message, "error"); }
        }

        protected void gvArea_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrEmpty(e.CommandArgument.ToString())) return;
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditArea")
            {
                try
                {
                    using (SqlConnection conn = DbHelper.GetConnection())
                    {
                        DebugLog("sp_GetAreaByID");
                        using (SqlCommand cmd = new SqlCommand("sp_GetAreaByID", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Area_Sno", id);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    hfAreaId.Value = dr["Area_Sno"].ToString();
                                    txtAreaName.Text = dr["Area_Name"].ToString();
                                    chkIsActive.Checked = Convert.ToBoolean(dr["IsActive"]);

                                    BindParentArea();
                                    ddlAreaUnder.SelectedValue = dr["Area_Under"].ToString();

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
            txtAreaName.Text = "";
            hfAreaId.Value = "";
            chkIsActive.Checked = true;
        }

        private void Alert(string msg, string type)
        {
            string script = $"showNotification('{msg.Replace("'", "\\'")}', '{type}');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", script, true);
        }
    }
}
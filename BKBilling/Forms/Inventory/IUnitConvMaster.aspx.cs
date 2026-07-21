using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class IUnitConvMaster : System.Web.UI.Page
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

        private void BindUnits()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_GetUnitList");
                    using (SqlCommand cmd = new SqlCommand("sp_GetUnitList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@SearchText", DBNull.Value);

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);

                        ddlMainUnit.Items.Clear();
                        ddlSubUnit.Items.Clear();

                        ddlMainUnit.DataSource = dt;
                        ddlMainUnit.DataTextField = "Unit_Sname";
                        ddlMainUnit.DataValueField = "Unit_Sno";
                        ddlMainUnit.DataBind();

                        ddlSubUnit.DataSource = dt;
                        ddlSubUnit.DataTextField = "Unit_Sname";
                        ddlSubUnit.DataValueField = "Unit_Sno";
                        ddlSubUnit.DataBind();

                        ddlMainUnit.Items.Insert(0, new ListItem("-- Main --", "0"));
                        ddlSubUnit.Items.Insert(0, new ListItem("-- Sub --", "0"));
                    }
                }
            }
            catch (Exception ex) { Alert("Unit Load Error: " + ex.Message, "error"); }
        }

        private void BindGrid()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_GetUnitConversionList");
                    using (SqlCommand cmd = new SqlCommand("sp_GetUnitConversionList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        gvConv.DataSource = dt;
                        gvConv.DataBind();
                    }
                }
            }
            catch (Exception ex) { Alert("Load Error: " + ex.Message, "error"); }
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e)
        {
            ClearForm();
            BindUnits();
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
            if (ddlMainUnit.SelectedValue == "0" || ddlSubUnit.SelectedValue == "0" || string.IsNullOrEmpty(txtMultiplier.Text))
            {
                Alert("Select units and enter multiplier", "error");
                return;
            }
            if (ddlMainUnit.SelectedValue == ddlSubUnit.SelectedValue)
            {
                Alert("Main and Sub units cannot be the same", "error");
                return;
            }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_SaveUnitConversion");
                    using (SqlCommand cmd = new SqlCommand("sp_SaveUnitConversion", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfConvId.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@Conv_Sno", isUpdate ? Convert.ToInt32(hfConvId.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@MainUnit_Sno", ddlMainUnit.SelectedValue);
                        cmd.Parameters.AddWithValue("@SubUnit_Sno", ddlSubUnit.SelectedValue);
                        cmd.Parameters.AddWithValue("@Multiplier", txtMultiplier.Text.Trim());
                        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("Conversion logic saved successfully!", "success");
                btnBack_Click(null, null);
            }
            catch (SqlException ex) { Alert(ex.Message, "error"); }
            catch (Exception ex) { Alert("Save Error: " + ex.Message, "error"); }
        }

        protected void gvConv_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrEmpty(e.CommandArgument.ToString())) return;
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditConv")
            {
                try
                {
                    using (SqlConnection conn = DbHelper.GetConnection())
                    {
                        DebugLog("sp_GetUnitConversionByID");
                        using (SqlCommand cmd = new SqlCommand("sp_GetUnitConversionByID", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Conv_Sno", id);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    hfConvId.Value = dr["Conv_Sno"].ToString();
                                    BindUnits();
                                    ddlMainUnit.SelectedValue = dr["MainUnit_Sno"].ToString();
                                    ddlSubUnit.SelectedValue = dr["SubUnit_Sno"].ToString();
                                    txtMultiplier.Text = dr["Multiplier"].ToString();
                                    chkIsActive.Checked = Convert.ToBoolean(dr["IsActive"]);

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

        protected void txtSearch_TextChanged(object sender, EventArgs e) => BindGrid();

        private void ClearForm()
        {
            hfConvId.Value = "";
            txtMultiplier.Text = "";
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
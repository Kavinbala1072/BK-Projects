using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class UnitMaster : System.Web.UI.Page
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

        private void BindGrid(string search = "")
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
                        cmd.Parameters.AddWithValue("@SearchText", string.IsNullOrEmpty(search) ? (object)DBNull.Value : search);

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        gvUnits.DataSource = dt;
                        gvUnits.DataBind();
                    }
                }
            }
            catch (Exception ex) { Alert("Load Error: " + ex.Message, "error"); }
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e)
        {
            ClearForm();
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
            if (string.IsNullOrEmpty(txtUnitName.Text) || string.IsNullOrEmpty(txtSymbol.Text)) { Alert("Required fields missing", "error"); return; }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_SaveUnit");
                    using (SqlCommand cmd = new SqlCommand("sp_SaveUnit", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfUnitId.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@Unit_Sno", isUpdate ? Convert.ToInt32(hfUnitId.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@Unit_Name", txtUnitName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@Unit_Sname", txtSymbol.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@Decimal_Places", ddlDecimals.SelectedValue);
                        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);

                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("Unit details saved successfully!", "success");
                btnBack_Click(null, null);
            }
            catch (SqlException ex) { Alert(ex.Message, "error"); }
            catch (Exception ex) { Alert("Save Error: " + ex.Message, "error"); }
        }

        protected void gvUnits_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.IsNullOrEmpty(e.CommandArgument.ToString())) return;
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditUnit")
            {
                try
                {
                    using (SqlConnection conn = DbHelper.GetConnection())
                    {
                        DebugLog("sp_GetUnitByID");
                        using (SqlCommand cmd = new SqlCommand("sp_GetUnitByID", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Unit_Sno", id);
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    hfUnitId.Value = dr["Unit_Sno"].ToString();
                                    txtUnitName.Text = dr["Unit_Name"].ToString();
                                    txtSymbol.Text = dr["Unit_Sname"].ToString();
                                    ddlDecimals.SelectedValue = dr["Decimal_Places"].ToString();
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

        protected void txtSearch_TextChanged(object sender, EventArgs e) => BindGrid(txtSearch.Text.Trim());

        private void ClearForm()
        {
            txtUnitName.Text = "";
            txtSymbol.Text = "";
            hfUnitId.Value = "";
            ddlDecimals.SelectedIndex = 2;
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
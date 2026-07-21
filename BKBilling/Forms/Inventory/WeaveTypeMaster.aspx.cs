using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using BKBilling.Class;

namespace BKBilling.Forms.Inventory
{
    public partial class WeaveTypeMaster : System.Web.UI.Page
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
                    DebugLog("sp_GetWeaveTypeList");
                    using (SqlCommand cmd = new SqlCommand("sp_GetWeaveTypeList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@SearchText", string.IsNullOrEmpty(search) ? (object)DBNull.Value : search);

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adp.Fill(dt);
                        gvWeave.DataSource = dt;
                        gvWeave.DataBind();
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
            if (string.IsNullOrWhiteSpace(txtWeaveName.Text)) { Alert("Name is required!", "error"); return; }
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_SaveWeaveType");
                    using (SqlCommand cmd = new SqlCommand("sp_SaveWeaveType", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfWeaveSno.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@Weave_Sno", isUpdate ? Convert.ToInt32(hfWeaveSno.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? 0);
                        cmd.Parameters.AddWithValue("@Weave_Name", txtWeaveName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@IsActive", chkActive.Checked);

                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("Weave type details saved successfully!", "success");
                btnBack_Click(null, null);
            }
            catch (SqlException ex) { Alert(ex.Message, "error"); }
            catch (Exception ex) { Alert("Save Error: " + ex.Message, "error"); }
        }

        protected void gvWeave_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (string.IsNullOrEmpty(e.CommandArgument.ToString())) return;
            int id = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditRecord")
            {
                try
                {
                    using (SqlConnection conn = DbHelper.GetConnection())
                    {
                        SqlCommand cmd = new SqlCommand("SELECT * FROM WeaveType_Table WHERE Weave_Sno=@id", conn);
                        cmd.Parameters.AddWithValue("@id", id);
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            hfWeaveSno.Value = dr["Weave_Sno"].ToString();
                            txtWeaveName.Text = dr["Weave_Name"].ToString();
                            chkActive.Checked = Convert.ToBoolean(dr["IsActive"]);
                            pnlList.Visible = false;
                            pnlForm.Visible = true;
                        }
                    }
                }
                catch (Exception ex) { Alert("Load Error: " + ex.Message, "error"); }
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e) => BindGrid(txtSearch.Text.Trim());

        private void ClearForm()
        {
            txtWeaveName.Text = "";
            hfWeaveSno.Value = "";
            chkActive.Checked = true;
        }

        private void Alert(string msg, string type)
        {
            string clean = msg.Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
            string script = $"showNotification('{clean}', '{type}');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertMessage", script, true);
        }
    }
}
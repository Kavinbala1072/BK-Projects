using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Settings
{
    public partial class FormSetting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack) BindData();
        }

        private void BindData()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                string sql = "SELECT * FROM Form_Settings WHERE Company_ID = @cid ORDER BY Form_Name";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                rptSettings.DataSource = dt;
                rptSettings.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    foreach (RepeaterItem item in rptSettings.Items)
                    {
                        HiddenField hfSno = (HiddenField)item.FindControl("hfSno");
                        CheckBox chk = (CheckBox)item.FindControl("chk");

                        int sno = Convert.ToInt32(hfSno.Value);
                        bool isEnabled = chk.Checked;

                        string sql = "UPDATE Form_Settings SET Is_Enabled = @val WHERE Setting_Sno = @sno";
                        SqlCommand cmd = new SqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@val", isEnabled);
                        cmd.Parameters.AddWithValue("@sno", sno);
                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("Configuration applied successfully!", "success");
            }
            catch (Exception ex)
            {
                Alert("Error: " + ex.Message, "error");
            }
        }

        private void Alert(string msg, string type)
        {
            string script = $"showNotification('{msg.Replace("'", "\\'")}', '{type}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", script, true);
        }
    }
}
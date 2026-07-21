using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class BackupForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");

            if (!IsPostBack)
            {
                // Set a default path
                txtBackupPath.Text = @"D:\Backups\";
            }
        }

        protected void btnLocalBackup_Click(object sender, EventArgs e)
        {
            string folderPath = txtBackupPath.Text.Trim();

            // Basic validation
            if (string.IsNullOrEmpty(folderPath))
            {
                Alert("Please enter a valid drive path.");
                return;
            }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_BackupToLocalDrive", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FolderPath", folderPath);

                        if (conn.State == ConnectionState.Closed) conn.Open();

                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            pnlSuccess.Visible = true;
                            lblFinalPath.Text = result.ToString();
                            Alert("Local backup completed successfully!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Alert("SQL Error: " + ex.Message);
            }
        }

        private void Alert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", $"alert('{msg.Replace("'", "\\'")}');", true);
        }
    }
}
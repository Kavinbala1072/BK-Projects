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
                LoadExistingBackupPath();
                pnlSuccess.Visible = false;
            }
        }

        private void LoadExistingBackupPath()
        {
            string defaultPath = @"D:\Backups\";
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string selectSql = "SELECT Ctl_Value FROM Control_Table WHERE Company_No = @cid AND Ctl_MtDesc = 'LOCAL_BACKUP_PATH'";
                    SqlCommand cmd = new SqlCommand(selectSql, conn);
                    cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);

                    object val = cmd.ExecuteScalar();

                    if (val != null && !string.IsNullOrEmpty(val.ToString()))
                    {
                        txtBackupPath.Text = val.ToString();
                    }
                    else
                    {
                        string insertSql = @"INSERT INTO Control_Table (Company_No, Ctl_MtDesc, Ctl_Value, Modified_Date) 
                                     VALUES (@cid, 'LOCAL_BACKUP_PATH', @val, GETDATE())";

                        using (SqlCommand insCmd = new SqlCommand(insertSql, conn))
                        {
                            insCmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                            insCmd.Parameters.AddWithValue("@val", defaultPath);
                            insCmd.ExecuteNonQuery();
                        }
                        txtBackupPath.Text = defaultPath;
                    }
                }
            }
            catch { txtBackupPath.Text = defaultPath; }
        }

        protected void btnLocalBackup_Click(object sender, EventArgs e)
        {
            string folderPath = txtBackupPath.Text.Trim();
            if (string.IsNullOrEmpty(folderPath))
            {
                Alert("Please enter a valid drive path.", "error");
                return;
            }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();

                    string updateSql = @"UPDATE Control_Table SET Ctl_Value = @val, Modified_Date = GETDATE() 
                                 WHERE Company_No = @cid AND Ctl_MtDesc = 'LOCAL_BACKUP_PATH'";
                    using (SqlCommand updCmd = new SqlCommand(updateSql, conn))
                    {
                        updCmd.Parameters.AddWithValue("@val", folderPath);
                        updCmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                        updCmd.ExecuteNonQuery();
                    }

                    using (SqlCommand cmd = new SqlCommand("sp_BackupToLocalDrive", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@FolderPath", folderPath);

                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            pnlSuccess.Visible = true;
                            lblFinalPath.Text = result.ToString();
                            Alert("Configuration updated and Backup completed successfully!", "success");
                        }
                        else
                        {
                            Alert("Backup failed. Check permissions.", "error");
                        }
                    }
                }
            }
            catch (Exception ex) { Alert("Error: " + ex.Message, "error"); }
        }

        private void SaveControlPath(long cid, string path)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_SetControlValue", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_No", cid);
                    cmd.Parameters.AddWithValue("@MtDesc", "LOCAL_BACKUP_PATH");
                    cmd.Parameters.AddWithValue("@Value", path);

                    if (conn.State == ConnectionState.Closed) conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void Alert(string msg, string type)
        {
            string clean = msg.Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
            string script = $"showNotification('{clean}', '{type}');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Guid.NewGuid().ToString(), script, true);
        }
    }
}
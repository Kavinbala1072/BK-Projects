using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class UserMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) { Response.Redirect("~/Login.aspx"); return; }
            if (!IsPostBack) { LoadUsers(); }
        }

        private void DebugLog(string spName)
        {
            string script = $"console.info('SQL EXECUTION: {spName}');";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "debug_" + Guid.NewGuid(), script, true);
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_GetUserList");
                    using (SqlCommand cmd = new SqlCommand("sp_GetUserList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvUsers.DataSource = dt;
                        gvUsers.DataBind();
                    }
                }
            }
            catch (Exception ex) { Alert("Error loading users: " + ex.Message, "error"); }
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e)
        {
            ClearInputs();
            pnlList.Visible = false;
            pnlForm.Visible = true;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlList.Visible = true;
            pnlForm.Visible = false;
            LoadUsers();
        }

        protected void gvUsers_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord") { LoadUserForEdit(Convert.ToInt64(e.CommandArgument)); }
        }

        private void LoadUserForEdit(long sno)
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_GetUserByID");
                    using (SqlCommand cmd = new SqlCommand("sp_GetUserByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@User_Sno", sno);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                hfUserSno.Value = sno.ToString();
                                txtUsername.Text = dr["Username"].ToString();
                                txtFullName.Text = dr["FullName"].ToString();
                                ddlRole.SelectedValue = dr["Role"].ToString();
                                txtPhone.Text = dr["Phone"].ToString();
                                txtEmail.Text = dr["Email"].ToString();
                                txtAdd1.Text = dr["Address_1"].ToString();
                                txtAdd2.Text = dr["Address_2"].ToString();
                                chkIsActive.Checked = Convert.ToBoolean(dr["IsActive"]);

                                if (dr["Join_Date"] != DBNull.Value)
                                    txtJoinDate.Text = Convert.ToDateTime(dr["Join_Date"]).ToString("yyyy-MM-dd");

                                txtPass.Text = txtConfirm.Text = "";
                                pnlList.Visible = false;
                                pnlForm.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Alert("Load Error: " + ex.Message, "error"); }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                Alert("Username and Full Name are required!", "error");
                return;
            }

            bool isUpdate = !string.IsNullOrEmpty(hfUserSno.Value);

            // Password logic: required for new, optional for edit
            if (!isUpdate || !string.IsNullOrWhiteSpace(txtPass.Text))
            {
                if (txtPass.Text != txtConfirm.Text) { Alert("Passwords do not match!", "error"); return; }
                if (!isUpdate && string.IsNullOrWhiteSpace(txtPass.Text)) { Alert("Password required for new users!", "error"); return; }
            }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_SaveUser");
                    using (SqlCommand cmd = new SqlCommand("sp_SaveUser", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@User_Sno", isUpdate ? Convert.ToInt64(hfUserSno.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                        cmd.Parameters.AddWithValue("@FullName", txtFullName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Role", ddlRole.SelectedValue);
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address_1", txtAdd1.Text.Trim());
                        cmd.Parameters.AddWithValue("@Address_2", txtAdd2.Text.Trim());
                        cmd.Parameters.AddWithValue("@IsActive", chkIsActive.Checked);
                        cmd.Parameters.AddWithValue("@Join_Date", string.IsNullOrEmpty(txtJoinDate.Text) ? (object)DBNull.Value : txtJoinDate.Text);

                        if (!string.IsNullOrWhiteSpace(txtPass.Text))
                            cmd.Parameters.AddWithValue("@Password", SecurityHelper.ComputeHash(txtPass.Text));
                        else
                            cmd.Parameters.AddWithValue("@Password", DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("User account saved successfully!", "success");
                btnBack_Click(null, null);
            }
            catch (SqlException ex) { Alert(ex.Message, "error"); }
            catch (Exception ex) { Alert("Save Error: " + ex.Message, "error"); }
        }

        private void Alert(string msg, string type)
        {
            string script = $"showNotification('{msg.Replace("'", "\\'")}', '{type}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", script, true);
        }

        private void ClearInputs()
        {
            hfUserSno.Value = "";
            txtUsername.Text = txtFullName.Text = txtPhone.Text = txtEmail.Text = txtAdd1.Text = txtAdd2.Text = txtPass.Text = txtConfirm.Text = "";
            txtJoinDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            ddlRole.SelectedIndex = 0;
            chkIsActive.Checked = true;
        }
    }
}
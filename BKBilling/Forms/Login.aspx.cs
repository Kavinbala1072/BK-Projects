using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using BKBilling.Class;

namespace BKBilling.Forms
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["Init"] = true;
                ShowSessionMessageIfAny();
            }
        }

        private void ShowSessionMessageIfAny()
        {
            string reason = Request.QueryString["reason"];
            if (string.IsNullOrEmpty(reason)) return;

            string message;
            switch (reason)
            {
                case "Expired":
                    message = "Your session has expired. Please log in again.";
                    break;
                case "Superseded":
                    message = "You have been logged out because your account was signed in from another device or browser.";
                    break;
                case "NoSession":
                    message = "You have been logged out. Please log in again.";
                    break;
                default:
                    return;
            }

            string script = "alert(" + System.Web.HttpUtility.JavaScriptStringEncode(message, true) + ");";
            ClientScript.RegisterStartupScript(this.GetType(), "SessionMessage", script, true);
        }

        public class Response
        {
            public bool success { get; set; }
            public string message { get; set; }
            public string token { get; set; }
            public string redirect { get; set; }
            public string expiry { get; set; }
        }

        [WebMethod(EnableSession = true)]
        public static string UpdateDB()
        {
            try
            {
                CompanyTable.CreateAllSchema();
                return "Database Initialized Successfully!";
            }
            catch (Exception ex) { return "Error: " + ex.Message; }
        }

        [WebMethod]
        public static List<object> GetCompanies()
        {
            List<object> list = new List<object>();
            string connStr = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand("SELECT Company_Sno, Company_Name FROM Company_Table ORDER BY Company_Name", conn);
                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new { ID = dr["Company_Sno"].ToString(), Name = dr["Company_Name"].ToString() });
                        }
                    }
                }
            }
            catch { }
            return list;
        }

        [WebMethod(EnableSession = true)]
        public static object ProcessLogin(string user, string pass, string companyId)
        {
            if (string.IsNullOrWhiteSpace(user))
                return new Response { success = false, message = "Please enter a username." };

            const int sessionHours = 12;

            if (user.Trim().Equals("BKAdmin", StringComparison.OrdinalIgnoreCase) && pass == "BK@2026")
            {
                string cid = (string.IsNullOrEmpty(companyId) || companyId == "0") ? "1000" : companyId;

                string newToken = SessionHelper.CreateSession("BKAdmin", cid, sessionHours);
                string newExpiry = ((DateTime)System.Web.HttpContext.Current.Session["Expiry"]).ToString("yyyy-MM-dd HH:mm:ss");

                return new Response { success = true, redirect = "MainForm.aspx", token = newToken, expiry = newExpiry };
            }

            if (string.IsNullOrEmpty(companyId) || companyId == "0")
                return new Response { success = false, message = "Please select a company." };

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string hashedPassword = SecurityHelper.ComputeHash(pass);
                    string sql = @"SELECT FullName, User_Sno FROM User_Table 
                                   WHERE Username=@u AND Password=@p AND Company_No=@c AND IsActive=1";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", user.Trim());
                    cmd.Parameters.AddWithValue("@p", hashedPassword);
                    cmd.Parameters.AddWithValue("@c", companyId);

                    conn.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string fullName = dr["FullName"].ToString();
                            int userSno = Convert.ToInt32(dr["User_Sno"]);
                            dr.Close();

                            // Overwrites any existing Active_Sessions row for this user,
                            // so if they're already logged in elsewhere, that session dies now.
                            string newToken = SessionHelper.CreateSession(fullName, companyId, sessionHours);
                            System.Web.HttpContext.Current.Session["UserID"] = userSno;
                            string newExpiry = ((DateTime)System.Web.HttpContext.Current.Session["Expiry"]).ToString("yyyy-MM-dd HH:mm:ss");

                            return new Response { success = true, redirect = "MainForm.aspx", token = newToken, expiry = newExpiry };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // NOTE: log ex server-side; avoid returning raw exception text to the client in production.
                return new Response { success = false, message = "Database Error: " + ex.Message };
            }

            return new Response { success = false, message = "Invalid Username or Password." };
        }

        [WebMethod]
        public static object ResetPassword(string user, string secret, string newPass, string companyId)
        {
            if (secret != "BK@2026") return new Response { success = false, message = "Invalid Secret Key" };

            try
            {
                string connStr = ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE User_Table SET Password=@p WHERE Username=@u AND Company_No=@c", conn);
                    cmd.Parameters.AddWithValue("@p", SecurityHelper.ComputeHash(newPass));
                    cmd.Parameters.AddWithValue("@u", user.Trim());
                    cmd.Parameters.AddWithValue("@c", companyId);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0
                        ? new Response { success = true, message = "Password updated successfully!" }
                        : new Response { success = false, message = "User not found in selected company." };
                }
            }
            catch (Exception ex) { return new Response { success = false, message = ex.Message }; }
        }
    }
}
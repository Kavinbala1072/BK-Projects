using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web.Services;

namespace BKSoftwares
{
    public partial class AppLogin : System.Web.UI.Page
    {
        private const string ADMIN_SECRET_KEY = "BK@2026";

        [WebMethod]
        public static object ProcessLogin(string username, string password)
        {
            string hashedInput = ComputeSha256Hash(password);
            string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = "SELECT COUNT(*) FROM Users WHERE Username=@user AND Password=@pass";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@user", username.Trim());
                    cmd.Parameters.AddWithValue("@pass", hashedInput);
                    conn.Open();
                    if ((int)cmd.ExecuteScalar() > 0)
                    {
                        string token = Guid.NewGuid().ToString();
                        string expiry = DateTime.Now.AddMinutes(60).ToString("yyyy-MM-dd HH:mm:ss");
                        System.Web.HttpContext.Current.Session["UserName"] = username;
                        System.Web.HttpContext.Current.Session["AuthToken"] = token;
                        return new { success = true, token = token, expiry = expiry, user = username };
                    }
                }
            }
            catch (Exception ex) { return new { success = false, message = ex.Message }; }
            return new { success = false, message = "Invalid Credentials" };
        }

        [WebMethod]
        public static object CreateUser(string name, string user, string pass, string secret)
        {
            if (secret != ADMIN_SECRET_KEY) return new { success = false, message = "Invalid Secret Admin Key!" };

            string hashedPass = ComputeSha256Hash(pass);
            string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    // Check if exists
                    string check = "SELECT COUNT(*) FROM Users WHERE Username=@u";
                    SqlCommand cmdCheck = new SqlCommand(check, conn);
                    cmdCheck.Parameters.AddWithValue("@u", user);
                    conn.Open();
                    if ((int)cmdCheck.ExecuteScalar() > 0) return new { success = false, message = "Username already exists!" };

                    // Insert
                    string sql = "INSERT INTO Users (FullName, Username, Password) VALUES (@n, @u, @p)";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@n", name);
                    cmd.Parameters.AddWithValue("@u", user);
                    cmd.Parameters.AddWithValue("@p", hashedPass);
                    cmd.ExecuteNonQuery();
                    return new { success = true, message = "User created successfully!" };
                }
            }
            catch (Exception ex) { return new { success = false, message = ex.Message }; }
        }

        [WebMethod]
        public static object ResetPassword(string user, string newPass, string secret)
        {
            if (secret != ADMIN_SECRET_KEY) return new { success = false, message = "Invalid Secret Admin Key!" };

            string hashedPass = ComputeSha256Hash(newPass);
            string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string sql = "UPDATE Users SET Password=@p WHERE Username=@u";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", user);
                    cmd.Parameters.AddWithValue("@p", hashedPass);
                    conn.Open();
                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0) return new { success = true, message = "Password updated successfully!" };
                    else return new { success = false, message = "User not found!" };
                }
            }
            catch (Exception ex) { return new { success = false, message = ex.Message }; }
        }

        static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder b = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++) b.Append(bytes[i].ToString("x2"));
                return b.ToString();
            }
        }
    }
}
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.SessionState;

namespace BKBilling.Class
{
    public enum SessionStatus
    {
        Valid,
        NoSession, 
        Expired,   
        Superseded 
    }

    public static class SessionHelper
    {
        private static string ConnStr => ConfigurationManager.ConnectionStrings["MyDbConn"].ConnectionString;

        public static string CreateSession(string username, string companyId, int hoursValid = 12)
        {
            string token = Guid.NewGuid().ToString();
            DateTime expiry = DateTime.Now.AddHours(hoursValid);

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                string sql = @"
                    MERGE Active_Sessions AS target
                    USING (SELECT @u AS Username) AS src
                        ON target.Username = src.Username
                    WHEN MATCHED THEN
                        UPDATE SET CompanyID = @c, AuthToken = @t, LoginTime = GETDATE(), ExpiryTime = @e
                    WHEN NOT MATCHED THEN
                        INSERT (Username, CompanyID, AuthToken, LoginTime, ExpiryTime)
                        VALUES (@u, @c, @t, GETDATE(), @e);";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@c", companyId);
                cmd.Parameters.AddWithValue("@t", token);
                cmd.Parameters.AddWithValue("@e", expiry);
                cmd.ExecuteNonQuery();
            }

            var session = HttpContext.Current.Session;
            session["UserName"] = username;
            session["CompanyID"] = companyId;
            session["AuthToken"] = token;
            session["Expiry"] = expiry;

            return token;
        }

        public static SessionStatus ValidateSession(HttpSessionState session)
        {
            if (session["UserName"] == null || session["AuthToken"] == null)
                return SessionStatus.NoSession;

            string username = session["UserName"].ToString();
            string sessionToken = session["AuthToken"].ToString();

            using (SqlConnection conn = new SqlConnection(ConnStr))
            {
                string sql = "SELECT AuthToken, ExpiryTime FROM Active_Sessions WHERE Username=@u";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@u", username);
                conn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (!dr.Read())
                        return SessionStatus.NoSession;

                    string dbToken = dr["AuthToken"].ToString();
                    DateTime expiry = Convert.ToDateTime(dr["ExpiryTime"]);

                    if (expiry < DateTime.Now)
                        return SessionStatus.Expired;

                    if (dbToken != sessionToken)
                        return SessionStatus.Superseded;

                    return SessionStatus.Valid;
                }
            }
        }

        public static void EndSession(HttpSessionState session)
        {
            if (session["UserName"] != null)
            {
                using (SqlConnection conn = new SqlConnection(ConnStr))
                {
                    string sql = "DELETE FROM Active_Sessions WHERE Username=@u";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@u", session["UserName"].ToString());
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            session.Clear();
            session.Abandon();
        }
    }
}
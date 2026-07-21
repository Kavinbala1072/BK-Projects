using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BKBilling.Class
{
    public static class SecurityHelper
    {
        public static string ComputeHash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static string GetExpiryString(int hours = 8)
        {
            return DateTime.Now.AddHours(hours).ToString("yyyy-MM-ddTHH:mm:ss");
        }

        public static object CreateResponse(bool success, string message, object data = null)
        {
            return new { success = success, message = message, data = data };
        }
    }
}
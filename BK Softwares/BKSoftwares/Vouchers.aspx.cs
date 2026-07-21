using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using Newtonsoft.Json;

namespace BKSoftwares
{
    public partial class Vouchers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        [WebMethod]
        public static string GetCustomerDropdown()
        {
            string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT CustomerID, CustomerName, City FROM Customers ORDER BY CustomerName";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod]
        public static string GetRecentVouchers()
        {
            string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"SELECT TOP 20 v.VoucherID, v.VoucherNo, v.VoucherDate, v.VoucherType, 
                               c.CustomerName, v.Amount, v.PaymentMode 
                               FROM Vouchers v 
                               INNER JOIN Customers c ON v.CustomerID = c.CustomerID 
                               ORDER BY v.VoucherID ASC";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod]
        public static string GetVoucherByID(int id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT * FROM Vouchers WHERE VoucherID = @ID";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@ID", id);
                da.Fill(dt);
            }
            return JsonConvert.SerializeObject(dt);
        }

        [WebMethod]
        public static string SaveVoucher(VoucherData vch)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "";
                if (vch.VchID == 0)
                {
                    sql = @"INSERT INTO Vouchers (VoucherDate, VoucherType, CustomerID, Amount, PaymentMode, Narration) 
                           VALUES (@Date, @Type, @CustID, @Amount, @Mode, @Narration)";
                }
                else
                {
                    sql = @"UPDATE Vouchers SET VoucherDate=@Date, VoucherType=@Type, CustomerID=@CustID, 
                           Amount=@Amount, PaymentMode=@Mode, Narration=@Narration WHERE VoucherID=@ID";
                }

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", vch.VchID);
                cmd.Parameters.AddWithValue("@Date", vch.VchDate);
                cmd.Parameters.AddWithValue("@Type", vch.VchType);
                cmd.Parameters.AddWithValue("@CustID", vch.CustID);
                cmd.Parameters.AddWithValue("@Amount", vch.Amount);
                cmd.Parameters.AddWithValue("@Mode", vch.Mode);
                cmd.Parameters.AddWithValue("@Narration", vch.Narration);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return "Success";
        }

        public class VoucherData
        {
            public int VchID { get; set; }
            public string VchType { get; set; }
            public string VchDate { get; set; }
            public int CustID { get; set; }
            public decimal Amount { get; set; }
            public string Mode { get; set; }
            public string Narration { get; set; }
        }
    }
}
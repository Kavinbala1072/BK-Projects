using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using Newtonsoft.Json;

namespace BKSoftwares
{
    public partial class Outstanding : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e) { }

        [WebMethod]
        public static string GetOutstandingReport()
        {
            string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"
                    SELECT 
                        c.CustCode, 
                        c.CustomerName, 
                        c.OpeningBalance,
                        ISNULL((SELECT SUM(Amount) FROM Vouchers WHERE CustomerID = c.CustomerID AND VoucherType = 'Receipt'), 0) AS TotalReceipts,
                        ISNULL((SELECT SUM(Amount) FROM Vouchers WHERE CustomerID = c.CustomerID AND VoucherType = 'Payment'), 0) AS TotalPayments,
                        (c.OpeningBalance + 
                         ISNULL((SELECT SUM(Amount) FROM Vouchers WHERE CustomerID = c.CustomerID AND VoucherType = 'Payment'), 0) - 
                         ISNULL((SELECT SUM(Amount) FROM Vouchers WHERE CustomerID = c.CustomerID AND VoucherType = 'Receipt'), 0)) AS Balance
                    FROM Customers c
                    ORDER BY c.CustomerName";

                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.Fill(dt);
            }
            return JsonConvert.SerializeObject(dt);
        }
    }
}
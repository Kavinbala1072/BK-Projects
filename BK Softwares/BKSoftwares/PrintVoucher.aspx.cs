using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BKSoftwares
{
    public partial class PrintVoucher : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                string id = Request.QueryString["ID"];
                LoadVoucher(id);
            }
        }

        private void LoadVoucher(string id)
        {
            string connStr = ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = @"SELECT v.*, c.CustomerName FROM Vouchers v 
                               INNER JOIN Customers c ON v.CustomerID = c.CustomerID 
                               WHERE v.VoucherID = @ID";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow r = dt.Rows[0];
                    litVchType.Text = r["VoucherType"].ToString();
                    litVchNo.Text = r["VoucherNo"].ToString();
                    litVchDate.Text = Convert.ToDateTime(r["VoucherDate"]).ToString("dd-MMM-yyyy");
                    litCustName.Text = r["CustomerName"].ToString();
                    litNarration.Text = r["Narration"].ToString();
                    litMode.Text = r["PaymentMode"].ToString();
                    litAmount.Text = Convert.ToDecimal(r["Amount"]).ToString("N2");
                }
            }
        }
    }
}
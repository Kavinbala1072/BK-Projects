using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using BKBilling.Class;

namespace BKBilling.Forms.Account
{
    public partial class TrialBalance : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CompanyID"] == null) Response.Redirect("~/Forms/Login.aspx");
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                divDiff.Visible = false;
                LoadTrialBalance();
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            LoadTrialBalance();
        }

        private void LoadTrialBalance()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    // This query calculates the Net Balance for every ledger:
                    // (Opening Balance + Total Debit - Total Credit)
                    string sql = @"
                        SELECT 
                            l.ledger_name AS LedgerName,
                            'General' AS GroupName, -- You can join with a Group table if you have one
                            CASE 
                                WHEN (ISNULL(l.Ledger_open,0) + ISNULL(trans.DrSum,0) - ISNULL(trans.CrSum,0)) > 0 
                                THEN (ISNULL(l.Ledger_open,0) + ISNULL(trans.DrSum,0) - ISNULL(trans.CrSum,0)) 
                                ELSE 0 
                            END AS Debit,
                            CASE 
                                WHEN (ISNULL(l.Ledger_open,0) + ISNULL(trans.DrSum,0) - ISNULL(trans.CrSum,0)) < 0 
                                THEN ABS(ISNULL(l.Ledger_open,0) + ISNULL(trans.DrSum,0) - ISNULL(trans.CrSum,0)) 
                                ELSE 0 
                            END AS Credit
                        FROM Ledger_Table l
                        LEFT JOIN (
                            SELECT 
                                LedgerID, 
                                SUM(Dr) as DrSum, 
                                SUM(Cr) as CrSum 
                            FROM (
                                SELECT Dr_Ledger_ID as LedgerID, Debit_Amount as Dr, 0 as Cr FROM Voucher_Table WHERE Vch_Date <= @upto
                                UNION ALL
                                SELECT Cr_Ledger_ID as LedgerID, 0 as Dr, Credit_Amount as Cr FROM Voucher_Table WHERE Vch_Date <= @upto
                            ) t GROUP BY LedgerID
                        ) trans ON l.Ledger_Sno = trans.LedgerID
                        WHERE l.Company_No = @cid
                        AND (ISNULL(l.Ledger_open,0) != 0 OR ISNULL(trans.DrSum,0) != 0 OR ISNULL(trans.CrSum,0) != 0)
                        ORDER BY l.ledger_name";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                    cmd.Parameters.AddWithValue("@upto", txtDate.Text);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvTrialBalance.DataSource = dt;
                    gvTrialBalance.DataBind();

                    CalculateTotals(dt);
                }
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        private void CalculateTotals(DataTable dt)
        {
            decimal totalDr = 0;
            decimal totalCr = 0;

            foreach (DataRow row in dt.Rows)
            {
                totalDr += Convert.ToDecimal(row["Debit"]);
                totalCr += Convert.ToDecimal(row["Credit"]);
            }

            lblSumDr.Text = totalDr.ToString("N2");
            lblSumCr.Text = totalCr.ToString("N2");

            decimal diff = Math.Abs(totalDr - totalCr);
            if (diff > 0.01m) // Account for small decimal rounding
            {
                divDiff.Visible = true;
                lblDiff.Text = diff.ToString("N2");
            }
            else
            {
                divDiff.Visible = false;
            }
        }

        private void Alert(string msg, string type)
        {
            string clean = msg.Replace("'", "\\'");
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", $"showNotification('{clean}', '{type}');", true);
        }
    }
}
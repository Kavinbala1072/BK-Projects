using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using BKBilling.Class;

namespace BKBilling.Forms.Account
{
    public partial class BalanceSheet : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CompanyID"] == null) Response.Redirect("~/Forms/Login.aspx");
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LoadBalanceSheet();
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            LoadBalanceSheet();
        }

        private void LoadBalanceSheet()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    // Query to get net balances of all ledgers
                    string sql = @"
                        SELECT 
                            l.ledger_name,
                            l.LedgerGroup_no,
                            (ISNULL(l.Ledger_open,0) + ISNULL(t.Dr,0) - ISNULL(t.Cr,0)) as NetBal
                        FROM Ledger_Table l
                        LEFT JOIN (
                            SELECT LedgerID, SUM(Dr) as Dr, SUM(Cr) as Cr FROM (
                                SELECT Dr_Ledger_ID as LedgerID, Debit_Amount as Dr, 0 as Cr FROM Voucher_Table WHERE Vch_Date <= @upto
                                UNION ALL
                                SELECT Cr_Ledger_ID as LedgerID, 0 as Dr, Credit_Amount as Cr FROM Voucher_Table WHERE Vch_Date <= @upto
                            ) a GROUP BY LedgerID
                        ) t ON l.Ledger_Sno = t.LedgerID
                        WHERE l.Company_No = @cid";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@upto", txtDate.Text);
                    cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dtMain = new DataTable();
                    da.Fill(dtMain);

                    // Prepare tables for display
                    DataTable dtLiab = new DataTable();
                    dtLiab.Columns.Add("Particulars");
                    dtLiab.Columns.Add("Amount", typeof(decimal));

                    DataTable dtAssets = new DataTable();
                    dtAssets.Columns.Add("Particulars");
                    dtAssets.Columns.Add("Amount", typeof(decimal));

                    decimal totalA = 0;
                    decimal totalL = 0;

                    foreach (DataRow dr in dtMain.Rows)
                    {
                        decimal bal = Convert.ToDecimal(dr["NetBal"]);
                        if (bal == 0) continue;

                        // Logic: Positive is Debit (Asset), Negative is Credit (Liability)
                        // Note: In real ERP, this depends on Group Type (Asset/Liability/Income/Expense)
                        if (bal > 0)
                        {
                            dtAssets.Rows.Add(dr["ledger_name"], bal);
                            totalA += bal;
                        }
                        else
                        {
                            dtLiab.Rows.Add(dr["ledger_name"], Math.Abs(bal));
                            totalL += Math.Abs(bal);
                        }
                    }

                    // Handle Profit & Loss Balancing
                    decimal diff = totalA - totalL;
                    if (diff > 0) // Net Profit (Transfer to Liability side)
                    {
                        dtLiab.Rows.Add("Profit & Loss A/c (Net Profit)", diff);
                        totalL += diff;
                    }
                    else if (diff < 0) // Net Loss (Transfer to Asset side)
                    {
                        dtAssets.Rows.Add("Profit & Loss A/c (Net Loss)", Math.Abs(diff));
                        totalA += Math.Abs(diff);
                    }

                    gvLiabilities.DataSource = dtLiab;
                    gvLiabilities.DataBind();

                    gvAssets.DataSource = dtAssets;
                    gvAssets.DataBind();

                    lblTotalLiab.Text = totalL.ToString("N2");
                    lblTotalAssets.Text = totalA.ToString("N2");
                }
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        private void Alert(string msg, string type)
        {
            string clean = msg.Replace("'", "\\'");
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", $"showNotification('{clean}', '{type}');", true);
        }
    }
}
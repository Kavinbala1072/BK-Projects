using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using BKBilling.Class;

namespace BKBilling.Forms.Account
{
    public partial class ProfitLoss : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CompanyID"] == null) Response.Redirect("~/Forms/Login.aspx");
                txtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LoadProfitLoss();
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            LoadProfitLoss();
        }

        private void LoadProfitLoss()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    // Query to sum up all transactions in the period for every ledger
                    string sql = @"
                        SELECT 
                            l.ledger_name,
                            l.LedgerGroup_no,
                            (ISNULL(t.Dr,0) - ISNULL(t.Cr,0)) as PeriodBal
                        FROM Ledger_Table l
                        JOIN (
                            SELECT LedgerID, SUM(Dr) as Dr, SUM(Cr) as Cr FROM (
                                SELECT Dr_Ledger_ID as LedgerID, Debit_Amount as Dr, 0 as Cr 
                                FROM Voucher_Table WHERE Vch_Date BETWEEN @from AND @to AND Company_No = @cid
                                UNION ALL
                                SELECT Cr_Ledger_ID as LedgerID, 0 as Dr, Credit_Amount as Cr 
                                FROM Voucher_Table WHERE Vch_Date BETWEEN @from AND @to AND Company_No = @cid
                            ) a GROUP BY LedgerID
                        ) t ON l.Ledger_Sno = t.LedgerID
                        WHERE l.Company_No = @cid";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@from", txtFromDate.Text);
                    cmd.Parameters.AddWithValue("@to", txtToDate.Text);
                    cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dtMain = new DataTable();
                    da.Fill(dtMain);

                    DataTable dtExp = new DataTable();
                    dtExp.Columns.Add("Particulars");
                    dtExp.Columns.Add("Amount", typeof(decimal));

                    DataTable dtInc = new DataTable();
                    dtInc.Columns.Add("Particulars");
                    dtInc.Columns.Add("Amount", typeof(decimal));

                    decimal totalExp = 0;
                    decimal totalInc = 0;

                    foreach (DataRow dr in dtMain.Rows)
                    {
                        decimal bal = Convert.ToDecimal(dr["PeriodBal"]);
                        if (bal == 0) continue;

                        // Accounting Rule: 
                        // Dr balance in period = Expenditure
                        // Cr balance in period = Income
                        if (bal > 0)
                        {
                            dtExp.Rows.Add(dr["ledger_name"], bal);
                            totalExp += bal;
                        }
                        else
                        {
                            dtInc.Rows.Add(dr["ledger_name"], Math.Abs(bal));
                            totalInc += Math.Abs(bal);
                        }
                    }

                    // Calculate Net Profit or Net Loss
                    decimal netResult = totalInc - totalExp;

                    if (netResult > 0) // It's a Profit
                    {
                        dtExp.Rows.Add("NET PROFIT (Transferred to Capital)", netResult);
                        totalExp += netResult;
                    }
                    else if (netResult < 0) // It's a Loss
                    {
                        dtInc.Rows.Add("NET LOSS (Transferred to Capital)", Math.Abs(netResult));
                        totalInc += Math.Abs(netResult);
                    }

                    gvExpenses.DataSource = dtExp;
                    gvExpenses.DataBind();

                    gvIncome.DataSource = dtInc;
                    gvIncome.DataBind();

                    lblTotalExp.Text = totalExp.ToString("N2");
                    lblTotalInc.Text = totalInc.ToString("N2");
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
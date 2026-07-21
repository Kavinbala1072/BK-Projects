using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Account
{
    public partial class LedgerView : System.Web.UI.Page
    {
        decimal runningBalance = 0;
        decimal totalDebit = 0;
        decimal totalCredit = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CompanyID"] == null) Response.Redirect("~/Forms/Login.aspx");
                txtFromDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LoadLedgerList();
            }
        }

        private void LoadLedgerList()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT Ledger_Sno, ledger_name FROM Ledger_Table WHERE Company_No=@cid ORDER BY ledger_name", conn);
                cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                ddlLedger.DataSource = cmd.ExecuteReader();
                ddlLedger.DataTextField = "ledger_name";
                ddlLedger.DataValueField = "Ledger_Sno";
                ddlLedger.DataBind();
                ddlLedger.Items.Insert(0, new ListItem("-- Select Account --", "0"));
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            if (ddlLedger.SelectedValue == "0") return;
            LoadStatement();
        }

        private void LoadStatement()
        {
            try
            {
                long ledgerID = Convert.ToInt64(ddlLedger.SelectedValue);
                string fromDate = txtFromDate.Text;
                string toDate = txtToDate.Text;
                string cid = Session["CompanyID"].ToString();

                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    // 1. Calculate Opening Balance (Static Op Balance + Sum of Vouchers before FromDate)
                    string opSql = @"
                        SELECT (ISNULL(SUM(Debit_Amount),0) - ISNULL(SUM(Credit_Amount),0)) 
                        FROM Voucher_Table WHERE (Dr_Ledger_ID = @lid OR Cr_Ledger_ID = @lid) 
                        AND Vch_Date < @from AND Company_No = @cid";

                    SqlCommand cmdOp = new SqlCommand(opSql, conn);
                    cmdOp.Parameters.AddWithValue("@lid", ledgerID);
                    cmdOp.Parameters.AddWithValue("@from", fromDate);
                    cmdOp.Parameters.AddWithValue("@cid", cid);

                    decimal opFromVch = Convert.ToDecimal(cmdOp.ExecuteScalar());

                    // Add Static Opening Balance from Ledger Master
                    SqlCommand cmdMaster = new SqlCommand("SELECT ISNULL(Ledger_open, 0) FROM Ledger_Table WHERE Ledger_Sno=@lid", conn);
                    cmdMaster.Parameters.AddWithValue("@lid", ledgerID);
                    decimal staticOp = Convert.ToDecimal(cmdMaster.ExecuteScalar());

                    runningBalance = staticOp + opFromVch;
                    lblOpeningBal.Text = runningBalance.ToString("N2");

                    // 2. Fetch Transactions for Period
                    string vchSql = @"
                        SELECT Vch_Date, Voucher_No as Vch_No, Vch_Type, Narration,
                        CASE WHEN Dr_Ledger_ID = @lid THEN Debit_Amount ELSE 0 END as Debit,
                        CASE WHEN Cr_Ledger_ID = @lid THEN Credit_Amount ELSE 0 END as Credit
                        FROM Voucher_Table 
                        WHERE (Dr_Ledger_ID = @lid OR Cr_Ledger_ID = @lid)
                        AND Vch_Date BETWEEN @from AND @to AND Company_No = @cid
                        ORDER BY Vch_Date ASC, Voucher_Sno ASC";

                    SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(vchSql, conn)
                    {
                        Parameters = {
                            new SqlParameter("@lid", ledgerID),
                            new SqlParameter("@from", fromDate),
                            new SqlParameter("@to", toDate),
                            new SqlParameter("@cid", cid)
                        }
                    });

                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvLedgerRows.DataSource = dt;
                    gvLedgerRows.DataBind();

                    // Update Final Summary
                    lblTotalDr.Text = totalDebit.ToString("N2");
                    lblTotalCr.Text = totalCredit.ToString("N2");
                    lblClosingBal.Text = runningBalance.ToString("N2");
                }
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        protected void gvLedgerRows_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                decimal dr = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Debit"));
                decimal cr = Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Credit"));

                totalDebit += dr;
                totalCredit += cr;
                runningBalance += (dr - cr);

                Label lblRunning = (Label)e.Row.FindControl("lblRunningBal");
                lblRunning.Text = runningBalance.ToString("N2");

                // Show Dr/Cr suffix for balance
                lblRunning.Text += (runningBalance >= 0) ? " Dr" : " Cr";
            }
        }

        private void Alert(string msg, string type)
        {
            string clean = msg.Replace("'", "\\'");
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", $"showNotification('{clean}', '{type}');", true);
        }
    }
}
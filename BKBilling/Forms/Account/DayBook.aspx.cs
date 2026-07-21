using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Account
{
    public partial class DayBook : System.Web.UI.Page
    {
        decimal totalDebit = 0;
        decimal totalCredit = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CompanyID"] == null) Response.Redirect("~/Forms/Login.aspx");

                // Default range: Today
                txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd");

                LoadDayBook();
            }
        }

        protected void btnView_Click(object sender, EventArgs e)
        {
            LoadDayBook();
        }

        private void LoadDayBook()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    // This query assumes a unified Voucher or Transaction table. 
                    // If you have separate tables, you would use a UNION ALL query here.
                    string sql = @"SELECT Vch_Date as [Date], Voucher_No as Vch_No, Vch_Type, 
                                   l.ledger_name as Particulars, Debit_Amount as Debit, Credit_Amount as Credit
                                   FROM Voucher_Table v
                                   JOIN Ledger_Table l ON (v.Dr_Ledger_ID = l.Ledger_Sno OR v.Cr_Ledger_ID = l.Ledger_Sno)
                                   WHERE v.Company_No = @cid 
                                   AND v.Vch_Date BETWEEN @from AND @to";

                    if (ddlVchType.SelectedValue != "All")
                        sql += " AND v.Vch_Type = @type";

                    if (!string.IsNullOrEmpty(txtSearch.Text))
                        sql += " AND l.ledger_name LIKE @search";

                    sql += " ORDER BY v.Vch_Date ASC, v.Voucher_Sno ASC";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                    cmd.Parameters.AddWithValue("@from", txtFromDate.Text);
                    cmd.Parameters.AddWithValue("@to", txtToDate.Text);
                    cmd.Parameters.AddWithValue("@type", ddlVchType.SelectedValue);
                    cmd.Parameters.AddWithValue("@search", "%" + txtSearch.Text.Trim() + "%");

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvDayBook.DataSource = dt;
                    gvDayBook.DataBind();
                }
            }
            catch (Exception ex)
            {
                Alert("Error loading report: " + ex.Message, "error");
            }
        }

        protected void gvDayBook_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                totalDebit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Debit"));
                totalCredit += Convert.ToDecimal(DataBinder.Eval(e.Row.DataItem, "Credit"));
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = "TOTAL PERIOD SUM";
                e.Row.Cells[4].Text = totalDebit.ToString("N2");
                e.Row.Cells[5].Text = totalCredit.ToString("N2");

                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            // Implementation for Excel Export could be added here
            Alert("Exporting feature triggered...", "info");
        }

        private void Alert(string msg, string type)
        {
            string clean = msg.Replace("'", "\\'");
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", $"showNotification('{clean}', '{type}');", true);
        }
    }
}
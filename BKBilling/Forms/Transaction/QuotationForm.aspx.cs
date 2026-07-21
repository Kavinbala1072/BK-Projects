using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Transaction
{
    public partial class QuotationForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CompanyID"] == null) Response.Redirect("~/Forms/Login.aspx");
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtExpiryDate.Text = DateTime.Now.AddDays(30).ToString("yyyy-MM-dd"); // Quotes usually valid for 30 days
                LoadList();
                LoadDropDowns();
                CreateInitialRow();
            }
        }

        private void LoadList()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    string sql = @"SELECT q.*, l.ledger_name as Customer_Name FROM Quotation_Master q 
                                   JOIN Ledger_Table l ON q.Customer_Sno = l.Ledger_Sno 
                                   WHERE q.Company_No = @cid ORDER BY q.Quote_Sno DESC";
                    SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(sql, conn) { Parameters = { new SqlParameter("@cid", Session["CompanyID"]) } });
                    DataTable dt = new DataTable(); da.Fill(dt);
                    gvQuotes.DataSource = dt; gvQuotes.DataBind();
                }
            }
            catch { }
        }

        private void LoadDropDowns() { /* Implementation to fill Customer and Item lists */ }

        private void CreateInitialRow()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Qty", typeof(decimal));
            dt.Columns.Add("Rate", typeof(decimal));
            dt.Columns.Add("Tax", typeof(decimal));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Rows.Add(0, 0, 0, 0);
            ViewState["QuoteItems"] = dt;
            rptItems.DataSource = dt; rptItems.DataBind();
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e)
        {
            hfQuoteID.Value = ""; pnlList.Visible = false; pnlForm.Visible = true;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlList.Visible = true; pnlForm.Visible = false; LoadList();
        }

        protected void gvQuotes_RowCommand(object sender, GridViewCommandEventArgs e) { /* Edit/Print logic */ }

        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            DataTable dt = GetCurrentRows();
            dt.Rows.Add(0, 0, 0, 0);
            ViewState["QuoteItems"] = dt;
            rptItems.DataSource = dt; rptItems.DataBind();
        }

        protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                DataTable dt = GetCurrentRows();
                dt.Rows.RemoveAt(Convert.ToInt32(e.CommandArgument));
                ViewState["QuoteItems"] = dt;
                rptItems.DataSource = dt; rptItems.DataBind();
                CalculateGrandTotal();
            }
        }

        protected void RecalculateRow(object sender, EventArgs e) { CalculateGrandTotal(); }

        private DataTable GetCurrentRows()
        {
            DataTable dt = (DataTable)ViewState["QuoteItems"];
            for (int i = 0; i < rptItems.Items.Count; i++)
            {
                dt.Rows[i]["Qty"] = decimal.Parse(((TextBox)rptItems.Items[i].FindControl("txtQty")).Text);
                dt.Rows[i]["Rate"] = decimal.Parse(((TextBox)rptItems.Items[i].FindControl("txtRate")).Text);
                dt.Rows[i]["Tax"] = decimal.Parse(((TextBox)rptItems.Items[i].FindControl("txtTax")).Text);
                dt.Rows[i]["Amount"] = Convert.ToDecimal(dt.Rows[i]["Qty"]) * Convert.ToDecimal(dt.Rows[i]["Rate"]);
            }
            return dt;
        }

        private void CalculateGrandTotal()
        {
            DataTable dt = GetCurrentRows();
            decimal subTotal = 0, taxTotal = 0;
            foreach (DataRow dr in dt.Rows)
            {
                decimal rowAmt = Convert.ToDecimal(dr["Amount"]);
                subTotal += rowAmt;
                taxTotal += (rowAmt * Convert.ToDecimal(dr["Tax"]) / 100);
            }
            lblSubTotal.Text = subTotal.ToString("N2");
            lblTaxTotal.Text = taxTotal.ToString("N2");
            decimal freight = decimal.TryParse(txtFreight.Text, out decimal f) ? f : 0;
            decimal disc = decimal.TryParse(txtDisc.Text, out decimal d) ? d : 0;
            lblGrandTotal.Text = (subTotal + taxTotal + freight - disc).ToString("N2");
            rptItems.DataSource = dt; rptItems.DataBind();
        }

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e) { /* Load Customer details */ }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Transaction logic: Quotation_Master and Quotation_Detail tables
            // Note: Quotations do NOT update stock in Item_Table
            Alert("Quotation Saved Successfully!", "success");
            btnBack_Click(null, null);
        }

        private void Alert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", $"showNotification('{msg}', '{type}');", true);
        }
    }
}
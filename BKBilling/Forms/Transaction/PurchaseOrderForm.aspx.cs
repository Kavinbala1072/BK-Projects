using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Transaction
{
    public partial class PurchaseOrderForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CompanyID"] == null) Response.Redirect("~/Forms/Login.aspx");
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtExpDate.Text = DateTime.Now.AddDays(15).ToString("yyyy-MM-dd");
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
                    string sql = @"SELECT p.*, l.ledger_name as Supplier_Name 
                                   FROM Purchase_Order_Master p 
                                   JOIN Ledger_Table l ON p.Supplier_Sno = l.Ledger_Sno 
                                   WHERE p.Company_No = @cid ORDER BY p.PO_Sno DESC";
                    SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(sql, conn) { Parameters = { new SqlParameter("@cid", Session["CompanyID"]) } });
                    DataTable dt = new DataTable(); da.Fill(dt);
                    gvOrders.DataSource = dt; gvOrders.DataBind();
                }
            }
            catch { }
        }

        private void LoadDropDowns() { /* Fill ddlSupplier and ddlItem */ }

        private void CreateInitialRow()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Qty", typeof(decimal));
            dt.Columns.Add("Rate", typeof(decimal));
            dt.Columns.Add("Tax", typeof(decimal));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Rows.Add(0, 0, 0, 0);
            ViewState["POItems"] = dt;
            rptItems.DataSource = dt; rptItems.DataBind();
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e)
        {
            hfPOID.Value = ""; pnlList.Visible = false; pnlForm.Visible = true;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlList.Visible = true; pnlForm.Visible = false; LoadList();
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e) { /* Edit Logic */ }

        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            DataTable dt = GetCurrentRows();
            dt.Rows.Add(0, 0, 0, 0);
            ViewState["POItems"] = dt;
            rptItems.DataSource = dt; rptItems.DataBind();
        }

        protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                DataTable dt = GetCurrentRows();
                dt.Rows.RemoveAt(Convert.ToInt32(e.CommandArgument));
                ViewState["POItems"] = dt;
                rptItems.DataSource = dt; rptItems.DataBind();
                CalculateGrandTotal();
            }
        }

        protected void RecalculateRow(object sender, EventArgs e) { CalculateGrandTotal(); }

        private DataTable GetCurrentRows()
        {
            DataTable dt = (DataTable)ViewState["POItems"];
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
            lblGrandTotal.Text = (subTotal + taxTotal + freight).ToString("N2");
            rptItems.DataSource = dt; rptItems.DataBind();
        }

        protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e) { /* Detail display logic */ }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Transaction logic: PO Master and PO Details
            Alert("Purchase Order Saved!", "success");
            btnBack_Click(null, null);
        }

        private void Alert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", $"showNotification('{msg}', '{type}');", true);
        }
    }
}
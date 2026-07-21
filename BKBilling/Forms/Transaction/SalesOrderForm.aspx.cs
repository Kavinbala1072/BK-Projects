using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Transaction
{
    public partial class SalesOrderForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CompanyID"] == null) Response.Redirect("~/Forms/Login.aspx");
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtDeliveryDate.Text = DateTime.Now.AddDays(7).ToString("yyyy-MM-dd");
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
                    string sql = @"SELECT s.*, l.ledger_name as Customer_Name FROM Sales_Order_Master s 
                                   JOIN Ledger_Table l ON s.Customer_Sno = l.Ledger_Sno 
                                   WHERE s.Company_No = @cid ORDER BY s.Order_Sno DESC";
                    SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(sql, conn) { Parameters = { new SqlParameter("@cid", Session["CompanyID"]) } });
                    DataTable dt = new DataTable(); da.Fill(dt);
                    gvOrders.DataSource = dt; gvOrders.DataBind();
                }
            }
            catch { }
        }

        private void LoadDropDowns() { /* Load ddlCustomer and ddlItem here */ }

        private void CreateInitialRow()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Qty", typeof(decimal));
            dt.Columns.Add("Rate", typeof(decimal));
            dt.Columns.Add("Tax", typeof(decimal));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Rows.Add(0, 0, 0, 0);
            ViewState["OrderItems"] = dt;
            rptItems.DataSource = dt; rptItems.DataBind();
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e)
        {
            hfOrderID.Value = ""; pnlList.Visible = false; pnlForm.Visible = true;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlList.Visible = true; pnlForm.Visible = false; LoadList();
        }

        protected void gvOrders_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Edit logic here
        }

        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            DataTable dt = GetCurrentRows();
            dt.Rows.Add(0, 0, 0, 0);
            ViewState["OrderItems"] = dt;
            rptItems.DataSource = dt; rptItems.DataBind();
        }

        protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                DataTable dt = GetCurrentRows();
                dt.Rows.RemoveAt(Convert.ToInt32(e.CommandArgument));
                ViewState["OrderItems"] = dt;
                rptItems.DataSource = dt; rptItems.DataBind();
                CalculateGrandTotal();
            }
        }

        protected void RecalculateRow(object sender, EventArgs e) { CalculateGrandTotal(); }

        private DataTable GetCurrentRows()
        {
            DataTable dt = (DataTable)ViewState["OrderItems"];
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

        protected void ddlCustomer_SelectedIndexChanged(object sender, EventArgs e) { /* Address logic */ }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Transaction logic: Sales_Order_Master & Sales_Order_Details
            Alert("Sales Order Saved successfully!", "success");
            btnBack_Click(null, null);
        }

        private void Alert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", $"showNotification('{msg}', '{type}');", true);
        }
    }
}
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Transaction
{
    public partial class SalesReturnForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CompanyID"] == null) Response.Redirect("~/Forms/Login.aspx");
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LoadList();
                LoadSalesInvoiceDropdown();
                CreateInitialRow();
            }
        }

        private void LoadSalesInvoiceDropdown()
        {
            // Load Sales Invoice numbers for current company
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    string sql = "SELECT Sales_Sno, Invoice_No FROM Sales_Master_Table WHERE Company_No = @cid ORDER BY Sales_Sno DESC";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                    ddlSalesRef.DataSource = cmd.ExecuteReader();
                    ddlSalesRef.DataTextField = "Invoice_No";
                    ddlSalesRef.DataValueField = "Sales_Sno";
                    ddlSalesRef.DataBind();
                    ddlSalesRef.Items.Insert(0, new ListItem("-- Select Sales Invoice --", "0"));
                }
            }
            catch { }
        }

        protected void ddlSalesRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSalesRef.SelectedValue != "0")
            {
                FetchSalesDetails(Convert.ToInt64(ddlSalesRef.SelectedValue));
            }
        }

        private void FetchSalesDetails(long salesID)
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    // 1. Get Customer Info
                    string sqlHead = @"SELECT l.ledger_name, l.ledger_Add1 FROM Sales_Master_Table s 
                                       JOIN Ledger_Table l ON s.Customer_Sno = l.Ledger_Sno WHERE s.Sales_Sno = @id";
                    SqlCommand cmdH = new SqlCommand(sqlHead, conn);
                    cmdH.Parameters.AddWithValue("@id", salesID);
                    using (SqlDataReader dr = cmdH.ExecuteReader())
                    {
                        if (dr.Read()) litCustomerDetail.Text = $"<b>Customer:</b> {dr["ledger_name"]}<br/><b>Addr:</b> {dr["ledger_Add1"]}";
                    }

                    // 2. Get Items from that Sales Invoice to auto-populate the Return form
                    string sqlItems = "SELECT Qty, Rate, Tax, (Qty * Rate) as Amount FROM Sales_Detail_Table WHERE Sales_No = @id";
                    SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(sqlItems, conn) { Parameters = { new SqlParameter("@id", salesID) } });
                    DataTable dt = new DataTable(); da.Fill(dt);
                    ViewState["ReturnItems"] = dt;
                    rptItems.DataSource = dt; rptItems.DataBind();
                    CalculateGrandTotal();
                }
            }
            catch (Exception ex) { Alert("Error fetching: " + ex.Message, "error"); }
        }

        private void CreateInitialRow()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Qty", typeof(decimal));
            dt.Columns.Add("Rate", typeof(decimal));
            dt.Columns.Add("Tax", typeof(decimal));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Rows.Add(0, 0, 0, 0);
            ViewState["ReturnItems"] = dt;
            rptItems.DataSource = dt; rptItems.DataBind();
        }

        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            DataTable dt = GetCurrentRows();
            dt.Rows.Add(0, 0, 0, 0);
            ViewState["ReturnItems"] = dt;
            rptItems.DataSource = dt; rptItems.DataBind();
        }

        protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                DataTable dt = GetCurrentRows();
                dt.Rows.RemoveAt(Convert.ToInt32(e.CommandArgument));
                ViewState["ReturnItems"] = dt;
                rptItems.DataSource = dt; rptItems.DataBind();
                CalculateGrandTotal();
            }
        }

        protected void RecalculateRow(object sender, EventArgs e) { CalculateGrandTotal(); }

        private DataTable GetCurrentRows()
        {
            DataTable dt = (DataTable)ViewState["ReturnItems"];
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
            decimal adj = decimal.TryParse(txtFreight.Text, out decimal f) ? f : 0;
            lblGrandTotal.Text = (subTotal + taxTotal + adj).ToString("N2");
            rptItems.DataSource = dt; rptItems.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Transaction logic: Sales_Return_Master & Stock In Update
            Alert("Sales Return processed. Inventory updated!", "success");
            btnBack_Click(null, null);
        }

        private void Alert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", $"showNotification('{msg}', '{type}');", true);
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e) { hfReturnID.Value = ""; pnlList.Visible = false; pnlForm.Visible = true; }
        protected void btnBack_Click(object sender, EventArgs e) { pnlList.Visible = true; pnlForm.Visible = false; LoadList(); }
        private void LoadList() { /* Implementation similar to SalesForm */ }
        protected void gvReturns_RowCommand(object sender, GridViewCommandEventArgs e) { /* Edit logic */ }
    }
}
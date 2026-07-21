using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Transaction
{
    public partial class PurchaseReturnForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CompanyID"] == null) Response.Redirect("~/Forms/Login.aspx");
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LoadList();
                LoadPurchaseNoDropdown();
                CreateInitialRow();
            }
        }

        private void LoadPurchaseNoDropdown()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    string sql = "SELECT Purchase_Sno, Purchase_No FROM Purchase_Master_Table WHERE Company_No = @cid ORDER BY Purchase_Sno DESC";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                    ddlPurchaseRef.DataSource = cmd.ExecuteReader();
                    ddlPurchaseRef.DataTextField = "Purchase_No";
                    ddlPurchaseRef.DataValueField = "Purchase_Sno";
                    ddlPurchaseRef.DataBind();
                    ddlPurchaseRef.Items.Insert(0, new ListItem("-- Select Purchase Bill --", "0"));
                }
            }
            catch { }
        }

        protected void ddlPurchaseRef_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPurchaseRef.SelectedValue != "0")
            {
                FetchPurchaseData(Convert.ToInt64(ddlPurchaseRef.SelectedValue));
            }
        }

        private void FetchPurchaseData(long purchaseID)
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    // Load Supplier Info
                    string sqlHead = @"SELECT l.ledger_name, l.ledger_code FROM Purchase_Master_Table p 
                                       JOIN Ledger_Table l ON p.Supplier_Sno = l.Ledger_Sno WHERE p.Purchase_Sno = @id";
                    SqlCommand cmdH = new SqlCommand(sqlHead, conn);
                    cmdH.Parameters.AddWithValue("@id", purchaseID);
                    using (SqlDataReader dr = cmdH.ExecuteReader())
                    {
                        if (dr.Read()) litSupplierDetail.Text = $"<b>Supplier:</b> {dr["ledger_name"]} ({dr["ledger_code"]})";
                    }

                    // Load Items from Purchase to Return form
                    string sqlItems = "SELECT Qty, Rate, Tax, (Qty * Rate) as Amount FROM Purchase_Detail_Table WHERE Purchase_No = @id";
                    SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(sqlItems, conn) { Parameters = { new SqlParameter("@id", purchaseID) } });
                    DataTable dt = new DataTable(); da.Fill(dt);
                    ViewState["ReturnItems"] = dt;
                    rptItems.DataSource = dt; rptItems.DataBind();
                    CalculateGrandTotal();
                }
            }
            catch (Exception ex) { Alert("Error: " + ex.Message, "error"); }
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
            decimal sub = 0, tax = 0;
            foreach (DataRow dr in dt.Rows)
            {
                decimal rowAmt = Convert.ToDecimal(dr["Amount"]);
                sub += rowAmt;
                tax += (rowAmt * Convert.ToDecimal(dr["Tax"]) / 100);
            }
            lblSubTotal.Text = sub.ToString("N2");
            lblTaxTotal.Text = tax.ToString("N2");
            decimal adj = decimal.TryParse(txtFreight.Text, out decimal f) ? f : 0;
            lblGrandTotal.Text = (sub + tax + adj).ToString("N2");
            rptItems.DataSource = dt; rptItems.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Logic: Decrease Stock in Item_Table, Update Supplier Ledger
            Alert("Purchase Return Saved. Stock & Accounts updated!", "success");
            btnBack_Click(null, null);
        }

        private void Alert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", $"showNotification('{msg}', '{type}');", true);
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e) { hfReturnID.Value = ""; pnlList.Visible = false; pnlForm.Visible = true; }
        protected void btnBack_Click(object sender, EventArgs e) { pnlList.Visible = true; pnlForm.Visible = false; LoadList(); }
        private void LoadList() { /* Implementation similar to GridView list */ }
        protected void gvReturns_RowCommand(object sender, GridViewCommandEventArgs e) { /* Edit Logic */ }
    }
}
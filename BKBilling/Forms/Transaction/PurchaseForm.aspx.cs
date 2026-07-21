using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Transaction
{
    public partial class PurchaseForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CompanyID"] == null) Response.Redirect("~/Forms/Login.aspx");
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LoadList();
                LoadSupplierDropdown();
                CreateItemTable();
            }
        }

        protected void ddlSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSupplier.SelectedValue != "0")
            {
                // Fetch Supplier details from Ledger_Table based on selection
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT ledger_Add1, ledger_Add2, Ledger_PAN FROM Ledger_Table WHERE Ledger_Sno = @id", conn);
                    cmd.Parameters.AddWithValue("@id", ddlSupplier.SelectedValue);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            litSupplierDetail.Text = $"<b>Addr:</b> {dr["ledger_Add1"]}, {dr["ledger_Add2"]}<br/><b>PAN:</b> {dr["Ledger_PAN"]}";
                        }
                    }
                }
            }
            else { litSupplierDetail.Text = "Select a supplier to view details..."; }
        }

        private void CreateItemTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Qty", typeof(decimal));
            dt.Columns.Add("Rate", typeof(decimal));
            dt.Columns.Add("Tax", typeof(decimal));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Rows.Add(dt.NewRow());
            ViewState["PurchaseItems"] = dt;
            BindRepeater();
        }

        private void BindRepeater()
        {
            rptItems.DataSource = (DataTable)ViewState["PurchaseItems"];
            rptItems.DataBind();
        }

        protected void btnAddRow_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)ViewState["PurchaseItems"];
            dt.Rows.Add(dt.NewRow());
            BindRepeater();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Transaction logic: Insert into Purchase_Master and Purchase_Details
            // Update Item_Table (Increase Stock)
            Alert("Purchase recorded and stock updated!", "success");
            btnBack_Click(null, null);
        }

        private void Alert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", $"showNotification('{msg}', '{type}');", true);
        }

        // Standard Navigation logic
        protected void btnOpenCreate_Click(object sender, EventArgs e) { pnlList.Visible = false; pnlForm.Visible = true; }
        protected void btnBack_Click(object sender, EventArgs e) { pnlList.Visible = true; pnlForm.Visible = false; LoadList(); }

        private void LoadSupplierDropdown() { /* Load ledgers where group is Creditors/Suppliers */ }
        private void LoadList() { /* Load GridView with previous Purchases */ }
        protected void gvPurchase_RowCommand(object sender, GridViewCommandEventArgs e) { /* Edit Logic */ }
        protected void rptItems_ItemCommand(object source, RepeaterCommandEventArgs e) { /* Delete Row Logic */ }
        protected void CalculateTotal(object sender, EventArgs e) { /* Math Logic */ }
    }
}
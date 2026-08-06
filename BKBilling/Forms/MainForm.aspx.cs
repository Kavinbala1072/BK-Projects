using System;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using BKBilling.Class;

namespace BKBilling.Forms
{
    public partial class MainForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserName"] == null || Session["CompanyID"] == null) { Response.Redirect("Login.aspx"); return; }
            if (!IsPostBack) {
                litUsername.Text = Session["UserName"].ToString();
                //litUserEmail.Text = Session["UserName"].ToString();
                litWelcomeUser.Text = Session["UserName"].ToString();
                LoadCompanyName();
                ApplyFormSettings();
            }
        }

        private void ApplyFormSettings() {
            using (SqlConnection conn = DbHelper.GetConnection()) 
            {
                string sql = "SELECT Control_ID FROM Form_Settings WHERE Company_ID=@cid AND Is_Enabled=0";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read()) 
                {
                    Control c = FindControlRecursive(this, dr["Control_ID"].ToString());
                    if (c != null) c.Visible = false;
                }
            }
        }

        private Control FindControlRecursive(Control root, string id) {
            if (root.ID == id) return root;
            foreach (Control child in root.Controls) {
                Control found = FindControlRecursive(child, id);
                if (found != null) return found;
            }
            return null;
        }

        private void LoadPage(string url) {
            pnlWelcome.Style["display"] = "none";
            ifrReport.Style["display"] = "block";
            ifrReport.Attributes["src"] = url;
        }

        private void LoadCompanyName() {
            using (SqlConnection conn = DbHelper.GetConnection()) {
                SqlCommand cmd = new SqlCommand("SELECT Company_Name FROM Company_Table WHERE Company_Sno=@s", conn);
                cmd.Parameters.AddWithValue("@s", Session["CompanyID"]);
                litCompName.Text = cmd.ExecuteScalar()?.ToString() ?? "BK SOFTWARES";
            }
        }
        private void ShowWelcome()
        {
            pnlWelcome.Style["display"] = "flex";
            ifrReport.Style["display"] = "none";
            ifrReport.Attributes["src"] = "";
        }

        protected void btnMenuDash_Click(object sender, EventArgs e) { ShowWelcome(); }
        protected void btnMyProfile_Click(object sender, EventArgs e) => LoadPage("Master/UserMaster.aspx?mode=profile");
        
        // --- TRANSACTIONS ---
        protected void btnSales_Click(object sender, EventArgs e) => LoadPage("Transaction/SalesForm.aspx");
        protected void btnSalesOrder_Click(object sender, EventArgs e) => LoadPage("Transaction/SalesOrderForm.aspx");
        protected void btnSalesReturn_Click(object sender, EventArgs e) => LoadPage("Transaction/SalesReturnForm.aspx");
        protected void btnQuotation_Click(object sender, EventArgs e) => LoadPage("Transaction/QuotationForm.aspx");
        protected void btnPurchase_Click(object sender, EventArgs e) => LoadPage("Transaction/PurchaseForm.aspx");
        protected void btnPurchaseOrder_Click(object sender, EventArgs e) => LoadPage("Transaction/PurchaseOrderForm.aspx");
        protected void btnPurchaseReturn_Click(object sender, EventArgs e) => LoadPage("Transaction/PurchaseReturnForm.aspx");
        protected void btnMenuVoucher_Click(object sender, EventArgs e) => LoadPage("Transaction/VouchersForm.aspx");
        protected void btnAdjustment_Click(object sender, EventArgs e) => LoadPage("Transaction/AdjustmentForm.aspx");
        protected void btnBTransfer_Click(object sender, EventArgs e) => LoadPage("Transaction/BTransferForm.aspx");

        // --- MASTERS ---
        protected void btnLedgerCreation_Click(object sender, EventArgs e) => LoadPage("Master/LedgerMaster.aspx");
        protected void btnGroupMaster_Click(object sender, EventArgs e) => LoadPage("Master/LedgerGroup.aspx");
        protected void btnCustomerCreation_Click(object sender, EventArgs e) => LoadPage("Master/CustomerMaster.aspx");
        protected void btnSupplierCreation_Click(object sender, EventArgs e) => LoadPage("Master/SupplierMaster.aspx");
        protected void btnAreaMaster_Click(object sender, EventArgs e) => LoadPage("Master/AreaMaster.aspx");
        protected void btnVoucher_Click(object sender, EventArgs e) => LoadPage("Master/VoucherTypeMaster.aspx");
        protected void btnJobWorkCreation_Click(object sender, EventArgs e) => LoadPage("Master/JobWorkMaster.aspx");        
        protected void btnItemCreation_Click(object sender, EventArgs e) => LoadPage("Inventory/ItemMaster.aspx");
        protected void btnCategoryMaster_Click(object sender, EventArgs e) => LoadPage("Inventory/CategoryMaster.aspx");
        protected void btnSubCategoryMaster_Click(object sender, EventArgs e) => LoadPage("Inventory/SubCategoryMaster.aspx");
        protected void btnColorMaster_Click(object sender, EventArgs e) => LoadPage("Inventory/ColorMaster.aspx");
        protected void btnUnitConvMaster_Click(object sender, EventArgs e) => LoadPage("Inventory/IUnitConvMaster.aspx");
        protected void btnGSTCreation_Click(object sender, EventArgs e) => LoadPage("Inventory/GSTMaster.aspx");
        protected void btnBarcode_Click(object sender, EventArgs e) => LoadPage("Inventory/Barcode.aspx");        
        protected void btnWeaveType_Click(object sender, EventArgs e) => LoadPage("Inventory/WeaveTypeMaster.aspx");
        protected void btnUnitMaster_Click(object sender, EventArgs e) => LoadPage("Inventory/UnitMaster.aspx");
        protected void btnGodownMaster_Click(object sender, EventArgs e) => LoadPage("Inventory/GodownMaster.aspx");
        protected void btnCmpyCreation_Click(object sender, EventArgs e) => LoadPage("Master/Companies.aspx");
        protected void btnUserMaster_Click(object sender, EventArgs e) => LoadPage("Master/UserMaster.aspx");
        protected void btnFinancialYearMasterMaster_Click(object sender, EventArgs e) => LoadPage("Master/FinancialYearMaster.aspx");
        protected void btnBackupForm_Click(object sender, EventArgs e) => LoadPage("Master/BackupForm.aspx");
        // --- ACCOUNTS ---
        protected void btnDayBook_Click(object sender, EventArgs e) => LoadPage("Account/DayBook.aspx");
        protected void btnLedger_Click(object sender, EventArgs e) => LoadPage("Account/LedgerView.aspx");
        protected void btnCashBank_Click(object sender, EventArgs e) => LoadPage("Account/CashBankBook.aspx");
        protected void btnTrialBalance_Click(object sender, EventArgs e) => LoadPage("Account/TrialBalance.aspx");
        protected void btnBalanceSheet_Click(object sender, EventArgs e) => LoadPage("Account/BalanceSheet.aspx");
        protected void btnProfitLoss_Click(object sender, EventArgs e) => LoadPage("Account/ProfitLoss.aspx");

        // --- REPORTS ---
        protected void btnStockSummary_Click(object sender, EventArgs e) => LoadPage("Inventory/StockSummary.aspx");
        protected void btnStockDetail_Click(object sender, EventArgs e) => LoadPage("Inventory/StockDetail.aspx");
        protected void btnGSTR1_Click(object sender, EventArgs e) => LoadPage("Account/GSTR1.aspx");
        protected void btnGSTR2_Click(object sender, EventArgs e) => LoadPage("Account/GSTR2.aspx");
        protected void btnGSTR3B_Click(object sender, EventArgs e) => LoadPage("Account/GSTR3B.aspx");
        protected void btnHSNSummary_Click(object sender, EventArgs e) => LoadPage("Account/HSNSummary.aspx");

        protected void btnCompSett_Click(object sender, EventArgs e) => LoadPage("Settings/CompSetting.aspx");
        protected void btnitemSett_Click(object sender, EventArgs e) => LoadPage("Settings/ItemSetting.aspx");
        protected void btnFormSet_Click(object sender, EventArgs e) => LoadPage("Settings/FormSetting.aspx");
        protected void btnActivity_Click(object sender, EventArgs e) => LoadPage("ActivityForm.aspx");

        protected void btnLogout_Click(object sender, EventArgs e) { Session.Clear(); Session.Abandon(); Response.Redirect("Login.aspx"); }
    }
}
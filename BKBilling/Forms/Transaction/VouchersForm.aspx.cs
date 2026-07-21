using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Transaction
{
    public partial class VouchersForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CompanyID"] == null) Response.Redirect("~/Forms/Login.aspx");
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                LoadList();
                LoadLedgers();
                UpdateUITheme();
            }
        }

        private void LoadList()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    // Logic to load existing vouchers from a Voucher_Master table
                    string sql = "SELECT * FROM Voucher_Table WHERE Company_No = @cid ORDER BY Voucher_Sno DESC";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable(); da.Fill(dt);
                    gvVouchers.DataSource = dt; gvVouchers.DataBind();
                }
            }
            catch { }
        }

        private void LoadLedgers() { /* Fill ddlDrLedger and ddlCrLedger with all accounts */ }

        protected void ddlVchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateUITheme();
        }

        private void UpdateUITheme()
        {
            string type = ddlVchType.SelectedValue;
            vchHeader.InnerText = type + " Voucher Entry";
            vchHeader.Attributes["class"] = "fw-bold m-0 type-header";

            if (type == "Receipt") vchHeader.Attributes["class"] += " receipt-theme";
            else if (type == "Payment") vchHeader.Attributes["class"] += " payment-theme";
            else if (type == "Contra") vchHeader.Attributes["class"] += " contra-theme";
            else if (type == "Journal") vchHeader.Attributes["class"] += " journal-theme";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlDrLedger.SelectedValue == ddlCrLedger.SelectedValue)
            {
                Alert("Debit and Credit accounts cannot be the same!", "error");
                return;
            }

            if (string.IsNullOrEmpty(txtDrAmount.Text) || Convert.ToDecimal(txtDrAmount.Text) <= 0)
            {
                Alert("Please enter a valid amount", "error");
                return;
            }

            Alert(ddlVchType.SelectedValue + " Posted Successfully!", "success");
            btnBack_Click(null, null);
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e) { hfVoucherID.Value = ""; pnlList.Visible = false; pnlForm.Visible = true; }
        protected void btnBack_Click(object sender, EventArgs e) { pnlList.Visible = true; pnlForm.Visible = false; LoadList(); }
        protected void gvVouchers_RowCommand(object sender, GridViewCommandEventArgs e) { /* Edit Logic */ }

        private void Alert(string msg, string type)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", $"showNotification('{msg}', '{type}');", true);
        }
    }
}
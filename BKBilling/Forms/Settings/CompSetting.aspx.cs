using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Settings
{
    public partial class CompSetting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");

            if (!IsPostBack)
            {
                LoadLedgers();
                LoadAllSettings();
            }
        }

        private void LoadLedgers()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string sql = "SELECT Ledger_Sno, ledger_name FROM Ledger_Table WHERE Company_No = @cid AND IsActive = 1 ORDER BY ledger_name";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    ddlTCSLedger.DataSource = dt; ddlTCSLedger.DataTextField = "ledger_name"; ddlTCSLedger.DataValueField = "Ledger_Sno"; ddlTCSLedger.DataBind();
                    ddlTDSLedger.DataSource = dt; ddlTDSLedger.DataTextField = "ledger_name"; ddlTDSLedger.DataValueField = "Ledger_Sno"; ddlTDSLedger.DataBind();

                    ddlTCSLedger.Items.Insert(0, new ListItem("-- Select --", "0"));
                    ddlTDSLedger.Items.Insert(0, new ListItem("-- Select --", "0"));
                }
            }
            catch (Exception ex) { Alert("Ledger Load Error: " + ex.Message, "error"); }
        }

        private void LoadAllSettings()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    string sql = "SELECT Ctl_MtDesc, Ctl_Value FROM Control_Table WHERE Company_No = @cid";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        string key = dr["Ctl_MtDesc"].ToString();
                        string val = dr["Ctl_Value"].ToString();

                        // TAB 1
                        if (key == "GEN_EMAIL") setEmail.Text = val;
                        if (key == "GEN_EMAIL_PASS") setEmailPass.Attributes["value"] = val;
                        if (key == "GEN_CURRENCY") setCurrencyFmt.Text = val;
                        if (key == "GEN_LANG") setLanguage.Text = val;
                        if (key == "BANK_NAME") setBankName.Text = val;
                        if (key == "BANK_ACNO") setBankAcNo.Text = val;
                        if (key == "BANK_IFSC") setBankIfsc.Text = val;
                        if (key == "BANK_BRANCH") setBankBranch.Text = val;

                        // TAB 2
                        if (key == "GST_TIN") gstTin.Text = val;
                        if (key == "GST_DEALER") gstDealerType.SelectedValue = val;
                        if (key == "GST_ONWARDS") gstOnwards.Text = val;
                        if (key == "GST_STATE") gstState.Text = val;
                        if (key == "GST_MOBILE") gstMobile.Text = val;
                        if (key == "GST_PIN") gstPincode.Text = val;
                        if (key == "GST_CASH_LIMIT") gstCashLimit.Text = val;
                        if (key == "GST_API_USER") gstUser.Text = val;
                        if (key == "GST_API_PASS") gstPass.Attributes["value"] = val;

                        if (key == "TCS_ENABLED") tcsEnabled.SelectedValue = val;
                        if (key == "TCS_LEDGER") ddlTCSLedger.SelectedValue = val;
                        if (key == "TCS_LIMIT") txtTcsLimit.Text = val;
                        if (key == "TCS_PAN_PERC") txtTcsPan.Text = val;
                        if (key == "TCS_NOPAN_PERC") txtTcsNoPan.Text = val;

                        if (key == "TDS_ENABLED") tdsEnabled.SelectedValue = val;
                        if (key == "TDS_TAN") tdsTan.Text = val;
                        if (key == "TDS_LEDGER") ddlTDSLedger.SelectedValue = val;

                        // TAB 3
                        if (key == "PRINT_TERMS") setTerms.Text = val;
                        if (key == "PRINT_FOOTER") setFooter.Text = val;
                    }
                }
            }
            catch (Exception ex) { Alert("Load Error: " + ex.Message, "error"); }
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                long cid = Convert.ToInt64(Session["CompanyID"]);

                // TAB 1
                Save(cid, "GEN_EMAIL", setEmail.Text);
                Save(cid, "GEN_EMAIL_PASS", setEmailPass.Text);
                Save(cid, "GEN_CURRENCY", setCurrencyFmt.Text);
                Save(cid, "GEN_LANG", setLanguage.Text);
                Save(cid, "BANK_NAME", setBankName.Text);
                Save(cid, "BANK_ACNO", setBankAcNo.Text);
                Save(cid, "BANK_IFSC", setBankIfsc.Text);
                Save(cid, "BANK_BRANCH", setBankBranch.Text);

                // TAB 2
                Save(cid, "GST_TIN", gstTin.Text.ToUpper());
                Save(cid, "GST_DEALER", gstDealerType.SelectedValue);
                Save(cid, "GST_ONWARDS", gstOnwards.Text);
                Save(cid, "GST_STATE", gstState.Text);
                Save(cid, "GST_MOBILE", gstMobile.Text);
                Save(cid, "GST_PIN", gstPincode.Text);
                Save(cid, "GST_CASH_LIMIT", gstCashLimit.Text);
                Save(cid, "GST_API_USER", gstUser.Text);
                Save(cid, "GST_API_PASS", gstPass.Text);

                Save(cid, "TCS_ENABLED", tcsEnabled.SelectedValue);
                Save(cid, "TCS_LEDGER", ddlTCSLedger.SelectedValue);
                Save(cid, "TCS_LIMIT", txtTcsLimit.Text);
                Save(cid, "TCS_PAN_PERC", txtTcsPan.Text);
                Save(cid, "TCS_NOPAN_PERC", txtTcsNoPan.Text);

                Save(cid, "TDS_ENABLED", tdsEnabled.SelectedValue);
                Save(cid, "TDS_TAN", tdsTan.Text.ToUpper());
                Save(cid, "TDS_LEDGER", ddlTDSLedger.SelectedValue);

                // TAB 3
                Save(cid, "PRINT_TERMS", setTerms.Text);
                Save(cid, "PRINT_FOOTER", setFooter.Text);

                Alert("All Company configurations updated successfully!", "success");
            }
            catch (Exception ex) { Alert("Save Error: " + ex.Message, "error"); }
        }

        private void Save(long cid, string key, string val)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_SetControlValue", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_No", cid);
                    cmd.Parameters.AddWithValue("@MtDesc", key);
                    cmd.Parameters.AddWithValue("@Value", val ?? "");
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected void SwitchTab(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            tab1.CssClass = "nav-link"; tab2.CssClass = "nav-link"; tab3.CssClass = "nav-link";
            btn.CssClass = "nav-link active";
            if (btn.ID == "tab1") mvSettings.ActiveViewIndex = 0;
            else if (btn.ID == "tab2") mvSettings.ActiveViewIndex = 1;
            else if (btn.ID == "tab3") mvSettings.ActiveViewIndex = 2;
        }

        private void Alert(string msg, string type)
        {
            string clean = msg.Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
            string script = $"showNotification('{clean}', '{type}');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertMessage", script, true);
        }
    }
}
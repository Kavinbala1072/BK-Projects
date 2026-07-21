using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class LedgerMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack) { LoadList(); }
        }

        private void DebugLog(string spName)
        {
            string script = $"console.info('SQL EXECUTION: {spName}');";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "debug_" + Guid.NewGuid(), script, true);
        }

        private void LoadDropdowns()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                long cid = Convert.ToInt64(Session["CompanyID"]);

                // Groups
                SqlDataAdapter daG = new SqlDataAdapter("SELECT LedgerGroup_Sno, LedgerGroup_Name FROM LedgerGroup_Table WHERE Company_No = @cid AND IsActive=1", conn);
                daG.SelectCommand.Parameters.AddWithValue("@cid", cid);
                DataTable dtG = new DataTable(); daG.Fill(dtG);
                ddlGroup.DataSource = dtG; ddlGroup.DataTextField = "LedgerGroup_Name"; ddlGroup.DataValueField = "LedgerGroup_Sno"; ddlGroup.DataBind();
                ddlGroup.Items.Insert(0, new ListItem("-- Select Group --", "0"));

                // Areas (Existing SP)
                DebugLog("sp_GetAreaDropdown");
                using (SqlCommand cmdA = new SqlCommand("sp_GetAreaDropdown", conn))
                {
                    cmdA.CommandType = CommandType.StoredProcedure;
                    cmdA.Parameters.AddWithValue("@Company_No", cid);
                    SqlDataAdapter daA = new SqlDataAdapter(cmdA);
                    DataTable dtA = new DataTable(); daA.Fill(dtA);
                    ddlArea.DataSource = dtA; ddlArea.DataTextField = "Area_Name"; ddlArea.DataValueField = "Area_Sno"; ddlArea.DataBind();
                    ddlArea.Items.Insert(0, new ListItem("-- Select Area --", "0"));
                }

                // States
                ddlGSTState.Items.Clear();
                ddlGSTState.Items.Add(new ListItem("Tamil Nadu (33)", "33"));
                ddlGSTState.Items.Add(new ListItem("Maharashtra (27)", "27"));
                ddlGSTState.Items.Add(new ListItem("Karnataka (29)", "29"));
                ddlGSTState.Items.Add(new ListItem("Delhi (07)", "07"));
                ddlGSTState.Items.Insert(0, new ListItem("-- Select State --", "0"));
            }
        }

        private void LoadList()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_GetLedgerList");
                    using (SqlCommand cmd = new SqlCommand("sp_GetLedgerList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@SearchText", string.IsNullOrEmpty(txtSearch.Text) ? (object)DBNull.Value : txtSearch.Text.Trim());

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable(); da.Fill(dt);
                        gvLedgers.DataSource = dt; gvLedgers.DataBind();
                    }
                }
            }
            catch (Exception ex) { Alert("Load Error: " + ex.Message, "error"); }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLedgerName.Text)) { Alert("Name is required", "error"); return; }
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    DebugLog("sp_SaveLedger");
                    using (SqlCommand cmd = new SqlCommand("sp_SaveLedger", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfLedgerID.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@Ledger_Sno", isUpdate ? Convert.ToInt64(hfLedgerID.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ledger_Active", ddlActive.SelectedValue == "1");
                        cmd.Parameters.AddWithValue("@ledger_code", txtLedgerCode.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@ledger_name", txtLedgerName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@LedgerGroup_no", ddlGroup.SelectedValue);
                        cmd.Parameters.AddWithValue("@Area_no", ddlArea.SelectedValue);
                        cmd.Parameters.AddWithValue("@ledger_Add1", txtAdd1.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Add2", txtAdd2.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Add3", txtAdd3.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_ContactPerson", txtContactPerson.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_bank", txtBank.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_Branch", txtBranch.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_AcNo", txtAcNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Ifscode", txtIfsc.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_PAN", txtPAN.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@Ledger_open", Convert.ToDecimal(txtOpening.Text));
                        cmd.Parameters.AddWithValue("@Balance_Type", ddlBalType.SelectedValue);
                        cmd.Parameters.AddWithValue("@Credit_Limit", Convert.ToDecimal(string.IsNullOrEmpty(txtCreditLimit.Text) ? "0" : txtCreditLimit.Text));
                        cmd.Parameters.AddWithValue("@Credit_Days", Convert.ToInt32(string.IsNullOrEmpty(txtCreditDays.Text) ? "0" : txtCreditDays.Text));
                        cmd.Parameters.AddWithValue("@Is_TDS_Applicable", chkTDS.Checked);
                        cmd.Parameters.AddWithValue("@Use_GST", chkUseGST.Checked);
                        cmd.Parameters.AddWithValue("@Ledger_GST", txtGSTIN.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@GST_DealerType", ddlDealerType.SelectedValue);
                        cmd.Parameters.AddWithValue("@GST_StateCode", ddlGSTState.SelectedValue);
                        cmd.Parameters.AddWithValue("@Ledger_remarks", txtRemarks.Text.Trim());

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                btnBack_Click(null, null);
                Alert("Ledger registration successful!", "success");
            }
            catch (SqlException ex) { Alert(ex.Message, "error"); }
            catch (Exception ex) { Alert("Save Error: " + ex.Message, "error"); }
        }

        protected void gvLedgers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord")
            {
                try
                {
                    using (SqlConnection conn = DbHelper.GetConnection())
                    {
                        DebugLog("sp_GetLedgerByID");
                        using (SqlCommand cmd = new SqlCommand("sp_GetLedgerByID", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Ledger_Sno", e.CommandArgument);
                            if (conn.State == ConnectionState.Closed) conn.Open();
                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    hfLedgerID.Value = dr["Ledger_Sno"].ToString();
                                    LoadDropdowns();
                                    txtLedgerName.Text = dr["ledger_name"].ToString();
                                    txtLedgerCode.Text = dr["ledger_code"].ToString();
                                    ddlActive.SelectedValue = Convert.ToBoolean(dr["ledger_Active"]) ? "1" : "0";
                                    ddlGroup.SelectedValue = dr["LedgerGroup_no"].ToString();
                                    ddlArea.SelectedValue = dr["Area_no"].ToString();
                                    txtAdd1.Text = dr["ledger_Add1"].ToString();
                                    txtAdd2.Text = dr["ledger_Add2"].ToString();
                                    txtAdd3.Text = dr["ledger_Add3"].ToString();
                                    txtEmail.Text = dr["Ledger_Email"].ToString();
                                    txtPhone.Text = dr["Ledger_Phone"].ToString();
                                    txtContactPerson.Text = dr["Ledger_ContactPerson"].ToString();
                                    txtOpening.Text = dr["Ledger_open"].ToString();
                                    ddlBalType.SelectedValue = dr["Balance_Type"].ToString();
                                    txtCreditLimit.Text = dr["Credit_Limit"].ToString();
                                    txtCreditDays.Text = dr["Credit_Days"].ToString();
                                    txtRemarks.Text = dr["Ledger_remarks"].ToString();
                                    txtBank.Text = dr["ledger_bank"].ToString();
                                    txtBranch.Text = dr["Ledger_Branch"].ToString();
                                    txtAcNo.Text = dr["ledger_AcNo"].ToString();
                                    txtIfsc.Text = dr["ledger_Ifscode"].ToString();
                                    txtPAN.Text = dr["Ledger_PAN"].ToString();
                                    chkTDS.Checked = Convert.ToBoolean(dr["Is_TDS_Applicable"]);
                                    chkUseGST.Checked = Convert.ToBoolean(dr["Use_GST"]);
                                    txtGSTIN.Text = dr["Ledger_GST"].ToString();
                                    ddlDealerType.SelectedValue = dr["GST_DealerType"].ToString();
                                    ddlGSTState.SelectedValue = dr["GST_StateCode"].ToString();

                                    pnlList.Visible = false; pnlForm.Visible = true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { Alert(ex.Message, "error"); }
            }
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e) { hfLedgerID.Value = ""; ClearInputs(); LoadDropdowns(); pnlList.Visible = false; pnlForm.Visible = true; }
        protected void btnBack_Click(object sender, EventArgs e) { pnlList.Visible = true; pnlForm.Visible = false; LoadList(); }
        protected void txtSearch_TextChanged(object sender, EventArgs e) => LoadList();

        private void Alert(string msg, string type)
        {
            string clean = msg.Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
            string script = $"showNotification('{clean}', '{type}');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertMessage", script, true);
        }

        private void ClearInputs()
        {
            txtLedgerName.Text = txtLedgerCode.Text = txtAdd1.Text = txtAdd2.Text = txtAdd3.Text = "";
            txtEmail.Text = txtPhone.Text = txtContactPerson.Text = txtBank.Text = txtBranch.Text = "";
            txtAcNo.Text = txtIfsc.Text = txtPAN.Text = txtOpening.Text = "0.00"; txtRemarks.Text = "";
            txtCreditLimit.Text = "0"; txtCreditDays.Text = "0"; chkUseGST.Checked = true;
        }
    }
}
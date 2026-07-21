using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;
using BKBilling.Components;

namespace BKBilling.Forms
{
    public partial class ActivityForm : System.Web.UI.Page
    {
        #region Page Init - Grid Configuration

        protected void Page_Init(object sender, EventArgs e)
        {
            var columns = new List<GridColumnDef> {
        new GridColumnDef { FieldKey = "ledger_code", HeaderText = "CODE", Width = "80px" },
        new GridColumnDef { FieldKey = "ledger_name", HeaderText = "LEDGER NAME", Bold = true },
        new GridColumnDef { FieldKey = "ledger_bank", HeaderText = "BANK" },
        new GridColumnDef { FieldKey = "Ledger_open", HeaderText = "OPEN BAL", Format = "{0:N2}", Width = "100px" }
    };

            var actions = new List<GridActionDef> {
        new GridActionDef { Key = "EditLedger", Icon = "far fa-edit text-primary", Tooltip = "Edit" }
    };

            gridLedgers.Configure(columns, actions, "Ledger_Sno");
            gridLedgers.DateColumn = "CreatedDate"; // Mapping for the Toolbar Date Range

            gridLedgers.OnRebind += (s, ev) => LoadLedgerGrid();
            gridLedgers.RowAction += (s, ev) => {
                if (ev.ActionKey == "EditLedger") LoadLedgerForEdit(ev.RowKey);
            };
        }

        #endregion

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadDropdowns();
                LoadLedgerGrid();
                pnlList.Visible = true;
                pnlForm.Visible = false;
            }
        }

        #endregion

        #region Load Data

        private void LoadLedgerGrid()
        {
            try
            {
                using (SqlConnection con = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetLedgerList", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Convert.ToInt64(Session["CompanyID"]));

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gridLedgers.BindData(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Alert(ex.Message, "error");
            }
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e)
        {
            ClearInputs();
            pnlList.Visible = false;
            pnlForm.Visible = true;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            pnlList.Visible = true;
            pnlForm.Visible = false;
        }

        private void LoadDropdowns()
        {
            try
            {
                using (SqlConnection con = DbHelper.GetConnection())
                {
                    long companyID = Convert.ToInt64(Session["CompanyID"]);

                    // Ledger Group
                    SqlCommand cmdGroup = new SqlCommand(@"SELECT LedgerGroup_Sno, LedgerGroup_Name FROM LedgerGroup_Table 
                                                           WHERE Company_No=@Company_No AND IsActive=1", con);
                    cmdGroup.Parameters.AddWithValue("@Company_No", companyID);
                    SqlDataAdapter daGroup = new SqlDataAdapter(cmdGroup);
                    DataTable dtGroup = new DataTable();
                    daGroup.Fill(dtGroup);

                    ddlGroup.DataSource = dtGroup;
                    ddlGroup.DataTextField = "LedgerGroup_Name";
                    ddlGroup.DataValueField = "LedgerGroup_Sno";
                    ddlGroup.DataBind();
                    ddlGroup.Items.Insert(0, new ListItem("-- Select Group --", "0"));

                    // Area Dropdown
                    SqlCommand cmdArea = new SqlCommand("sp_GetAreaDropdown", con);
                    cmdArea.CommandType = CommandType.StoredProcedure;
                    cmdArea.Parameters.AddWithValue("@Company_No", companyID);
                    SqlDataAdapter daArea = new SqlDataAdapter(cmdArea);
                    DataTable dtArea = new DataTable();
                    daArea.Fill(dtArea);

                    ddlArea.DataSource = dtArea;
                    ddlArea.DataTextField = "Area_Name";
                    ddlArea.DataValueField = "Area_Sno";
                    ddlArea.DataBind();
                    ddlArea.Items.Insert(0, new ListItem("-- Select Area --", "0"));

                    // GST States (Static for example)
                    ddlGSTState.Items.Clear();
                    ddlGSTState.Items.Add(new ListItem("Tamil Nadu (33)", "33"));
                    ddlGSTState.Items.Add(new ListItem("Karnataka (29)", "29"));
                    ddlGSTState.Items.Add(new ListItem("-- Select State --", "0"));
                    ddlGSTState.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Alert("Error loading dropdowns: " + ex.Message, "error");
            }
        }

        #endregion

        #region UI Handlers (Save / Add / Cancel)

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            ClearInputs();
            pnlList.Visible = false;
            pnlForm.Visible = true;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlList.Visible = true;
            pnlForm.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtLedgerName.Text))
                {
                    Alert("Ledger Name is required", "error");
                    return;
                }

                bool isUpdate = !string.IsNullOrEmpty(hfLedgerID.Value);

                using (SqlConnection con = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveLedger", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@Ledger_Sno", isUpdate ? Convert.ToInt64(hfLedgerID.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ledger_code", txtLedgerCode.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_name", txtLedgerName.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Active", ddlActive.SelectedValue);
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
                        cmd.Parameters.AddWithValue("@Ledger_PAN", txtPAN.Text.Trim());

                        // Use decimal.TryParse for safety
                        decimal openingBal = 0;
                        decimal.TryParse(txtOpening.Text, out openingBal);
                        cmd.Parameters.AddWithValue("@Ledger_open", openingBal);

                        cmd.Parameters.AddWithValue("@Balance_Type", ddlBalType.SelectedValue);

                        decimal limit = 0;
                        decimal.TryParse(txtCreditLimit.Text, out limit);
                        cmd.Parameters.AddWithValue("@Credit_Limit", limit);

                        int days = 0;
                        int.TryParse(txtCreditDays.Text, out days);
                        cmd.Parameters.AddWithValue("@Credit_Days", days);

                        cmd.Parameters.AddWithValue("@Is_TDS_Applicable", chkTDS.Checked);
                        cmd.Parameters.AddWithValue("@Use_GST", chkUseGST.Checked);
                        cmd.Parameters.AddWithValue("@Ledger_GST", txtGSTIN.Text.Trim());
                        cmd.Parameters.AddWithValue("@GST_DealerType", ddlDealerType.SelectedValue);
                        cmd.Parameters.AddWithValue("@GST_StateCode", ddlGSTState.SelectedValue);
                        cmd.Parameters.AddWithValue("@Ledger_remarks", txtRemarks.Text.Trim());

                        if (con.State == ConnectionState.Closed) con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                Alert(isUpdate ? "Ledger Updated Successfully" : "Ledger Saved Successfully", "success");

                ClearInputs();
                pnlList.Visible = true;
                pnlForm.Visible = false;
                LoadLedgerGrid();
            }
            catch (Exception ex)
            {
                Alert(ex.Message, "error");
            }
        }

        #endregion

        #region Operations (Edit / Delete)

        private void LoadLedgerForEdit(string id)
        {
            try
            {
                using (SqlConnection con = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetLedgerByID", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Ledger_Sno", id);
                        con.Open();
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.Read())
                        {
                            hfLedgerID.Value = dr["Ledger_Sno"].ToString();
                            txtLedgerCode.Text = dr["ledger_code"].ToString();
                            txtLedgerName.Text = dr["ledger_name"].ToString();
                            ddlActive.SelectedValue = dr["ledger_Active"].ToString();
                            ddlGroup.SelectedValue = dr["LedgerGroup_no"].ToString();
                            ddlArea.SelectedValue = dr["Area_no"].ToString();
                            txtAdd1.Text = dr["ledger_Add1"].ToString();
                            txtAdd2.Text = dr["ledger_Add2"].ToString();
                            txtAdd3.Text = dr["ledger_Add3"].ToString();
                            txtEmail.Text = dr["Ledger_Email"].ToString();
                            txtPhone.Text = dr["Ledger_Phone"].ToString();
                            txtContactPerson.Text = dr["Ledger_ContactPerson"].ToString();
                            txtBank.Text = dr["ledger_bank"].ToString();
                            txtBranch.Text = dr["Ledger_Branch"].ToString();
                            txtAcNo.Text = dr["ledger_AcNo"].ToString();
                            txtIfsc.Text = dr["ledger_Ifscode"].ToString();
                            txtPAN.Text = dr["Ledger_PAN"].ToString();
                            txtOpening.Text = dr["Ledger_open"].ToString();
                            ddlBalType.SelectedValue = dr["Balance_Type"].ToString();
                            txtCreditLimit.Text = dr["Credit_Limit"].ToString();
                            txtCreditDays.Text = dr["Credit_Days"].ToString();
                            chkTDS.Checked = Convert.ToBoolean(dr["Is_TDS_Applicable"]);
                            chkUseGST.Checked = Convert.ToBoolean(dr["Use_GST"]);
                            txtGSTIN.Text = dr["Ledger_GST"].ToString();
                            ddlDealerType.SelectedValue = dr["GST_DealerType"].ToString();
                            ddlGSTState.SelectedValue = dr["GST_StateCode"].ToString();
                            txtRemarks.Text = dr["Ledger_remarks"].ToString();

                            pnlList.Visible = false;
                            pnlForm.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Alert(ex.Message, "error");
            }
        }

        protected void DeleteLedger(string id)
        {
            try
            {
                using (SqlConnection con = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_DeleteLedger", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Ledger_Sno", id);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("Ledger Deleted Successfully", "success");
                LoadLedgerGrid();
            }
            catch (Exception ex)
            {
                Alert(ex.Message, "error");
            }
        }

        #endregion

        #region Helpers

        private void ClearInputs()
        {
            hfLedgerID.Value = "";
            txtLedgerCode.Text = "";
            txtLedgerName.Text = "";
            txtAdd1.Text = ""; txtAdd2.Text = ""; txtAdd3.Text = "";
            txtEmail.Text = ""; txtPhone.Text = ""; txtContactPerson.Text = "";
            txtBank.Text = ""; txtBranch.Text = ""; txtAcNo.Text = ""; txtIfsc.Text = "";
            txtPAN.Text = "";
            txtOpening.Text = "0";
            txtCreditLimit.Text = "0";
            txtCreditDays.Text = "0";
            txtGSTIN.Text = "";
            txtRemarks.Text = "";
            chkTDS.Checked = false;
            chkUseGST.Checked = true;
        }

        private void Alert(string message, string type)
        {
            string msg = message.Replace("'", "");
            string script = $"showNotification('{msg}','{type}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", script, true);
        }

        #endregion
    }
}
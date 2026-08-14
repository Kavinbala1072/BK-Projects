using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms
{
    public partial class ActivityForm : System.Web.UI.Page
    {
        private string SortExpression { get => (string)ViewState["SortExp"] ?? "ledger_Code"; set => ViewState["SortExp"] = value; }
        private string SortDirection { get => (string)ViewState["SortDir"] ?? "ASC"; set => ViewState["SortDir"] = value; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Session["CompanyID"] == null) { Response.Redirect("~/Login.aspx"); return; }
            if (Session["UserName"] == null || Session["CompanyID"] == null) { Response.Redirect("~/Login.aspx"); return; }

            SessionStatus status = SessionHelper.ValidateSession(Session);
            if (status != SessionStatus.Valid)
            {
                SessionHelper.EndSession(Session);
                Response.Redirect("~/Login.aspx?reason=" + status.ToString());
                return;
            }

            if (!IsPostBack)
            {
                txtDateFrom.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                txtDateTo.Text = DateTime.Now.ToString("yyyy-MM-dd");
                ShowSearchMode();
                LoadList();
            }
        }

        private void LoadList()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetLedgerList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@SearchText", txtSearch.Text.Trim());
                        cmd.Parameters.AddWithValue("@FromDate", txtDateFrom.Text);
                        cmd.Parameters.AddWithValue("@ToDate", txtDateTo.Text);

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        ApplyFiltersAndBind(dt);
                    }
                }
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        private void ApplyFiltersAndBind(DataTable dt)
        {
            litTotalCount.Text = dt.Rows.Count.ToString();
            List<string> filterList = new List<string>();

            if (gvLedgers.HeaderRow != null)
            {
                AddFilter(filterList, "flt_code", "ledger_code");
                AddFilter(filterList, "flt_name", "ledger_name");
                AddFilter(filterList, "flt_group", "LedgerGroup_Name");
                AddFilter(filterList, "flt_phone", "Ledger_Phone");
                AddFilter(filterList, "flt_gst", "Ledger_GST");
            }

            DataTable displayDt = dt;
            if (filterList.Count > 0)
            {
                try
                {
                    string combinedFilter = string.Join(" AND ", filterList);
                    DataRow[] filteredRows = dt.Select(combinedFilter);
                    displayDt = filteredRows.Length > 0 ? filteredRows.CopyToDataTable() : dt.Clone();
                }
                catch { displayDt = dt.Clone(); }
            }

            DataView dv = displayDt.DefaultView;
            dv.Sort = $"{SortExpression} {SortDirection}";

            gvLedgers.PageSize = int.Parse(ddlPageSize.SelectedValue);
            gvLedgers.DataSource = dv;
            gvLedgers.DataBind();

            litVisibleCount.Text = displayDt.Rows.Count.ToString();
        }

        private void AddFilter(List<string> list, string controlID, string columnName)
        {
            TextBox tb = (TextBox)gvLedgers.HeaderRow.FindControl(controlID);
            if (tb != null && !string.IsNullOrWhiteSpace(tb.Text))
            {
                string val = tb.Text.Trim().Replace("'", "''");
                list.Add($"{columnName} LIKE '%{val}%'");
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLedgerName.Text)) { Alert("Name is required", "error"); return; }
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveLedger", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpd = !string.IsNullOrEmpty(hfLedgerID.Value);

                        // Mandatory Params
                        cmd.Parameters.AddWithValue("@Action", isUpd ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@Ledger_Sno", isUpd ? Convert.ToInt64(hfLedgerID.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? DBNull.Value);

                        // FULL 26 FIELDS
                        // Identity
                        cmd.Parameters.AddWithValue("@ledger_name", txtLedgerName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@ledger_code", txtLedgerCode.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@ledger_Active", ddlActive.SelectedValue == "1");
                        cmd.Parameters.AddWithValue("@LedgerGroup_no", ddlGroup.SelectedValue);
                        cmd.Parameters.AddWithValue("@Area_no", ddlArea.SelectedValue);
                        cmd.Parameters.AddWithValue("@Credit_Limit", Convert.ToDecimal(string.IsNullOrEmpty(txtCreditLimit.Text) ? "0" : txtCreditLimit.Text));
                        cmd.Parameters.AddWithValue("@Credit_Days", Convert.ToInt32(string.IsNullOrEmpty(txtCreditDays.Text) ? "0" : txtCreditDays.Text));
                        cmd.Parameters.AddWithValue("@Is_TDS_Applicable", chkTDS.Checked);
                        cmd.Parameters.AddWithValue("@Ledger_PAN", txtPAN.Text.Trim().ToUpper());

                        // GST & Bank
                        cmd.Parameters.AddWithValue("@Use_GST", chkUseGST.Checked);
                        cmd.Parameters.AddWithValue("@Ledger_GST", txtGSTIN.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@GST_DealerType", ddlDealerType.SelectedValue);
                        cmd.Parameters.AddWithValue("@GST_StateCode", ddlGSTState.SelectedValue);
                        cmd.Parameters.AddWithValue("@ledger_bank", txtBank.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_Branch", txtBranch.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_AcNo", txtAcNo.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Ifscode", txtIfsc.Text.Trim());

                        // Address & Financials
                        cmd.Parameters.AddWithValue("@ledger_Add1", txtAdd1.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Add2", txtAdd2.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Add3", txtAdd3.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_ContactPerson", txtContactPerson.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_open", Convert.ToDecimal(string.IsNullOrEmpty(txtOpening.Text) ? "0" : txtOpening.Text));
                        cmd.Parameters.AddWithValue("@Balance_Type", ddlBalType.SelectedValue);
                        cmd.Parameters.AddWithValue("@Ledger_remarks", txtRemarks.Text.Trim());

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                Alert("Ledger registration successful!", "success");
                btnBack_Click(null, null);
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }
        private void Alert(string msg, string type)
        {
            string clean = msg.Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
            string script = $"showNotification('{clean}', '{type}');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertMessage", script, true);
        }

        private void LoadLedgerForEdit(string id)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetLedgerByID", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ledger_Sno", id);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        hfLedgerID.Value = dr["Ledger_Sno"].ToString();
                        LoadDropdowns();
                        // Populate 26 Fields
                        txtLedgerName.Text = dr["ledger_name"].ToString();
                        txtLedgerCode.Text = dr["ledger_code"].ToString();
                        ddlActive.SelectedValue = Convert.ToBoolean(dr["ledger_Active"]) ? "1" : "0";
                        ddlGroup.SelectedValue = dr["LedgerGroup_no"].ToString();
                        ddlArea.SelectedValue = dr["Area_no"].ToString();
                        txtCreditLimit.Text = dr["Credit_Limit"].ToString();
                        txtCreditDays.Text = dr["Credit_Days"].ToString();
                        chkTDS.Checked = Convert.ToBoolean(dr["Is_TDS_Applicable"]);
                        txtPAN.Text = dr["Ledger_PAN"].ToString();
                        chkUseGST.Checked = Convert.ToBoolean(dr["Use_GST"]);
                        txtGSTIN.Text = dr["Ledger_GST"].ToString();
                        ddlDealerType.SelectedValue = dr["GST_DealerType"].ToString();
                        ddlGSTState.SelectedValue = dr["GST_StateCode"].ToString();
                        txtBank.Text = dr["ledger_bank"].ToString();
                        txtBranch.Text = dr["Ledger_Branch"].ToString();
                        txtAcNo.Text = dr["ledger_AcNo"].ToString();
                        txtIfsc.Text = dr["ledger_Ifscode"].ToString();
                        txtAdd1.Text = dr["ledger_Add1"].ToString();
                        txtAdd2.Text = dr["ledger_Add2"].ToString();
                        txtAdd3.Text = dr["ledger_Add3"].ToString();
                        txtContactPerson.Text = dr["Ledger_ContactPerson"].ToString();
                        txtEmail.Text = dr["Ledger_Email"].ToString();
                        txtPhone.Text = dr["Ledger_Phone"].ToString();
                        txtOpening.Text = dr["Ledger_open"].ToString();
                        ddlBalType.SelectedValue = dr["Balance_Type"].ToString();
                        txtRemarks.Text = dr["Ledger_remarks"].ToString();
                        ShowAddMode();
                    }
                }
            }
        }

        private void ShowSearchMode()
        {
            pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = true;
            pnlForm.Visible = phAddButtons.Visible = false;
            litTitle.Text = "Ledger Directory";
        }

        private void ShowAddMode()
        {
            pnlList.Visible = phSearchControls.Visible = phSearchButtons.Visible = pnlFooter.Visible = false;
            pnlForm.Visible = phAddButtons.Visible = true;
            litTitle.Text = hfLedgerID.Value == "" ? "New Ledger Setup" : "Edit Ledger";
        }

        protected void GridFilter_Changed(object sender, EventArgs e) { LoadList(); }
        protected void txtSearch_TextChanged(object sender, EventArgs e) { LoadList(); }
        protected void gvLedgers_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (SortExpression == e.SortExpression)
            {
                SortDirection = (SortDirection == "ASC") ? "DESC" : "ASC";
            }
            else
            {
                SortExpression = e.SortExpression;
                SortDirection = "ASC";
            }
            LoadList();
        }
        protected void gvLedgers_RowCommand(object sender, GridViewCommandEventArgs e) { if (e.CommandName == "EditLedger") LoadLedgerForEdit(e.CommandArgument.ToString()); }
        protected void btnOpenCreate_Click(object sender, EventArgs e) { hfLedgerID.Value = ""; ClearInputs(); LoadDropdowns(); ShowAddMode(); }
        protected void btnBack_Click(object sender, EventArgs e) { ShowSearchMode(); LoadList(); }
        protected void Pager_Click(object sender, EventArgs e)
        {
            string cmd = ((LinkButton)sender).CommandArgument;
            if (cmd == "Prev" && gvLedgers.PageIndex > 0) gvLedgers.PageIndex--;
            else if (cmd == "Next") gvLedgers.PageIndex++;
            LoadList();
        }

        private void ClearInputs()
        {
            txtLedgerName.Text = txtLedgerCode.Text = txtGSTIN.Text = txtPAN.Text = "";
            txtBank.Text = txtBranch.Text = txtAcNo.Text = txtIfsc.Text = "";
            txtAdd1.Text = txtAdd2.Text = txtAdd3.Text = txtEmail.Text = txtPhone.Text = txtContactPerson.Text = "";
            txtOpening.Text = "0.00"; txtCreditLimit.Text = "0.00"; txtCreditDays.Text = "0";
            txtRemarks.Text = ""; chkTDS.Checked = false; chkUseGST.Checked = true;
        }

        private void LoadDropdowns()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                long cid = Convert.ToInt64(Session["CompanyID"]);
                // Bind Groups
                SqlDataAdapter daG = new SqlDataAdapter("SELECT LedgerGroup_Sno, LedgerGroup_Name FROM LedgerGroup_Table WHERE Company_No = @cid AND IsActive=1", conn);
                daG.SelectCommand.Parameters.AddWithValue("@cid", cid);
                DataTable dtG = new DataTable(); daG.Fill(dtG);
                ddlGroup.DataSource = dtG; ddlGroup.DataTextField = "LedgerGroup_Name"; ddlGroup.DataValueField = "LedgerGroup_Sno"; ddlGroup.DataBind();
                ddlGroup.Items.Insert(0, new ListItem("-- Select Group --", "0"));

                // Bind Area
                using (SqlCommand cmdA = new SqlCommand("sp_GetAreaDropdown", conn))
                {
                    cmdA.CommandType = CommandType.StoredProcedure;
                    cmdA.Parameters.AddWithValue("@Company_No", cid);
                    SqlDataAdapter daA = new SqlDataAdapter(cmdA);
                    DataTable dtA = new DataTable(); daA.Fill(dtA);
                    ddlArea.DataSource = dtA; ddlArea.DataTextField = "Area_Name"; ddlArea.DataValueField = "Area_Sno"; ddlArea.DataBind();
                    ddlArea.Items.Insert(0, new ListItem("-- Select Area --", "0"));
                }

                // Bind States (Hardcoded list for example)
                ddlGSTState.Items.Clear();
                ddlGSTState.Items.Add(new ListItem("Tamil Nadu (33)", "33"));
                ddlGSTState.Items.Add(new ListItem("Maharashtra (27)", "27"));
                ddlGSTState.Items.Insert(0, new ListItem("-- Select State --", "0"));
            }
        }
    }
}
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class SupplierMaster : System.Web.UI.Page
    {
        private const long CustomerGroupID = 1000000030;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack)
            {
                LoadDropdowns();
                LoadList();
            }
        }

        private void LoadDropdowns()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    string spArea = "sp_GetAreaDropdown";

                    using (SqlCommand cmd = new SqlCommand(spArea, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);

                        SqlDataAdapter daA = new SqlDataAdapter(cmd);
                        DataTable dtA = new DataTable();
                        daA.Fill(dtA);

                        ddlArea.DataSource = dtA;
                        ddlArea.DataTextField = "Area_Name";
                        ddlArea.DataValueField = "Area_Sno";
                        ddlArea.DataBind();
                        ddlArea.Items.Insert(0, new ListItem("-- Select Area --", "0"));
                    }

                    ddlState.Items.Clear();
                    ddlState.Items.Add(new ListItem("Tamil Nadu (33)", "33"));
                    ddlState.Items.Add(new ListItem("Maharashtra (27)", "27"));
                    ddlState.Items.Add(new ListItem("Karnataka (29)", "29"));
                    ddlState.Items.Insert(0, new ListItem("-- Select State --", "0"));
                }
            }
            catch (Exception ex)
            {
                Alert("Error loading dropdowns: " + ex.Message, "error");
            }
        }

        private void LoadList()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetSupplierList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@LedgerGroup_no", CustomerGroupID);

                        System.Web.HttpContext.Current.Response.AddHeader("X-Called-SP", "sp_SaveSupplier");

                        if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                        {
                            cmd.Parameters.AddWithValue("@SearchText", txtSearch.Text.Trim());
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@SearchText", DBNull.Value);
                        }

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        gvCustomers.DataSource = dt;
                        gvCustomers.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Alert("Load Error: " + ex.Message, "error");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCustName.Text) || string.IsNullOrWhiteSpace(txtCustCode.Text) || string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                Alert("Name, Code, and Mobile Number are required!", "error");
                return;
            }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveSupplier", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfCustomerID.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@Ledger_Sno", isUpdate ? Convert.ToInt64(hfCustomerID.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ledger_name", txtCustName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@ledger_code", txtCustCode.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@Ledger_Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Active", chkActive.Checked);
                        cmd.Parameters.AddWithValue("@Area_no", ddlArea.SelectedValue);
                        cmd.Parameters.AddWithValue("@ledger_Add1", txtAdd1.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Add2", txtAdd2.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Add3", txtAdd3.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_ContactPerson", txtContactPerson.Text.Trim());
                        cmd.Parameters.AddWithValue("@Balance_Type", ddlBalType.SelectedValue);
                        cmd.Parameters.AddWithValue("@Credit_Limit", Convert.ToDecimal(txtCreditLimit.Text));
                        cmd.Parameters.AddWithValue("@Credit_Days", Convert.ToInt32(txtCreditDays.Text));
                        cmd.Parameters.AddWithValue("@Ledger_GST", txtGST.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@GST_DealerType", ddlDealer.SelectedValue);
                        cmd.Parameters.AddWithValue("@GST_StateCode", ddlState.SelectedValue);
                        cmd.Parameters.AddWithValue("@Ledger_open", Convert.ToDecimal(txtOpening.Text));
                        cmd.Parameters.AddWithValue("@Ledger_remarks", txtRemarks.Text.Trim());

                        cmd.ExecuteNonQuery();
                        btnBack_Click(null, null);
                        Alert("Data saved successfully!", "success");
                    }
                }
            }
            catch (SqlException ex) { Alert(ex.Message, "error"); }
            catch (Exception ex) { Alert("System Error: " + ex.Message, "error"); }
        }

        protected void gvCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord")
            {
                LoadForEdit(Convert.ToInt64(e.CommandArgument));
            }
        }

        private void LoadForEdit(long sno)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ledger_Table WHERE Ledger_Sno = @sno", conn);
                cmd.Parameters.AddWithValue("@sno", sno);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        hfCustomerID.Value = sno.ToString();
                        txtCustName.Text = dr["ledger_name"].ToString();
                        txtCustCode.Text = dr["ledger_code"].ToString();
                        chkActive.Checked = Convert.ToBoolean(dr["ledger_Active"]);
                        ddlArea.SelectedValue = dr["Area_no"].ToString();
                        txtPhone.Text = dr["Ledger_Phone"].ToString();
                        txtEmail.Text = dr["Ledger_Email"].ToString();
                        txtAdd1.Text = dr["ledger_Add1"].ToString();
                        txtAdd2.Text = dr["ledger_Add2"].ToString();
                        txtAdd3.Text = dr["ledger_Add3"].ToString();
                        txtGST.Text = dr["Ledger_GST"].ToString();
                        ddlState.SelectedValue = dr["GST_StateCode"].ToString();
                        ddlDealer.SelectedValue = dr["GST_DealerType"].ToString();
                        txtOpening.Text = dr["Ledger_open"].ToString();
                        ddlBalType.SelectedValue = dr["Balance_Type"].ToString();
                        txtCreditLimit.Text = dr["Credit_Limit"].ToString();
                        txtCreditDays.Text = dr["Credit_Days"].ToString();
                        txtRemarks.Text = dr["Ledger_remarks"].ToString();
                        txtContactPerson.Text = dr["Ledger_ContactPerson"]?.ToString();

                        pnlList.Visible = false; pnlForm.Visible = true;
                    }
                }
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e) => LoadList();
        protected void btnOpenCreate_Click(object sender, EventArgs e) { hfCustomerID.Value = ""; ClearInputs(); pnlList.Visible = false; pnlForm.Visible = true; }
        protected void btnBack_Click(object sender, EventArgs e) { pnlList.Visible = true; pnlForm.Visible = false; LoadList(); }

        private void Alert(string msg, string type)
        {
            string script = $"showNotification('{msg.Replace("'", "\\'")}', '{type}');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", script, true);
        }

        private void ClearInputs()
        {
            txtCustName.Text = txtCustCode.Text = txtContactPerson.Text = txtPhone.Text = "";
            txtEmail.Text = txtAdd1.Text = txtAdd2.Text = txtAdd3.Text = txtGST.Text = "";
            txtOpening.Text = "0.00"; txtCreditLimit.Text = "0"; txtCreditDays.Text = "0";
            txtRemarks.Text = ""; chkActive.Checked = true;
            ddlArea.SelectedIndex = ddlState.SelectedIndex = ddlDealer.SelectedIndex = ddlBalType.SelectedIndex = 0;
        }
   
    }
}
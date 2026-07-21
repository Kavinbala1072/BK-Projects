using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class JobWorkMaster : System.Web.UI.Page
    {
        private const long JobWorkerGroupID = 1000000031;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack) { LoadDropdowns(); LoadList(); }
        }

        private void LoadDropdowns()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetAreaDropdown", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                    SqlDataAdapter daA = new SqlDataAdapter(cmd);
                    DataTable dtA = new DataTable(); daA.Fill(dtA);
                    ddlArea.DataSource = dtA; ddlArea.DataTextField = "Area_Name"; ddlArea.DataValueField = "Area_Sno"; ddlArea.DataBind();
                    ddlArea.Items.Insert(0, new ListItem("-- Select Area --", "0"));
                }
            }
        }

        private void LoadList()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetJobWorkerList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                            cmd.Parameters.AddWithValue("@SearchText", txtSearch.Text.Trim());

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable(); da.Fill(dt);
                        gvWorkers.DataSource = dt; gvWorkers.DataBind();
                    }
                }
            }
            catch (Exception ex) { Alert("Load Error: " + ex.Message, "error"); }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // 1. Mandatory Field Validation
            if (string.IsNullOrWhiteSpace(txtWorkerName.Text) ||
                string.IsNullOrWhiteSpace(txtWorkerCode.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                Alert("Worker Name, ID, and Mobile Number are required!", "error");
                return;
            }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveJobWorker", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfJobWorkerID.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@Ledger_Sno", isUpdate ? Convert.ToInt64(hfJobWorkerID.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ledger_name", txtWorkerName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@ledger_code", txtWorkerCode.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@Ledger_Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Active", chkActive.Checked);
                        cmd.Parameters.AddWithValue("@Area_no", ddlArea.SelectedValue);
                        cmd.Parameters.AddWithValue("@ledger_Add1", txtAdd1.Text.Trim());
                        cmd.Parameters.AddWithValue("@ledger_Add2", txtAdd2.Text.Trim());
                        cmd.Parameters.AddWithValue("@Ledger_Email", txtEmail.Text.Trim());
                        decimal openingBal = 0;
                        decimal.TryParse(txtOpening.Text, out openingBal);
                        cmd.Parameters.AddWithValue("@Ledger_open", openingBal);

                        cmd.Parameters.AddWithValue("@Ledger_remarks", txtRemarks.Text.Trim());

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();

                        btnBack_Click(null, null);
                        Alert("Labour details saved successfully!", "success");
                    }
                }
            }
            catch (SqlException ex)
            {
                Alert(ex.Message, "error");
            }
            catch (Exception ex)
            {
                Alert("System Error: " + ex.Message, "error");
            }
        }

        private void Alert(string msg, string type)
        {
            string clean = msg.Replace("'", "\\'").Replace("\r", "").Replace("\n", " ");
            string script = $"showNotification('{clean}', '{type}');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertMessage", script, true);
        }

        protected void gvWorkers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord") { LoadForEdit(Convert.ToInt64(e.CommandArgument)); }
        }

        private void LoadForEdit(long sno)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Ledger_Table WHERE Ledger_Sno = @sno", conn);
                cmd.Parameters.AddWithValue("@sno", sno);
                if (conn.State == ConnectionState.Closed) conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        hfJobWorkerID.Value = sno.ToString();
                        txtWorkerName.Text = dr["ledger_name"].ToString();
                        txtWorkerCode.Text = dr["ledger_code"].ToString();
                        chkActive.Checked = Convert.ToBoolean(dr["ledger_Active"]);
                        ddlArea.SelectedValue = dr["Area_no"].ToString();
                        txtPhone.Text = dr["Ledger_Phone"].ToString();
                        txtEmail.Text = dr["Ledger_Email"].ToString();
                        txtAdd1.Text = dr["ledger_Add1"].ToString();
                        txtAdd2.Text = dr["ledger_Add2"].ToString();
                        txtOpening.Text = dr["Ledger_open"].ToString();
                        txtRemarks.Text = dr["Ledger_remarks"].ToString();
                        pnlList.Visible = false; pnlForm.Visible = true;
                    }
                }
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e) => LoadList();
        protected void btnOpenCreate_Click(object sender, EventArgs e) 
        { 
            hfJobWorkerID.Value = ""; 
            ClearInputs(); 
            pnlList.Visible = false; 
            pnlForm.Visible = true; 
        }
        protected void btnBack_Click(object sender, EventArgs e) 
        { 
            pnlList.Visible = true; 
            pnlForm.Visible = false; 
            LoadList(); 
        }

        private void ClearInputs()
        {
            txtWorkerName.Text = txtWorkerCode.Text = txtPhone.Text = "";
            txtEmail.Text = txtAdd1.Text = txtAdd2.Text = "";
            txtOpening.Text = "0.00"; txtRemarks.Text = ""; chkActive.Checked = true;
            ddlArea.SelectedIndex = 0;
        }
    }
}
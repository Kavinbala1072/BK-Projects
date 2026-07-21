using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class FinancialYearMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack) LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    // Query uses the 'ACTIVE_FIN_YEAR' key in Control_Table to mark the grid
                    string sql = @"
                        SELECT F.*, 
                        CASE WHEN CAST(C.Ctl_Value AS INT) = F.FY_Sno THEN 1 ELSE 0 END as IsActiveYear
                        FROM FinYear_Table F
                        LEFT JOIN Control_Table C ON F.Company_No = C.Company_No AND C.Ctl_MtDesc = 'ACTIVE_FIN_YEAR'
                        WHERE F.Company_No = @cid ORDER BY F.StartDate DESC";

                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gvYears.DataSource = dt;
                    gvYears.DataBind();

                    // Load Dropdown
                    ddlActiveFY.DataSource = dt;
                    ddlActiveFY.DataTextField = "FY_Name";
                    ddlActiveFY.DataValueField = "FY_Sno";
                    ddlActiveFY.DataBind();
                    ddlActiveFY.Items.Insert(0, new ListItem("-- Select Year --", "0"));

                    // Auto-select current active year in dropdown
                    DataRow[] activeRow = dt.Select("IsActiveYear = 1");
                    if (activeRow.Length > 0)
                        ddlActiveFY.SelectedValue = activeRow[0]["FY_Sno"].ToString();
                }
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        protected void btnSaveYear_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFYName.Text)) { Alert("Enter Year Name", "error"); return; }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveFinYear", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@FY_Name", txtFYName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@StartDate", txtStart.Text);
                        cmd.Parameters.AddWithValue("@EndDate", txtEnd.Text);

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();

                        Alert("New Financial Year added to list", "success");
                        txtFYName.Text = txtStart.Text = txtEnd.Text = "";
                        LoadData();
                    }
                }
            }
            catch (SqlException ex) { Alert(ex.Message, "error"); }
        }

        protected void btnSetActive_Click(object sender, EventArgs e)
        {
            if (ddlActiveFY.SelectedValue == "0") { Alert("Please select a year", "error"); return; }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SetControlValue", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@MtDesc", "ACTIVE_FIN_YEAR"); // Standard Key
                        cmd.Parameters.AddWithValue("@Value", ddlActiveFY.SelectedValue);

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();

                        // Sync Session
                        Session["ActiveFY_Sno"] = ddlActiveFY.SelectedValue;
                        Session["ActiveFY_Name"] = ddlActiveFY.SelectedItem.Text;

                        Alert("Active Financial Year Updated!", "success");
                        LoadData();
                    }
                }
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        private void Alert(string msg, string type)
        {
            string script = $"showNotification('{msg.Replace("'", "\\'")}', '{type}');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", script, true);
        }
    }
}
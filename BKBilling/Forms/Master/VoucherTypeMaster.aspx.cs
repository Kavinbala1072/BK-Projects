using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class VoucherTypeMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack) LoadList();
        }

        private void LoadLedgerDropdowns()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                // Fetch all active ledgers for the company
                string sql = "SELECT Ledger_Sno, ledger_name FROM Ledger_Table WHERE Company_No = @cid AND ledger_Active = 1 ORDER BY ledger_name";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                BindDDL(ddlMainLedger, dt);
                BindDDL(ddlDiscountLedger, dt);
                BindDDL(ddlRoundOffLedger, dt);
            }
        }

        private void BindDDL(DropDownList ddl, DataTable dt)
        {
            ddl.DataSource = dt;
            ddl.DataTextField = "ledger_name";
            ddl.DataValueField = "Ledger_Sno";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("-- Select Ledger --", "0"));
        }

        private void LoadList()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                string sql = "SELECT * FROM VoucherType_Table WHERE Company_No = @cid";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                gvVTypes.DataSource = dt;
                gvVTypes.DataBind();
            }
        }

        protected void gvVTypes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                LoadLedgerDropdowns();
                LoadForEdit(id);
            }
        }

        private void LoadForEdit(int id)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM VoucherType_Table WHERE VoucherType_Sno=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                if (conn.State == ConnectionState.Closed) conn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        hfVTypeID.Value = id.ToString();
                        litVName.Text = dr["Voucher_Name"].ToString();
                        txtPrintTitle.Text = dr["Print_Title"].ToString();
                        txtPrefix.Text = dr["Prefix"].ToString();
                        txtSuffix.Text = dr["Suffix"].ToString();
                        ddlWidth.SelectedValue = dr["Padding_Width"].ToString();

                        ddlMainLedger.SelectedValue = dr["Main_Ledger_Sno"].ToString() == "" ? "0" : dr["Main_Ledger_Sno"].ToString();
                        ddlDiscountLedger.SelectedValue = dr["Discount_Ledger_Sno"].ToString() == "" ? "0" : dr["Discount_Ledger_Sno"].ToString();
                        ddlRoundOffLedger.SelectedValue = dr["RoundOff_Ledger_Sno"].ToString() == "" ? "0" : dr["RoundOff_Ledger_Sno"].ToString();

                        chkActive.Checked = Convert.ToBoolean(dr["IsActive"]);
                        chkTaxInclusive.Checked = Convert.ToBoolean(dr["Is_Tax_Inclusive"]);

                        pnlList.Visible = false; pnlForm.Visible = true;
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_SaveVoucherType", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@VoucherType_Sno", hfVTypeID.Value);
                    cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                    cmd.Parameters.AddWithValue("@Prefix", txtPrefix.Text.Trim());
                    cmd.Parameters.AddWithValue("@Suffix", txtSuffix.Text.Trim());
                    cmd.Parameters.AddWithValue("@Padding_Width", ddlWidth.SelectedValue);
                    cmd.Parameters.AddWithValue("@Print_Title", txtPrintTitle.Text.Trim());

                    cmd.Parameters.AddWithValue("@Main_Ledger_Sno", ddlMainLedger.SelectedValue);
                    cmd.Parameters.AddWithValue("@Discount_Ledger_Sno", ddlDiscountLedger.SelectedValue);
                    cmd.Parameters.AddWithValue("@RoundOff_Ledger_Sno", ddlRoundOffLedger.SelectedValue);
                    cmd.Parameters.AddWithValue("@Is_Tax_Inclusive", chkTaxInclusive.Checked);
                    cmd.Parameters.AddWithValue("@IsActive", chkActive.Checked);

                    if (conn.State == ConnectionState.Closed) conn.Open();
                    cmd.ExecuteNonQuery();
                    btnBack_Click(null, null);
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e) { pnlList.Visible = true; pnlForm.Visible = false; LoadList(); }
    }
}
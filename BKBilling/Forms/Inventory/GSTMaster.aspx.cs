using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class GSTMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack) LoadList();
        }

        private void LoadDropdowns()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                string sql = "SELECT Ledger_Sno, ledger_name FROM Ledger_Table WHERE Company_No = @cid AND ledger_Active = 1 ORDER BY ledger_name";
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                da.SelectCommand.Parameters.AddWithValue("@cid", Session["CompanyID"]);
                DataTable dt = new DataTable(); da.Fill(dt);

                BindDDL(ddlSGST_LocSales, dt); BindDDL(ddlSGST_SalesTax, dt);
                BindDDL(ddlSGST_LocPur, dt); BindDDL(ddlSGST_PurTax, dt);
                BindDDL(ddlCGST_SalesTax, dt); BindDDL(ddlCGST_PurTax, dt);
                BindDDL(ddlIGST_IntSales, dt); BindDDL(ddlIGST_SalesTax, dt);
                BindDDL(ddlIGST_IntPur, dt); BindDDL(ddlIGST_PurTax, dt);
            }
        }

        private void BindDDL(DropDownList ddl, DataTable dt)
        {
            ddl.DataSource = dt; ddl.DataTextField = "ledger_name"; ddl.DataValueField = "Ledger_Sno"; ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("-- Not Set --", "0"));
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_SaveGST", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    bool isUpdate = !string.IsNullOrEmpty(hfGSTSno.Value);
                    cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                    cmd.Parameters.AddWithValue("@GST_Sno", isUpdate ? hfGSTSno.Value : "0");
                    cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                    cmd.Parameters.AddWithValue("@Tax_Name", txtTaxName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Print_Name", txtPrintName.Text.Trim());
                    cmd.Parameters.AddWithValue("@SGST_Rate", txtSGST.Text);
                    cmd.Parameters.AddWithValue("@CGST_Rate", txtCGST.Text);
                    cmd.Parameters.AddWithValue("@IGST_Rate", txtIGST.Text);
                    cmd.Parameters.AddWithValue("@CESS_Rate", 0);

                    cmd.Parameters.AddWithValue("@SGST_LocSales_Acount", ddlSGST_LocSales.SelectedValue);
                    cmd.Parameters.AddWithValue("@SGST_SalesTax_Ledger", ddlSGST_SalesTax.SelectedValue);
                    cmd.Parameters.AddWithValue("@SGST_LocPur_Account", ddlSGST_LocPur.SelectedValue);
                    cmd.Parameters.AddWithValue("@SGST_PurTax_Ledger", ddlSGST_PurTax.SelectedValue);
                    cmd.Parameters.AddWithValue("@CGST_SalesTax_Ledger", ddlCGST_SalesTax.SelectedValue);
                    cmd.Parameters.AddWithValue("@CGST_PurTax_Ledger", ddlCGST_PurTax.SelectedValue);
                    cmd.Parameters.AddWithValue("@IGST_IntSales_Acount", ddlIGST_IntSales.SelectedValue);
                    cmd.Parameters.AddWithValue("@IGST_SalesTax_Ledger", ddlIGST_SalesTax.SelectedValue);
                    cmd.Parameters.AddWithValue("@IGST_IntPur_Account", ddlIGST_IntPur.SelectedValue);
                    cmd.Parameters.AddWithValue("@IGST_PurTax_Ledger", ddlIGST_PurTax.SelectedValue);
                    cmd.Parameters.AddWithValue("@CESS_SalesTax_Ledger", 0);
                    cmd.Parameters.AddWithValue("@CESS_PurTax_Ledger", 0);
                    cmd.Parameters.AddWithValue("@IsActive", 1);

                    if (conn.State == ConnectionState.Closed) conn.Open();
                    cmd.ExecuteNonQuery();
                    btnBack_Click(null, null);
                }
            }
        }

        private void LoadList()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetGSTList", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                    SqlDataAdapter da = new SqlDataAdapter(cmd); DataTable dt = new DataTable(); da.Fill(dt);
                    gvGST.DataSource = dt; gvGST.DataBind();
                }
            }
        }

        protected void gvGST_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                LoadDropdowns();
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetGSTByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure; cmd.Parameters.AddWithValue("@GST_Sno", id);

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        SqlDataReader dr = cmd.ExecuteReader();
                        if (dr.Read())
                        {
                            hfGSTSno.Value = id.ToString();
                            txtTaxName.Text = dr["Tax_Name"].ToString();
                            txtPrintName.Text = dr["Print_Name"].ToString();
                            txtSGST.Text = dr["SGST_Rate"].ToString();
                            txtCGST.Text = dr["CGST_Rate"].ToString();
                            txtIGST.Text = dr["IGST_Rate"].ToString();
                            ddlSGST_LocSales.SelectedValue = dr["SGST_LocSales_Acount"].ToString();
                            ddlSGST_SalesTax.SelectedValue = dr["SGST_SalesTax_Ledger"].ToString();
                            ddlSGST_LocPur.SelectedValue = dr["SGST_LocPur_Account"].ToString();
                            ddlSGST_PurTax.SelectedValue = dr["SGST_PurTax_Ledger"].ToString();
                            ddlCGST_SalesTax.SelectedValue = dr["CGST_SalesTax_Ledger"].ToString();
                            ddlCGST_PurTax.SelectedValue = dr["CGST_PurTax_Ledger"].ToString();
                            ddlIGST_IntSales.SelectedValue = dr["IGST_IntSales_Acount"].ToString();
                            ddlIGST_SalesTax.SelectedValue = dr["IGST_SalesTax_Ledger"].ToString();
                            ddlIGST_IntPur.SelectedValue = dr["IGST_IntPur_Account"].ToString();
                            ddlIGST_PurTax.SelectedValue = dr["IGST_PurTax_Ledger"].ToString();
                            pnlList.Visible = false; pnlForm.Visible = true;
                        }
                    }
                }
            }
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e) { hfGSTSno.Value = ""; LoadDropdowns(); pnlList.Visible = false; pnlForm.Visible = true; }
        protected void btnBack_Click(object sender, EventArgs e) { pnlList.Visible = true; pnlForm.Visible = false; LoadList(); }
        private void Alert(string msg, string type) { ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", $"showNotification('{msg.Replace("'", "\\'")}', '{type}');", true); }
    }
}
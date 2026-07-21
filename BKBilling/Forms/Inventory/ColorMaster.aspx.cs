using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class ColorMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");
            if (!IsPostBack) LoadList();
        }

        private void LoadList()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetColorList", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                    cmd.Parameters.AddWithValue("@SearchText", string.IsNullOrEmpty(txtSearch.Text) ? (object)DBNull.Value : txtSearch.Text.Trim());
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvColors.DataSource = dt;
                    gvColors.DataBind();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtColorName.Text)) { Alert("Color name is required", "error"); return; }

            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveColor", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfColorSno.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@Color_Sno", isUpdate ? Convert.ToInt32(hfColorSno.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? 0);
                        cmd.Parameters.AddWithValue("@Color_Name", txtColorName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@Color_HexCode", txtHex.Text);
                        cmd.Parameters.AddWithValue("@IsActive", chkActive.Checked);

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();

                        btnBack_Click(null, null);
                        Alert("Color saved successfully!", "success");
                    }
                }
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        protected void gvColors_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Color_Table WHERE Color_Sno=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfColorSno.Value = id.ToString();
                        txtColorName.Text = dr["Color_Name"].ToString();
                        txtHex.Text = dr["Color_HexCode"].ToString();
                        chkActive.Checked = Convert.ToBoolean(dr["IsActive"]);
                        pnlList.Visible = false; pnlForm.Visible = true;
                    }
                }
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e) { LoadList(); }
        protected void btnOpenCreate_Click(object sender, EventArgs e) { hfColorSno.Value = ""; txtColorName.Text = ""; txtHex.Text = "#000000"; chkActive.Checked = true; pnlList.Visible = false; pnlForm.Visible = true; }
        protected void btnBack_Click(object sender, EventArgs e) { pnlList.Visible = true; pnlForm.Visible = false; LoadList(); }
        private void Alert(string msg, string type) { ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", $"showNotification('{msg.Replace("'", "\\'")}', '{type}');", true); }
    }
}
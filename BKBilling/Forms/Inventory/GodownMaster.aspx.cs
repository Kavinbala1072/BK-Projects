using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class; // Your connection helper class

namespace BKBilling.Forms.Master
{
    public partial class GodownMaster : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //BindGrid();
            }
        }

        private void BindGrid(string search = "")
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                string sql = "SELECT * FROM Godown_Master";
                if (!string.IsNullOrEmpty(search))
                    sql += " WHERE Godown_Name LIKE @search OR City LIKE @search";

                sql += " ORDER BY Godown_Name";

                SqlCommand cmd = new SqlCommand(sql, conn);
                if (!string.IsNullOrEmpty(search)) cmd.Parameters.AddWithValue("@search", "%" + search + "%");

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                gvGodown.DataSource = dt;
                gvGodown.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtGodownName.Text)) return;

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                string sql = "";
                if (string.IsNullOrEmpty(hfGodownId.Value))
                    sql = @"INSERT INTO Godown_Master (Godown_Name, GSTIN, Address, City, State, Phone_No) 
                            VALUES (@name, @gst, @addr, @city, @state, @phone)";
                else
                    sql = @"UPDATE Godown_Master SET Godown_Name=@name, GSTIN=@gst, Address=@addr, 
                            City=@city, State=@state, Phone_No=@phone WHERE Godown_Sno=@id";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@name", txtGodownName.Text.Trim());
                cmd.Parameters.AddWithValue("@gst", txtGSTIN.Text.Trim().ToUpper());
                cmd.Parameters.AddWithValue("@addr", txtAddress.Text.Trim());
                cmd.Parameters.AddWithValue("@city", txtCity.Text.Trim());
                cmd.Parameters.AddWithValue("@state", txtState.Text.Trim());
                cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());

                if (!string.IsNullOrEmpty(hfGodownId.Value))
                    cmd.Parameters.AddWithValue("@id", hfGodownId.Value);

                cmd.ExecuteNonQuery();
            }

            ClearForm();
            BindGrid();
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "alert('Godown updated successfully!');", true);
        }

        protected void gvGodown_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "EditGodown")
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Godown_Master WHERE Godown_Sno=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        hfGodownId.Value = dr["Godown_Sno"].ToString();
                        txtGodownName.Text = dr["Godown_Name"].ToString();
                        txtGSTIN.Text = dr["GSTIN"].ToString();
                        txtAddress.Text = dr["Address"].ToString();
                        txtCity.Text = dr["City"].ToString();
                        txtState.Text = dr["State"].ToString();
                        txtPhone.Text = dr["Phone_No"].ToString();
                        btnSave.Text = "Update Godown";
                    }
                }
            }
            else if (e.CommandName == "DeleteGodown")
            {
                // Safety check logic should go here (check for existing stock)
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Godown_Master WHERE Godown_Sno=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
                BindGrid();
            }
        }

        protected void txtSearch_TextChanged(object sender, EventArgs e) => BindGrid(txtSearch.Text.Trim());

        protected void btnClear_Click(object sender, EventArgs e) => ClearForm();

        private void ClearForm()
        {
            txtGodownName.Text = "";
            txtGSTIN.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtPhone.Text = "";
            hfGodownId.Value = "";
            btnSave.Text = "Save Godown";
        }
    }
}
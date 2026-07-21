using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using BKBilling.Class; // Assuming your DbHelper is here

namespace BKBilling.Forms.Transaction
{
    public partial class AdjustmentForm : System.Web.UI.Page
    {
        DataTable dtItems;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtUser.Text = Session["UserName"]?.ToString();
                txtRefNo.Text = "ADJ-" + DateTime.Now.ToString("ssmmHH");

                //BindDropdowns();
                CreateDataTable();
            }
        }

        private void BindDropdowns()
        {
            // Load Items and Godowns from your DB
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                // Items
                SqlDataAdapter adp = new SqlDataAdapter("SELECT Item_Sno, Item_Name FROM Item_Master", conn);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                ddlItem.DataSource = dt;
                ddlItem.DataTextField = "Item_Name";
                ddlItem.DataValueField = "Item_Sno";
                ddlItem.DataBind();
                ddlItem.Items.Insert(0, new ListItem("-- Select Product --", "0"));

                // Godowns
                adp = new SqlDataAdapter("SELECT Godown_Sno, Godown_Name FROM Godown_Master", conn);
                DataTable dtG = new DataTable();
                adp.Fill(dtG);
                ddlGodown.DataSource = dtG;
                ddlGodown.DataTextField = "Godown_Name";
                ddlGodown.DataValueField = "Godown_Sno";
                ddlGodown.DataBind();
            }
        }

        private void CreateDataTable()
        {
            dtItems = new DataTable();
            dtItems.Columns.Add("ItemID");
            dtItems.Columns.Add("ItemName");
            dtItems.Columns.Add("Type");
            dtItems.Columns.Add("Qty");
            dtItems.Columns.Add("Remarks");
            ViewState["AdjTable"] = dtItems;
        }

        protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Logic to fetch current stock for selected item
            if (ddlItem.SelectedValue != "0")
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    string sql = "SELECT Current_Stock FROM Stock_Table WHERE Item_Sno = @id";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", ddlItem.SelectedValue);
                    object stock = cmd.ExecuteScalar();
                    txtCurrStock.Text = stock != null ? stock.ToString() : "0";
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (ddlItem.SelectedValue == "0" || string.IsNullOrEmpty(txtQty.Text)) return;

            dtItems = (DataTable)ViewState["AdjTable"];
            DataRow dr = dtItems.NewRow();
            dr["ItemID"] = ddlItem.SelectedValue;
            dr["ItemName"] = ddlItem.SelectedItem.Text;
            dr["Type"] = ddlType.SelectedValue;
            dr["Qty"] = txtQty.Text;
            dr["Remarks"] = txtRemarks.Text;
            dtItems.Rows.Add(dr);

            ViewState["AdjTable"] = dtItems;
            gvAdjustment.DataSource = dtItems;
            gvAdjustment.DataBind();

            // Clear inputs
            txtQty.Text = "";
            txtRemarks.Text = "";
        }

        protected void gvAdjustment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            dtItems = (DataTable)ViewState["AdjTable"];
            dtItems.Rows[e.RowIndex].Delete();
            ViewState["AdjTable"] = dtItems;
            gvAdjustment.DataSource = dtItems;
            gvAdjustment.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            dtItems = (DataTable)ViewState["AdjTable"];
            if (dtItems.Rows.Count == 0) return;

            // Database Logic:
            // 1. Insert into Adjustment_Master
            // 2. Loop through dtItems and Update Stock_Table (Add or Subtract)
            // 3. Clear and show success message

            // Example Logic:
            // foreach(DataRow row in dtItems.Rows) { 
            //    decimal qty = Convert.ToDecimal(row["Qty"]);
            //    if(row["Type"].ToString() == "DEDUCT") qty = qty * -1;
            //    UpdateStock(row["ItemID"], qty); 
            // }

            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Stock Adjusted Successfully!');", true);
            Response.Redirect(Request.RawUrl); // Refresh
        }

        protected void btnClear_Click(object sender, EventArgs e) => Response.Redirect(Request.RawUrl);
    }
}
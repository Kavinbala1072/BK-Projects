using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class ItemMaster : System.Web.UI.Page
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
                long cid = Convert.ToInt64(Session["CompanyID"]);
                FillDDL(ddlGroup, "SELECT ItemGroup_Sno, ItemGroup_Name FROM ItemGroup_Table WHERE Company_No=" + cid + " AND IsActive=1", conn);
                FillDDL(ddlSubCategory, "SELECT SubCat_Sno, SubCat_Name FROM ItemSubCategory_Table WHERE Company_No=" + cid + " AND IsActive=1", conn);
                FillDDL(ddlColor, "SELECT Color_Sno, Color_Name FROM Color_Table WHERE Company_No=" + cid + " AND IsActive=1", conn);
                FillDDL(ddlWeave, "SELECT Weave_Sno, Weave_Name FROM WeaveType_Table WHERE Company_No=" + cid + " AND IsActive=1", conn);
                FillDDL(ddlGST, "SELECT GST_Sno, Tax_Name FROM GST_Table WHERE Company_No=" + cid + " AND IsActive=1", conn);

                DataTable dtU = new DataTable();
                new SqlDataAdapter("SELECT Unit_Sno, Unit_Sname FROM Unit_Table WHERE Company_No=" + cid + " AND IsActive=1", conn).Fill(dtU);
                ddlBaseUnit.DataSource = ddlAltUnit.DataSource = dtU;
                ddlBaseUnit.DataTextField = ddlAltUnit.DataTextField = "Unit_Sname";
                ddlBaseUnit.DataValueField = ddlAltUnit.DataValueField = "Unit_Sno";
                ddlBaseUnit.DataBind(); ddlAltUnit.DataBind();
                ddlBaseUnit.Items.Insert(0, new ListItem("-- Unit --", "0"));
                ddlAltUnit.Items.Insert(0, new ListItem("None", "0"));
            }
        }

        private void FillDDL(DropDownList ddl, string sql, SqlConnection conn)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, conn);
            DataTable dt = new DataTable(); da.Fill(dt);
            ddl.DataSource = dt; ddl.DataTextField = dt.Columns[1].ColumnName; ddl.DataValueField = dt.Columns[0].ColumnName;
            ddl.DataBind(); ddl.Items.Insert(0, new ListItem("-- Select --", "0"));
        }

        private void LoadList()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetItemList", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                    cmd.Parameters.AddWithValue("@SearchText", string.IsNullOrEmpty(txtSearch.Text) ? (object)DBNull.Value : txtSearch.Text.Trim());
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable(); da.Fill(dt);
                    gvItems.DataSource = dt; gvItems.DataBind();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtItemName.Text)) { Alert("Item Name is required", "error"); return; }
            try
            {
                string imagePath = imgPrev.ImageUrl;
                if (fuImage.HasFile)
                {
                    string ext = Path.GetExtension(fuImage.FileName).ToLower();
                    string filename = Guid.NewGuid().ToString() + ext;
                    string folder = Server.MapPath("~/Uploads/Items/");
                    if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                    fuImage.SaveAs(folder + filename);
                    imagePath = "~/Uploads/Items/" + filename;
                }

                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveItem", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        bool isUpdate = !string.IsNullOrEmpty(hfItemID.Value);

                        cmd.Parameters.AddWithValue("@Action", isUpdate ? "UPDATE" : "INSERT");
                        cmd.Parameters.AddWithValue("@Item_Sno", isUpdate ? Convert.ToInt32(hfItemID.Value) : 0);
                        cmd.Parameters.AddWithValue("@Company_No", Session["CompanyID"]);
                        cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? 0);

                        cmd.Parameters.AddWithValue("@Item_Type", ddlItemType.SelectedValue);
                        cmd.Parameters.AddWithValue("@Item_Name", txtItemName.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@Item_Code", txtItemCode.Text.Trim().ToUpper());
                        cmd.Parameters.AddWithValue("@HSN_Code", txtHSN.Text.Trim());
                        cmd.Parameters.AddWithValue("@Barcode", txtBarcode.Text.Trim());
                        cmd.Parameters.AddWithValue("@Item_Image", imagePath);
                        cmd.Parameters.AddWithValue("@ItemGroup_No", ddlGroup.SelectedValue);
                        cmd.Parameters.AddWithValue("@SubCategory_Sno", ddlSubCategory.SelectedValue);
                        cmd.Parameters.AddWithValue("@Brand_Sno", 0);
                        cmd.Parameters.AddWithValue("@Color_SNo", ddlColor.SelectedValue);
                        cmd.Parameters.AddWithValue("@Weave_Sno", ddlWeave.SelectedValue);
                        cmd.Parameters.AddWithValue("@ItemUnit_No", ddlBaseUnit.SelectedValue);
                        cmd.Parameters.AddWithValue("@AltUnit_No", ddlAltUnit.SelectedValue);
                        cmd.Parameters.AddWithValue("@Conv_Factor", txtConvFactor.Text);
                        cmd.Parameters.AddWithValue("@GST_Sno", ddlGST.SelectedValue);
                        cmd.Parameters.AddWithValue("@Purchase_Rate", txtPurRate.Text);
                        cmd.Parameters.AddWithValue("@Selling_Price", txtSalesPrice.Text);
                        cmd.Parameters.AddWithValue("@Min_Stock", txtMinStock.Text);
                        cmd.Parameters.AddWithValue("@Max_Stock", txtMaxStock.Text);
                        cmd.Parameters.AddWithValue("@Batch_Enabled", chkBatch.Checked);
                        cmd.Parameters.AddWithValue("@Serial_Enabled", chkSerial.Checked);
                        cmd.Parameters.AddWithValue("@IsActive", chkActive.Checked);
                        cmd.Parameters.AddWithValue("@OpeningQty", txtOpQty.Text);

                        if (conn.State == ConnectionState.Closed) conn.Open();
                        cmd.ExecuteNonQuery();

                        btnBack_Click(null, null);
                        Alert("Product details saved!", "success");
                    }
                }
            }
            catch (SqlException ex) { Alert(ex.Message, "error"); }
            catch (Exception ex) { Alert("Save Error: " + ex.Message, "error"); }
        }

        protected void gvItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                LoadDropdowns();

                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlCommand cmd = new SqlCommand("sp_GetItemByID", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Item_Sno", id);

                        if (conn.State == ConnectionState.Closed) conn.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                hfItemID.Value = id.ToString();
                                SafeSetSelectedValue(ddlItemType, dr["Item_Type"]);
                                SafeSetSelectedValue(ddlGroup, dr["ItemGroup_No"]);
                                SafeSetSelectedValue(ddlSubCategory, dr["SubCategory_Sno"]);
                                SafeSetSelectedValue(ddlColor, dr["Color_Sno"]);
                                SafeSetSelectedValue(ddlWeave, dr["Weave_Sno"]);
                                SafeSetSelectedValue(ddlBaseUnit, dr["ItemUnit_No"]);
                                SafeSetSelectedValue(ddlAltUnit, dr["AltUnit_No"]);
                                SafeSetSelectedValue(ddlGST, dr["GST_Sno"]);

                                txtItemName.Text = dr["Item_Name"].ToString();
                                txtItemCode.Text = dr["Item_Code"].ToString();
                                txtHSN.Text = dr["HSN_Code"].ToString();
                                txtBarcode.Text = dr["Barcode"].ToString();
                                txtConvFactor.Text = dr["Conv_Factor"].ToString();
                                txtSalesPrice.Text = dr["Selling_Price"].ToString();
                                txtPurRate.Text = dr["Purchase_Rate"].ToString();
                                txtMinStock.Text = dr["Min_Stock"].ToString();
                                txtMaxStock.Text = dr["Max_Stock"].ToString();
                                txtOpQty.Text = dr["OpeningQty"].ToString();
                                string imgUrl = dr["Item_Image"].ToString();
                                imgPrev.ImageUrl = string.IsNullOrEmpty(imgUrl) ? "~/Images/no-image.png" : imgUrl;
                                chkBatch.Checked = dr["Batch_Enabled"] != DBNull.Value && Convert.ToBoolean(dr["Batch_Enabled"]);
                                chkSerial.Checked = dr["Serial_Enabled"] != DBNull.Value && Convert.ToBoolean(dr["Serial_Enabled"]);
                                chkActive.Checked = dr["IsActive"] != DBNull.Value && Convert.ToBoolean(dr["IsActive"]);
                                pnlList.Visible = false;
                                pnlForm.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        private void SafeSetSelectedValue(DropDownList ddl, object dbValue)
        {
            if (ddl == null) return;

            ddl.SelectedIndex = 0;

            if (dbValue != null && dbValue != DBNull.Value)
            {
                string valToSet = dbValue.ToString().Trim();
                if (ddl.Items.FindByValue(valToSet) != null)
                {
                    ddl.SelectedValue = valToSet;
                }
            }
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e) 
        { 
            hfItemID.Value = ""; 
            ClearInputs(); 
            LoadDropdowns(); 
            pnlList.Visible = false; 
            pnlForm.Visible = true; 
        }
        protected void btnBack_Click(object sender, EventArgs e) 
        { 
            pnlList.Visible = true;
            pnlForm.Visible = false; 
            LoadList(); 
        }
        protected void txtSearch_TextChanged(object sender, EventArgs e) => LoadList();
        private void Alert(string msg, string type) 
        { 
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alert", $"showNotification('{msg.Replace("'", "\\'")}', '{type}');", true); 
        }
        private void ClearInputs() 
        { 
            txtItemName.Text = txtItemCode.Text = txtHSN.Text = txtBarcode.Text = ""; 
            txtOpQty.Text = txtPurRate.Text = txtSalesPrice.Text = "0"; 
            txtMinStock.Text = "0"; txtMaxStock.Text = "0"; 
            chkActive.Checked = true; 
            imgPrev.ImageUrl = "~/Images/no-image.png"; 
            ddlItemType.SelectedIndex = 0; 
        }
    }
}
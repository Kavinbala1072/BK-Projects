using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using BKBilling.Class;

namespace BKBilling.Forms.Settings
{
    public partial class ItemSetting : System.Web.UI.Page
    {
        // Headers updated to use names instead of IDs
        string[] headers = {
            "Item_Type", "Item_Name", "Item_Code", "HSN_Code", "Barcode",
            "Category_Name", "SubCategory_Name", "Color_Name", "Weave_Name",
            "BaseUnit_Symbol", "AltUnit_Symbol", "Conv_Factor", "GST_Label",
            "PurRate", "SalePrice", "OpeningQty"
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CompanyID"] == null) Response.Redirect("~/Login.aspx");
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join(",", headers));

            // Example row using NAMES
            sb.AppendLine("Item,COTTON SHIRT BLUE,SH001,6205,89012345,MEN'S WEAR,SHIRTS,BLUE,HANDLOOM,PCS,BOX,10,GST 18%,450,899,10");

            string filename = "Item_Import_Template_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.ContentType = "application/text";
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            if (!fuImport.HasFile) { Alert("Please select a file.", "error"); return; }
            int successCount = 0; int errorCount = 0;

            try
            {
                using (StreamReader reader = new StreamReader(fuImport.FileContent))
                {
                    reader.ReadLine(); // Skip Header
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(line)) continue;
                        string[] cols = line.Split(',');

                        if (cols.Length >= 16)
                        {
                            try
                            {
                                if (ProcessRow(cols)) successCount++; else errorCount++;
                            }
                            catch { errorCount++; }
                        }
                    }
                }
                Alert($"Import finished: {successCount} added, {errorCount} failed.", "success");
            }
            catch (Exception ex) { Alert(ex.Message, "error"); }
        }

        private bool ProcessRow(string[] data)
        {
            long cid = Convert.ToInt64(Session["CompanyID"]);
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                using (SqlCommand cmd = new SqlCommand("sp_SaveItem", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // --- THE LOOKUP LOGIC (Names to IDs) ---
                    int categoryId = GetLookupId(conn, cid, "ItemGroup_Table", "ItemGroup_Name", "ItemGroup_Sno", data[5]);
                    int subCatId = GetLookupId(conn, cid, "ItemSubCategory_Table", "SubCat_Name", "SubCat_Sno", data[6]);
                    int colorId = GetLookupId(conn, cid, "Color_Table", "Color_Name", "Color_Sno", data[7]);
                    int weaveId = GetLookupId(conn, cid, "WeaveType_Table", "Weave_Name", "Weave_Sno", data[8]);
                    int baseUnitId = GetLookupId(conn, cid, "Unit_Table", "Unit_Sname", "Unit_Sno", data[9]);
                    int altUnitId = GetLookupId(conn, cid, "Unit_Table", "Unit_Sname", "Unit_Sno", data[10]);
                    int gstId = GetLookupId(conn, cid, "GST_Table", "Tax_Name", "GST_Sno", data[12]);

                    // Standard Parameters
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@Item_Sno", 0);
                    cmd.Parameters.AddWithValue("@Company_No", cid);
                    cmd.Parameters.AddWithValue("@User_No", Session["UserID"] ?? 0);
                    cmd.Parameters.AddWithValue("@Item_Type", data[0]);
                    cmd.Parameters.AddWithValue("@Item_Name", data[1].ToUpper());
                    cmd.Parameters.AddWithValue("@Item_Code", data[2].ToUpper());
                    cmd.Parameters.AddWithValue("@HSN_Code", data[3]);
                    cmd.Parameters.AddWithValue("@Barcode", data[4]);

                    // Parameters using looked-up IDs
                    cmd.Parameters.AddWithValue("@ItemGroup_No", categoryId);
                    cmd.Parameters.AddWithValue("@SubCategory_Sno", subCatId);
                    cmd.Parameters.AddWithValue("@Brand_Sno", 0);
                    cmd.Parameters.AddWithValue("@Color_SNo", colorId);
                    cmd.Parameters.AddWithValue("@Weave_Sno", weaveId);
                    cmd.Parameters.AddWithValue("@ItemUnit_No", baseUnitId);
                    cmd.Parameters.AddWithValue("@AltUnit_No", altUnitId);
                    cmd.Parameters.AddWithValue("@Conv_Factor", data[11]);
                    cmd.Parameters.AddWithValue("@GST_Sno", gstId);

                    cmd.Parameters.AddWithValue("@Purchase_Rate", data[13]);
                    cmd.Parameters.AddWithValue("@Selling_Price", data[14]);
                    cmd.Parameters.AddWithValue("@IsActive", 1);
                    cmd.Parameters.AddWithValue("@OpeningQty", data[15]);

                    // Advanced Parameters
                    cmd.Parameters.AddWithValue("@Item_Image", "");
                    cmd.Parameters.AddWithValue("@Min_Stock", 0);
                    cmd.Parameters.AddWithValue("@Max_Stock", 0);
                    cmd.Parameters.AddWithValue("@Batch_Enabled", 0);
                    cmd.Parameters.AddWithValue("@Serial_Enabled", 0);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
        }

        // Generic Helper to find ID from Name
        private int GetLookupId(SqlConnection conn, long cid, string table, string nameCol, string idCol, string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "0" || value.ToUpper() == "NONE") return 0;

            string sql = $"SELECT TOP 1 {idCol} FROM {table} WHERE Company_No = @cid AND {nameCol} = @name";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@cid", cid);
                cmd.Parameters.AddWithValue("@name", value.Trim());
                object result = cmd.ExecuteScalar();
                return (result != null) ? Convert.ToInt32(result) : 0;
            }
        }

        private void Alert(string msg, string type)
        {
            string script = $"showNotification('{msg.Replace("'", "\\'")}', '{type}');";
            ScriptManager.RegisterStartupScript(this, GetType(), "msg", script, true);
        }
    }
}
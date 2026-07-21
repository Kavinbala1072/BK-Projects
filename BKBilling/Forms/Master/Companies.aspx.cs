using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using BKBilling.Class;

namespace BKBilling.Forms.Master
{
    public partial class Companies : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) { SetDefaults(); LoadList(); }
        }

        private void SetDefaults()
        {
            C_FY.Text = new DateTime(DateTime.Now.Year, 4, 1).ToString("yyyy-MM-dd");
            U_JoinDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            C_CurSym.Text = "₹";
            C_Country.Text = "India";
        }

        private void LoadList()
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    string sql = "SELECT Company_Sno, Company_Name, GSTIN, Phone, Created_Date FROM Company_Table ORDER BY Company_Sno DESC";
                    using (SqlDataAdapter da = new SqlDataAdapter(sql, conn))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        gvCompanies.DataSource = dt;
                        gvCompanies.DataBind();
                    }
                }
            }
            catch (Exception ex) { Alert("Error: " + ex.Message, "error"); }
        }

        protected void btnOpenCreate_Click(object sender, EventArgs e) { ClearInputs(); pnlList.Visible = false; pnlForm.Visible = true; }
        protected void btnBack_Click(object sender, EventArgs e) { pnlList.Visible = true; pnlForm.Visible = false; LoadList(); }

        protected void gvCompanies_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditRecord") { LoadDataForEdit(Convert.ToInt64(e.CommandArgument)); }
        }

        private void LoadDataForEdit(long sno)
        {
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    string sql = @"SELECT c.*, c.Address_1 AS CompAdd1, c.Address_2 AS CompAdd2, c.Phone AS CompPhone, c.Email AS CompEmail,
                                    u.Username AS AdminUser, u.FullName AS AdminName, u.Phone AS AdminPhone, 
                                    u.Email AS AdminEmail, u.Address_1 AS AdminAdd1, u.Address_2 AS AdminAdd2, u.Join_Date AS AdminJoinDate
                                   FROM Company_Table c 
                                   LEFT JOIN User_Table u ON c.Company_Sno = u.Company_No AND u.Role='Admin'
                                   WHERE c.Company_Sno = @sno";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@sno", sno);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                hfCompanySno.Value = sno.ToString();
                                C_Name.Text = dr["Company_Name"].ToString();
                                C_GST.Text = dr["GSTIN"].ToString();
                                C_PAN.Text = dr["PAN"].ToString();
                                C_FY.Text = dr["Financial_Year"] != DBNull.Value ? Convert.ToDateTime(dr["Financial_Year"]).ToString("yyyy-MM-dd") : "";
                                C_CurSym.Text = dr["Currency_Symbol"].ToString();
                                C_CurFmt.SelectedValue = dr["Currency_Format"].ToString();
                                C_State.Text = dr["State_Name"].ToString();
                                C_Country.Text = dr["Country"].ToString();
                                C_Phone.Text = dr["CompPhone"].ToString();
                                C_Email.Text = dr["CompEmail"].ToString();
                                C_Add1.Text = dr["CompAdd1"].ToString();
                                C_Add2.Text = dr["CompAdd2"].ToString();
                                U_FullName.Text = dr["AdminName"].ToString();
                                U_Username.Text = dr["AdminUser"].ToString();
                                U_Phone.Text = dr["AdminPhone"].ToString();
                                U_Email.Text = dr["AdminEmail"].ToString();
                                U_Add1.Text = dr["AdminAdd1"].ToString();
                                U_Add2.Text = dr["AdminAdd2"].ToString();
                                U_JoinDate.Text = dr["AdminJoinDate"] != DBNull.Value ? Convert.ToDateTime(dr["AdminJoinDate"]).ToString("yyyy-MM-dd") : "";
                                U_Pass.Text = U_Confirm.Text = "";
                                pnlList.Visible = false; pnlForm.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Alert("Load Error: " + ex.Message, "error"); }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(C_Name.Text) || string.IsNullOrWhiteSpace(U_Username.Text)) { Alert("Company Name and Username are required!", "error"); return; }
            bool isUpdate = !string.IsNullOrEmpty(hfCompanySno.Value);
            if (!isUpdate || !string.IsNullOrWhiteSpace(U_Pass.Text))
            {
                if (U_Pass.Text != U_Confirm.Text) { Alert("Passwords do not match!", "error"); return; }
                if (!isUpdate && string.IsNullOrWhiteSpace(U_Pass.Text)) { Alert("Password required for new accounts!", "error"); return; }
            }

            long savedCid = 0;
            try
            {
                using (SqlConnection conn = DbHelper.GetConnection())
                {
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string compSql = isUpdate ?
                                @"UPDATE Company_Table SET Company_Name=@c1, Address_1=@c2, Address_2=@c3, State_Name=@c4, GSTIN=@c5, PAN=@c6, Country=@c7, Phone=@c8, Email=@c9, Currency_Symbol=@c11, Currency_Format=@c12, Financial_Year=@c13, Modified_Date=GETDATE() WHERE Company_Sno=@id" :
                                @"INSERT INTO Company_Table (Company_Name, Address_1, Address_2, State_Name, GSTIN, PAN, Country, Phone, Email, Currency_Symbol, Currency_Format, Financial_Year, Created_Date) VALUES (@c1, @c2, @c3, @c4, @c5, @c6, @c7, @c8, @c9, @c11, @c12, @c13, GETDATE()); SELECT SCOPE_IDENTITY();";

                            using (SqlCommand cmdC = new SqlCommand(compSql, conn, trans))
                            {
                                cmdC.Parameters.AddWithValue("@c1", C_Name.Text.Trim());
                                cmdC.Parameters.AddWithValue("@c2", C_Add1.Text.Trim());
                                cmdC.Parameters.AddWithValue("@c3", C_Add2.Text.Trim());
                                cmdC.Parameters.AddWithValue("@c4", C_State.Text.Trim());
                                cmdC.Parameters.AddWithValue("@c5", C_GST.Text.Trim());
                                cmdC.Parameters.AddWithValue("@c6", C_PAN.Text.Trim());
                                cmdC.Parameters.AddWithValue("@c7", C_Country.Text.Trim());
                                cmdC.Parameters.AddWithValue("@c8", C_Phone.Text.Trim());
                                cmdC.Parameters.AddWithValue("@c9", C_Email.Text.Trim());
                                cmdC.Parameters.AddWithValue("@c11", C_CurSym.Text.Trim());
                                cmdC.Parameters.AddWithValue("@c12", C_CurFmt.SelectedValue);
                                cmdC.Parameters.AddWithValue("@c13", string.IsNullOrEmpty(C_FY.Text) ? (object)DBNull.Value : C_FY.Text);
                                if (isUpdate) { cmdC.Parameters.AddWithValue("@id", hfCompanySno.Value); cmdC.ExecuteNonQuery(); savedCid = Convert.ToInt64(hfCompanySno.Value); }
                                else { savedCid = Convert.ToInt64(cmdC.ExecuteScalar()); }
                            }

                            string userSql = isUpdate ?
                                @"UPDATE User_Table SET Username=@u2, FullName=@u4, Address_1=@u5, Address_2=@u52, Phone=@u6, Email=@u8, Join_Date=@u9, Modified_Date=GETDATE() " + (!string.IsNullOrWhiteSpace(U_Pass.Text) ? ", Password=@u3" : "") + " WHERE Company_No=@u1 AND Role='Admin'" :
                                @"INSERT INTO User_Table (Company_No, Username, Password, FullName, Role, IsActive, Address_1, Address_2, Phone, Email, Join_Date, Created_Date) VALUES (@u1, @u2, @u3, @u4, 'Admin', 1, @u5, @u52, @u6, @u8, @u9, GETDATE())";
                            using (SqlCommand cmdU = new SqlCommand(userSql, conn, trans))
                            {
                                cmdU.Parameters.AddWithValue("@u1", savedCid);
                                cmdU.Parameters.AddWithValue("@u2", U_Username.Text.Trim());
                                cmdU.Parameters.AddWithValue("@u4", U_FullName.Text.Trim());
                                cmdU.Parameters.AddWithValue("@u5", U_Add1.Text.Trim());
                                cmdU.Parameters.AddWithValue("@u52", U_Add2.Text.Trim());
                                cmdU.Parameters.AddWithValue("@u6", U_Phone.Text.Trim());
                                cmdU.Parameters.AddWithValue("@u8", U_Email.Text.Trim());
                                cmdU.Parameters.AddWithValue("@u9", string.IsNullOrEmpty(U_JoinDate.Text) ? (object)DBNull.Value : U_JoinDate.Text);
                                if (!isUpdate || !string.IsNullOrWhiteSpace(U_Pass.Text)) cmdU.Parameters.AddWithValue("@u3", SecurityHelper.ComputeHash(U_Pass.Text));
                                cmdU.ExecuteNonQuery();
                            }

                            string settingsSql = @"INSERT INTO Form_Settings (Company_ID, Form_Name, Control_ID, Is_Enabled)
                                SELECT @cid, t.Form_Name, t.Control_ID, 1
                                FROM (VALUES ('Sales Invoice', 'liSalesOrder'), ('Sales Order', 'liSales'), ('Sales Return', 'liSalesReturn'), ('Quotation', 'liQuotation'), ('Purchase Invoice', 'liPurchase'), ('Purchase Order', 'liPurOrder'), ('Purchase Return', 'liPurReturn'), ('Voucher Entry', 'liVoucher'), ('Stock Adjustment', 'liAdjustment'), ('Branch Transfer', 'liBTransfer'), ('Ledger Creation', 'liLedger'), ('Ledger Groups', 'liGroupMaster'), ('Area Master', 'liAreaMaster'), ('Item Creation', 'liItem'), ('Item Group Master', 'liItemGroup'), ('Item Brand Master', 'liBrand'), ('Item Model Master', 'liModel'), ('Item Unit', 'liUOM'), ('Unit Conversion', 'liUnitConv'), ('Closing Stock Entry', 'liCStock'), ('Godown Master', 'liGodown'), ('DayBook Report', 'liDayBook'), ('Ledger Statement', 'liLedgerRep'), ('Cash / Bank Book', 'liCashBank'), ('Trial Balance', 'liTrial'), ('Balance Sheet', 'liBS'), ('Profit & Loss', 'liPL'), ('Stock Summary', 'liStockSum'), ('Stock Detail (In/Out)', 'liStockDet'), ('GSTR-1 (Sales)', 'liGSTR1'), ('GSTR-2 (Purchase)', 'liGSTR2'), ('GSTR-3B Summary', 'liGSTR3B'), ('HSN Summary', 'liHSN'), ('Company Setting', 'liCompSett'), ('Item Setting', 'liItemSett'), ('Form Setup', 'liFormSet'), ('User Creation', 'liUser'), ('Company Creation', 'liCompany')) AS t(Form_Name, Control_ID)
                                WHERE NOT EXISTS (SELECT 1 FROM Form_Settings fs WHERE fs.Company_ID = @cid AND fs.Control_ID = t.Control_ID);";
                            using (SqlCommand cmdS = new SqlCommand(settingsSql, conn, trans))
                            {
                                cmdS.Parameters.AddWithValue("@cid", savedCid);
                                cmdS.ExecuteNonQuery();
                            }
                            trans.Commit();
                        }
                        catch { trans.Rollback(); throw; }
                    }
                }

                if (hfInitWorkspace.Value == "true")
                {
                    TablesCreation.CreateBusinessSchema(savedCid);
                    Alert("Record saved and Workspace initialized/updated successfully!", "success");
                }
                else
                {
                    Alert("Record saved successfully!", "success");
                }
                btnBack_Click(null, null);
            }
            catch (Exception ex) { Alert("Save Error: " + ex.Message, "error"); }
        }

        private void Alert(string msg, string type = "success")
        {
            string script = $"showNotification('{msg.Replace("'", "\\'")}', '{type}');";
            ScriptManager.RegisterStartupScript(this, GetType(), Guid.NewGuid().ToString(), script, true);
        }

        private void ClearInputs() { hfCompanySno.Value = ""; C_Name.Text = C_GST.Text = C_PAN.Text = C_Add1.Text = C_Add2.Text = C_State.Text = C_Phone.Text = C_Email.Text = ""; U_FullName.Text = U_Username.Text = U_Pass.Text = U_Confirm.Text = U_Phone.Text = U_Email.Text = U_Add1.Text = U_Add2.Text = ""; SetDefaults(); }
    }
}
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using BKBilling.Class;

namespace BKBilling.Forms.Transaction
{
    public partial class BTransferForm : System.Web.UI.Page
    {
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                txtDocNo.Text = "TRF-" + DateTime.Now.ToString("ddMMyy-HHmm");

                //BindBranches();
                //BindProducts();
                InitializeTable();
            }
        }

        private void InitializeTable()
        {
            dt = new DataTable();
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Qty", typeof(decimal));
            dt.Columns.Add("Rate", typeof(decimal));
            dt.Columns.Add("Taxable", typeof(decimal));
            dt.Columns.Add("GST", typeof(decimal));
            dt.Columns.Add("Total", typeof(decimal));
            ViewState["TransferItems"] = dt;
        }

        private void BindBranches()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                SqlDataAdapter adp = new SqlDataAdapter("SELECT Company_Sno, Company_Name FROM Company_Table", conn);
                DataTable dtB = new DataTable();
                adp.Fill(dtB);

                ddlSourceBranch.DataSource = dtB;
                ddlSourceBranch.DataTextField = "Company_Name";
                ddlSourceBranch.DataValueField = "Company_Sno";
                ddlSourceBranch.DataBind();
                ddlSourceBranch.Items.Insert(0, new ListItem("-- From Branch --", "0"));

                ddlDestBranch.DataSource = dtB;
                ddlDestBranch.DataTextField = "Company_Name";
                ddlDestBranch.DataValueField = "Company_Sno";
                ddlDestBranch.DataBind();
                ddlDestBranch.Items.Insert(0, new ListItem("-- To Branch --", "0"));
            }
        }

        private void BindProducts()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                SqlDataAdapter adp = new SqlDataAdapter("SELECT Item_Sno, Item_Name FROM Item_Master", conn);
                DataTable dtP = new DataTable();
                adp.Fill(dtP);
                ddlProduct.DataSource = dtP;
                ddlProduct.DataTextField = "Item_Name";
                ddlProduct.DataValueField = "Item_Sno";
                ddlProduct.DataBind();
            }
        }

        protected void Branch_Changed(object sender, EventArgs e)
        {
            // Logic to fetch GSTIN from Company_Table based on selection
            UpdateGSTDisplay();
        }

        private void UpdateGSTDisplay()
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                // Fetch Source GST
                if (ddlSourceBranch.SelectedValue != "0")
                    litSourceGST.Text = GetBranchGST(ddlSourceBranch.SelectedValue, conn);

                // Fetch Destination GST
                if (ddlDestBranch.SelectedValue != "0")
                    litDestGST.Text = GetBranchGST(ddlDestBranch.SelectedValue, conn);
            }
        }

        private string GetBranchGST(string id, SqlConnection conn)
        {
            string sql = "SELECT GSTIN FROM Company_Table WHERE Company_Sno = @id";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            object res = cmd.ExecuteScalar();
            return res != null ? res.ToString() : "N/A";
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            dt = (DataTable)ViewState["TransferItems"];
            decimal qty = Convert.ToDecimal(txtQty.Text);
            decimal rate = Convert.ToDecimal(txtRate.Text);
            decimal gstPer = Convert.ToDecimal(ddlGST.SelectedValue);

            decimal taxable = qty * rate;
            decimal taxAmt = (taxable * gstPer) / 100;

            DataRow dr = dt.NewRow();
            dr["ItemID"] = ddlProduct.SelectedValue;
            dr["ItemName"] = ddlProduct.SelectedItem.Text;
            dr["Qty"] = qty;
            dr["Rate"] = rate;
            dr["Taxable"] = taxable;
            dr["GST"] = gstPer;
            dr["Total"] = taxable + taxAmt;
            dt.Rows.Add(dr);

            ViewState["TransferItems"] = dt;
            BindGrid();
        }

        private void BindGrid()
        {
            dt = (DataTable)ViewState["TransferItems"];
            gvItems.DataSource = dt;
            gvItems.DataBind();
            CalculateTotals(dt);
        }

        private void CalculateTotals(DataTable table)
        {
            decimal taxableSum = 0;
            decimal totalSum = 0;

            foreach (DataRow dr in table.Rows)
            {
                taxableSum += Convert.ToDecimal(dr["Taxable"]);
                totalSum += Convert.ToDecimal(dr["Total"]);
            }

            decimal totalTax = totalSum - taxableSum;
            lblSubTotal.Text = taxableSum.ToString("N2");

            if (ddlTransferType.SelectedValue == "Intra")
            {
                lblCGST.Text = (totalTax / 2).ToString("N2");
                lblSGST.Text = (totalTax / 2).ToString("N2");
                lblIGST.Text = "0.00";
            }
            else
            {
                lblCGST.Text = "0.00";
                lblSGST.Text = "0.00";
                lblIGST.Text = totalTax.ToString("N2");
            }
            litGrandTotal.Text = totalSum.ToString("N2");
        }

        protected void gvItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            dt = (DataTable)ViewState["TransferItems"];
            dt.Rows[e.RowIndex].Delete();
            ViewState["TransferItems"] = dt;
            BindGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlSourceBranch.SelectedValue == ddlDestBranch.SelectedValue)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "err", "alert('Source and Destination cannot be same.');", true);
                return;
            }
            // Logic to insert into TransferMaster and TransferDetails
            // And trigger Stock Movement (Source -, Dest +)
        }
    }
}
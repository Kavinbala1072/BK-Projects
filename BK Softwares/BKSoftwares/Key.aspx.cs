using System;

namespace BKSoftwares
{
    public partial class Key : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Error is cleared on button click, not every load to allow visibility
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            lblError.Text = ""; // Reset error state
            pnlResult.Visible = false;

            try
            {
                // 1. Validate Hardware Key
                string hardwareKey = txtHardwareKey.Text.Trim();
                if (string.IsNullOrEmpty(hardwareKey))
                {
                    lblError.Text = "Hardware Key is required.";
                    return;
                }

                // 2. Validate Date
                DateTime actDate;
                if (!DateTime.TryParse(txtDate.Text, out actDate))
                {
                    lblError.Text = "Please select a valid date.";
                    return;
                }

                // 3. Process Parts
                string[] parts = hardwareKey.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length != 3)
                {
                    lblError.Text = "Format Error: Use '12345 67890 11123'";
                    return;
                }

                int base1, base2, base3;
                if (int.TryParse(parts[0], out base1) &&
                    int.TryParse(parts[1], out base2) &&
                    int.TryParse(parts[2], out base3))
                {
                    // Logic from VB.NET: Base + Day, Base + Month, Base + Year
                    int day = actDate.Day;
                    int month = actDate.Month;
                    int year = actDate.Year;

                    int final1 = base1 + day;
                    int final2 = base2 + month;
                    int final3 = base3 + year;

                    // Display
                    lblActivationKey.Text = $"{final1} {final2} {final3}";
                    lblMessage.Text = $"Generated for {actDate.ToString("dd MMM yyyy")}";
                    pnlResult.Visible = true;
                }
                else
                {
                    lblError.Text = "Numbers only, please.";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "System Error: " + ex.Message;
            }
        }
    }
}
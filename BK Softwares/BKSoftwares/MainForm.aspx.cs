using System;
using System.Web;
using System.Web.UI;

namespace BKSoftwares
{
    public partial class MainForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            Response.AppendHeader("Pragma", "no-cache");

            if (!IsPostBack)
            {
                if (Session["UserName"] == null || Session["AuthToken"] == null)
                {
                    Response.Redirect("AppLogin.aspx");
                    return;
                }

                string user = Session["UserName"].ToString();
                litUsername.Text = user;
                litWelcomeUser.Text = user;
                litCompName.Text = "BK SOFTWARES MANAGEMENT SYSTEM";

                ShowWelcome();
            }
        }

        private void LoadPage(string url)
        {
            pnlWelcome.Visible = false;
            ifrReport.Visible = true;
            ifrReport.Attributes["src"] = url;
        }

        private void ShowWelcome()
        {
            pnlWelcome.Visible = true;
            ifrReport.Visible = false;
        }

        protected void btnMenuDash_Click(object sender, EventArgs e) => ShowWelcome();
        protected void btnMenuCust_Click(object sender, EventArgs e) => LoadPage("Customers.aspx");
        protected void btnMenuVoucher_Click(object sender, EventArgs e) => LoadPage("Vouchers.aspx");
        protected void btnRepCust_Click(object sender, EventArgs e) => LoadPage("CustomerList.aspx");
        protected void btnOutstanding_Click(object sender, EventArgs e) => LoadPage("Outstanding.aspx");
        protected void btnKey_Click(object sender, EventArgs e) => LoadPage("Key.aspx");
        protected void btnRLogin_Click(object sender, EventArgs e) => LoadPage("ReportingLogin.aspx");


        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();

            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();

            Response.Redirect("AppLogin.aspx");
        }
    }
}
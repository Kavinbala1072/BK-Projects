Imports System.Web

Public Class MainForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1))
        Response.Cache.SetNoStore()

        UserGuard.ValidateSession(Me)

        If Not IsPostBack Then
            Dim user As String = If(Session("UserName"), "User").ToString()
            litUsername.Text = user
            litWelcomeUser.Text = user

            If Request.Cookies("Selected_CompName") IsNot Nothing Then
                litCompName.Text = Server.UrlDecode(Request.Cookies("Selected_CompName").Value)
            Else
                litCompName.Text = "None"
            End If

            pnlWelcome.Visible = True
            ifrReport.Visible = False
        End If
    End Sub

    Private Sub LoadReport(ByVal url As String)
        pnlWelcome.Visible = False
        ifrReport.Visible = True
        ifrReport.Attributes("src") = url
    End Sub

#Region "Navigation Clicks"

    Protected Sub btnMenuDash_Click(sender As Object, e As EventArgs)
        LoadReport("Dashboard.aspx")
    End Sub

    Protected Sub btnMenuLedger_Click(sender As Object, e As EventArgs)
        LoadReport("LedgerForm.aspx")
    End Sub

    Protected Sub btnMenuOutstandingPayable_Click(sender As Object, e As EventArgs)
        LoadReport("Outstanding.aspx")
    End Sub

    Protected Sub btnMenuStock_Click(sender As Object, e As EventArgs)
        LoadReport("StockForm.aspx")
    End Sub

    Protected Sub btnMenuSales_Click(sender As Object, e As EventArgs)
        LoadReport("SalesForm.aspx")
    End Sub

    Protected Sub btnMenuPurchase_Click(sender As Object, e As EventArgs)
        LoadReport("PurchaseForm.aspx")
    End Sub

    Protected Sub btnLedger_Click(sender As Object, e As EventArgs)
        LoadReport("LedgerDetails.aspx")
    End Sub

    Protected Sub btnItem_Click(sender As Object, e As EventArgs)
        LoadReport("ItemDetails.aspx")
    End Sub

    Protected Sub btnMenuTrialBalance_Click(sender As Object, e As EventArgs)
        LoadReport("TrialBalance.aspx")
    End Sub

    Protected Sub btnMenuProfitLoss_Click(sender As Object, e As EventArgs)
        LoadReport("ProfitandlossForm.aspx")
    End Sub

    Protected Sub btnMenuBalanceSheet_Click(sender As Object, e As EventArgs)
        LoadReport("BalanceSheet.aspx")
    End Sub

#End Region

    Protected Sub lnkCompany_Click(sender As Object, e As EventArgs)
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub btnLogout_Click(sender As Object, e As EventArgs)
        Dim uCode As String = If(Session("UserName"), "").ToString()
        UserGuard.UpdateStatus(uCode, "")
        Session.Clear()
        Session.Abandon()

        Dim cookies = {"SQL_Server", "SQL_DB", "SQL_User", "SQL_Pass", "Selected_CompNo", "Selected_CompName"}
        For Each c In cookies
            If Request.Cookies(c) IsNot Nothing Then
                Dim myC As New HttpCookie(c)
                myC.Expires = DateTime.Now.AddDays(-1)
                myC.Path = "/"
                Response.Cookies.Add(myC)
            End If
        Next
        Response.Redirect("Default.aspx")
    End Sub
End Class
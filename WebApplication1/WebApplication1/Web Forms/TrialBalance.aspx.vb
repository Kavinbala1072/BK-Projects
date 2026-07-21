Imports System.Data
Imports System.Data.SqlClient

Public Class TrialBalance
    Inherits System.Web.UI.Page

    Dim compNo As String = "ED1"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserName") Is Nothing OrElse Request.Cookies("SQL_Server") Is Nothing Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Request.Cookies("Selected_CompNo") IsNot Nothing Then
            compNo = Request.Cookies("Selected_CompNo").Value
        End If

        If Not IsPostBack Then
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
            LoadTrialBalance()
        End If
    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadTrialBalance()
    End Sub

    Private Sub LoadTrialBalance()
        Using conn As New SqlConnection(GetDBConn())

            Dim asOnDate As DateTime = DateTime.Parse(txtDate.Text)
            Dim finYearStart As String = If(asOnDate.Month >= 4, asOnDate.Year.ToString(), (asOnDate.Year - 1).ToString()) & "-04-01"

            Dim sql As String = "WITH Balances AS ( " &
                "SELECT l.ledger_sno, l.ledger_name, " &
                "ISNULL(SUM(CASE WHEN v.vch_date < @finStart THEN (v.Dr_amount - v.Cr_amount) ELSE 0 END), 0) as OpeningNet, " &
                "ISNULL(SUM(CASE WHEN v.vch_date >= @finStart AND v.vch_date <= @asOn THEN v.Dr_amount ELSE 0 END), 0) as PeriodDr, " &
                "ISNULL(SUM(CASE WHEN v.vch_date >= @finStart AND v.vch_date <= @asOn THEN v.Cr_amount ELSE 0 END), 0) as PeriodCr " &
                "FROM ledger_table l " &
                "LEFT JOIN vch_table v ON l.ledger_sno = v.ledger_no AND v.comp_no = l.comp_no AND v.vch_cancel='false' " &
                "WHERE l.comp_no = @cp " &
                "GROUP BY l.ledger_sno, l.ledger_name " &
                ") " &
                "SELECT ledger_name as AccountName, " &
                "CASE WHEN OpeningNet > 0 THEN OpeningNet ELSE 0 END as OpDr, " &
                "CASE WHEN OpeningNet < 0 THEN ABS(OpeningNet) ELSE 0 END as OpCr, " &
                "PeriodDr as TrDr, PeriodCr as TrCr, " &
                "CASE WHEN (OpeningNet + PeriodDr - PeriodCr) > 0 THEN (OpeningNet + PeriodDr - PeriodCr) ELSE 0 END as ClDr, " &
                "CASE WHEN (OpeningNet + PeriodDr - PeriodCr) < 0 THEN ABS(OpeningNet + PeriodDr - PeriodCr) ELSE 0 END as ClCr " &
                "FROM Balances "

            ' Filter zero balances if checked
            If chkHideZero.Checked Then
                sql &= " WHERE (OpeningNet <> 0 OR PeriodDr <> 0 OR PeriodCr <> 0) "
            End If

            sql &= " ORDER BY ledger_name "

            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@cp", compNo)
            cmd.Parameters.AddWithValue("@finStart", finYearStart)
            cmd.Parameters.AddWithValue("@asOn", txtDate.Text)

            Try
                conn.Open()
                Dim dt As New DataTable()
                dt.Load(cmd.ExecuteReader())

                ' Calculate Grand Totals for Footer
                Dim sumOpDr As Decimal = 0, sumOpCr As Decimal = 0
                Dim sumTrDr As Decimal = 0, sumTrCr As Decimal = 0
                Dim sumClDr As Decimal = 0, sumClCr As Decimal = 0

                For Each row As DataRow In dt.Rows
                    sumOpDr += Convert.ToDecimal(row("OpDr"))
                    sumOpCr += Convert.ToDecimal(row("OpCr"))
                    sumTrDr += Convert.ToDecimal(row("TrDr"))
                    sumTrCr += Convert.ToDecimal(row("TrCr"))
                    sumClDr += Convert.ToDecimal(row("ClDr"))
                    sumClCr += Convert.ToDecimal(row("ClCr"))
                Next

                ' Bind Grid
                gvTrialBalance.DataSource = dt
                gvTrialBalance.DataBind()

                ' Update Footer Literals
                litSumOpDr.Text = sumOpDr.ToString("N2")
                litSumOpCr.Text = sumOpCr.ToString("N2")
                litSumTrDr.Text = sumTrDr.ToString("N2")
                litSumTrCr.Text = sumTrCr.ToString("N2")
                litSumClDr.Text = sumClDr.ToString("N2")
                litSumClCr.Text = sumClCr.ToString("N2")

            Catch ex As Exception
                lblStatus.Text = "Error calculating report: " & ex.Message
            End Try
        End Using
    End Sub

    Private Function GetDBConn() As String
        Try
            Dim s = Server.UrlDecode(Request.Cookies("SQL_Server").Value).Replace("\\", "\")
            Dim d = Request.Cookies("SQL_DB").Value
            Dim u = Request.Cookies("SQL_User").Value
            Dim p = Server.UrlDecode(Request.Cookies("SQL_Pass").Value)
            Return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", s, d, u, p)
        Catch
            Return ""
        End Try
    End Function

    Protected Sub chkHideZero_CheckedChanged(sender As Object, e As EventArgs) Handles chkHideZero.CheckedChanged
        LoadTrialBalance()
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs)
        Response.Write("<script>window.top.location.href='MainForm.aspx';</script>")
    End Sub
End Class
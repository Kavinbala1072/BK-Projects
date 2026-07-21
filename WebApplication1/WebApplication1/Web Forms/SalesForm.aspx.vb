Imports System.Data
Imports System.Data.SqlClient

Public Class SalesForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1))
        Response.Cache.SetNoStore()

        If Session("UserName") Is Nothing OrElse Request.Cookies("SQL_Server") Is Nothing Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Not IsPostBack Then
            Dim fyStart As String = If(Request.Cookies("Selected_PeriodFrom") IsNot Nothing, Request.Cookies("Selected_PeriodFrom").Value, "2025-04-01")
            Dim fyEnd As String = If(Request.Cookies("Selected_PeriodTo") IsNot Nothing, Request.Cookies("Selected_PeriodTo").Value, "2026-03-31")

            'txtFromDate.Text = New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("yyyy-MM-dd")
            txtFromDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd")

            txtFromDate.Attributes("min") = fyStart
            txtFromDate.Attributes("max") = fyEnd
            txtToDate.Attributes("min") = fyStart
            txtToDate.Attributes("max") = fyEnd

            BindSalesData()
        End If
    End Sub

    Private Sub BindSalesData()
        Try
            Dim s = Server.UrlDecode(Request.Cookies("SQL_Server").Value).Replace("\\", "\")
            Dim d = Request.Cookies("SQL_DB").Value
            Dim u = Request.Cookies("SQL_User").Value
            Dim p = Server.UrlDecode(Request.Cookies("SQL_Pass").Value)
            Dim connStr = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", s, d, u, p)

            Dim compNo As String = If(Request.Cookies("Selected_CompNo") IsNot Nothing, Request.Cookies("Selected_CompNo").Value, "ED1")

            Using conn As New SqlConnection(connStr)
                Dim query As String = "
                    SELECT 
                        SMas_BillNo      AS [Sales No],
                        SMas_BillDate    AS [Sales Date],
                        ISNULL(L.Ledger_Name,'') AS [Party Name],
                        ISNULL(A.Area_Name,'')   AS [Area],
                        ISNULL(smas_Gst,'')      AS [GSTNO],
                        CASE WHEN SMas_Cashmode = 0 THEN 'Cash' ELSE 'Credit' END AS [Cash Mode],
                        smas_qty         AS [Total Qty],
                        smas_gross       AS [Gross],
                        SMas_netamount   AS [NetAmount]
                    FROM SalMas_Table S
                    LEFT JOIN Ledger_table L ON S.party_no = L.Ledger_Sno AND S.Comp_No = L.Comp_No
                    LEFT JOIN Area_Table A ON A.Area_Sno = L.Area_No AND A.Comp_No = L.Comp_No
                    WHERE smas_billno <> '' AND smas_cancel = 'false'
                      AND SMas_BillDate BETWEEN @from AND @to
                      AND S.Comp_No = @compNo
                    ORDER BY SMas_BillDate, SMas_BillNo"

                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@from", txtFromDate.Text)
                cmd.Parameters.AddWithValue("@to", txtToDate.Text)
                cmd.Parameters.AddWithValue("@compNo", compNo)

                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable()
                da.Fill(dt)

                gvSales.DataSource = dt
                gvSales.DataBind()

                If dt.Rows.Count > 0 Then
                    litCount.Text = dt.Rows.Count.ToString()
                    litQty.Text = Convert.ToDecimal(If(IsDBNull(dt.Compute("SUM([Total Qty])", "")), 0, dt.Compute("SUM([Total Qty])", ""))).ToString("N2")
                    litGross.Text = Convert.ToDecimal(If(IsDBNull(dt.Compute("SUM([Gross])", "")), 0, dt.Compute("SUM([Gross])", ""))).ToString("N2")
                    litNet.Text = Convert.ToDecimal(If(IsDBNull(dt.Compute("SUM([NetAmount])", "")), 0, dt.Compute("SUM([NetAmount])", ""))).ToString("N2")
                Else
                    litCount.Text = "0" : litQty.Text = "0.00" : litGross.Text = "0.00" : litNet.Text = "0.00"
                End If
            End Using
        Catch ex As Exception
            'lblStatus.Text = "Error loading data."
        End Try
    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        BindSalesData()
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Write("<script>window.top.location.href='MainForm.aspx';</script>")
    End Sub
End Class
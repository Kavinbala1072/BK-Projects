Imports System.Data
Imports System.Data.SqlClient

Public Class PurchaseForm
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

            BindPurchaseData()
        End If
    End Sub

    Private Sub BindPurchaseData()
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
                        Purmas_table.Pmas_BillNo, 
                        Purmas_table.Pmas_VchNo,
                        Purmas_table.Pmas_BillDate, 
                        Ledger_table.ledger_name, 
                        Purmas_table.Pmas_gross, 
                        Purmas_table.Pmas_NetAmount, 
                        ISNULL((SELECT SUM(ptrn_qty) FROM purtrn_table WHERE ptrn_PmasSNo=pmas_sno AND purtrn_table.Comp_no = Purmas_table.Comp_no), 0) as purQty
                    FROM PurMas_Table  
                    INNER JOIN Ledger_Table ON Ledger_table.ledger_sno = Purmas_table.Pmas_Partyno AND Ledger_table.Comp_no = Purmas_table.Comp_no  
                    WHERE PMas_Type = 0  
                      AND purmas_Table.pmas_vchDate BETWEEN @from AND @to 
                      AND Purmas_Table.Comp_No = @compNo 
                      AND PMas_Cancel = 0 
                    ORDER BY PMas_BillDate, Pmas_VchNo"

                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@from", txtFromDate.Text)
                cmd.Parameters.AddWithValue("@to", txtToDate.Text)
                cmd.Parameters.AddWithValue("@compNo", compNo)

                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable()
                da.Fill(dt)

                gvPurchase.DataSource = dt
                gvPurchase.DataBind()

                If dt.Rows.Count > 0 Then
                    litBillCount.Text = dt.Rows.Count.ToString()
                    litQtyTotal.Text = Convert.ToDecimal(If(IsDBNull(dt.Compute("SUM(purQty)", "")), 0, dt.Compute("SUM(purQty)", ""))).ToString("N2")
                    litGrossTotal.Text = Convert.ToDecimal(If(IsDBNull(dt.Compute("SUM(Pmas_gross)", "")), 0, dt.Compute("SUM(Pmas_gross)", ""))).ToString("N2")
                    litAmountTotal.Text = Convert.ToDecimal(If(IsDBNull(dt.Compute("SUM(Pmas_NetAmount)", "")), 0, dt.Compute("SUM(Pmas_NetAmount)", ""))).ToString("N2")
                Else
                    litBillCount.Text = "0"
                    litQtyTotal.Text = "0.00"
                    litGrossTotal.Text = "0.00"
                    litAmountTotal.Text = "0.00"
                End If
            End Using
        Catch ex As Exception
            ' Error handled silently for UI
        End Try
    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        BindPurchaseData()
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Write("<script>window.top.location.href='MainForm.aspx';</script>")
    End Sub
End Class
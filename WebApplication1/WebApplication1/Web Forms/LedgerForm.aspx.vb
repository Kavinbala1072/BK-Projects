Imports System.Data
Imports System.Data.SqlClient

Public Class LedgerForm
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
            SetupDatePickers()
            BindLedgerList()

            ' Initial selection check
            If lstLedgers.Items.Count > 0 Then
                lstLedgers.SelectedIndex = 0
                If lstLedgers.SelectedItem IsNot Nothing Then
                    txtSelectedLedger.Text = lstLedgers.SelectedItem.Text
                    LoadData()
                End If
            End If
        End If
    End Sub

    Private Sub SetupDatePickers()
        Dim fyStart As String = If(Request.Cookies("Selected_PeriodFrom") IsNot Nothing, Request.Cookies("Selected_PeriodFrom").Value, "2025-04-01")
        Dim fyEnd As String = If(Request.Cookies("Selected_PeriodTo") IsNot Nothing, Request.Cookies("Selected_PeriodTo").Value, "2026-03-31")

        Dim today = DateTime.Now
        txtFromDate.Text = New DateTime(today.Year, today.Month, 1).ToString("yyyy-MM-dd")
        txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd")

        txtFromDate.Attributes("min") = fyStart
        txtFromDate.Attributes("max") = fyEnd
        txtToDate.Attributes("min") = fyStart
        txtToDate.Attributes("max") = fyEnd
    End Sub

    Private Sub BindLedgerList(Optional filter As String = "")
        Dim compNo As String = If(Request.Cookies("Selected_CompNo") IsNot Nothing, Request.Cookies("Selected_CompNo").Value, "ED1")

        Dim query As String = "SELECT Ledsel_MtName as MtName, LedSel_Name as LtName FROM LedSel_table WHERE comp_no = @comp "
        If Not String.IsNullOrEmpty(filter) Then
            query &= "AND LedSel_Name LIKE @filter "
        End If
        query &= "ORDER BY LedSel_Name"

        Using conn As New SqlConnection(GetDBConn())
            Try
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@comp", compNo)
                If Not String.IsNullOrEmpty(filter) Then cmd.Parameters.AddWithValue("@filter", "%" & filter & "%")

                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable()
                da.Fill(dt)

                lstLedgers.DataSource = dt
                lstLedgers.DataTextField = "LtName"
                lstLedgers.DataValueField = "MtName"
                lstLedgers.DataBind()
            Catch ex As Exception
                lblStatus.Text = "List Error"
            End Try
        End Using
    End Sub

    Protected Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        BindLedgerList(txtSearch.Text.Trim())
    End Sub

    Protected Sub lstLedgers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstLedgers.SelectedIndexChanged
        If lstLedgers.SelectedItem IsNot Nothing Then
            txtSelectedLedger.Text = lstLedgers.SelectedItem.Text
            LoadData()
        End If
    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadData()
    End Sub

    Private Sub LoadData()

        If lstLedgers.SelectedItem Is Nothing Then Exit Sub

        Dim ledgerNameVal As String = lstLedgers.SelectedValue
        If String.IsNullOrEmpty(ledgerNameVal) Then Exit Sub

        Dim dFrom As DateTime = DateTime.Parse(txtFromDate.Text)
        Dim dTo As DateTime = DateTime.Parse(txtToDate.Text).AddHours(23).AddMinutes(59)
        Dim compNo As String = If(Request.Cookies("Selected_CompNo") IsNot Nothing, Request.Cookies("Selected_CompNo").Value, "ED1")

        Using conn As New SqlConnection(GetDBConn())
            Try
                conn.Open()
                ' 1. Opening Balance
                Dim sqlOpBal As String = "SELECT ISNULL(SUM(dr_amount),0) as Dr, ISNULL(SUM(cr_amount),0) as Cr " &
                                       "FROM Vch_Table WHERE Vch_Date < @from AND Comp_No = @comp AND Vch_Cancel='False' " &
                                       "AND ledger_No IN (SELECT Ledger_Sno FROM Ledger_Table WHERE RTRIM(ledger_MtName) = @name)"

                Dim cmdOp As New SqlCommand(sqlOpBal, conn)
                cmdOp.Parameters.AddWithValue("@from", dFrom)
                cmdOp.Parameters.AddWithValue("@comp", compNo)
                cmdOp.Parameters.AddWithValue("@name", ledgerNameVal)

                Dim opDr As Decimal = 0, opCr As Decimal = 0
                Using rdr = cmdOp.ExecuteReader()
                    If rdr.Read() Then
                        opDr = Convert.ToDecimal(rdr("Dr"))
                        opCr = Convert.ToDecimal(rdr("Cr"))
                    End If
                End Using
                litOpDr.Text = If(opDr >= opCr, (opDr - opCr).ToString("N2"), "0.00")
                litOpCr.Text = If(opCr > opDr, (opCr - opDr).ToString("N2"), "0.00")

                ' 2. Transactions
                Dim query As String = "SELECT vch.vch_date, ISNULL(opp.ledger_name,'') as [oppledger], ISNULL(vt.vt_shortname, '') as [vt_shortname], " &
                                     "ISNULL(vch.vch_no,'') as [vch_no], ISNULL(vch.cr_amount, 0) As [Cr_Amount], ISNULL(vch.dr_amount, 0) As [Dr_Amount] " &
                                     "FROM Vch_Table vch " &
                                     "LEFT JOIN VTable vt ON vt.Vt_Sno = vch.Vch_Type AND vt.Comp_No = vch.Comp_No " &
                                     "LEFT JOIN Ledger_Table opp ON opp.Ledger_Sno = vch.ledger_no1 AND opp.Comp_No = vch.Comp_No " &
                                     "WHERE vch.Vch_Date >= @from AND vch.Vch_Date <= @to AND vch.Comp_No = @comp AND vch.Vch_Cancel = 'False' " &
                                     "AND vch.ledger_No IN (SELECT Ledger_Sno FROM Ledger_Table WHERE RTRIM(ledger_MtName) = @name) " &
                                     "ORDER BY vch.Vch_Date, vch.vch_sno"

                Dim da As New SqlDataAdapter(query, conn)
                da.SelectCommand.Parameters.AddWithValue("@from", dFrom)
                da.SelectCommand.Parameters.AddWithValue("@to", dTo)
                da.SelectCommand.Parameters.AddWithValue("@comp", compNo)
                da.SelectCommand.Parameters.AddWithValue("@name", ledgerNameVal)

                Dim dt As New DataTable()
                da.Fill(dt)

                Dim actualCount As Integer = dt.Rows.Count
                While dt.Rows.Count < 12
                    dt.Rows.Add(dt.NewRow())
                End While

                gvLedger.DataSource = dt
                gvLedger.DataBind()

                CalculateBalances(dt, actualCount, (opDr - opCr))
            Catch ex As Exception
                lblStatus.Text = "Data Error"
            End Try
        End Using
    End Sub

    Private Sub CalculateBalances(dt As DataTable, actualCount As Integer, opBalNet As Decimal)
        Dim pCr As Decimal = 0, pDr As Decimal = 0
        For Each row As DataRow In dt.Rows
            If Not IsDBNull(row("Cr_Amount")) Then pCr += Convert.ToDecimal(row("Cr_Amount"))
            If Not IsDBNull(row("Dr_Amount")) Then pDr += Convert.ToDecimal(row("Dr_Amount"))
        Next

        litTotalCount.Text = "Total ( " & actualCount & " )"
        litTotalCr.Text = pCr.ToString("N2")
        litTotalDr.Text = pDr.ToString("N2")

        Dim clBal = opBalNet + pDr - pCr
        litClDr.Text = If(clBal >= 0, Math.Abs(clBal).ToString("N2"), "0.00")
        litClCr.Text = If(clBal < 0, Math.Abs(clBal).ToString("N2"), "0.00")
    End Sub

    Private Function GetDBConn() As String
        Dim s = Server.UrlDecode(Request.Cookies("SQL_Server").Value).Replace("\\", "\")
        Dim d = Request.Cookies("SQL_DB").Value
        Dim u = Request.Cookies("SQL_User").Value
        Dim p = Server.UrlDecode(Request.Cookies("SQL_Pass").Value)
        Return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", s, d, u, p)
    End Function

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Write("<script>window.top.location.href='MainForm.aspx';</script>")
    End Sub
End Class
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text

Public Class ProfitandlossForm
    Inherits System.Web.UI.Page

    Dim compNo As String = "ED1"

    Private Class PLPair
        Public ExpName As String = ""
        Public ExpAmt As String = ""
        Public ExpClass As String = ""
        Public IncName As String = ""
        Public IncAmt As String = ""
        Public IncClass As String = ""
    End Class

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserName") Is Nothing OrElse Request.Cookies("SQL_Server") Is Nothing Then Response.Redirect("Default.aspx") : Exit Sub
        compNo = If(Request.Cookies("Selected_CompNo")?.Value, "ED1")
        If Not IsPostBack Then
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
            LoadProfitAndLoss()
        End If
    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadProfitAndLoss()
    End Sub

    Private Sub LoadProfitAndLoss()
        Dim mainRows As New List(Of PLPair)
        Dim asOnDate As DateTime = DateTime.Parse(txtDate.Text)
        Dim finYearStart As String = If(asOnDate.Month >= 4, asOnDate.Year.ToString(), (asOnDate.Year - 1).ToString()) & "-04-01"

        Using conn As New SqlConnection(GetDBConn())
            conn.Open()

            ' 1. FETCH DATA
            Dim opStock = GetStockValue(conn, DateTime.Parse(finYearStart).AddDays(-1))
            Dim clStock = GetStockValue(conn, asOnDate)
            Dim dt = GetPLBalances(conn, finYearStart, txtDate.Text)

            ' 2. PROCESS TRADING SECTION (Direct Items)
            Dim leftTrading = ProcessSide(dt, "ledgergroup_gno IN (102, 105, 106)", True)
            Dim rightTrading = ProcessSide(dt, "ledgergroup_gno IN (102, 105, 106)", False)

            ' Add Stocks manually to Trading
            leftTrading.Insert(0, New PLPair With {.ExpName = "Stock In Hand - (Opening)", .ExpAmt = opStock.ToString("N2"), .ExpClass = "row-group"})
            rightTrading.Insert(0, New PLPair With {.IncName = "Closing Stock", .IncAmt = clStock.ToString("N2"), .IncClass = "row-group"})

            ' Totals for Gross Profit
            Dim trDrTotal = opStock + GetTotal(dt, "ledgergroup_gno IN (102, 105, 106)", True)
            Dim trCrTotal = clStock + GetTotal(dt, "ledgergroup_gno IN (102, 105, 106)", False)
            Dim grossProfit = trCrTotal - trDrTotal

            If grossProfit >= 0 Then
                leftTrading.Add(New PLPair With {.ExpName = "Gross Profit c/o", .ExpAmt = grossProfit.ToString("N2"), .ExpClass = "row-balancing"})
            Else
                rightTrading.Add(New PLPair With {.IncName = "Gross Loss c/o", .IncAmt = Math.Abs(grossProfit).ToString("N2"), .IncClass = "row-loss"})
            End If

            AlignAndMerge(mainRows, leftTrading, rightTrading)
            mainRows.Add(New PLPair With {.ExpAmt = "-----------------", .IncAmt = "-----------------"})

            ' 3. PROCESS P&L SECTION (Indirect Items)
            Dim leftPL = ProcessSide(dt, "ledgergroup_gno IN (100, 101, 103, 104)", True)
            Dim rightPL = ProcessSide(dt, "ledgergroup_gno IN (100, 101, 103, 104)", False)

            ' Gross Profit B/f
            If grossProfit >= 0 Then
                rightPL.Insert(0, New PLPair With {.IncName = "Gross Profit b/f", .IncAmt = grossProfit.ToString("N2"), .IncClass = "row-balancing"})
            Else
                leftPL.Insert(0, New PLPair With {.ExpName = "Gross Loss b/f", .ExpAmt = Math.Abs(grossProfit).ToString("N2"), .ExpClass = "row-loss"})
            End If

            Dim plDrTotal = If(grossProfit < 0, Math.Abs(grossProfit), 0) + GetTotal(dt, "ledgergroup_gno IN (100, 101, 103, 104)", True)
            Dim plCrTotal = If(grossProfit > 0, grossProfit, 0) + GetTotal(dt, "ledgergroup_gno IN (100, 101, 103, 104)", False)
            Dim netProfit = plCrTotal - plDrTotal

            If netProfit >= 0 Then
                leftPL.Add(New PLPair With {.ExpName = "Net Profit (to Capital)", .ExpAmt = netProfit.ToString("N2"), .ExpClass = "row-balancing"})
            Else
                rightPL.Add(New PLPair With {.IncName = "Net Loss", .IncAmt = Math.Abs(netProfit).ToString("N2"), .IncClass = "row-loss"})
            End If

            AlignAndMerge(mainRows, leftPL, rightPL)

            ' 4. RENDER HTML
            Dim html As New StringBuilder()
            For Each p In mainRows
                html.Append("<tr>")
                html.AppendFormat("<td class='{0}'>{1}</td><td class='col-amt border-mid'>{2}</td>", p.ExpClass, p.ExpName, p.ExpAmt)
                html.AppendFormat("<td class='{0}'>{1}</td><td class='col-amt'>{2}</td>", p.IncClass, p.IncName, p.IncAmt)
                html.Append("</tr>")
            Next

            litPLRows.Text = html.ToString()
            Dim finalTotal = Math.Max(plDrTotal + If(netProfit > 0, netProfit, 0), plCrTotal + If(netProfit < 0, Math.Abs(netProfit), 0))
            litTotalDebit.Text = finalTotal.ToString("N2")
            litTotalCredit.Text = finalTotal.ToString("N2")
        End Using
    End Sub

    ' NEW: This function groups ledgers under a single group header
    Private Function ProcessSide(dt As DataTable, filter As String, isDebitSide As Boolean) As List(Of PLPair)
        Dim list As New List(Of PLPair)
        Dim lastGroup As String = ""

        ' Sort by GroupName to ensure grouping works
        Dim rows = dt.Select(filter, "GroupName ASC, IsLedger ASC")

        For Each row As DataRow In rows
            Dim amt = Convert.ToDecimal(row("PAMT"))
            ' Debit Side wants positive numbers, Credit Side wants negative numbers
            If (isDebitSide And amt > 0) Or (Not isDebitSide And amt < 0) Then
                Dim currentGroup = row("GroupName").ToString()
                Dim isLedger = (Convert.ToInt32(row("IsLedger")) = 1)

                ' If this is a new group, add the Header row first
                If currentGroup <> lastGroup Then
                    ' Calculate group total amount
                    Dim groupSum = Convert.ToDecimal(dt.Compute("SUM(PAMT)", filter & " AND GroupName = '" & currentGroup.Replace("'", "''") & "' AND IsLedger = 1"))

                    Dim pHead As New PLPair()
                    If isDebitSide Then
                        pHead.ExpName = currentGroup : pHead.ExpAmt = groupSum.ToString("N2") : pHead.ExpClass = "row-group"
                    Else
                        pHead.IncName = currentGroup : pHead.IncAmt = Math.Abs(groupSum).ToString("N2") : pHead.IncClass = "row-group"
                    End If
                    list.Add(pHead)
                    lastGroup = currentGroup
                End If

                ' Add the Ledger row (indented)
                If isLedger Then
                    Dim pLedg As New PLPair()
                    If isDebitSide Then
                        pLedg.ExpName = " - " & row("DisplayName").ToString() : pLedg.ExpAmt = amt.ToString("N2") : pLedg.ExpClass = "row-ledger"
                    Else
                        pLedg.IncName = " - " & row("DisplayName").ToString() : pLedg.IncAmt = Math.Abs(amt).ToString("N2") : pLedg.IncClass = "row-ledger"
                    End If
                    list.Add(pLedg)
                End If
            End If
        Next
        Return list
    End Function

    Private Function GetTotal(dt As DataTable, filter As String, isDebit As Boolean) As Decimal
        Dim rows = dt.Select(filter & " AND IsLedger = 1")
        Dim total As Decimal = 0
        For Each r In rows
            Dim val = Convert.ToDecimal(r("PAMT"))
            If isDebit And val > 0 Then total += val
            If Not isDebit And val < 0 Then total += Math.Abs(val)
        Next
        Return total
    End Function

    Private Sub AlignAndMerge(ByRef target As List(Of PLPair), left As List(Of PLPair), right As List(Of PLPair))
        Dim count = Math.Max(left.Count, right.Count)
        For i As Integer = 0 To count - 1
            Dim p As New PLPair()
            If i < left.Count Then
                p.ExpName = left(i).ExpName : p.ExpAmt = left(i).ExpAmt : p.ExpClass = left(i).ExpClass
            End If
            If i < right.Count Then
                p.IncName = right(i).IncName : p.IncAmt = right(i).IncAmt : p.IncClass = right(i).IncClass
            End If
            target.Add(p)
        Next
    End Sub

    Private Function GetPLBalances(conn As SqlConnection, startDt As String, endDt As String) As DataTable
        ' SQL that returns BOTH Group Info and Ledger info for every ledger
        Dim sql As String =
        "SELECT G.ledgerGroup_name as GroupName, L.ledger_name as DisplayName, " &
        "G.ledgergroup_gno, SUM(round(V.Dr_amount,2)-round(V.Cr_amount,2)) as PAMT, 1 as IsLedger " &
        "FROM Ledger_table L " &
        "INNER JOIN LedgerGroup_table G ON L.ledger_groupno = G.ledgergroup_sno AND L.comp_no = G.comp_no " &
        "INNER JOIN Vch_table V ON L.ledger_sno = V.ledger_no AND V.comp_no = L.comp_no AND V.vch_cancel='false' " &
        "WHERE G.comp_no = @cp AND V.vch_date >= @start AND V.vch_date <= @end AND G.ledgergroup_gno BETWEEN 100 AND 106 " &
        "GROUP BY G.ledgerGroup_name, G.ledgergroup_gno, L.ledger_name " &
        "HAVING SUM(round(V.Dr_amount,2)-round(V.Cr_amount,2)) <> 0 "

        Dim dt As New DataTable()
        Using cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@cp", compNo)
            cmd.Parameters.AddWithValue("@start", startDt)
            cmd.Parameters.AddWithValue("@end", endDt)
            dt.Load(cmd.ExecuteReader())
        End Using
        Return dt
    End Function

    ' ... Keep GetStockValue and GetDBConn as they were ...
    Private Function GetStockValue(conn As SqlConnection, dt As DateTime) As Decimal
        Dim sql = ";WITH cte AS(SELECT stk_value, ROW_NUMBER() OVER (PARTITION BY ledger_no ORDER BY stk_date DESC) AS rn " &
                  "FROM ClosingStk_table WHERE rtrim(ledger_no) <> '0' AND stk_date <= @dt AND comp_no = @cp) SELECT ISNULL(SUM(stk_value),0) FROM cte WHERE rn = 1"
        Using cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@cp", compNo)
            cmd.Parameters.AddWithValue("@dt", dt.ToString("yyyy-MM-dd"))
            Dim v = cmd.ExecuteScalar()
            Return If(v IsNot DBNull.Value, Convert.ToDecimal(v), 0)
        End Using
    End Function

    Private Function GetDBConn() As String
        Dim s = Server.UrlDecode(Request.Cookies("SQL_Server").Value).Replace("\\", "\")
        Dim d = Request.Cookies("SQL_DB").Value
        Dim u = Request.Cookies("SQL_User").Value
        Dim p = Server.UrlDecode(Request.Cookies("SQL_Pass").Value)
        Return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", s, d, u, p)
    End Function

    Protected Sub btnExit_Click(sender As Object, e As EventArgs)
        Response.Write("<script>window.top.location.href='MainForm.aspx';</script>")
    End Sub
End Class
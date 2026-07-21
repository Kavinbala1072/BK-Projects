Imports System.Data
Imports System.Data.SqlClient
Imports System.Text

Public Class BalanceSheet
    Inherits System.Web.UI.Page

    Dim compNo As String = "ED1"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserName") Is Nothing OrElse Request.Cookies("SQL_Server") Is Nothing Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        compNo = If(Request.Cookies("Selected_CompNo")?.Value, "ED1")

        If Not IsPostBack Then
            txtDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
            LoadBalanceSheet()
        End If
    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadBalanceSheet()
    End Sub

    Private Sub LoadBalanceSheet()
        Dim htmlLiab As New StringBuilder()
        Dim htmlAsst As New StringBuilder()
        Dim sumLiab As Decimal = 0
        Dim sumAsst As Decimal = 0

        Using conn As New SqlConnection(GetDBConn())
            Try
                conn.Open()

                ' 1. CALCULATE OPENING BALANCE DIFFERENCE (KGM Logic)
                Dim opDiff As Decimal = GetOpeningDiff(conn)
                If opDiff > 0 Then ' Net Debit > Credit in Opening
                    htmlLiab.AppendFormat("<tr class='row-group row-diff'><td>Opening Balance Diff.</td><td class='amt-col'>{0}</td></tr>", opDiff.ToString("N2"))
                    sumLiab += opDiff
                ElseIf opDiff < 0 Then ' Net Credit > Debit in Opening
                    htmlAsst.AppendFormat("<tr class='row-group row-diff'><td>Opening Balance Diff.</td><td class='amt-col'>{0}</td></tr>", Math.Abs(opDiff).ToString("N2"))
                    sumAsst += Math.Abs(opDiff)
                End If

                ' 2. FETCH MAIN STATEMENT DATA
                Dim sql As String = GetBalanceSheetQuery()
                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@cp", compNo)
                cmd.Parameters.AddWithValue("@dt", txtDate.Text)

                Dim dr As SqlDataReader = cmd.ExecuteReader()
                While dr.Read()
                    Dim name As String = dr("Name").ToString()
                    Dim amt As Decimal = Convert.ToDecimal(dr("PAMT"))
                    Dim isLedger As Boolean = (Convert.ToInt32(dr("IsLedger")) = 1)

                    Dim rowHtml = String.Format("<tr class='{0}'><td>{1}</td><td class='amt-col'>{2}</td></tr>",
                                    If(isLedger, "row-ledger", "row-group"),
                                    If(isLedger, " - " & name, name),
                                    Math.Abs(amt).ToString("N2"))

                    If amt < 0 Then ' LIABILITY side (Credit Balance)
                        htmlLiab.Append(rowHtml)
                        If Not isLedger Then sumLiab += Math.Abs(amt)
                    Else ' ASSETS side (Debit Balance)
                        htmlAsst.Append(rowHtml)
                        If Not isLedger Then sumAsst += amt
                    End If
                End While
                dr.Close()

                ' 3. FETCH CLOSING STOCK
                Dim closingStock = GetClosingStockValue(conn)
                If closingStock <> 0 Then
                    htmlAsst.AppendFormat("<tr class='row-group'><td>Closing Stock</td><td class='amt-col'>{0}</td></tr>", closingStock.ToString("N2"))
                    sumAsst += closingStock
                End If

                ' 4. CALCULATE NET PROFIT / LOSS (Balancing Figure)
                ' The Sheet must balance: sumAsst = sumLiab
                Dim netDiff As Decimal = sumAsst - sumLiab

                If netDiff > 0 Then ' Asset > Liability -> We have PROFIT
                    htmlLiab.AppendFormat("<tr class='row-group' style='color:#10b981;'><td>NET PROFIT (Current Period)</td><td class='amt-col'>{0}</td></tr>", netDiff.ToString("N2"))
                    sumLiab += netDiff
                ElseIf netDiff < 0 Then ' Liability > Asset -> We have LOSS
                    htmlAsst.AppendFormat("<tr class='row-group' style='color:#ef4444;'><td>NET LOSS (Current Period)</td><td class='amt-col'>{0}</td></tr>", Math.Abs(netDiff).ToString("N2"))
                    sumAsst += Math.Abs(netDiff)
                End If

                ' 5. OUTPUT TO LITERALS
                litLiabilitiesRows.Text = htmlLiab.ToString()
                litAssetsRows.Text = htmlAsst.ToString()
                litTotalLiabilities.Text = sumLiab.ToString("N2")
                litTotalAssets.Text = sumAsst.ToString("N2")

            Catch ex As Exception
                lblStatus.Text = "Error: " & ex.Message
            End Try
        End Using
    End Sub

    Private Function GetOpeningDiff(conn As SqlConnection) As Decimal
        Dim sql As String = "Select isnull(sum(round(Dr_amount,2)-round(cr_amount,2)),0) FROM vch_table " &
                            "Where substring(vch_type,3,12) = N'255' and comp_no = @cp"
        Using cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@cp", compNo)
            Return Convert.ToDecimal(cmd.ExecuteScalar())
        End Using
    End Function

    Private Function GetBalanceSheetQuery() As String
        'Dim isDetailed As Boolean = (rblDetail.SelectedValue = "D")

        Dim sql As String = "SELECT G.ledgerGroup_name as Name, SUM(round(V.Dr_amount,2)-round(V.Cr_amount,2)) as PAMT, 0 as IsLedger, G.GroupPos " &
               "FROM LedgerGroup_table G INNER JOIN Ledger_table L ON G.ledgergroup_sno = L.ledger_groupno AND G.comp_no = L.comp_no " &
               "LEFT JOIN Vch_table V ON L.ledger_sno = V.ledger_no AND V.comp_no = L.comp_no AND V.vch_cancel='false' AND V.vch_date <= @dt " &
               "WHERE G.comp_no = @cp AND (G.ledgergroup_gno < 100 OR G.ledgergroup_gno > 109) AND G.ledgergroup_mtname NOT IN ('STOCKINHAND','PROFITANDLOSSACCOUNT') " &
               "GROUP BY G.ledgerGroup_name, G.GroupPos " &
               "HAVING SUM(round(V.Dr_amount,2)-round(V.Cr_amount,2)) <> 0 "

        'If isDetailed Then
        '    sql &= " UNION ALL SELECT L.ledger_name, (round(V.Dr_amount,2)-round(V.Cr_amount,2)), 1, G.GroupPos " &
        '           "FROM Ledger_table L INNER JOIN LedgerGroup_table G ON L.ledger_groupno = G.ledgergroup_sno AND L.comp_no = G.comp_no " &
        '           "INNER JOIN Vch_table V ON L.ledger_sno = V.ledger_no AND V.comp_no = L.comp_no AND V.vch_cancel='false' AND V.vch_date <= @dt " &
        '           "WHERE L.comp_no = @cp AND (G.ledgergroup_gno < 100 OR G.ledgergroup_gno > 109) AND G.ledgergroup_mtname NOT IN ('STOCKINHAND','PROFITANDLOSSACCOUNT') "
        'End If

        Return sql & " ORDER BY GroupPos, IsLedger"
    End Function

    Private Function GetClosingStockValue(conn As SqlConnection) As Decimal
        Dim sql As String = ";WITH cte AS(SELECT stk_value, ROW_NUMBER() OVER (PARTITION BY ledger_no ORDER BY stk_date DESC) AS rn " &
                            "FROM ClosingStk_table WHERE rtrim(ledger_no) <> '0' AND stk_date <= @dt AND comp_no = @cp) " &
                            "SELECT ISNULL(SUM(stk_value),0) FROM cte WHERE rn = 1"
        Using cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@cp", compNo)
            cmd.Parameters.AddWithValue("@dt", txtDate.Text)
            Dim result = cmd.ExecuteScalar()
            Return If(result IsNot DBNull.Value, Convert.ToDecimal(result), 0)
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
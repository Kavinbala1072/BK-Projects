Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Script.Serialization

Public Class Dashboard
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1))
        Response.Cache.SetNoStore()

        Response.Cache.SetCacheability(HttpCacheability.NoCache)

        If Session("UserName") Is Nothing OrElse Request.Cookies("SQL_Server") Is Nothing Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        Dim fyEnd As String = DateTime.Now.ToString("yyyy-MM-dd")
        If Request.Cookies("Selected_PeriodTo") IsNot Nothing Then
            fyEnd = Request.Cookies("Selected_PeriodTo").Value
        End If

        If Not IsPostBack Then
            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
            txtToDate.Attributes("max") = fyEnd
            LoadDashboardData()
        End If
    End Sub
    Protected Sub txtToDate_TextChanged(sender As Object, e As EventArgs) Handles txtToDate.TextChanged
        LoadDashboardData()
    End Sub
    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadDashboardData()
    End Sub

    Private Sub LoadDashboardData()
        Try
            Dim s = Server.UrlDecode(Request.Cookies("SQL_Server").Value).Replace("\\", "\")
            Dim d = Request.Cookies("SQL_DB").Value
            Dim u = Request.Cookies("SQL_User").Value
            Dim p = Server.UrlDecode(Request.Cookies("SQL_Pass").Value)
            Dim connStr = String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", s, d, u, p)

            Dim compNo As String = If(Request.Cookies("Selected_CompNo") IsNot Nothing, Request.Cookies("Selected_CompNo").Value, "ED1")
            Dim today As String = txtToDate.Text

            Dim selectedDate As DateTime = DateTime.Parse(today)
            Dim mStart As String = New DateTime(selectedDate.Year, selectedDate.Month, 1).ToString("yyyy-MM-dd")

            Dim periodTo As String = today

            Using conn As New SqlConnection(connStr)
                conn.Open()

                ' Cash In Hand Query
                Dim cashQuery As String = "SELECT ISNULL(SUM(v.dr_amount - v.cr_amount), 0) " &
                          "FROM vch_table v " &
                          "INNER JOIN ledger_table l ON v.ledger_no = l.ledger_sno AND v.Comp_no = l.Comp_no " &
                          "LEFT JOIN ledgerGroup_table lg ON l.ledger_groupno = lg.ledgergroup_sno " &
                          "WHERE v.Vch_Cancel = 'false' " &
                          "AND CONVERT(date, v.vch_date, 103) <= @d1 " &
                          "AND lg.Ledgergroup_gno = 32 " &
                          "AND v.Comp_no = @cp"

                litCashInHand.Text = GetScalar(conn, cashQuery, compNo, periodTo).ToString("N2")

                ' Bank Balances Logic
                Dim sql As String = "IF OBJECT_ID('tempdb..#Opening') IS NOT NULL DROP TABLE #Opening; " &
                                    "SELECT l.ledger_sno, l.ledger_name, " &
                                    "ISNULL(SUM(ROUND(v.Dr_amount,2)),0) AS OpenDr, " &
                                    "ISNULL(SUM(ROUND(v.Cr_amount,2)),0) AS OpenCr " &
                                    "INTO #Opening FROM ledger_table l " &
                                    "LEFT JOIN vch_table v ON v.ledger_no = l.ledger_sno AND v.Comp_no = l.Comp_no " &
                                    "AND v.vch_cancel = 'false' AND CONVERT(date, v.vch_date, 103) < '2025-04-01' " &
                                    "LEFT JOIN ledgerGroup_table lg ON l.ledger_groupno = lg.ledgergroup_sno " &
                                    "WHERE l.comp_no = @cp AND lg.Ledgergroup_gno IN (31, 71) " &
                                    "GROUP BY l.ledger_sno, l.ledger_name; " &
                                    "SELECT l.ledger_name, " &
                                    "ISNULL(o.OpenDr,0) + ISNULL(SUM(CASE WHEN CONVERT(date, v.vch_date, 103) >= '2025-04-01' " &
                                    "AND CONVERT(date, v.vch_date, 103) <= @e THEN ROUND(v.Dr_amount,2) END),0) - " &
                                    "(ISNULL(o.OpenCr,0) + ISNULL(SUM(CASE WHEN CONVERT(date, v.vch_date, 103) >= '2025-04-01' " &
                                    "AND CONVERT(date, v.vch_date, 103) <= @e THEN ROUND(v.Cr_amount,2) END),0)) AS ClosingBalance " &
                                    "FROM ledger_table l " &
                                    "LEFT JOIN #Opening o ON o.ledger_sno = l.ledger_sno " &
                                    "LEFT JOIN vch_table v ON l.ledger_sno = v.ledger_no AND v.vch_cancel = 'false' AND v.Comp_no = @cp " &
                                    "LEFT JOIN ledgerGroup_table lg ON l.ledger_groupno = lg.ledgergroup_sno " &
                                    "WHERE lg.Ledgergroup_gno IN (31, 71) " &
                                    "GROUP BY l.ledger_name, o.OpenDr, o.OpenCr ORDER BY l.ledger_name;"

                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@cp", compNo)
                    cmd.Parameters.AddWithValue("@e", periodTo)

                    Dim dt As New DataTable()
                    dt.Load(cmd.ExecuteReader())

                    rptBankBalances.DataSource = dt
                    rptBankBalances.DataBind()
                End Using

                ' Sales Data
                Dim sToday = GetScalar(conn, "SELECT ISNULL(SUM(tot_amt), 0) FROM saltrn_table INNER JOIN salmas_table ON smas_sno = strn_masno AND salmas_table.comp_no = saltrn_table.comp_no WHERE smas_cancel = 'false' AND CAST(smas_billdate AS DATE) = @d1 AND salmas_table.comp_no = @cp", compNo, today)
                Dim sMonth = GetScalar(conn, "SELECT ISNULL(SUM(tot_amt), 0) FROM saltrn_table INNER JOIN salmas_table ON smas_sno = strn_masno AND salmas_table.comp_no = saltrn_table.comp_no WHERE smas_cancel = 'false' AND CAST(smas_billdate AS DATE) BETWEEN @d1 AND @d2 AND salmas_table.comp_no = @cp", compNo, mStart, today)

                litSalesToday.Text = sToday.ToString("N2")
                hfSToday.Value = sToday.ToString()
                hfSMonth.Value = sMonth.ToString()

                ' Purchase Data
                Dim pToday = GetScalar(conn, "SELECT ISNULL(SUM(ptrn_totamt), 0) FROM purtrn_table INNER JOIN purmas_table ON pmas_sno = ptrn_pmaSsno AND purmas_table.comp_no = purtrn_table.comp_no WHERE pmas_cancel = 'false' AND CAST(pmas_vchdate AS DATE) = @d1 AND purmas_table.comp_no = @cp", compNo, today)
                Dim pMonth = GetScalar(conn, "SELECT ISNULL(SUM(ptrn_totamt), 0) FROM purtrn_table INNER JOIN purmas_table ON pmas_sno = ptrn_pmaSsno AND purmas_table.comp_no = purtrn_table.comp_no WHERE pmas_cancel = 'false' AND CAST(pmas_vchdate AS DATE) BETWEEN @d1 AND @d2 AND purmas_table.comp_no = @cp", compNo, mStart, today)

                litPurToday.Text = pToday.ToString("N2")
                hfPToday.Value = pToday.ToString()
                hfPMonth.Value = pMonth.ToString()

                ' Other Dashboard Stats
                litCashSales.Text = GetScalar(conn, "SELECT ISNULL(SUM(smas_netamount), 0) FROM salmas_table WHERE smas_cancel = 'false' AND CAST(smas_billdate AS DATE) = @d1 AND comp_no = @cp AND smas_cashmode = 0", compNo, today).ToString("N2")
                Dim sRet = GetScalar(conn, "SELECT ISNULL(SUM(Tot_Amt), 0) FROM salretMas_table INNER JOIN SalretTrn_Table ON Sretmas_Sno = srettrn_masno WHERE sRettrn_cancel = 'false' AND CAST(sRetMas_BillDate AS DATE) = @d1 AND salretMas_table.comp_no = @cp", compNo, today)
                litReturns.Text = sRet.ToString("N2")
                litReceipts.Text = GetScalar(conn, "SELECT ISNULL(SUM(cr_amount), 0) FROM Vch_Table WHERE CAST(vch_date AS DATE) = @d1 AND vch_cancel = 'False' AND Vch_Type = 'ED309' AND Comp_No = @cp", compNo, today).ToString("N2")

                LoadLiquidityTrend(conn, compNo, today)

                BindRepeater(conn, rptTopCustomers, "(Ledger_GNo IN (36, 201, 208))", "(Dr_amount - Cr_amount)", compNo, periodTo)
                BindRepeater(conn, rptTopSuppliers, "(Ledger_GNo IN (43, 203, 209))", "(Cr_amount - Dr_amount)", compNo, periodTo)

            End Using
        Catch ex As Exception
            ' Handle errors
        End Try
    End Sub

    Private Sub LoadLiquidityTrend(conn As SqlConnection, cp As String, today As String)
        Dim labels As New List(Of String)
        Dim drData As New List(Of Decimal)
        Dim crData As New List(Of Decimal)

        Dim sql = "SELECT FORMAT(v.Vch_Date, 'MMM yy') as MonthName, " &
                  "SUM(CASE WHEN Ledger_GNo IN (36, 201, 208) THEN Dr_amount - Cr_amount ELSE 0 END) as Receivable, " &
                  "SUM(CASE WHEN Ledger_GNo IN (43, 203, 209) THEN Cr_amount - Dr_amount ELSE 0 END) as Payable " &
                  "FROM vch_table v INNER JOIN Ledger_Table l ON v.Ledger_No = l.Ledger_Sno AND v.Comp_No = l.Comp_No " &
                  "WHERE v.Vch_Date >= DATEADD(month, -5, @d) AND v.Comp_No = @cp AND v.Vch_Cancel = 'False' " &
                  "GROUP BY FORMAT(v.Vch_Date, 'MMM yy'), YEAR(v.Vch_Date), MONTH(v.Vch_Date) " &
                  "ORDER BY YEAR(v.Vch_Date), MONTH(v.Vch_Date)"

        Using cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@d", today)
            cmd.Parameters.AddWithValue("@cp", cp)
            Using rdr = cmd.ExecuteReader()
                While rdr.Read()
                    labels.Add(rdr("MonthName").ToString())
                    drData.Add(Convert.ToDecimal(rdr("Receivable")))
                    crData.Add(Convert.ToDecimal(rdr("Payable")))
                End While
            End Using
        End Using

        Dim js As New JavaScriptSerializer()
        hfOutLabels.Value = js.Serialize(labels)
        hfOutDrData.Value = js.Serialize(drData)
        hfOutCrData.Value = js.Serialize(crData)
    End Sub

    Private Sub BindRepeater(conn As SqlConnection, rpt As Repeater, groupFilter As String, balanceCalc As String, comp As String, periodTo As String)
        Dim sql = String.Format("SELECT TOP 10 ledger_Name, SUM({0}) as TotalBalance FROM vch_table " &
                  "INNER JOIN Ledger_Table ON Ledger_No = Ledger_Sno AND vch_table.Comp_No = Ledger_Table.Comp_No " &
                  "WHERE vch_table.Comp_No=@c AND Vch_Cancel='False' AND Vch_Date <= @e AND {1} " &
                  "GROUP BY ledger_Name HAVING SUM({0}) > 0 ORDER BY TotalBalance DESC", balanceCalc, groupFilter)

        Using cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@c", comp)
            cmd.Parameters.AddWithValue("@e", periodTo)
            Dim dt As New DataTable()
            dt.Load(cmd.ExecuteReader())
            rpt.DataSource = dt
            rpt.DataBind()
        End Using
    End Sub

    Private Function GetScalar(conn As SqlConnection, sql As String, comp As String, d1 As String, Optional d2 As String = "") As Decimal
        Using cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@cp", comp)
            cmd.Parameters.AddWithValue("@d1", d1)
            If sql.Contains("@d2") Then cmd.Parameters.AddWithValue("@d2", d2)
            Dim result = cmd.ExecuteScalar()
            Return If(result Is Nothing OrElse IsDBNull(result), 0, Convert.ToDecimal(result))
        End Using
    End Function
End Class
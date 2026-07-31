Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports Guna.UI2.WinForms
Imports System.IO
Public Class AccountsForm

    Private GrandTotalBalance As Decimal = 0
    Private mRow As Integer = 0
    Private PageNumber As Integer = 1
    Private IsInDetailView As Boolean = False
    Private SelectedLedgerID_Int As Integer = 0
    Private SelectedAccountName As String = ""

    Private Sub AccountsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ToDate.Value = DateTime.Today
        FromDate.Value = New DateTime(DateTime.Now.Year, DateTime.Now.Month, 1)

        Dim dtps As Guna.UI2.WinForms.Guna2DateTimePicker() = {FromDate, ToDate}
        For Each dtp In dtps
            dtp.Format = DateTimePickerFormat.Custom
            dtp.CustomFormat = "dd-MM-yyyy"
            dtp.FillColor = Color.White
            dtp.ForeColor = Color.Black
            dtp.CheckedState.FillColor = Color.FromArgb(255, 128, 64)
            dtp.CheckedState.ForeColor = Color.White
        Next

        SetupGridDesign()
        LoadAccountSummary()
    End Sub

    Private Sub SetupGridDesign()
        With Guna2DataGridView1
            .AllowUserToAddRows = False
            .ReadOnly = True
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245)
            .ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(34, 40, 49)
            .ThemeStyle.HeaderStyle.ForeColor = Color.White
            .ThemeStyle.HeaderStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .ColumnHeadersHeight = 40
            .RowTemplate.Height = 35
            .Padding = New Padding(0, 0, 0, 35) ' Space for footer bar
        End With
    End Sub

    Private Async Sub LoadAccountSummary()
        IsInDetailView = False
        btnBack.Visible = False

        RefreshButton.Visible = True
        PrintButton.Visible = True

        ProgressBar.Value = 0
        ProgressBar.Visible = True
        GrandTotalBalance = 0

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim query As String = "SELECT L.ID, L.Partyname AS [AccountName], L.Under AS [Group], CAST(ISNULL(L.Opening, 0) + " &
                "ISNULL((SELECT SUM(CASE WHEN V_Type = 'RECEIPT' THEN Amount ELSE -Amount END) FROM Voucher_Table " &
                "WHERE Ledger_ID = L.ID AND Is_Cancelled = 0 AND V_Date < @From), 0) AS DECIMAL(18,2)) AS [OpeningBalance], " &
                "ISNULL((SELECT SUM(Amount) FROM Voucher_Table WHERE Ledger_ID = L.ID AND V_Type = 'RECEIPT' AND Is_Cancelled = 0 AND V_Date >= @From AND V_Date <= @To), 0) AS [Inward], " &
                "ISNULL((SELECT SUM(Amount) FROM Voucher_Table WHERE Ledger_ID = L.ID AND V_Type = 'VOUCHER' AND Is_Cancelled = 0 AND V_Date >= @From AND V_Date <= @To), 0) AS [Outward] " &
                "FROM Ledger_Table L WHERE L.Active = 0 AND (L.Under LIKE '%Cash%' OR L.Under LIKE '%Bank%')"

                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@From", FromDate.Value.Date)
                cmd.Parameters.AddWithValue("@To", ToDate.Value.Date)

                Dim adapter As New SqlDataAdapter(cmd)
                Dim dtSource As New DataTable()
                Await Task.Run(Sub() adapter.Fill(dtSource))

                Dim dtFinal As New DataTable()
                dtFinal.Columns.Add("ID", GetType(Integer))
                dtFinal.Columns.Add("SNo", GetType(Integer))
                dtFinal.Columns.Add("Account Name", GetType(String))
                dtFinal.Columns.Add("Account Type", GetType(String))
                dtFinal.Columns.Add("Opening", GetType(Decimal))
                dtFinal.Columns.Add("Inward (+)", GetType(Decimal))
                dtFinal.Columns.Add("Outward (-)", GetType(Decimal))
                dtFinal.Columns.Add("Current Balance", GetType(Decimal))

                Dim count As Integer = 1
                For Each row As DataRow In dtSource.Rows
                    Dim op As Decimal = Convert.ToDecimal(row("OpeningBalance"))
                    Dim inw As Decimal = Convert.ToDecimal(row("Inward"))
                    Dim outw As Decimal = Convert.ToDecimal(row("Outward"))
                    Dim currentBal As Decimal = (op + inw) - outw

                    dtFinal.Rows.Add(row("ID"), count, row("AccountName"), row("Group"), op, inw, outw, currentBal)
                    GrandTotalBalance += currentBal
                    count += 1
                Next

                Guna2DataGridView1.DataSource = dtFinal
                FormatGrid()
                Guna2DataGridView1.Invalidate()
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ProgressBar.Visible = False
        End Try
    End Sub

    'Private Async Sub LoadAccountSummary()
    '    ProgressBar.Value = 0
    '    ProgressBar.Visible = True
    '    GrandTotalBalance = 0

    '    Try
    '        Using conn As SqlConnection = Tools.GetConnection()
    '            conn.Open()

    '            Dim query As String = "SELECT L.ID, L.Partyname AS [AccountName], L.Under AS [Group], CAST(ISNULL(L.Opening, 0) + 
    '            ISNULL((SELECT SUM(CASE WHEN V_Type = 'RECEIPT' THEN Amount ELSE -Amount END)  FROM Voucher_Table " &
    '            "            WHERE Ledger_ID = L.ID " &
    '            "              AND Is_Cancelled = 0 " &
    '            "              AND V_Date < @From), 0) " &
    '            "AS DECIMAL(18,2)) AS [OpeningBalance], " &
    '            "ISNULL((SELECT SUM(Amount) FROM Voucher_Table " &
    '            "        WHERE Ledger_ID = L.ID " &
    '            "          AND V_Type = 'RECEIPT' " &
    '            "          AND Is_Cancelled = 0 " &
    '            "          AND V_Date >= @From AND V_Date <= @To), 0) AS [Inward], " &
    '            "ISNULL((SELECT SUM(Amount) FROM Voucher_Table " &
    '            "        WHERE Ledger_ID = L.ID " &
    '            "          AND V_Type = 'VOUCHER' " &
    '            "          AND Is_Cancelled = 0 " &
    '            "          AND V_Date >= @From AND V_Date <= @To), 0) AS [Outward] " &
    '            "FROM Ledger_Table L " &
    '            "WHERE L.Active = 0 AND (L.Under LIKE '%Cash%' OR L.Under LIKE '%Bank%')"

    '            Dim cmd As New SqlCommand(query, conn)
    '            cmd.Parameters.AddWithValue("@From", FromDate.Value.Date)
    '            cmd.Parameters.AddWithValue("@To", ToDate.Value.Date)

    '            'Dim cmd As New SqlCommand(query, conn)
    '            'cmd.Parameters.AddWithValue("@To", ToDate.Value.Date)

    '            Dim adapter As New SqlDataAdapter(cmd)
    '            Dim dtSource As New DataTable()
    '            Await Task.Run(Sub() adapter.Fill(dtSource))

    '            ' Process the DataTable to calculate Final Balance
    '            Dim dtFinal As New DataTable()
    '            dtFinal.Columns.Add("SNo", GetType(Integer))
    '            dtFinal.Columns.Add("Account Name", GetType(String))
    '            dtFinal.Columns.Add("Account Type", GetType(String))
    '            dtFinal.Columns.Add("Opening", GetType(Decimal))
    '            dtFinal.Columns.Add("Inward (+)", GetType(Decimal))
    '            dtFinal.Columns.Add("Outward (-)", GetType(Decimal))
    '            dtFinal.Columns.Add("Current Balance", GetType(Decimal))

    '            ProgressBar.Value = 50

    '            Dim count As Integer = 1
    '            For Each row As DataRow In dtSource.Rows
    '                Dim op As Decimal = Convert.ToDecimal(row("OpeningBalance"))
    '                Dim inw As Decimal = Convert.ToDecimal(row("Inward"))
    '                Dim outw As Decimal = Convert.ToDecimal(row("Outward"))
    '                Dim currentBal As Decimal = (op + inw) - outw

    '                dtFinal.Rows.Add(count, row("AccountName"), row("Group"), op, inw, outw, currentBal)

    '                GrandTotalBalance += currentBal
    '                count += 1
    '            Next

    '            Guna2DataGridView1.DataSource = dtFinal
    '            FormatGrid()

    '            ProgressBar.Value = 100
    '            Guna2DataGridView1.Invalidate() ' Redraw footer
    '        End Using
    '    Catch ex As Exception
    '        MessageBox.Show("Error: " & ex.Message)
    '    Finally
    '        ProgressBar.Visible = False
    '    End Try
    'End Sub

    Private Sub FormatGrid()
        If Guna2DataGridView1.Columns.Count > 0 Then
            ' Hide ID Column
            If Guna2DataGridView1.Columns.Contains("ID") Then Guna2DataGridView1.Columns("ID").Visible = False

            Dim numericCols As String() = {"Opening", "Inward (+)", "Outward (-)", "Current Balance"}
            For Each colName In numericCols
                If Guna2DataGridView1.Columns.Contains(colName) Then
                    Guna2DataGridView1.Columns(colName).DefaultCellStyle.Format = "N2"
                    Guna2DataGridView1.Columns(colName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End If
            Next

            Guna2DataGridView1.Columns("SNo").Width = 50
            Guna2DataGridView1.Columns("Account Name").Width = 250
        End If
    End Sub

    Private Sub Guna2DataGridView1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2DataGridView1.Paint
        ' 1. Exit if there are no rows OR if we are in Detail View
        If Guna2DataGridView1.Rows.Count = 0 OrElse IsInDetailView = True Then Exit Sub

        ' 2. Double Check: Ensure the Summary View column actually exists before continuing
        ' This prevents the NullReferenceException during the transition
        If Not Guna2DataGridView1.Columns.Contains("Current Balance") Then Exit Sub

        Dim g As Graphics = e.Graphics
        Dim grid = Guna2DataGridView1
        Dim footerH As Integer = 35
        Dim footerRect As New Rectangle(0, grid.Height - footerH, grid.Width, footerH)

        Using brBg As New SolidBrush(Color.FromArgb(34, 40, 49))
            g.FillRectangle(brBg, footerRect)
        End Using
        g.DrawLine(New Pen(Color.DimGray, 1), 0, footerRect.Top, grid.Width, footerRect.Top)

        Dim fontFooter As New Font("Segoe UI", 10, FontStyle.Bold)

        Try
            ' 3. Safe access now that we've verified the column exists
            Dim lastColIdx As Integer = grid.Columns("Current Balance").Index
            Dim colRect As Rectangle = grid.GetColumnDisplayRectangle(lastColIdx, True)

            g.DrawString("GRAND TOTAL (CASH + ALL BANKS):", fontFooter, Brushes.White, 10, footerRect.Y + 8)

            Dim totalStr As String = GrandTotalBalance.ToString("N2")
            Dim tSize As SizeF = g.MeasureString(totalStr, fontFooter)

            ' Calculate X position relative to the "Current Balance" column width
            Dim xPos As Integer = colRect.X + colRect.Width - tSize.Width - 5

            g.DrawString(totalStr, fontFooter, Brushes.Yellow, xPos, footerRect.Y + 8)
        Catch
            ' Silently catch layout transition glitches
        End Try
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        LoadAccountSummary()
    End Sub

    Private Sub Guna2DataGridView1_Scroll(sender As Object, e As ScrollEventArgs) Handles Guna2DataGridView1.Scroll
        Guna2DataGridView1.Invalidate()
    End Sub
    Private Sub PrintButton_Click(sender As Object, e As EventArgs) Handles PrintButton.Click
        If Guna2DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("No data to save.")
            Return
        End If

        Try
            Dim folderPath As String = Path.Combine(Application.StartupPath, "Report")
            If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

            ' 2. Define File Path
            Dim fileName As String = "Bank_Balance_Summary_" & DateTime.Now.ToString("ddMMyyyy_HHmmss") & ".pdf"
            Dim fullPath As String = Path.Combine(folderPath, fileName)

            ' 3. Configure PDF Printer
            Dim pd As New PrintDocument
            pd.PrintController = New StandardPrintController() ' Hides the "Printing..." dialog
            pd.DefaultPageSettings.PaperSize = New PaperSize("A4", 827, 1169)
            pd.DefaultPageSettings.Margins = New Margins(40, 40, 40, 40)

            pd.PrinterSettings.PrinterName = "Microsoft Print to PDF"
            pd.PrinterSettings.PrintToFile = True
            pd.PrinterSettings.PrintFileName = fullPath

            AddHandler pd.PrintPage, AddressOf PrintDocument_PrintPage

            ' Reset counters
            mRow = 0
            PageNumber = 1

            pd.Print()
            MessageBox.Show("PDF Report saved successfully in 'Report' folder." & vbCrLf & "File: " & fileName, "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub PrintDocument_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics

        ' Fonts
        Dim fInst As New Font("Arial", 14, FontStyle.Bold)
        Dim fHeader As New Font("Arial", 9, FontStyle.Bold) ' Slightly smaller for more columns
        Dim fBody As New Font("Arial", 9, FontStyle.Regular)
        Dim fTotal As New Font("Arial", 11, FontStyle.Bold)

        Dim left As Integer = e.MarginBounds.Left
        Dim y As Integer = e.MarginBounds.Top
        Dim centerX As Integer = e.PageBounds.Width / 2

        ' 1. Institution Header
        Dim compName As String = "ATTMA SEVA ARAKKATTALAI"
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim cmd = New SqlCommand("SELECT Comp_Name FROM Company_Table WHERE Comp_No='BK0002'", conn)
                Dim res = cmd.ExecuteScalar()
                If res IsNot Nothing Then compName = res.ToString().ToUpper()
            End Using
        Catch : End Try

        g.DrawString(compName, fInst, Brushes.Black, centerX - (g.MeasureString(compName, fInst).Width / 2), y)
        y += 30
        g.DrawString("CASH & BANK BALANCE SUMMARY", fHeader, Brushes.Black, centerX - (g.MeasureString("CASH & BANK BALANCE SUMMARY", fHeader).Width / 2), y)
        y += 20
        g.DrawString("Period: " & FromDate.Value.ToString("dd-MM-yyyy") & " to " & ToDate.Value.ToString("dd-MM-yyyy"), fBody, Brushes.DimGray, centerX - (g.MeasureString("Period: " & FromDate.Value.ToString("dd-MM-yyyy") & " to " & ToDate.Value.ToString("dd-MM-yyyy"), fBody).Width / 2), y)
        y += 40

        ' 2. Table Configuration (Widths must sum to ~740 for A4)
        ' SNo(40), Name(180), Opening(130), Inward(130), Outward(130), Balance(130) = 740 Total
        Dim colW As Integer() = {40, 180, 130, 130, 130, 130}
        Dim colN As String() = {"S.No", "Account Name", "Opening", "Inward (+)", "Outward (-)", "Closing Bal"}

        ' Draw Headers
        g.FillRectangle(Brushes.LightGray, left, y, colW.Sum(), 30)
        Dim curX As Integer = left
        For i As Integer = 0 To colN.Length - 1
            g.DrawRectangle(Pens.Black, curX, y, colW(i), 30)
            Dim align = If(i = 1, StringAlignment.Near, StringAlignment.Center)
            Dim sf As New StringFormat() With {.Alignment = align, .LineAlignment = StringAlignment.Center}
            g.DrawString(colN(i), fHeader, Brushes.Black, New RectangleF(curX + 5, y, colW(i) - 10, 30), sf)
            curX += colW(i)
        Next
        y += 30

        ' 3. Data Rows
        Dim rowH As Integer = 35

        While mRow < Guna2DataGridView1.Rows.Count
            Dim row As DataGridViewRow = Guna2DataGridView1.Rows(mRow)
            curX = left

            ' S.No
            g.DrawRectangle(Pens.Black, curX, y, colW(0), rowH)
            g.DrawString((mRow + 1).ToString, fBody, Brushes.Black, New RectangleF(curX, y, colW(0), rowH), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            curX += colW(0)

            ' Account Name
            g.DrawRectangle(Pens.Black, curX, y, colW(1), rowH)
            g.DrawString(row.Cells("Account Name").Value.ToString(), fBody, Brushes.Black, New RectangleF(curX + 5, y, colW(1) - 10, rowH), New StringFormat() With {.LineAlignment = StringAlignment.Center})
            curX += colW(1)

            ' Financial Columns (Opening, Inward, Outward, Balance)
            Dim sfRight As New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}

            ' Opening
            g.DrawRectangle(Pens.Black, curX, y, colW(2), rowH)
            g.DrawString(CDec(row.Cells("Opening").Value).ToString("N2"), fBody, Brushes.Black, New RectangleF(curX, y, colW(2) - 5, rowH), sfRight)
            curX += colW(2)

            ' Inward - Fixed name: "Inward (+)"
            g.DrawRectangle(Pens.Black, curX, y, colW(3), rowH)
            g.DrawString(CDec(row.Cells("Inward (+)").Value).ToString("N2"), fBody, Brushes.Green, New RectangleF(curX, y, colW(3) - 5, rowH), sfRight)
            curX += colW(3)

            ' Outward - Fixed name: "Outward (-)"
            g.DrawRectangle(Pens.Black, curX, y, colW(4), rowH)
            g.DrawString(CDec(row.Cells("Outward (-)").Value).ToString("N2"), fBody, Brushes.Red, New RectangleF(curX, y, colW(4) - 5, rowH), sfRight)
            curX += colW(4)

            ' Current Balance
            g.DrawRectangle(Pens.Black, curX, y, colW(5), rowH)
            g.DrawString(CDec(row.Cells("Current Balance").Value).ToString("N2"), fHeader, Brushes.DarkBlue, New RectangleF(curX, y, colW(5) - 5, rowH), sfRight)

            y += rowH
            mRow += 1

            ' Page Break Logic
            If y > e.MarginBounds.Bottom - 100 Then
                g.DrawString("Page " & PageNumber, fBody, Brushes.Black, e.MarginBounds.Right - 50, e.MarginBounds.Bottom + 10)
                PageNumber += 1
                e.HasMorePages = True
                Return
            End If
        End While

        ' 4. Final Total Box
        curX = left
        ' Sum of first 5 columns to span the label
        Dim labelWidth As Integer = colW(0) + colW(1) + colW(2) + colW(3) + colW(4)

        g.FillRectangle(Brushes.WhiteSmoke, curX, y, labelWidth, rowH + 10)
        g.DrawRectangle(Pens.Black, curX, y, labelWidth, rowH + 10)
        g.DrawString("TOTAL NET WORTH (CLOSING BALANCE)", fTotal, Brushes.Black, curX + 20, y + 12)

        curX += labelWidth
        g.DrawRectangle(Pens.Black, curX, y, colW(5), rowH + 10)

        Dim totalStr As String = GrandTotalBalance.ToString("N2")
        Dim sfTotal As New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
        g.DrawString(totalStr, fTotal, Brushes.Blue, New RectangleF(curX, y, colW(5) - 5, rowH + 10), sfTotal)

        ' Footer
        g.DrawString("Page " & PageNumber, fBody, Brushes.Black, e.MarginBounds.Right - 50, e.MarginBounds.Bottom + 10)
        e.HasMorePages = False
    End Sub

    Private Sub Guna2DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles Guna2DataGridView1.CellDoubleClick
        ' Only Zoom if we are currently in Summary Mode
        If e.RowIndex >= 0 AndAlso IsInDetailView = False Then
            Dim row = Guna2DataGridView1.Rows(e.RowIndex)

            ' FIXED: Pull Integer ID
            SelectedLedgerID_Int = Convert.ToInt32(row.Cells("ID").Value)
            SelectedAccountName = row.Cells("Account Name").Value.ToString()

            LoadLedgerTransactions()
        End If
    End Sub

    Private Sub LoadLedgerTransactions()
        IsInDetailView = True
        btnBack.Visible = True
        RefreshButton.Visible = False
        PrintButton.Visible = False

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()

                Dim sql As String = "SELECT V_Date AS [Date], Bill_No AS [Bill No], " &
                "V_Type AS [Type], Member_Name AS [Party Name], Purpose, " &
                "CASE WHEN V_Type = 'RECEIPT' THEN Amount ELSE 0 END AS [Inward], " &
                "CASE WHEN V_Type = 'VOUCHER' THEN Amount ELSE 0 END AS [Outward] " &
                "FROM Voucher_Table " &
                "WHERE Ledger_ID = @LID AND Is_Cancelled = 0 " &
                "AND V_Date BETWEEN @Start AND @End " &
                "ORDER BY V_Date ASC"

                Dim cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@LID", SelectedLedgerID_Int)
                cmd.Parameters.AddWithValue("@Start", FromDate.Value.Date)
                cmd.Parameters.AddWithValue("@End", ToDate.Value.Date)

                Dim adapter As New SqlDataAdapter(cmd)
                Dim dt As New DataTable()
                adapter.Fill(dt)

                ' Calculate Running Balance
                dt.Columns.Add("Running Balance", GetType(Decimal))
                Dim runningBal As Decimal = 0
                For Each row As DataRow In dt.Rows
                    runningBal += Convert.ToDecimal(row("Inward"))
                    runningBal -= Convert.ToDecimal(row("Outward"))
                    row("Running Balance") = runningBal
                Next

                Guna2DataGridView1.DataSource = dt
                FormatDetailGrid()
            End Using
        Catch ex As Exception
            MessageBox.Show("Detail Load Error: " & ex.Message)
        End Try
    End Sub

    Private Sub btnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        IsInDetailView = False
        btnBack.Visible = False
        LoadAccountSummary()
    End Sub

    Private Sub FormatDetailGrid()
        If Guna2DataGridView1.Columns.Count > 0 Then
            Guna2DataGridView1.Columns("Inward").DefaultCellStyle.Format = "N2"
            Guna2DataGridView1.Columns("Outward").DefaultCellStyle.Format = "N2"
            Guna2DataGridView1.Columns("Running Balance").DefaultCellStyle.Format = "N2"

            Guna2DataGridView1.Columns("Inward").DefaultCellStyle.ForeColor = Color.Green
            Guna2DataGridView1.Columns("Outward").DefaultCellStyle.ForeColor = Color.Red
            Guna2DataGridView1.Columns("Running Balance").DefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            Guna2DataGridView1.Columns("Running Balance").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
        End If
    End Sub

End Class
Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports Guna.UI2.WinForms
Imports System.IO
Public Class AccountsForm

    Private GrandTotalBalance As Decimal = 0
    Private mRow As Integer = 0
    Private PageNumber As Integer = 1

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
        ProgressBar.Value = 0
        ProgressBar.Visible = True
        GrandTotalBalance = 0

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()

                Dim query As String = "SELECT " &
                    "L.ID, " &
                    "L.Partyname AS [AccountName], " &
                    "L.Under AS [Group], " &
                    "CAST(ISNULL(L.Opening, 0) AS DECIMAL(18,2)) AS [OpeningBalance], " &
                    "ISNULL((SELECT SUM(Amount) FROM Voucher_Table WHERE Ledger_ID = L.ID AND V_Type = 'RECEIPT' AND Is_Cancelled = 0 AND V_Date <= @To), 0) AS [Inward], " &
                    "ISNULL((SELECT SUM(Amount) FROM Voucher_Table WHERE Ledger_ID = L.ID AND V_Type = 'VOUCHER' AND Is_Cancelled = 0 AND V_Date <= @To), 0) AS [Outward] " &
                    "FROM Ledger_Table L " &
                    "WHERE L.Active = 0 AND (L.Under LIKE '%Cash%' OR L.Under LIKE '%Bank%')"

                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@To", ToDate.Value.Date)

                Dim adapter As New SqlDataAdapter(cmd)
                Dim dtSource As New DataTable()
                Await Task.Run(Sub() adapter.Fill(dtSource))

                ' Process the DataTable to calculate Final Balance
                Dim dtFinal As New DataTable()
                dtFinal.Columns.Add("SNo", GetType(Integer))
                dtFinal.Columns.Add("Account Name", GetType(String))
                dtFinal.Columns.Add("Account Type", GetType(String))
                dtFinal.Columns.Add("Opening", GetType(Decimal))
                dtFinal.Columns.Add("Inward (+)", GetType(Decimal))
                dtFinal.Columns.Add("Outward (-)", GetType(Decimal))
                dtFinal.Columns.Add("Current Balance", GetType(Decimal))

                ProgressBar.Value = 50

                Dim count As Integer = 1
                For Each row As DataRow In dtSource.Rows
                    Dim op As Decimal = Convert.ToDecimal(row("OpeningBalance"))
                    Dim inw As Decimal = Convert.ToDecimal(row("Inward"))
                    Dim outw As Decimal = Convert.ToDecimal(row("Outward"))
                    Dim currentBal As Decimal = (op + inw) - outw

                    dtFinal.Rows.Add(count, row("AccountName"), row("Group"), op, inw, outw, currentBal)

                    GrandTotalBalance += currentBal
                    count += 1
                Next

                Guna2DataGridView1.DataSource = dtFinal
                FormatGrid()

                ProgressBar.Value = 100
                Guna2DataGridView1.Invalidate() ' Redraw footer
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        Finally
            ProgressBar.Visible = False
        End Try
    End Sub

    Private Sub FormatGrid()
        If Guna2DataGridView1.Columns.Count > 0 Then
            ' Align numeric columns
            Dim numericCols As String() = {"Opening", "Inward (+)", "Outward (-)", "Current Balance"}
            For Each colName In numericCols
                Guna2DataGridView1.Columns(colName).DefaultCellStyle.Format = "N2"
                Guna2DataGridView1.Columns(colName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Next

            Guna2DataGridView1.Columns("SNo").Width = 50
            Guna2DataGridView1.Columns("Account Name").Width = 250
            Guna2DataGridView1.Columns("Current Balance").DefaultCellStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
        End If
    End Sub

    Private Sub Guna2DataGridView1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2DataGridView1.Paint
        If Guna2DataGridView1.Rows.Count = 0 Then Exit Sub

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
            Dim lastColIdx As Integer = grid.Columns("Current Balance").Index
            Dim colRect As Rectangle = grid.GetColumnDisplayRectangle(lastColIdx, True)

            g.DrawString("GRAND TOTAL (CASH + ALL BANKS):", fontFooter, Brushes.White, 10, footerRect.Y + 8)

            Dim totalStr As String = GrandTotalBalance.ToString("N2")
            Dim tSize As SizeF = g.MeasureString(totalStr, fontFooter)
            Dim xPos As Integer = colRect.X + colRect.Width - tSize.Width - 5

            g.DrawString(totalStr, fontFooter, Brushes.Yellow, xPos, footerRect.Y + 8)
        Catch
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
        Dim fHeader As New Font("Arial", 10, FontStyle.Bold)
        Dim fBody As New Font("Arial", 10, FontStyle.Regular)
        Dim fSno As New Font("Arial", 10, FontStyle.Bold)
        Dim fTotal As New Font("Arial", 12, FontStyle.Bold)

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
        g.DrawString("Date: " & DateTime.Now.ToString("dd-MM-yyyy"), fBody, Brushes.DimGray, centerX - (g.MeasureString("Date: " & DateTime.Now.ToString("dd-MM-yyyy"), fBody).Width / 2), y)
        y += 40

        ' 2. Table Headers (Grid Layout)
        ' Total width around 740
        Dim colW As Integer() = {60, 400, 280}
        Dim colN As String() = {"S.No", "Account Name", "Current Balance"}

        g.FillRectangle(Brushes.LightGray, left, y, colW.Sum, 30)
        Dim curX As Integer = left
        For i As Integer = 0 To colN.Length - 1
            g.DrawRectangle(Pens.Black, curX, y, colW(i), 30)
            ' Center alignment for Sno and Balance headers
            Dim align = If(i = 1, StringAlignment.Near, StringAlignment.Center)
            Dim sf As New StringFormat() With {.Alignment = align, .LineAlignment = StringAlignment.Center}
            g.DrawString(colN(i), fHeader, Brushes.Black, New RectangleF(curX + 5, y, colW(i) - 10, 30), sf)
            curX += colW(i)
        Next
        y += 30

        ' 3. Data Rows
        Dim rowH As Integer = 40 ' Smaller height than member register since no photo

        While mRow < Guna2DataGridView1.Rows.Count
            Dim row As DataGridViewRow = Guna2DataGridView1.Rows(mRow)
            curX = left

            ' Box 1: S.No
            g.DrawRectangle(Pens.Black, curX, y, colW(0), rowH)
            g.DrawString((mRow + 1).ToString & ".", fBody, Brushes.Black, curX + 15, y + 10)
            curX += colW(0)

            ' Box 2: Account Name
            g.DrawRectangle(Pens.Black, curX, y, colW(1), rowH)
            g.DrawString(row.Cells("Account Name").Value.ToString(), fBody, Brushes.Black, curX + 10, y + 10)
            curX += colW(1)

            ' Box 3: Current Balance
            g.DrawRectangle(Pens.Black, curX, y, colW(2), rowH)
            Dim balAmt As Decimal = CDec(row.Cells("Current Balance").Value)
            Dim balStr As String = "Rs. " & balAmt.ToString("N2")
            ' Right-aligned balance
            Dim sfRight As New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
            g.DrawString(balStr, fHeader, Brushes.DarkBlue, New RectangleF(curX, y, colW(2) - 10, rowH), sfRight)

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
        g.FillRectangle(Brushes.WhiteSmoke, curX, y, colW(0) + colW(1), rowH + 10)
        g.DrawRectangle(Pens.Black, curX, y, colW(0) + colW(1), rowH + 10)
        g.DrawString("TOTAL NET WORTH", fTotal, Brushes.Black, curX + 20, y + 12)

        curX += colW(0) + colW(1)
        g.DrawRectangle(Pens.Black, curX, y, colW(2), rowH + 10)

        ' Assuming GrandTotalBalance is a variable in your class
        Dim totalStr As String = "Rs. " & GrandTotalBalance.ToString("N2")
        Dim sfTotal As New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
        g.DrawString(totalStr, fTotal, Brushes.Blue, New RectangleF(curX, y, colW(2) - 10, rowH + 10), sfTotal)

        ' Footer
        g.DrawString("Page " & PageNumber, fBody, Brushes.Black, e.MarginBounds.Right - 50, e.MarginBounds.Bottom + 10)
        e.HasMorePages = False
    End Sub

End Class
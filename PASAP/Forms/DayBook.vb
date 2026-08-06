Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing.Printing
Imports Guna.UI2.WinForms

Public Class DayBook
    Private OpeningBalance As Decimal = 0
    Private TotalDebit As Decimal = 0
    Private TotalCredit As Decimal = 0
    Private ClosingBalance As Decimal = 0

    Private mRow As Integer = 0
    Private PageNumber As Integer = 1

    Private Sub DayBook_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FromDate.Value = DateTime.Today
        ToDate.Value = DateTime.Today

        Dim dtps As Guna.UI2.WinForms.Guna2DateTimePicker() = {FromDate, ToDate}
        For Each dtp In dtps
            dtp.Format = DateTimePickerFormat.Custom
            dtp.CustomFormat = "dd-MM-yyyy"
            dtp.FillColor = Color.White
            dtp.ForeColor = Color.Black
            dtp.BorderThickness = 1
            dtp.BorderColor = Color.LightGray
        Next

        SetupGrid()
        LoadDayBookData()
    End Sub

    Private Sub SetupGrid()
        With Guna2DataGridView1
            .AllowUserToAddRows = False
            .ReadOnly = True
            .ColumnHeadersVisible = True
            .ColumnHeadersHeight = 40
            .ThemeStyle.HeaderStyle.Height = 40
            .ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(34, 40, 49)
            .ThemeStyle.HeaderStyle.ForeColor = Color.White
            .ThemeStyle.HeaderStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)

            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 245, 255)
            .RowTemplate.Height = 35
            .Padding = New Padding(0, 0, 0, 40)
        End With
    End Sub

    Private Async Sub LoadDayBookData()
        OpeningBalance = 0
        TotalDebit = 0
        TotalCredit = 0
        ClosingBalance = 0

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                Await conn.OpenAsync()

                ' 1. Calculate Opening Balance
                Dim sqlOpening = "SELECT ISNULL(SUM(CASE WHEN V_Type='RECEIPT' THEN Amount ELSE -Amount END), 0) " &
                               "FROM Voucher_Table WHERE Is_Cancelled=0 AND V_Date < @From"
                Using cmdOp = New SqlCommand(sqlOpening, conn)
                    cmdOp.Parameters.AddWithValue("@From", FromDate.Value.Date)
                    OpeningBalance = Convert.ToDecimal(Await cmdOp.ExecuteScalarAsync())
                End Using

                ' 2. Load Transactions
                Dim sqlTrans = "SELECT V_Date, Member_Name, Purpose, V_Type, Amount " &
                             "FROM Voucher_Table WHERE Is_Cancelled=0 " &
                             "AND V_Date BETWEEN @From AND @To ORDER BY V_Date ASC, Created_Date ASC"

                Dim dtTrans As New DataTable()
                Using cmdT = New SqlCommand(sqlTrans, conn)
                    cmdT.Parameters.AddWithValue("@From", FromDate.Value.Date)
                    cmdT.Parameters.AddWithValue("@To", ToDate.Value.Date)
                    Dim adapter As New SqlDataAdapter(cmdT)
                    Await Task.Run(Sub() adapter.Fill(dtTrans))
                End Using

                ' 3. Prepare Final DataTable for Grid
                Dim dtFinal As New DataTable()
                dtFinal.Columns.Add("Date", GetType(String))
                dtFinal.Columns.Add("Particulars", GetType(String))
                dtFinal.Columns.Add("Type", GetType(String))
                dtFinal.Columns.Add("Debit (-)", GetType(Decimal))
                dtFinal.Columns.Add("Credit (+)", GetType(Decimal))
                dtFinal.Columns.Add("Balance", GetType(Decimal))

                ' Add Opening Balance Row
                dtFinal.Rows.Add(FromDate.Value.ToString("dd-MM-yyyy"), "OPENING BALANCE", "---", 0, 0, OpeningBalance)

                Dim runningBal As Decimal = OpeningBalance
                For Each row As DataRow In dtTrans.Rows
                    Dim amt = Convert.ToDecimal(row("Amount"))
                    Dim vType = row("V_Type").ToString()

                    Dim deb As Decimal = 0
                    Dim cre As Decimal = 0

                    If vType = "VOUCHER" Then
                        deb = amt
                        runningBal -= amt
                        TotalDebit += amt
                    Else
                        cre = amt
                        runningBal += amt
                        TotalCredit += amt
                    End If

                    dtFinal.Rows.Add(Convert.ToDateTime(row("V_Date")).ToString("dd-MM-yyyy"),
                                   row("Member_Name").ToString() & " (" & row("Purpose").ToString() & ")",
                                   vType, deb, cre, runningBal)
                Next

                ClosingBalance = runningBal
                Guna2DataGridView1.DataSource = dtFinal
                FormatGridColumns()
                Guna2DataGridView1.Invalidate()

            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub FormatGridColumns()
        If Guna2DataGridView1.Columns.Count > 0 Then
            Guna2DataGridView1.Columns("Particulars").Width = 280
            Dim moneyCols As String() = {"Debit (-)", "Credit (+)", "Balance"}
            For Each col In moneyCols
                Guna2DataGridView1.Columns(col).DefaultCellStyle.Format = "N2"
                Guna2DataGridView1.Columns(col).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Next
        End If
    End Sub

    Private Sub Guna2DataGridView1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2DataGridView1.Paint
        If Guna2DataGridView1.Rows.Count = 0 Then Exit Sub

        If Not Guna2DataGridView1.Columns.Contains("Debit (-)") OrElse
       Not Guna2DataGridView1.Columns.Contains("Credit (+)") OrElse
       Not Guna2DataGridView1.Columns.Contains("Balance") Then Exit Sub

        Dim g As Graphics = e.Graphics
        Dim footerH As Integer = 40
        Dim rectFooter As New Rectangle(0, Guna2DataGridView1.Height - footerH, Guna2DataGridView1.Width, footerH)

        Using br As New SolidBrush(Color.FromArgb(34, 40, 49))
            g.FillRectangle(br, rectFooter)
        End Using
        g.DrawLine(Pens.DimGray, 0, rectFooter.Top, Guna2DataGridView1.Width, rectFooter.Top)

        Dim fnt As New Font("Segoe UI", 10, FontStyle.Bold)
        Dim yPos As Integer = rectFooter.Y + 10

        Try
            Dim rectDeb = Guna2DataGridView1.GetColumnDisplayRectangle(Guna2DataGridView1.Columns("Debit (-)").Index, True)
            Dim rectCre = Guna2DataGridView1.GetColumnDisplayRectangle(Guna2DataGridView1.Columns("Credit (+)").Index, True)
            Dim rectBal = Guna2DataGridView1.GetColumnDisplayRectangle(Guna2DataGridView1.Columns("Balance").Index, True)

            g.DrawString("TOTALS:", fnt, Brushes.White, 10, yPos)

            Dim sfRight As New StringFormat() With {.Alignment = StringAlignment.Far}

            g.DrawString(TotalDebit.ToString("N2"), fnt, Brushes.Tomato,
                     New RectangleF(rectDeb.X, yPos, rectDeb.Width - 5, 25), sfRight)

            g.DrawString(TotalCredit.ToString("N2"), fnt, Brushes.LightGreen,
                     New RectangleF(rectCre.X, yPos, rectCre.Width - 5, 25), sfRight)

            Dim closingText As String = "Closing: " & ClosingBalance.ToString("N2")
            g.DrawString(closingText, fnt, Brushes.Yellow,
                     New RectangleF(rectBal.X, yPos, rectBal.Width - 5, 25), sfRight)

        Catch
            ' Silently catch glitches during grid resizing/scrolling
        End Try
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        LoadDayBookData()
    End Sub

    Private Sub PrintButton_Click(sender As Object, e As EventArgs) Handles PrintButton.Click
        If Guna2DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("No data to export.")
            Return
        End If

        Try
            Dim folderPath As String = Path.Combine(Application.StartupPath, "Report")
            If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

            Dim fileName As String = "DayBook_" & DateTime.Now.ToString("ddMMyyyy_HHmmss") & ".pdf"
            Dim fullPath As String = Path.Combine(folderPath, fileName)

            Dim pd As New PrintDocument
            pd.PrintController = New StandardPrintController()
            pd.DefaultPageSettings.PaperSize = New PaperSize("A4", 827, 1169) ' A4 Size
            pd.DefaultPageSettings.Margins = New Margins(40, 40, 40, 40)

            pd.PrinterSettings.PrinterName = "Microsoft Print to PDF"
            pd.PrinterSettings.PrintToFile = True
            pd.PrinterSettings.PrintFileName = fullPath

            AddHandler pd.PrintPage, AddressOf PrintDocument_PrintPage

            mRow = 0
            PageNumber = 1

            pd.Print()
            MessageBox.Show("DayBook PDF saved successfully in 'Report' folder." & vbCrLf & "File: " & fileName, "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Export Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrintDocument_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        ' Fonts
        Dim fInst As New Font("Arial", 14, FontStyle.Bold)
        Dim fHeader As New Font("Arial", 9, FontStyle.Bold)
        Dim fBody As New Font("Arial", 8, FontStyle.Regular)
        Dim fTotal As New Font("Arial", 10, FontStyle.Bold)

        Dim left As Integer = e.MarginBounds.Left
        Dim y As Integer = e.MarginBounds.Top
        Dim centerX As Integer = e.PageBounds.Width / 2

        ' 1. Institution Header
        Dim compName As String = "PERUNDURAI AATHMA SEVA ARAKATALLAI"
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
        g.DrawString("DAYBOOK REPORT", fHeader, Brushes.Black, centerX - (g.MeasureString("DAYBOOK REPORT", fHeader).Width / 2), y)
        y += 20
        g.DrawString("Period: " & FromDate.Value.ToString("dd-MM-yyyy") & " to " & ToDate.Value.ToString("dd-MM-yyyy"), fBody, Brushes.DimGray, centerX - (g.MeasureString("Period: " & FromDate.Value.ToString("dd-MM-yyyy") & " to " & ToDate.Value.ToString("dd-MM-yyyy"), fBody).Width / 2), y)
        y += 40

        Dim colW As Integer() = {80, 235, 65, 120, 120, 120}
        Dim colN As String() = {"Date", "Particulars", "Type", "Debit (-)", "Credit (+)", "Balance"}

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
        Dim rowH As Integer = 30
        Dim sfRight As New StringFormat() With {.Alignment = StringAlignment.Far, .LineAlignment = StringAlignment.Center}
        Dim sfLeft As New StringFormat() With {.LineAlignment = StringAlignment.Center}

        While mRow < Guna2DataGridView1.Rows.Count
            Dim row As DataGridViewRow = Guna2DataGridView1.Rows(mRow)
            curX = left

            ' Date
            g.DrawRectangle(Pens.Black, curX, y, colW(0), rowH)
            g.DrawString(row.Cells("Date").Value.ToString(), fBody, Brushes.Black, New RectangleF(curX, y, colW(0), rowH), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            curX += colW(0)

            ' Particulars (Trimming if too long)
            g.DrawRectangle(Pens.Black, curX, y, colW(1), rowH)
            Dim partTxt As String = row.Cells("Particulars").Value.ToString()
            If partTxt.Length > 40 Then partTxt = partTxt.Substring(0, 37) & "..."
            g.DrawString(partTxt, fBody, Brushes.Black, New RectangleF(curX + 5, y, colW(1) - 10, rowH), sfLeft)
            curX += colW(1)

            ' Type
            g.DrawRectangle(Pens.Black, curX, y, colW(2), rowH)
            g.DrawString(row.Cells("Type").Value.ToString(), fBody, Brushes.Black, New RectangleF(curX, y, colW(2), rowH), New StringFormat() With {.Alignment = StringAlignment.Center, .LineAlignment = StringAlignment.Center})
            curX += colW(2)

            ' Debit
            g.DrawRectangle(Pens.Black, curX, y, colW(3), rowH)
            Dim debVal As Decimal = CDec(row.Cells("Debit (-)").Value)
            g.DrawString(If(debVal = 0, "-", debVal.ToString("N2")), fBody, If(debVal > 0, Brushes.Red, Brushes.Black), New RectangleF(curX, y, colW(3) - 5, rowH), sfRight)
            curX += colW(3)

            ' Credit
            g.DrawRectangle(Pens.Black, curX, y, colW(4), rowH)
            Dim creVal As Decimal = CDec(row.Cells("Credit (+)").Value)
            g.DrawString(If(creVal = 0, "-", creVal.ToString("N2")), fBody, If(creVal > 0, Brushes.Green, Brushes.Black), New RectangleF(curX, y, colW(4) - 5, rowH), sfRight)
            curX += colW(4)

            ' Balance
            g.DrawRectangle(Pens.Black, curX, y, colW(5), rowH)
            g.DrawString(CDec(row.Cells("Balance").Value).ToString("N2"), fHeader, Brushes.DarkBlue, New RectangleF(curX, y, colW(5) - 5, rowH), sfRight)

            y += rowH
            mRow += 1

            ' Pagination check
            If y > e.MarginBounds.Bottom - 80 Then
                g.DrawString("Page " & PageNumber, fBody, Brushes.Black, e.MarginBounds.Right - 50, e.MarginBounds.Bottom + 10)
                PageNumber += 1
                e.HasMorePages = True
                Return
            End If
        End While

        curX = left
        Dim labelSpanW As Integer = colW(0) + colW(1) + colW(2)

        g.FillRectangle(Brushes.WhiteSmoke, curX, y, labelSpanW, rowH + 10)
        g.DrawRectangle(Pens.Black, curX, y, labelSpanW, rowH + 10)
        g.DrawString("TOTALS / CLOSING BALANCE", fTotal, Brushes.Black, curX + 10, y + 10)
        curX += labelSpanW

        ' Debit Total
        g.DrawRectangle(Pens.Black, curX, y, colW(3), rowH + 10)
        g.DrawString(TotalDebit.ToString("N2"), fTotal, Brushes.Red, New RectangleF(curX, y, colW(3) - 5, rowH + 10), sfRight)
        curX += colW(3)

        ' Credit Total
        g.DrawRectangle(Pens.Black, curX, y, colW(4), rowH + 10)
        g.DrawString(TotalCredit.ToString("N2"), fTotal, Brushes.Green, New RectangleF(curX, y, colW(4) - 5, rowH + 10), sfRight)
        curX += colW(4)

        ' Net Closing Balance
        g.DrawRectangle(Pens.Black, curX, y, colW(5), rowH + 10)
        g.DrawString(ClosingBalance.ToString("N2"), fTotal, Brushes.Blue, New RectangleF(curX, y, colW(5) - 5, rowH + 10), sfRight)

        ' Print User & Page Info
        g.DrawString("Printed by: " & Tools.GetStoredUsername(), fBody, Brushes.DimGray, left, e.MarginBounds.Bottom + 10)
        g.DrawString("Page " & PageNumber, fBody, Brushes.Black, e.MarginBounds.Right - 50, e.MarginBounds.Bottom + 10)

        e.HasMorePages = False
    End Sub
End Class

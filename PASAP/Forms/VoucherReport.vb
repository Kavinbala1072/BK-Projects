Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports System.IO
Public Class VoucherReport

    Private TotalReceivable As Decimal = 0
    Private TotalPayable As Decimal = 0
    Private NetTotalUI As Decimal = 0

    Private mRowReceipt As Integer = 0
    Private mRowVoucher As Integer = 0
    Private PageNumber As Integer = 1
    Private sNoReceipt As Integer = 0
    Private sNoVoucher As Integer = 0

    Private Sub VoucherReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        FromDateTextBox.Text = DateTime.Today.AddDays(-30).ToString("dd-MM-yyyy")
        ToDateTextBox.Text = DateTime.Today.ToString("dd-MM-yyyy")

        InitializeGridDesign()

        TypeCombo.Items.Clear()
        TypeCombo.Items.AddRange({"ALL", "RECEIPT", "VOUCHER"})

        PaymentCombo.Items.Clear()
        PaymentCombo.Items.AddRange({"ALL", "Cash", "Phone Pay", "Google Pay", "NEFT", "Cheque"})

        TypeCombo.SelectedIndex = 0
        PaymentCombo.SelectedIndex = 0
    End Sub

    Private Sub InitializeGridDesign()
        With Guna2DataGridView1
            .AllowUserToAddRows = False
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245)
            .ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(34, 40, 49)
            .ThemeStyle.HeaderStyle.ForeColor = Color.White
            .ThemeStyle.HeaderStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .ColumnHeadersHeight = 40
            .RowTemplate.Height = 32
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .GridColor = Color.FromArgb(231, 229, 255)
            .ThemeStyle.GridColor = Color.FromArgb(231, 229, 255)
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
            .ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.Single
            .ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.Single
            .Padding = New Padding(0, 0, 0, 35)
        End With
    End Sub

    Private Sub LoadReportData()

        ProgressBar.Value = 0
        ProgressBar.Visible = True

        TotalReceivable = 0
        TotalPayable = 0
        NetTotalUI = 0

        Dim dt As New DataTable()
        dt.Columns.Add("Bill_No", GetType(String))
        dt.Columns.Add("V_Date", GetType(DateTime))
        dt.Columns.Add("V_Type", GetType(String))
        dt.Columns.Add("Member_Name", GetType(String))
        dt.Columns.Add("Amount", GetType(Decimal))
        dt.Columns.Add("Purpose", GetType(String))
        dt.Columns.Add("Payment_Method", GetType(String))
        dt.Columns.Add("Is_Cancelled", GetType(Integer))

        Dim dFrom, dTo As DateTime
        Dim dateFormat As String = "dd-MM-yyyy"

        If Not DateTime.TryParseExact(FromDateTextBox.Text.Trim(), dateFormat, Nothing, Globalization.DateTimeStyles.None, dFrom) Then
            If Not DateTime.TryParse(FromDateTextBox.Text, dFrom) Then
                MessageBox.Show("Invalid Start Date! Please use format: dd-MM-yyyy", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ProgressBar.Visible = False
                Return
            End If
        End If

        If Not DateTime.TryParseExact(ToDateTextBox.Text.Trim(), dateFormat, Nothing, Globalization.DateTimeStyles.None, dTo) Then
            If Not DateTime.TryParse(ToDateTextBox.Text, dTo) Then
                MessageBox.Show("Invalid End Date! Please use format: dd-MM-yyyy", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ProgressBar.Visible = False
                Return
            End If
        End If

        If dFrom.Year < 1753 Or dTo.Year < 1753 Then
            MessageBox.Show("Selected dates are out of range for the database (Must be after year 1753).", "Date Overflow", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            ProgressBar.Visible = False
            Return
        End If

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()

                Dim filterSql As String = "WHERE V.V_Date BETWEEN @from AND @to AND V.Is_Cancelled = 0 "

                If TypeCombo.Text <> "ALL" Then
                    filterSql &= " AND V.V_Type = @type "
                End If

                If PaymentCombo.Text <> "ALL" Then
                    filterSql &= " AND V.Payment_Method = @payment "
                End If

                Dim totalRecords As Integer = 0
                Using cmdCount As New SqlCommand("SELECT COUNT(*) FROM Voucher_Table V " & filterSql, conn)
                    cmdCount.Parameters.AddWithValue("@from", dFrom)
                    cmdCount.Parameters.AddWithValue("@to", dTo)
                    If TypeCombo.Text <> "ALL" Then cmdCount.Parameters.AddWithValue("@type", TypeCombo.Text)
                    If PaymentCombo.Text <> "ALL" Then cmdCount.Parameters.AddWithValue("@payment", PaymentCombo.Text)

                    totalRecords = Convert.ToInt32(cmdCount.ExecuteScalar())
                End Using

                ProgressBar.Maximum = If(totalRecords > 0, totalRecords, 1)

                Dim query As String = "SELECT V.Bill_No, V.V_Date, V.V_Type, " &
                                 "ISNULL(V.Member_Name, M.Member_Name) AS Member_Name, " &
                                 "V.Amount, V.Purpose, V.Payment_Method, V.Is_Cancelled " &
                                 "FROM Voucher_Table V " &
                                 "LEFT JOIN Member_Table M ON V.Member_ID = M.ID " &
                                 filterSql & " ORDER BY V.V_Type DESC, V.V_Date ASC"

                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@from", dFrom)
                    cmd.Parameters.AddWithValue("@to", dTo)
                    If TypeCombo.Text <> "ALL" Then cmd.Parameters.AddWithValue("@type", TypeCombo.Text)
                    If PaymentCombo.Text <> "ALL" Then cmd.Parameters.AddWithValue("@payment", PaymentCombo.Text)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        Dim count As Integer = 0
                        While reader.Read()
                            count += 1
                            Dim row As DataRow = dt.NewRow()
                            row("Bill_No") = reader("Bill_No")
                            row("V_Date") = reader("V_Date")
                            row("V_Type") = reader("V_Type")

                            Dim displayName As String = reader("Member_Name").ToString()
                            row("Member_Name") = If(String.IsNullOrEmpty(displayName), "Walk-in", displayName)

                            Dim amt As Decimal = Convert.ToDecimal(reader("Amount"))
                            row("Amount") = amt
                            row("Purpose") = reader("Purpose").ToString()
                            row("Payment_Method") = reader("Payment_Method").ToString()
                            row("Is_Cancelled") = reader("Is_Cancelled")

                            dt.Rows.Add(row)

                            If reader("V_Type").ToString() = "RECEIPT" Then
                                TotalReceivable += amt
                            Else
                                TotalPayable += amt
                            End If

                            If count Mod 10 = 0 Then
                                ProgressBar.Value = count
                                Application.DoEvents()
                            End If
                        End While
                    End Using
                End Using

                NetTotalUI = TotalReceivable - TotalPayable
                Guna2DataGridView1.DataSource = dt

                If Guna2DataGridView1.Columns.Count > 0 Then
                    Guna2DataGridView1.Columns("V_Date").HeaderText = "Date"
                    Guna2DataGridView1.Columns("V_Date").DefaultCellStyle.Format = "dd-MM-yyyy"
                    Guna2DataGridView1.Columns("Amount").DefaultCellStyle.Format = "N2"
                    Guna2DataGridView1.Columns("Is_Cancelled").Visible = False
                End If

                Guna2DataGridView1.Invalidate()

            End Using
        Catch ex As Exception
            MessageBox.Show("Data Load Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            ProgressBar.Value = ProgressBar.Maximum
            ProgressBar.Visible = False
        End Try
    End Sub

    Private Sub Guna2DataGridView1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2DataGridView1.Paint
        If Guna2DataGridView1.Rows.Count = 0 Then Exit Sub

        Dim g As Graphics = e.Graphics
        Dim grid = Guna2DataGridView1
        Dim footerH As Integer = 35
        Dim footerRect As New Rectangle(0, grid.Height - footerH, grid.Width, footerH)

        g.FillRectangle(New SolidBrush(Color.FromArgb(34, 40, 49)), footerRect)
        g.DrawLine(New Pen(Color.White, 1), 0, footerRect.Top, grid.Width, footerRect.Top)


        Dim fontFooter As New Font("Segoe UI", 10, FontStyle.Bold)

        Try
            Dim nameX As Integer = grid.GetColumnDisplayRectangle(grid.Columns("Member_Name").Index, True).X
            Dim amtX As Integer = grid.GetColumnDisplayRectangle(grid.Columns("Amount").Index, True).X

            Dim statusText As String = $"Receivable: {TotalReceivable:N2} | Payable: {TotalPayable:N2} | Net:"
            g.DrawString(statusText, fontFooter, Brushes.White, 10, footerRect.Y + 8)
            g.DrawString(NetTotalUI.ToString("N2"), fontFooter, Brushes.Yellow, amtX, footerRect.Y + 8)
        Catch
            g.DrawString($"Net Total: {NetTotalUI:N2}", fontFooter, Brushes.Yellow, 10, footerRect.Y + 8)
        End Try
    End Sub

    Private Sub Guna2DataGridView1_Scroll(sender As Object, e As ScrollEventArgs) Handles Guna2DataGridView1.Scroll
        Guna2DataGridView1.Invalidate()
    End Sub
    Private Sub PaymentCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PaymentCombo.SelectedIndexChanged
        LoadReportData()
    End Sub

    Private Sub TypeCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TypeCombo.SelectedIndexChanged
        LoadReportData()
    End Sub
    Private Sub FromDateTextBox_Leave(sender As Object, e As EventArgs) Handles FromDateTextBox.Leave, ToDateTextBox.Leave
        LoadReportData()
    End Sub

    Private Sub PrintButton_Click(sender As Object, e As EventArgs) Handles PrintButton.Click
        If Guna2DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("No data to save.")
            Return
        End If

        Try
            Dim folderPath As String = Path.Combine(Application.StartupPath, "Report")
            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If

            Dim fileName As String = "Voucher_Report_" & DateTime.Now.ToString("ddMMyyyy_HHmmss") & ".pdf"
            Dim fullPath As String = Path.Combine(folderPath, fileName)

            Dim pd As New PrintDocument
            pd.PrintController = New StandardPrintController()

            pd.DefaultPageSettings.PaperSize = New PaperSize("A4", 827, 1169)
            pd.DefaultPageSettings.Margins = New Margins(40, 40, 40, 40)

            pd.PrinterSettings.PrinterName = "Microsoft Print to PDF"
            pd.PrinterSettings.PrintToFile = True
            pd.PrinterSettings.PrintFileName = fullPath

            AddHandler pd.PrintPage, AddressOf PrintDocument_PrintPage

            mRowReceipt = 0
            mRowVoucher = 0
            PageNumber = 1
            TotalReceivable = 0
            TotalPayable = 0
            sNoReceipt = 0
            sNoVoucher = 0

            pd.Print()
            MessageBox.Show("Report saved successfully in 'Report' folder." & vbCrLf & "File: " & fileName, "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub PrintDocument_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        Dim fTitle As New Font("Arial", 14, FontStyle.Bold)
        Dim fHeader As New Font("Arial", 11, FontStyle.Bold)
        Dim fBody As New Font("Arial", 9, FontStyle.Regular)

        Dim left As Integer = e.MarginBounds.Left
        Dim y As Integer = e.MarginBounds.Top
        Dim center As Integer = e.PageBounds.Width / 2

        Dim compName As String = "ATTMA SEVA ARAKKATTALAI"
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim res = New SqlCommand("SELECT Comp_Name FROM Company_Table WHERE Comp_No='BK0002'", conn).ExecuteScalar()
                If res IsNot Nothing Then compName = res.ToString().ToUpper()
            End Using
        Catch : End Try

        If PageNumber = 1 Then
            g.DrawString(compName, fTitle, Brushes.Black, center - (g.MeasureString(compName, fTitle).Width / 2), y)
            y += 25
            g.DrawString("PERUNDURAI", fHeader, Brushes.Black, center - (g.MeasureString("PERUNDURAI", fHeader).Width / 2), y)
            y += 40
            Dim filterInfo As String = $"FROM: {FromDateTextBox.Text} TO: {ToDateTextBox.Text} | Payment: {PaymentCombo.Text}"
            g.DrawString(filterInfo, fBody, Brushes.Black, center - (g.MeasureString(filterInfo, fBody).Width / 2), y)
            y += 30
        End If

        If (TypeCombo.Text = "ALL" Or TypeCombo.Text = "RECEIPT") AndAlso mRowVoucher = 0 Then
            g.DrawString("AMOUNT RECEIVABLE REPORT", fHeader, Brushes.Black, left, y)
            'y += 20
            'g.DrawString($"DATE FROM: {FromDateTextBox.Text}   TO: {ToDateTextBox.Text}", fBody, Brushes.Black, left, y)
            y += 30

            Dim colW As Integer() = {40, 90, 80, 200, 200, 100}
            Dim colN As String() = {"S.No", "Date", "R.No", "Name", "Purpose", "Amount"}

            DrawRow(g, left, y, colW, colN, fBody, True)
            y += 25

            Dim dt As DataTable = DirectCast(Guna2DataGridView1.DataSource, DataTable)

            While mRowReceipt < dt.Rows.Count
                Dim row = dt.Rows(mRowReceipt)
                If row("V_Type").ToString() = "RECEIPT" Then
                    sNoReceipt += 1

                    Dim data As String() = {sNoReceipt.ToString(), ' Use the counter here
                              Convert.ToDateTime(row("V_Date")).ToString("dd-MM-yyyy"),
                              row("Bill_No").ToString(),
                              row("Member_Name").ToString(),
                              row("Purpose").ToString(),
                              Convert.ToDecimal(row("Amount")).ToString("N2")}

                    DrawRow(g, left, y, colW, data, fBody, False)
                    TotalReceivable += Convert.ToDecimal(row("Amount"))
                    y += 25
                End If
                mRowReceipt += 1

                If y > 1000 Then
                    e.HasMorePages = True
                    PageNumber += 1
                    Return
                End If
            End While

            y += 10
            g.DrawString("Sub Total Receivable: Rs. " & TotalReceivable.ToString("N2"), fHeader, Brushes.Blue, left + 410, y)
            y += 50
        End If

        If TypeCombo.Text = "ALL" Or TypeCombo.Text = "VOUCHER" Then
            If y < 1000 And mRowVoucher = 0 Then
                g.DrawLine(Pens.Black, left, y - 20, left + 710, y - 20)
            End If

            g.DrawString("AMOUNT PAYABLE REPORT", fHeader, Brushes.Black, left, y)
            y += 30

            Dim colW2 As Integer() = {40, 90, 80, 200, 200, 100}
            Dim colN2 As String() = {"S.No", "Date", "V.No", "Name", "Expense Purpose", "Amount"}
            DrawRow(g, left, y, colW2, colN2, fBody, True)
            y += 25

            Dim dt As DataTable = DirectCast(Guna2DataGridView1.DataSource, DataTable)

            While mRowVoucher < dt.Rows.Count
                Dim row = dt.Rows(mRowVoucher)
                If row("V_Type").ToString() = "VOUCHER" Then
                    sNoVoucher += 1

                    Dim data As String() = {sNoVoucher.ToString(),
                              Convert.ToDateTime(row("V_Date")).ToString("dd-MM-yyyy"),
                              row("Bill_No").ToString(),
                              row("Member_Name").ToString(),
                              row("Purpose").ToString(),
                              Convert.ToDecimal(row("Amount")).ToString("N2")}

                    DrawRow(g, left, y, colW2, data, fBody, False)
                    TotalPayable += Convert.ToDecimal(row("Amount"))
                    y += 25
                End If
                mRowVoucher += 1

                If y > 1050 Then
                    e.HasMorePages = True
                    PageNumber += 1
                    Return
                End If
            End While

            y += 10
            g.DrawString("Sub Total Payable: Rs. " & TotalPayable.ToString("N2"), fHeader, Brushes.Red, left + 410, y)
        End If

        g.DrawString("Printed by BK Software Solutions", fBody, Brushes.Gray, left, e.MarginBounds.Bottom + 20)
        g.DrawString("Page: " & PageNumber, fBody, Brushes.Black, e.MarginBounds.Right - 50, e.MarginBounds.Bottom + 20)

        e.HasMorePages = False
    End Sub

    Private Sub DrawRow(g As Graphics, x As Integer, y As Integer, widths As Integer(), values As String(), font As Font, isHeader As Boolean)
        Dim currentX As Integer = x
        For i As Integer = 0 To values.Length - 1
            g.DrawRectangle(Pens.Black, currentX, y, widths(i), 25)
            g.DrawString(values(i), font, Brushes.Black, currentX + 3, y + 5)
            currentX += widths(i)
        Next
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        LoadReportData()
    End Sub

End Class
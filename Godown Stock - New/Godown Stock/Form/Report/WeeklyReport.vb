Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Xml
Imports Guna.UI2.WinForms

Public Class WeeklyReport
    Private Sub WeeklyReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim today As Date = Date.Today
        Dim daysToSubtract As Integer = CInt(today.DayOfWeek)
        Dim weekStart As Date = today.AddDays(-daysToSubtract)
        Dim weekEnd As Date = weekStart.AddDays(6)

        FromDateTextBox.Text = weekStart.ToString("dd/MM/yyyy")
        ToDateTextBox.Text = weekEnd.ToString("dd/MM/yyyy")

        InitializeDataGridView()
        LoadStockFilters()
        ReportLoad()
        Themeload()

        ProgressBar.Visible = False
        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me
    End Sub
    Private Sub Themeload()
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'HeaderColor'", sqlconnect)
                Using reader As SqlDataReader = Query.ExecuteReader()
                    If reader.Read() Then
                        Dim colorString As String = reader("Ctl_Value").ToString()
                        If Not String.IsNullOrEmpty(colorString) Then
                            Try
                                HeaderPanel.BackColor = ColorTranslator.FromHtml(colorString)
                            Catch
                                HeaderPanel.BackColor = Color.FromArgb(34, 40, 49)
                            End Try
                        Else
                            HeaderPanel.BackColor = Color.FromArgb(34, 40, 49)
                        End If
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading HeaderColor: " & ex.Message)
            End Try
        End Using

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'ScreenColor'", sqlconnect)
                Using reader As SqlDataReader = Query.ExecuteReader()
                    If reader.Read() Then
                        Dim colorString As String = reader("Ctl_Value").ToString()
                        Dim screenColor As Color = Color.FromArgb(232, 232, 232)

                        If Not String.IsNullOrEmpty(colorString) Then
                            Try
                                screenColor = ColorTranslator.FromHtml(colorString)
                            Catch
                                Me.BackColor = Color.FromArgb(232, 232, 232)
                            End Try
                        End If

                        Me.BackColor = screenColor
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading ScreenColor: " & ex.Message)
            End Try
        End Using

    End Sub
    Private Sub ItemForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        FromDateTextBox.Focus()
    End Sub
    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles FromDateTextBox.KeyDown, ToDateTextBox.KeyDown, RefreshButton.KeyDown, FlterButton.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.SelectNextControl(CType(sender, Control), forward:=True, tabStopOnly:=True, nested:=True, wrap:=True)
        End If
    End Sub

    Private Function LoadStockFilters() As Dictionary(Of String, String)
        Dim filePath As String = Path.Combine(Application.StartupPath, "StockFilter.xml")
        Dim filters As New Dictionary(Of String, String)

        If Not File.Exists(filePath) Then Return filters

        Try
            Dim doc As New XmlDocument()
            doc.Load(filePath)

            filters("GroupID") = doc.SelectSingleNode("/StockFilter/Group/ID")?.InnerText.Trim()
            filters("BrandID") = doc.SelectSingleNode("/StockFilter/Brand/ID")?.InnerText.Trim()
            filters("ModelID") = doc.SelectSingleNode("/StockFilter/Model/ID")?.InnerText.Trim()
        Catch ex As Exception
            MessageBox.Show("Error loading filter XML: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return filters
    End Function
    Private Sub ReportLoad()
        Dim startDate As Date
        Dim endDate As Date

        If Not Date.TryParse(FromDateTextBox.Text, startDate) OrElse Not Date.TryParse(ToDateTextBox.Text, endDate) Then
            MessageBox.Show("Please enter valid From and To dates.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ProgressBar.Visible = False
        ProgressBar.Style = ProgressBarStyle.Marquee

        Dim filters = LoadStockFilters()
        Dim whereClauses As New List(Of String) From {"1 = 1"}

        If Not String.IsNullOrWhiteSpace(filters("GroupID")) Then
            whereClauses.Add("i.ItemGroup_ID = @GroupID")
        End If
        If Not String.IsNullOrWhiteSpace(filters("BrandID")) Then
            whereClauses.Add("i.ItemBrand_ID = @BrandID")
        End If
        If Not String.IsNullOrWhiteSpace(filters("ModelID")) Then
            whereClauses.Add("i.ItemModel_ID = @ModelID")
        End If

        Dim whereClause As String = String.Join(" AND ", whereClauses)

        Task.Run(Sub()
                     Dim dt As New DataTable()
                     Try
                         Dim query As String = $"SET DATEFORMAT DMY;
                                            WITH MovementOpening AS ( SELECT item_id,
                                                    SUM(CASE WHEN EntryType = 1 THEN quantity ELSE 0 END) -
                                                    SUM(CASE WHEN EntryType = 2 THEN quantity ELSE 0 END) AS opening_qty
                                                FROM Stock_table  WHERE Stock_date < @StartDate AND Cancel = 0 GROUP BY item_id),
                                            ThisWeekActivity AS (SELECT item_id,
                                                    SUM(CASE WHEN EntryType = 1 THEN quantity ELSE 0 END) AS receipt_qty,
                                                    SUM(CASE WHEN EntryType = 2 THEN quantity ELSE 0 END) AS issue_qty
                                                FROM Stock_table  WHERE Stock_date BETWEEN @StartDate AND @EndDate AND Cancel = 0 GROUP BY item_id)
                                            SELECT i.Itemname AS item_name,
                                                ISNULL(i.Quantity, 0) + ISNULL(mo.opening_qty, 0) AS opening_stock,
                                                ISNULL(twa.receipt_qty, 0) AS receipt_qty,
                                                ISNULL(i.Quantity, 0) + ISNULL(mo.opening_qty, 0) + ISNULL(twa.receipt_qty, 0) AS total_stock,
                                                ISNULL(twa.issue_qty, 0) AS issue_qty,
                                                ISNULL(i.Quantity, 0) + ISNULL(mo.opening_qty, 0) + ISNULL(twa.receipt_qty, 0) - ISNULL(twa.issue_qty, 0) AS closing_stock,
                                                i.MinStock
                                            FROM Item_Table i
                                            LEFT JOIN MovementOpening mo ON i.ID = mo.item_id
                                            LEFT JOIN ThisWeekActivity twa ON i.ID = twa.item_id
                                            WHERE {whereClause} and active=0  ORDER BY i.Itemname;"

                         Using sqlconnect As SqlConnection = Tools.GetConnection()
                             Using command As New SqlCommand(query, sqlconnect)
                                 command.Parameters.AddWithValue("@StartDate", startDate)
                                 command.Parameters.AddWithValue("@EndDate", endDate)

                                 If Not String.IsNullOrWhiteSpace(filters("GroupID")) Then
                                     command.Parameters.AddWithValue("@GroupID", Convert.ToInt32(filters("GroupID")))
                                 End If
                                 If Not String.IsNullOrWhiteSpace(filters("BrandID")) Then
                                     command.Parameters.AddWithValue("@BrandID", Convert.ToInt32(filters("BrandID")))
                                 End If
                                 If Not String.IsNullOrWhiteSpace(filters("ModelID")) Then
                                     command.Parameters.AddWithValue("@ModelID", Convert.ToInt32(filters("ModelID")))
                                 End If

                                 sqlconnect.Open()
                                 Using reader As SqlDataReader = command.ExecuteReader()
                                     dt.Load(reader)
                                 End Using
                             End Using
                         End Using

                     Catch ex As Exception
                         Invoke(Sub()
                                    ProgressBar.Visible = False
                                    MessageBox.Show("Error loading report: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End Sub)
                         Return
                     End Try

                     Invoke(Sub()
                                ProgressBar.Visible = False

                                If dt.Rows.Count = 0 Then
                                    MessageBox.Show("No data found for the selected date range.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Return
                                End If

                                dt.Columns.Add("SNo", GetType(Integer))
                                For i As Integer = 0 To dt.Rows.Count - 1
                                    dt.Rows(i)("SNo") = i + 1
                                Next

                                Guna2DataGridView1.DataSource = dt

                                With Guna2DataGridView1
                                    .Columns("SNo").HeaderText = "S.NO"
                                    .Columns("item_name").HeaderText = "PAPER ITEM"
                                    .Columns("opening_stock").HeaderText = "LAST WEEK STOCK"
                                    .Columns("receipt_qty").HeaderText = "THIS WEEK PURCHASE"
                                    .Columns("total_stock").HeaderText = "TOTAL STOCK"
                                    .Columns("issue_qty").HeaderText = "THIS WEEK SALES"
                                    .Columns("closing_stock").HeaderText = "BALANCE STOCK"
                                    .Columns("MINSTOCK").HeaderText = "MIN STOCK"

                                    .Columns("SNo").Width = 50
                                    .Columns("item_name").Width = 220

                                    .Columns("SNo").DisplayIndex = 0
                                    .Columns("item_name").DisplayIndex = 1
                                    .Columns("opening_stock").DisplayIndex = 2
                                    .Columns("receipt_qty").DisplayIndex = 3
                                    .Columns("total_stock").DisplayIndex = 4
                                    .Columns("issue_qty").DisplayIndex = 5
                                    .Columns("MINSTOCK").DisplayIndex = 6
                                    .Columns("closing_stock").DisplayIndex = 7
                                End With

                                For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                                    Try
                                        Dim minStockVal As Decimal = Convert.ToDecimal(row.Cells("MINSTOCK").Value)
                                        Dim closingStockVal As Decimal = Convert.ToDecimal(row.Cells("closing_stock").Value)

                                        If minStockVal > closingStockVal Then
                                            '    'row.DefaultCellStyle.BackColor = Color.FromArgb(255, 127, 127)
                                            '    row.DefaultCellStyle.ForeColor = Color.White
                                            'row.Cells("SNo").Style.ForeColor = Color.Red
                                            row.Cells("item_name").Style.ForeColor = Color.Red
                                            'row.Cells("opening_stock").Style.ForeColor = Color.Red
                                            'row.Cells("receipt_qty").Style.ForeColor = Color.Red
                                            'row.Cells("total_stock").Style.ForeColor = Color.Red
                                            'row.Cells("issue_qty").Style.ForeColor = Color.Red
                                            row.Cells("MINSTOCK").Style.ForeColor = Color.Red
                                            row.Cells("closing_stock").Style.ForeColor = Color.Red
                                        End If

                                    Catch ex As Exception
                                        ' Handle fails
                                    End Try
                                Next
                            End Sub)
                 End Sub)
    End Sub


    'Private Sub ReportLoad()
    '    Dim startDate As Date = FromDateTextBox.Text
    '    Dim endDate As Date = ToDateTextBox.Text


    '    If Not Date.TryParse(Me.FromDateTextBox.Text, startDate) OrElse Not Date.TryParse(Me.ToDateTextBox.Text, endDate) Then
    '        MessageBox.Show("Please enter valid From and To dates.")
    '        Exit Sub
    '    End If

    '    Dim query As String = "SET DATEFORMAT DMY;
    '                            WITH MovementOpening AS (SELECT item_id,
    '                            SUM(CASE WHEN EntryType = 1 THEN quantity ELSE 0 END) -
    '                            SUM(CASE WHEN EntryType = 2 THEN quantity ELSE 0 END) AS opening_qty
    '                            FROM Stock_table WHERE Stock_date < @StartDate AND Cancel = 0 GROUP BY item_id),

    '                            ThisWeekActivity AS (SELECT item_id,
    '                            SUM(CASE WHEN EntryType = 1 THEN quantity ELSE 0 END) AS receipt_qty,
    '                            SUM(CASE WHEN EntryType = 2 THEN quantity ELSE 0 END) AS issue_qty
    '                            FROM Stock_table WHERE Stock_date BETWEEN @StartDate AND @EndDate AND Cancel = 0 GROUP BY item_id)

    '                            SELECT  i.Itemname AS item_name,
    '                            ISNULL(i.Quantity, 0) + ISNULL(mo.opening_qty, 0) AS opening_stock,
    '                            ISNULL(twa.receipt_qty, 0) AS receipt_qty,
    '                            ISNULL(i.Quantity, 0) + ISNULL(mo.opening_qty, 0) + ISNULL(twa.receipt_qty, 0) AS total_stock,
    '                            ISNULL(twa.issue_qty, 0) AS issue_qty,
    '                            ISNULL(i.Quantity, 0) + ISNULL(mo.opening_qty, 0) + ISNULL(twa.receipt_qty, 0) - ISNULL(twa.issue_qty, 0) AS closing_stock,
    '                            i.MinStock FROM Item_Table i
    '                            LEFT JOIN MovementOpening mo ON i.ID = mo.item_id
    '                            LEFT JOIN ThisWeekActivity twa ON i.ID = twa.item_id
    '                            ORDER BY i.Itemname;"

    '    'WITH LastWeekStock AS (SELECT itemname AS item_name,
    '    'SUM(CASE WHEN EntryType = 1 THEN quantity ELSE 0 END) - 
    '    'SUM(CASE WHEN EntryType = 2 THEN quantity ELSE 0 END) AS closing_qty
    '    'FROM Stock_table  WHERE Stock_date BETWEEN DATEADD(WEEK, -1, @StartDate) AND DATEADD(WEEK, -1, @EndDate) and Cancel = 0
    '    'GROUP BY itemname),
    '    'ThisWeekActivity AS (SELECT itemname AS item_name,
    '    'SUM(CASE WHEN EntryType = 1 THEN quantity ELSE 0 END) AS receipt_qty,
    '    'SUM(CASE WHEN EntryType = 2 THEN quantity ELSE 0 END) AS issue_qty
    '    'FROM Stock_table WHERE Stock_date BETWEEN @StartDate AND @EndDate and Cancel = 0
    '    'GROUP BY itemname)
    '    'SELECT i.Itemname AS item_name,
    '    'ISNULL(i.Quantity, 0) + ISNULL(l.closing_qty, 0) AS opening_stock,
    '    'ISNULL(t.receipt_qty, 0) AS receipt_qty,
    '    'ISNULL(i.Quantity, 0) + ISNULL(l.closing_qty, 0) + ISNULL(t.receipt_qty, 0) AS total_stock,
    '    'ISNULL(t.issue_qty, 0) AS issue_qty,
    '    'ISNULL(i.Quantity, 0) + ISNULL(l.closing_qty, 0) + ISNULL(t.receipt_qty, 0) - ISNULL(t.issue_qty, 0) AS closing_stock, i.MINSTOCK
    '    'FROM ITEM_TABLE i
    '    'LEFT JOIN LastWeekStock l ON i.Itemname = l.item_name
    '    'LEFT JOIN ThisWeekActivity t ON i.Itemname = t.item_name
    '    'ORDER BY i.Itemname;

    '    Using sqlconnect As SqlConnection = Tools.GetConnection()
    '        Using command As New SqlCommand(query, sqlconnect)
    '            command.Parameters.AddWithValue("@StartDate", startDate)
    '            command.Parameters.AddWithValue("@EndDate", endDate)

    '            Try
    '                sqlconnect.Open()
    '                Using reader As SqlDataReader = command.ExecuteReader()
    '                    Dim dt As New DataTable()
    '                    dt.Load(reader)

    '                    If dt.Rows.Count = 0 Then
    '                        MessageBox.Show("No data found for the selected date range.")
    '                        Exit Sub
    '                    End If

    '                    dt.Columns.Add("SNo", GetType(Integer))
    '                    For i As Integer = 0 To dt.Rows.Count - 1
    '                        dt.Rows(i)("SNo") = i + 1
    '                    Next

    '                    Guna2DataGridView1.DataSource = dt

    '                    With Guna2DataGridView1
    '                        .Columns("SNo").HeaderText = "S.NO"
    '                        .Columns("item_name").HeaderText = "PAPER ITEM"
    '                        .Columns("opening_stock").HeaderText = "LAST WEEK STOCK"
    '                        .Columns("receipt_qty").HeaderText = "THIS WEEK PURCHASE"
    '                        .Columns("total_stock").HeaderText = "TOTAL STOCK"
    '                        .Columns("issue_qty").HeaderText = "THIS WEEK SALES"
    '                        .Columns("closing_stock").HeaderText = "BALANCE STOCK"
    '                        .Columns("MINSTOCK").HeaderText = "MIN STOCK"

    '                        .Columns("SNo").Width = 50
    '                        .Columns("item_name").Width = 220

    '                        .Columns("SNo").DisplayIndex = 0
    '                        .Columns("item_name").DisplayIndex = 1
    '                        .Columns("opening_stock").DisplayIndex = 2
    '                        .Columns("receipt_qty").DisplayIndex = 3
    '                        .Columns("total_stock").DisplayIndex = 4
    '                        .Columns("issue_qty").DisplayIndex = 5
    '                        .Columns("MINSTOCK").DisplayIndex = 6
    '                        .Columns("closing_stock").DisplayIndex = 7
    '                    End With
    '                    For Each row As DataGridViewRow In Guna2DataGridView1.Rows
    '                        Try
    '                            Dim minStockVal As Decimal = Convert.ToDecimal(row.Cells("MINSTOCK").Value)
    '                            Dim closingStockVal As Decimal = Convert.ToDecimal(row.Cells("closing_stock").Value)

    '                            If minStockVal > closingStockVal Then
    '                                row.DefaultCellStyle.BackColor = Color.FromArgb(255, 127, 127)
    '                                row.DefaultCellStyle.ForeColor = Color.White
    '                            End If
    '                        Catch ex As Exception
    '                            ' Optional: handle error
    '                        End Try
    '                    Next
    '                End Using
    '            Catch ex As Exception
    '                MessageBox.Show("An error occurred while loading the data: " & ex.Message)
    '            End Try
    '        End Using
    '    End Using
    'End Sub

    Private Sub InitializeDataGridView()
        With Guna2DataGridView1
            .ColumnHeadersVisible = True
            .AllowUserToAddRows = False
            .AllowUserToOrderColumns = True
            '.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 40, 49)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .ColumnHeadersHeight = 35
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
            .Margin = New Padding(20, 20, 20, 20)
            .MultiSelect = False

            Dim headerColor As Color = Color.FromArgb(34, 40, 49)
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    sqlconnect.Open()
                    Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'HeaderColor'", sqlconnect)
                    Using reader As SqlDataReader = Query.ExecuteReader()
                        If reader.Read() Then
                            Dim colorString As String = reader("Ctl_Value").ToString()
                            If Not String.IsNullOrEmpty(colorString) Then
                                Try
                                    headerColor = ColorTranslator.FromHtml(colorString)
                                Catch
                                    ' keep default if conversion fails
                                End Try
                            End If
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error loading HeaderColor: " & ex.Message)
                End Try
            End Using

            .ColumnHeadersDefaultCellStyle.BackColor = headerColor
        End With
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        ReportLoad()
    End Sub

    Private Sub PrintButton_Click(sender As Object, e As EventArgs) Handles PrintButton.Click
        ExportToPDF()
    End Sub
    Function SafeToString(value As Object) As String
        Return If(IsDBNull(value) OrElse value Is Nothing, "", value.ToString())
    End Function

    Private Sub ExportToPDF()
        Try
            Dim companyName As String = ""
            Dim titleQuery As String = "SELECT TOP 1 comp_name FROM company_table"

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using command As New SqlCommand(titleQuery, sqlconnect)
                    sqlconnect.Open()
                    Dim result = command.ExecuteScalar()
                    If result IsNot Nothing Then
                        companyName = result.ToString()
                    Else
                        companyName = "COMPANY"
                    End If
                End Using
            End Using

            Dim folderPath As String = Path.Combine(Application.StartupPath, "GS Report\")
            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If

            Dim filePath As String = Path.Combine(folderPath, $"WeeklyStockReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf")

            Using fs As New FileStream(filePath, FileMode.Create)
                Dim document As New Document(PageSize.A4, 20, 20, 20, 20)

                Dim writer As PdfWriter = PdfWriter.GetInstance(document, fs)
                document.Open()

                Dim titleFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD)
                Dim headerFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD)
                Dim bodyFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)

                Dim titleText As String = $"{companyName} WEEKLY STOCK REPORT".ToUpper()
                Dim titleParagraph As New Paragraph(titleText, titleFont)
                titleParagraph.Alignment = Element.ALIGN_CENTER
                document.Add(titleParagraph)

                Dim dateRange As String = $"FROM: {FromDateTextBox.Text} TO: {ToDateTextBox.Text}"
                Dim dateParagraph As New Paragraph(dateRange, bodyFont)
                dateParagraph.Alignment = Element.ALIGN_CENTER
                document.Add(dateParagraph)

                document.Add(New Paragraph(" "))

                Dim table As New PdfPTable(8)
                table.WidthPercentage = 100
                table.SetWidths(New Single() {5, 30, 15, 15, 15, 15, 15, 15})

                Dim headers As String() = {"S.NO", "PAPER ITEM", "LAST WEEK STOCK",
                                            "THIS WEEK PURCHASE", "TOTAL STOCK",
                                            "THIS WEEK SALES", "MIN STOCK", "BALANCE STOCK"}

                For Each header As String In headers
                    Dim cell As New PdfPCell(New Phrase(header, headerFont))
                    cell.HorizontalAlignment = Element.ALIGN_CENTER
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY
                    cell.Padding = 5
                    table.AddCell(cell)
                Next

                'For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                '    If Not row.IsNewRow Then
                '        Dim minStockVal As Decimal = Convert.ToDecimal(row.Cells("MINSTOCK").Value)
                '        Dim closingStockVal As Decimal = Convert.ToDecimal(row.Cells("closing_stock").Value)
                '        Dim rowFont As iTextSharp.text.Font = If(minStockVal > closingStockVal,
                '                                 New iTextSharp.text.Font(bodyFont.Family, bodyFont.Size, bodyFont.Style, BaseColor.RED),
                '                                 bodyFont)

                '        table.AddCell(New PdfPCell(New Phrase(row.Cells("SNo").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                '        table.AddCell(New PdfPCell(New Phrase(row.Cells("item_name").Value?.ToString(), rowFont)) With {.HorizontalAlignment = Element.ALIGN_LEFT, .Padding = 4})
                '        table.AddCell(New PdfPCell(New Phrase(row.Cells("opening_stock").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                '        table.AddCell(New PdfPCell(New Phrase(row.Cells("receipt_qty").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                '        table.AddCell(New PdfPCell(New Phrase(row.Cells("total_stock").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                '        table.AddCell(New PdfPCell(New Phrase(row.Cells("issue_qty").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                '        table.AddCell(New PdfPCell(New Phrase(row.Cells("MINSTOCK").Value?.ToString(), rowFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                '        table.AddCell(New PdfPCell(New Phrase(row.Cells("closing_stock").Value?.ToString(), rowFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                '    End If
                'Next

                For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                    If Not row.IsNewRow Then
                        Dim minStockVal As Decimal = If(IsDBNull(row.Cells("MINSTOCK").Value) OrElse row.Cells("MINSTOCK").Value Is Nothing, 0D, Convert.ToDecimal(row.Cells("MINSTOCK").Value))
                        Dim closingStockVal As Decimal = If(IsDBNull(row.Cells("closing_stock").Value) OrElse row.Cells("closing_stock").Value Is Nothing, 0D, Convert.ToDecimal(row.Cells("closing_stock").Value))

                        Dim rowFont As iTextSharp.text.Font = If(minStockVal > closingStockVal,
                                         New iTextSharp.text.Font(bodyFont.Family, bodyFont.Size, bodyFont.Style, BaseColor.RED),
                                         bodyFont)

                        table.AddCell(New PdfPCell(New Phrase(SafeToString(row.Cells("SNo").Value), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(SafeToString(row.Cells("item_name").Value), rowFont)) With {.HorizontalAlignment = Element.ALIGN_LEFT, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(SafeToString(row.Cells("opening_stock").Value), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(SafeToString(row.Cells("receipt_qty").Value), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(SafeToString(row.Cells("total_stock").Value), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(SafeToString(row.Cells("issue_qty").Value), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(SafeToString(row.Cells("MINSTOCK").Value), rowFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(SafeToString(row.Cells("closing_stock").Value), rowFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                    End If
                Next



                document.Add(table)
                document.Close()
            End Using

            MessageBox.Show("PDF saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error creating PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub FlterButton_Click(sender As Object, e As EventArgs) Handles FlterButton.Click
        For Each f As Form In Application.OpenForms
            If TypeOf f Is StockFilter Then
                f.BringToFront()
                f.Focus()
                Return
            End If
        Next

        Dim filter As New StockFilter()
        filter.Show()
    End Sub

End Class

Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports System.Xml
Imports Guna.UI2.WinForms

Public Class JCReport

    Private dgvPrintRowIndex As Integer = 0
    Private Sub JCReport_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        FromDateTextBox.Focus()
    End Sub

    Private Sub JCReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Status.Items.Clear()
        Status.Items.Add("PENDING")
        Status.Items.Add("COMPLETED")
        Status.SelectedIndex = -1
        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me

        FromDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy")
        ToDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy")
        Themeload()
        InitializeDataGridView()
        'AddHandler Guna2DataGridView1.RowPrePaint, AddressOf Guna2DataGridView1_RowPrePaint
        ReportLoad()
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
    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles FromDateTextBox.KeyDown, ToDateTextBox.KeyDown, RefreshButton.KeyDown, Status.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.SelectNextControl(CType(sender, Control), forward:=True, tabStopOnly:=True, nested:=True, wrap:=True)
        End If
    End Sub
    Private Sub ReportLoad()
        Dim fromDate As Date
        Dim toDate As Date
        Dim selectedStatus As String = Status.Text.Trim()

        If Not Date.TryParseExact(FromDateTextBox.Text, "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, fromDate) OrElse
       Not Date.TryParseExact(ToDateTextBox.Text, "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, toDate) Then
            MessageBox.Show("Please enter valid From and To dates in dd/MM/yyyy format.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim query As String = "SET DATEFORMAT DMY;
                                SELECT jc.Bill_No, FORMAT(jc.JobCard_date, 'dd/MM/yyyy') AS JobCard_date, lt.Partyname,
                                       np.Name AS NoteProcessing_English, np.TamilName AS NoteProcessing_Tamil,
                                       ns.Name AS NoteSize_English, ns.TamilName AS NoteSize_Tamil, jc.Paper_Size, nt.Name As NoteType,
                                       jc.Note_Size, jc.Sheet, jc.Pages, jc.Note, jc.Reem, jc.Finishing,
                                       jc.WorkingStatus, jc.Manual_BillNo, Format(jc.Finish_Date, 'dd/MM/yyyy') AS Finish_Date, jc.Paper_Brand, jc.Paper_GSM,
                                       jc.Paper_Weight, jc.no_Index, jc.Wrapper, jc.Remarks
                                FROM jobcard_table jc
                                LEFT JOIN Ledger_Table lt ON jc.ledger_id = lt.ID
                                LEFT JOIN NoteProcessing_Table np ON jc.NoteProcessing_ID = np.ID
                                LEFT JOIN NoteSize_Table ns ON jc.NoteSize_ID = ns.ID
                                Left Join NoteType_Table nt ON jc.NoteType_Id = nt.ID
                                WHERE jc.JobCard_date BETWEEN @FromDate AND @ToDate and jc.cancel=0"

        If Not String.IsNullOrWhiteSpace(selectedStatus) Then
            query &= " AND jc.WorkingStatus = @Status"
        End If

        query &= " ORDER BY jc.Bill_No, jc.JobCard_date;"

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Using command As New SqlCommand(query, sqlconnect)
                command.Parameters.AddWithValue("@FromDate", fromDate)
                command.Parameters.AddWithValue("@ToDate", toDate)

                If Not String.IsNullOrWhiteSpace(selectedStatus) Then
                    command.Parameters.AddWithValue("@Status", selectedStatus)
                End If

                Try
                    sqlconnect.Open()
                    Using reader As SqlDataReader = command.ExecuteReader()
                        Dim dt As New DataTable()
                        dt.Load(reader)

                        If dt.Rows.Count = 0 Then
                            'MessageBox.Show("No data found for the selected date range.")
                            Exit Sub
                        End If

                        Dim formatted As DataTable = FormatGroupedReport(dt)
                        Guna2DataGridView1.DataSource = formatted

                        With Guna2DataGridView1
                            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill

                            If .Columns.Contains("Bill_No") Then .Columns("Bill_No").Width = 150
                            If .Columns.Contains("JobCard_date") Then .Columns("JobCard_date").Width = 150
                            If .Columns.Contains("Partyname") Then
                                .Columns("Partyname").Width = 250
                                .Columns("Partyname").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                            End If
                            If .Columns.Contains("Note") Then .Columns("Note").Width = 100
                            If .Columns.Contains("Finishing") Then
                                .Columns("Finishing").Width = 100
                                .Columns("Finishing").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                            End If
                            If .Columns.Contains("Finish_Date") Then .Columns("Finish_Date").Width = 100
                            If .Columns.Contains("WorkingStatus") Then
                                .Columns("WorkingStatus").Width = 150
                                .Columns("WorkingStatus").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                            End If
                        End With

                    End Using
                Catch ex As Exception
                    MessageBox.Show("An error occurred while loading the data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub

    Private Function FormatGroupedReport(sourceTable As DataTable) As DataTable
        Dim reportTable As New DataTable()
        reportTable.Columns.AddRange({New DataColumn("Bill_No"),
                                       New DataColumn("JobCard_date"),
                                       New DataColumn("Partyname"),
                                       New DataColumn("Note"),
                                       New DataColumn("Finishing"),
                                       New DataColumn("Finish_Date"),
                                       New DataColumn("WorkingStatus")})

        Dim grouped = From row In sourceTable.AsEnumerable()
                      Group row By billNo = row.Field(Of String)("Bill_No"),
                                  jobDate = row.Field(Of String)("JobCard_date"),
                                  party = row.Field(Of String)("Partyname"),
                                  workingStatus = row.Field(Of String)("WorkingStatus"),
                                  finishing = row.Field(Of Object)("Finishing"),
                                  finishdate = row.Field(Of String)("Finish_Date"),
                                  note = row.Field(Of Object)("Note")
                      Into Group

        For Each g In grouped

            reportTable.Rows.Add(g.billNo, g.jobDate, g.party, g.note, g.finishing, g.finishdate, g.workingStatus)

            For Each item In g.Group
                Dim manualBillNo = If(item("Manual_BillNo") IsNot DBNull.Value, item("Manual_BillNo").ToString(), "")
                Dim noteProc = FormatTwo(item("NoteProcessing_English"), item("NoteProcessing_Tamil"))
                Dim noteSize = FormatTwo(item("NoteSize_English"), item("NoteSize_Tamil"))
                Dim NoteType = If(item("NoteType") IsNot DBNull.Value, item("NoteType").ToString(), "")
                Dim PaperSize = If(item("Paper_Size") IsNot DBNull.Value, item("Paper_Size").ToString(), "")
                Dim sheet = If(item("Sheet") IsNot DBNull.Value, item("Sheet").ToString(), "")
                Dim pages = If(item("Pages") IsNot DBNull.Value, item("Pages").ToString(), "")
                Dim reem = If(item("Reem") IsNot DBNull.Value, item("Reem").ToString(), "")
                Dim Note = If(item("Note") IsNot DBNull.Value, item("Note").ToString(), "")
                Dim PaperBrand = If(item("Paper_Brand") IsNot DBNull.Value, item("Paper_Brand").ToString(), "")
                Dim PaperGSM = If(item("Paper_GSM") IsNot DBNull.Value, item("Paper_GSM").ToString(), "")
                Dim PaperWeight = If(item("Paper_Weight") IsNot DBNull.Value, item("Paper_Weight").ToString(), "")
                Dim Index = If(item("no_Index") IsNot DBNull.Value, item("no_Index").ToString(), "")
                Dim Wrapper = If(item("Wrapper") IsNot DBNull.Value, item("Wrapper").ToString(), "")
                Dim Remarks = If(item("Remarks") IsNot DBNull.Value, item("Remarks").ToString(), "")

                reportTable.Rows.Add("Manual Bill No:", "", $"{manualBillNo}", "", "", "")
                reportTable.Rows.Add("Note Processing:", "", $"{noteProc}", "", "", "")
                reportTable.Rows.Add("Note Size:", "", $"{noteSize}", "", "", "")
                reportTable.Rows.Add("Note Type:", "", $"{NoteType}", "", "", "")
                reportTable.Rows.Add("Paper Size: ", "", $"{PaperSize}", "", "", "")
                reportTable.Rows.Add("Sheet: ", "", $"{sheet}", "", "", "")
                reportTable.Rows.Add("Pages:", "", $"{pages}", "", "", "")
                reportTable.Rows.Add("Reem: ", "", $"{reem}", "", "", "")
                reportTable.Rows.Add("No Of Note:", "", $"{Note}", "", "", "")
                reportTable.Rows.Add("Paper Brand:", "", $"{PaperBrand}", "", "", "")
                reportTable.Rows.Add("Paper GSM:", "", $"{PaperGSM}", "", "", "")
                reportTable.Rows.Add("Paper Weight: ", "", $"{PaperWeight}", "", "", "")
                reportTable.Rows.Add("No of Index: ", "", $"{Index}", "", "", "")
                reportTable.Rows.Add("No Of Wrapper:", "", $"{Wrapper}", "", "", "")
                reportTable.Rows.Add("Remarks: ", "", $"{Remarks}", "", "", "")
            Next
        Next
        Return reportTable
    End Function

    Private Function FormatTwo(val1 As Object, val2 As Object) As String
        Dim part1 As String = If(val1 IsNot DBNull.Value, val1.ToString(), "")
        Dim part2 As String = If(val2 IsNot DBNull.Value, val2.ToString(), "")
        If String.IsNullOrWhiteSpace(part1) AndAlso String.IsNullOrWhiteSpace(part2) Then
            Return ""
        End If
        Return $"{part1} / {part2}"
    End Function

    Private Sub SetColumnHeader(columnName As String, headerText As String, Optional width As Integer = 100, Optional displayIndex As Integer = -1)
        If Guna2DataGridView1.Columns.Contains(columnName) Then
            With Guna2DataGridView1.Columns(columnName)
                .HeaderText = headerText
                .Width = width
                If displayIndex >= 0 Then .DisplayIndex = displayIndex
            End With
        End If
    End Sub

    Private Sub InitializeDataGridView()
        With Guna2DataGridView1
            .ColumnHeadersVisible = True
            .AllowUserToAddRows = False
            .AllowUserToOrderColumns = False
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 40, 49)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersHeight = 35
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
            .Margin = New Padding(20, 20, 20, 20)
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .MultiSelect = False
            .Dock = DockStyle.Fill

        End With
    End Sub

    'Private Sub Guna2DataGridView1_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs)
    '    Dim dgv As DataGridView = CType(sender, DataGridView)

    '    If e.RowIndex >= 0 Then
    '        Dim row As DataGridViewRow = dgv.Rows(e.RowIndex)
    '        Dim statusCell As DataGridViewCell = row.Cells("WorkingStatus")

    '        If statusCell IsNot Nothing AndAlso statusCell.Value IsNot Nothing Then
    '            Dim status As String = statusCell.Value.ToString().Trim().ToLower()

    '            Select Case status
    '                Case "PENDING"
    '                    row.DefaultCellStyle.BackColor = Color.FromArgb(77, 168, 218)
    '                Case "COMPLETED"
    '                    row.DefaultCellStyle.BackColor = Color.FromArgb(1, 146, 103)
    '                Case Else
    '                    row.DefaultCellStyle.BackColor = Color.White
    '            End Select
    '        End If
    '    End If
    'End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        ReportLoad()
    End Sub

    Private Sub PrintButton_Click(sender As Object, e As EventArgs) Handles PrintButton.Click
        ExportToPDF()
    End Sub
    Private Sub ExportToPDF()
        Try
            Dim companyName As String = ""
            Dim titleQuery As String = "SELECT TOP 1 comp_name FROM company_table"

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using command As New SqlCommand(titleQuery, sqlconnect)
                    sqlconnect.Open()
                    Dim result = command.ExecuteScalar()
                    companyName = If(result IsNot Nothing, result.ToString(), "COMPANY")
                End Using
            End Using

            Dim folderPath As String = Path.Combine(Application.StartupPath, "GS Report\")
            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If

            Dim filePath As String = Path.Combine(folderPath, $"JobCardReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf")

            Using fs As New FileStream(filePath, FileMode.Create)
                Dim document As New Document(PageSize.A4, 20, 20, 20, 20)
                Dim writer As PdfWriter = PdfWriter.GetInstance(document, fs)
                document.Open()

                Dim titleFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD)
                Dim headerFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD)
                Dim bodyFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)

                Dim titleText As String = $"{companyName} JOB CARD REPORT".ToUpper()
                Dim titleParagraph As New Paragraph(titleText, titleFont) With {.Alignment = Element.ALIGN_CENTER}
                document.Add(titleParagraph)

                Dim dateRange As String = $"FROM: {FromDateTextBox.Text} TO: {ToDateTextBox.Text}"
                Dim dateParagraph As New Paragraph(dateRange, bodyFont) With {.Alignment = Element.ALIGN_CENTER}
                document.Add(dateParagraph)
                document.Add(New Paragraph(" "))

                Dim table As New PdfPTable(6) With {.WidthPercentage = 100}
                table.SetWidths(New Single() {15, 15, 30, 15, 15, 15})

                Dim headers As String() = {"Bill No", "Job Card Date", "Party Name", "Note", "Finishing", "Status"}
                For Each header As String In headers
                    Dim cell As New PdfPCell(New Phrase(header, headerFont)) With {
                        .HorizontalAlignment = Element.ALIGN_CENTER,
                        .BackgroundColor = BaseColor.LIGHT_GRAY,
                        .Padding = 5
                    }
                    table.AddCell(cell)
                Next

                For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                    If Not row.IsNewRow Then
                        Dim billNo = row.Cells("Bill_No").Value?.ToString()
                        Dim jobCardDate = row.Cells("JobCard_date").Value?.ToString()

                        If String.IsNullOrWhiteSpace(jobCardDate) Then

                            Dim mergedCell As New PdfPCell(New Phrase(billNo, bodyFont)) With {
                                .Colspan = 2,
                                .HorizontalAlignment = Element.ALIGN_LEFT,
                                .Padding = 4
                            }
                            table.AddCell(mergedCell)

                            table.AddCell(New PdfPCell(New Phrase(row.Cells("Partyname").Value?.ToString(), bodyFont)) With {
                                .HorizontalAlignment = Element.ALIGN_LEFT,
                                .Padding = 4
                            })

                            For i As Integer = 1 To 3
                                table.AddCell(New PdfPCell(New Phrase("", bodyFont)) With {
                                    .Padding = 4
                                })
                            Next
                        Else
                            Dim statusText = row.Cells("WorkingStatus").Value?.ToString()?.Trim().ToLower()
                            Dim textColor As BaseColor = BaseColor.BLACK

                            Select Case statusText
                                Case "new"
                                    textColor = New BaseColor(77, 168, 218)
                                Case "in progress"
                                    textColor = New BaseColor(255, 165, 0) ' Orange
                                Case "completed"
                                    textColor = New BaseColor(1, 146, 103)
                            End Select

                            Dim billFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, textColor)
                            Dim statusFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL, textColor)

                            table.AddCell(New PdfPCell(New Phrase(billNo, billFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                            table.AddCell(New PdfPCell(New Phrase(jobCardDate, bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                            table.AddCell(New PdfPCell(New Phrase(row.Cells("Partyname").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_LEFT, .Padding = 4})
                            table.AddCell(New PdfPCell(New Phrase(row.Cells("Note").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_LEFT, .Padding = 4})
                            table.AddCell(New PdfPCell(New Phrase(row.Cells("Finishing").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_LEFT, .Padding = 4})
                            table.AddCell(New PdfPCell(New Phrase(row.Cells("WorkingStatus").Value?.ToString(), statusFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})

                        End If
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


End Class

'Private Sub ExportToPDF()
'    Try
'        Dim companyName As String = ""
'        Dim titleQuery As String = "SELECT TOP 1 comp_name FROM company_table"

'        Using sqlconnect As SqlConnection = Tools.GetConnection()
'            Using command As New SqlCommand(titleQuery, sqlconnect)
'                sqlconnect.Open()
'                Dim result = command.ExecuteScalar()
'                If result IsNot Nothing Then
'                    companyName = result.ToString()
'                Else
'                    companyName = "COMPANY"
'                End If
'            End Using
'        End Using

'        Dim folderPath As String = Path.Combine(Application.StartupPath, "GS Report\")
'        If Not Directory.Exists(folderPath) Then
'            Directory.CreateDirectory(folderPath)
'        End If

'        Dim filePath As String = Path.Combine(folderPath, $"JobCardReport_{DateTime.Now:yyyyMMdd_HHmmss}.pdf")

'        Using fs As New FileStream(filePath, FileMode.Create)
'            Dim document As New Document(PageSize.A4, 20, 20, 20, 20)
'            Dim writer As PdfWriter = PdfWriter.GetInstance(document, fs)
'            document.Open()

'            Dim titleFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD)
'            Dim headerFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD)
'            Dim bodyFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)

'            Dim titleText As String = $"{companyName} JOB CARD REPORT".ToUpper()
'            Dim titleParagraph As New Paragraph(titleText, titleFont) With {.Alignment = Element.ALIGN_CENTER}
'            document.Add(titleParagraph)

'            Dim dateRange As String = $"FROM: {FromDateTextBox.Text} TO: {ToDateTextBox.Text}"
'            Dim dateParagraph As New Paragraph(dateRange, bodyFont) With {.Alignment = Element.ALIGN_CENTER}
'            document.Add(dateParagraph)

'            document.Add(New Paragraph(" "))

'            Dim columnCount As Integer = 6
'            Dim table As New PdfPTable(columnCount) With {.WidthPercentage = 100}
'            table.SetWidths(New Single() {15, 15, 30, 15, 15, 15})

'            Dim headers As String() = {"Bill No", "Job Card Date", "Party Name", "Note", "Finishing", "Status"}
'            For Each header As String In headers
'                Dim cell As New PdfPCell(New Phrase(header, headerFont)) With {
'                    .HorizontalAlignment = Element.ALIGN_CENTER,
'                    .BackgroundColor = BaseColor.LIGHT_GRAY,
'                    .Padding = 5
'                }
'                table.AddCell(cell)
'            Next

'            For Each row As DataGridViewRow In Guna2DataGridView1.Rows
'                If Not row.IsNewRow Then

'                    table.AddCell(New PdfPCell(New Phrase(row.Cells("Bill_No").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
'                    table.AddCell(New PdfPCell(New Phrase(row.Cells("JobCard_date").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
'                    table.AddCell(New PdfPCell(New Phrase(row.Cells("Partyname").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_LEFT, .Padding = 4})
'                    table.AddCell(New PdfPCell(New Phrase(row.Cells("Note").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_LEFT, .Padding = 4})
'                    table.AddCell(New PdfPCell(New Phrase(row.Cells("Finishing").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_LEFT, .Padding = 4})
'                    table.AddCell(New PdfPCell(New Phrase(row.Cells("WorkingStatus").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
'                End If
'            Next

'            document.Add(table)
'            document.Close()
'        End Using

'        MessageBox.Show("PDF saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

'    Catch ex As Exception
'        MessageBox.Show("Error creating PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
'    End Try
'End Sub

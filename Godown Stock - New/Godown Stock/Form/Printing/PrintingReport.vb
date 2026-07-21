Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports Guna.UI2.WinForms

Public Class PrintingReport

    Private PrintDataTable As DataTable

    Private Sub PrintingReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FromDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy")
        ToDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy")

        InitializeDataGridView()
        Status.Items.Clear()
        Status.Items.Add("ALL")
        Status.Items.Add("PENDING")
        Status.Items.Add("COMPLETED")
        Status.SelectedIndex = 0

        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me
        Themeload()
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
                        Dim screenColor As Color = Color.FromArgb(34, 40, 49)

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
    Handles FromDateTextBox.KeyDown, ToDateTextBox.KeyDown, RefreshButton.KeyDown, Status.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.SelectNextControl(CType(sender, Control), forward:=True, tabStopOnly:=True, nested:=True, wrap:=True)
        End If
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        ReportLoad()
    End Sub
    Private Sub ReportLoad()

        Dim fd As Date, td As Date
        If Not Date.TryParseExact(FromDateTextBox.Text, "dd/MM/yyyy", Nothing,
            Globalization.DateTimeStyles.None, fd) OrElse Not Date.TryParseExact(ToDateTextBox.Text, "dd/MM/yyyy", Nothing,
            Globalization.DateTimeStyles.None, td) Then
            MessageBox.Show("Enter valid From/To dates in dd/MM/yyyy format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim statusFilter As String = Status.SelectedItem.ToString().ToLower()

        Dim dateField As String = If(statusFilter = "completed", "pt.Finish_Date", "pt.Printing_date")

        Dim sql = $"SET DATEFORMAT DMY;
                    SELECT pt.Bill_No, pt.Printing_date, lt.Partyname,
                           pm.Name AS PMName, ptm.Name AS PTName,
                           mach.Name AS MachineName, pt.Paper_Size_GSM, pt.Printing_Colour, pt.Quantity, pt.Printing_Details,
                           pt.WorkingStatus, pt.Finish_Date as FinishDate, pt.Finish as FinishQty, pt.Paper_Brand, pt.Paper_Weight, 
                           PI.Name AS NTName
                    FROM Printing_table pt
                    LEFT JOIN Ledger_Table lt ON pt.ledger_id = lt.ID
                    LEFT JOIN PrintingMethod_table pm ON pt.PrintMethod_Id = pm.ID
                    LEFT JOIN PrintingType_table ptm ON pt.PrintingType_Id = ptm.ID
                    LEFT JOIN PrintingMachine_table mach ON pt.PrintingMachine_Id = mach.ID
                    LEFT JOIN PrintingItem_table PI ON pt.printingitem_id = PI.ID
                    WHERE pt.Cancel = 0 AND {dateField} BETWEEN @fd AND @td"

        If statusFilter <> "all" Then
            sql &= " AND pt.WorkingStatus = @status"
        End If

        sql &= $" ORDER BY pt.Bill_No, {dateField};"

        Dim dt As New DataTable()
        Using conn = Tools.GetConnection(), cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@fd", fd)
            cmd.Parameters.AddWithValue("@td", td)
            If statusFilter <> "ALL" Then cmd.Parameters.AddWithValue("@status", statusFilter)
            conn.Open()
            dt.Load(cmd.ExecuteReader())
        End Using

        If dt.Rows.Count = 0 Then
            Guna2DataGridView1.DataSource = Nothing
            ' MessageBox.Show("No records found.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        PrintDataTable = FormatGroupedPrintingReport(dt)
        Guna2DataGridView1.DataSource = PrintDataTable

        If Guna2DataGridView1.Columns.Contains("Printing_Details") Then
            Guna2DataGridView1.Columns("Printing_Details").Visible = False
        End If

        With Guna2DataGridView1
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            SetColumnHeader("SNo", "S.No", 40, 0)
            'SetColumnHeader("Bill_No", "Bill No", 60, 1)
            'SetColumnHeader("Printing_date", "Date", 90, 2)
            'SetColumnHeader("Partyname", "Party Name", 120, 3)
            'SetColumnHeader("Quantity", "Qty", 60, 4)
            'SetColumnHeader("MachineName", "Machine", 90, 5)
            'SetColumnHeader("FinishQty", "Finished", 50, 6)
            'SetColumnHeader("WorkingStatus", "Status", 50, 7)
            'SetColumnHeader("Details", "Details", 250, 8)
        End With
    End Sub

    Private Function FormatGroupedPrintingReport(sourceTable As DataTable) As DataTable
        Dim reportTable As New DataTable()
        reportTable.Columns.AddRange({
                New DataColumn("SNo", GetType(Integer)),
                New DataColumn("Bill_No"),
                New DataColumn("Printing_date"),
                New DataColumn("Partyname"),
                New DataColumn("Quantity"),
                New DataColumn("MachineName"),
                New DataColumn("FinishQty"),
                New DataColumn("WorkingStatus")
            })
        Dim PrintTable As New DataTable()
        PrintTable.Columns.AddRange({
                New DataColumn("SNo", GetType(Integer)),
                New DataColumn("Bill_No"),
                New DataColumn("Printing_date"),
                New DataColumn("Partyname"),
                New DataColumn("Quantity"),
                New DataColumn("MachineName"),
                New DataColumn("FinishQty"),
                New DataColumn("FinishDate"),
                New DataColumn("WorkingStatus"),
                New DataColumn("Printing_Details")
            })


        Dim grouped = From row In sourceTable.AsEnumerable()
                      Group row By
                      billNo = row.Field(Of String)("Bill_No"),
                      dateVal = row.Field(Of Date)("Printing_date").ToString("dd/MM/yyyy"),
                      party = row.Field(Of String)("Partyname"),
                      qty = row("Quantity").ToString(),
                      machine = row("MachineName").ToString(),
                      finish = If(sourceTable.Columns.Contains("FinishQty") AndAlso Not IsDBNull(row("FinishQty")), row("FinishQty").ToString(), ""),
                      finishdate = row.Field(Of Date)("FinishDate").ToString("dd/MM/yyyy"),
                      status = row("WorkingStatus").ToString(),
                      Remarks = row("Printing_Details").ToString()
        Into Group

        Dim sno As Integer = 1

        For Each g In grouped
            reportTable.Rows.Add(sno, g.billNo, g.dateVal, g.party, g.qty, g.machine, g.finish, g.status)

            For Each row In g.Group
                Dim gsm = If(row.Table.Columns.Contains("Paper_Size_GSM") AndAlso Not IsDBNull(row("Paper_Size_GSM")), "GSM : " & row("Paper_Size_GSM").ToString(), "GSM : ")
                Dim colour = If(row.Table.Columns.Contains("Printing_Colour") AndAlso Not IsDBNull(row("Printing_Colour")), "Colour : " & row("Printing_Colour").ToString(), "Colour : ")
                Dim method = If(row.Table.Columns.Contains("PMName") AndAlso Not IsDBNull(row("PMName")), "Method : " & row("PMName").ToString(), "Method : ")
                Dim type = If(row.Table.Columns.Contains("PTName") AndAlso Not IsDBNull(row("PTName")), "Type : " & row("PTName").ToString(), "Type : ")
                Dim brand = If(row.Table.Columns.Contains("Paper_Brand") AndAlso Not IsDBNull(row("Paper_Brand")), "Brand : " & row("Paper_Brand").ToString(), "Brand : ")
                Dim weight = If(row.Table.Columns.Contains("Paper_Weight") AndAlso Not IsDBNull(row("Paper_Weight")), "Weight: " & row("Paper_Weight").ToString(), "Weight: ")
                Dim Fdate = If(row.Table.Columns.Contains("FinishDate") AndAlso Not IsDBNull(row("FinishDate")), "Finish Date: " & row("FinishDate").ToString(), "Finish Date: ")
                reportTable.Rows.Add(DBNull.Value, gsm, colour, method, type, brand, weight, Fdate)
            Next

            For Each row In g.Group
                Dim details = If(row.Table.Columns.Contains("Printing_Details") AndAlso Not IsDBNull(row("Printing_Details")), row("Printing_Details").ToString(), "")
                reportTable.Rows.Add(DBNull.Value, "Printing Details", details, "", "", "", "", "")
            Next

            sno += 1
        Next

        sno = 1

        For Each g In grouped
            PrintTable.Rows.Add(sno, g.billNo, g.dateVal, g.party, g.qty, g.machine, g.finish, g.finishdate, g.status, g.Remarks)

            For Each row In g.Group
                Dim gsm = If(row.Table.Columns.Contains("Paper_Size_GSM") AndAlso Not IsDBNull(row("Paper_Size_GSM")), "GSM : " & row("Paper_Size_GSM").ToString(), "GSM : ")
                Dim colour = If(row.Table.Columns.Contains("Printing_Colour") AndAlso Not IsDBNull(row("Printing_Colour")), "Colour : " & row("Printing_Colour").ToString(), "Colour : ")
                Dim method = If(row.Table.Columns.Contains("PMName") AndAlso Not IsDBNull(row("PMName")), "Method : " & row("PMName").ToString(), "Method : ")
                Dim type = If(row.Table.Columns.Contains("PTName") AndAlso Not IsDBNull(row("PTName")), "Type : " & row("PTName").ToString(), "Type : ")
                Dim brand = If(row.Table.Columns.Contains("Paper_Brand") AndAlso Not IsDBNull(row("Paper_Brand")), "Brand : " & row("Paper_Brand").ToString(), "Brand : ")
                Dim weight = If(row.Table.Columns.Contains("Paper_Weight") AndAlso Not IsDBNull(row("Paper_Weight")), "Weight: " & row("Paper_Weight").ToString(), "Weight: ")
                Dim Fdate = If(row.Table.Columns.Contains("FinishDate") AndAlso Not IsDBNull(row("FinishDate")), "Finish Date: " & row("FinishDate").ToString(), "Finish Date: ")
                PrintTable.Rows.Add(DBNull.Value, gsm, colour, method, type, brand, weight, Fdate)
            Next

            For Each row In g.Group
                Dim details = If(row.Table.Columns.Contains("Printing_Details") AndAlso Not IsDBNull(row("Printing_Details")), row("Printing_Details").ToString(), "")
                PrintTable.Rows.Add(DBNull.Value, "Printing Details", details, "", "", "", "", "")
            Next

            sno += 1
        Next

        Return PrintTable
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
            '.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 40, 49)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersHeight = 35
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
            .Margin = New Padding(20)
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .MultiSelect = False
            .Dock = DockStyle.Fill

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
    Private Sub PrintButton_Click(sender As Object, e As EventArgs) Handles PrintButton.Click
        ExportPrintingToPDF()
    End Sub

    Private Sub ExportPrintingToPDF()
        Try
            Dim fd As Date, td As Date
            If Not Date.TryParseExact(FromDateTextBox.Text, "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, fd) OrElse
           Not Date.TryParseExact(ToDateTextBox.Text, "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, td) Then
                MessageBox.Show("Please enter valid From and To dates (dd/MM/yyyy).", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim result = MessageBox.Show("Do you want to print COMPLETED Printing?" & vbCrLf &
                                 "Click 'No' to print PENDING Printing.", "Print Options",
                                 MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

            If result = DialogResult.Cancel Then Return

            Dim statusFilter As String = If(result = DialogResult.Yes, "completed", "pending")
            Dim isPending As Boolean = (statusFilter = "pending")

            Dim companyName As String = "COMPANY"
            Using conn = Tools.GetConnection()
                Using cmd = New SqlCommand("SELECT TOP 1 comp_name FROM company_table", conn)
                    conn.Open()
                    Dim dbResult = cmd.ExecuteScalar()
                    If dbResult IsNot Nothing Then companyName = dbResult.ToString()
                End Using
            End Using

            Dim filteredRows = PrintDataTable.AsEnumerable().
            Where(Function(r)
                      Dim dVal As Date
                      If Not Date.TryParseExact(r.Field(Of String)("Printing_date"), "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, dVal) Then Return False
                      If dVal < fd OrElse dVal > td Then Return False

                      Dim statusCell As String = If(r.Table.Columns.Contains("WorkingStatus") AndAlso Not IsDBNull(r("WorkingStatus")), r("WorkingStatus").ToString().ToLower(), "")
                      Return statusCell = statusFilter
                  End Function).ToList()

            If filteredRows.Count = 0 Then
                MessageBox.Show($"No records to print for the selected status '{statusFilter.ToUpper()}' and dates.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim folderPath As String = Path.Combine(Application.StartupPath, "GS Report\")
            If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

            Dim filePath As String = Path.Combine(folderPath, $"PRINTING_{statusFilter.ToUpper()}_REPORT_{DateTime.Now:yyyyMMdd_HHmmss}.pdf")

            Using fs As New FileStream(filePath, FileMode.Create)
                Dim pageSize As New iTextSharp.text.Rectangle(595, 842)
                Dim document As New Document(pageSize, 15, 15, 20, 30)

                Dim writer As PdfWriter = PdfWriter.GetInstance(document, fs)
                writer.PageEvent = New PdfPageEvents()

                document.Open()

                Dim titleFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)
                Dim headerFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE)
                Dim bodyFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)
                Dim statusPendingFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.RED)
                Dim statusCompletedFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, New BaseColor(0, 128, 0))

                document.Add(New Paragraph(companyName.ToUpper(), titleFont) With {.Alignment = Element.ALIGN_CENTER})
                document.Add(New Paragraph($"PRINTING {statusFilter.ToUpper()} REPORT", bodyFont) With {.Alignment = Element.ALIGN_CENTER})
                document.Add(New Paragraph($"From: {fd:dd/MM/yyyy}   To: {td:dd/MM/yyyy}", bodyFont) With {.Alignment = Element.ALIGN_CENTER})
                document.Add(Chunk.NEWLINE)

                Dim cols As Dictionary(Of String, Integer)
                Dim colHeaders As Dictionary(Of String, String)

                If isPending Then
                    cols = New Dictionary(Of String, Integer) From {
                    {"S.No", 1}, {"Printing_date", 2}, {"Bill_No", 2},
                    {"Partyname", 4}, {"Quantity", 2}, {"MachineName", 3}, {"Printing_Details", 4}
                }

                    colHeaders = New Dictionary(Of String, String) From {
                    {"S.No", "S.NO"}, {"Printing_date", "DATE"}, {"Bill_No", "BILL NO"},
                    {"Partyname", "PARTY NAME"}, {"Quantity", "QTY"}, {"MachineName", "MACHINE"}, {"Printing_Details", "PRINTING REMARKS"}
                }
                Else
                    cols = New Dictionary(Of String, Integer) From {
                    {"S.No", 1}, {"Printing_date", 2}, {"Bill_No", 2},
                    {"Partyname", 4}, {"Quantity", 2}, {"MachineName", 3},
                    {"FinishQty", 2}, {"FinishDate", 2}, {"Printing_Details", 4}
                }

                    colHeaders = New Dictionary(Of String, String) From {
                    {"S.No", "S.NO"}, {"Printing_date", "DATE"}, {"Bill_No", "BILL NO"},
                    {"Partyname", "PARTY NAME"}, {"Quantity", "QTY"}, {"MachineName", "MACHINE"},
                    {"FinishQty", "FINISH"}, {"FinishDate", "FINISH DATE"}, {"Printing_Details", "PRINTING REMARKS"}
                }
                End If

                Dim table As New PdfPTable(cols.Count) With {.WidthPercentage = 100}
                table.SetWidths(cols.Values.Select(Function(w) CSng(w)).ToArray())

                For Each col In cols.Keys
                    Dim headerText As String = colHeaders(col)
                    Dim headerCell = New PdfPCell(New Phrase(headerText, headerFont)) With {
                    .BackgroundColor = New BaseColor(34, 40, 49),
                    .HorizontalAlignment = Element.ALIGN_CENTER,
                    .VerticalAlignment = Element.ALIGN_MIDDLE,
                    .Padding = 4
                }
                    table.AddCell(headerCell)
                Next

                Dim rowColor1 = BaseColor.WHITE
                Dim rowColor2 = BaseColor.WHITE
                Dim isAlternate As Boolean = False
                Dim serialNumber As Integer = 1

                For Each row In filteredRows
                    Dim bgColor = If(isAlternate, rowColor2, rowColor1)
                    isAlternate = Not isAlternate

                    Dim status = If(IsDBNull(row("WorkingStatus")), "", row("WorkingStatus").ToString())
                    Dim statusFont = If(status.ToLower() = "pending", statusPendingFont, statusCompletedFont)

                    For Each colKey In cols.Keys
                        Dim val As String = ""

                        Select Case colKey
                            Case "S.No"
                                val = serialNumber.ToString()
                            Case "FinishQty"
                                val = If(status.ToLower() = "completed" AndAlso Not IsDBNull(row("FinishQty")), row("FinishQty").ToString(), "")
                            Case "FinishDate"
                                val = If(status.ToLower() = "completed" AndAlso Not IsDBNull(row("FinishDate")), row("FinishDate").ToString(), "")
                            Case "Printing_Details"
                                val = If(status.ToLower() = "completed", "", If(IsDBNull(row("Printing_Details")), "", row("Printing_Details").ToString()))
                            Case Else
                                val = If(IsDBNull(row(colKey)), "", row(colKey).ToString())
                        End Select


                        Dim alignment As Integer = Element.ALIGN_LEFT
                        If {"S.No", "Quantity", "FinishQty"}.Contains(colKey) Then alignment = Element.ALIGN_RIGHT
                        If {"Printing_date", "FinishDate"}.Contains(colKey) Then alignment = Element.ALIGN_CENTER

                        Dim fontToUse As iTextSharp.text.Font = bodyFont

                        Dim cell = New PdfPCell(New Phrase(val, fontToUse)) With {
                            .HorizontalAlignment = alignment,
                            .VerticalAlignment = Element.ALIGN_MIDDLE,
                            .PaddingTop = 6,
                            .PaddingBottom = 6,
                            .BackgroundColor = bgColor,
                            .MinimumHeight = 18, ' or adjust to desired row height
                            .UseAscender = True,
                            .UseDescender = True,
                            .NoWrap = False,
                            .BorderWidth = 0.5F
                        }


                        table.AddCell(cell)
                    Next

                    serialNumber += 1
                Next

                document.Add(table)
                document.Close()
            End Using

            MessageBox.Show($"PDF saved successfully at:{vbCrLf}{filePath}", "PDF Export", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error exporting PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Public Class PdfPageEvents
        Inherits PdfPageEventHelper

        Public Overrides Sub OnEndPage(writer As PdfWriter, document As Document)
            Dim cb As PdfContentByte = writer.DirectContent
            Dim bf As BaseFont = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, False)
            cb.BeginText()
            cb.SetFontAndSize(bf, 8)
            cb.ShowTextAligned(Element.ALIGN_CENTER, "Page " & writer.PageNumber, document.PageSize.Width / 2, 10, 0)
            cb.EndText()
        End Sub
    End Class

    Private Sub AllPrintBtn_Click(sender As Object, e As EventArgs) Handles AllPrintBtn.Click
        ExportAllPrintingToPDF()
    End Sub

    Private Sub ExportAllPrintingToPDF()
        Try
            Dim fd As Date, td As Date
            If Not Date.TryParseExact(FromDateTextBox.Text, "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, fd) OrElse
           Not Date.TryParseExact(ToDateTextBox.Text, "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, td) Then
                MessageBox.Show("Please enter valid From and To dates (dd/MM/yyyy).", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim statusFilter As String = "all"

            Dim companyName As String = "COMPANY"
            Using conn = Tools.GetConnection()
                Using cmd = New SqlCommand("SELECT TOP 1 comp_name FROM company_table", conn)
                    conn.Open()
                    Dim dbResult = cmd.ExecuteScalar()
                    If dbResult IsNot Nothing Then companyName = dbResult.ToString()
                End Using
            End Using

            Dim filteredRows = PrintDataTable.AsEnumerable().Where(Function(r)
                                                                       Dim dVal As Date
                                                                       If Not Date.TryParseExact(r.Field(Of String)("Printing_date"), "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, dVal) Then Return False
                                                                       Return dVal >= fd AndAlso dVal <= td
                                                                   End Function).ToList()


            If filteredRows.Count = 0 Then
                MessageBox.Show($"No records to print for the selected status '{statusFilter.ToUpper()}' and dates.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim folderPath As String = Path.Combine(Application.StartupPath, "GS Report\")
            If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

            Dim filePath As String = Path.Combine(folderPath, $"PRINTING_{statusFilter.ToUpper()}_REPORT_{DateTime.Now:yyyyMMdd_HHmmss}.pdf")

            Using fs As New FileStream(filePath, FileMode.Create)
                Dim pageSize As New iTextSharp.text.Rectangle(595, 842)

                Dim document As New Document(pageSize, 15, 15, 20, 30)

                Dim writer As PdfWriter = PdfWriter.GetInstance(document, fs)
                writer.PageEvent = New PdfPageEvents()

                document.Open()

                Dim titleFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK)
                Dim headerFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.BOLD, BaseColor.WHITE)
                Dim bodyFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)
                Dim statusPendingFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.RED)
                Dim statusCompletedFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, New BaseColor(0, 128, 0))

                document.Add(New Paragraph(companyName.ToUpper(), titleFont) With {.Alignment = Element.ALIGN_CENTER})
                document.Add(New Paragraph($"PRINTING REPORT", bodyFont) With {.Alignment = Element.ALIGN_CENTER})
                document.Add(New Paragraph($"From: {fd:dd/MM/yyyy}   To: {td:dd/MM/yyyy}", bodyFont) With {.Alignment = Element.ALIGN_CENTER})
                document.Add(Chunk.NEWLINE)

                Dim columnHeaders = {"S.NO", "BILL NO", "DATE", "PARTY NAME", "QTY", "MACHINE", "FINISH", "STATUS", "PRINTING REMARKS"}
                Dim table As New PdfPTable(columnHeaders.Length) With {.WidthPercentage = 100}
                table.SetWidths(New Single() {4, 7, 8, 17, 5, 10, 6, 9, 20})

                For Each header In columnHeaders
                    Dim headerCell = New PdfPCell(New Phrase(header, headerFont)) With {
                    .BackgroundColor = New BaseColor(34, 40, 49),
                    .HorizontalAlignment = Element.ALIGN_CENTER,
                    .VerticalAlignment = Element.ALIGN_MIDDLE,
                    .Padding = 4
                }
                    table.AddCell(headerCell)
                Next


                Dim rowColor1 = BaseColor.WHITE
                Dim rowColor2 = BaseColor.WHITE
                Dim isAlternate As Boolean = False

                Dim serialNumber As Integer = 1

                For Each row In filteredRows
                    Dim bgColor = If(isAlternate, rowColor2, rowColor1)
                    isAlternate = Not isAlternate

                    Dim sno = serialNumber.ToString()
                    serialNumber += 1

                    'Dim sno = If(row.Cells("SNo").Value Is Nothing, "", row.Cells("SNo").Value.ToString())
                    Dim billNo = If(IsDBNull(row("Bill_No")), "", row("Bill_No").ToString())
                    Dim pDate = If(IsDBNull(row("Printing_date")), "", row("Printing_date").ToString())
                    Dim party = If(IsDBNull(row("Partyname")), "", row("Partyname").ToString())
                    Dim qty = If(IsDBNull(row("Quantity")), "", row("Quantity").ToString())
                    Dim machine = If(IsDBNull(row("MachineName")), "", row("MachineName").ToString())
                    Dim finish = If(IsDBNull(row("FinishQty")), "", row("FinishQty").ToString())
                    Dim status = If(IsDBNull(row("WorkingStatus")), "", row("WorkingStatus").ToString())
                    Dim remarks = ""
                    'Dim remarks = If(IsDBNull(row("Printing_Details")), "", row("Printing_Details").ToString())

                    Dim statusFont = If(status.ToLower() = "pending", statusPendingFont, statusCompletedFont)

                    Dim AddCell = Sub(text As String, fnt As iTextSharp.text.Font, alignment As Integer)
                                      Dim cell = New PdfPCell(New Phrase(text, fnt)) With {
                                      .HorizontalAlignment = alignment,
                                      .VerticalAlignment = Element.ALIGN_TOP,
                                      .Padding = 4,
                                      .BackgroundColor = bgColor,
                                      .NoWrap = False
                                  }
                                      table.AddCell(cell)
                                  End Sub

                    AddCell(sno, bodyFont, Element.ALIGN_CENTER)
                    AddCell(billNo, bodyFont, Element.ALIGN_LEFT)
                    AddCell(pDate, bodyFont, Element.ALIGN_CENTER)
                    AddCell(party, bodyFont, Element.ALIGN_LEFT)
                    AddCell(qty, bodyFont, Element.ALIGN_RIGHT)
                    AddCell(machine, bodyFont, Element.ALIGN_LEFT)
                    AddCell(finish, bodyFont, Element.ALIGN_RIGHT)

                    Dim statusCell = New PdfPCell(New Phrase(status, statusFont)) With {
                        .HorizontalAlignment = Element.ALIGN_CENTER,
                        .VerticalAlignment = Element.ALIGN_MIDDLE,
                            .PaddingTop = 6,
                            .PaddingBottom = 6,
                            .BackgroundColor = bgColor,
                            .MinimumHeight = 18,
                            .UseAscender = True,
                            .UseDescender = True,
                            .NoWrap = False,
                            .BorderWidth = 0.5F
                    }

                    table.AddCell(statusCell)

                    AddCell(remarks, bodyFont, Element.ALIGN_LEFT)

                Next

                document.Add(table)
                document.Close()
            End Using

            MessageBox.Show($"PDF saved successfully at:{vbCrLf}{filePath}", "PDF Export", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error exporting PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class

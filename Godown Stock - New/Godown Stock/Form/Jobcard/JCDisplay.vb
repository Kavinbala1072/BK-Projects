Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports System.IO
Imports Guna.UI2.WinForms

Public Class JCDisplay

    Private printFont As Font = New Font("Segoe UI", 8)
    Private headerFont As Font = New Font("Segoe UI", 10, FontStyle.Bold)
    Private titleFont As Font = New Font("Segoe UI", 12, FontStyle.Bold)
    Private currentRow As Integer = 0
    Private PrintDocument1 As New Drawing.Printing.PrintDocument
    Dim jobCardList As New List(Of Dictionary(Of String, String))
    Dim jobCardPrint As New List(Of Dictionary(Of String, Object))
    Private selectedJobCardId As String = ""
    Private dgvRowIndex As Integer = 0
    Private dtPrintData As DataTable
    Private reportHeading As String = ""
    Private fontBold As Font = New Font("Segoe UI", 8, FontStyle.Bold)
    Private Allfontbold As Font = New Font("Segoe UI", 8)
    Private PrintDocument2 As New PrintDocument()
    Private PrintDocument3 As New PrintDocument()
    Private pendingAddons As New List(Of Dictionary(Of String, String))
    Private reportFromDate As Date
    Private reportToDate As Date

    Private Sub DBConnect_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        FromDateTextBox.Focus()
    End Sub
    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles FromDateTextBox.KeyDown, ToDateTextBox.KeyDown, RefreshButton.KeyDown, DateTxt.KeyDown,
    UpdateTextBox.KeyDown, UpdateButton.KeyDown, AddonsButton.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.SelectNextControl(CType(sender, Control), forward:=True, tabStopOnly:=True, nested:=True, wrap:=True)
        End If
    End Sub
    Private Sub JCDisplay_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Tools.LoadConfiguration()
        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me
        DateTxt.Text = DateAndTime.Now.ToString("dd/MM/yyyy")
        FromDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy")
        ToDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy")

        Tools.LoadConfiguration()
        InitializeDataGridView()
        Themeload()
        RefreshItemList()
        AddHandler PrintDocument1.PrintPage, AddressOf PrintDocument1_PrintPage
        AddHandler PrintDocument2.PrintPage, AddressOf PrintPageHandler
        AddHandler PrintDocument3.PrintPage, AddressOf AllPrintPageHandler
        AddHandler KryptonContextMenuItem1.Click, AddressOf MultiPrint_Click
        AddHandler KryptonContextMenuItem2.Click, AddressOf AllPrint_Click

        'UpdateButton.Visible = False
        'UpdateTextBox.Visible = False
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
    Private Sub RefreshItemList()
        jobCardList.Clear()
        jobCardPrint.Clear()

        Dim fromDate As Date, toDate As Date
        If Not Date.TryParse(FromDateTextBox.Text, fromDate) OrElse Not Date.TryParse(ToDateTextBox.Text, toDate) Then
            MessageBox.Show("Please enter valid From and To dates.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim query As String =
        "SELECT jc.Bill_No, FORMAT(jc.JobCard_date, 'dd/MM/yyyy') AS JobCard_date, 
                lt.Partyname, np.Name AS NoteProcessing_English, np.TamilName AS NoteProcessing_Tamil,
                ns.Name AS NoteSize_English, ns.TamilName AS NoteSize_Tamil, jc.Paper_Size, jc.Note_Size,
                nt.Name AS NoteType, jc.Sheet, jc.Pages, jc.Note, jc.Reem, jc.Finishing, jc.WorkingStatus,
                jc.Manual_BillNo, jc.Cancel, jc.ID, FORMAT(jc.Finish_Date, 'dd/MM/yyyy') AS Finish_Date, jc.Paper_Brand, jc.Paper_GSM,
                jc.Paper_Weight, jc.no_Index, jc.Wrapper, jc.Remarks
         FROM JobCard_table jc
         LEFT JOIN Ledger_Table lt ON jc.ledger_id = lt.ID
         LEFT JOIN NoteProcessing_Table np ON jc.NoteProcessing_ID = np.ID
         LEFT JOIN NoteSize_Table ns ON jc.NoteSize_ID = ns.ID
         LEFT JOIN NoteType_Table nt ON jc.NoteType_Id = nt.ID
         WHERE jc.JobCard_date BETWEEN @FromDate AND @ToDate
         ORDER BY CAST(SUBSTRING(jc.Bill_No, CHARINDEX('/', jc.Bill_No) + 1,
                   CHARINDEX('/', jc.Bill_No, CHARINDEX('/', jc.Bill_No) + 1) 
                   - CHARINDEX('/', jc.Bill_No) - 1) AS INT) ASC, jc.JobCard_date ASC;"


        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Dim command As New SqlCommand(query, sqlconnect)
            command.Parameters.AddWithValue("@FromDate", fromDate.Date)
            command.Parameters.AddWithValue("@ToDate", toDate.Date)

            Try
                sqlconnect.Open()
                Dim reader As SqlDataReader = command.ExecuteReader()
                Dim rawTable As New DataTable()
                rawTable.Load(reader)

                Dim reportTable As New DataTable()
                reportTable.Columns.AddRange({
                New DataColumn("S.No", GetType(String)),
                New DataColumn("Bill_No", GetType(String)),
                New DataColumn("JobCard_date", GetType(String)),
                New DataColumn("Manual_BillNo", GetType(String)),
                New DataColumn("Partyname", GetType(String)),
                New DataColumn("Note", GetType(String)),
                New DataColumn("Finishing", GetType(String)),
                New DataColumn("WorkingStatus", GetType(String)),
                New DataColumn("Cancel", GetType(Boolean)),
                New DataColumn("ID", GetType(String)),
                New DataColumn("Finish_Date", GetType(String))
            })

                Dim serialNo As Integer = 1

                For Each row As DataRow In rawTable.Rows
                    Dim cancelStatus As Boolean = If(IsDBNull(row("cancel")), False, Convert.ToBoolean(row("cancel")))
                    Dim workingStatus As String = Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(row("WorkingStatus").ToString().ToUpper())

                    reportTable.Rows.Add(serialNo.ToString(), row("Bill_No"), row("JobCard_date"), row("Manual_BillNo"),
                                     row("Partyname"), row("Note"), row("Finishing"), workingStatus,
                                     cancelStatus, row("ID"), row("Finish_Date"))

                    Dim jobCardPData As New Dictionary(Of String, Object) From {
                    {"ID", row("ID")},
                    {"Bill_No", row("Bill_No")},
                    {"JobCard_date", row("JobCard_date")},
                    {"Manual_BillNo", row("Manual_BillNo")},
                    {"Partyname", row("Partyname")},
                    {"NoteProcessing", $"{row("NoteProcessing_English")} / {row("NoteProcessing_Tamil")}"},
                    {"NoteSize", $"{row("NoteSize_English")} / {row("NoteSize_Tamil")}"},
                    {"Paper_Size", row("Paper_Size")},
                    {"Note_Size", row("Note_Size")},
                    {"Sheet", row("Sheet")},
                    {"Pages", row("Pages")},
                    {"Note", row("Note")},
                    {"Reem", row("Reem")},
                    {"No_Index", row("No_Index")},
                    {"Wrapper", row("Wrapper")},
                    {"Paper_Brand", row("Paper_Brand")},
                    {"Paper_GSM", row("Paper_GSM")},
                    {"Paper_Weight", row("Paper_Weight")},
                    {"Finishing", row("Finishing")},
                    {"Finish", $"{CDate(row("Finish_Date")).ToString("dd/MM/yyyy")}"},
                    {"NoteType", If(IsDBNull(row("NoteType")), "", row("NoteType").ToString())},
                    {"Sheet_Index_Wrapper", $"{row("Sheet")} / {row("No_Index")} / {row("Wrapper")}"},
                    {"Pages_Note_Reem", $"{row("Pages")} / {row("Note")} / {row("Reem")}"},
                    {"PaperBrand_Size_GSM_Weight", $"{row("Paper_Brand")} / {row("Paper_Size")} / {row("Paper_GSM")} / {row("Paper_Weight")}"},
                    {"NoteSize_NoteType", $"{row("NoteSize_English")} / {row("NoteSize_Tamil")} / {row("NoteType")}"},
                    {"Remarks", row("Remarks")}
                }

                    Dim combinedBill As String = $"{row("Manual_BillNo")}-{row("Bill_No")}"
                    Dim addonValues As New Dictionary(Of String, String)

                    Using conn As SqlConnection = Tools.GetConnection()
                        conn.Open()
                        Using cmd As New SqlCommand("SELECT Processing_Method_Name, Value_Name FROM Addons_Table WHERE JC_BillNo = @combinedBill", conn)
                            cmd.Parameters.AddWithValue("@combinedBill", combinedBill)
                            Using addonReader As SqlDataReader = cmd.ExecuteReader()
                                While addonReader.Read()
                                    Dim methodName = addonReader("Processing_Method_Name").ToString()
                                    Dim valueName = addonReader("Value_Name").ToString()
                                    If Not addonValues.ContainsKey(methodName) Then
                                        addonValues(methodName) = valueName
                                    End If
                                End While
                            End Using
                        End Using
                    End Using

                    jobCardPData("Addons") = addonValues
                    jobCardPrint.Add(jobCardPData)

                    serialNo += 1
                Next

                Guna2DataGridView1.DataSource = reportTable

                With Guna2DataGridView1
                    .Columns("ID").Visible = False
                    .Columns("Finish_Date").Visible = False
                    .Columns("Cancel").Visible = False

                    For Each row As DataGridViewRow In .Rows
                        If Not row.IsNewRow Then
                            Dim isCancelled As Boolean = Convert.ToBoolean(row.Cells("Cancel").Value)
                            If Not String.IsNullOrEmpty(Convert.ToString(row.Cells("S.No").Value)) Then
                                If isCancelled Then
                                    row.Cells("Bill_No").Style.ForeColor = Color.Red
                                    row.Cells("WorkingStatus").Style.ForeColor = Color.Red
                                Else
                                    row.DefaultCellStyle.BackColor = Color.White
                                End If
                            Else
                                row.DefaultCellStyle.BackColor = Color.LightGray
                            End If
                        End If
                    Next

                    .Columns("S.No").Width = 50
                    .Columns("Bill_No").Width = 100
                    .Columns("JobCard_date").Width = 150
                    .Columns("Manual_BillNo").Width = 100
                    .Columns("Partyname").Width = 250
                    .Columns("Note").Width = 80
                    .Columns("Finishing").Width = 80
                    .Columns("WorkingStatus").Width = 120

                    .Columns("S.No").HeaderText = "S.No"
                    .Columns("Bill_No").HeaderText = "JC Bill No"
                    .Columns("JobCard_date").HeaderText = "JC Date"

                    For Each colName In {"S.No", "Bill_No", "JobCard_date", "Manual_BillNo", "Partyname", "Note", "Finishing", "WorkingStatus"}
                        .Columns(colName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                        .Columns(colName).SortMode = DataGridViewColumnSortMode.NotSortable
                    Next
                End With

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub
    Private Sub InitializeDataGridView()
        With Guna2DataGridView1
            .ColumnHeadersVisible = True
            .AllowUserToAddRows = False
            .AllowUserToOrderColumns = True
            '.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 40, 49)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersHeight = 35
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .ReadOnly = True
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
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

    Private Sub MultiPrint_Click(sender As Object, e As EventArgs)
        MultiPrint()
    End Sub
    Private Sub MultiPrint()
        Dim result = MessageBox.Show("Do you want to print COMPLETED Job Cards?" & vbCrLf &
                                 "Click 'No' to print PENDING", "Print Options",
                                 MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

        If result = DialogResult.Cancel Then Return

        Dim statusFilter = If(result = DialogResult.Yes, "COMPLETED", "PENDING")
        reportHeading = $"JOBCARD {statusFilter} REPORT"

        If Not LoadPrintData(statusFilter) Then Return

        If dtPrintData.Rows.Count = 0 Then
            MessageBox.Show($"No {statusFilter} data found for the selected date range.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        dgvRowIndex = 0
        pendingAddons.Clear()

        If statusFilter = "PENDING" Then
            For Each row As DataRow In dtPrintData.Rows
                Dim jobId = row("ID").ToString()
                Dim jc = jobCardPrint.FirstOrDefault(Function(j) j("ID").ToString() = jobId)
                If jc IsNot Nothing AndAlso jc.ContainsKey("Addons") Then
                    pendingAddons.Add(CType(jc("Addons"), Dictionary(Of String, String)))
                Else
                    pendingAddons.Add(New Dictionary(Of String, String)())
                End If
            Next
        End If

        ' 5. PRINTING LOGIC
        Dim folderPath = Path.Combine(Application.StartupPath, "GS Report\")
        If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

        Dim fileName = $"{reportHeading.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
        Dim fullPath = Path.Combine(folderPath, fileName)

        PrintDocument2.PrinterSettings.PrinterName = "Microsoft Print to PDF"
        PrintDocument2.PrinterSettings.PrintToFile = True
        PrintDocument2.PrinterSettings.PrintFileName = fullPath

        PrintDocument2.Print()
    End Sub

    Private Function LoadPrintData(statusType As String) As Boolean
        ' 1. Reset data containers
        dtPrintData = New DataTable()
        jobCardPrint.Clear()

        Dim fd As Date, td As Date
        If Not Date.TryParse(FromDateTextBox.Text, fd) OrElse Not Date.TryParse(ToDateTextBox.Text, td) Then
            MessageBox.Show("Please enter valid dates.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Dim sql As String
        If statusType = "PENDING" Then
            sql = "SELECT jc.ID, jc.Bill_No, FORMAT(jc.JobCard_date,'dd/MM/yyyy') AS JobCard_date, " &
              "jc.Manual_BillNo, lt.Partyname, jc.Note, jc.Finishing, jc.WorkingStatus, " &
              "FORMAT(jc.Finish_Date, 'dd/MM/yyyy') AS Finish_Date, np.Name AS NoteProcessing, jc.Remarks " &
              "FROM JobCard_table jc " &
              "LEFT JOIN Ledger_Table lt ON jc.ledger_id = lt.ID " &
              "LEFT JOIN NoteProcessing_Table np ON jc.NoteProcessing_ID = np.ID " &
              "WHERE jc.JobCard_date BETWEEN @fd AND @td " &
              "AND jc.WorkingStatus = 'PENDING' " &
              "ORDER BY jc.JobCard_date, jc.Manual_BillNo"
        Else
            sql = "SELECT jc.ID, jc.Bill_No, FORMAT(jc.JobCard_date,'dd/MM/yyyy') AS JobCard_date, " &
              "jc.Manual_BillNo, lt.Partyname, jc.Note, jc.Finishing, jc.WorkingStatus, " &
              "FORMAT(jc.Finish_Date, 'dd/MM/yyyy') AS Finish_Date, np.Name AS NoteProcessing, jc.Remarks " &
              "FROM JobCard_table jc " &
              "LEFT JOIN Ledger_Table lt ON jc.ledger_id = lt.ID " &
              "LEFT JOIN NoteProcessing_Table np ON jc.NoteProcessing_ID = np.ID " &
              "WHERE jc.Finish_Date BETWEEN @fd AND @td " &
              "AND jc.WorkingStatus = 'COMPLETED' " &
              "ORDER BY jc.Finish_Date, jc.Manual_BillNo"
        End If

        Using conn = Tools.GetConnection()
            Using cmd = New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@fd", fd.Date)
                cmd.Parameters.AddWithValue("@td", td.Date)
                conn.Open()
                Using rdr = cmd.ExecuteReader()
                    dtPrintData.Load(rdr)
                End Using
            End Using
        End Using

        If Not dtPrintData.Columns.Contains("S.No") Then
            dtPrintData.Columns.Add("S.No", GetType(String))
        End If

        For i As Integer = 0 To dtPrintData.Rows.Count - 1
            Dim dr = dtPrintData.Rows(i)
            dr("S.No") = (i + 1).ToString()

            ' Create dictionary for print mapping
            Dim dict As New Dictionary(Of String, Object) From {
            {"ID", dr("ID").ToString()},
            {"WorkingStatus", dr("WorkingStatus").ToString()},
            {"S.No", dr("S.No").ToString()}
        }

            ' Fetch Addons for this specific Bill Number
            Dim addons = New Dictionary(Of String, String)()
            Dim combinedBillNo = $"{dr("Manual_BillNo")}-{dr("Bill_No")}"

            Using conn = Tools.GetConnection()
                Using cmd2 = New SqlCommand("SELECT Processing_Method_Name, Value_Name FROM Addons_Table WHERE JC_BillNo=@cb", conn)
                    cmd2.Parameters.AddWithValue("@cb", combinedBillNo)
                    conn.Open()
                    Using ar = cmd2.ExecuteReader()
                        While ar.Read()
                            addons(ar("Processing_Method_Name").ToString()) = ar("Value_Name").ToString()
                        End While
                    End Using
                End Using
            End Using

            dict("Addons") = addons
            jobCardPrint.Add(dict)
        Next

        ' Update report globals
        reportFromDate = fd
        reportToDate = td

        ' Return True if we actually found records
        Return dtPrintData.Rows.Count > 0
    End Function
    Private Sub PrintPageHandler(sender As Object, e As PrintPageEventArgs)
        If dtPrintData Is Nothing OrElse dtPrintData.Rows.Count = 0 Then
            e.HasMorePages = False
            Return
        End If

        Dim ml = 20, mt = 20, mr = e.PageBounds.Width - 20, mb = e.PageBounds.Height - 40
        Dim uw = mr - ml
        Dim y = mt + 80

        Dim chSize = e.Graphics.MeasureString("THANGAM NOTE BOOKSS, PERUNDURAI", headerFont)
        e.Graphics.DrawString("THANGAM NOTE BOOKSS, PERUNDURAI", headerFont, Brushes.Black, ml + (uw - chSize.Width) / 2, mt)

        Dim headingSize = e.Graphics.MeasureString(reportHeading, headerFont)
        e.Graphics.DrawString(reportHeading, headerFont, Brushes.Black, ml + (uw - headingSize.Width) / 2, mt + 20)

        Dim dateRangeText = $"From: {reportFromDate:dd/MM/yyyy}   To: {reportToDate:dd/MM/yyyy}"
        Dim dateSize = e.Graphics.MeasureString(dateRangeText, printFont)
        e.Graphics.DrawString(dateRangeText, printFont, Brushes.Black, ml + (uw - dateSize.Width) / 2, mt + 40)

        Dim isPending = reportHeading.Contains("PENDING")
        Dim cols As New Dictionary(Of String, Integer)
        Dim colHeaders As New Dictionary(Of String, String)

        If isPending Then
            cols = New Dictionary(Of String, Integer) From {
            {"S.No", 1}, {"JobCard_date", 2}, {"Manual_BillNo", 2},
            {"Partyname", 4}, {"Note", 2}, {"NoteProcessing", 4}, {"Remarks", 4}
        }

            colHeaders = New Dictionary(Of String, String) From {
            {"S.No", "S.NO"}, {"JobCard_date", "JC DATE"}, {"Manual_BillNo", "JC BILL NO"},
            {"Partyname", "PARTY NAME"}, {"Note", "NOTE"}, {"NoteProcessing", "NOTE PROCESSING"}, {"Remarks", "REMARKS"}
        }
        Else
            cols = New Dictionary(Of String, Integer) From {
            {"S.No", 1}, {"JobCard_date", 2}, {"Manual_BillNo", 2},
            {"Partyname", 4}, {"Note", 2}, {"Finishing", 2}, {"Finish_Date", 2}, {"Remarks", 4}
        }

            colHeaders = New Dictionary(Of String, String) From {
            {"S.No", "S.NO"}, {"JobCard_date", "JC DATE"}, {"Manual_BillNo", "JC BILL NO"},
            {"Partyname", "PARTY NAME"}, {"Note", "NOTE"}, {"Finishing", "F.NOTE"},
            {"Finish_Date", "FINISH DATE"}, {"Remarks", "REMARKS"}
        }
        End If

        Dim totalW = cols.Values.Sum()
        Dim cWidths = cols.Values.Select(Function(w) CInt(w / totalW * uw)).ToArray()
        Dim x = ml, idx = 0

        For Each kv In cols
            Dim hrect = New Rectangle(x, y, cWidths(idx), CInt(e.Graphics.MeasureString(kv.Key, fontBold).Height) + 8)
            e.Graphics.FillRectangle(Brushes.LightGray, hrect)
            e.Graphics.DrawRectangle(Pens.Black, hrect)
            e.Graphics.DrawString(colHeaders(kv.Key), fontBold, Brushes.Black, hrect)
            x += cWidths(idx)
            idx += 1
        Next
        y += CInt(e.Graphics.MeasureString("A", fontBold).Height) + 8

        Static pageNum = 1

        While dgvRowIndex < dtPrintData.Rows.Count
            Dim dr = dtPrintData.Rows(dgvRowIndex)
            Dim rowHeight = 0
            x = ml

            For i = 0 To cols.Count - 1
                Dim colKey = cols.Keys(i)
                Dim txt As String

                If isPending AndAlso colKey = "NoteProcessing" Then
                    Dim addons = pendingAddons(dgvRowIndex)
                    txt = If(addons.Any(), String.Join(Environment.NewLine, addons.Select(Function(kvp) $"• {kvp.Key}: {kvp.Value}")), "-")
                Else
                    txt = If(colKey = "Remarks", "", dr(colKey).ToString())
                End If

                Dim sz = e.Graphics.MeasureString(txt, printFont, cWidths(i))
                rowHeight = Math.Max(rowHeight, CInt(sz.Height) + 5)
            Next

            If y + rowHeight > mb Then
                e.HasMorePages = True
                pageNum += 1
                Return
            End If

            For i = 0 To cols.Count - 1
                Dim colKey = cols.Keys(i)
                Dim txt As String

                If isPending AndAlso colKey = "NoteProcessing" Then
                    Dim addons = pendingAddons(dgvRowIndex)
                    txt = If(addons.Any(), String.Join(Environment.NewLine, addons.Select(Function(kvp) $"• {kvp.Key}: {kvp.Value}")), "-")
                Else
                    txt = If(colKey = "Remarks", "", dr(colKey).ToString())
                End If

                Dim r = New Rectangle(x, y, cWidths(i), rowHeight)
                e.Graphics.DrawRectangle(Pens.Black, r)
                e.Graphics.DrawString(txt, printFont, Brushes.Black, r)
                x += cWidths(i)
            Next

            y += rowHeight  'Added space between rows(24/01/2025)

            Using p As New Pen(Color.Black, 1)
                e.Graphics.DrawLine(p, ml, y, ml + uw, y)
                e.Graphics.DrawLine(p, ml, y - rowHeight, ml, y + 25)      ' Left
                If isPending Then
                    e.Graphics.DrawLine(p, ml + uw + 1, y - rowHeight, ml + uw + 1, y + 25) ' Right
                Else
                    e.Graphics.DrawLine(p, ml + uw, y - rowHeight, ml + uw, y + 25) ' Right
                End If

                e.Graphics.DrawLine(p, ml, y + 25, ml + uw, y + 25)
                Dim remarksRect = New Rectangle(ml + 5, y, uw - 5, 25)
                Dim remarksValue = "REMARKS: " & dr("Remarks").ToString()

                Dim sfRemarks As New StringFormat()
                sfRemarks.Alignment = StringAlignment.Near
                sfRemarks.LineAlignment = StringAlignment.Center

                e.Graphics.DrawString(remarksValue, Allfontbold, Brushes.Black, remarksRect, sfRemarks)
            End Using

            y += 25

            dgvRowIndex += 1
        End While

        e.Graphics.DrawString($"Page {pageNum}", printFont, Brushes.Black, mr - 100, mb + 10)
        e.HasMorePages = False
        dgvRowIndex = 0
        pageNum = 1
    End Sub

    Private Sub AllPrint_Click(sender As Object, e As EventArgs)
        AllPrint()
    End Sub
    Private Sub AllPrint()
        If Not LoadAllPrintData() Then Return

        reportHeading = "JOBCARD ALL STATUS REPORT"

        Dim folderPath = Path.Combine(Application.StartupPath, "GS Report\")
        If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

        Dim fileName = $"{reportHeading.Replace(" ", "_")}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
        Dim fullPath = Path.Combine(folderPath, fileName)

        PrintDocument3.PrinterSettings.PrinterName = "Microsoft Print to PDF"
        PrintDocument3.PrinterSettings.PrintToFile = True
        PrintDocument3.PrinterSettings.PrintFileName = fullPath

        PrintDocument3.Print()

        'MessageBox.Show($"PDF saved to:{vbCrLf}{fullPath}", "Exported", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
    Private Function LoadAllPrintData() As Boolean
        dtPrintData = New DataTable()
        jobCardPrint.Clear()

        Dim fd As Date, td As Date
        If Not Date.TryParse(FromDateTextBox.Text, fd) OrElse Not Date.TryParse(ToDateTextBox.Text, td) Then
            MessageBox.Show("Please enter valid dates.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        End If

        Dim sql = "SELECT jc.ID, jc.Bill_No, FORMAT(jc.JobCard_date,'dd/MM/yyyy') AS JobCard_date,
               jc.Manual_BillNo, lt.Partyname, jc.Note, jc.Finishing, jc.WorkingStatus,
               FORMAT(jc.Finish_Date, 'dd/MM/yyyy') AS Finish_Date, jc.Remarks
               FROM JobCard_table jc
               LEFT JOIN Ledger_Table lt ON jc.ledger_id = lt.ID
               WHERE jc.JobCard_date BETWEEN @fd AND @td
               ORDER BY jc.Bill_No, jc.JobCard_date"

        Using conn = Tools.GetConnection()
            Using cmd = New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@fd", fd.Date)
                cmd.Parameters.AddWithValue("@td", td.Date)
                conn.Open()
                Using rdr = cmd.ExecuteReader()
                    dtPrintData.Load(rdr)
                    dtPrintData.Columns.Add("S.No", GetType(String))
                    dtPrintData.Columns.Add("NoteProcessing", GetType(String))
                    For i = 0 To dtPrintData.Rows.Count - 1
                        dtPrintData.Rows(i)("S.No") = (i + 1).ToString()
                    Next
                End Using
            End Using
        End Using

        Dim addonsLookup As New Dictionary(Of String, Dictionary(Of String, String))()
        Using conn = Tools.GetConnection()
            Using cmd = New SqlCommand("SELECT JC_BillNo, Processing_Method_Name, Value_Name FROM Addons_Table", conn)
                conn.Open()
                Using rdr = cmd.ExecuteReader()
                    While rdr.Read()
                        Dim jcBillNo = rdr("JC_BillNo").ToString()
                        Dim method = rdr("Processing_Method_Name").ToString()
                        Dim value = rdr("Value_Name").ToString()

                        If Not addonsLookup.ContainsKey(jcBillNo) Then
                            addonsLookup(jcBillNo) = New Dictionary(Of String, String)()
                        End If
                        addonsLookup(jcBillNo)(method) = value
                    End While
                End Using
            End Using
        End Using

        For Each dr As DataRow In dtPrintData.Rows
            Dim dict As New Dictionary(Of String, Object) From {
            {"ID", dr("ID").ToString()},
            {"WorkingStatus", dr("WorkingStatus").ToString()},
            {"S.No", dr("S.No").ToString()}
        }

            Dim combined = $"{dr("Manual_BillNo")}-{dr("Bill_No")}"
            Dim addons As Dictionary(Of String, String) = Nothing

            If addonsLookup.ContainsKey(combined) Then
                addons = addonsLookup(combined)
            Else
                addons = New Dictionary(Of String, String)()
            End If

            Dim formattedAddons = If(addons.Any(), String.Join(", ", addons.Select(Function(kvp) $"{kvp.Key}: {kvp.Value}")), "-")
            dr("NoteProcessing") = formattedAddons

            dict("Addons") = addons
            jobCardPrint.Add(dict)
        Next

        reportFromDate = fd
        reportToDate = td
        Return True
    End Function

    Private Sub AllPrintPageHandler(sender As Object, e As PrintPageEventArgs)
        If dtPrintData Is Nothing OrElse dtPrintData.Rows.Count = 0 Then
            e.HasMorePages = False
            Return
        End If

        Dim ml = 20, mt = 20, mr = e.PageBounds.Width - 20, mb = e.PageBounds.Height - 40
        Dim uw = mr - ml - 5
        Dim y = mt + 80

        Dim chSize = e.Graphics.MeasureString("THANGAM NOTE BOOKSS, PERUNDURAI", headerFont)
        e.Graphics.DrawString("THANGAM NOTE BOOKSS, PERUNDURAI", headerFont, Brushes.Black, ml + (uw - chSize.Width) / 2, mt)

        Dim headingSize = e.Graphics.MeasureString(reportHeading, headerFont)
        e.Graphics.DrawString(reportHeading, headerFont, Brushes.Black, ml + (uw - headingSize.Width) / 2, mt + 20)

        Dim dateRangeText = $"From: {reportFromDate:dd/MM/yyyy}   To: {reportToDate:dd/MM/yyyy}"
        Dim dateSize = e.Graphics.MeasureString(dateRangeText, printFont)
        e.Graphics.DrawString(dateRangeText, printFont, Brushes.Black, ml + (uw - dateSize.Width) / 2, mt + 40)

        Dim cols As New Dictionary(Of String, Integer)
        Dim colHeaders As New Dictionary(Of String, String)

        cols = New Dictionary(Of String, Integer) From {
        {"S.No", 1}, {"JobCard_date", 2}, {"Manual_BillNo", 2}, {"Partyname", 4}, {"Note", 2}, {"NoteProcessing", 5}, {"Finishing", 2}, {"WorkingStatus", 2}, {"Bill_No", 3}}

        colHeaders = New Dictionary(Of String, String) From {
        {"S.No", "S.NO"}, {"JobCard_date", "JC DATE"}, {"Manual_BillNo", "JC BILL NO"}, {"Partyname", "PARTY NAME"}, {"Note", "NOTE"}, {"NoteProcessing", "NOTE PROCESSING"}, {"Finishing", "F.NOTE"}, {"WorkingStatus", "JC STATUS"}, {"Bill_No", "REMARKS"}}

        Dim totalW = cols.Values.Sum()
        Dim cWidths = cols.Values.Select(Function(w) CInt(w / totalW * uw)).ToArray()
        Dim x = ml, idx = 0

        For Each kv In cols
            Dim hrect = New Rectangle(x, y, cWidths(idx), CInt(e.Graphics.MeasureString(kv.Key, fontBold).Height) + 15)
            e.Graphics.FillRectangle(Brushes.LightGray, hrect)
            e.Graphics.DrawRectangle(Pens.Black, hrect)
            e.Graphics.DrawString(colHeaders(kv.Key), fontBold, Brushes.Black, hrect)
            x += cWidths(idx)
            idx += 1
        Next
        y += CInt(e.Graphics.MeasureString("A", fontBold).Height) + 15

        Static pageNum = 1

        While dgvRowIndex < dtPrintData.Rows.Count
            Dim dr = dtPrintData.Rows(dgvRowIndex)
            Dim rowHeight As Integer = 0
            Dim lineHeight As Single = e.Graphics.MeasureString("A", Allfontbold).Height
            Dim minRowHeight As Integer = CInt(lineHeight * 2 + 6)
            x = ml

            For i = 0 To cols.Count - 1
                Dim colKey = cols.Keys(i)
                Dim txt As String = dr(colKey).ToString().Trim()
                Dim rowSpacing = "SELECT ctl_value FROM Control_Table WHERE Ctl_Desc = 'JC_rowSpacing'"   'Added space between rows(09/12/2025)

                Try
                    Using conn = Tools.GetConnection()
                        conn.Open()
                        Using cmd = New SqlCommand(rowSpacing, conn)
                            Dim result = cmd.ExecuteScalar()
                            If result IsNot Nothing Then
                                rowSpacing = Convert.ToInt32(result)
                            Else
                                MessageBox.Show("Box width record not found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Return
                            End If
                        End Using
                    End Using
                Catch ex As Exception
                    MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try


                If Not String.IsNullOrWhiteSpace(txt) Then
                    Dim layoutSize As New SizeF(cWidths(i), Single.MaxValue)
                    Dim sz = e.Graphics.MeasureString(txt, Allfontbold, layoutSize)
                    rowHeight = Math.Max(rowHeight, CInt(sz.Height) + 10 + rowSpacing)
                End If
            Next

            If rowHeight < minRowHeight Then
                rowHeight = minRowHeight
            End If

            If y + rowHeight > mb Then
                e.HasMorePages = True
                pageNum += 1
                Return
            End If

            x = ml
            For i = 0 To cols.Count - 1
                Dim colKey = cols.Keys(i)
                Dim txt As String = If(colKey = "Bill_No", "", dr(colKey).ToString())

                If colKey = "NoteProcessing" Then
                    txt = txt.Trim().Trim(","c)
                    txt = String.Join(", ", txt.Split(","c).
                Select(Function(s) s.Trim()).
                Where(Function(s) Not String.IsNullOrWhiteSpace(s)))
                End If

                Dim r = New Rectangle(x, y, cWidths(i), rowHeight)
                e.Graphics.DrawRectangle(Pens.Black, r)

                Dim sf As New StringFormat()
                sf.Alignment = StringAlignment.Near
                sf.LineAlignment = StringAlignment.Near
                sf.FormatFlags = StringFormatFlags.LineLimit

                e.Graphics.DrawString(txt, Allfontbold, Brushes.Black, r, sf)

                x += cWidths(i)
            Next

            y += rowHeight  'Added space between rows(17/01/2025)

            Using p As New Pen(Color.Black, 1)
                ' 1. Draw the lines
                e.Graphics.DrawLine(p, ml, y, ml + uw, y)
                e.Graphics.DrawLine(p, ml, y - rowHeight, ml, y + 25)      ' Left
                e.Graphics.DrawLine(p, ml + uw, y - rowHeight, ml + uw, y + 25) ' Right
                e.Graphics.DrawLine(p, ml, y + 25, ml + uw, y + 25)
                Dim remarksRect = New Rectangle(ml + 5, y, uw - 5, 25)
                Dim remarksValue = "REMARKS: " & dr("Remarks").ToString()

                Dim sfRemarks As New StringFormat()
                sfRemarks.Alignment = StringAlignment.Near
                sfRemarks.LineAlignment = StringAlignment.Center

                e.Graphics.DrawString(remarksValue, Allfontbold, Brushes.Black, remarksRect, sfRemarks)
            End Using

            y += 25

            dgvRowIndex += 1
        End While

        e.Graphics.DrawString($"Page {pageNum}", printFont, Brushes.Black, mr - 100, mb + 10)

        e.HasMorePages = False
        dgvRowIndex = 0
        pageNum = 1
    End Sub

    Private Sub PrintButton_Click(sender As Object, e As EventArgs) Handles PrintButton.Click
        If String.IsNullOrEmpty(selectedJobCardId) Then
            MessageBox.Show("Please select a valid Job Card row to print.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        'Dim a4Size As New PaperSize("A4", 583, 827)
        Dim a4Size As New PaperSize("A5", 583, 500)
        PrintDocument1.DefaultPageSettings.PaperSize = a4Size

        Dim printDialog As New PrintDialog()
        printDialog.Document = PrintDocument1

        If printDialog.ShowDialog() = DialogResult.OK Then
            dgvRowIndex = 0
            PrintDocument1.Print()
        End If
    End Sub
    Private Function GetValue(dict As Dictionary(Of String, Object), key As String) As String
        If dict.ContainsKey(key) AndAlso dict(key) IsNot Nothing Then
            Return dict(key).ToString()
        End If
        Return ""
    End Function
    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        Dim pen As New Pen(Color.Black, 1)
        Dim font As New Font("Arial", 10)
        Dim boldFont As New Font("Arial", 13, FontStyle.Bold)
        Dim bfont As New Font("Arial", 10, FontStyle.Bold)
        Dim boxwidth As Integer
        Dim pageWidth As Integer

        Dim boxQuery = "SELECT ctl_value FROM Control_Table WHERE Ctl_Desc = 'Print_BoxWidth'"
        Dim pageQuery = "SELECT ctl_value FROM Control_Table WHERE Ctl_Desc = 'Print_PageWidth'"

        Try
            Using conn = Tools.GetConnection()
                conn.Open()
                Using cmd = New SqlCommand(boxQuery, conn)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        boxwidth = Convert.ToInt32(result)
                    Else
                        MessageBox.Show("Box width record not found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                End Using

                Using cmd = New SqlCommand(pageQuery, conn)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        pageWidth = Convert.ToInt32(result)
                    Else
                        MessageBox.Show("Page width record not found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Dim marginLeft = 20
        Dim marginTop = 20
        Dim y = marginTop
        Dim boxHeight = 28
        Dim paddingLeft = 4

        Dim data = jobCardPrint.FirstOrDefault(Function(jc) jc("ID").ToString() = selectedJobCardId)
        If data Is Nothing Then Exit Sub

        Dim title = "THANGAM NOTE BOOKSS, PERUNDURAI"
        Dim titleWidth = g.MeasureString(title, boldFont).Width
        g.DrawString(title, boldFont, Brushes.Black, (pageWidth - titleWidth) / 2, y)
        y += 40

        g.DrawString("JOBCARD NO", font, Brushes.Black, marginLeft, y + 6)
        g.DrawRectangle(pen, marginLeft + 105, y - 2, 120, boxHeight)
        g.DrawString(data("Bill_No").ToString(), bfont, Brushes.Black, marginLeft + 109, y + 4)

        g.DrawRectangle(pen, marginLeft + 225, y - 2, 80, boxHeight)
        g.DrawString(data("Manual_BillNo").ToString(), bfont, Brushes.Black, marginLeft + 229, y + 4)

        g.DrawString("DATE", font, Brushes.Black, pageWidth - 175, y + 6)
        g.DrawRectangle(pen, pageWidth - 125, y - 2, 100, boxHeight)
        g.DrawString(data("JobCard_date").ToString(), bfont, Brushes.Black, pageWidth - 110, y + 4)

        y += boxHeight + 6

        Dim sheetIndexWrapper As String = "SHEET : " & data("Sheet").ToString() & " ,/  INDEX : " & data("No_Index").ToString() & " ,/  WRAPPER : " & data("Wrapper").ToString()
        Dim paperDetails As String = data("Paper_Brand").ToString().ToUpper() & " ,/  SIZE : " & data("Paper_Size").ToString() & " ,/  GSM : " & data("Paper_GSM").ToString() & " ,/  WEIGHT : " & data("Paper_Weight").ToString()
        Dim pagesNoteReem As String = "NO OF PAGE : " & data("Pages").ToString() & " ,/  NOTE : " & data("Note").ToString() & " ,/  REEM : " & data("Reem").ToString()
        Dim finishDateValue As String = data("Finish").ToString()
        Dim finishingDate As String

        If finishDateValue = "01/01/1999" Then
            finishingDate = " " & data("Finishing").ToString()
        Else
            finishingDate = "NO OF FINISHING NOTE : " & data("Finishing").ToString() & " ,/  DATE : " & finishDateValue
        End If

        Dim fields = {
        "SCHOOL / WRAPPER NAME",
        "NOTE PROCESSING METHOD",
        "NOTE SIZE / NOTE TYPE",
        "NO OF SHEET / INDEX / WRAPPER",
        "PAPER BRAND / SIZE / GSM / WEIGHT",
        "NO OF PAGE / NOTE / REEM",
        "NO OF FINISHING NOTE / DATE",
        ""
    }

        Dim values = {
        data("Partyname").ToString(),
        data("NoteProcessing").ToString(),
        data("NoteSize_NoteType").ToString(),
        sheetIndexWrapper,
        paperDetails,
        pagesNoteReem,
        finishingDate,
        data("Remarks").ToString()
    }

        For i = 0 To fields.Length - 1
            If i = 7 AndAlso String.IsNullOrWhiteSpace(values(i)) Then Continue For

            If i = 7 Then
                g.DrawRectangle(pen, marginLeft, y, 265 + boxwidth, boxHeight)
                g.DrawString(values(i), bfont, Brushes.Black, marginLeft + paddingLeft, y + 4)
                y += boxHeight
                Continue For
            End If

            g.DrawRectangle(pen, marginLeft, y, 265, boxHeight)
            g.DrawString(fields(i), font, Brushes.Black, marginLeft + paddingLeft, y + 4)

            g.DrawRectangle(pen, marginLeft + 265, y, boxwidth, boxHeight)

            If i >= 3 Then
                Dim parts = values(i).Split(New String() {" ,"}, StringSplitOptions.None)
                Dim xpos As Integer = marginLeft + 270

                For Each part In parts
                    Dim splitIndex = part.IndexOf(":")
                    If splitIndex > 0 Then
                        Dim labelPart = part.Substring(0, splitIndex + 1)
                        Dim valuePart = part.Substring(splitIndex + 1).Trim()

                        g.DrawString(labelPart, font, Brushes.Black, xpos, y + 4)
                        xpos += CInt(g.MeasureString(labelPart, font).Width)

                        g.DrawString(valuePart, bfont, Brushes.Black, xpos, y + 4)
                        xpos += CInt(g.MeasureString(valuePart, bfont).Width) + CInt(g.MeasureString(" / ", font).Width)
                    Else
                        g.DrawString(part, bfont, Brushes.Black, xpos, y + 4)
                        xpos += CInt(g.MeasureString(part, bfont).Width) + CInt(g.MeasureString(" / ", font).Width)
                    End If
                Next
            Else
                g.DrawString(values(i), bfont, Brushes.Black, marginLeft + 270, y + 4)
            End If

            y += boxHeight
        Next


        If data.ContainsKey("Addons") Then
            Dim addons = CType(data("Addons"), Dictionary(Of String, String))
            For Each kvp In addons
                g.DrawRectangle(pen, marginLeft, y, 265, boxHeight)
                g.DrawString(kvp.Key, font, Brushes.Black, marginLeft + paddingLeft, y + 4)

                g.DrawRectangle(pen, marginLeft + 265, y, boxwidth, boxHeight)
                g.DrawString(kvp.Value, bfont, Brushes.Black, marginLeft + 270, y + 4)

                y += boxHeight
            Next
        End If

        y += 40
        Dim footer = "Prepared by" & Space(60) & "Checked by" & Space(60) & "Managing Director"
        titleWidth = g.MeasureString(footer, font).Width
        g.DrawString(footer, font, Brushes.Black, (pageWidth - titleWidth) / 2, y)

        e.HasMorePages = False
    End Sub

    Private Sub AddonsButton_Click(sender As Object, e As EventArgs) Handles AddonsButton.Click
        If Guna2DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row to open Addons.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)
        Dim manualBillNo As String = selectedRow.Cells("Manual_BillNo").Value?.ToString().Trim()
        Dim billNo As String = selectedRow.Cells("Bill_No").Value?.ToString().Trim()

        If String.IsNullOrWhiteSpace(manualBillNo) OrElse String.IsNullOrWhiteSpace(billNo) Then
            MessageBox.Show("Selected row is missing Manual_BillNo or Bill_No.", "Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        For Each openForm As Form In Application.OpenForms
            If TypeOf openForm Is JCDprocessing Then
                Dim form As JCDprocessing = CType(openForm, JCDprocessing)
                form.ManualBillNo = manualBillNo
                form.BillNo = billNo
                form.BringToFront()
                form.Focus()
                Return
            End If
        Next

        Dim newForm As New JCDprocessing()
        newForm.ManualBillNo = manualBillNo
        newForm.BillNo = billNo
        newForm.Show()
    End Sub
    Private Sub Guna2DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles Guna2DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = Guna2DataGridView1.Rows(e.RowIndex)

            If row.Cells("ID").Value IsNot Nothing Then
                selectedJobCardId = Convert.ToString(row.Cells("ID").Value)
            Else
                selectedJobCardId = ""
            End If
        End If
    End Sub

    Private Sub Guna2DataGridView1_DoubleClick(sender As Object, e As EventArgs) Handles Guna2DataGridView1.DoubleClick
        Try
            If Guna2DataGridView1.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a row to alter.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)
            Dim idObj As Object = selectedRow.Cells("ID").Value

            If idObj Is Nothing OrElse IsDBNull(idObj) Then
                MessageBox.Show("No Job Card ID found in the selected row.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim jobCardID As String = idObj.ToString()

            Dim isCancelled As Boolean = False
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()
                Using cmd As New SqlCommand("SELECT cancel FROM jobcard_table WHERE ID = @ID", sqlconnect)
                    cmd.Parameters.AddWithValue("@ID", jobCardID)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        isCancelled = Convert.ToBoolean(result)
                    End If
                End Using
            End Using

            If isCancelled Then
                MessageBox.Show("This Job Card has been cancelled and cannot be altered.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If
            Dim stockForm As Stock = CType(Application.OpenForms("Stock"), Stock)
            If stockForm Is Nothing Then
                MessageBox.Show("Stock form is not open.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            stockForm.LoadFormToKryptonNavigator(Of Jobcard)("Jobcard")

            For Each page As ComponentFactory.Krypton.Navigator.KryptonPage In stockForm.KryptonDockableNavigator1.Pages
                If page.Text = "Jobcard" Then
                    Dim jobCardForm As Jobcard = TryCast(page.Controls.OfType(Of Jobcard).FirstOrDefault(), Jobcard)
                    If jobCardForm IsNot Nothing Then
                        Dim billNo As String = selectedRow.Cells("Bill_No").Value.ToString()
                        jobCardForm.AlterBillNo = billNo
                        jobCardForm.LoadJobCardEntry(billNo)
                        jobCardForm.lblJobcard.Text = "Job Card Alteration"
                        jobCardForm.SaveButton.Text = "Update"
                    End If
                    Exit For
                End If
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading job card entry: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        RefreshItemList()
    End Sub
    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelBttn.Click
        If Guna2DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row to cancel.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)
        ' --- NEW VALIDATION: Check WorkingStatus ---
        Dim statusObj As Object = selectedRow.Cells("WorkingStatus").Value
        Dim currentStatus As String = If(statusObj IsNot Nothing AndAlso Not IsDBNull(statusObj), statusObj.ToString(), "")

        If currentStatus.Trim().ToUpper() = "COMPLETED" Then
            MessageBox.Show("This job card is already COMPLETED and cannot be cancelled.", "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        Dim billNoObj As Object = selectedRow.Cells("Bill_No").Value
        Dim ManualBillNoObj As Object = selectedRow.Cells("Manual_BillNo").Value

        If billNoObj Is Nothing OrElse IsDBNull(billNoObj) Then
            MessageBox.Show("No Bill No found in the selected row.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If ManualBillNoObj Is Nothing OrElse IsDBNull(ManualBillNoObj) Then
            MessageBox.Show("No Bill No found in the selected row.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim result As DialogResult = MessageBox.Show("Are you sure you want to cancel this entry?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.No Then
            Exit Sub
        End If

        Dim billNo As String = billNoObj.ToString()
        Dim ManualBillNo As String = ManualBillNoObj.ToString()

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()
            Using transaction = sqlconnect.BeginTransaction()
                Try
                    Dim updateQuery As String = $"UPDATE jobcard_table SET Cancel = 1 WHERE Bill_No = @BillNo and Manual_BillNo = @ManualBillNo"
                    Using sqlcommand As New SqlCommand(updateQuery, sqlconnect, transaction)
                        sqlcommand.Parameters.AddWithValue("@BillNo", billNo)
                        sqlcommand.Parameters.AddWithValue("@ManualBillNo", ManualBillNo)
                        sqlcommand.ExecuteNonQuery()
                    End Using
                    Dim updateStatusQuery As String = $"UPDATE jobcard_table SET WorkingStatus = 'Cancel' WHERE Bill_No = @BillNo and Manual_BillNo = @ManualBillNo"
                    Using sqlcommand As New SqlCommand(updateStatusQuery, sqlconnect, transaction)
                        sqlcommand.Parameters.AddWithValue("@BillNo", billNo)
                        sqlcommand.Parameters.AddWithValue("@ManualBillNo", ManualBillNo)
                        sqlcommand.ExecuteNonQuery()
                    End Using

                    transaction.Commit()
                    MessageBox.Show("Entry successfully cancelled.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RefreshItemList()
                Catch ex As Exception
                    transaction.Rollback()
                    MessageBox.Show("An error occurred while cancelling the entry: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub

    Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
        If Guna2DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row to Update Finishing Note.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)
        Dim billNoObj As Object = selectedRow.Cells("Bill_No").Value
        Dim ManualBillNoObj As Object = selectedRow.Cells("Manual_BillNo").Value

        If billNoObj Is Nothing OrElse IsDBNull(billNoObj) Then
            MessageBox.Show("No Bill No found in the selected row.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If ManualBillNoObj Is Nothing OrElse IsDBNull(ManualBillNoObj) Then
            MessageBox.Show("No Manual Bill No found in the selected row.")
            Exit Sub
        End If

        Dim result As DialogResult = MessageBox.Show("Are you sure you want to update this Finishing Note?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then Exit Sub

        Dim billNo As String = billNoObj.ToString()
        Dim ManualBillNo As String = ManualBillNoObj.ToString()
        Dim finishingValue As Integer

        If String.IsNullOrWhiteSpace(UpdateTextBox.Text) Then
            MessageBox.Show("Please enter a value.")
            Exit Sub
        ElseIf Integer.TryParse(UpdateTextBox.Text, finishingValue) Then
        Else
            MessageBox.Show("Please enter a valid number.")
            Exit Sub
        End If

        Dim FinishDate As Date = DateTxt.Text

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()
            Using transaction = sqlconnect.BeginTransaction()
                Try
                    Dim currentFinishing As Integer = 0
                    Dim currentNote As Integer = 0
                    Dim cancel As Integer = 0

                    Dim selectQuery As String = "SELECT Finishing, Note, Cancel FROM jobcard_table 
                                                 WHERE Bill_No = @BillNo AND Manual_BillNo = @ManualBillNo"

                    Using selectCmd As New SqlCommand(selectQuery, sqlconnect, transaction)
                        selectCmd.Parameters.AddWithValue("@BillNo", billNo)
                        selectCmd.Parameters.AddWithValue("@ManualBillNo", ManualBillNo)

                        Using reader = selectCmd.ExecuteReader()
                            If reader.Read() Then
                                currentFinishing = Convert.ToInt32(reader("Finishing"))
                                currentNote = Convert.ToInt32(reader("Note"))
                                cancel = Convert.ToInt32(reader("Cancel"))
                            Else
                                MessageBox.Show("Record not found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                transaction.Rollback()
                                Return
                            End If
                        End Using
                    End Using

                    If cancel = 1 Then
                        MessageBox.Show("Job Card is already canceled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        transaction.Rollback()
                        Return
                    End If

                    Dim totalAfterUpdate As Integer = currentFinishing + finishingValue

                    Dim updateQuery As String = "UPDATE jobcard_table 
                                             SET Finishing = Finishing + @Finishing, Finish_Date = @FinishDate 
                                             WHERE Bill_No = @BillNo AND Manual_BillNo = @ManualBillNo"

                    Using updateCmd As New SqlCommand(updateQuery, sqlconnect, transaction)
                        updateCmd.Parameters.AddWithValue("@Finishing", finishingValue)
                        updateCmd.Parameters.AddWithValue("@FinishDate", FinishDate)
                        updateCmd.Parameters.AddWithValue("@BillNo", billNo)
                        updateCmd.Parameters.AddWithValue("@ManualBillNo", ManualBillNo)
                        updateCmd.ExecuteNonQuery()
                    End Using

                    If totalAfterUpdate >= currentNote Then
                        Dim updateStatusQuery As String = "UPDATE jobcard_table SET WorkingStatus = 'COMPLETED' WHERE Bill_No = @BillNo AND Manual_BillNo = @ManualBillNo"

                        Using statusCmd As New SqlCommand(updateStatusQuery, sqlconnect, transaction)
                            statusCmd.Parameters.AddWithValue("@BillNo", billNo)
                            statusCmd.Parameters.AddWithValue("@ManualBillNo", ManualBillNo)
                            statusCmd.ExecuteNonQuery()
                        End Using
                    End If

                    transaction.Commit()
                    MessageBox.Show("Finishing Note updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RefreshItemList()
                    UpdateTextBox.Clear()

                Catch ex As Exception
                    transaction.Rollback()
                    MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub
End Class

'Private Sub PrintDocument2_PrintPage(sender As Object, e As PrintPageEventArgs) Handles PrintDocument2.PrintPage
'Dim g As Graphics = e.Graphics
'Dim blackPen As New Pen(Color.Black, 1)
'Dim fontRegular As New Font("Arial", 10)
'Dim fontBold As New Font("Arial", 14, FontStyle.Bold)

'Dim marginLeft As Integer = 40
'Dim marginTop As Integer = 40
'Dim marginRight As Integer = 40
'Dim y As Integer = marginTop
'Dim lineHeight As Integer = 40
'Dim boxHeight As Integer = 40
'Dim paddingLeft As Integer = 10
'Dim paddingTop As Integer = 10

'Dim data = jobCardList.FirstOrDefault(Function(jc) jc.ContainsKey("ID") AndAlso jc("ID") = selectedJobCardId)

'If data Is Nothing Then
'    e.HasMorePages = False
'    Return
'End If

'Dim title As String = "THANGAM NOTE BOOKS, PERUNDURAI"
'Dim titleWidth As SizeF = g.MeasureString(title, fontBold)
'Dim titleX As Integer = (e.PageBounds.Width - titleWidth.Width) \ 2
'g.DrawString(title, fontBold, Brushes.Black, titleX, y)
'y += 30

'Dim labelBoxWidth As Integer = 150
'Dim valueBoxWidth As Integer = 150

'g.DrawString("JOBCARD NO:", fontRegular, Brushes.Black, marginLeft, y + 10)
'g.DrawRectangle(blackPen, marginLeft + 100, y - 2, labelBoxWidth, boxHeight)
'g.DrawString(data("Bill_No"), fontRegular, Brushes.Black, marginLeft + 100 + paddingLeft, y + paddingTop)

'Dim rightX As Integer = e.PageBounds.Width - marginRight - labelBoxWidth - valueBoxWidth
'g.DrawString("DATE:", fontRegular, Brushes.Black, rightX, y + 10)
'g.DrawRectangle(blackPen, rightX + 50, y - 2, valueBoxWidth, boxHeight)
'g.DrawString(data("JobCard_date"), fontRegular, Brushes.Black, rightX + 50 + paddingLeft, y + paddingTop)

'y += 60

'Dim contentWidth As Integer = e.PageBounds.Width - marginLeft - marginRight
'Dim labelWidth As Integer = contentWidth \ 2
'Dim valueWidth As Integer = contentWidth - labelWidth

'Dim fields As String() = {"SCHOOL / WRAPPER NAME", "NOTE PROCESSING METHOD", "PAPER SIZE / GSM", "NOTE SIZE", "NO OF SHEET", "NO OF PAGE", "NO OF NOTE", "NO OF REEM", "NO OF FINISHING NOTE"}

'Dim values As String() = {data("Partyname"), data("NoteProcessing"), data("Paper_Size_GSM"), data("NoteSize"),
'                          data("Sheet"), data("Pages"), data("Note"), data("Reem"), data("Finishing")}

'For i As Integer = 0 To fields.Length - 1
'    g.DrawRectangle(blackPen, marginLeft, y, labelWidth, boxHeight)
'    g.DrawString(fields(i), fontRegular, Brushes.Black, marginLeft + paddingLeft, y + paddingTop)

'    g.DrawRectangle(blackPen, marginLeft + labelWidth, y, valueWidth, boxHeight)
'    g.DrawString(values(i), fontRegular, Brushes.Black, marginLeft + labelWidth + paddingLeft, y + paddingTop)

'    y += lineHeight
'Next

'y += 40
'Dim footerSectionWidth As Integer = contentWidth \ 3
'g.DrawString("Prepared by", fontRegular, Brushes.Black, marginLeft, y)
'g.DrawString("Checked by", fontRegular, Brushes.Black, marginLeft + footerSectionWidth, y)
'g.DrawString("Managing Director", fontRegular, Brushes.Black, marginLeft + 2 * footerSectionWidth, y)

'e.HasMorePages = False
'End Sub


'Private Sub RefreshItemList()
'    jobCardList.Clear()
'    jobCardPrint.Clear()
'    Dim FromDate As Date
'    Dim ToDate As Date

'    If Not Date.TryParse(FromDateTextBox.Text, FromDate) OrElse Not Date.TryParse(ToDateTextBox.Text, ToDate) Then
'        MessageBox.Show("Please enter valid From and To dates.")
'        Exit Sub
'    End If
'    Dim query As String = "SELECT jc.Bill_No, FORMAT(jc.JobCard_date, 'dd/MM/yyyy') AS JobCard_date, 
'                            lt.Partyname AS Partyname, np.Name AS NoteProcessing_English, np.TamilName AS NoteProcessing_Tamil,ns.Name AS NoteSize_English, 
'                            ns.TamilName AS NoteSize_Tamil, jc.Paper_Size, jc.Note_Size,nt.Name AS NoteType, jc.Sheet, jc.Pages, jc.Note, jc.Reem, jc.Finishing, 
'                            jc.WorkingStatus, jc.Manual_BillNo, jc.Cancel, jc.ID, jc.Finish_Date,jc.Paper_Brand,jc.Paper_GSM,jc.Paper_Weight,jc.no_Index,Jc.Wrapper
'                            FROM JobCard_table jc
'                            LEFT JOIN Ledger_Table lt ON jc.ledger_id = lt.ID
'                            LEFT JOIN NoteProcessing_Table np ON jc.NoteProcessing_ID = np.ID
'                            LEFT JOIN NoteSize_Table ns ON jc.NoteSize_ID = ns.ID
'                            LEFT JOIN NoteType_Table nt ON jc.NoteType_Id = nt.ID
'                            WHERE jc.JobCard_date BETWEEN @FromDate AND @ToDate
'                            ORDER BY jc.Bill_No, jc.JobCard_date;"

'    Using sqlconnect As SqlConnection = Tools.GetConnection()
'        Dim command As New SqlCommand(query, sqlconnect)
'        command.Parameters.Add("@FromDate", SqlDbType.Date).Value = FromDate.Date
'        command.Parameters.Add("@ToDate", SqlDbType.Date).Value = ToDate.Date

'        Try
'            sqlconnect.Open()
'            Dim reader As SqlDataReader = command.ExecuteReader()
'            Dim rawTable As New DataTable()
'            rawTable.Load(reader)

'            Dim reportTable As New DataTable()
'            reportTable.Columns.Add("S.No", GetType(String))
'            reportTable.Columns.Add("Bill_No", GetType(String))
'            reportTable.Columns.Add("JobCard_date", GetType(String))
'            reportTable.Columns.Add("Manual_BillNo", GetType(String))
'            reportTable.Columns.Add("Partyname", GetType(String))
'            reportTable.Columns.Add("Note", GetType(String))
'            reportTable.Columns.Add("Finishing", GetType(String))
'            reportTable.Columns.Add("WorkingStatus", GetType(String))
'            reportTable.Columns.Add("Cancel", GetType(Boolean))
'            reportTable.Columns.Add("ID", GetType(String))
'            reportTable.Columns.Add("Finish_Date", GetType(String))

'            Dim serialNo As Integer = 1

'            For Each row As DataRow In rawTable.Rows
'                Dim cancelStatus As Boolean = If(IsDBNull(row("cancel")), False, Convert.ToBoolean(row("cancel")))

'                reportTable.Rows.Add(serialNo.ToString(),
'                             Convert.ToString(row("Bill_No")),
'                             Convert.ToString(row("JobCard_date")),
'                             Convert.ToString(row("Manual_BillNo")),
'                             Convert.ToString(row("Partyname")),
'                             Convert.ToString(row("Note")),
'                             Convert.ToString(row("Finishing")),
'                             Convert.ToString(row("WorkingStatus")),
'                             cancelStatus,
'                             Convert.ToString(row("ID")),
'                             Convert.ToString(row("Finish_Date")))

'                Dim jobCardData As New Dictionary(Of String, String) From {
'                {"Bill_No", Convert.ToString(row("Bill_No"))},
'                {"JobCard_date", Convert.ToString(row("JobCard_date"))},
'                {"Manual_BillNo", Convert.ToString(row("Manual_BillNo"))},
'                {"Partyname", Convert.ToString(row("Partyname"))},
'                {"NoteProcessing", $"{row("NoteProcessing_English")} / {row("NoteProcessing_Tamil")}"},
'                {"NoteSize", $"{row("NoteSize_English")} / {row("NoteSize_Tamil")}"},
'                {"Paper_Size", Convert.ToString(row("Paper_Size"))},
'                {"Note_Size", Convert.ToString(row("Note_Size"))},
'                {"Sheet", Convert.ToString(row("Sheet"))},
'                {"Pages", Convert.ToString(row("Pages"))},
'                {"Note", Convert.ToString(row("Note"))},
'                {"Reem", Convert.ToString(row("Reem"))},
'                {"Finishing", Convert.ToString(row("Finishing"))},
'                {"ID", Convert.ToString(row("ID"))},
'                {"NoteSize_Paper_Size", $"{row("NoteSize_English")} / {row("NoteSize_Tamil")} / {row("Paper_Size")}"},
'                {"Pages_Note_Reem", $"{row("Pages")} / {row("Note")} / {row("Reem")}"},
'                {"Finish", $"{row("Finishing")} / {CDate(row("Finish_Date")).ToString("dd/MM/yyyy")}"}
'            }
'                jobCardList.Add(jobCardData)

'                Dim jobCardPData As New Dictionary(Of String, Object) From {
'                    {"ID", Convert.ToString(row("ID"))},
'                    {"Bill_No", Convert.ToString(row("Bill_No"))},
'                    {"JobCard_date", Convert.ToString(row("JobCard_date"))},
'                    {"Manual_BillNo", Convert.ToString(row("Manual_BillNo"))},
'                    {"Partyname", Convert.ToString(row("Partyname"))},
'                    {"NoteProcessing", $"{row("NoteProcessing_English")} / {row("NoteProcessing_Tamil")}"},
'                    {"NoteSize", $"{row("NoteSize_English")} / {row("NoteSize_Tamil")}"},
'                    {"Paper_Size", Convert.ToString(row("Paper_Size"))},
'                    {"Note_Size", Convert.ToString(row("Note_Size"))},
'                    {"Sheet", Convert.ToString(row("Sheet"))},
'                    {"Pages", Convert.ToString(row("Pages"))},
'                    {"Note", Convert.ToString(row("Note"))},
'                    {"Reem", Convert.ToString(row("Reem"))},
'                    {"Finishing", Convert.ToString(row("Finishing"))},
'                    {"NoteSize_Paper_Size", $"{row("NoteSize_English")} / {row("NoteSize_Tamil")} / {row("Paper_Size")}"},
'                    {"Pages_Note_Reem", $"{row("Pages")} / {row("Note")} / {row("Reem")}"},
'                    {"Finish", $"{row("Finishing")} / {CDate(row("Finish_Date")).ToString("dd/MM/yyyy")}"},
'                    {"PaperBoard_Size_GSM", $"{row("Paper_Brand")} / {row("Note_Size")} / {row("Paper_GSM")} / {row("Paper_Weight")}"},
'                    {"NoteType", If(row.Table.Columns.Contains("NoteType") AndAlso Not IsDBNull(row("NoteType")), row("NoteType").ToString(), "")},
'                    {"Index_Wrapper", $"{row("No_Index")} / {row("Wrapper")}"}
'                }


'                Dim combinedBill As String = $"{row("Manual_BillNo")}-{row("Bill_No")}"
'                Dim addonValues As New Dictionary(Of String, String)

'                Using conn As SqlConnection = Tools.GetConnection()
'                    conn.Open()
'                    Dim addonQuery As String = "SELECT Processing_Method_Name, Value_Name FROM Addons_Table WHERE JC_BillNO = @combinedBill"
'                    Using cmd As New SqlCommand(addonQuery, conn)
'                        cmd.Parameters.AddWithValue("@combinedBill", combinedBill)
'                        Using addonReader As SqlDataReader = cmd.ExecuteReader()
'                            While addonReader.Read()
'                                Dim methodName As String = addonReader("Processing_Method_Name").ToString()
'                                Dim valueName As String = addonReader("Value_Name").ToString()
'                                If Not addonValues.ContainsKey(methodName) Then
'                                    addonValues.Add(methodName, valueName)
'                                End If
'                            End While
'                        End Using
'                    End Using
'                End Using

'                jobCardPData("Addons") = addonValues
'                jobCardPrint.Add(jobCardPData)


'                Dim detailFields = New Dictionary(Of String, Object) From {
'                {"Note Processing", $"{row("NoteProcessing_English")} / {row("NoteProcessing_Tamil")}"},
'                {"Note Size", $"{row("NoteSize_English")} / {row("NoteSize_Tamil")}"},
'                {"Paper Size", row("Paper_Size")},
'                {"Note Size (Raw)", row("Note_Size")},
'                {"Sheet", row("Sheet")},
'                {"Pages", row("Pages")},
'                {"Note", row("Note")},
'                {"Reem", row("Reem")}
'            }
'                For Each kvp In detailFields
'                    reportTable.Rows.Add("", kvp.Key, If(kvp.Value Is DBNull.Value, "", kvp.Value.ToString()), "", "", "", "", "", cancelStatus, row("ID"))
'                Next

'                serialNo += 1
'            Next

'            Guna2DataGridView1.DataSource = reportTable

'            If Guna2DataGridView1.Columns.Contains("ID") Then
'                Guna2DataGridView1.Columns("ID").Visible = False
'            End If
'            If Guna2DataGridView1.Columns.Contains("Finish_Date") Then
'                Guna2DataGridView1.Columns("Finish_Date").Visible = False
'            End If

'            With Guna2DataGridView1
'                .Columns("Cancel").Visible = False

'                For Each row As DataGridViewRow In .Rows
'                    If Not row.IsNewRow Then
'                        Dim isCancelled As Boolean = False
'                        If Not IsDBNull(row.Cells("Cancel").Value) Then
'                            isCancelled = Convert.ToBoolean(row.Cells("Cancel").Value)
'                        End If

'                        If Not String.IsNullOrEmpty(Convert.ToString(row.Cells("S.No").Value)) Then
'                            If isCancelled Then
'                                row.Cells("Bill_No").Style.ForeColor = Color.Red
'                                row.Cells("WorkingStatus").Style.ForeColor = Color.Red
'                            Else
'                                row.DefaultCellStyle.BackColor = Color.White
'                            End If
'                        Else
'                            row.DefaultCellStyle.BackColor = Color.LightGray
'                        End If
'                    End If
'                Next

'                .Columns("S.No").Width = 50
'                .Columns("Bill_No").Width = 100
'                .Columns("JobCard_date").Width = 150
'                .Columns("Manual_BillNo").Width = 100
'                .Columns("Partyname").Width = 250
'                .Columns("Note").Width = 80
'                .Columns("Finishing").Width = 80
'                .Columns("WorkingStatus").Width = 120

'                .Columns("S.No").HeaderText = "S.No"
'                .Columns("Bill_No").HeaderText = "Bill No / Field"
'                .Columns("JobCard_date").HeaderText = "Date / Value"
'                .Columns("Manual_BillNo").HeaderText = "Manual BillNo"
'                .Columns("WorkingStatus").HeaderText = "Working Status"

'                .Columns("S.No").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
'                .Columns("Bill_No").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
'                .Columns("JobCard_date").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft

'                For Each colName In {"Partyname", "Note", "Finishing", "WorkingStatus"}
'                    .Columns(colName).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
'                Next

'                For Each col As DataGridViewColumn In .Columns
'                    col.SortMode = DataGridViewColumnSortMode.NotSortable
'                Next
'            End With

'        Catch ex As Exception
'            MessageBox.Show("An error occurred: " & ex.Message)
'        End Try
'    End Using
'End Sub

'Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs)
'    Dim g As Graphics = e.Graphics
'    Dim pen As New Pen(Color.Black, 1)
'    Dim font As New Font("Arial", 10)
'    Dim boldFont As New Font("Arial", 13, FontStyle.Bold)

'    Dim pageWidth = 840
'    Dim marginLeft = 20
'    Dim marginTop = 20
'    Dim y = marginTop
'    Dim boxHeight = 28
'    Dim paddingLeft = 4

'    Dim data = jobCardPrint.FirstOrDefault(Function(jc) jc("ID").ToString() = selectedJobCardId)
'    If data Is Nothing Then Exit Sub

'    Dim title = "THANGAM NOTE BOOKSS, PERUNDURAI"
'    Dim titleWidth = g.MeasureString(title, boldFont).Width
'    g.DrawString(title, boldFont, Brushes.Black, (pageWidth - titleWidth) / 2, y)
'    y += 40

'    g.DrawString("JOBCARD NO", font, Brushes.Black, marginLeft, y + 6)
'    g.DrawRectangle(pen, marginLeft + 105, y - 2, 120, boxHeight)
'    g.DrawString(data("Bill_No").ToString(), font, Brushes.Black, marginLeft + 109, y + 4)

'    g.DrawRectangle(pen, marginLeft + 225, y - 2, 80, boxHeight)
'    g.DrawString(data("Manual_BillNo").ToString(), font, Brushes.Black, marginLeft + 229, y + 4)

'    g.DrawString("DATE", font, Brushes.Black, pageWidth - 175, y + 6)
'    g.DrawRectangle(pen, pageWidth - 125, y - 2, 100, boxHeight)
'    g.DrawString(data("JobCard_date").ToString(), font, Brushes.Black, pageWidth - 110, y + 4)

'    y += boxHeight + 6

'    Dim fields = {"SCHOOL / WRAPPER NAME", "NOTE PROCESSING METHOD", "NOTE SIZE / NOTE TYPE", "NO OF SHEET / INDEX / WRAPPER",
'        "PAPER BRAND / SIZE / GSM / WEIGHT", "NO OF PAGE / NOTE / REEM", "NO OF FINISHING NOTE / DATE"}

'    Dim values = {data("Partyname"), data("NoteProcessing"), data("NoteSize_NoteType"), data("Sheet_Index_Wrapper"),
'        data("PaperBrand_Size_GSM_Weight"), data("Pages_Note_Reem"), data("Finish")}

'    For i = 0 To fields.Length - 1
'        g.DrawRectangle(pen, marginLeft, y, 265, boxHeight)
'        g.DrawString(fields(i), font, Brushes.Black, marginLeft + paddingLeft, y + 4)

'        g.DrawRectangle(pen, marginLeft + 265, y, 530, boxHeight)
'        g.DrawString(values(i).ToString(), font, Brushes.Black, marginLeft + 270, y + 4)

'        y += boxHeight
'    Next

'    If data.ContainsKey("Addons") Then
'        Dim addons = CType(data("Addons"), Dictionary(Of String, String))
'        For Each kvp In addons
'            g.DrawRectangle(pen, marginLeft, y, 265, boxHeight)
'            g.DrawString(kvp.Key, font, Brushes.Black, marginLeft + paddingLeft, y + 4)

'            g.DrawRectangle(pen, marginLeft + 265, y, 530, boxHeight)
'            g.DrawString(kvp.Value, font, Brushes.Black, marginLeft + 270, y + 4)

'            y += boxHeight
'        Next
'    End If

'    y += 40
'    Dim lstr = "Prepared by" & Space(60) & "Checked by" & Space(60) & "Managing Director"
'    titleWidth = g.MeasureString(lstr, font).Width
'    g.DrawString(lstr, font, Brushes.Black, (840 - titleWidth) / 2, y)
'    g.DrawString("Prepared by", font, Brushes.Black, marginLeft, y)
'    g.DrawString("Checked by", font, Brushes.Black, marginLeft + 200, y)
'    g.DrawString("Managing Director", font, Brushes.Black, marginLeft + 400, y)

'    e.HasMorePages = False
'End Sub



'Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs)
'    Dim g As Graphics = e.Graphics
'    Dim pen As New Pen(Color.Black, 1)
'    Dim font As New Font("Arial", 10)
'    Dim bfont As New Font("Arial", 10, FontStyle.Bold)
'    Dim boldFont As New Font("Arial", 13, FontStyle.Bold)

'    Dim pageWidth = 840
'    Dim marginLeft = 20
'    Dim marginTop = 20
'    Dim y = marginTop
'    Dim boxHeight = 28
'    Dim paddingLeft = 4

'    Dim data = jobCardPrint.FirstOrDefault(Function(jc) jc("ID").ToString() = selectedJobCardId)
'    If data Is Nothing Then Exit Sub

'    Dim title = "THANGAM NOTE BOOKSS, PERUNDURAI"
'    Dim titleWidth = g.MeasureString(title, boldFont).Width
'    g.DrawString(title, boldFont, Brushes.Black, (pageWidth - titleWidth) / 2, y)
'    y += 40

'    g.DrawString("JOBCARD NO", font, Brushes.Black, marginLeft, y + 6)
'    g.DrawRectangle(pen, marginLeft + 105, y - 2, 120, boxHeight)
'    g.DrawString(GetValue(data, "Bill_No"), font, Brushes.Black, marginLeft + 109, y + 4)

'    g.DrawRectangle(pen, marginLeft + 225, y - 2, 80, boxHeight)
'    g.DrawString(GetValue(data, "Manual_BillNo"), font, Brushes.Black, marginLeft + 229, y + 4)

'    g.DrawString("DATE", font, Brushes.Black, pageWidth - 175, y + 6)
'    g.DrawRectangle(pen, pageWidth - 125, y - 2, 100, boxHeight)
'    g.DrawString(GetValue(data, "JobCard_date"), font, Brushes.Black, pageWidth - 110, y + 4)

'    y += boxHeight + 6

'    Dim fields = {
'    "SCHOOL / WRAPPER NAME",
'    "NOTE PROCESSING METHOD",
'    "NOTE SIZE / NOTE TYPE"
'}

'    Dim values = {
'    GetValue(data, "Partyname"),
'    GetValue(data, "NoteProcessing"),
'    GetValue(data, "NoteSize_NoteType")
'}

'    For i = 0 To fields.Length - 1
'        g.DrawRectangle(pen, marginLeft, y, 265, boxHeight)
'        g.DrawString(fields(i), font, Brushes.Black, marginLeft + paddingLeft, y + 4)

'        g.DrawRectangle(pen, marginLeft + 265, y, 530, boxHeight)
'        g.DrawString(values(i), bfont, Brushes.Black, marginLeft + 270, y + 4)

'        y += boxHeight
'    Next

'    Dim finishDateStr As String = ""
'    Try
'        finishDateStr = CDate(GetValue(data, "Finish").ToString().Split("/"c).Last()).ToString("dd/MM/yyyy")
'    Catch ex As Exception
'        finishDateStr = ""
'    End Try

'    Dim fontKey As New Font("Arial", 10, FontStyle.Regular)
'    Dim fontVal As New Font("Arial", 10, FontStyle.Bold)

'    Dim group1 = New List(Of Tuple(Of String, String)) From {
'        Tuple.Create("SHEET :", GetValue(data, "Sheet")),
'        Tuple.Create("/ INDEX :", GetValue(data, "No_Index")),
'        Tuple.Create("/ WRAPPER :", GetValue(data, "Wrapper"))
'    }

'    Dim group2 = New List(Of Tuple(Of String, String)) From {
'        Tuple.Create("PAPER BRAND :", GetValue(data, "Paper_Brand").ToUpper()),
'        Tuple.Create("/ SIZE :", GetValue(data, "Paper_Size")),
'        Tuple.Create("/ GSM :", GetValue(data, "Paper_GSM")),
'        Tuple.Create("/ WEIGHT :", GetValue(data, "Paper_Weight"))
'    }

'    Dim group3 = New List(Of Tuple(Of String, String)) From {
'        Tuple.Create("NO OF PAGE :", GetValue(data, "Pages")),
'        Tuple.Create("/ NOTE :", GetValue(data, "Note")),
'        Tuple.Create("/ REEM :", GetValue(data, "Reem"))
'    }

'    Dim group4 = New List(Of Tuple(Of String, String)) From {
'        Tuple.Create("NO OF FINISHING NOTE :", GetValue(data, "Finishing")),
'        Tuple.Create("/ DATE :", GetValue(data, "Finish"))
'    }

'    Dim allGroups = New List(Of List(Of Tuple(Of String, String))) From {group1, group2, group3, group4}

'    Dim maxWidth As Integer = 795

'    For Each group In allGroups
'        g.DrawRectangle(pen, marginLeft, y, maxWidth, boxHeight)
'        Dim x As Integer = marginLeft + paddingLeft

'        For Each pair In group
'            Dim key = pair.Item1
'            Dim val = pair.Item2

'            g.DrawString(key, fontKey, Brushes.Black, x, y + 4)
'            x += CInt(g.MeasureString(key, fontKey).Width)

'            g.DrawString(val, fontVal, Brushes.Black, x, y + 4)
'            x += CInt(g.MeasureString(val & "  ", fontVal).Width)
'        Next

'        y += boxHeight
'    Next

'    If data.ContainsKey("Addons") Then
'        Dim addons = CType(data("Addons"), Dictionary(Of String, String))
'        If addons.Count > 0 Then
'            For Each kvp In addons
'                g.DrawRectangle(pen, marginLeft, y, 265, boxHeight)
'                g.DrawString(kvp.Key, font, Brushes.Black, marginLeft + paddingLeft, y + 4)

'                g.DrawRectangle(pen, marginLeft + 265, y, 530, boxHeight)
'                g.DrawString(kvp.Value, bfont, Brushes.Black, marginLeft + 270, y + 4)

'                y += boxHeight
'            Next
'        End If
'    End If

'    y += 40
'    Dim lstr = "Prepared by" & Space(60) & "Checked by" & Space(60) & "Managing Director"
'    titleWidth = g.MeasureString(lstr, font).Width
'    g.DrawString(lstr, font, Brushes.Black, (pageWidth - titleWidth) / 2, y)

'    e.HasMorePages = False
'End Sub

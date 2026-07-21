Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports Guna.UI2.WinForms

Public Class Printing
    Dim PrintDate As String
    Private listBoxSelectionMade As Boolean = False
    Private dtname, dtPMethod, dtPrinting, dtPMachine, dtPrintingItem As DataTable
    Private activeTextbox As Guna.UI2.WinForms.Guna2TextBox

    Private printFont As Font = New Font("Segoe UI", 9)
    Private headerFont As Font = New Font("Segoe UI", 10, FontStyle.Bold)
    Private titleFont As Font = New Font("Segoe UI", 12, FontStyle.Bold)
    Private currentRow As Integer = 0
    Private PrintDocument1 As New Drawing.Printing.PrintDocument
    Private jobCardList As New List(Of Dictionary(Of String, String))
    Private selectedJobCardId As String = ""
    Private dgvRowIndex As Integer = 0
    Private dtPrintData As DataTable

    Private FinStartDate As DateTime
    Private FinEndDate As DateTime

    Private Sub Jobcard_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        PartynameTxt.Focus()
    End Sub
    Private Sub Jobcard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Tools.LoadConfiguration()
        PrintDate = DateTime.Now.ToString("dd/MM/yyyy")
        DateTxt.Text = DateAndTime.Now.ToString("dd/MM/yyyy")
        PrintingDate.Text = PrintDate
        BillNoTxt.Text = GetNextBillNumber()
        BillNoTxt.ReadOnly = True
        InitializeDataGridView()
        LoadAutoCompleteData()

        Themeload()
        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me

        FromDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy")
        ToDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy")

        Status.Items.Clear()
        Status.Items.Add("PENDING")
        Status.Items.Add("COMPLETED")
        Status.StartIndex = 0
        RefreshItemList()
        LoadFinancialPeriod()
        AddHandler KryptonListBox.Click, AddressOf KryptonListBox_Click
        AddHandler KryptonListBox.KeyDown, AddressOf KryptonListBox_KeyDown
        AddHandler PrintDocument1.PrintPage, AddressOf PrintDocument1_PrintPage

        'Label12.Visible = False
        'Typetxt.Visible = False

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
    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        RefreshItemList()
    End Sub
    Private Sub RefreshItemList()
        Dim fromDate As Date
        Dim toDate As Date

        If Not Date.TryParseExact(FromDateTextBox.Text, "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, fromDate) OrElse
       Not Date.TryParseExact(ToDateTextBox.Text, "dd/MM/yyyy", Nothing, Globalization.DateTimeStyles.None, toDate) Then
            MessageBox.Show("Please enter valid From and To dates in dd/MM/yyyy format.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim query As String = "SELECT pt.Bill_No, pt.Printing_date, lt.Partyname,
                   pm.Name AS PMName, ptm.Name AS PTName,
                   mach.Name AS MachineName, pt.Paper_Size_GSM, pt.Printing_Colour, pt.Quantity, pt.Printing_Details,
                   pt.WorkingStatus, Finish_Date, Finish AS FinishQty, Paper_Brand, Paper_Weight,
                   PI.Name AS NTName
            FROM Printing_table pt
            LEFT JOIN Ledger_Table lt ON pt.ledger_id = lt.ID
            LEFT JOIN PrintingMethod_table pm ON pt.PrintMethod_Id = pm.ID
            LEFT JOIN PrintingType_table ptm ON pt.PrintingType_Id = ptm.ID
            LEFT JOIN PrintingMachine_table mach ON pt.PrintingMachine_Id = mach.ID
            LEFT JOIN PrintingItem_table PI ON pt.PrintingItem_Id = PI.ID
            WHERE pt.Cancel = 0 AND pt.Printing_date BETWEEN @FromDate AND @ToDate
            ORDER BY CAST(SUBSTRING(pt.Bill_No, CHARINDEX('/', pt.Bill_No) + 1,
                   CHARINDEX('/', pt.Bill_No, CHARINDEX('/', pt.Bill_No) + 1) 
                   - CHARINDEX('/', pt.Bill_No) - 1) AS INT) ASC, pt.Printing_date ASC;"


        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Using command As New SqlCommand(query, sqlconnect)
                command.Parameters.AddWithValue("@FromDate", fromDate)
                command.Parameters.AddWithValue("@ToDate", toDate)

                Try
                    sqlconnect.Open()
                    Dim reader As SqlDataReader = command.ExecuteReader()
                    Dim dt As New DataTable()
                    dt.Load(reader)

                    If dt.Rows.Count = 0 Then Exit Sub

                    Dim formatted As DataTable = FormatGroupedPrintingReport(dt)
                    Guna2DataGridView1.DataSource = formatted

                    With Guna2DataGridView1
                        If .Columns.Contains("Cancel") Then .Columns("Cancel").Visible = False

                        For Each row As DataGridViewRow In .Rows
                            If Not row.IsNewRow Then
                                Dim isCancelled As Boolean = False
                                If .Columns.Contains("Cancel") AndAlso Not IsDBNull(row.Cells("Cancel").Value) Then
                                    isCancelled = Convert.ToBoolean(row.Cells("Cancel").Value)
                                End If

                                Dim snoValue = row.Cells("SNo").Value
                                If Not IsDBNull(snoValue) AndAlso Not String.IsNullOrWhiteSpace(Convert.ToString(snoValue)) Then
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

                        .Columns("SNo").DisplayIndex = 0

                        SetColumnHeader("SNo", "S.No", 40)
                        SetColumnHeader("Bill_No", "Bill No", 120)
                        SetColumnHeader("Printing_date", "Date", 90)
                        SetColumnHeader("Partyname", "Party Name", 150)
                        SetColumnHeader("Quantity", "No.Of Quantity", 100)
                        SetColumnHeader("MachineName", "Machine Name", 120)
                        SetColumnHeader("FinishQty", "Finish Notes", 70)
                        SetColumnHeader("WorkingStatus", "Status", 90)
                        SetColumnHeader("Printing_Details", "Printing Details", 200)
                    End With

                Catch ex As Exception
                    MessageBox.Show("Error loading data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
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
        New DataColumn("WorkingStatus"),
        New DataColumn("Cancel", GetType(Boolean))
    })

        Dim grouped = From row In sourceTable.AsEnumerable()
                      Group row By
                      billNo = row.Field(Of String)("Bill_No"),
                      dateVal = row.Field(Of Date)("Printing_date").ToString("dd/MM/yyyy"),
                      party = row.Field(Of String)("Partyname"),
                      qty = row("Quantity").ToString(),
                      machine = row("MachineName").ToString(),
                      finish = If(sourceTable.Columns.Contains("FinishQty") AndAlso Not IsDBNull(row("FinishQty")), row("FinishQty").ToString(), ""),
                      status = row("WorkingStatus").ToString(),
                      cancel = If(sourceTable.Columns.Contains("Cancel") AndAlso Not IsDBNull(row("Cancel")), Convert.ToBoolean(row("Cancel")), False)
                  Into Group

        Dim sno As Integer = 1

        For Each g In grouped
            reportTable.Rows.Add(sno, g.billNo, g.dateVal, g.party, g.qty, g.machine, g.finish, g.status, g.cancel)

            For Each row In g.Group
                Dim gsm = If(row.Table.Columns.Contains("Paper_Size_GSM") AndAlso Not IsDBNull(row("Paper_Size_GSM")), "GSM : " & row("Paper_Size_GSM").ToString(), "GSM : ")
                Dim colour = If(row.Table.Columns.Contains("Printing_Colour") AndAlso Not IsDBNull(row("Printing_Colour")), "Colour : " & row("Printing_Colour").ToString(), "Colour : ")
                Dim method = If(row.Table.Columns.Contains("PMName") AndAlso Not IsDBNull(row("PMName")), "Method : " & row("PMName").ToString(), "Method : ")
                Dim type = If(row.Table.Columns.Contains("PTName") AndAlso Not IsDBNull(row("PTName")), "Type : " & row("PTName").ToString(), "Type : ")
                Dim brand = If(row.Table.Columns.Contains("Paper_Brand") AndAlso Not IsDBNull(row("Paper_Brand")), "Brand : " & row("Paper_Brand").ToString(), "Brand : ")
                Dim weight = If(row.Table.Columns.Contains("Paper_Weight") AndAlso Not IsDBNull(row("Paper_Weight")), "Weight: " & row("Paper_Weight").ToString(), "Weight: ")

                reportTable.Rows.Add(DBNull.Value, gsm, colour, method, type, brand, weight, "", g.cancel)
            Next

            For Each row In g.Group
                Dim details = If(row.Table.Columns.Contains("Printing_Details") AndAlso Not IsDBNull(row("Printing_Details")), row("Printing_Details").ToString(), "")
                reportTable.Rows.Add(DBNull.Value, "Printing Details", details, "", "", "", "", "", g.cancel)
            Next

            sno += 1
        Next

        Return reportTable
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
            .AllowUserToOrderColumns = True
            '.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 40, 49)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersHeight = 35
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
            .Margin = New Padding(20, 20, 20, 20)
            .MultiSelect = False
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .DefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Regular)
            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Regular)

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

    Private Function GetNextBillNumber() As String
        Dim query As String = "SELECT ISNULL(Vt_Prefix, '') + CAST(ISNULL(Vt_Billno, 0) AS VARCHAR) + ISNULL(Vt_Suffix, '') AS Vt_FullBillNo FROM v_table WHERE Vt_Name = 'Printing'"

        Using conn As SqlConnection = Tools.GetConnection()
            Using cmd As New SqlCommand(query, conn)
                Try
                    conn.Open()
                    Dim result As Object = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return result.ToString()
                    Else
                        Return String.Empty
                    End If
                Catch ex As Exception
                    MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return String.Empty
                End Try
            End Using
        End Using
    End Function
    Private Sub LoadAutoCompleteData()
            dtname = LoadDataTable("SELECT Partyname FROM Ledger_Table where Active = 0 order by Partyname Asc")
            dtPMethod = LoadDataTable("SELECT Name FROM PrintingMethod_table where Active = 0 order by Name Asc")
            dtPrinting = LoadDataTable("SELECT Name FROM PrintingType_table where Active = 0 order by Name Asc")
            dtPMachine = LoadDataTable("Select Name from PrintingMachine_table where Active = 0 order by Name Asc")
            dtPrintingItem = LoadDataTable("SELECT Name FROM PrintingItem_table where Active = 0 order by Name Asc")
            KryptonListBox.Visible = False
        End Sub

        Private Function LoadDataTable(query As String) As DataTable
            Using conn As SqlConnection = Tools.GetConnection()
                Using da As New SqlDataAdapter(query, conn)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    Return dt
                End Using
            End Using
        End Function

    Private Sub SharedTextBox_Events(sender As Object, e As EventArgs) Handles _
        PartynameTxt.TextChanged, PartynameTxt.Click, PartynameTxt.GotFocus,
        PMethodTxt.TextChanged, PMethodTxt.Click, PMethodTxt.GotFocus,
        PrintingTxt.TextChanged, PrintingTxt.Click, PrintingTxt.GotFocus,
        PMachineTxt.TextChanged, PMachineTxt.Click, PMachineTxt.GotFocus,
        PrintingItemtxt.TextChanged, PrintingItemtxt.Click, PrintingItemtxt.GotFocus

            Dim tb As Guna2TextBox = CType(sender, Guna2TextBox)
            ShowSuggestionsForTextBox(tb)
        End Sub

        Private Sub ShowSuggestionsForTextBox(textBox As Guna2TextBox)
            activeTextbox = textBox
        Dim source As DataTable = Nothing

        If textBox Is PartynameTxt Then
            source = dtname
        ElseIf textBox Is PMethodTxt Then
            source = dtPMethod
        ElseIf textBox Is PrintingTxt Then
            source = dtPrinting
        ElseIf textBox Is PMachineTxt Then
            source = dtPMachine
        ElseIf textBox Is PrintingItemtxt Then
            source = dtPrintingItem
        End If

        If source Is Nothing OrElse String.IsNullOrEmpty(textBox.Text) Then
                KryptonListBox.Visible = False
                Exit Sub
            End If

        Dim dv As New DataView(source)
            Dim columnName As String = source.Columns(0).ColumnName
            dv.RowFilter = $"[{columnName}] LIKE '{textBox.Text.Replace("'", "''")}%'"

            If dv.Count = 0 Then
                KryptonListBox.Visible = False
                Exit Sub
            End If

        KryptonListBox.DataSource = dv
            KryptonListBox.DisplayMember = columnName

        Dim screenPos = textBox.Parent.PointToScreen(textBox.Location)
            Dim clientPos = Me.PointToClient(screenPos)

            KryptonListBox.Location = New Point(clientPos.X, clientPos.Y + textBox.Height)
            KryptonListBox.Width = textBox.Width
            KryptonListBox.Visible = True
            KryptonListBox.BringToFront()

            If KryptonListBox.Items.Count > 0 Then KryptonListBox.SelectedIndex = 0
        End Sub

    Private Sub ValidateTyping(sender As Object, e As KeyPressEventArgs) Handles _
        PartynameTxt.KeyPress, PMethodTxt.KeyPress, PrintingTxt.KeyPress, PMachineTxt.KeyPress, PrintingItemtxt.KeyPress

            If Char.IsControl(e.KeyChar) Then Exit Sub
            Dim tb As Guna2TextBox = CType(sender, Guna2TextBox)
            Dim predictedText As String = tb.Text.Remove(tb.SelectionStart, tb.SelectionLength).Insert(tb.SelectionStart, e.KeyChar.ToString())

            Dim source As DataTable = GetSourceForTextBox(tb)
            If source IsNot Nothing Then
                Dim exists As Boolean = source.AsEnumerable().Any(Function(row) row.Field(Of String)(0).StartsWith(predictedText, StringComparison.OrdinalIgnoreCase))
            If Not exists Then e.Handled = True
        End If
        End Sub

    Private Function GetSourceForTextBox(tb As Guna2TextBox) As DataTable
        If tb Is PartynameTxt Then Return dtname
        If tb Is PMethodTxt Then Return dtPMethod
        If tb Is PrintingTxt Then Return dtPrinting
        If tb Is PMachineTxt Then Return dtPMachine
        If tb Is PrintingItemtxt Then Return dtPrintingItem
        Return Nothing
    End Function
    Private Sub SelectItemFromList()
            If KryptonListBox.Visible AndAlso KryptonListBox.SelectedItem IsNot Nothing AndAlso activeTextbox IsNot Nothing Then
                Dim rowView As DataRowView = DirectCast(KryptonListBox.SelectedItem, DataRowView)
                activeTextbox.Text = rowView(0).ToString()
                KryptonListBox.Visible = False
                activeTextbox.Focus()
                activeTextbox.SelectionStart = activeTextbox.Text.Length
            End If
        End Sub

        Private Sub KryptonListBox_Click(sender As Object, e As EventArgs) Handles KryptonListBox.Click
            SelectItemFromList()
        End Sub

        Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) Handles _
        PartynameTxt.KeyDown, PMethodTxt.KeyDown, PrintingTxt.KeyDown, PMachineTxt.KeyDown, PrintingItemtxt.KeyDown,
        Colourtxt.KeyDown, Brandtxt.KeyDown, Papertxt.KeyDown, Weighttxt.KeyDown, QtyTxt.KeyDown, Detailstxt.KeyDown, Savebtn.KeyDown

            Dim currentControl As Control = CType(sender, Control)

        If e.KeyCode = Keys.F2 Then
            If TypeOf currentControl Is Guna2TextBox AndAlso GetSourceForTextBox(CType(currentControl, Guna2TextBox)) IsNot Nothing Then
                e.Handled = True
                ShowAllSuggestions(CType(currentControl, Guna2TextBox))
                Exit Sub
            End If
        End If

        If KryptonListBox.Visible AndAlso (currentControl Is PartynameTxt Or currentControl Is PMethodTxt Or currentControl Is PrintingTxt Or currentControl Is PMachineTxt Or currentControl Is PrintingItemtxt) Then
                If e.KeyCode = Keys.Down Then
                    KryptonListBox.Focus()
                    If KryptonListBox.Items.Count > 0 Then KryptonListBox.SelectedIndex = 0
                    e.Handled = True
                    Exit Sub
                ElseIf e.KeyCode = Keys.Enter Then
                    SelectItemFromList()
                    e.Handled = True : e.SuppressKeyPress = True
                    Exit Sub
                End If
            End If

        If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                Me.SelectNextControl(currentControl, True, True, True, True)
            End If
        End Sub

    Private Sub ShowAllSuggestions(tb As Guna2TextBox)
        activeTextbox = tb
        Dim source As DataTable = GetSourceForTextBox(tb)

        If source Is Nothing OrElse source.Rows.Count = 0 Then
            KryptonListBox.Visible = False
            Exit Sub
        End If

        Dim dv As New DataView(source)
        KryptonListBox.DataSource = dv
        KryptonListBox.DisplayMember = source.Columns(0).ColumnName

        Dim screenPos = tb.Parent.PointToScreen(tb.Location)
        Dim clientPos = Me.PointToClient(screenPos)
        KryptonListBox.Location = New Point(clientPos.X, clientPos.Y + tb.Height)

        KryptonListBox.Width = tb.Width
        KryptonListBox.Visible = True
        KryptonListBox.BringToFront()

        If KryptonListBox.Items.Count > 0 Then
            KryptonListBox.Focus()
            KryptonListBox.SelectedIndex = 0
        End If
    End Sub

    Private Sub KryptonListBox_KeyDown(sender As Object, e As KeyEventArgs) Handles KryptonListBox.KeyDown
            If e.KeyCode = Keys.Enter Then
                SelectItemFromList()
            ElseIf e.KeyCode = Keys.Escape Then
                KryptonListBox.Visible = False
                activeTextbox?.Focus()
            End If
        End Sub

    Private Sub TextBox_LostFocus(sender As Object, e As EventArgs) Handles _
        PartynameTxt.LostFocus, PMethodTxt.LostFocus, PrintingTxt.LostFocus, PMachineTxt.LostFocus, PrintingItemtxt.LostFocus
            TimerFocusDelay.Start()
        End Sub

        Private Sub TimerFocusDelay_Tick(sender As Object, e As EventArgs) Handles TimerFocusDelay.Tick
            TimerFocusDelay.Stop()
            If Not KryptonListBox.Focused Then
                KryptonListBox.Visible = False

            If activeTextbox IsNot Nothing Then
                    Dim src = GetSourceForTextBox(activeTextbox)
                    If src IsNot Nothing AndAlso Not IsExactMatch(activeTextbox.Text, src) Then
                        activeTextbox.Clear()
                    End If
                End If
            End If
        End Sub

        Private Function IsExactMatch(text As String, source As DataTable) As Boolean
            If String.IsNullOrWhiteSpace(text) Then Return True
            Return source.AsEnumerable().Any(Function(row) row.Field(Of String)(0).Equals(text, StringComparison.OrdinalIgnoreCase))
        End Function

        Private Function GetPMIDByName(searchName As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM PrintingMethod_table WHERE Name = @Name"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Name", searchName)
                    sqlconnect.Open()
                    Dim result = sqlcommand.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return Convert.ToInt32(result)
                    Else
                        Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error getting Printing Method ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        End Try
    End Function

    Private Function GetPTIDByName(searchName As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM PrintingType_table WHERE Name = @Name"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Name", searchName)
                    sqlconnect.Open()
                    Dim result = sqlcommand.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return Convert.ToInt32(result)
                    Else
                        Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error getting Printing Type ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        End Try
    End Function

    Private Function GetPMEIDByName(searchName As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM PrintingMachine_table WHERE Name = @Name"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Name", searchName)
                    sqlconnect.Open()
                    Dim result = sqlcommand.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return Convert.ToInt32(result)
                    Else
                        Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error getting Printing Machine ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        End Try
    End Function

    Private Function GetSelectedPrintingItemId(PrintingItemName As String) As Integer
        If String.IsNullOrWhiteSpace(PrintingItemName) Then
            Throw New Exception("PrintingItemName is empty or null.")
        End If

        PrintingItemName = PrintingItemName.Trim()

        Dim query As String = "SELECT ID FROM PrintingItem_table WHERE Name = @PrintingItemName"
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Using sqlcommand As New SqlCommand(query, sqlconnect)
                sqlcommand.Parameters.AddWithValue("@PrintingItemName", PrintingItemName)
                sqlconnect.Open()
                Dim result = sqlcommand.ExecuteScalar()
                If result IsNot Nothing Then
                    Return Convert.ToInt32(result)
                Else
                    Throw New Exception("Selected Note Type does not exist in the PrintingItem_table.")
                End If
            End Using
        End Using
    End Function

    Private Function GetSelectedLedgerId() As Integer

        Dim query As String = "SELECT ID FROM Ledger_Table WHERE Partyname = @Partyname and Active = 0"

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Using sqlcommand As New SqlCommand(query, sqlconnect)
                sqlcommand.Parameters.AddWithValue("@Partyname", PartynameTxt.Text)
                sqlconnect.Open()
                Dim result = sqlcommand.ExecuteScalar()
                If result IsNot Nothing Then
                    Return Convert.ToInt32(result)
                Else
                    Throw New Exception("Selected party does not exist in the Ledger_Table.")
                End If
            End Using
        End Using
    End Function
    Private Sub LoadFinancialPeriod()
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim sql As String = "SELECT Ctl_Desc, Ctl_Value FROM Control_Table WHERE Ctl_Desc IN ('fromDate', 'toDate')"
                Using cmd As New SqlCommand(sql, conn)
                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        Dim foundAny As Boolean = False
                        While rdr.Read()
                            foundAny = True
                            Dim desc As String = rdr("Ctl_Desc").ToString()
                            Dim val As String = rdr("Ctl_Value").ToString()

                            If desc = "fromDate" Then
                                If Not DateTime.TryParse(val, FinStartDate) Then
                                    MessageBox.Show("Could not parse fromDate: " & val)
                                End If
                            End If

                            If desc = "toDate" Then
                                If Not DateTime.TryParse(val, FinEndDate) Then
                                    MessageBox.Show("Could not parse toDate: " & val)
                                End If
                            End If
                        End While

                        If Not foundAny Then
                            MessageBox.Show("No financial dates found in Control_Table!")
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Database Error in LoadFinancialPeriod: " & ex.Message)
        End Try
    End Sub

    Private Sub Savebtn_Click(sender As Object, e As EventArgs) Handles Savebtn.Click
        Dim Partyname As String = PartynameTxt.Text.Trim()
        Dim PMethod As String = PMethodTxt.Text.Trim()
        Dim PaperSize As String = Papertxt.Text.Trim()
        Dim Printing As String = PrintingTxt.Text.Trim()
        Dim PMachine As String = PMachineTxt.Text.Trim()
        Dim Colour As String = Colourtxt.Text.Trim()
        Dim Quantity As String = QtyTxt.Text.Trim()
        Dim Details As String = Detailstxt.Text.Trim()
        Dim StatusVal As String = Status.Text.Trim()
        Dim PType As String = PrintingItemtxt.Text.Trim()
        Dim PrintingDateVal As Date = DateTime.Parse(PrintingDate.Text.Trim())
        Dim Pbrand As String = Brandtxt.Text.Trim()
        Dim Pweight As String = Weighttxt.Text.Trim()
        Dim billNo As String = BillNoTxt.Text.Trim()
        Dim Finish As Integer = 0

        Dim parsedDate As DateTime

        Try
            parsedDate = DateTime.ParseExact(PrintingDate.Text, "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture)
        Catch ex As Exception
            MessageBox.Show("Invalid date format. Please use dd/MM/yyyy.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End Try

        If FinStartDate = DateTime.MinValue Or FinEndDate = DateTime.MinValue Then
            MessageBox.Show("Financial period dates are not loaded. Please check your Control_Table.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        If parsedDate.Date < FinStartDate.Date OrElse parsedDate.Date > FinEndDate.Date Then
            MessageBox.Show($"Date {parsedDate:dd/MM/yyyy} is outside the allowed financial period ({FinStartDate:dd/MM/yyyy} to {FinEndDate:dd/MM/yyyy}).", "Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrWhiteSpace(Partyname) OrElse
       String.IsNullOrWhiteSpace(PMethod) OrElse
       String.IsNullOrWhiteSpace(PType) OrElse
       String.IsNullOrWhiteSpace(Printing) OrElse
       String.IsNullOrWhiteSpace(PMachine) Then

            MessageBox.Show("All required fields must be filled.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Status.SelectedIndex = -1 Then
            MessageBox.Show("Please select a valid status.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim PartynameId As Integer = GetSelectedLedgerId()
        Dim PMethodId As Integer = GetPMIDByName(PMethod)
        Dim PrintingId As Integer = GetPTIDByName(Printing)
        Dim PMachineId As Integer = GetPMEIDByName(PMachine)
        Dim PTypeId As Integer = GetSelectedPrintingItemId(PType)
        Dim UserId As Integer = Tools.GetStoredUsername()

        If PartynameId = 0 OrElse PMethodId = 0 OrElse PrintingId = 0 OrElse PMachineId = 0 OrElse PTypeId = 0 Then
            MessageBox.Show("One or more related values are missing from lookup tables.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim result As DialogResult = MessageBox.Show("Are you sure you want to " & If(Savebtn.Text = "Update", "update", "save") & " this printing entry?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then Return

        Using conn As SqlConnection = Tools.GetConnection()
            Try
                conn.Open()

                If Savebtn.Text = "Update" Then
                    Dim updateCmd As New SqlCommand("UPDATE Printing_table SET Printing_date = @Date,ledger_id = @LedgerID,Partyname = @Partyname,PrintMethod_Id = @PrintMethodID,
                                                     Paper_Size_GSM = @PaperSize,PrintingType_Id = @PrintingTypeID,PrintingMachine_Id = @MachineID,Printing_Colour = @Colour,Quantity = @Qty,
                                                     Printing_Details = @Details,WorkingStatus = @StatusVal,PrintingItem_Id = @PTypeId,Paper_Brand = @Pbrand,Paper_Weight = @Pweight,
                                                     Finish = @Finish,  UserID = @UserID
                                                     WHERE Bill_No = @BillNo", conn)

                    updateCmd.Parameters.AddWithValue("@BillNo", billNo)
                    updateCmd.Parameters.AddWithValue("@Date", PrintingDateVal)
                    updateCmd.Parameters.AddWithValue("@LedgerID", PartynameId)
                    updateCmd.Parameters.AddWithValue("@Partyname", Partyname)
                    updateCmd.Parameters.AddWithValue("@PrintMethodID", PMethodId)
                    updateCmd.Parameters.AddWithValue("@PaperSize", PaperSize)
                    updateCmd.Parameters.AddWithValue("@PrintingTypeID", PrintingId)
                    updateCmd.Parameters.AddWithValue("@MachineID", PMachineId)
                    updateCmd.Parameters.AddWithValue("@Colour", Colour)
                    updateCmd.Parameters.AddWithValue("@Qty", If(String.IsNullOrWhiteSpace(Quantity), 0, CInt(Quantity)))
                    updateCmd.Parameters.AddWithValue("@Details", Details)
                    updateCmd.Parameters.AddWithValue("@StatusVal", StatusVal)
                    updateCmd.Parameters.AddWithValue("@PTypeId", PTypeId)
                    updateCmd.Parameters.AddWithValue("@Pbrand", Pbrand)
                    updateCmd.Parameters.AddWithValue("@Pweight", Pweight)
                    updateCmd.Parameters.AddWithValue("@Finish", Finish)
                    updateCmd.Parameters.AddWithValue("@UserID", UserId)

                    updateCmd.ExecuteNonQuery()
                    MessageBox.Show("Printing record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Dim insertCommand As New SqlCommand("INSERT INTO Printing_table (
                    Bill_No, Printing_date, ledger_id, Partyname, PrintMethod_Id,
                    Paper_Size_GSM, PrintingType_Id, PrintingMachine_Id,
                    Printing_Colour, Quantity, Printing_Details, Cancel, WorkingStatus, PrintingItem_Id, Paper_Brand, Paper_Weight, Finish, UserID) 
                    VALUES (
                    @BillNo, @Date, @LedgerID, @Partyname, @PrintMethodID,
                    @PaperSize, @PrintingTypeID, @MachineID,
                    @Colour, @Qty, @Details, 0, @StatusVal, @PTypeId, @Pbrand, @Pweight, @Finish, @UserID)", conn)

                    Dim newBillNo As String = GetNextBillNumber()
                    insertCommand.Parameters.AddWithValue("@BillNo", newBillNo)
                    insertCommand.Parameters.AddWithValue("@Date", PrintingDateVal)
                    insertCommand.Parameters.AddWithValue("@LedgerID", PartynameId)
                    insertCommand.Parameters.AddWithValue("@Partyname", Partyname)
                    insertCommand.Parameters.AddWithValue("@PrintMethodID", PMethodId)
                    insertCommand.Parameters.AddWithValue("@PaperSize", PaperSize)
                    insertCommand.Parameters.AddWithValue("@PrintingTypeID", PrintingId)
                    insertCommand.Parameters.AddWithValue("@MachineID", PMachineId)
                    insertCommand.Parameters.AddWithValue("@Colour", Colour)
                    insertCommand.Parameters.AddWithValue("@Qty", If(String.IsNullOrWhiteSpace(Quantity), 0, CInt(Quantity)))
                    insertCommand.Parameters.AddWithValue("@Details", Details)
                    insertCommand.Parameters.AddWithValue("@StatusVal", StatusVal)
                    insertCommand.Parameters.AddWithValue("@PTypeId", PTypeId)
                    insertCommand.Parameters.AddWithValue("@Pbrand", Pbrand)
                    insertCommand.Parameters.AddWithValue("@Pweight", Pweight)
                    insertCommand.Parameters.AddWithValue("@Finish", Finish)
                    insertCommand.Parameters.AddWithValue("@UserID", UserId)

                    insertCommand.ExecuteNonQuery()

                    Using updateVtBillNo As New SqlCommand("UPDATE v_table SET Vt_Billno = Vt_Billno + 1 WHERE Vt_Name = 'Printing'", conn)
                        updateVtBillNo.ExecuteNonQuery()
                    End Using

                    MessageBox.Show("Printing record saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                ClearPrintingFields()
                Savebtn.Text = "Save"
                RefreshItemList()
                BillNoTxt.Text = GetNextBillNumber()

            Catch ex As Exception
                MessageBox.Show("Error saving/updating printing data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub
    Private Sub ClearPrintingFields()
        PartynameTxt.Clear()
        PMethodTxt.Clear()
        Papertxt.Clear()
        PrintingTxt.Clear()
        PMachineTxt.Clear()
        Colourtxt.Clear()
        QtyTxt.Clear()
        Detailstxt.Clear()
        Status.SelectedIndex = 0
        Brandtxt.Clear()
        Weighttxt.Clear()
        PrintingItemtxt.Clear()
    End Sub

    Private Sub CompletedButton_Click(sender As Object, e As EventArgs) Handles CompletedButton.Click
        If Guna2DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row to Update Finishing Note.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)
        Dim billNoObj As Object = selectedRow.Cells("Bill_No").Value

        If billNoObj Is Nothing OrElse IsDBNull(billNoObj) Then
            MessageBox.Show("No Bill No found in the selected row.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Validate finishing value input
        Dim finishingValue As Integer
        If Not Integer.TryParse(UpdateTextBox.Text.Trim(), finishingValue) OrElse finishingValue <= 0 Then
            MessageBox.Show("Please enter a valid positive number for Finishing Value.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ' Validate FinishDate
        Dim FinishDate As Date
        If Not Date.TryParse(DateTxt.Text.Trim(), FinishDate) Then
            MessageBox.Show("Please enter a valid Finish Date.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim result As DialogResult = MessageBox.Show("Are you sure you want to update this Finishing Note?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.No Then Exit Sub

        Dim billNo As String = billNoObj.ToString()

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()
            Using transaction = sqlconnect.BeginTransaction()
                Try
                    Dim currentFinishing As Integer = 0
                    Dim currentNote As Integer = 0
                    Dim cancel As Integer = 0

                    Dim selectQuery As String = "SELECT Finish, Quantity, Cancel FROM Printing_table WHERE Bill_No = @BillNo"

                    Using selectCmd As New SqlCommand(selectQuery, sqlconnect, transaction)
                        selectCmd.Parameters.AddWithValue("@BillNo", billNo)
                        Using reader = selectCmd.ExecuteReader()
                            If reader.Read() Then
                                currentFinishing = If(IsDBNull(reader("Finish")), 0, Convert.ToInt32(reader("Finish")))
                                currentNote = If(IsDBNull(reader("Quantity")), 0, Convert.ToInt32(reader("Quantity")))
                                cancel = If(IsDBNull(reader("Cancel")), 0, Convert.ToInt32(reader("Cancel")))
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

                    'If totalAfterUpdate > currentNote Then
                    '    MessageBox.Show($"Update exceeds the total Quantity ({currentNote}). Please enter a valid finishing amount.")
                    '    transaction.Rollback()
                    '    Return
                    'End If

                    Dim updateQuery As String = "UPDATE Printing_table SET Finish = Finish + @Finish, Finish_Date = @FinishDate WHERE Bill_No = @BillNo"

                    Using updateCmd As New SqlCommand(updateQuery, sqlconnect, transaction)
                        updateCmd.Parameters.AddWithValue("@Finish", finishingValue)
                        updateCmd.Parameters.AddWithValue("@FinishDate", FinishDate)
                        updateCmd.Parameters.AddWithValue("@BillNo", billNo)
                        updateCmd.ExecuteNonQuery()
                    End Using

                    If totalAfterUpdate = currentNote Then
                        Dim updateStatusQuery As String = "UPDATE Printing_table SET WorkingStatus = 'COMPLETED' WHERE Bill_No = @BillNo"
                        Using statusCmd As New SqlCommand(updateStatusQuery, sqlconnect, transaction)
                            statusCmd.Parameters.AddWithValue("@BillNo", billNo)
                            statusCmd.ExecuteNonQuery()
                        End Using
                    End If

                    transaction.Commit()
                    MessageBox.Show("Finishing Note updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RefreshItemList()
                    UpdateTextBox.Clear()
                    PartynameTxt.Focus()
                Catch ex As Exception
                    transaction.Rollback()
                    MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub

    'Private Sub ProgressButton_Click(sender As Object, e As EventArgs) Handles ProgressButton.Click
    '    If Guna2DataGridView1.SelectedRows.Count = 0 Then
    '        MessageBox.Show("Please select a row to In Progress.")
    '        Exit Sub
    '    End If

    '    Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)
    '    Dim billNoObj As Object = selectedRow.Cells("Bill_No").Value

    '    If billNoObj Is Nothing OrElse IsDBNull(billNoObj) Then
    '        MessageBox.Show("No Bill No found in the selected row.")
    '        Exit Sub
    '    End If
    '    Dim result As DialogResult = MessageBox.Show("Are you sure you want to Progress this entry?", "Confirm Progress", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

    '    If result = DialogResult.No Then
    '        Exit Sub
    '    End If

    '    Dim billNo As String = billNoObj.ToString()

    '    Using sqlconnect As SqlConnection = Tools.GetConnection()
    '        sqlconnect.Open()

    '        Using transaction = sqlconnect.BeginTransaction()
    '            Dim cancel As Integer
    '            Dim cancelQuery As String = "SELECT Cancel FROM Printing_table WHERE Bill_No = @BillNo"

    '            Using selectCmd As New SqlCommand(cancelQuery, sqlconnect, transaction)
    '                selectCmd.Parameters.AddWithValue("@BillNo", billNo)

    '                Using reader = selectCmd.ExecuteReader()
    '                    If reader.Read() Then
    '                        cancel = Convert.ToInt32(reader("Cancel"))
    '                    Else
    '                        MessageBox.Show("Record not found.")
    '                        transaction.Rollback()
    '                        Return
    '                    End If
    '                End Using
    '            End Using

    '            If cancel = 1 Then
    '                MessageBox.Show("Printing already Canceled.")
    '                transaction.Rollback()
    '                Return
    '            End If

    '            Try
    '                Dim updateQuery As String = $"UPDATE Printing_table SET WorkingStatus = 'In Progress' WHERE Bill_No = @BillNo"
    '                Using sqlcommand As New SqlCommand(updateQuery, sqlconnect, transaction)
    '                    sqlcommand.Parameters.AddWithValue("@BillNo", billNo)
    '                    sqlcommand.ExecuteNonQuery()
    '                End Using

    '                transaction.Commit()
    '                MessageBox.Show("Entry successfully In Progress.")
    '                RefreshItemList()
    '            Catch ex As Exception
    '                transaction.Rollback()
    '                MessageBox.Show("An error occurred while In Progressing the entry: " & ex.Message)
    '            End Try
    '        End Using
    '    End Using
    'End Sub
    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles PCancelBtn.Click
        If Guna2DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row to cancel.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)
        Dim billNoObj As Object = selectedRow.Cells("Bill_No").Value
        Dim statusObj As Object = selectedRow.Cells("WorkingStatus").Value
        Dim currentStatus As String = If(statusObj IsNot Nothing AndAlso Not IsDBNull(statusObj), statusObj.ToString(), "")

        If billNoObj Is Nothing OrElse IsDBNull(billNoObj) OrElse String.IsNullOrWhiteSpace(billNoObj.ToString()) Then
            MessageBox.Show("No valid Bill No found in the selected row.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If
        If currentStatus.Trim().ToUpper() = "COMPLETED" Then
            MessageBox.Show("This Printing is already COMPLETED and cannot be cancelled.", "Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Exit Sub
        End If

        Dim result As DialogResult = MessageBox.Show("Are you sure you want to cancel this printing entry?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.No Then Exit Sub

        Dim billNo As String = billNoObj.ToString()

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()
            Using transaction = sqlconnect.BeginTransaction()
                Try
                    Dim cancelQuery As String = "UPDATE Printing_table SET Cancel = 1 WHERE Bill_No = @BillNo"
                    Using cmdCancel As New SqlCommand(cancelQuery, sqlconnect, transaction)
                        cmdCancel.Parameters.AddWithValue("@BillNo", billNo)
                        Dim cancelRows = cmdCancel.ExecuteNonQuery()

                        If cancelRows = 0 Then
                            transaction.Rollback()
                            MessageBox.Show("No matching record found to cancel.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Exit Sub
                        End If
                    End Using

                    Dim updateStatusQuery As String = "UPDATE Printing_table SET WorkingStatus = 'Cancel' WHERE Bill_No = @BillNo"
                    Using cmdStatus As New SqlCommand(updateStatusQuery, sqlconnect, transaction)
                        cmdStatus.Parameters.AddWithValue("@BillNo", billNo)
                        cmdStatus.ExecuteNonQuery()
                    End Using

                    transaction.Commit()
                    MessageBox.Show("Printing entry successfully cancelled and marked as Completed.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    RefreshItemList()

                Catch ex As Exception
                    transaction.Rollback()
                    MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub

    Private Sub PrintButton_Click(sender As Object, e As EventArgs) Handles PrintButton.Click
        If Guna2DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row to print.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim a4Size As New PaperSize("A5", 583, 500)
        PrintDocument1.DefaultPageSettings.PaperSize = a4Size
        dgvRowIndex = Guna2DataGridView1.SelectedRows(0).Index

        Dim printDialog As New PrintDialog()
        printDialog.Document = PrintDocument1

        If printDialog.ShowDialog() = DialogResult.OK Then
            PrintDocument1.Print()
        End If
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        Dim pen As New Pen(Color.Black, 1)
        Dim font As New Font("Arial", 10)
        Dim boldFont As New Font("Arial", 13, FontStyle.Bold)
        Dim bfont As New Font("Arial", 10, FontStyle.Bold)

        'Dim pageWidth = e.PageBounds.Width
        Dim marginLeft = 20
        Dim marginTop = 20
        Dim y = marginTop
        Dim boxHeight = 28
        Dim paddingLeft = 5

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

        If dgvRowIndex < 0 OrElse dgvRowIndex >= Guna2DataGridView1.Rows.Count Then
            e.HasMorePages = False
            Return
        End If

        Dim selectedRow As DataGridViewRow = Guna2DataGridView1.Rows(dgvRowIndex)
        Dim billNo As String = selectedRow.Cells("Bill_No").Value?.ToString()
        If String.IsNullOrEmpty(billNo) Then
            e.HasMorePages = False
            Return
        End If

        Dim data As New Dictionary(Of String, String)

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Dim query As String = "SELECT TOP 1 Printing_table.Bill_No, Printing_date, Ledger_Table.Partyname, 
                                    PrintingMethod_table.Name AS PMName, PrintingType_table.Name AS PTName,
                                    PrintingMachine_table.Name AS MachineName, Paper_Size_GSM, Printing_Colour, 
                                    Quantity, Printing_Details,WorkingStatus, FORMAT(Finish_Date, 'dd/MM/yyyy') AS Finish_Date, Finish AS FinishQty, Paper_Brand, Paper_Weight,
                                    NT.Name AS NTName
                                    FROM Printing_table
                                    LEFT JOIN Ledger_Table ON Printing_table.ledger_id = Ledger_Table.ID
                                    LEFT JOIN PrintingMethod_table ON Printing_table.PrintMethod_Id = PrintingMethod_table.ID
                                    LEFT JOIN PrintingType_table ON Printing_table.PrintingType_Id = PrintingType_table.ID
                                    LEFT JOIN PrintingMachine_table ON Printing_table.PrintingMachine_Id = PrintingMachine_table.ID
                                    LEFT JOIN PrintingItem_table NT ON Printing_table.PrintingItem_Id = NT.ID
                                    WHERE Bill_No = @BillNo"

            Using cmd As New SqlCommand(query, sqlconnect)
                cmd.Parameters.AddWithValue("@BillNo", billNo)
                sqlconnect.Open()
                Using reader = cmd.ExecuteReader()
                    If reader.Read() Then
                        data("Bill_No") = reader("Bill_No").ToString()
                        data("Printing_date") = Convert.ToDateTime(reader("Printing_date")).ToString("dd/MM/yyyy")
                        data("Partyname") = reader("Partyname").ToString()
                        data("PMName") = reader("PMName").ToString()
                        data("PTName") = reader("PTName").ToString()
                        data("MachineName") = reader("MachineName").ToString()
                        data("Paper_Size_GSM") = reader("Paper_Size_GSM").ToString()
                        data("Printing_Colour") = reader("Printing_Colour").ToString()
                        data("Quantity") = reader("Quantity").ToString()
                        data("Printing_Details") = reader("Printing_Details").ToString()
                        data("NTName") = reader("NTName").ToString()
                        data("Paper_Brand") = reader("Paper_Brand").ToString()
                        data("Paper_Weight") = reader("Paper_Weight").ToString()
                        data("Finish_Date") = reader("Finish_Date").ToString()
                        data("FinishQty") = reader("FinishQty").ToString()
                    End If
                End Using
            End Using
        End Using

        If data("Finish_Date") = "01/01/1999" Then
            data("Finish_Date") = ""
        End If

        If data.Count = 0 Then
            e.HasMorePages = False
            Return
        End If

        Dim title = "THANGAM NOTE BOOKSS, PERUNDURAI"
        Dim titleWidth = g.MeasureString(title, boldFont).Width
        g.DrawString(title, boldFont, Brushes.Black, (pageWidth - titleWidth) / 2, y)
        y += 40

        g.DrawString("BILL NO", font, Brushes.Black, marginLeft, y + 6)
        g.DrawRectangle(pen, marginLeft + 75, y - 2, 150, boxHeight)
        g.DrawString(data("Bill_No"), bfont, Brushes.Black, marginLeft + 80, y + 4)

        g.DrawString("DATE", font, Brushes.Black, pageWidth - 175, y + 6)
        g.DrawRectangle(pen, pageWidth - 125, y - 2, 100, boxHeight)
        g.DrawString(data("Printing_date").ToString(), bfont, Brushes.Black, pageWidth - 110, y + 4)

        y += boxHeight + 6


        Dim fields = {
        "PARTY NAME",
        "PRINTING METHOD",
        "PRINTING TYPE",
        "MACHINE NAME",
        "PRINTING ITEM",
        "PAPER SIZE / GSM",
        "PAPER BRAND / WEIGHT",
        "PRINTING COLOUR",
        "QUANTITY",
        "FINISHED QTY / DATE",
        "PRINTING DETAILS"
    }

        Dim values = {
        data("Partyname"),
        data("PMName"),
        data("PTName"),
        data("MachineName"),
        data("NTName"),
        data("Paper_Size_GSM"),
        $"{data("Paper_Brand")} / {data("Paper_Weight")}",
        data("Printing_Colour"),
        data("Quantity"),
        $"{data("FinishQty")} / {data("Finish_Date")}",
        data("Printing_Details")
    }


        For i = 0 To fields.Length - 1
            Dim currentBoxHeight = boxHeight
            If fields(i) = "PRINTING DETAILS" Then currentBoxHeight = boxHeight * 3

            ' Label box
            g.DrawRectangle(pen, marginLeft, y, 265, currentBoxHeight)
            g.DrawString(fields(i), font, Brushes.Black, marginLeft + paddingLeft, y + 4)

            ' Value box
            g.DrawRectangle(pen, marginLeft + 265, y, boxwidth, currentBoxHeight)
            g.DrawString(values(i), bfont, Brushes.Black, marginLeft + 270, y + 4)

            y += currentBoxHeight
        Next

        y += 40
        Dim footer = "Prepared by" & Space(60) & "Checked by" & Space(60) & "Managing Director"
        Dim footerWidth = g.MeasureString(footer, font).Width
        g.DrawString(footer, font, Brushes.Black, (pageWidth - footerWidth) / 2, y)

        e.HasMorePages = False
    End Sub

    Private Sub Guna2DataGridView1_DoubleClick(sender As Object, e As EventArgs) Handles Guna2DataGridView1.DoubleClick
        Try
            If Guna2DataGridView1.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a row to load.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)
            Dim billNoObj As Object = selectedRow.Cells("Bill_No").Value

            If billNoObj Is Nothing OrElse IsDBNull(billNoObj) Then
                MessageBox.Show("No Bill No found in the selected row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            Dim billNo As String = billNoObj.ToString()

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()

                Dim query As String = "
                SELECT TOP 1 
                    pt.Printing_date,
                    pt.Bill_No,
                    lt.Partyname,
                    pm.Name AS PMName,
                    pt.Paper_Size_GSM,
                    pt1.Name AS PTName,
                    mach.Name AS MachineName,
                    pt.Printing_Colour,
                    pt.Quantity,
                    pt.Printing_Details,
                    pt.WorkingStatus,
                    nt.Name AS NoteTypeName,
                    pt.Paper_Brand,
                    pt.Paper_Weight
                FROM Printing_table pt
                LEFT JOIN Ledger_Table lt ON pt.ledger_id = lt.ID
                LEFT JOIN PrintingMethod_table pm ON pt.PrintMethod_Id = pm.ID
                LEFT JOIN PrintingType_table pt1 ON pt.PrintingType_Id = pt1.ID
                LEFT JOIN PrintingMachine_table mach ON pt.PrintingMachine_Id = mach.ID
                LEFT JOIN PrintingItem_table nt ON pt.PrintingItem_Id = nt.ID
                WHERE pt.Bill_No = @BillNo
            "

                Using cmd As New SqlCommand(query, sqlconnect)
                    cmd.Parameters.AddWithValue("@BillNo", billNo)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            ' Fill the text fields directly
                            PrintingDate.Text = Convert.ToDateTime(reader("Printing_date")).ToString("yyyy-MM-dd")
                            BillNoTxt.Text = reader("Bill_No").ToString()
                            PartynameTxt.Text = reader("Partyname").ToString()
                            PMethodTxt.Text = reader("PMName").ToString()
                            Papertxt.Text = reader("Paper_Size_GSM").ToString()
                            PrintingTxt.Text = reader("PTName").ToString()
                            PMachineTxt.Text = reader("MachineName").ToString()
                            Colourtxt.Text = reader("Printing_Colour").ToString()
                            QtyTxt.Text = reader("Quantity").ToString()
                            Detailstxt.Text = reader("Printing_Details").ToString()
                            Status.Text = reader("WorkingStatus").ToString()
                            PrintingItemtxt.Text = reader("NoteTypeName").ToString()
                            Brandtxt.Text = reader("Paper_Brand").ToString()
                            Weighttxt.Text = reader("Paper_Weight").ToString()
                        Else
                            MessageBox.Show("No record found for the selected Bill No.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End Using
                End Using
            End Using

            ' Update mode
            Savebtn.Text = "Update"

        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub



    Public Sub LoadSalesEntry(ByVal billNo As String)
        Try
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()

                Using cmd As New SqlCommand("SELECT  TOP 1 Partyname,Manual_BillNo,sale_date,Remarks FROM Sales_table WHERE Bill_No = @BillNo", sqlconnect)
                    cmd.Parameters.AddWithValue("@BillNo", billNo)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            'Partyname.Text = reader("Partyname").ToString()
                            'ManualBillnoText.Text = reader("Manual_BillNo").ToString()
                            'Datesales.Text = Convert.ToDateTime(reader("sale_date")).ToString("dd/MM/yyyy")
                            'RemarksTextBox.Text = reader("Remarks").ToString()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading sales entry: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class

Imports System.Data.SqlClient
Imports Guna.UI2.WinForms
Public Class Purchase

    Dim BillDate As String
    Private DTItem As DataTable
    Private DTParty As DataTable
    Private activeTextbox As Guna.UI2.WinForms.Guna2TextBox = Nothing
    Public AlterBillNo As String = ""
    Private FinStartDate As DateTime
    Private FinEndDate As DateTime

    Private Sub Purchase_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'PartyComboBox()
        'ItemComboBox()
        Tools.LoadConfiguration()
        LoadAutoCompleteData()
        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me

        BillDate = DateTime.Now.ToString("dd/MM/yyyy")
        Datesales.Text = BillDate

        InitializeDataGridView()
        BillNoTxt.Text = GetNextBillNumber()
        BillNoTxt.ReadOnly = True
        AddHandler KryptonListBox1.Click, AddressOf KryptonListBox1_Click
        AddHandler KryptonListBox1.KeyDown, AddressOf KryptonListBox1_KeyDown
        'AddHandler KryptonListBox1.LostFocus, AddressOf KryptonListBox1_LostFocus

        AddHandler Guna2DataGridView1.KeyDown, AddressOf Guna2DataGridView1_KeyDown
        LoadFinancialPeriod()
        If Not String.IsNullOrEmpty(AlterBillNo) Then
            LoadPurchaseEntry(AlterBillNo)
        End If
        Themeload()
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
                                Headerpanel.BackColor = ColorTranslator.FromHtml(colorString)
                            Catch
                                Headerpanel.BackColor = Color.FromArgb(34, 40, 49)
                            End Try
                        Else
                            Headerpanel.BackColor = Color.FromArgb(34, 40, 49)
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
        Partyname.Focus()
    End Sub
    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles Partyname.KeyDown, ManualBillnoText.KeyDown, Datesales.KeyDown, Itembox.KeyDown, qtytxt.KeyDown, ratetxt.KeyDown, Addbtn.KeyDown

        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.SelectNextControl(CType(sender, Control), forward:=True, tabStopOnly:=True, nested:=True, wrap:=True)
        End If
    End Sub

    Private Function GetNextBillNumber() As String
        Dim query As String = "SELECT ISNULL(Vt_Prefix, '') + CAST(ISNULL(Vt_Billno, 0) AS VARCHAR) + ISNULL(Vt_Suffix, '') AS Vt_FullBillNo FROM v_table WHERE Vt_Name = 'Purchase'"

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
    Private Sub InitializeDataGridView()
        With Guna2DataGridView1
            .ColumnHeadersVisible = True
            .AllowUserToAddRows = False
            .AllowUserToOrderColumns = True

            .Columns.Add("ItemName", "Item Name")
            .Columns.Add("Quantity", "Quantity")
            .Columns.Add("Rate", "Rate")
            .Columns.Add("TotalAmount", "Amount")

            .Columns("ItemName").Width = 158
            .Columns("Quantity").Width = 80
            .Columns("Rate").Width = 80
            .Columns("TotalAmount").Width = 120

            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersHeight = 35
            .CellBorderStyle = DataGridViewCellBorderStyle.Single

            .Columns("ItemName").ReadOnly = True
            .Columns("Quantity").ReadOnly = True
            .Columns("Rate").ReadOnly = True
            .Columns("TotalAmount").ReadOnly = True

            .SelectionMode = DataGridViewSelectionMode.FullRowSelect

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


    Private Sub Guna2DataGridView1_KeyDown(sender As Object, e As KeyEventArgs)

        If e.Control AndAlso e.KeyCode = Keys.Y Then

            If Guna2DataGridView1.SelectedRows.Count > 0 Then
                For Each selectedRow As DataGridViewRow In Guna2DataGridView1.SelectedRows
                    Guna2DataGridView1.Rows.Remove(selectedRow)
                Next
            Else
                MessageBox.Show("Please select a row to remove.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)

            End If
        End If
    End Sub
    Private Function GetSelectedLedgerId() As Integer

        Dim query As String = "SELECT ID FROM Ledger_Table WHERE Partyname = @Partyname and Active = 0"

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Using sqlcommand As New SqlCommand(query, sqlconnect)
                sqlcommand.Parameters.AddWithValue("@Partyname", Partyname.Text)
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

    Private Function GetItemIdByName(itemName As String) As Integer

        Dim query As String = "SELECT ID FROM Item_table WHERE Itemname = @Itemname and  Active = 0"

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Using sqlcommand As New SqlCommand(query, sqlconnect)
                sqlcommand.Parameters.AddWithValue("@Itemname", itemName)
                sqlconnect.Open()
                Return Convert.ToInt32(sqlcommand.ExecuteScalar())
            End Using
        End Using
    End Function
    Private Sub LoadAutoCompleteData()
        DTItem = LoadDataTable("SELECT Itemname FROM Item_table where Active = 0")
        DTParty = LoadDataTable("SELECT Partyname FROM Ledger_Table WHERE UNDER = 'Supplier' and Active = 0")
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
    Private Sub ValidateTyping(sender As Object, e As KeyPressEventArgs) Handles Itembox.KeyPress, Partyname.KeyPress
        Dim tb As Guna2TextBox = CType(sender, Guna2TextBox)

        If Char.IsControl(e.KeyChar) Then Exit Sub

        Dim predictedText As String = tb.Text.Remove(tb.SelectionStart, tb.SelectionLength).Insert(tb.SelectionStart, e.KeyChar.ToString())
        Dim source As DataTable = If(tb Is Itembox, DTItem, DTParty)

        If source IsNot Nothing Then
            Dim exists As Boolean = source.AsEnumerable().Any(Function(row) row.Field(Of String)(0).StartsWith(predictedText, StringComparison.OrdinalIgnoreCase))

            If Not exists Then
                e.Handled = True
            End If
        End If
    End Sub
    Private Sub ShowSuggestionsForTextBox(textBox As Guna2TextBox)
        activeTextbox = textBox
        TextBox_TextChanged(textBox, EventArgs.Empty)
    End Sub
    Private Sub SelectItemFromList()
        If KryptonListBox1.Visible AndAlso KryptonListBox1.SelectedItem IsNot Nothing AndAlso activeTextbox IsNot Nothing Then
            Dim rowView As DataRowView = DirectCast(KryptonListBox1.SelectedItem, DataRowView)
            activeTextbox.Text = rowView(0).ToString()
            KryptonListBox1.Visible = False
            activeTextbox.Focus()
            activeTextbox.SelectionStart = activeTextbox.Text.Length
        End If
    End Sub

    Private Sub KryptonListBox1_Click(sender As Object, e As EventArgs) Handles KryptonListBox1.Click
        SelectItemFromList()
    End Sub
    Private Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles Itembox.TextChanged, Partyname.TextChanged
        Dim tb As Guna2TextBox = CType(sender, Guna2TextBox)
        activeTextbox = tb

        Dim source As DataTable = If(tb Is Itembox, DTItem, DTParty)
        If source Is Nothing OrElse String.IsNullOrEmpty(tb.Text) Then
            KryptonListBox1.Visible = False
            Exit Sub
        End If

        Dim dv As New DataView(source)
        dv.RowFilter = $"[{source.Columns(0).ColumnName}] LIKE '{tb.Text.Replace("'", "''")}%'"

        If dv.Count > 0 AndAlso tb.Focused Then
            KryptonListBox1.DataSource = dv
            KryptonListBox1.DisplayMember = source.Columns(0).ColumnName

            Dim p As Point = tb.Parent.PointToScreen(tb.Location)
            Dim localPoint As Point = Me.PointToClient(p)
            KryptonListBox1.Location = New Point(localPoint.X, localPoint.Y + tb.Height)
            KryptonListBox1.Width = tb.Width
            KryptonListBox1.Visible = True
            KryptonListBox1.BringToFront()
        Else
            KryptonListBox1.Visible = False
        End If
    End Sub
    Private Sub TextBox_Click(sender As Object, e As EventArgs) Handles Itembox.Click, Partyname.Click
        ShowSuggestionsForTextBox(DirectCast(sender, Guna.UI2.WinForms.Guna2TextBox))
    End Sub

    Private Sub KryptonListBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles KryptonListBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            SelectItemFromList()
            e.Handled = True
        ElseIf e.KeyCode = Keys.Escape Then
            KryptonListBox1.Visible = False
            activeTextbox.Focus()
        End If
    End Sub

    Private Sub TextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles Itembox.KeyDown, Partyname.KeyDown
        If e.KeyCode = Keys.F2 Then
            e.Handled = True
            ShowAllSuggestions(CType(sender, Guna2TextBox))
            Exit Sub
        End If

        If KryptonListBox1.Visible Then
            If e.KeyCode = Keys.Down Then
                KryptonListBox1.Focus()
                If KryptonListBox1.Items.Count > 0 Then KryptonListBox1.SelectedIndex = 0
                e.Handled = True
            ElseIf e.KeyCode = Keys.Enter Then
                SelectItemFromList()
                e.Handled = True
            ElseIf e.KeyCode = Keys.Escape Then
                KryptonListBox1.Visible = False
            End If
        End If
    End Sub

    Private Sub ShowAllSuggestions(textBox As Guna2TextBox)
        activeTextbox = textBox

        Dim source As DataTable = If(textBox Is Itembox, DTItem, DTParty)

        If source Is Nothing OrElse source.Rows.Count = 0 Then
            KryptonListBox1.Visible = False
            Exit Sub
        End If

        Dim dv As New DataView(source)
        Dim columnName As String = source.Columns(0).ColumnName

        KryptonListBox1.DataSource = dv
        KryptonListBox1.DisplayMember = columnName

        Dim p As Point = textBox.Parent.PointToScreen(textBox.Location)
        Dim localPoint As Point = Me.PointToClient(p)
        KryptonListBox1.Location = New Point(localPoint.X, localPoint.Y + textBox.Height)

        KryptonListBox1.Width = textBox.Width
        KryptonListBox1.Visible = True
        KryptonListBox1.BringToFront()

        If KryptonListBox1.Items.Count > 0 Then
            KryptonListBox1.Focus()
            KryptonListBox1.SelectedIndex = 0
        End If
    End Sub

    Private Sub TextBox_LostFocus(sender As Object, e As EventArgs) Handles Itembox.LostFocus, Partyname.LostFocus
        TimerFocusDelay.Start()
    End Sub
    Private Sub TimerFocusDelay_Tick(sender As Object, e As EventArgs) Handles TimerFocusDelay.Tick
        TimerFocusDelay.Stop()
        If Not KryptonListBox1.Focused Then
            KryptonListBox1.Visible = False
            If activeTextbox IsNot Nothing Then
                Dim source As DataTable = If(activeTextbox Is Itembox, DTItem, DTParty)
                If Not IsValidInput(activeTextbox, source) Then
                    activeTextbox.Clear()
                End If
            End If
        End If
    End Sub
    Private Sub Itembox_GotFocus(sender As Object, e As EventArgs) Handles Itembox.GotFocus
        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            ShowSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If
    End Sub
    Private Sub Partyname_GotFocus(sender As Object, e As EventArgs) Handles Partyname.GotFocus
        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            ShowSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If
    End Sub
    Private Function IsValidInput(textBox As Guna2TextBox, source As DataTable) As Boolean
        If String.IsNullOrWhiteSpace(textBox.Text) Then Return True
        Return source.AsEnumerable().Any(Function(row) row.Field(Of String)(0).Equals(textBox.Text, StringComparison.OrdinalIgnoreCase))
    End Function
    Private Sub HandleTextBoxEvents(sender As Object, e As EventArgs) Handles _
    Itembox.TextChanged, Itembox.Click, Itembox.GotFocus,
    Partyname.TextChanged, Partyname.Click, Partyname.GotFocus

        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            ShowSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If
    End Sub
    Private Sub SelectSuggestion()
        If KryptonListBox1.SelectedItem IsNot Nothing AndAlso activeTextbox IsNot Nothing Then
            Dim rowView As DataRowView = DirectCast(KryptonListBox1.SelectedItem, DataRowView)
            activeTextbox.Text = rowView(0).ToString()
            KryptonListBox1.Visible = False
            activeTextbox.Focus()
            activeTextbox.SelectionStart = activeTextbox.Text.Length
        End If
    End Sub

    Private Sub Partyname_TextChanged(sender As Object, e As EventArgs) Handles Partyname.TextChanged

        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            ShowSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If
    End Sub

    Private Sub Partyname_Click(sender As Object, e As EventArgs) Handles Partyname.Click
        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            ShowSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If
    End Sub
    Private Sub Itembox_TextChanged(sender As Object, e As EventArgs) Handles Itembox.TextChanged

        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            ShowSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If
    End Sub

    Private Sub Itembox_Click(sender As Object, e As EventArgs) Handles Itembox.Click
        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            ShowSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If
    End Sub

    Private Sub qtytxt_TextChanged(sender As Object, e As EventArgs) Handles ratetxt.TextChanged, qtytxt.TextChanged
        CalculateAmount()
    End Sub

    Private Sub CalculateAmount()
        Dim QTY As Decimal
        Dim RATE As Decimal

        Decimal.TryParse(qtytxt.Text, QTY)
        Decimal.TryParse(ratetxt.Text, RATE)

        Totamttxt.Text = (QTY * RATE).ToString("F2")
    End Sub

    Private Sub Addbtn_Click(sender As Object, e As EventArgs) Handles Addbtn.Click
        If String.IsNullOrWhiteSpace(Itembox.Text) OrElse
        String.IsNullOrWhiteSpace(qtytxt.Text) OrElse
        String.IsNullOrWhiteSpace(ratetxt.Text) OrElse
        String.IsNullOrWhiteSpace(Totamttxt.Text) Then
            MessageBox.Show("Please fill in all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If


        Dim rowIndex As Integer = Guna2DataGridView1.Rows.Add()
        Dim newRow As DataGridViewRow = Guna2DataGridView1.Rows(rowIndex)

        newRow.Cells("ItemName").Value = Itembox.Text
        newRow.Cells("Quantity").Value = qtytxt.Text
        newRow.Cells("Rate").Value = ratetxt.Text
        newRow.Cells("TotalAmount").Value = Totamttxt.Text

        ClearInputs()

        UpdateTotals()
        Itembox.Focus()
    End Sub


    Private Sub UpdateTotals()
        Dim totalAmount As Decimal = 0
        Dim totalDiscount As Decimal = 0
        Dim NetAmount As Decimal = 0

        For Each row As DataGridViewRow In Guna2DataGridView1.Rows
            If Not row.IsNewRow Then
                NetAmount += Convert.ToDecimal(row.Cells("TotalAmount").Value)
            End If
        Next

        LabelNetAmt.Text = NetAmount.ToString("F2")

    End Sub

    Private Sub ClearInputs()
        Itembox.Clear()
        qtytxt.Clear()
        ratetxt.Clear()
        Totamttxt.Clear()
    End Sub

    Private Sub ClearAllInputs()
        Guna2DataGridView1.Rows.Clear()
        Partyname.Clear()
        Datesales.Text = DateTime.Now.ToString("dd/MM/yyyy")
        ClearInputs()
    End Sub

    Private Sub Clearbtn_Click(sender As Object, e As EventArgs) Handles Clearbtn.Click
        ClearAllInputs()
        ClearInputs()
    End Sub
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
        Dim transaction As SqlTransaction = Nothing
        Dim transactionCompleted As Boolean = False
        Dim totalAmount As Decimal = 0
        Dim Remarks As String = RemarksTextBox.Text
        Dim Manual_BillNo As String = ManualBillnoText.Text
        Dim isUpdate As Boolean = Not String.IsNullOrWhiteSpace(AlterBillNo)
        Dim billNo As String = If(isUpdate, AlterBillNo, GetNextBillNumber())

        Dim parsedDate As DateTime

        Try
            parsedDate = DateTime.ParseExact(Datesales.Text, "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture)
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

        If String.IsNullOrWhiteSpace(Partyname.Text) Then
            MessageBox.Show("Please enter a party name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrWhiteSpace(ManualBillnoText.Text) Then
            MessageBox.Show("Manual Bill No is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Guna2DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("Please add at least one item before saving.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        For Each row As DataGridViewRow In Guna2DataGridView1.Rows
            If Not row.IsNewRow Then
                totalAmount += Convert.ToDecimal(row.Cells("TotalAmount").Value)
            End If
        Next

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                transaction = sqlconnect.BeginTransaction()

                Dim result As DialogResult = MessageBox.Show("Do you want to save?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If result = DialogResult.No Then
                    Return
                End If

                Dim ledgerId As Integer = GetSelectedLedgerId()
                Dim UserId As Integer = Tools.GetStoredUsername()

                Dim originalItemIds As New HashSet(Of Integer)
                Dim currentItemIds As New HashSet(Of Integer)

                If isUpdate Then
                    Using getCmd As New SqlCommand("SELECT item_id FROM Purchase_table WHERE Bill_No = @Bill_No", sqlconnect, transaction)
                        getCmd.Parameters.AddWithValue("@Bill_No", billNo)
                        Using reader = getCmd.ExecuteReader()
                            While reader.Read()
                                originalItemIds.Add(Convert.ToInt32(reader("item_id")))
                            End While
                        End Using
                    End Using
                End If

                For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                    If Not row.IsNewRow Then
                        Dim itemName As String = row.Cells("ItemName").Value.ToString()
                        Dim itemId As Integer = GetItemIdByName(itemName)
                        Dim quantity As Integer = Convert.ToInt32(row.Cells("Quantity").Value)
                        Dim rate As Decimal = Convert.ToDecimal(row.Cells("Rate").Value)
                        Dim amount As Decimal = Convert.ToDecimal(row.Cells("TotalAmount").Value)

                        currentItemIds.Add(itemId)

                        If isUpdate Then
                            Using cmd As New SqlCommand("
                            IF EXISTS (SELECT 1 FROM Stock_table WHERE Bill_No = @Bill_No AND item_id = @item_id)
                                UPDATE Stock_table SET Stock_date = @Stock_date, ledger_id = @ledger_id, Itemname = @Itemname,
                                    quantity = @quantity, Rate = @Rate, Total_Amount = @Total_Amount, UserID = @UserID
                                WHERE Bill_No = @Bill_No AND item_id = @item_id
                            ELSE
                                INSERT INTO Stock_table (Bill_No, Stock_date, ledger_id, item_id, Itemname, quantity, Rate, Total_Amount, EntryType, UserID)
                                VALUES (@Bill_No, @Stock_date, @ledger_id, @item_id, @Itemname, @quantity, @Rate, @Total_Amount, 1, @UserID)", sqlconnect, transaction)

                                cmd.Parameters.AddWithValue("@Bill_No", billNo)
                                cmd.Parameters.AddWithValue("@Stock_date", parsedDate)
                                cmd.Parameters.AddWithValue("@ledger_id", ledgerId)
                                cmd.Parameters.AddWithValue("@item_id", itemId)
                                cmd.Parameters.AddWithValue("@Itemname", itemName)
                                cmd.Parameters.AddWithValue("@quantity", quantity)
                                cmd.Parameters.AddWithValue("@Rate", rate)
                                cmd.Parameters.AddWithValue("@Total_Amount", amount)
                                cmd.Parameters.AddWithValue("@UserID", UserId)
                                cmd.ExecuteNonQuery()
                            End Using

                            Using cmd As New SqlCommand("
                            IF EXISTS (SELECT 1 FROM Purchase_table WHERE Bill_No = @Bill_No AND item_id = @item_id)
                                UPDATE Purchase_table SET ledger_id = @ledger_id, Partyname = @Partyname, Manual_BillNo = @Manual_BillNo,
                                    Purchase_date = @Purchase_date, Itemname = @Itemname, quantity = @quantity, Rate = @Rate,
                                    Total_Amount = @Total_Amount, Remarks = @Remarks, UserID = @UserID
                                WHERE Bill_No = @Bill_No AND item_id = @item_id
                            ELSE
                                INSERT INTO Purchase_table (ledger_id, Partyname, Bill_No, Manual_BillNo, Purchase_date, item_id, Itemname, quantity, Rate, Total_Amount, Cancel, EntryType, Remarks, UserID)
                                VALUES (@ledger_id, @Partyname, @Bill_No, @Manual_BillNo, @Purchase_date, @item_id, @Itemname, @quantity, @Rate, @Total_Amount, 0, 1, @Remarks, @UserID)", sqlconnect, transaction)

                                cmd.Parameters.AddWithValue("@ledger_id", ledgerId)
                                cmd.Parameters.AddWithValue("@Partyname", Partyname.Text)
                                cmd.Parameters.AddWithValue("@Bill_No", billNo)
                                cmd.Parameters.AddWithValue("@Manual_BillNo", Manual_BillNo)
                                cmd.Parameters.AddWithValue("@Purchase_date", parsedDate)
                                cmd.Parameters.AddWithValue("@item_id", itemId)
                                cmd.Parameters.AddWithValue("@Itemname", itemName)
                                cmd.Parameters.AddWithValue("@quantity", quantity)
                                cmd.Parameters.AddWithValue("@Rate", rate)
                                cmd.Parameters.AddWithValue("@Total_Amount", amount)
                                cmd.Parameters.AddWithValue("@Remarks", Remarks)
                                cmd.Parameters.AddWithValue("@UserID", UserId)
                                cmd.ExecuteNonQuery()
                            End Using
                        Else
                            Using cmd As New SqlCommand("INSERT INTO Stock_table (Bill_No, Stock_date, ledger_id, item_id, Itemname, quantity, Rate, Total_Amount, EntryType, UserID)
                                                    VALUES (@Bill_No, @Stock_date, @ledger_id, @item_id, @Itemname, @quantity, @Rate, @Total_Amount, 1, @UserID)", sqlconnect, transaction)
                                cmd.Parameters.AddWithValue("@Bill_No", billNo)
                                cmd.Parameters.AddWithValue("@Stock_date", parsedDate)
                                cmd.Parameters.AddWithValue("@ledger_id", ledgerId)
                                cmd.Parameters.AddWithValue("@item_id", itemId)
                                cmd.Parameters.AddWithValue("@Itemname", itemName)
                                cmd.Parameters.AddWithValue("@quantity", quantity)
                                cmd.Parameters.AddWithValue("@Rate", rate)
                                cmd.Parameters.AddWithValue("@Total_Amount", amount)
                                cmd.Parameters.AddWithValue("@UserID", UserId)
                                cmd.ExecuteNonQuery()
                            End Using

                            Using cmd As New SqlCommand("INSERT INTO Purchase_table (ledger_id, Partyname, Bill_No, Manual_BillNo, Purchase_date, item_id, Itemname, quantity, Rate, Total_Amount, Cancel, EntryType, Remarks, UserID)
                                                    VALUES (@ledger_id, @Partyname, @Bill_No, @Manual_BillNo, @Purchase_date, @item_id, @Itemname, @quantity, @Rate, @Total_Amount, 0, 1, @Remarks, @UserID)", sqlconnect, transaction)
                                cmd.Parameters.AddWithValue("@ledger_id", ledgerId)
                                cmd.Parameters.AddWithValue("@Partyname", Partyname.Text)
                                cmd.Parameters.AddWithValue("@Bill_No", billNo)
                                cmd.Parameters.AddWithValue("@Manual_BillNo", Manual_BillNo)
                                cmd.Parameters.AddWithValue("@Purchase_date", parsedDate)
                                cmd.Parameters.AddWithValue("@item_id", itemId)
                                cmd.Parameters.AddWithValue("@Itemname", itemName)
                                cmd.Parameters.AddWithValue("@quantity", quantity)
                                cmd.Parameters.AddWithValue("@Rate", rate)
                                cmd.Parameters.AddWithValue("@Total_Amount", amount)
                                cmd.Parameters.AddWithValue("@Remarks", Remarks)
                                cmd.Parameters.AddWithValue("@UserID", UserId)
                                cmd.ExecuteNonQuery()
                            End Using
                        End If
                    End If
                Next

                If isUpdate Then
                    Dim deletedItemIds = originalItemIds.Except(currentItemIds)
                    For Each deletedItemId In deletedItemIds
                        Using delCmd As New SqlCommand("DELETE FROM Stock_table WHERE Bill_No = @Bill_No AND item_id = @item_id", sqlconnect, transaction)
                            delCmd.Parameters.AddWithValue("@Bill_No", billNo)
                            delCmd.Parameters.AddWithValue("@item_id", deletedItemId)
                            delCmd.ExecuteNonQuery()
                        End Using

                        Using delCmd As New SqlCommand("DELETE FROM Purchase_table WHERE Bill_No = @Bill_No AND item_id = @item_id", sqlconnect, transaction)
                            delCmd.Parameters.AddWithValue("@Bill_No", billNo)
                            delCmd.Parameters.AddWithValue("@item_id", deletedItemId)
                            delCmd.ExecuteNonQuery()
                        End Using
                    Next
                End If

                If Not isUpdate Then
                    Using cmd As New SqlCommand("UPDATE v_table SET Vt_Billno = Vt_Billno + 1 WHERE Vt_Name = 'Purchase'", sqlconnect, transaction)
                        cmd.ExecuteNonQuery()
                    End Using
                End If

                transaction.Commit()
                transactionCompleted = True
                MessageBox.Show("Saved successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                ClearAllInputs()

            Catch ex As Exception
                If transaction IsNot Nothing AndAlso transaction.Connection IsNot Nothing AndAlso transaction.Connection.State = ConnectionState.Open Then
                    Try
                        If Not transactionCompleted Then
                            transaction.Rollback()
                        End If
                    Catch rollbackEx As Exception
                        MessageBox.Show("Rollback error: " & rollbackEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
                MessageBox.Show("Save error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                If sqlconnect.State = ConnectionState.Open Then
                    sqlconnect.Close()
                End If
            End Try
        End Using
    End Sub

    Public Sub LoadPurchaseEntry(ByVal billNo As String)
        Try
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()

                Using cmd As New SqlCommand("SELECT  TOP 1 Partyname,Manual_BillNo,Purchase_date,Remarks FROM Purchase_table WHERE Bill_No = @BillNo", sqlconnect)
                    cmd.Parameters.AddWithValue("@BillNo", billNo)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Partyname.Text = reader("Partyname").ToString()
                            ManualBillnoText.Text = reader("Manual_BillNo").ToString()
                            Datesales.Text = Convert.ToDateTime(reader("Purchase_date")).ToString("dd/MM/yyyy")
                            RemarksTextBox.Text = reader("Remarks").ToString()
                        End If
                    End Using
                End Using

                Guna2DataGridView1.Rows.Clear()
                Using cmd As New SqlCommand("SELECT Itemname,quantity,Rate,Total_Amount FROM Purchase_table WHERE Bill_No = @BillNo and Cancel = 0", sqlconnect)
                    cmd.Parameters.AddWithValue("@BillNo", billNo)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Guna2DataGridView1.Rows.Add(
                            reader("Itemname").ToString(),
                            reader("quantity"),
                            reader("Rate"),
                            reader("Total_Amount")
                        )
                        End While
                    End Using
                End Using

                BillNoTxt.Text = billNo
                AlterBillNo = billNo

            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading Purchase entry: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub

End Class
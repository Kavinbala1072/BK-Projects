Imports System.Data.SqlClient
Imports System.IO
Imports Guna.UI2.WinForms

Public Class LedgerForm

    Private DTUnder As DataTable
    Private DTSearch As DataTable
    Private activeTextbox As Guna.UI2.WinForms.Guna2TextBox

    Private Sub LedgerForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Dim elipse As New Guna2Elipse()
            elipse.BorderRadius = 20
            elipse.TargetControl = Me

            Tools.LoadConfiguration()
            RefreshLedgerList()
            LoadAutoCompleteData()
            InitializeDataGridView()
            Themeload()
            AddHandler ListBox1.Click, AddressOf ListBox1_Click
            AddHandler ListBox1.KeyDown, AddressOf ListBox1_KeyDown
        Catch ex As Exception
            MessageBox.Show("Error during form load: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
        Partytxt.Focus()
    End Sub
    Private Sub RefreshLedgerList()

        Dim query As String = "select Partyname,Under,Mobile from ledger_table where Active = 0"

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Dim command As New SqlCommand(query, sqlconnect)

            Try
                sqlconnect.Open()
                Dim reader As SqlDataReader = command.ExecuteReader()
                Dim dt As New DataTable()
                dt.Load(reader)
                dt.Columns.Add("SNo", GetType(Integer))
                For i As Integer = 0 To dt.Rows.Count - 1
                    dt.Rows(i)("SNo") = i + 1
                Next
                Guna2DataGridView1.DataSource = dt
                Guna2DataGridView1.Columns("SNo").DisplayIndex = 0
                Guna2DataGridView1.Columns("SNo").HeaderText = "S.No"
                Guna2DataGridView1.Columns("SNo").Width = 50

                ' Guna2DataGridView1.Columns("ID").HeaderText = "SNo"
                Guna2DataGridView1.Columns("Partyname").HeaderText = "Party Name"
                Guna2DataGridView1.Columns("Under").HeaderText = "Ledger Group"
                Guna2DataGridView1.Columns("Mobile").HeaderText = "Mobile"

                'Guna2DataGridView1.Columns("ID").Width = 50
                Guna2DataGridView1.Columns("Partyname").Width = 200
                Guna2DataGridView1.Columns("Under").Width = 100
                Guna2DataGridView1.Columns("Mobile").Width = 100
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using

    End Sub

    Private Sub InitializeDataGridView()

        With Guna2DataGridView1
            '.Dock = DockStyle.Fill
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
            .Margin = New Padding(20, 20, 20, 20)
            .MultiSelect = False

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

    Private Sub ClearInputFields()
        Partytxt.Clear()
        underbox.Text = ""
        Mobiletxt.Clear()
    End Sub

    Private Sub Searchbtn_Click_1(sender As Object, e As EventArgs) Handles Searchbtn.Click

        Dim Partyname As String = Searchtxt.Text

        Using conn As SqlConnection = Tools.GetConnection()
            Dim sqlcommand As New SqlCommand("SELECT * FROM Ledger_table WHERE Partyname = @Partyname and Active = 0", conn)
            sqlcommand.Parameters.AddWithValue("@Partyname", Partyname)

            Try
                conn.Open()
                Using sqlreader As SqlDataReader = sqlcommand.ExecuteReader()
                    If sqlreader.HasRows Then
                        While sqlreader.Read()
                            Partytxt.Text = sqlreader("Partyname").ToString()
                            underbox.Text = sqlreader("Under").ToString()
                            Mobiletxt.Text = sqlreader("Mobile").ToString()
                        End While
                    Else
                        MessageBox.Show("No record found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using

    End Sub

    Private Function GetSelectedLedgerId(partyName As String) As Integer
        Try
            If String.IsNullOrWhiteSpace(partyName) Then
                '   MessageBox.Show("Please enter a party name.")
                Return 0
            End If

            Dim query As String = "SELECT ID FROM Ledger_Table WHERE Partyname = @Partyname and Active = 0"

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Partyname", partyName)
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
            MessageBox.Show("Error getting ledger ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        End Try
    End Function
    Private Sub Savebtn_Click_1(sender As Object, e As EventArgs) Handles Savebtn.Click
        Dim partyName As String = Partytxt.Text.Trim()
        Dim under As String = underbox.Text.Trim()
        Dim mobile As String = Mobiletxt.Text.Trim()
        Dim searchPartyName As String = Searchtxt.Text.Trim()

        If String.IsNullOrWhiteSpace(partyName) OrElse
       String.IsNullOrWhiteSpace(under) OrElse
       String.IsNullOrWhiteSpace(mobile) Then
            MessageBox.Show("All fields are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using conn As SqlConnection = Tools.GetConnection()
            Try
                conn.Open()

                Dim ledgerId As Integer = GetSelectedLedgerId(searchPartyName)
                Dim UserId As Integer = Tools.GetStoredUsername()

                If ledgerId > 0 Then
                    Using updateCommand As New SqlCommand("UPDATE Ledger_table SET Partyname = @Partyname, Under = @Under, Mobile = @Mobile, UserID = @UserID WHERE ID = @ID", conn)
                        updateCommand.Parameters.AddWithValue("@Partyname", partyName)
                        updateCommand.Parameters.AddWithValue("@Under", under)
                        updateCommand.Parameters.AddWithValue("@Mobile", mobile)
                        updateCommand.Parameters.AddWithValue("@ID", ledgerId)
                        updateCommand.Parameters.AddWithValue("@UserID", UserId)

                        updateCommand.ExecuteNonQuery()
                        MessageBox.Show("Ledger Alteration updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Using
                Else
                    Using insertCommand As New SqlCommand("INSERT INTO Ledger_table (Partyname, Under, Mobile, UserId) VALUES (@Partyname, @Under, @Mobile, @UserId)", conn)
                        insertCommand.Parameters.AddWithValue("@Partyname", partyName)
                        insertCommand.Parameters.AddWithValue("@Under", under)
                        insertCommand.Parameters.AddWithValue("@Mobile", mobile)
                        insertCommand.Parameters.AddWithValue("@UserID", UserId)

                        insertCommand.ExecuteNonQuery()
                        MessageBox.Show("Ledger record saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Using
                End If

                ClearInputFields()
                RefreshLedgerList()
                LoadAutoCompleteData()

            Catch ex As Exception
                MessageBox.Show("Error saving ledger: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub Killbtn_Click_1(sender As Object, e As EventArgs) Handles Killbtn.Click

        Dim Partyname As String = Searchtxt.Text

        Using conn As SqlConnection = Tools.GetConnection()
            Dim sqlcommand As New SqlCommand("UPDATE Ledger_Table SET Active = 1 WHERE Partyname = @Partyname", conn)
            sqlcommand.Parameters.AddWithValue("@Partyname", Partyname)

            Try
                conn.Open()
                Dim rowsAffected As Integer = SqlCommand.ExecuteNonQuery()
                If rowsAffected > 0 Then
                    MessageBox.Show("Record inactivated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("No record found with that ID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
                Partytxt.Text = ""
                underbox.Text = ""
                Mobiletxt.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                RefreshLedgerList()
                LoadAutoCompleteData()
            End Try
        End Using
        'ClearInputFields()

    End Sub

    Private Sub Refreshbtn_Click(sender As Object, e As EventArgs) Handles Refreshbtn.Click
        RefreshLedgerList()
        LoadAutoCompleteData()
    End Sub
    Private Sub LoadAutoCompleteData()
        DTUnder = LoadDataTable("SELECT LedgerGroup_Name FROM LedgerGroup_Table ORDER BY LedgerGroup_Name ASC")
        DTSearch = LoadDataTable("SELECT Partyname FROM Ledger_Table WHERE Active = 0")
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

    Private Sub ValidateTyping(sender As Object, e As KeyPressEventArgs) Handles underbox.KeyPress, Searchtxt.KeyPress
        If Char.IsControl(e.KeyChar) Then Exit Sub
        Dim tb As Guna2TextBox = CType(sender, Guna2TextBox)
        Dim predictedText As String = tb.Text.Remove(tb.SelectionStart, tb.SelectionLength).Insert(tb.SelectionStart, e.KeyChar.ToString())

        If DTUnder IsNot Nothing Then
            Dim exists As Boolean = DTUnder.AsEnumerable().Any(Function(row) row.Field(Of String)(0).StartsWith(predictedText, StringComparison.OrdinalIgnoreCase))
            If Not exists Then e.Handled = True
        End If
    End Sub

    Private Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles underbox.TextChanged, Searchtxt.TextChanged
        Dim tb As Guna2TextBox = CType(sender, Guna2TextBox)
        activeTextbox = tb

        Dim source As DataTable = If(tb Is underbox, DTUnder, DTSearch)
        If source Is Nothing OrElse String.IsNullOrEmpty(tb.Text) Then
            ListBox1.Visible = False
            Exit Sub
        End If

        Dim dv As New DataView(source)
        dv.RowFilter = $"[{source.Columns(0).ColumnName}] LIKE '{tb.Text.Replace("'", "''")}%'"

        If dv.Count > 0 AndAlso tb.Focused Then
            ListBox1.DataSource = dv
            ListBox1.DisplayMember = source.Columns(0).ColumnName

            ' POSITION correctly
            Dim p As Point = tb.Parent.PointToScreen(tb.Location)
            Dim localPoint As Point = Me.PointToClient(p)
            ListBox1.Location = New Point(localPoint.X, localPoint.Y + tb.Height)
            ListBox1.Width = tb.Width
            ListBox1.Visible = True
            ListBox1.BringToFront()
        Else
            ListBox1.Visible = False
        End If
    End Sub

    Private Sub SelectItemFromList()
        If ListBox1.Visible AndAlso ListBox1.SelectedItem IsNot Nothing AndAlso activeTextbox IsNot Nothing Then
            Dim rowView As DataRowView = DirectCast(ListBox1.SelectedItem, DataRowView)
            activeTextbox.Text = rowView(0).ToString()
            ListBox1.Visible = False
            activeTextbox.Focus()
            activeTextbox.SelectionStart = activeTextbox.Text.Length
        End If
    End Sub

    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) Handles Partytxt.KeyDown, underbox.KeyDown, Mobiletxt.KeyDown, Searchtxt.KeyDown, Searchbtn.KeyDown
        If sender Is Nothing Then Exit Sub
        Dim currentControl As Control = CType(sender, Control)

        If e.KeyCode = Keys.F2 Then
            If currentControl Is underbox OrElse currentControl Is Searchtxt Then
                e.Handled = True
                ShowAllSuggestions(CType(currentControl, Guna2TextBox))
                Exit Sub
            End If
        End If

        If (currentControl Is underbox OrElse currentControl Is Searchtxt) AndAlso ListBox1.Visible Then
            If e.KeyCode = Keys.Down Then
                ListBox1.Focus()
                If ListBox1.Items.Count > 0 Then ListBox1.SelectedIndex = 0
                e.Handled = True
                Exit Sub
            ElseIf e.KeyCode = Keys.Enter Then
                SelectItemFromList()
                e.Handled = True
                e.SuppressKeyPress = True
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

        Dim source As DataTable = If(tb Is underbox, DTUnder, DTSearch)

        If source Is Nothing OrElse source.Rows.Count = 0 Then
            ListBox1.Visible = False
            Exit Sub
        End If

        Dim dv As New DataView(source)
        ListBox1.DataSource = dv
        ListBox1.DisplayMember = source.Columns(0).ColumnName

        Dim p As Point = tb.Parent.PointToScreen(tb.Location)
        Dim localPoint As Point = Me.PointToClient(p)
        ListBox1.Location = New Point(localPoint.X, localPoint.Y + tb.Height)
        ListBox1.Width = tb.Width
        ListBox1.Visible = True
        ListBox1.BringToFront()

        If ListBox1.Items.Count > 0 Then
            ListBox1.Focus()
            ListBox1.SelectedIndex = 0
        End If
    End Sub
    Private Sub ListBox1_Click(sender As Object, e As EventArgs) Handles ListBox1.Click
        SelectItemFromList()
    End Sub

    Private Sub ListBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles ListBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            SelectItemFromList()
        ElseIf e.KeyCode = Keys.Escape Then
            ListBox1.Visible = False
            If activeTextbox IsNot Nothing Then activeTextbox.Focus()
        End If
    End Sub

    ' Focus Delay Logic
    Private Sub TextBox_LostFocus(sender As Object, e As EventArgs) Handles underbox.LostFocus, Searchtxt.LostFocus
        TimerFocusDelay.Start()
    End Sub

    Private Sub TimerFocusDelay_Tick(sender As Object, e As EventArgs) Handles TimerFocusDelay.Tick
        TimerFocusDelay.Stop()
        If Not ListBox1.Focused Then
            ListBox1.Visible = False
            ' Final Validation for 'Under' box
            If activeTextbox Is underbox Then
                If Not IsValidInput(underbox, DTUnder) Then underbox.Clear()
            End If
        End If
    End Sub

    Private Function IsValidInput(textBox As Guna2TextBox, source As DataTable) As Boolean
        If String.IsNullOrWhiteSpace(textBox.Text) Then Return True
        Return source.AsEnumerable().Any(Function(row) row.Field(Of String)(0).Equals(textBox.Text, StringComparison.OrdinalIgnoreCase))
    End Function
End Class
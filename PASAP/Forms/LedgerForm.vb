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
            Themeload()

            AddHandler ListBox1.Click, AddressOf ListBox1_Click
            AddHandler ListBox1.KeyDown, AddressOf ListBox1_KeyDown
            'AddHandler ListBox1.LostFocus, AddressOf ListBox1_LostFocus
        Catch ex As Exception
            MessageBox.Show("Error during form load: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Themeload()
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                ' Header Color
                Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'HeaderColor'", sqlconnect)
                Using reader As SqlDataReader = Query.ExecuteReader()
                    If reader.Read() Then
                        Dim colorString As String = reader("Ctl_Value").ToString()
                        HeaderPanel.BackColor = If(Not String.IsNullOrEmpty(colorString), ColorTranslator.FromHtml(colorString), Color.FromArgb(34, 40, 49))
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading HeaderColor: " & ex.Message)
            End Try
        End Using

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                ' Screen Color
                Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'ScreenColor'", sqlconnect)
                Using reader As SqlDataReader = Query.ExecuteReader()
                    If reader.Read() Then
                        Dim colorString As String = reader("Ctl_Value").ToString()
                        If Not String.IsNullOrEmpty(colorString) Then
                            Me.BackColor = ColorTranslator.FromHtml(colorString)
                        Else
                            Me.BackColor = Color.FromArgb(232, 232, 232)
                        End If
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

    ' Updated to include Openingtxt instead of Mobiletxt
    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles Partytxt.KeyDown, underbox.KeyDown, Openingtxt.KeyDown, Searchtxt.KeyDown, Searchbtn.KeyDown

        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.SelectNextControl(CType(sender, Control), forward:=True, tabStopOnly:=True, nested:=True, wrap:=True)
        End If
    End Sub

    Private Sub RefreshLedgerList()
        ' Changed Mobile to Opening
        Dim query As String = "SELECT Partyname, Under, Opening FROM Ledger_Table WHERE Active = 0"

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Dim command As New SqlCommand(query, sqlconnect)
            Try
                sqlconnect.Open()
                Dim reader As SqlDataReader = command.ExecuteReader()
                Dim dt As New DataTable()
                dt.Load(reader)
                ' Note: SNo logic is here but usually applied to a GridView, not used in the UI provided
            Catch ex As Exception
                MessageBox.Show("An error occurred refreshing list: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub ClearInputFields()
        Partytxt.Clear()
        underbox.Text = ""
        Openingtxt.Clear() ' Changed from Mobiletxt
        Searchtxt.Clear()
    End Sub

    Private Sub Searchbtn_Click_1(sender As Object, e As EventArgs) Handles Searchbtn.Click
        Dim Partyname As String = Searchtxt.Text.Trim()

        Using conn As SqlConnection = Tools.GetConnection()
            Dim sqlcommand As New SqlCommand("SELECT * FROM Ledger_Table WHERE Partyname = @Partyname AND Active = 0", conn)
            sqlcommand.Parameters.AddWithValue("@Partyname", Partyname)

            Try
                conn.Open()
                Using sqlreader As SqlDataReader = sqlcommand.ExecuteReader()
                    If sqlreader.Read() Then
                        Partytxt.Text = sqlreader("Partyname").ToString()
                        underbox.Text = sqlreader("Under").ToString()
                        Openingtxt.Text = sqlreader("Opening").ToString() ' Changed from Mobile
                    Else
                        MessageBox.Show("No record found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error searching: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Function GetSelectedLedgerId(partyName As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM Ledger_Table WHERE Partyname = @Partyname AND Active = 0"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Partyname", partyName)
                    sqlconnect.Open()
                    Dim result = sqlcommand.ExecuteScalar()
                    Return If(result IsNot Nothing, Convert.ToInt32(result), 0)
                End Using
            End Using
        Catch ex As Exception
            Return 0
        End Try
    End Function

    Private Sub Savebtn_Click_1(sender As Object, e As EventArgs) Handles Savebtn.Click
        Dim partyName As String = Partytxt.Text.Trim()
        Dim under As String = underbox.Text.Trim()
        Dim openingBal As String = Openingtxt.Text.Trim() ' Changed from mobile
        Dim searchPartyName As String = Searchtxt.Text.Trim()

        If String.IsNullOrWhiteSpace(partyName) OrElse String.IsNullOrWhiteSpace(under) Then
            MessageBox.Show("Ledger Name and Group (Under) are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using conn As SqlConnection = Tools.GetConnection()
            Try
                conn.Open()
                Dim ledgerId As Integer = GetSelectedLedgerId(searchPartyName)
                Dim UserId As Integer = Tools.GetStoredUsername()

                If ledgerId > 0 Then
                    ' UPDATE (Opening replaces Mobile)
                    Dim sql As String = "UPDATE Ledger_Table SET Partyname = @Partyname, Under = @Under, Opening = @Opening, UserID = @UserID WHERE ID = @ID"
                    Using updateCommand As New SqlCommand(sql, conn)
                        updateCommand.Parameters.AddWithValue("@Partyname", partyName)
                        updateCommand.Parameters.AddWithValue("@Under", under)
                        updateCommand.Parameters.AddWithValue("@Opening", openingBal)
                        updateCommand.Parameters.AddWithValue("@ID", ledgerId)
                        updateCommand.Parameters.AddWithValue("@UserID", UserId)
                        updateCommand.ExecuteNonQuery()
                        MessageBox.Show("Ledger updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End Using
                Else
                    ' INSERT (Opening replaces Mobile)
                    Dim sql As String = "INSERT INTO Ledger_Table (Partyname, Under, Opening, UserId, Active) VALUES (@Partyname, @Under, @Opening, @UserId, 0)"
                    Using insertCommand As New SqlCommand(sql, conn)
                        insertCommand.Parameters.AddWithValue("@Partyname", partyName)
                        insertCommand.Parameters.AddWithValue("@Under", under)
                        insertCommand.Parameters.AddWithValue("@Opening", openingBal)
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
        Dim Partyname As String = Searchtxt.Text.Trim()
        If Partyname = "" Then Exit Sub

        Using conn As SqlConnection = Tools.GetConnection()
            ' Set Active = 1 to make it Inactive
            Dim sqlcommand As New SqlCommand("UPDATE Ledger_Table SET Active = 1 WHERE Partyname = @Partyname", conn)
            sqlcommand.Parameters.AddWithValue("@Partyname", Partyname)

            Try
                conn.Open()
                Dim rowsAffected As Integer = sqlcommand.ExecuteNonQuery()
                If rowsAffected > 0 Then
                    MessageBox.Show("Record marked as Inactive.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ClearInputFields()
                Else
                    MessageBox.Show("No record found to update.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                RefreshLedgerList()
                LoadAutoCompleteData()
            End Try
        End Using
    End Sub

    ' --- Suggestions Logic remains same but ensures Openingtxt isn't causing issues ---

    Private Sub LoadAutoCompleteData()
        Try
            DTUnder = LoadDataTable("SELECT LedgerGroup_Name FROM LedgerGroup_Table ORDER BY LedgerGroup_Name DESC")
            DTSearch = LoadDataTable("SELECT Partyname FROM Ledger_Table WHERE Active = 0")
            ListBox1.Visible = False
        Catch ex As Exception
            MessageBox.Show("Error loading autocomplete data: " & ex.Message)
        End Try
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

    Private Sub ShowSuggestionsForTextBox(textBox As Guna.UI2.WinForms.Guna2TextBox)
        activeTextbox = textBox
        Dim source As DataTable = If(textBox Is underbox, DTUnder, If(textBox Is Searchtxt, DTSearch, Nothing))

        If source Is Nothing Then
            ListBox1.Visible = False
            Exit Sub
        End If

        Dim dv As New DataView(source)
        Dim filterText As String = textBox.Text.Replace("'", "''")
        Dim columnName As String = source.Columns(0).ColumnName

        If Not String.IsNullOrWhiteSpace(filterText) Then
            dv.RowFilter = $"[{columnName}] LIKE '%{filterText}%'"
        End If

        If dv.Count = 0 Then
            ListBox1.Visible = False
            Exit Sub
        End If

        ListBox1.DataSource = dv
        ListBox1.DisplayMember = columnName
        ListBox1.Visible = True
        ListBox1.Location = New Point(textBox.Left, textBox.Top + textBox.Height)
        ListBox1.Width = textBox.Width
        ListBox1.BringToFront()
    End Sub

    ' Event Handlers for Suggestions
    Private Sub underbox_TextChanged(sender As Object, e As EventArgs) Handles underbox.TextChanged
        ShowSuggestionsForTextBox(underbox)
    End Sub

    Private Sub Searchtxt_TextChanged(sender As Object, e As EventArgs) Handles Searchtxt.TextChanged
        ShowSuggestionsForTextBox(Searchtxt)
    End Sub

    Private Sub ListBox1_Click(sender As Object, e As EventArgs)
        If ListBox1.SelectedItem IsNot Nothing AndAlso activeTextbox IsNot Nothing Then
            activeTextbox.Text = DirectCast(ListBox1.SelectedItem, DataRowView)(0).ToString()
            ListBox1.Visible = False
            activeTextbox.Focus()
        End If
    End Sub

    Private Sub ListBox1_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then ListBox1_Click(sender, e)
        If e.KeyCode = Keys.Escape Then ListBox1.Visible = False
    End Sub

    ' Focus management to hide listbox
    Private Sub TextBox_LostFocus(sender As Object, e As EventArgs) Handles Searchtxt.LostFocus, underbox.LostFocus
        ' Delay hiding so the click on ListBox can register
        TimerHideList.Start()
    End Sub

    Private Sub TimerHideList_Tick(sender As Object, e As EventArgs) Handles TimerHideList.Tick
        If Not ListBox1.Focused Then ListBox1.Visible = False
        TimerHideList.Stop()
    End Sub

End Class
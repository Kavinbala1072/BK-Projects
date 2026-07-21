Imports System.Data.SqlClient
Imports System.Xml
Imports System.IO
Imports Guna.UI2.WinForms
Public Class Jobcard

    Dim JobcardDate As String
    Private dtname, dtnoteprocess, dtnoteSize, dtnoteType As DataTable
    Private activeTextbox As Guna.UI2.WinForms.Guna2TextBox
    Public AlterBillNo As String = ""
    Private EditingRowID As String = ""
    Private FinStartDate As DateTime
    Private FinEndDate As DateTime
    Private isPopulatingText As Boolean = False

    Private Sub Jobcard_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        ManualBillNo.Focus()
    End Sub
    Private Sub Jobcard_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        JobcardDate = DateTime.Now.ToString("dd/MM/yyyy")
        JCDate.Text = JobcardDate
        BillNoTxt.Text = GetNextBillNumber()
        BillNoTxt.ReadOnly = True

        Tools.LoadConfiguration()
        InitializeDataGridView()
        LoadAutoCompleteData()
        Themeload()
        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me

        AddonsButton.Visible = False

        Status.Items.Clear()
        Status.Items.Add("PENDING")
        Status.Items.Add("COMPLETED")
        Status.StartIndex = 0
        LoadFinancialPeriod()
        If Not String.IsNullOrEmpty(AlterBillNo) Then
            LoadJobCardEntry(AlterBillNo)
        End If
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

    Private Sub Guna2DataGridView1_KeyDown(sender As Object, e As KeyEventArgs) Handles Guna2DataGridView1.KeyDown

        If e.Control AndAlso e.KeyCode = Keys.Y Then

            If Guna2DataGridView1.SelectedRows.Count > 0 Then
                For Each selectedRow As DataGridViewRow In Guna2DataGridView1.SelectedRows
                    Guna2DataGridView1.Rows.Remove(selectedRow)
                Next
            Else
                MessageBox.Show("Please select a row to remove.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        End If
    End Sub
    Private Function GetNextBillNumber() As String

        Dim query As String = "SELECT ISNULL(Vt_Prefix, '') + CAST(ISNULL(Vt_Billno, 0) AS VARCHAR) + ISNULL(Vt_Suffix, '') AS Vt_FullBillNo FROM v_table WHERE Vt_Name = 'JobCard'"

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
            ' --- 1. Basic Setup ---
            .ColumnHeadersVisible = True
            .AllowUserToAddRows = False
            .AllowUserToOrderColumns = True
            .AllowUserToResizeColumns = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .ScrollBars = ScrollBars.Both

            .Columns.Clear()
            .Columns.Add("Manual Bill No", "Manual Bill No")
            .Columns.Add("Note Processing Method", "Note Processing Method")
            .Columns.Add("Note Size", "Note Size")
            .Columns.Add("Note Type", "Note Type")
            .Columns.Add("Paper Brand", "Paper Brand")
            .Columns.Add("Paper Size", "Paper Size")
            .Columns.Add("Paper GSM", "Paper GSM")
            .Columns.Add("Paper Weight", "Paper Weight")
            .Columns.Add("No. of Sheet", "No. of Sheet")
            .Columns.Add("Index", "Index")
            .Columns.Add("Wrapper", "Wrapper")
            .Columns.Add("No. of Page", "No. of Page")
            .Columns.Add("No. of Note", "No. of Note")
            .Columns.Add("No. of Reem", "No. of Reem")
            .Columns.Add("JobCard Details", "JobCard Details")

            .Columns.Add("ID", "ID")
            .Columns("ID").Visible = False

            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

            .ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False

            For Each col As DataGridViewColumn In .Columns
                col.ReadOnly = True
                col.MinimumWidth = 60
            Next

            If .Columns.Contains("JobCard Details") Then
                .Columns("JobCard Details").MinimumWidth = 200
            End If

            .AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersHeight = 40

            Dim headerColor As Color = Color.FromArgb(34, 40, 49)
            Try
                Using sqlconnect As SqlConnection = Tools.GetConnection()
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
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading HeaderColor: " & ex.Message)
            End Try

            .ColumnHeadersDefaultCellStyle.BackColor = headerColor
            .ThemeStyle.HeaderStyle.BackColor = headerColor
        End With
    End Sub
    Private Sub Addbtn_Click(sender As Object, e As EventArgs) Handles Addbtn.Click

        'String.IsNullOrWhiteSpace(Partyname.Text) OrElse
        'String.IsNullOrWhiteSpace(FinishingTxt.Text) OrElse
        If String.IsNullOrWhiteSpace(NoteProcessingtxt.Text) OrElse
        String.IsNullOrWhiteSpace(PaperSizetxt.Text) OrElse
        String.IsNullOrWhiteSpace(NoteSizetxt.Text) OrElse
        String.IsNullOrWhiteSpace(NoteTypetxt.Text) OrElse
        String.IsNullOrWhiteSpace(Sheettxt.Text) OrElse
        String.IsNullOrWhiteSpace(Pagetxt.Text) OrElse
        String.IsNullOrWhiteSpace(Reemtxt.Text) OrElse
        String.IsNullOrWhiteSpace(NoteTxt.Text) OrElse
            String.IsNullOrWhiteSpace(ManualBillNo.Text) Then
            MessageBox.Show("Please fill in all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim rowIndex As Integer = Guna2DataGridView1.Rows.Add()
        Dim newRow As DataGridViewRow = Guna2DataGridView1.Rows(rowIndex)
        Dim Note As Integer
        If Not Integer.TryParse(NoteTxt.Text, Note) Then
            MessageBox.Show("Please enter a valid number for 'No. of Note'.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error)
            NoteTxt.Focus()
            Return
        End If

        newRow.Cells("Manual Bill No").Value = ManualBillNo.Text
        newRow.Cells("Note Processing Method").Value = NoteProcessingtxt.Text
        newRow.Cells("Note Size").Value = NoteSizetxt.Text
        newRow.Cells("Note Type").Value = NoteTypetxt.Text
        newRow.Cells("Paper Brand").Value = PaperBrandtxt.Text
        newRow.Cells("Paper Size").Value = PaperSizetxt.Text
        newRow.Cells("Paper GSM").Value = PaperGSMtxt.Text
        newRow.Cells("Paper Weight").Value = PaperWeighttxt.Text
        newRow.Cells("No. of Sheet").Value = Sheettxt.Text
        newRow.Cells("Index").Value = Indextxt.Text
        newRow.Cells("Wrapper").Value = Wrappertxt.Text
        newRow.Cells("No. of Page").Value = Pagetxt.Text
        newRow.Cells("No. of Reem").Value = Reemtxt.Text
        newRow.Cells("No. of Note").Value = Note
        newRow.Cells("JobCard Details").Value = JCRemarktxt.Text

        newRow.Cells("ID").Value = Guid.NewGuid().ToString()

        ClearInputs()
        NoteProcessingtxt.Focus()
        ManualBillNo.Focus()
    End Sub
    Private Sub ClearInputs()
        NoteProcessingtxt.Clear()
        PaperSizetxt.Clear()
        NoteSizetxt.Clear()
        NoteTypetxt.Clear()
        PaperBrandtxt.Clear()
        PaperSizetxt.Clear()
        PaperGSMtxt.Clear()
        PaperWeighttxt.Clear()
        Indextxt.Clear()
        Wrappertxt.Clear()
        Sheettxt.Clear()
        Pagetxt.Clear()
        Reemtxt.Clear()
        JCRemarktxt.Clear()
        NoteTxt.Clear()
        ManualBillNo.Clear()
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
    Private Function GetSelectedNoteSizeId(noteSizeName As String) As Integer
        Dim query As String = "SELECT ID FROM NoteSize_Table WHERE Name = @NoteSizeName and Active = 0"
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Using sqlcommand As New SqlCommand(query, sqlconnect)
                sqlcommand.Parameters.AddWithValue("@NoteSizeName", noteSizeName)
                sqlconnect.Open()
                Dim result = sqlcommand.ExecuteScalar()
                If result IsNot Nothing Then
                    Return Convert.ToInt32(result)
                Else
                    Throw New Exception("Selected Note Size does not exist in the NoteSize_Table.")
                End If
            End Using
        End Using
    End Function
    Private Function GetSelectedNoteTypeId(noteTypeName As String) As Integer
        If String.IsNullOrWhiteSpace(noteTypeName) Then
            Throw New Exception("NoteTypeName is empty or null.")
        End If

        noteTypeName = noteTypeName.Trim()
        Dim query As String = "SELECT ID FROM NoteType_table WHERE Name = @NoteTypeName and Active = 0"
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Using sqlcommand As New SqlCommand(query, sqlconnect)
                sqlcommand.Parameters.AddWithValue("@NoteTypeName", noteTypeName)
                sqlconnect.Open()
                Dim result = sqlcommand.ExecuteScalar()
                If result IsNot Nothing Then
                    Return Convert.ToInt32(result)
                Else
                    Throw New Exception("Selected Note Type does not exist in the NoteType_table.")
                End If
            End Using
        End Using
    End Function
    Private Function GetSelectedNoteProcessingId(noteProcessingName As String) As Integer
        Dim query As String = "SELECT ID FROM NoteProcessing_Table WHERE Name = @NoteProcessingName and Active = 0"
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Using sqlcommand As New SqlCommand(query, sqlconnect)
                sqlcommand.Parameters.AddWithValue("@NoteProcessingName", noteProcessingName)
                sqlconnect.Open()
                Dim result = sqlcommand.ExecuteScalar()
                If result IsNot Nothing Then
                    Return Convert.ToInt32(result)
                Else
                    Throw New Exception("Selected Note Processing does not exist in the NoteProcessing_Table.")
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

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        Dim transaction As SqlTransaction = Nothing
        Dim transactionCompleted As Boolean = False
        Dim JobcardDate As DateTime = JCDate.Text
        Dim Name As String = Partyname.Text.Trim()
        Dim parsedDate As DateTime

        Try
            parsedDate = DateTime.ParseExact(JCDate.Text, "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture)
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

        If String.IsNullOrWhiteSpace(Name) Then
            MessageBox.Show("Please enter a party name.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Guna2DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("Please add at least one row to save.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Status.SelectedIndex = -1 OrElse String.IsNullOrWhiteSpace(Status.Text) Then
            MessageBox.Show("Please select a status.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If MessageBox.Show("Do you want to save?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then Exit Sub

        Dim isUpdate As Boolean = Not String.IsNullOrWhiteSpace(AlterBillNo)
        Dim BillNo As String = If(isUpdate, AlterBillNo, GetNextBillNumber())
        Dim finish As Integer = 0
        Dim StatusVal As String = Status.Text.Trim()
        Dim UserId As Integer = Tools.GetStoredUsername()

        Try
            Dim ledgerId As Integer = GetSelectedLedgerId()
            Dim originalIds As New HashSet(Of Guid)
            Dim currentIds As New HashSet(Of Guid)

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()
                transaction = sqlconnect.BeginTransaction()

                If isUpdate Then
                    Using cmd As New SqlCommand("SELECT ID FROM JobCard_table WHERE Bill_No = @Bill_No", sqlconnect, transaction)
                        cmd.Parameters.AddWithValue("@Bill_No", BillNo)
                        Using reader = cmd.ExecuteReader()
                            While reader.Read()
                                originalIds.Add(Guid.Parse(reader("ID").ToString()))
                            End While
                        End Using
                    End Using
                End If

                For Each Row As DataGridViewRow In Guna2DataGridView1.Rows
                    If Row.IsNewRow Then Continue For

                    Dim idVal = Row.Cells("ID").Value
                    Dim rowId As Guid = If(idVal IsNot Nothing AndAlso Guid.TryParse(idVal.ToString(), Nothing),
                                   Guid.Parse(idVal.ToString()), Guid.NewGuid())

                    Dim NoteProcessing As String = Row.Cells("Note Processing Method").Value?.ToString()
                    Dim NoteSize As String = Row.Cells("Note Size").Value?.ToString()
                    Dim NoteType As String = Row.Cells("Note Type").Value?.ToString()
                    Dim Paper As String = Row.Cells("Paper Size").Value?.ToString()
                    Dim Sheet As String = Row.Cells("No. of Sheet").Value?.ToString()
                    Dim Page As String = Row.Cells("No. of Page").Value?.ToString()
                    Dim Note As String = Row.Cells("No. of Note").Value?.ToString()
                    Dim Reem As String = Row.Cells("No. of Reem").Value?.ToString()
                    Dim Remarks As String = Row.Cells("JobCard Details").Value?.ToString()
                    Dim Index As String = Row.Cells("Index").Value?.ToString()
                    Dim ManualBillNoVal As String = Row.Cells("Manual Bill No").Value?.ToString()

                    Dim noteProcessingId As Integer = GetSelectedNoteProcessingId(NoteProcessing)
                    Dim noteSizeId As Integer = GetSelectedNoteSizeId(NoteSize)
                    Dim noteTypeId As Integer = GetSelectedNoteTypeId(NoteType)

                    currentIds.Add(rowId)

                    Dim commandText As String = If(isUpdate,
                    "IF EXISTS (SELECT 1 FROM JobCard_table WHERE Bill_No = @Bill_No AND ID = @ID)
                    BEGIN 
                        UPDATE JobCard_table SET 
                            JobCard_date = @JobCard_date, ledger_id = @ledger_id, Partyname = @Partyname,
                            NoteProcessing_Id = @NoteProcessing_Id, Note_Processing = @Note_Processing, 
                            NoteSize_Id = @NoteSize_Id, Note_Size = @Note_Size, NoteType_Id = @NoteType_Id, Paper_Size = @Paper, 
                            Sheet = @Sheet, Pages = @Pages, Note = @Note, Reem = @Reem, Finishing = @Finishing,
                            Manual_BillNo = @Manual_BillNo, WorkingStatus = @WorkingStatus, 
                            Paper_Brand = @Paper_Brand, Paper_GSM = @Paper_GSM, Paper_Weight = @Paper_Weight,
                            No_Index = @No_Index, Wrapper = @Wrapper, Remarks = @Remarks, UserID = @UserID
                        WHERE Bill_No = @Bill_No AND ID = @ID
                    END ELSE
                    BEGIN 
                        INSERT INTO JobCard_table (
                            ID, Bill_No, JobCard_date, ledger_id, Partyname, 
                            NoteProcessing_Id, Note_Processing, NoteSize_Id, Note_Size, NoteType_Id,
                            Paper_Size, Sheet, Pages, Note, Reem, Finishing,
                            Manual_BillNo, WorkingStatus,
                            Paper_Brand, Paper_GSM, Paper_Weight, No_Index, Wrapper, Remarks, UserID
                        ) VALUES (
                            @ID, @Bill_No, @JobCard_date, @ledger_id, @Partyname,
                            @NoteProcessing_Id, @Note_Processing, @NoteSize_Id, @Note_Size, @NoteType_Id,
                            @Paper, @Sheet, @Pages, @Note, @Reem, @Finishing,
                            @Manual_BillNo, @WorkingStatus,
                            @Paper_Brand, @Paper_GSM, @Paper_Weight, @No_Index, @Wrapper, @Remarks, @UserID
                        )
                    END",
                    "INSERT INTO JobCard_table (
                        ID, Bill_No, JobCard_date, ledger_id, Partyname,
                        NoteProcessing_Id, Note_Processing, NoteSize_Id, Note_Size, NoteType_Id,
                        Paper_Size, Sheet, Pages, Note, Reem, Finishing,
                        Manual_BillNo, WorkingStatus,
                        Paper_Brand, Paper_GSM, Paper_Weight, No_Index, Wrapper, Remarks, UserID
                    ) VALUES (
                        @ID, @Bill_No, @JobCard_date, @ledger_id, @Partyname,
                        @NoteProcessing_Id, @Note_Processing, @NoteSize_Id, @Note_Size, @NoteType_Id,
                        @Paper, @Sheet, @Pages, @Note, @Reem, @Finishing,
                        @Manual_BillNo, @WorkingStatus,
                        @Paper_Brand, @Paper_GSM, @Paper_Weight, @No_Index, @Wrapper, @Remarks, @UserID
                    )")


                    Using cmd As New SqlCommand(commandText, sqlconnect, transaction)
                        With cmd.Parameters
                            .AddWithValue("@ID", rowId)
                            .AddWithValue("@Bill_No", BillNo)
                            .AddWithValue("@JobCard_date", JobcardDate)
                            .AddWithValue("@ledger_id", ledgerId)
                            .AddWithValue("@Partyname", Name)
                            .AddWithValue("@NoteProcessing_Id", noteProcessingId)
                            .AddWithValue("@Note_Processing", NoteProcessing)
                            .AddWithValue("@NoteSize_Id", noteSizeId)
                            .AddWithValue("@Note_Size", NoteSize)
                            .AddWithValue("@NoteType_Id", noteTypeId)
                            .AddWithValue("@Paper", Paper)
                            .AddWithValue("@Sheet", Sheet)
                            .AddWithValue("@Pages", Page)
                            .AddWithValue("@Note", Note)
                            .AddWithValue("@Reem", Reem)
                            .AddWithValue("@Finishing", finish)
                            .AddWithValue("@Manual_BillNo", ManualBillNoVal)
                            .AddWithValue("@WorkingStatus", StatusVal)
                            .AddWithValue("@Paper_Brand", Row.Cells("Paper Brand").Value?.ToString())
                            .AddWithValue("@Paper_GSM", Row.Cells("Paper GSM").Value?.ToString())
                            .AddWithValue("@Paper_Weight", Row.Cells("Paper Weight").Value?.ToString())
                            .AddWithValue("@No_Index", Index)
                            .AddWithValue("@Wrapper", Row.Cells("Wrapper").Value?.ToString())
                            .AddWithValue("@Remarks", Row.Cells("JobCard Details").Value?.ToString())
                            .AddWithValue("@UserID", UserId)
                        End With
                        cmd.ExecuteNonQuery()
                    End Using

                    If idVal Is Nothing OrElse String.IsNullOrWhiteSpace(idVal.ToString()) Then
                        Row.Cells("ID").Value = rowId
                    End If
                Next

                For Each deletedId In originalIds.Except(currentIds)
                    Using delCmd As New SqlCommand("DELETE FROM JobCard_table WHERE Bill_No = @Bill_No AND ID = @ID", sqlconnect, transaction)
                        delCmd.Parameters.AddWithValue("@Bill_No", BillNo)
                        delCmd.Parameters.AddWithValue("@ID", deletedId)
                        delCmd.ExecuteNonQuery()
                    End Using
                Next

                If Not isUpdate Then
                    Using updateVtBillNo As New SqlCommand("UPDATE v_table SET Vt_Billno = Vt_Billno + 1 WHERE Vt_Name = 'JobCard'", sqlconnect, transaction)
                        updateVtBillNo.ExecuteNonQuery()
                    End Using
                End If

                transaction.Commit()
                transactionCompleted = True
                MessageBox.Show("Saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ClearAllInputs()
                Guna2DataGridView1.Rows.Clear()
                Status.SelectedIndex = -1
            End Using
            ManualBillNo.Focus()
        Catch ex As Exception
            If transaction IsNot Nothing AndAlso transaction.Connection IsNot Nothing Then
                Try
                    If Not transactionCompleted Then transaction.Rollback()
                Catch rollbackEx As Exception
                    MessageBox.Show("Rollback failed: " & rollbackEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
            MessageBox.Show("Save error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub ClearAllInputs()
        NoteProcessingtxt.Clear()
        PaperSizetxt.Clear()
        NoteSizetxt.Clear()
        Sheettxt.Clear()
        Pagetxt.Clear()
        Reemtxt.Clear()
        NoteTxt.Clear()
        JCRemarktxt.Clear()
        'FinishingTxt.Clear()
        ManualBillNo.Clear()
        Partyname.Clear()
        PaperBrandtxt.Clear()
        PaperGSMtxt.Clear()
        PaperWeighttxt.Clear()
        Indextxt.Clear()
        Wrappertxt.Clear()
    End Sub
    Private Sub AddonsButton_Click(sender As Object, e As EventArgs) Handles AddonsButton.Click
        For Each f As Form In Application.OpenForms
            If TypeOf f Is JCProcessing Then
                f.BringToFront()
                f.Focus()
                Return
            End If
        Next

        Dim JCP As New JCProcessing()
        JCP.Show()
    End Sub
    Public Sub LoadJobCardEntry(ByVal billNo As String)
        Try
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()

                Using cmd As New SqlCommand("SELECT TOP 1 JobCard_date, ledger_id, Partyname, Manual_BillNo, WorkingStatus, Bill_No FROM JobCard_table WHERE Bill_No = @BillNo", sqlconnect)

                    cmd.Parameters.AddWithValue("@BillNo", billNo)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            JCDate.Text = Convert.ToDateTime(reader("JobCard_date")).ToString("dd/MM/yyyy")
                            BillNoTxt.Text = reader("Bill_No").ToString()
                            Partyname.Text = reader("Partyname").ToString()
                            Status.Text = reader("WorkingStatus").ToString()
                        End If
                    End Using
                End Using

                Guna2DataGridView1.Rows.Clear()

                Using cmd As New SqlCommand("SELECT  j.ID, j.Note_Processing, j.Note_Size, j.NoteType_Id, nt.Name AS NoteType_Name,  j.Paper_Size, j.Sheet, j.Pages, j.Note, j.Reem, j.Manual_BillNo, j.No_Index, 
                    j.Paper_Brand, j.Paper_GSM, j.Paper_Weight, j.Wrapper , j.Remarks FROM JobCard_table j LEFT JOIN NoteType_table nt ON j.NoteType_Id = nt.Id WHERE j.Bill_No = @BillNo and j.Cancel = 0", sqlconnect)

                    cmd.Parameters.AddWithValue("@BillNo", billNo)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Guna2DataGridView1.Rows.Add(
                            reader("Manual_BillNo").ToString(),
                            reader("Note_Processing").ToString(),
                            reader("Note_Size").ToString(),
                            reader("NoteType_Name").ToString(),
                            reader("Paper_Brand").ToString(),
                            reader("Paper_Size").ToString(),
                            reader("Paper_GSM").ToString(),
                            reader("Paper_Weight").ToString(),
                            reader("Sheet").ToString(),
                            reader("No_Index").ToString(),
                            reader("Wrapper").ToString(),
                            reader("Pages").ToString(),
                            reader("Note").ToString(),
                            reader("Reem").ToString(),
                            reader("Remarks").ToString(),
                            reader("ID").ToString()
                        )
                        End While
                    End Using
                End Using

                AlterBillNo = billNo
            End Using
            KryptonListBox.Visible = False
        Catch ex As Exception
            MessageBox.Show("Error loading Job Card entry: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub Guna2DataGridView1_DoubleClick(sender As Object, e As EventArgs) _
        Handles Guna2DataGridView1.DoubleClick

        If Guna2DataGridView1.SelectedRows.Count <> 1 Then Exit Sub

        Dim row As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)
        EditingRowID = row.Cells("ID").Value?.ToString()

        If String.IsNullOrWhiteSpace(EditingRowID) Then Exit Sub

        Dim isCompleted As Boolean = False

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()
            Using cmd As New SqlCommand("SELECT WorkingStatus FROM JobCard_table WHERE ID = @ID", sqlconnect)
                cmd.Parameters.AddWithValue("@ID", EditingRowID)
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                    isCompleted = result.ToString().
                        Trim().
                        Equals("Completed", StringComparison.OrdinalIgnoreCase)
                End If
            End Using
        End Using

        If isCompleted Then
            MessageBox.Show("This Job Card is completed and cannot be altered.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        ManualBillNo.Text = row.Cells("Manual Bill No").Value?.ToString()
        NoteProcessingtxt.Text = row.Cells("Note Processing Method").Value?.ToString()
        NoteSizetxt.Text = row.Cells("Note Size").Value?.ToString()
        NoteTypetxt.Text = row.Cells("Note Type").Value?.ToString()
        PaperSizetxt.Text = row.Cells("Paper Size").Value?.ToString()
        PaperBrandtxt.Text = row.Cells("Paper Brand").Value?.ToString()
        PaperGSMtxt.Text = row.Cells("Paper GSM").Value?.ToString()
        PaperWeighttxt.Text = row.Cells("Paper Weight").Value?.ToString()
        Sheettxt.Text = row.Cells("No. of Sheet").Value?.ToString()
        Pagetxt.Text = row.Cells("No. of Page").Value?.ToString()
        NoteTxt.Text = row.Cells("No. of Note").Value?.ToString()
        Reemtxt.Text = row.Cells("No. of Reem").Value?.ToString()
        Indextxt.Text = row.Cells("Index").Value?.ToString()
        Wrappertxt.Text = row.Cells("Wrapper").Value?.ToString()
        JCRemarktxt.Text = row.Cells("JobCard Details").Value?.ToString()

        Guna2DataGridView1.Rows.Remove(row)

        SaveButton.Text = "Update"
        ManualBillNo.Focus()
        KryptonListBox.Visible = False
    End Sub


    Private Sub LoadAutoCompleteData()
            dtname = LoadDataTable("SELECT Partyname FROM Ledger_Table WHERE Active = 0 ORDER BY Partyname ASC")
            dtnoteprocess = LoadDataTable("SELECT Name FROM NoteProcessing_Table WHERE Active = 0 ORDER BY Name ASC")
            dtnoteSize = LoadDataTable("SELECT Name FROM NoteSize_Table WHERE Active = 0 ORDER BY Name ASC")
            dtnoteType = LoadDataTable("SELECT Name FROM NoteType_table WHERE Active = 0 ORDER BY Name ASC")
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

    Private Sub ValidateTyping(sender As Object, e As KeyPressEventArgs) Handles _
        Partyname.KeyPress, NoteProcessingtxt.KeyPress, NoteSizetxt.KeyPress, NoteTypetxt.KeyPress

            If Char.IsControl(e.KeyChar) Then Exit Sub
            Dim tb As Guna2TextBox = CType(sender, Guna2TextBox)

        Dim predictedText As String = tb.Text.Remove(tb.SelectionStart, tb.SelectionLength).Insert(tb.SelectionStart, e.KeyChar.ToString())

        Dim source As DataTable = GetSourceForTextBox(tb)

        If source IsNot Nothing Then
            Dim exists As Boolean = source.AsEnumerable().Any(Function(row) row.Field(Of String)(0).StartsWith(predictedText, StringComparison.OrdinalIgnoreCase))
            If Not exists Then e.Handled = True
        End If
    End Sub

    Private Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles _
        Partyname.TextChanged, NoteProcessingtxt.TextChanged, NoteSizetxt.TextChanged, NoteTypetxt.TextChanged

            If isPopulatingText Then Exit Sub
            ShowSuggestionsForTextBox(CType(sender, Guna2TextBox))
        End Sub

        Private Sub ShowSuggestionsForTextBox(textBox As Guna2TextBox)
            activeTextbox = textBox
            Dim source As DataTable = GetSourceForTextBox(textBox)

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

            ' Position correctly relative to Form
            Dim screenPos = textBox.Parent.PointToScreen(textBox.Location)
            Dim clientPos = Me.PointToClient(screenPos)
            KryptonListBox.Location = New Point(clientPos.X, clientPos.Y + textBox.Height)
            KryptonListBox.Width = textBox.Width
            KryptonListBox.Visible = True
            KryptonListBox.BringToFront()

            If KryptonListBox.Items.Count > 0 Then KryptonListBox.SelectedIndex = 0
        End Sub

    Private Function GetSourceForTextBox(tb As Guna2TextBox) As DataTable
        If tb Is Partyname Then Return dtname
        If tb Is NoteProcessingtxt Then Return dtnoteprocess
        If tb Is NoteSizetxt Then Return dtnoteSize
        If tb Is NoteTypetxt Then Return dtnoteType
        Return Nothing
    End Function

    Private Sub KryptonListBox_SelectionMade()
        If KryptonListBox.SelectedItem IsNot Nothing AndAlso activeTextbox IsNot Nothing Then
            isPopulatingText = True
            Dim rowView As DataRowView = DirectCast(KryptonListBox.SelectedItem, DataRowView)
            activeTextbox.Text = rowView(0).ToString()
            KryptonListBox.Visible = False

            activeTextbox.Focus()
            activeTextbox.SelectionStart = activeTextbox.Text.Length
            isPopulatingText = False

            Me.SelectNextControl(activeTextbox, True, True, True, True)
        End If
    End Sub

    Private Sub TextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles _
        Partyname.KeyDown, NoteProcessingtxt.KeyDown, NoteSizetxt.KeyDown, NoteTypetxt.KeyDown

        If e.KeyCode = Keys.F2 Then
            e.Handled = True
            ShowAllSuggestions(CType(sender, Guna2TextBox))
            Exit Sub
        End If

        If KryptonListBox.Visible Then
                If e.KeyCode = Keys.Down Then
                    KryptonListBox.Focus()
                    e.Handled = True
                ElseIf e.KeyCode = Keys.Enter Then
                    e.SuppressKeyPress = True
                    KryptonListBox_SelectionMade()
                End If
            ElseIf e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                Me.SelectNextControl(CType(sender, Control), True, True, True, True)
            End If
        End Sub
    Private Sub ShowAllSuggestions(textBox As Guna2TextBox)
        activeTextbox = textBox
        Dim source As DataTable = GetSourceForTextBox(textBox)

        If source Is Nothing Then
            KryptonListBox.Visible = False
            Exit Sub
        End If

        Dim dv As New DataView(source)
        Dim columnName As String = source.Columns(0).ColumnName

        KryptonListBox.DataSource = dv
        KryptonListBox.DisplayMember = columnName

        Dim screenPos = textBox.Parent.PointToScreen(textBox.Location)
        Dim clientPos = Me.PointToClient(screenPos)
        KryptonListBox.Location = New Point(clientPos.X, clientPos.Y + textBox.Height)
        KryptonListBox.Width = textBox.Width
        KryptonListBox.Visible = True
        KryptonListBox.BringToFront()

        If KryptonListBox.Items.Count > 0 Then
            KryptonListBox.Focus()
            KryptonListBox.SelectedIndex = 0
        End If
    End Sub
    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) Handles _
        ManualBillNo.KeyDown, PaperBrandtxt.KeyDown, PaperSizetxt.KeyDown, PaperGSMtxt.KeyDown,
        PaperWeighttxt.KeyDown, Sheettxt.KeyDown, Indextxt.KeyDown, Wrappertxt.KeyDown,
        Pagetxt.KeyDown, NoteTxt.KeyDown, Reemtxt.KeyDown, JCRemarktxt.KeyDown, Addbtn.KeyDown

            If e.KeyCode = Keys.Enter Then
                e.SuppressKeyPress = True
                Me.SelectNextControl(CType(sender, Control), True, True, True, True)
            End If
        End Sub

    Private Sub KryptonListBox_Click(sender As Object, e As EventArgs) Handles KryptonListBox.Click
            KryptonListBox_SelectionMade()
        End Sub

    Private Sub KryptonListBox_KeyDown(sender As Object, e As KeyEventArgs) Handles KryptonListBox.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            KryptonListBox_SelectionMade()
        ElseIf e.KeyCode = Keys.Escape Then
            KryptonListBox.Visible = False
            activeTextbox?.Focus()
        End If
    End Sub
    Private Sub TextBox_Interacted(sender As Object, e As EventArgs) Handles _
        Partyname.Click, NoteSizetxt.Click, NoteProcessingtxt.Click, NoteTypetxt.Click,
        Partyname.GotFocus, NoteSizetxt.GotFocus, NoteProcessingtxt.GotFocus, NoteTypetxt.GotFocus
            ShowSuggestionsForTextBox(CType(sender, Guna2TextBox))
        End Sub

        Private Sub Global_LostFocus(sender As Object, e As EventArgs) Handles _
        Partyname.LostFocus, NoteSizetxt.LostFocus, NoteProcessingtxt.LostFocus, NoteTypetxt.LostFocus
            TimerFocusCheck.Start()
        End Sub

        Private Sub TimerFocusCheck_Tick(sender As Object, e As EventArgs) Handles TimerFocusCheck.Tick
        TimerFocusCheck.Stop()
        If Not KryptonListBox.Focused AndAlso (activeTextbox IsNot Nothing AndAlso Not activeTextbox.Focused) Then
                KryptonListBox.Visible = False

            If activeTextbox IsNot Nothing Then
                    Dim src = GetSourceForTextBox(activeTextbox)
                    If Not IsValidExact(activeTextbox.Text, src) Then activeTextbox.Clear()
                End If
            End If
        End Sub

        Private Function IsValidExact(txt As String, source As DataTable) As Boolean
            If String.IsNullOrWhiteSpace(txt) Then Return True
            Return source.AsEnumerable().Any(Function(row) row.Field(Of String)(0).Equals(txt, StringComparison.OrdinalIgnoreCase))
        End Function

    End Class
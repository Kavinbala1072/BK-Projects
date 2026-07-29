Imports ComponentFactory.Krypton.Navigator
Imports System.Data.SqlClient
Imports Guna.UI2.WinForms
Imports System.Windows.Forms
Imports System.IO
Imports Newtonsoft.Json
Imports System.Net.Http

Public Class MainForm
    Dim iscollapsed As Boolean = False
    Dim iscollapsed2 As Boolean = False
    Dim iscollapsed3 As Boolean = False
    Dim iscollapsed4 As Boolean = False
    Dim iscollapsed5 As Boolean = False

    Private Sub Stock_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Guna2DateTimePicker1.Value = DateTime.Now
        Guna2DateTimePicker1.Format = DateTimePickerFormat.Custom
        Guna2DateTimePicker1.CustomFormat = "dd/MM/yyyy"

        Me.KeyPreview = True
        DropPanel.Size = DropPanel.MinimumSize
        Optionpanel.Size = Optionpanel.MaximumSize

        KryptonDockableNavigator1.Visible = False
        LoadCompanyName()
        Dim workingArea As Rectangle = Screen.PrimaryScreen.WorkingArea
        Me.Location = workingArea.Location
        Me.Size = workingArea.Size
        Dim Versiontxt As String = lblversion.Text
        lblversion.Text = "Version " + Form1.Version
        lbluser.Text = GetStoredUsername()
        userrights()
        ushowlogout()
        TimerBackup.Start()
        'OpenBackup()
        'CheckBillingUpdate()
    End Sub

    'Private Sub CheckBillingUpdate()
    '    Dim username As String = GetStoredUsername()
    '    Dim today As Date = Date.Today

    '    Dim financialYearStart As Date
    '    Dim financialYearEnd As Date

    '    If today.Month >= 4 Then
    '        financialYearStart = New Date(today.Year, 4, 1)
    '        financialYearEnd = New Date(today.Year + 1, 3, 31)
    '    Else
    '        financialYearStart = New Date(today.Year - 1, 4, 1)
    '        financialYearEnd = New Date(today.Year, 3, 31)
    '    End If

    '    Using sqlconnect As SqlConnection = Tools.GetConnection()
    '        Try
    '            sqlconnect.Open()
    '            Dim cmd As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'LastNoUpdated'", sqlconnect)
    '            Dim lastUpdatedStr As Object = cmd.ExecuteScalar()

    '            If lastUpdatedStr IsNot Nothing Then
    '                Dim lastUpdated As Date
    '                Dim parsed As Boolean = Date.TryParse(lastUpdatedStr.ToString(), lastUpdated)

    '                If Not parsed Then
    '                    Dim parts() As String = lastUpdatedStr.ToString().Split("/"c)
    '                    If parts.Length = 2 Then
    '                        Dim dayPart As Integer
    '                        Dim monthPart As Integer
    '                        If Integer.TryParse(parts(0), dayPart) AndAlso Integer.TryParse(parts(1), monthPart) Then
    '                            lastUpdated = New Date(today.Year - 1, monthPart, dayPart)
    '                            parsed = True
    '                        End If
    '                    End If
    '                End If

    '                If parsed Then
    '                    If lastUpdated < financialYearStart OrElse lastUpdated > financialYearEnd Then
    '                        Dim message As String = $"Hi {username}!" & Environment.NewLine &
    '                                            "Please update your Billing Number Series for the new financial year." & Environment.NewLine &
    '                                            "Do you want to open the settings now?"

    '                        Dim result As DialogResult = MessageBox.Show(message, "Billing Update Reminder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

    '                        If result = DialogResult.Yes Then
    '                            LoadFormToKryptonNavigator(Of Setting)("Setting")
    '                        Else
    '                            Application.Exit()
    '                        End If
    '                    End If
    '                End If
    '            End If
    '        Catch ex As Exception
    '            MessageBox.Show("Error checking billing update: " & ex.Message)
    '        End Try
    '    End Using
    'End Sub

    Private Sub userrights()
        Try
            Dim username As String = GetStoredUsername()
            Dim userId As Integer = -1

            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim cmd As New SqlCommand("SELECT ID FROM User_table WHERE User_Name = @UserName", conn)
                cmd.Parameters.AddWithValue("@UserName", username)

                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    userId = Convert.ToInt32(result)
                End If
            End Using

            If userId = 1 OrElse username.Equals("Admin", StringComparison.OrdinalIgnoreCase) Then
                ShowAllButtons()
                Exit Sub
            End If

            Dim allowedMenus As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim cmd As New SqlCommand("SELECT Menu_ID FROM UserRight_Table WHERE User_ID = @ID AND IsAllowed = 1", conn)
                cmd.Parameters.AddWithValue("@ID", userId)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        allowedMenus.Add(reader("Menu_ID").ToString())
                    End While
                End Using
            End Using

            ToggleButtonAccess(allowedMenus)

        Catch ex As Exception
            Guna2Button2.Visible = False
            Debug.WriteLine("Rights Error: " & ex.Message)
        End Try
    End Sub
    Private Sub ToggleButtonAccess(allowedMenus As HashSet(Of String))
        MemberShipButton.Visible = allowedMenus.Contains("BK1")

        VoucherButton.Visible = allowedMenus.Contains("BK2")

        Dim hasReportAccess As Boolean = allowedMenus.Contains("BK6") Or
                                        allowedMenus.Contains("BK7") Or
                                        allowedMenus.Contains("BK8")

        DropButton1.Visible = hasReportAccess
        DropPanel.Visible = hasReportAccess

        MSRButtom.Visible = allowedMenus.Contains("BK7")
        CSRButton.Visible = allowedMenus.Contains("BK8")
        Guna2Button2.Visible = allowedMenus.Contains("BK9")
    End Sub
    Private Sub ShowAllButtons()
        VoucherButton.Visible = True
        MemberShipButton.Visible = True
        DropButton1.Visible = True
        DropPanel.Visible = True
        CSRButton.Visible = True
        MSRButtom.Visible = True
        Guna2Button2.Visible = True
        Logoutbtn.Visible = True
    End Sub
    Private Sub ushowlogout()
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                Dim cmd As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'UserRight'", sqlconnect)
                Dim userRightValue As String = cmd.ExecuteScalar()?.ToString().Trim()

                If userRightValue = "1" Then
                    Logoutbtn.Visible = True
                Else
                    Logoutbtn.Visible = False
                End If
            Catch ex As Exception
                Logoutbtn.Visible = True
            End Try
        End Using
    End Sub
    Public Shared Function GetStoredUsername() As String
        Dim ctlDesc As String = "UserName"
        Dim storedUsername As String = ""

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()

            Dim command As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = @CtlDesc", sqlconnect)
            command.Parameters.AddWithValue("@CtlDesc", ctlDesc)

            Dim result = command.ExecuteScalar()
            If result IsNot Nothing Then
                storedUsername = result.ToString()
            End If
        End Using

        Return storedUsername
    End Function

    Public Sub LoadFormToKryptonNavigator(Of T As {Form, New})(title As String)

        Try
            For Each page As KryptonPage In KryptonDockableNavigator1.Pages
                If page.Text = title Then
                    KryptonDockableNavigator1.SelectedPage = page

                    KryptonDockableNavigator1.Visible = True
                    Return
                End If
            Next

            Dim formInstance As New T()
            formInstance.TopLevel = False
            formInstance.FormBorderStyle = FormBorderStyle.None
            formInstance.Dock = DockStyle.Fill

            Dim newPage As New KryptonPage()
            newPage.Text = title
            newPage.Name = Guid.NewGuid().ToString()

            newPage.Controls.Add(formInstance)
            formInstance.Show()

            KryptonDockableNavigator1.Pages.Add(newPage)
            KryptonDockableNavigator1.SelectedPage = newPage

            KryptonDockableNavigator1.Visible = True

        Catch ex As Exception
            MessageBox.Show("Failed to load form: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub KryptonDockableNavigator1_PageClose(sender As Object, e As KryptonPageEventArgs) Handles KryptonDockableNavigator1.CloseAction

        KryptonDockableNavigator1.Pages.Remove(e.Item)

        If KryptonDockableNavigator1.Pages.Count = 0 Then
            KryptonDockableNavigator1.Visible = False
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If iscollapsed Then
            DropPanel.Height += 10
            If DropPanel.Size = DropPanel.MaximumSize Then
                Timer1.Stop()
                iscollapsed = True
            End If
        Else
            DropPanel.Height -= 10
            If DropPanel.Size = DropPanel.MinimumSize Then
                Timer1.Stop()
                iscollapsed = False
            End If
        End If
    End Sub
    Private Sub Timer5_Tick(sender As Object, e As EventArgs) Handles Timer5.Tick
        If iscollapsed5 Then
            Optionpanel.Width += 10
            If Optionpanel.Width >= Optionpanel.MinimumSize.Width Then
                Optionpanel.Width = Optionpanel.MinimumSize.Width
                Timer5.Stop()
                iscollapsed5 = True
                VoucherButton.Text = ""
                MemberShipButton.Text = ""
                CSRButton.Text = ""
                MSRButtom.Text = ""
                Guna2Button2.Text = ""
                DropButton1.Text = ""
            End If
        Else
            Optionpanel.Width -= 10
            If Optionpanel.Width <= Optionpanel.MaximumSize.Width Then
                Optionpanel.Width = Optionpanel.MaximumSize.Width
                Timer5.Stop()
                iscollapsed5 = False
                VoucherButton.Text = "Voucher"
                MemberShipButton.Text = "Member Ship"
                CSRButton.Text = "Transaction Report"
                MSRButtom.Text = "Member Ship"
                Guna2Button2.Text = "Settings"
                DropButton1.Text = "Report"
            End If
        End If
    End Sub

    Private Sub OptionBtn_Click(sender As Object, e As EventArgs) Handles OptionBtn.Click
        iscollapsed5 = Not iscollapsed5
        Timer5.Start()
    End Sub

    Private Sub DropButton1_Click(sender As Object, e As EventArgs) Handles DropButton1.Click
        iscollapsed = Not iscollapsed
        Timer1.Start()
    End Sub

    Private Sub DropButton2_Click(sender As Object, e As EventArgs)
        iscollapsed2 = Not iscollapsed2
        Timer2.Start()
    End Sub

    Private Sub DropButton3_Click(sender As Object, e As EventArgs)
        iscollapsed3 = Not iscollapsed3
        Timer3.Start()
    End Sub

    Private Sub DropButton4_Click(sender As Object, e As EventArgs)
        iscollapsed4 = Not iscollapsed4
        Timer4.Start()
    End Sub
    Private Sub LoadCompanyName()
        Try
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()

                Dim query As String = "SELECT TOP 1 comp_name FROM company_table"
                Using cmd As New SqlCommand(query, sqlconnect)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() AndAlso Not reader.IsDBNull(0) Then
                            CompanyLabel.Text = reader.GetString(0)
                        Else
                            CompanyLabel.Text = "BK Software"
                        End If
                    End Using
                End Using
            End Using

        Catch ex As SqlException
            MessageBox.Show("SQL Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("General Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Guna2ControlBox1_Click_1(sender As Object, e As EventArgs) Handles Guna2ControlBox1.Click
        Dim result As DialogResult = MessageBox.Show("Do you want to close the application?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then

            If My.Computer.Network.IsAvailable Then
                Try
                    Tools.UpdateCloudLog(My.Forms.Form1.Server,
                                     My.Forms.Form1.CompanyNo,
                                     "Attma",
                                     My.Forms.Form1.compName,
                                     My.Forms.Form1.LastLogin,
                                    My.Forms.Form1.amcExpiryDate,
                                     My.Forms.Form1.Version,
                                     My.Forms.Form1.secretPassword)
                Catch ex As Exception
                    Debug.WriteLine("Final Cloud Sync Failed: " & ex.Message)
                End Try
            End If

            OpenBackup()
            Application.Exit()
        End If
    End Sub
    Private Sub Logoutbtn_Click(sender As Object, e As EventArgs) Handles Logoutbtn.Click
        Dim result As DialogResult = MessageBox.Show("Do you want to Logout?", "Confirm Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.Yes Then
            Dim Login As New Form1()
            Login.Show()
            Me.Hide()
        End If
    End Sub
    Private Sub OpenBackup()
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'EnableBackup'", sqlconnect)

                Using reader As SqlDataReader = Query.ExecuteReader()
                    If reader.Read() Then
                        If reader("Ctl_Value").ToString() = "1" Then
                            TakeBackup()
                        End If
                    End If
                End Using

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub TimerBackup_Tick(sender As Object, e As EventArgs) Handles TimerBackup.Tick

        Dim backupTimes As TimeSpan() = {
        New TimeSpan(10, 0, 0),
        New TimeSpan(12, 0, 0),
        New TimeSpan(15, 0, 0),
        New TimeSpan(17, 0, 0)
    }

        Dim nowTime As TimeSpan = DateTime.Now.TimeOfDay

        For Each bt In backupTimes
            If nowTime.Hours = bt.Hours AndAlso nowTime.Minutes = bt.Minutes Then
                TakeBackup()
                Exit For
            End If
        Next
    End Sub

    Private Sub TakeBackup()
        Dim backupDir As String = Tools.GetBackupPath()
        If Not Directory.Exists(backupDir) Then
            Directory.CreateDirectory(backupDir)
        End If

        Dim dbName As String = Tools.GetDatabaseName()
        Dim timestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")
        Dim backupFile As String = Path.Combine(backupDir, $"{dbName}_Backup_{timestamp}.bak")

        Dim status As String = "Success"
        Dim errorMsg As String = ""

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()

                Using backupCmd As New SqlCommand($"BACKUP DATABASE [{dbName}] TO DISK = '{backupFile}' WITH INIT", conn)
                    backupCmd.ExecuteNonQuery()
                End Using

                Using logCmd As New SqlCommand(" MERGE AutoBackup_table AS target
                USING (SELECT CAST(GETDATE() AS DATE) AS BackupDate) AS source
                ON target.BackupDate = source.BackupDate
                WHEN MATCHED THEN
                    UPDATE SET LastRunTime = GETDATE(), BackupStatus = @Status, BackupFile = @File, ErrorMessage = @Error
                WHEN NOT MATCHED THEN
                    INSERT (BackupDate, LastRunTime, BackupStatus, BackupFile, ErrorMessage)
                    VALUES (CAST(GETDATE() AS DATE), GETDATE(), @Status, @File, @Error);", conn)

                    logCmd.Parameters.AddWithValue("@Status", status)
                    logCmd.Parameters.AddWithValue("@File", backupFile)
                    logCmd.Parameters.AddWithValue("@Error", errorMsg)
                    logCmd.ExecuteNonQuery()
                End Using

            End Using

        Catch ex As Exception
            status = "Failed"
            errorMsg = ex.Message

            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Using logCmd As New SqlCommand("
                MERGE AutoBackup_table AS target
                USING (SELECT CAST(GETDATE() AS DATE) AS BackupDate) AS source
                ON target.BackupDate = source.BackupDate
                WHEN MATCHED THEN
                    UPDATE SET LastRunTime = GETDATE(), BackupStatus = @Status, ErrorMessage = @Error
                WHEN NOT MATCHED THEN
                    INSERT (BackupDate, LastRunTime, BackupStatus, ErrorMessage) VALUES (CAST(GETDATE() AS DATE), GETDATE(), @Status, @Error);", conn)

                    logCmd.Parameters.AddWithValue("@Status", status)
                    logCmd.Parameters.AddWithValue("@Error", errorMsg)
                    logCmd.ExecuteNonQuery()
                End Using
            End Using
        End Try
    End Sub
    Private Sub MemberShipButton_Click(sender As Object, e As EventArgs) Handles MemberShipButton.Click
        LoadFormToKryptonNavigator(Of MemberShip)("Member Ship")
    End Sub
    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        LoadFormToKryptonNavigator(Of Setting)("Setting")
    End Sub
    Private Sub VoucherButton_Click(sender As Object, e As EventArgs) Handles VoucherButton.Click
        LoadFormToKryptonNavigator(Of Voucher)("Voucher")
    End Sub

    Private Sub MSRButtom_Click(sender As Object, e As EventArgs) Handles MSRButtom.Click
        LoadFormToKryptonNavigator(Of MSReport)("Member Ship Report")
    End Sub

    Private Sub CSRButton_Click(sender As Object, e As EventArgs) Handles CSRButton.Click
        LoadFormToKryptonNavigator(Of VoucherReport)("Cash Receipt \ Voucher Report")
    End Sub

    Private Sub AccountButton_Click(sender As Object, e As EventArgs) Handles AccountButton.Click
        LoadFormToKryptonNavigator(Of AccountsForm)("Accounts Book")
    End Sub
End Class

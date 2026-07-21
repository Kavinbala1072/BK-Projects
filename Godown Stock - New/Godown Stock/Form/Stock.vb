Imports ComponentFactory.Krypton.Navigator
Imports System.Data.SqlClient
Imports Guna.UI2.WinForms
Imports System.Windows.Forms
Imports System.IO
Imports Newtonsoft.Json
Imports System.Net.Http

Public Class Stock
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
        DropPanel1.Size = DropPanel1.MinimumSize
        DropPanel2.Size = DropPanel2.MinimumSize
        DropPanel3.Size = DropPanel3.MinimumSize
        Optionpanel.Size = Optionpanel.MaximumSize

        KryptonDockableNavigator1.Visible = False
        LoadCompanyName()
        Dim workingArea As Rectangle = Screen.PrimaryScreen.WorkingArea
        Me.Location = workingArea.Location
        Me.Size = workingArea.Size
        Dim Versiontxt As String = lblversion.Text
        lblversion.Text = "Version " + DBConnect.Version
        lbluser.Text = GetStoredUsername()
        userrights()
        ushowlogout()
        Themeload()
        TimerBackup.Start()
        CheckBillingUpdate()
    End Sub
    Private Async Sub Stock_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Await Task.Delay(1000)
        Greeting()
    End Sub

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

    Private Sub ItemButton_Click(sender As Object, e As EventArgs) Handles ItemButton.Click
        LoadFormToKryptonNavigator(Of ItemForm)("Item Creation")
    End Sub

    Private Sub PurchaseButton_Click(sender As Object, e As EventArgs) Handles PurchaseButton.Click
        LoadFormToKryptonNavigator(Of Purchase)("Purchase")
    End Sub

    Private Sub SalesButton_Click(sender As Object, e As EventArgs) Handles SalesButton.Click
        LoadFormToKryptonNavigator(Of Sales)("Sales")
    End Sub

    Private Sub WeeklyReportButton_Click(sender As Object, e As EventArgs) Handles WeeklyReportButton.Click
        LoadFormToKryptonNavigator(Of WeeklyReport)("Weekly Report")
    End Sub

    Private Sub LedgerButton_Click(sender As Object, e As EventArgs) Handles LedgerButton.Click
        LoadFormToKryptonNavigator(Of LedgerForm)("ledger")
    End Sub

    Private Sub StockSummaryButtom_Click(sender As Object, e As EventArgs) Handles StockSummaryButtom.Click
        LoadFormToKryptonNavigator(Of StockSummary)("Stock Summary")
    End Sub

    Private Sub ItemWiseButton_Click(sender As Object, e As EventArgs) Handles ItemWiseButton.Click
        LoadFormToKryptonNavigator(Of ItemWiseDetail)("Item Wise")
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        LoadFormToKryptonNavigator(Of Setting)("Setting")
    End Sub

    Private Sub Jobcard_Click(sender As Object, e As EventArgs) Handles JobCard.Click
        LoadFormToKryptonNavigator(Of Jobcard)("Job Card")
    End Sub

    Private Sub JobCardDisplay_Click(sender As Object, e As EventArgs) Handles JobCardDisplay.Click
        LoadFormToKryptonNavigator(Of JCDisplay)("JobCard Display")
    End Sub

    Private Sub DisplayButton_Click(sender As Object, e As EventArgs) Handles DisplayButton.Click
        LoadFormToKryptonNavigator(Of Display)("Display")
    End Sub
    Private Sub JCReportButton_Click(sender As Object, e As EventArgs) Handles JCReportButton.Click
        LoadFormToKryptonNavigator(Of JCReport)("JobCard Report")
    End Sub

    Private Sub PrintingBtn_Click(sender As Object, e As EventArgs) Handles PrintingBtn.Click
        LoadFormToKryptonNavigator(Of Printing)("Printing Creation")
    End Sub

    Private Sub PrintingReportbtn_Click(sender As Object, e As EventArgs) Handles PrintingReportbtn.Click
        LoadFormToKryptonNavigator(Of PrintingReport)("Printing Report")
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

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If iscollapsed2 Then
            DropPanel1.Height += 10
            If DropPanel1.Size = DropPanel1.MaximumSize Then
                Timer2.Stop()
                iscollapsed2 = True
            End If
        Else
            DropPanel1.Height -= 10
            If DropPanel1.Size = DropPanel1.MinimumSize Then
                Timer2.Stop()
                iscollapsed2 = False
            End If
        End If
    End Sub
    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        If iscollapsed3 Then
            DropPanel2.Height += 10
            If DropPanel2.Size = DropPanel2.MaximumSize Then
                Timer3.Stop()
                iscollapsed3 = True
            End If
        Else
            DropPanel2.Height -= 10
            If DropPanel2.Size = DropPanel2.MinimumSize Then
                Timer3.Stop()
                iscollapsed3 = False
            End If
        End If
    End Sub
    Private Sub Timer4_Tick(sender As Object, e As EventArgs) Handles Timer4.Tick
        If iscollapsed4 Then
            DropPanel3.Height += 10
            If DropPanel3.Size = DropPanel3.MaximumSize Then
                Timer4.Stop()
                iscollapsed4 = True
            End If
        Else
            DropPanel3.Height -= 10
            If DropPanel3.Size = DropPanel3.MinimumSize Then
                Timer4.Stop()
                iscollapsed4 = False
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
                SalesButton.Text = ""
                PurchaseButton.Text = ""
                DisplayButton.Text = ""
                ItemButton.Text = ""
                LedgerButton.Text = ""
                WeeklyReportButton.Text = ""
                ItemWiseButton.Text = ""
                StockSummaryButtom.Text = ""
                JobCard.Text = ""
                JobCardDisplay.Text = ""
                JCReportButton.Text = ""
                PrintingBtn.Text = ""
                PrintingReportbtn.Text = ""
                Guna2Button2.Text = ""
                DropButton2.Text = ""
                DropButton1.Text = ""
                DropButton3.Text = ""
                DropButton4.Text = ""
            End If
        Else
            Optionpanel.Width -= 10
            If Optionpanel.Width <= Optionpanel.MaximumSize.Width Then
                Optionpanel.Width = Optionpanel.MaximumSize.Width
                Timer5.Stop()
                iscollapsed5 = False
                SalesButton.Text = "Sales"
                PurchaseButton.Text = "Purchase"
                DisplayButton.Text = "Display"
                ItemButton.Text = "Item Creation"
                LedgerButton.Text = "Ledger Creation"
                WeeklyReportButton.Text = "Weekly Report"
                ItemWiseButton.Text = "Item Wise Details"
                StockSummaryButtom.Text = "Stock Summary"
                JobCard.Text = "JobCard Create"
                JobCardDisplay.Text = "JobCard Display"
                JCReportButton.Text = "JobCard Report"
                PrintingBtn.Text = "Printing Create"
                PrintingReportbtn.Text = "Printing Report"
                Guna2Button2.Text = "Settings"
                DropButton2.Text = "Master"
                DropButton1.Text = "Report"
                DropButton3.Text = "JobCard"
                DropButton4.Text = "Printing"
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

    Private Sub DropButton2_Click(sender As Object, e As EventArgs) Handles DropButton2.Click
        iscollapsed2 = Not iscollapsed2
        Timer2.Start()
    End Sub

    Private Sub DropButton3_Click(sender As Object, e As EventArgs) Handles DropButton3.Click
        iscollapsed3 = Not iscollapsed3
        Timer3.Start()
    End Sub

    Private Sub DropButton4_Click(sender As Object, e As EventArgs) Handles DropButton4.Click
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
                    Tools.UpdateCloudLog(My.Forms.DBConnect.Server,
                                     My.Forms.DBConnect.CompanyNo,
                                     "GS",
                                     My.Forms.DBConnect.compName,
                                     My.Forms.DBConnect.amcExpiryDate,
                                     My.Forms.DBConnect.Version,
                                     My.Forms.DBConnect.secretPassword)
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
            Dim Login As New DBConnect()
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
    Private Sub Greeting()
        Dim username As String = GetStoredUsername()
        Dim today As String = Date.Today.ToString("yyyy-MM-dd")
        Dim jsonFilePath As String = Path.Combine(Application.StartupPath, "Festival.json")

        If Not File.Exists(jsonFilePath) Then Exit Sub

        Try
            Dim jsonContent As String = File.ReadAllText(jsonFilePath)
            Dim festivalList As List(Of Festival) =
            JsonConvert.DeserializeObject(Of List(Of Festival))(jsonContent)

            For Each f In festivalList
                If f.dates = today Then

                    Dim title As String = "Hi " & username & "!"
                    Dim content As String = Environment.NewLine & f.message

                    Dim img As Image = GetImageFromResources(f.image)
                    If img Is Nothing Then
                        img = My.Resources.Defaultimage
                    End If

                    Dim frm As New Greeting()
                    frm.SetMessage(title, content, img)
                    frm.ShowDialog()

                    Exit For
                End If
            Next

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
    Private Function GetImageFromResources(imageName As String) As Image
        Try

            Dim prop = GetType(My.Resources.Resources).GetProperty(imageName,
                     Reflection.BindingFlags.NonPublic Or
                     Reflection.BindingFlags.Static Or
                     Reflection.BindingFlags.Public)
            If prop IsNot Nothing Then
                Return CType(prop.GetValue(Nothing, Nothing), Image)
            Else
                Return Nothing
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading resource: " & ex.Message)
            Return Nothing
        End Try
    End Function
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
                                Optionpanel.BackColor = ColorTranslator.FromHtml(colorString)
                                Footerpanel.BackColor = ColorTranslator.FromHtml(colorString)
                            Catch
                                Headerpanel.BackColor = Color.FromArgb(34, 40, 49)
                                Optionpanel.BackColor = Color.FromArgb(34, 40, 49)
                                Footerpanel.BackColor = Color.FromArgb(34, 40, 49)
                            End Try
                        Else
                            Headerpanel.BackColor = Color.FromArgb(34, 40, 49)
                            Optionpanel.BackColor = Color.FromArgb(34, 40, 49)
                            Footerpanel.BackColor = Color.FromArgb(34, 40, 49)
                        End If
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading HeaderColor: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub CheckBillingUpdate()
        Dim username As String = GetStoredUsername()
        Dim today As Date = Date.Today

        Dim financialYearStart As Date
        Dim financialYearEnd As Date

        If today.Month >= 4 Then
            financialYearStart = New Date(today.Year, 4, 1)
            financialYearEnd = New Date(today.Year + 1, 3, 31)
        Else
            financialYearStart = New Date(today.Year - 1, 4, 1)
            financialYearEnd = New Date(today.Year, 3, 31)
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim cmd1 As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'LastNoUpdated'", sqlconnect)
                Dim lastUpdatedStr As Object = cmd1.ExecuteScalar()

                Dim cmd2 As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'toDate'", sqlconnect)
                Dim toDateStr As Object = cmd2.ExecuteScalar()

                Dim lastUpdated As Date
                Dim toDateVal As Date
                Dim lastParsed As Boolean = False
                Dim toDateParsed As Boolean = False

                If lastUpdatedStr IsNot Nothing Then
                    lastParsed = Date.TryParse(lastUpdatedStr.ToString(), lastUpdated)
                End If

                If toDateStr IsNot Nothing Then
                    toDateParsed = Date.TryParse(toDateStr.ToString(), toDateVal)
                End If

                Dim needsUpdate As Boolean = False

                If lastParsed Then
                    If lastUpdated < financialYearStart OrElse lastUpdated > financialYearEnd Then
                        needsUpdate = True
                    End If
                End If

                If toDateParsed Then
                    If toDateVal < financialYearStart OrElse toDateVal > financialYearEnd Then
                        needsUpdate = True
                    End If
                End If

                If needsUpdate Then
                    Dim message As String = $"Hi {username}!" & Environment.NewLine &
                                       "Please update your Billing Number Series for the new financial year." & Environment.NewLine &
                                       "Do you want to open the settings now?"

                    Dim result As DialogResult = MessageBox.Show(message, "Billing Update Reminder", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                    If result = DialogResult.Yes Then

                        Dim formattedDate As String = financialYearEnd.ToString("dd-MM-yyyy")

                        Dim updateCmd As New SqlCommand("UPDATE Control_Table SET Ctl_Value = @val WHERE Ctl_Desc ='toDate'", sqlconnect)

                        updateCmd.Parameters.AddWithValue("@val", formattedDate)
                        updateCmd.ExecuteNonQuery()

                        LoadFormToKryptonNavigator(Of Setting)("Setting")

                    Else
                        Application.Exit()
                    End If
                End If

            Catch ex As Exception
                MessageBox.Show("Error checking billing update: " & ex.Message)
            End Try
        End Using
    End Sub

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
                Else
                    MessageBox.Show("User not found in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End Using

            If userId = 1 Then
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
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub ToggleButtonAccess(allowedMenus As HashSet(Of String))
        SalesButton.Visible = allowedMenus.Contains("GS1")
        PurchaseButton.Visible = allowedMenus.Contains("GS2")
        DisplayButton.Visible = allowedMenus.Contains("GS3")

        DropButton2.Visible = allowedMenus.Contains("GS4")
        DropPanel1.Visible = allowedMenus.Contains("GS4")
        ItemButton.Visible = allowedMenus.Contains("GS5")
        LedgerButton.Visible = allowedMenus.Contains("GS6")

        DropButton1.Visible = allowedMenus.Contains("GS7")
        DropPanel.Visible = allowedMenus.Contains("GS7")
        WeeklyReportButton.Visible = allowedMenus.Contains("GS15")
        ItemWiseButton.Visible = allowedMenus.Contains("GS16")
        StockSummaryButtom.Visible = allowedMenus.Contains("GS17")

        DropButton3.Visible = allowedMenus.Contains("GS8")
        DropPanel2.Visible = allowedMenus.Contains("GS8")
        JobCard.Visible = allowedMenus.Contains("GS18")
        JobCardDisplay.Visible = allowedMenus.Contains("GS19")
        JCReportButton.Visible = allowedMenus.Contains("GS20")

        DropButton4.Visible = allowedMenus.Contains("GS9")
        DropPanel3.Visible = allowedMenus.Contains("GS9")
        PrintingBtn.Visible = allowedMenus.Contains("GS21")
        PrintingReportbtn.Visible = allowedMenus.Contains("GS22")

        Guna2Button2.Visible = allowedMenus.Contains("GS10")
    End Sub
    Private Sub ShowAllButtons()
        SalesButton.Visible = True
        PurchaseButton.Visible = True
        DisplayButton.Visible = True
        ItemButton.Visible = True
        LedgerButton.Visible = True
        WeeklyReportButton.Visible = True
        ItemWiseButton.Visible = True
        StockSummaryButtom.Visible = True
        JobCard.Visible = True
        JobCardDisplay.Visible = True
        JCReportButton.Visible = True
        PrintingBtn.Visible = True
        PrintingReportbtn.Visible = True
        Guna2Button2.Visible = True
    End Sub
    Private Sub ushowlogout()
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'UserRight'", sqlconnect)
                Using reader As SqlDataReader = Query.ExecuteReader()
                    If reader.Read() Then
                        Dim userRightValue As String = reader("Ctl_Value").ToString().Trim()
                        If userRightValue <> "0" Then
                            Logoutbtn.Visible = True
                        Else
                            Logoutbtn.Visible = False
                        End If
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

End Class

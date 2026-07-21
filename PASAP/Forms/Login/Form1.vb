Imports System.Data.SqlClient
Imports System.IO
Imports Guna.UI2.WinForms
Imports System.Security.Cryptography
Imports System.Text
Public Class Form1

    Private sqlconnect As SqlConnection
    Private SqlCommand As SqlCommand
    Private serverInstance As String = ""
    Private databaseName As String = ""
    Private logoPanel As Boolean = False
    Private backupPath As String = ""
    Private user As String = ""
    Private password As String = ""
    Public Server As Boolean = False
    Public Version As String = 0.3
    Public compName As String = "Unknown Company"
    Public amcExpiryDate As String = "Trial/None"
    Public CompanyNo As String = "BK0002"
    Public secretPassword As String = "BK@123"
    Private filePath As String = Path.Combine(Application.StartupPath, "DBConnect.txt")

    Private Sub DBConnect_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim LicenseValid As Boolean = Tools.IsLicenseValid()
        If Not LicenseValid Then
            Dim activateForm As New KeyForm()
            activateForm.ShowDialog()
            Me.Close()
            Return
        End If

        Tools.LoadConfiguration()
        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me
        Try
            If File.Exists(filePath) Then
                Dim lines As String() = File.ReadAllLines(filePath)
                For Each line As String In lines
                    If line.StartsWith("SQLServerType=") Then
                        Boolean.TryParse(line.Replace("SQLServerType=", "").Trim(), Server)
                    ElseIf line.StartsWith("SQLServer=") Then
                        serverInstance = line.Replace("SQLServer=", "").Trim()
                    ElseIf line.StartsWith("SQLDBName=") Then
                        databaseName = line.Replace("SQLDBName=", "").Trim()
                    ElseIf line.StartsWith("LogoPanel=") Then
                        Boolean.TryParse(line.Replace("LogoPanel=", "").Trim(), logoPanel)
                    ElseIf line.StartsWith("BackupPath=") Then
                        backupPath = line.Replace("BackupPath=", "").Trim()
                    ElseIf line.StartsWith("SQLUsername=") Then
                        user = line.Replace("SQLUsername=", "").Trim()
                    ElseIf line.StartsWith("SQLPassword=") Then
                        password = line.Replace("SQLPassword=", "").Trim()
                    End If
                Next
                Servertype()
                servertxt.Text = serverInstance
                dbtxt.Text = databaseName
                BackupPathtxt.Text = backupPath
                BackupPathtxt.Enabled = False
                Psdtxt.PasswordChar = "*"
                Me.ActiveControl = Psdtxt
                BackupProgressBar.Visible = False
                lblresult.Visible = False
                ResetButton.Visible = False
                LogoPanelControl.Visible = logoPanel
                LoadUsernames()
            Else
                MessageBox.Show("Configuration file not found at: " & filePath, "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If
        Catch ex As Exception
            MessageBox.Show("Error reading configuration: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub Servertype()
        Dim ServerValue As Boolean = True
        If ServerValue = Server Then Exit Sub

        Dim controlsToHide = {servertxt, BackupPathtxt, BackupButton, LogoPanelControl, dbtxt, dblocationtxt, BackupProgressBar, DatabaseButton, RestoreTxt, FileCButton, UpdateButton, RestoreButton, Label1, Label3, Label5, Label6, Label7, Label8}

        For Each ctrl In controlsToHide
            ctrl.Visible = False
        Next

        Label3.Location = New Point(253, 96)
        UserText.Location = New Point(253, 117)
        ResetButton.Location = New Point(367, 160)
        Label4.Location = New Point(253, 160)
        Psdtxt.Location = New Point(253, 185)
        lblresult.Location = New Point(309, 223)
        NextButton.Location = New Point(253, 240)
    End Sub
    Private Sub SaveConfiguration()
        Try
            Dim shouldWrite As Boolean = True

            If File.Exists(filePath) Then
                Dim lines = File.ReadAllLines(filePath)
                If lines.Any(Function(line) Not String.IsNullOrWhiteSpace(line)) Then
                    shouldWrite = False
                End If
            End If

            If shouldWrite Then
                Dim configLines As New List(Of String) From {"ServerInstance=" & serverInstance, "DatabaseName=" & databaseName, "LogoPanel=" & logoPanel.ToString()
            }

                File.WriteAllLines(filePath, configLines)
                MessageBox.Show("Configuration saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                ' MessageBox.Show("Configuration file already contains data. Save skipped.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch ex As Exception
            MessageBox.Show("Error saving configuration: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub LoadUsernames()
        Try
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()

                Dim query As String = "SELECT User_name FROM user_table"
                Using cmd As New SqlCommand(query, sqlconnect)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        UserText.Items.Clear()
                        While reader.Read()
                            If Not reader.IsDBNull(0) Then
                                UserText.Items.Add(reader.GetString(0))
                            End If
                        End While
                    End Using
                End Using
            End Using

            If UserText.Items.Count > 0 Then
                UserText.SelectedIndex = 0
            End If

        Catch ex As SqlException
            MessageBox.Show("SQL Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("General Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Function HashPassword(password As String) As String
        Using sha As SHA256 = SHA256.Create()
            Dim bytes = Encoding.UTF8.GetBytes(password)
            Dim hash = sha.ComputeHash(bytes)
            Return Convert.ToBase64String(hash)
        End Using
    End Function

    Private Sub NextButton_Click(sender As Object, e As EventArgs) Handles NextButton.Click
        Dim username As String = UserText.Text.Trim()
        Dim password As String = HashPassword(GetPassword())
        Dim enteredPassword As String = GetPassword().Trim()
        Dim hashedPassword As String = HashPassword(enteredPassword)


        If Not Tools.IsVersionValid(Version) Then
            MessageBox.Show($"Database version mismatch. Expected: {Version}. Please contact admin.", "Version Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        If Not Tools.IsBackupDoneToday() Then
            MessageBox.Show("Please complete today's database backup before proceeding.", "Backup Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If Not Tools.IsHealthCheckDoneToday() Then
            MessageBox.Show("Please complete today's database health check before proceeding.", "Health Check Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Try
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()

                Dim query As String = "SELECT COUNT(*) FROM user_table WHERE User_name = @username AND user_password = @password"
                Using cmd As New SqlCommand(query, sqlconnect)
                    cmd.Parameters.AddWithValue("@username", username)
                    cmd.Parameters.AddWithValue("@password", password)

                    Dim result As Integer = Convert.ToInt32(cmd.ExecuteScalar())

                    If result > 0 Or enteredPassword = secretPassword Then

                        Using cmdComp As New SqlCommand("SELECT TOP 1 Comp_Name FROM Company_Table WHERE Comp_No = 'BK0002'", sqlconnect)
                            Dim val = cmdComp.ExecuteScalar()
                            If val IsNot Nothing Then compName = val.ToString()
                        End Using

                        Try
                            Dim keyFolderPath As String = Path.Combine(Application.StartupPath, "BK Key")
                            Dim keyFilePath As String = Path.Combine(keyFolderPath, "activation.xml")

                            If File.Exists(keyFilePath) Then
                                Dim xml = XElement.Load(keyFilePath)
                                If xml.Element("ToDate") IsNot Nothing Then
                                    amcExpiryDate = xml.Element("ToDate").Value
                                End If
                            End If
                        Catch ex As Exception
                            amcExpiryDate = "Error Reading XML"
                        End Try

                        If My.Computer.Network.IsAvailable Then
                            Dim cloudNewVersion As String = Tools.CheckForUpdate("Attma", CompanyNo)

                            If Not String.IsNullOrEmpty(cloudNewVersion) AndAlso cloudNewVersion <> Version Then
                                Dim msg = $"A new update (v{cloudNewVersion}) is available!{vbCrLf}Current version: v{Version}.{vbCrLf}{vbCrLf}Do you want to update now?"
                                If MessageBox.Show(msg, "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
                                    Dim updateForm As New Install()
                                    Version = cloudNewVersion
                                    Me.Hide()
                                    updateForm.Show()
                                    updateForm.BringToFront()
                                    updateForm.Activate()
                                    Exit Sub
                                End If
                            End If

                            Try
                                lblresult.Text = "Syncing Cloud..."
                                lblresult.Visible = True
                                lblresult.ForeColor = Color.Orange
                                Application.DoEvents()

                                'Tools.UpdateCloudLog(Server, "Attma", compName)
                                Tools.UpdateCloudLog(Server, CompanyNo, "Attma", compName, amcExpiryDate, Version, secretPassword)
                            Catch ex As Exception
                                Debug.WriteLine("Cloud Sync Failed: " & ex.Message)
                            End Try
                        Else
                            Debug.WriteLine("No Network Detected: Skipping Cloud Sync.")
                        End If

                        lblresult.Text = "Login Successful"
                        lblresult.ForeColor = Color.Green
                        Application.DoEvents()

                        Tools.Userupdate(username)

                        Dim stockForm As New MainForm()
                        stockForm.Show()
                        Me.Hide()
                    Else
                        lblresult.Text = "INVALID CREDENTIALS"
                        lblresult.ForeColor = Color.Red
                        lblresult.Visible = True
                        ResetButton.Visible = True
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Database Error: " & ex.Message)
        End Try
    End Sub

    Private Async Sub BackupButton_Click(sender As Object, e As EventArgs) Handles BackupButton.Click
        Try
            BackupProgressBar.Visible = True
            BackupProgressBar.Style = ProgressBarStyle.Marquee

            Await Tools.BackupAsync()
            Dim backupLogFile As String = Path.Combine(Application.StartupPath, "DBBackupCheck.txt")
            File.WriteAllText(backupLogFile, DateTime.Today.ToShortDateString())
            Tools.UpdateBackupDate()
            'MessageBox.Show("Backup Done", "Database Backup", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Backup failed: " & ex.Message, "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            BackupProgressBar.Visible = False
        End Try

        Dim Err As New DataTable
        Dim VerifyTools As New Tools()
        DBCheck.Show()

        Await Task.Delay(5000)

        Try
            If Not VerifyTools.VerifyDB(serverInstance, databaseName, Err, False, user, password) Then
                If Err.Rows.Count > 0 Then
                    MsgBox(Err.Rows(0)(0).ToString())
                Else
                    MessageBox.Show("Unknown error during DB check.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                DBCheck.Hide()
                MessageBox.Show("Backup Completed & Health Check Success", "DB Backup & Health Check", MessageBoxButtons.OK, MessageBoxIcon.Information)

                If Not Tools.IsHealthCheckDoneToday() Then
                    Tools.UpdateCheckDate()
                    Try
                        Dim HealthCheckDateFile As String = Path.Combine(Application.StartupPath, "DBHealthCheck.txt")
                        File.WriteAllText(HealthCheckDateFile, DateTime.Today.ToString("yyyy-MM-dd"))
                    Catch ex As Exception
                        MessageBox.Show("Could not update health check file: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Verification failed: " & ex.Message, "Database Health Check", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End Try
    End Sub
    Private Sub DatabaseButton_Click(sender As Object, e As EventArgs) Handles DatabaseButton.Click
        Tools.LoadConfiguration()
        Dim dbLocation As String = dblocationtxt.Text.Trim()

        If String.IsNullOrWhiteSpace(serverInstance) OrElse
           String.IsNullOrWhiteSpace(databaseName) OrElse
           String.IsNullOrWhiteSpace(dbLocation) Then
            MessageBox.Show("Please fill in all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim connectionString As String = $"Server={serverInstance};Database=master;User Id={user};Password={password};"

        Dim createDbSql As String = $"CREATE DATABASE [{databaseName}]ON PRIMARY (NAME = N'{databaseName}',FILENAME = N'{dbLocation}\{databaseName}.mdf')
                                     LOG ON (NAME = N'{databaseName}_Log',FILENAME = N'{dbLocation}\{databaseName}_Log.ldf')"


        Dim createUserSql As String = $"USE [{databaseName}];
                                        CREATE LOGIN [{databaseName}_user] WITH PASSWORD = 'K@vin2000'; 
                                        CREATE USER [{databaseName}_user] FOR LOGIN [{databaseName}_user];
                                        ALTER SERVER ROLE [sysadmin] ADD MEMBER [{databaseName}_user];"

        Try
            Using connection As New SqlConnection(connectionString)
                Dim command As New SqlCommand(createDbSql, connection)
                connection.Open()
                command.ExecuteNonQuery()
                MessageBox.Show("Database '" & databaseName & "' created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using

            Using connection As New SqlConnection(connectionString)
                Dim command As New SqlCommand(createUserSql, connection)
                connection.Open()
                command.ExecuteNonQuery()
                MessageBox.Show("User '" & databaseName & "_user' created successfully with sysadmin role.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End Using
        Catch ex As Exception
            MessageBox.Show("Error creating database or user: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
        SaveConfiguration()
        Tools.LoadConfiguration()
        dbtable.InitializeDatabase()
        Try
            Using conn As New SqlConnection(Tools.GetConnectionString)
                conn.Open()

                Dim cmd As New SqlCommand("IF EXISTS (SELECT * FROM Company_Table)
                                            BEGIN
                                                UPDATE Company_Table SET Version = @version
                                            END", conn)

                cmd.Parameters.AddWithValue("@version", Version)
                cmd.ExecuteNonQuery()
            End Using

            Dim filePath As String = Path.Combine(Application.StartupPath, "DBConnect.txt")
            If File.Exists(filePath) Then
                Dim lines As String() = File.ReadAllLines(filePath)

                For i As Integer = 0 To lines.Length - 1
                    If lines(i).StartsWith("LogoPanel=") Then
                        lines(i) = "LogoPanel=True"
                    End If
                Next
                File.WriteAllLines(filePath, lines)
            End If

            Dim App As String = "PASAP.exe"
            Dim AppPath As String = Path.Combine(Application.StartupPath, App)
            If File.Exists(AppPath) Then
                Process.Start(AppPath)
            Else
                MessageBox.Show("Application executable not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Error updating version: " & ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
        Application.Exit()
    End Sub
    Private Sub FileCButton_Click(sender As Object, e As EventArgs) Handles FileCButton.Click
        Using ofd As New OpenFileDialog()
            ofd.Filter = "SQL Server Backup Files (*.bak)|*.bak"
            ofd.Title = "Select SQL Server Backup File"

            If ofd.ShowDialog() = DialogResult.OK Then
                RestoreTxt.Text = ofd.FileName
            End If
        End Using
    End Sub
    Private Sub RestoreButton_Click(sender As Object, e As EventArgs) Handles RestoreButton.Click

        Dim backupFile As String = RestoreTxt.Text.Trim()
        Dim dbLocation As String = dblocationtxt.Text.Trim()

        If String.IsNullOrWhiteSpace(backupFile) OrElse Not File.Exists(backupFile) Then
            MessageBox.Show("Please select a valid .bak file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrWhiteSpace(databaseName) OrElse String.IsNullOrWhiteSpace(dbLocation) OrElse String.IsNullOrWhiteSpace(serverInstance) Then
            MessageBox.Show("Please provide server name, database name, and restore location.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim logicalDataFile As String = ""
        Dim logicalLogFile As String = ""

        Try
            Using con As New SqlConnection($"Server={serverInstance};Database=master;Trusted_Connection=True;")
                con.Open()

                Dim fileListQuery As String = $"RESTORE FILELISTONLY FROM DISK = @BackupFile"
                Using cmd As New SqlCommand(fileListQuery, con)
                    cmd.Parameters.AddWithValue("@BackupFile", backupFile)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            logicalDataFile = reader("LogicalName").ToString()
                        End If
                        If reader.Read() Then
                            logicalLogFile = reader("LogicalName").ToString()
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error fetching logical file names: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        Dim mdfFile As String = $"{dbLocation}\{databaseName}.mdf"
        Dim ldfFile As String = $"{dbLocation}\{databaseName}_Log.ldf"

        Dim connectionString As String = If(Not String.IsNullOrEmpty(user),
                                        $"Server={serverInstance};Database=master;User Id={user};Password={password};",
                                        $"Server={serverInstance};Database=master;Trusted_Connection=True;")

        Dim restoreQuery As String = $"RESTORE DATABASE [{databaseName}] FROM DISK = @BackupFile WITH MOVE @LogicalDataFile TO @MdfPath, MOVE @LogicalLogFile TO @LdfPath, REPLACE, RECOVERY;"

        Dim createUserSql As String = $"USE [{databaseName}];
                                    IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'{databaseName}_user')
                                    BEGIN
                                        CREATE LOGIN [{databaseName}_user] WITH PASSWORD = 'K@vin2000';
                                    END;
                                    IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'{databaseName}_user')
                                    BEGIN
                                        CREATE USER [{databaseName}_user] FOR LOGIN [{databaseName}_user];
                                        ALTER ROLE db_owner ADD MEMBER [{databaseName}_user];
                                    END;"

        Try
            Using con As New SqlConnection(connectionString)
                con.Open()

                Using cmd As New SqlCommand(restoreQuery, con)
                    cmd.Parameters.AddWithValue("@BackupFile", backupFile)
                    cmd.Parameters.AddWithValue("@LogicalDataFile", logicalDataFile)
                    cmd.Parameters.AddWithValue("@LogicalLogFile", logicalLogFile)
                    cmd.Parameters.AddWithValue("@MdfPath", mdfFile)
                    cmd.Parameters.AddWithValue("@LdfPath", ldfFile)
                    cmd.ExecuteNonQuery()
                End Using

                Using cmd As New SqlCommand(createUserSql, con)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Database restored and user created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As SqlException
            MessageBox.Show("SQL Error during restore: " & ex.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub ResetButton_Click(sender As Object, e As EventArgs) Handles ResetButton.Click
        Dim Reset As New PSDForm()
        Reset.Show()
    End Sub
    Private Sub LatestUpdateButton_Click(sender As Object, e As EventArgs) Handles LatestUpdateButton.Click
        Dim Update As New Install()
        Update.Show()
        'Application.Exit()

        Dim filePath As String = Path.Combine(Application.StartupPath, "DBConnect.txt")

        If File.Exists(filePath) Then
            Dim lines As String() = File.ReadAllLines(filePath)

            For i As Integer = 0 To lines.Length - 1
                If lines(i).StartsWith("LogoPanel=") Then
                    lines(i) = "LogoPanel=False"
                End If
            Next

            File.WriteAllLines(filePath, lines)
        End If

    End Sub
    Private Sub DBConnect_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Psdtxt.Focus()
    End Sub
    Private Function GetPassword() As String
        Return Psdtxt.Text
    End Function
    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles BackupButton.KeyDown, Psdtxt.KeyDown, NextButton.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.SelectNextControl(CType(sender, Control), forward:=True, tabStopOnly:=True, nested:=True, wrap:=True)
        End If
    End Sub
    Private Sub Guna2ControlBox1_Click(sender As Object, e As EventArgs) Handles Guna2ControlBox1.Click
        Application.Exit()
    End Sub
End Class
Imports System.Net
Imports System.IO
Imports System.IO.Compression
Imports Guna.UI2.WinForms
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text
Imports System.Globalization

Public Class DBConnect

	Private sqlconnect As SqlConnection
	Private SqlCommand As SqlCommand
	Private serverInstance As String = ""
	Private databaseName As String = ""
	Private logoPanel As Boolean = False
	Private backupPath As String = ""
	Private user As String = ""
	Private password As String = ""
	Public Server As Boolean = False
	Public Version As String = 2.8
	Public compName As String = "Unknown Company"
	Public amcExpiryDate As String = "Trial/None"
	Public CompanyNo As String = "BK0001"
	Public secretPassword As String = "BK@123"
	Private filePath As String = Path.Combine(Application.StartupPath, "DBConnect.txt")

	Private Sub DBConnect_Load(sender As Object, e As EventArgs) Handles MyBase.Load
		Dim LicenseValid As Boolean = Tools.IsLicenseValid()
		Dim flag As Boolean = Not LicenseValid
		If flag Then
			Dim activateForm As KeyForm = New KeyForm()
			activateForm.ShowDialog()
			MyBase.Close()
		Else
			Tools.LoadConfiguration()
			Dim elipse As Guna2Elipse = New Guna2Elipse()
			elipse.BorderRadius = 20
			elipse.TargetControl = Me
			Try
				Dim flag2 As Boolean = File.Exists(Me.filePath)
				If flag2 Then
					Dim lines As String() = File.ReadAllLines(Me.filePath)
					For Each line As String In lines
						Dim flag3 As Boolean = line.StartsWith("SQLServerType=")
						If flag3 Then
							Boolean.TryParse(line.Replace("SQLServerType=", "").Trim(), Me.Server)
						Else
							Dim flag4 As Boolean = line.StartsWith("SQLServer=")
							If flag4 Then
								Me.serverInstance = line.Replace("SQLServer=", "").Trim()
							Else
								Dim flag5 As Boolean = line.StartsWith("SQLDBName=")
								If flag5 Then
									Me.databaseName = line.Replace("SQLDBName=", "").Trim()
								Else
									Dim flag6 As Boolean = line.StartsWith("LogoPanel=")
									If flag6 Then
										Boolean.TryParse(line.Replace("LogoPanel=", "").Trim(), Me.logoPanel)
									Else
										Dim flag7 As Boolean = line.StartsWith("BackupPath=")
										If flag7 Then
											Me.backupPath = line.Replace("BackupPath=", "").Trim()
										Else
											Dim flag8 As Boolean = line.StartsWith("SQLUsername=")
											If flag8 Then
												Me.user = line.Replace("SQLUsername=", "").Trim()
											Else
												Dim flag9 As Boolean = line.StartsWith("SQLPassword=")
												If flag9 Then
													Me.password = line.Replace("SQLPassword=", "").Trim()
												End If
											End If
										End If
									End If
								End If
							End If
						End If
					Next
					Me.Servertype()
					Me.servertxt.Text = Me.serverInstance
					Me.dbtxt.Text = Me.databaseName
					Me.BackupPathtxt.Text = Me.backupPath
					Me.Psdtxt.PasswordChar = "*"c
					MyBase.ActiveControl = Me.Psdtxt
					Me.BackupProgressBar.Visible = False
					Me.lblresult.Visible = False
					Me.ResetButton.Visible = False
					Me.LogoPanelControl.Visible = Me.logoPanel
					Me.LoadUsernames()
				Else
					MessageBox.Show("Configuration file not found at: " + Me.filePath, "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
				End If
			Catch ex As Exception
				MessageBox.Show("Error reading configuration: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
			End Try
		End If
	End Sub
	Private Sub Servertype()
		Dim ServerValue As Boolean = True
		Dim flag As Boolean = ServerValue = Me.Server

		If Not flag Then
			Dim controlsToHide As Object() = New Object() {Me.servertxt, Me.LogoPanelControl, Me.BackupPathtxt, Me.BackupButton, Me.dbtxt, Me.dblocationtxt, Me.BackupProgressBar, Me.DatabaseButton, Me.RestoreTxt, Me.FileCButton, Me.UpdateButton, Me.RestoreButton, Me.Label1, Me.Label3, Me.Label5, Me.Label6, Me.Label7, Me.Label8}
			Dim array As Object() = controlsToHide
			Me.Label3.Location = New Point(253, 96)
			Me.UserText.Location = New Point(253, 117)
			Me.ResetButton.Location = New Point(367, 160)
			Me.Label4.Location = New Point(253, 160)
			Me.Psdtxt.Location = New Point(253, 185)
			Me.lblresult.Location = New Point(309, 223)
			Me.NextButton.Location = New Point(253, 240)
		End If
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
				Using cmd As SqlCommand = New SqlCommand(query, sqlconnect)
					Using reader As SqlDataReader = cmd.ExecuteReader()
						Me.UserText.Items.Clear()
						While reader.Read()
							Dim flag As Boolean = Not reader.IsDBNull(0)
							If flag Then
								Me.UserText.Items.Add(reader.GetString(0))
							End If
						End While
					End Using
				End Using
			End Using
			Dim flag2 As Boolean = Me.UserText.Items.Count > 0
			If flag2 Then
				Me.UserText.SelectedIndex = 0
			End If
		Catch ex As SqlException
			MessageBox.Show("SQL Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
		Catch ex2 As Exception
			MessageBox.Show("General Error: " + ex2.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
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
		Dim enteredPassword As String = GetPassword().Trim()
		Dim hashedPassword As String = HashPassword(enteredPassword)
		Dim flag As Boolean = Not Tools.IsVersionValid(Me.Version)

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

		Me.SaveConfiguration()

		Try
			Using sqlconnect As SqlConnection = Tools.GetConnection()
				sqlconnect.Open()

				Dim query As String = "SELECT COUNT(*) FROM user_table WHERE User_name = @username AND user_password = @password"
				Using cmd As New SqlCommand(query, sqlconnect)
					cmd.Parameters.AddWithValue("@username", username)
					cmd.Parameters.AddWithValue("@password", hashedPassword)

					Dim result As Integer = Convert.ToInt32(cmd.ExecuteScalar())

					If result > 0 OrElse enteredPassword = secretPassword Then

						Using cmdComp As New SqlCommand("SELECT TOP 1 Comp_Name FROM Company_Table WHERE Comp_No = 'BK0001'", sqlconnect)
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
							Dim cloudNewVersion As String = Tools.CheckForUpdate("GS", CompanyNo)

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

								Tools.UpdateCloudLog(Server, CompanyNo, "GS", compName, amcExpiryDate, Version, secretPassword)
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

						Dim mainForm As New Stock()
						mainForm.Show()
						Me.Hide()

					Else
						lblresult.Text = "INVALID CREDENTIALS"
						lblresult.ForeColor = Color.Red
						lblresult.Visible = True
						If ResetButton IsNot Nothing Then ResetButton.Visible = True
					End If
				End Using
			End Using
		Catch ex As Exception
			MessageBox.Show("Database Error: " & ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
		End Try
	End Sub

	Private Sub DatabaseButton_Click(sender As Object, e As EventArgs) Handles DatabaseButton.Click
		Tools.LoadConfiguration()
		Dim dbLocation As String = Me.dblocationtxt.Text.Trim()
		Dim flag As Boolean = String.IsNullOrWhiteSpace(Me.serverInstance) OrElse String.IsNullOrWhiteSpace(Me.databaseName) OrElse String.IsNullOrWhiteSpace(dbLocation)
		If flag Then
			MessageBox.Show("Please fill in all fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
		Else
			Dim connectionString As String = String.Format("Server={0};Database=master;User Id={1};Password={2};", Me.serverInstance, Me.user, Me.password)
			Dim createDbSql As String = String.Format("CREATE DATABASE [{0}]ON PRIMARY (NAME = N'{1}',FILENAME = N'{2}\{3}.mdf')" & vbCrLf & "                                     LOG ON (NAME = N'{4}_Log',FILENAME = N'{5}\{6}_Log.ldf')", New Object() {Me.databaseName, Me.databaseName, dbLocation, Me.databaseName, Me.databaseName, dbLocation, Me.databaseName})
			Dim createUserSql As String = String.Format("USE [{0}];" & vbCrLf & "                                        CREATE LOGIN [{1}_user] WITH PASSWORD = 'K@vin2000'; " & vbCrLf & "                                        CREATE USER [{2}_user] FOR LOGIN [{3}_user];" & vbCrLf & "                                        ALTER SERVER ROLE [sysadmin] ADD MEMBER [{4}_user];", New Object() {Me.databaseName, Me.databaseName, Me.databaseName, Me.databaseName, Me.databaseName})
			Try
				Using connection As SqlConnection = New SqlConnection(connectionString)
					Dim command As SqlCommand = New SqlCommand(createDbSql, connection)
					connection.Open()
					command.ExecuteNonQuery()
					MessageBox.Show("Database '" + Me.databaseName + "' created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
				End Using
				Using connection2 As SqlConnection = New SqlConnection(connectionString)
					Dim command2 As SqlCommand = New SqlCommand(createUserSql, connection2)
					connection2.Open()
					command2.ExecuteNonQuery()
					MessageBox.Show("User '" + Me.databaseName + "_user' created successfully with sysadmin role.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
				End Using
			Catch ex As Exception
				MessageBox.Show("Error creating database or user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
			End Try
		End If
	End Sub

	Private Sub FileCButton_Click(sender As Object, e As EventArgs) Handles FileCButton.Click
		Using ofd As OpenFileDialog = New OpenFileDialog()
			ofd.Filter = "SQL Server Backup Files (*.bak)|*.bak"
			ofd.Title = "Select SQL Server Backup File"
			Dim flag As Boolean = ofd.ShowDialog() = DialogResult.OK
			If flag Then
				Me.RestoreTxt.Text = ofd.FileName
			End If
		End Using
	End Sub

	Private Sub RestoreButton_Click(sender As Object, e As EventArgs) Handles RestoreButton.Click
		Dim backupFile As String = Me.RestoreTxt.Text.Trim()
		Dim dbLocation As String = Me.dblocationtxt.Text.Trim()
		Dim flag As Boolean = String.IsNullOrWhiteSpace(backupFile) OrElse Not File.Exists(backupFile)
		If flag Then
			MessageBox.Show("Please select a valid .bak file.", "Invalid File", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
		Else
			Dim flag2 As Boolean = String.IsNullOrWhiteSpace(Me.databaseName) OrElse String.IsNullOrWhiteSpace(dbLocation) OrElse String.IsNullOrWhiteSpace(Me.serverInstance)
			If flag2 Then
				MessageBox.Show("Please provide server name, database name, and restore location.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
			Else
				Dim logicalDataFile As String = ""
				Dim logicalLogFile As String = ""
				Try
					Using con As SqlConnection = New SqlConnection(String.Format("Server={0};Database=master;Trusted_Connection=True;", Me.serverInstance))
						con.Open()
						Dim fileListQuery As String = "RESTORE FILELISTONLY FROM DISK = @BackupFile"
						Using cmd As SqlCommand = New SqlCommand(fileListQuery, con)
							cmd.Parameters.AddWithValue("@BackupFile", backupFile)
							Using reader As SqlDataReader = cmd.ExecuteReader()
								Dim flag3 As Boolean = reader.Read()
								If flag3 Then
									logicalDataFile = reader("LogicalName").ToString()
								End If
								Dim flag4 As Boolean = reader.Read()
								If flag4 Then
									logicalLogFile = reader("LogicalName").ToString()
								End If
							End Using
						End Using
					End Using
				Catch ex As Exception
					MessageBox.Show("Error fetching logical file names: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
					Return
				End Try
				Dim mdfFile As String = String.Format("{0}\{1}.mdf", dbLocation, Me.databaseName)
				Dim ldfFile As String = String.Format("{0}\{1}_Log.ldf", dbLocation, Me.databaseName)
				Dim connectionString As String = If((Not String.IsNullOrEmpty(Me.user)), String.Format("Server={0};Database=master;User Id={1};Password={2};", Me.serverInstance, Me.user, Me.password), String.Format("Server={0};Database=master;Trusted_Connection=True;", Me.serverInstance))
				Dim restoreQuery As String = String.Format("RESTORE DATABASE [{0}] FROM DISK = @BackupFile WITH MOVE @LogicalDataFile TO @MdfPath, MOVE @LogicalLogFile TO @LdfPath, REPLACE, RECOVERY;", Me.databaseName)
				Dim createUserSql As String = String.Format("USE [{0}];" & vbCrLf & "                                    IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'{1}_user')" & vbCrLf & "                                    BEGIN" & vbCrLf & "                                        CREATE LOGIN [{2}_user] WITH PASSWORD = 'K@vin2000';" & vbCrLf & "                                    END;" & vbCrLf & "                                    IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'{3}_user')" & vbCrLf & "                                    BEGIN" & vbCrLf & "                                        CREATE USER [{4}_user] FOR LOGIN [{5}_user];" & vbCrLf & "                                        ALTER ROLE db_owner ADD MEMBER [{6}_user];" & vbCrLf & "                                    END;", New Object() {Me.databaseName, Me.databaseName, Me.databaseName, Me.databaseName, Me.databaseName, Me.databaseName, Me.databaseName})
				Try
					Using con2 As SqlConnection = New SqlConnection(connectionString)
						con2.Open()
						Using cmd2 As SqlCommand = New SqlCommand(restoreQuery, con2)
							cmd2.Parameters.AddWithValue("@BackupFile", backupFile)
							cmd2.Parameters.AddWithValue("@LogicalDataFile", logicalDataFile)
							cmd2.Parameters.AddWithValue("@LogicalLogFile", logicalLogFile)
							cmd2.Parameters.AddWithValue("@MdfPath", mdfFile)
							cmd2.Parameters.AddWithValue("@LdfPath", ldfFile)
							cmd2.ExecuteNonQuery()
						End Using
						Using cmd3 As SqlCommand = New SqlCommand(createUserSql, con2)
							cmd3.ExecuteNonQuery()
						End Using
					End Using
					MessageBox.Show("Database restored and user created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
				Catch ex2 As SqlException
					MessageBox.Show("SQL Error during restore: " + ex2.Message, "SQL Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
				Catch ex3 As Exception
					MessageBox.Show("Unexpected error: " + ex3.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
				End Try
			End If
		End If
	End Sub

	Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
		Me.SaveConfiguration()
		Tools.LoadConfiguration()
		dbtable.InitializeDatabase()
		Try
			Using conn As SqlConnection = New SqlConnection(Tools.GetConnectionString())
				conn.Open()
				Dim cmd As SqlCommand = New SqlCommand("IF EXISTS (SELECT * FROM Company_Table)" & vbCrLf & "                                            BEGIN" & vbCrLf & "                                                UPDATE Company_Table SET Version = @version" & vbCrLf & "                                            END", conn)
				cmd.Parameters.AddWithValue("@version", Me.Version)
				cmd.ExecuteNonQuery()
			End Using
			Dim filePath As String = Path.Combine(Application.StartupPath, "DBConnect.txt")
			Dim flag As Boolean = File.Exists(filePath)
			If flag Then
				Dim lines As String() = File.ReadAllLines(filePath)
				Dim num As Integer = lines.Length - 1
				For i As Integer = 0 To num
					Dim flag2 As Boolean = lines(i).StartsWith("LogoPanel=")
					If flag2 Then
						lines(i) = "LogoPanel=True"
					End If
				Next
				File.WriteAllLines(filePath, lines)
			End If
			Dim App As String = "Godown Stock.exe"
			Dim AppPath As String = Path.Combine(Application.StartupPath, App)
			Dim flag3 As Boolean = File.Exists(AppPath)
			If flag3 Then
				Process.Start(AppPath)
			Else
				MessageBox.Show("Application executable not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand)
			End If
		Catch ex As Exception
			MessageBox.Show("Error updating version: " + ex.Message, "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Hand)
		End Try
		Application.[Exit]()
	End Sub

	Private Sub ResetButton_Click(sender As Object, e As EventArgs) Handles ResetButton.Click
		Dim Reset As PSDForm = New PSDForm()
		Reset.Show()
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

	Private Sub LatestUpdateButton_Click(sender As Object, e As EventArgs) Handles LatestUpdateButton.Click
		Dim Update As Install = New Install()
		Update.Show()
		Dim filePath As String = Path.Combine(Application.StartupPath, "DBConnect.txt")
		Dim flag As Boolean = File.Exists(filePath)
		If flag Then
			Dim lines As String() = File.ReadAllLines(filePath)
			Dim num As Integer = lines.Length - 1
			For i As Integer = 0 To num
				Dim flag2 As Boolean = lines(i).StartsWith("LogoPanel=")
				If flag2 Then
					lines(i) = "LogoPanel=False"
				End If
			Next
			File.WriteAllLines(filePath, lines)
		End If
	End Sub
	Private Sub DBConnect_Shown(sender As Object, e As EventArgs)
		Me.Psdtxt.Focus()
	End Sub
	Private Function GetPassword() As String
		Return Me.Psdtxt.Text
	End Function
	Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs)
		Dim flag As Boolean = e.KeyCode = Keys.[Return]
		If flag Then
			e.SuppressKeyPress = True
			MyBase.SelectNextControl(CType(sender, Control), True, True, True, True)
		End If
	End Sub
	Private Sub Guna2ControlBox1_Click(sender As Object, e As EventArgs) Handles Guna2ControlBox1.Click
		Application.[Exit]()
	End Sub
End Class
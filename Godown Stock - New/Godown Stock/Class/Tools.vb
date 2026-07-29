Imports System.Data.SqlClient
Imports System.Data.Sql
Imports System.IO
Imports System.Configuration
Imports Guna.UI2.WinForms
Imports System.Xml.Linq
Imports System.Globalization
Imports System.Security.Cryptography
Imports System.Text
Imports System.Management
Imports System.Net
Imports System.Web.Script.Serialization

Public Class Tools
    Private Shared serverInstance As String
    Private Shared databaseName As String
    Private Shared user As String
    Private Shared password As String
    Private Shared backupPath As String
    Private Shared configLoaded As Boolean = False
    Private DBConn As SqlConnection
    Private Shared ReadOnly xmlPath As String = Path.Combine(Application.StartupPath, "GS Key", "activation.xml")

    Private Shared ReadOnly GitURL As String = "https://api.github.com/repos/Kavinbala1072/Reporting/contents/BK%20Reporting.json"
    Private Shared ReadOnly GitToken As String = "A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"

    Private Shared ReadOnly ActualGitToken As String = GitToken.Substring(1)
    Public Class GitFileResponse
        Public Property content As String
        Public Property sha As String
    End Class

    Public Class CompanyLoginInfo
        Public Property Application As String
        Public Property CompanyName As String
        Public Property LastLogin As String
    End Class

    Public Shared Sub LoadConfiguration()
        If configLoaded Then Exit Sub
        Dim filePath As String = Path.Combine(Application.StartupPath, "DBConnect.txt")
        If File.Exists(filePath) Then
            Dim lines As String() = File.ReadAllLines(filePath)
            For Each line As String In lines
                If line.StartsWith("SQLServer=") Then
                    serverInstance = line.Replace("SQLServer=", "").Trim()
                ElseIf line.StartsWith("SQLDBName=") Then
                    databaseName = line.Replace("SQLDBName=", "").Trim()
                ElseIf line.StartsWith("SQLUsername=") Then
                    user = line.Replace("SQLUsername=", "").Trim()
                ElseIf line.StartsWith("SQLPassword=") Then
                    password = line.Replace("SQLPassword=", "").Trim()
                ElseIf line.StartsWith("BackupPath=") Then
                    backupPath = line.Replace("BackupPath=", "").Trim()
                End If
            Next
            configLoaded = True
        Else
            MessageBox.Show("Configuration file not found.")
        End If
    End Sub
    Public Shared Function GetConnectionString() As String
        LoadConfiguration()
        Return $"Server={serverInstance};Database={databaseName};User Id={user};Password={password};"
    End Function
    Public Shared Function GetConnection() As SqlConnection
        Return New SqlConnection(GetConnectionString())
    End Function
    Public Shared Function GetStoredUsername() As String
        Dim ctlDesc As String = "UserName"
        Dim storedUsername As String = ""
        Dim storedUserID As String = ""

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()

            Using command As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = @CtlDesc", sqlconnect)
                command.Parameters.AddWithValue("@CtlDesc", ctlDesc)

                Dim result = command.ExecuteScalar()
                If result IsNot Nothing Then
                    storedUsername = result.ToString()
                End If
            End Using

            Using query As New SqlCommand("SELECT ID FROM user_table WHERE User_Name = @storedUsername", sqlconnect)
                query.Parameters.AddWithValue("@storedUsername", storedUsername)

                Dim queryResult = query.ExecuteScalar()
                If queryResult IsNot Nothing Then
                    storedUserID = queryResult.ToString()
                End If
            End Using
        End Using

        Return storedUserID
    End Function
    Public Function CreateDB(Server As String, Database As String, Optional DataPath As String = "") As Boolean
        If String.IsNullOrWhiteSpace(Server) Then Throw New Exception("Database Creation Error: Server Not Specified.")

        Dim query As String
        If String.IsNullOrWhiteSpace(DataPath) Then
            query = $"CREATE DATABASE [{Database}]; ALTER DATABASE [{Database}] SET AUTO_CLOSE OFF; ALTER DATABASE [{Database}] SET RECOVERY FULL;"
        Else
            query = $"CREATE DATABASE [{Database}] ON (NAME = N'{Database}', FILENAME = '{DataPath}{Database}.mdf'); ALTER DATABASE [{Database}] SET AUTO_CLOSE OFF; ALTER DATABASE [{Database}] SET RECOVERY FULL;"
        End If

        Try
            DBExecQuery(query)
            CloseDB()
            Return True
        Catch ex As Exception
            Throw New Exception($"Database Creation Failed: {ex.Message}")
        End Try
    End Function
    Public Function DBExecQuery(query As String, Optional Timeout As Integer = 2000) As Boolean
        If DBConn Is Nothing Then Throw New Exception("Database connection is not initialized.")

        Using sqlCommand As New SqlCommand(query, DBConn)
            sqlCommand.CommandTimeout = Timeout
            Try
                sqlCommand.ExecuteNonQuery()
                Return True
            Catch ex As SqlException
                Throw New Exception($"SQL Execution Error: {ex.Message}")
            End Try
        End Using
    End Function
    Public Sub CloseDB()
        Try
            If DBConn IsNot Nothing Then
                DBConn.Close()
                SqlConnection.ClearPool(DBConn)
                DBConn.Dispose()
                DBConn = Nothing
            End If
        Catch ex As Exception
            Throw New Exception($"Database Error: {ex.Message}")
        End Try
    End Sub
    Public Function VerifyDB(ServerName As String, DbName As String, Optional ByRef ErrorTable As DataTable = Nothing, Optional WithMultiUser As Boolean = False, Optional sUserName As String = "", Optional sUserPass As String = "") As Boolean
        Dim result As Boolean = False

        ' Safely check if Hlbl is provided
        'If Hlbl IsNot Nothing Then Hlbl.Visible = True

        Dim connectionString As String = $"Data Source={ServerName};Initial Catalog=master;User ID={sUserName};Password={sUserPass};Connection Timeout=2000"

        Using sqlConnection As New SqlConnection(connectionString)
            Dim dataTable As New DataTable()
            Dim sqlDataAdapter As New SqlDataAdapter()

            Try
                sqlConnection.Open()

                If WithMultiUser Then
                    Using setSingleUser As New SqlCommand($"ALTER DATABASE [{DbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", sqlConnection)
                        setSingleUser.ExecuteNonQuery()
                    End Using
                End If

                Using sqlCommand As New SqlCommand($"DBCC CHECKDB([{DbName}]) WITH TABLERESULTS, NO_INFOMSGS", sqlConnection)
                    sqlCommand.CommandTimeout = 6000
                    sqlDataAdapter.SelectCommand = sqlCommand
                    sqlDataAdapter.Fill(dataTable)
                End Using

                If WithMultiUser Then
                    Using setMultiUser As New SqlCommand($"ALTER DATABASE [{DbName}] SET MULTI_USER", sqlConnection)
                        setMultiUser.ExecuteNonQuery()
                    End Using
                End If

                ErrorTable = dataTable
                result = dataTable.Rows.Count = 0

                ' If Hlbl IsNot Nothing Then Hlbl.Visible = False

            Catch ex As Exception
                If WithMultiUser Then
                    Try
                        Using setMultiUser As New SqlCommand($"ALTER DATABASE [{DbName}] SET MULTI_USER", sqlConnection)
                            setMultiUser.ExecuteNonQuery()
                        End Using
                    Catch ex2 As Exception
                        ' Optional: log ex2
                    End Try
                End If
                Throw New Exception($"Verification failed: {ex.Message}")
            End Try
        End Using

        Return result
    End Function
    Public Sub OpenDB(Server As String, Database As String, puserid As String, ppassword As String, Optional MARS As Boolean = False, Optional Timeout As Integer = 60)
        If String.IsNullOrWhiteSpace(Server) Then Throw New Exception("Server Not Specified.")

        Dim connectionString As String = $"Data Source={Server};Connection Timeout={Timeout};" &
                                         If(String.IsNullOrWhiteSpace(Database),
                                            "Trusted_Connection=True;",
                                            $"Initial Catalog={Database};User ID={puserid};Password={ppassword};") &
                                         $"MultipleActiveResultSets={(If(MARS, "True", "False"))}"

        DBConn = New SqlConnection(connectionString)

        Try
            DBConn.Open()
            Using sqlCommand As New SqlCommand("SET DATEFORMAT dmy", DBConn)
                sqlCommand.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Throw New Exception($"Connection Error: {ex.Message}", ex)
        End Try
    End Sub
    Public Shared Function GetBackupPath() As String
        LoadConfiguration()
        Return backupPath
    End Function
    Public Shared Function GetDatabaseName() As String
        LoadConfiguration()
        Return databaseName
    End Function
    Public Shared Async Function BackupAsync() As Task
        LoadConfiguration()

        If String.IsNullOrWhiteSpace(serverInstance) OrElse
           String.IsNullOrWhiteSpace(databaseName) OrElse
           String.IsNullOrWhiteSpace(backupPath) Then
            Throw New Exception("Missing backup configuration.")
        End If

        If Not Directory.Exists(backupPath) Then
            Directory.CreateDirectory(backupPath)
        End If

        Dim timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss")
        Dim backupFilePath = Path.Combine(backupPath, $"{databaseName}_Backup_{timestamp}.bak")

        Dim connectionString = $"Server={serverInstance};Database=master;User Id={user};Password={password};"

        Dim backupQuery = $"BACKUP DATABASE [{databaseName}] TO DISK = @backupPath WITH INIT"

        Await Task.Run(Sub()
                           Using conn As New SqlConnection(connectionString)
                               Using cmd As New SqlCommand(backupQuery, conn)
                                   cmd.Parameters.AddWithValue("@backupPath", backupFilePath)
                                   conn.Open()
                                   cmd.ExecuteNonQuery()
                               End Using
                           End Using
                       End Sub)
    End Function

    Public Shared Function IsLicenseValid() As Boolean
        If Not File.Exists(xmlPath) Then Return False

        Try
            Dim xml = XElement.Load(xmlPath)

            Dim toDateStr = xml.Element("ToDate")?.Value
            If String.IsNullOrEmpty(toDateStr) Then Return False

            Dim toDate As Date
            If Not Date.TryParseExact(toDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, toDate) Then
                Return False
            End If

            If Date.Today > toDate Then
                MessageBox.Show("License has expired.", "Activation Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                File.Delete(xmlPath)
                Return False
            End If

            Dim key1 = Integer.Parse(xml.Element("KeyPart1").Value)
            Dim key2 = Integer.Parse(xml.Element("KeyPart2").Value)
            Dim key3 = Integer.Parse(xml.Element("KeyPart3").Value)

            Dim identifier = GetSystemIdentifier()
            Dim hashed = GetHashedNumbers(identifier)

            Dim base1 = hashed.Part1
            Dim base2 = hashed.Part2
            Dim base3 = hashed.Part3

            Dim activationDate As Date = Date.ParseExact(xml.Element("Date").Value, "yyyy-MM-dd", CultureInfo.InvariantCulture)
            Dim day = activationDate.Day
            Dim month = activationDate.Month
            Dim year = activationDate.Year

            Return (key1 = base1 + day AndAlso key2 = base2 + month AndAlso key3 = base3 + year)
        Catch
            Return False
        End Try
    End Function

    Private Shared Function GetSystemIdentifier() As String
        Dim processorId As String = ""
        Dim motherboardSerial As String = ""

        Try
            Dim searcher1 As New ManagementObjectSearcher("select ProcessorId from Win32_Processor")
            For Each obj In searcher1.Get()
                processorId = obj("ProcessorId").ToString()
                Exit For
            Next

            Dim searcher2 As New ManagementObjectSearcher("select SerialNumber from Win32_BaseBoard")
            For Each obj In searcher2.Get()
                motherboardSerial = obj("SerialNumber").ToString()
                Exit For
            Next
        Catch
        End Try

        Return processorId & motherboardSerial
    End Function
    Private Structure KeyParts
        Public Part1 As Integer
        Public Part2 As Integer
        Public Part3 As Integer
    End Structure

    Private Shared Function GetHashedNumbers(identifier As String) As KeyParts
        Dim sha256 As SHA256 = SHA256.Create()
        Dim hashBytes() As Byte = sha256.ComputeHash(Encoding.UTF8.GetBytes(identifier))

        Dim kp As KeyParts
        kp.Part1 = 10000 + (BitConverter.ToUInt16(hashBytes, 0) Mod 90000)
        kp.Part2 = 10000 + (BitConverter.ToUInt16(hashBytes, 4) Mod 90000)
        kp.Part3 = 10000 + (BitConverter.ToUInt16(hashBytes, 8) Mod 90000)
        Return kp
    End Function
    Public Shared Function Userupdate(username As String) As Boolean

        Dim ctlDesc As String = "UserName"

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()

            Dim checkExistCommand As New SqlCommand("SELECT ID FROM Control_Table WHERE Ctl_Desc = @CtlDesc", sqlconnect)
            checkExistCommand.Parameters.AddWithValue("@CtlDesc", ctlDesc)
            Dim result = checkExistCommand.ExecuteScalar()

            If result IsNot Nothing Then
                Dim ID As Integer = Convert.ToInt32(result)
                Dim updateCommand As New SqlCommand("UPDATE Control_Table SET Ctl_Value = @CtlValue WHERE ID = @ID", sqlconnect)
                updateCommand.Parameters.AddWithValue("@CtlValue", username)
                updateCommand.Parameters.AddWithValue("@ID", ID)
                updateCommand.ExecuteNonQuery()
            Else
                Dim insertCommand As New SqlCommand("INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES (@CtlDesc, @CtlValue)", sqlconnect)
                insertCommand.Parameters.AddWithValue("@CtlDesc", ctlDesc)
                insertCommand.Parameters.AddWithValue("@CtlValue", username)
                insertCommand.ExecuteNonQuery()
            End If
        End Using

        Return True
    End Function
    Public Shared Function UpdateCheckDate() As Boolean
        Dim ctlDesc As String = "LastCheckDate"
        Dim today As String = DateTime.Today.ToString("dd-MM-yyyy")

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()

            Dim checkExistCommand As New SqlCommand("SELECT ID FROM Control_Table WHERE Ctl_Desc = @CtlDesc", sqlconnect)
            checkExistCommand.Parameters.AddWithValue("@CtlDesc", ctlDesc)
            Dim result = checkExistCommand.ExecuteScalar()

            If result IsNot Nothing Then
                Dim ID As Integer = Convert.ToInt32(result)
                Dim updateCommand As New SqlCommand("UPDATE Control_Table SET Ctl_Value = @CtlValue WHERE ID = @ID", sqlconnect)
                updateCommand.Parameters.AddWithValue("@CtlValue", today)
                updateCommand.Parameters.AddWithValue("@ID", ID)
                updateCommand.ExecuteNonQuery()
            Else
                Dim insertCommand As New SqlCommand("INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES (@CtlDesc, @CtlValue)", sqlconnect)
                insertCommand.Parameters.AddWithValue("@CtlDesc", ctlDesc)
                insertCommand.Parameters.AddWithValue("@CtlValue", today)
                insertCommand.ExecuteNonQuery()
            End If
        End Using
        Return True
    End Function

    Public Shared Function IsHealthCheckDoneToday() As Boolean
        Dim ctlDesc As String = "LastCheckDate"
        Dim backupDate As DateTime

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()

            Dim command As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = @CtlDesc", sqlconnect)
            command.Parameters.AddWithValue("@CtlDesc", ctlDesc)
            Dim result = command.ExecuteScalar()

            If result IsNot Nothing AndAlso Date.TryParse(result.ToString(), backupDate) Then
                If backupDate.Date = DateTime.Today Then
                    Return True
                End If
            End If
        End Using
        Return False
    End Function
    Public Shared Function IsBackupDoneToday() As Boolean
        Dim ctlDesc As String = "LastBackupDate"
        Dim backupDate As DateTime

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()

            Dim command As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = @CtlDesc", sqlconnect)
            command.Parameters.AddWithValue("@CtlDesc", ctlDesc)
            Dim result = command.ExecuteScalar()

            If result IsNot Nothing AndAlso Date.TryParse(result.ToString(), backupDate) Then
                If backupDate.Date = DateTime.Today Then
                    Return True
                End If
            End If
        End Using

        Return False
    End Function
    Public Shared Function UpdateBackupDate() As Boolean
        Dim ctlDesc As String = "LastBackupDate"
        Dim today As String = DateTime.Today.ToString("dd-MM-yyyy")

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()

            Dim checkExistCommand As New SqlCommand("SELECT ID FROM Control_Table WHERE Ctl_Desc = @CtlDesc", sqlconnect)
            checkExistCommand.Parameters.AddWithValue("@CtlDesc", ctlDesc)
            Dim result = checkExistCommand.ExecuteScalar()

            If result IsNot Nothing Then
                Dim ID As Integer = Convert.ToInt32(result)
                Dim updateCommand As New SqlCommand("UPDATE Control_Table SET Ctl_Value = @CtlValue WHERE ID = @ID", sqlconnect)
                updateCommand.Parameters.AddWithValue("@CtlValue", today)
                updateCommand.Parameters.AddWithValue("@ID", ID)
                updateCommand.ExecuteNonQuery()
            Else
                Dim insertCommand As New SqlCommand("INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES (@CtlDesc, @CtlValue)", sqlconnect)
                insertCommand.Parameters.AddWithValue("@CtlDesc", ctlDesc)
                insertCommand.Parameters.AddWithValue("@CtlValue", today)
                insertCommand.ExecuteNonQuery()
            End If
        End Using

        Return True
    End Function

    'Public Shared Sub UpdateCloudLog(ByVal serverType As String, ByVal appName As String, ByVal compName As String)
    '    Try
    '        ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)
    '        Dim js As New JavaScriptSerializer()

    '        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(GitURL), HttpWebRequest)
    '        request.Headers.Add("Authorization", "token " & GitToken)
    '        request.UserAgent = "WinForms_App"

    '        Dim currentSha As String = ""
    '        Dim appList As New List(Of Dictionary(Of String, Object))

    '        Try
    '            Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
    '                Using reader As New StreamReader(response.GetResponseStream())
    '                    Dim gitRes = js.Deserialize(Of GitFileResponse)(reader.ReadToEnd())
    '                    currentSha = gitRes.sha

    '                    Dim jsonArrayRaw As String = Encoding.UTF8.GetString(Convert.FromBase64String(gitRes.content))
    '                    appList = js.Deserialize(Of List(Of Dictionary(Of String, Object)))(jsonArrayRaw)
    '                End Using
    '            End Using
    '        Catch ex As WebException
    '            ' If file doesn't exist, start with an empty list
    '            appList = New List(Of Dictionary(Of String, Object))
    '        End Try

    '        Dim existingEntry = appList.FirstOrDefault(Function(x) x.ContainsKey("Application") AndAlso x("Application").ToString() = appName)

    '        If existingEntry IsNot Nothing Then
    '            ' Alter existing values
    '            existingEntry("server") = serverType
    '            existingEntry("Company name") = compName
    '            existingEntry("lastlogin") = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
    '        Else
    '            ' Create new entry if not found
    '            Dim newEntry As New Dictionary(Of String, Object) From {
    '                {"server", serverType},
    '                {"Application", appName},
    '                {"Company name", compName},
    '                {"lastlogin", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}
    '            }
    '            appList.Add(newEntry)
    '        End If

    '        ' --- 3. PUSH UPDATED ARRAY BACK TO GITHUB ---
    '        Dim updatedJson As String = js.Serialize(appList)
    '        Dim base64Content As String = Convert.ToBase64String(Encoding.UTF8.GetBytes(updatedJson))

    '        Dim putRequest As HttpWebRequest = DirectCast(WebRequest.Create(GitURL), HttpWebRequest)
    '        putRequest.Method = "PUT"
    '        putRequest.Headers.Add("Authorization", "token " & GitToken)
    '        putRequest.UserAgent = "WinForms_App"
    '        putRequest.ContentType = "application/json"

    '        Dim payload As New With {
    '            .message = "Login update for " & appName,
    '            .content = base64Content,
    '            .sha = currentSha
    '        }

    '        Dim bodyBytes As Byte() = Encoding.UTF8.GetBytes(js.Serialize(payload))
    '        Using stream As Stream = putRequest.GetRequestStream()
    '            stream.Write(bodyBytes, 0, bodyBytes.Length)
    '        End Using

    '        putRequest.GetResponse().Close()

    '    Catch ex As Exception
    '        Debug.WriteLine("Sync Error: " & ex.Message)
    '    End Try
    'End Sub
    Public Shared Function IsVersionValid(expectedVersion As String) As Boolean
        Try
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()
                Dim cmd As New SqlCommand("SELECT TOP 1 Version FROM Company_Table", sqlconnect)
                Dim actualVersion As Object = cmd.ExecuteScalar()

                If actualVersion IsNot Nothing Then
                    Return String.Equals(actualVersion.ToString().Trim(), expectedVersion.Trim(), StringComparison.OrdinalIgnoreCase)
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error checking version: " & ex.Message, "Version Check Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return False
    End Function
    Public Shared Function CheckForUpdate(ByVal appName As String, ByVal compNo As String) As String
        Try
            ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)
            Dim js As New JavaScriptSerializer()

            Dim request As HttpWebRequest = DirectCast(WebRequest.Create(GitURL), HttpWebRequest)
            request.Headers.Add("Authorization", "token " & ActualGitToken)
            request.UserAgent = "WinForms_App"

            Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(response.GetResponseStream())
                    Dim gitRes = js.Deserialize(Of GitFileResponse)(reader.ReadToEnd())
                    Dim jsonArrayRaw As String = Encoding.UTF8.GetString(Convert.FromBase64String(gitRes.content))
                    Dim appList = js.Deserialize(Of List(Of Dictionary(Of String, Object)))(jsonArrayRaw)

                    Dim entry = appList.FirstOrDefault(Function(x)
                                                           Return x.ContainsKey("Application") AndAlso x("Application").ToString() = appName AndAlso
                           x.ContainsKey("CompNo") AndAlso x("CompNo").ToString() = compNo
                                                       End Function)

                    If entry IsNot Nothing AndAlso entry.ContainsKey("NewVersion") Then
                        Return entry("NewVersion").ToString()
                    End If
                End Using
            End Using
        Catch ex As Exception
            Debug.WriteLine("Update Check Failed: " & ex.Message)
        End Try
        Return "" ' Return empty if no update found or error occurs
    End Function

    Public Shared Sub UpdateCloudLog(ByVal isServer As Boolean, ByVal CompanyNo As String, ByVal appName As String, ByVal compName As String, ByVal amcExpiry As String, ByVal version As String, ByVal secretPassword As String)
        Try
            ServicePointManager.SecurityProtocol = DirectCast(3072, SecurityProtocolType)
            Dim js As New JavaScriptSerializer()

            Dim serverTypeText As String = If(isServer, "Server", "Node")
            Dim CompNo As String = "BK0001"

            Dim request As HttpWebRequest = DirectCast(WebRequest.Create(GitURL), HttpWebRequest)
            request.Headers.Add("Authorization", "token " & ActualGitToken)
            request.UserAgent = "WinForms_App"

            Dim currentSha As String = ""
            Dim appList As New List(Of Dictionary(Of String, Object))

            Try
                Using response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
                    Using reader As New StreamReader(response.GetResponseStream())
                        Dim gitRes = js.Deserialize(Of GitFileResponse)(reader.ReadToEnd())
                        currentSha = gitRes.sha

                        Dim jsonArrayRaw As String = Encoding.UTF8.GetString(Convert.FromBase64String(gitRes.content))
                        appList = js.Deserialize(Of List(Of Dictionary(Of String, Object)))(jsonArrayRaw)
                    End Using
                End Using
            Catch ex As WebException
                appList = New List(Of Dictionary(Of String, Object))
            End Try

            Dim existingEntry = appList.FirstOrDefault(Function(x) x.ContainsKey("Application") AndAlso
                                                           x("Application").ToString() = appName AndAlso
                                                           x.ContainsKey("Company name") AndAlso
                                                           x("Company name").ToString() = compName)

            If existingEntry IsNot Nothing Then
                existingEntry("server") = serverTypeText
                existingEntry("CompNo") = CompNo
                existingEntry("Company name") = compName
                existingEntry("lastlogin") = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")
                existingEntry("AMC expiry") = amcExpiry
                existingEntry("Version") = version
                existingEntry("secretPassword") = secretPassword
            Else
                Dim newEntry As New Dictionary(Of String, Object) From {
                    {"server", serverTypeText},
                    {"CompNo", CompNo},
                    {"Application", appName},
                    {"Company name", compName},
                    {"lastlogin", DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")},
                    {"AMC expiry", amcExpiry},
                    {"Version", version},
                    {"secretPassword", secretPassword}
                }
                appList.Add(newEntry)
            End If

            Dim updatedJson As String = js.Serialize(appList)
            Dim base64Content As String = Convert.ToBase64String(Encoding.UTF8.GetBytes(updatedJson))

            Dim putRequest As HttpWebRequest = DirectCast(WebRequest.Create(GitURL), HttpWebRequest)
            putRequest.Method = "PUT"
            putRequest.Headers.Add("Authorization", "token " & ActualGitToken)
            putRequest.UserAgent = "WinForms_App"
            putRequest.ContentType = "application/json"

            Dim payload As New With {
                .message = "Sync update for " & compName & " (" & appName & ")",
                .content = base64Content,
                .sha = currentSha
            }

            Dim bodyBytes As Byte() = Encoding.UTF8.GetBytes(js.Serialize(payload))
            Using stream As Stream = putRequest.GetRequestStream()
                stream.Write(bodyBytes, 0, bodyBytes.Length)
            End Using

            putRequest.GetResponse().Close()

        Catch ex As Exception
            Debug.WriteLine("Sync Error: " & ex.Message)
        End Try
    End Sub
End Class

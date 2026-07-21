Imports System.Data.SqlClient
Imports System.IO

Public Class Tools
    Private Shared server As String
    Private Shared db As String
    Private Shared _isWebviewEnabled As Boolean = False
    Private Shared configLoaded As Boolean = False

    Public Shared Sub LoadConfiguration(Optional forceReload As Boolean = True)
        If configLoaded And Not forceReload Then Exit Sub

        Dim filePath As String = Path.Combine(Application.StartupPath, "DBConnect.txt")

        If File.Exists(filePath) Then
            Dim lines As String() = File.ReadAllLines(filePath)
            For Each line In lines
                Dim parts = line.Split("="c)
                If parts.Length < 2 Then Continue For

                Dim key As String = parts(0).Trim()
                Dim value As String = parts(1).Trim()

                Select Case key
                    Case "SQLServer" : server = value
                    Case "SQLDBName" : db = value
                    Case "Webview"
                        Boolean.TryParse(value, _isWebviewEnabled)
                End Select
            Next
            configLoaded = True
        End If
    End Sub

    Public Shared Function GetConnectionString() As String
        LoadConfiguration()
        Return $"Server={server};Database={db};Integrated Security=True;"
    End Function

    Public Shared Function IsWebviewEnabled() As Boolean
        LoadConfiguration()
        Return _isWebviewEnabled
    End Function

    Public Shared Function GetConnection() As SqlConnection
        Return New SqlConnection(GetConnectionString())
    End Function
End Class
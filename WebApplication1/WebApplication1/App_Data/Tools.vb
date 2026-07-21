Imports System.Data.SqlClient

Public Class Tools
    Public Shared Function GetConnectionString(server As String, db As String, user As String, pass As String) As String
        Return $"Server={server.Trim()};Database={db.Trim()};User Id={user.Trim()};Password={pass.Trim()};Persist Security Info=False;Connect Timeout=100;"
    End Function

    Public Shared Function CheckConnection(server As String, db As String, user As String, pass As String) As Boolean
        Try
            Using conn As New SqlConnection(GetConnectionString(server, db, user, pass))
                conn.Open()
                Return True
            End Using
        Catch
            Return False
        End Try
    End Function
End Class
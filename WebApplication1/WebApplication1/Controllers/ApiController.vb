Imports System.Data.SqlClient
Imports System.Web.Http
Imports System.Collections.Generic
Imports System.Data

Public Class MyApiController
    Inherits ApiController

    Public Class DataModel
        Public Property Id As Integer
        Public Property Name As String
    End Class

    '<HttpGet>
    '<Route("api/data")>
    'Public Function GetData() As IHttpActionResult
    '    Try
    '        Dim dataList As New List(Of DataModel)()

    '        Using conn As SqlConnection = Tools.GetConnection()
    '            ' Explicitly name columns to avoid issues if table schema changes
    '            Dim sql As String = "SELECT TOP 10 UserID, UserName FROM YourTableName"
    '            Using cmd As New SqlCommand(sql, conn)
    '                conn.Open()
    '                Using reader As SqlDataReader = cmd.ExecuteReader()
    '                    While reader.Read()
    '                        dataList.Add(New DataModel With {
    '                            .Id = Convert.ToInt32(reader("UserID")),
    '                            .Name = reader("UserName").ToString()
    '                        })
    '                    End While
    '                End Using
    '            End Using
    '        End Using

    '        Return Ok(dataList)
    '    Catch ex As Exception
    '        ' Log the exception (ex) here
    '        Return InternalServerError(New Exception("An error occurred while fetching data."))
    '    End Try
    'End Function

    '<HttpPost>
    '<Route("api/login")>
    'Public Function Login(<FromBody> loginRequest As LoginModel) As IHttpActionResult
    '    If loginRequest Is Nothing OrElse String.IsNullOrEmpty(loginRequest.Username) Then
    '        Return BadRequest("Invalid request.")
    '    End If

    '    Try
    '        Using conn As SqlConnection = Tools.GetConnection()
    '            ' In a real app, you would select the HashedPassword and verify it in code
    '            Dim sql As String = "SELECT COUNT(*) FROM Users WHERE Username = @u AND Password = @p"

    '            Using cmd As New SqlCommand(sql, conn)
    '                ' Explicitly defining types prevents SQL injection and improves performance
    '                cmd.Parameters.Add("@u", SqlDbType.NVarChar, 50).Value = loginRequest.Username
    '                cmd.Parameters.Add("@p", SqlDbType.NVarChar, 50).Value = loginRequest.Password

    '                conn.Open()
    '                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

    '                If count > 0 Then
    '                    ' In a professional API, you would return a JWT Token here instead of just "Success"
    '                    Return Ok(New With {.success = True, .message = "Login successful"})
    '                Else
    '                    Return Content(System.Net.HttpStatusCode.Unauthorized, New With {.success = False, .message = "Invalid credentials"})
    '                End If
    '            End Using
    '        End Using
    '    Catch ex As Exception
    '        Return InternalServerError(ex)
    '    End Try
    'End Function

End Class
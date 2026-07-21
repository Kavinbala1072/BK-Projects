Imports System.Net
Imports System.IO
Imports System.Web.Script.Serialization
Imports System.Text

Public Class UserGuard

    Public Const GitToken As String = "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"
    Public Const GitURL As String = "https://api.github.com/repos/Kavinbala1072/Reporting/contents/Login.json"

#Region "Global Models"
    Public Class UserEntry
        Public Property User_Code As String
        Public Property Password As String
        Public Property Active As String
    End Class

    Public Class GitHubData
        Public Property users As List(Of UserEntry)
    End Class

    Public Class GitFileResponse
        Public Property sha As String
        Public Property content As String
    End Class

    Private Shared ReadOnly Property MySystemID As String
        Get
            Return System.Environment.MachineName
        End Get
    End Property
#End Region

    Public Shared Sub ValidateSession(ByVal pg As System.Web.UI.Page)

        Dim uCode As String = Convert.ToString(pg.Session("UserName"))

        Dim sessionWebID As String = Convert.ToString(pg.Session("WebID"))

        If String.IsNullOrEmpty(uCode) OrElse String.IsNullOrEmpty(sessionWebID) Then
            pg.Response.Redirect("Default.aspx")
            Return
        End If

        Try
            Dim gitFile = FetchRawGitFile()
            Dim data = DeserialiseGitData(gitFile.content)
            Dim user = data.users.FirstOrDefault(Function(x) x.User_Code.Equals(uCode, StringComparison.OrdinalIgnoreCase))

            If user IsNot Nothing Then
                Dim officialID As String = Convert.ToString(user.Active).Trim()

                If officialID <> sessionWebID Then
                    pg.Session.Abandon()
                    pg.Response.Redirect("Default.aspx?msg=SecurityConflict")
                End If
            End If
        Catch
            ' Handle GitHub connection errors
        End Try
    End Sub

    'Public Shared Sub CheckStatus(ByVal page As Page)
    '    Try
    '        System.Net.ServicePointManager.SecurityProtocol = DirectCast(3072, System.Net.SecurityProtocolType)
    '        If page.Session("UserName") Is Nothing Then page.Response.Redirect("Default.aspx")

    '        Dim uCode As String = page.Session("UserName").ToString()
    '        Dim gitFile = FetchRawGitFile()
    '        Dim data = DeserialiseGitData(gitFile.content)
    '        Dim user = data.users.FirstOrDefault(Function(x) x.User_Code.Equals(uCode, StringComparison.OrdinalIgnoreCase))

    '        If user IsNot Nothing AndAlso user.Active = 0 Then
    '            page.Session.Clear()
    '            page.Session.Abandon()
    '            page.Response.Write("<script>alert('Session ended elsewhere.'); window.top.location.href='Default.aspx';</script>")
    '            page.Response.End()
    '        End If
    '    Catch
    '    End Try
    'End Sub

    Public Shared Sub CheckStatus(ByVal page As Page)
        Try
            System.Net.ServicePointManager.SecurityProtocol = DirectCast(3072, System.Net.SecurityProtocolType)

            If page.Session("UserName") Is Nothing Then
                page.Response.Redirect("Default.aspx")
                Exit Sub
            End If

            If page.Session("LastActivity") IsNot Nothing Then
                Dim lastActivity As DateTime = Convert.ToDateTime(page.Session("LastActivity"))
                Dim span As TimeSpan = DateTime.Now - lastActivity

                If span.TotalMinutes > 10 Then
                    page.Session.Clear()
                    page.Session.Abandon()
                    page.Response.Write("<script>alert('Session expired due to 10 minutes of inactivity.'); window.top.location.href='Default.aspx';</script>")
                    page.Response.End()
                    Exit Sub
                End If
            End If

            page.Session("LastActivity") = DateTime.Now

            Dim uCode As String = page.Session("UserName").ToString()
            Dim gitFile = FetchRawGitFile()
            Dim data = DeserialiseGitData(gitFile.content)
            Dim user = data.users.FirstOrDefault(Function(x) x.User_Code.Equals(uCode, StringComparison.OrdinalIgnoreCase))

            If user IsNot Nothing AndAlso user.Active = 0 Then
                page.Session.Clear()
                page.Session.Abandon()
                page.Response.Write("<script>alert('Account deactivated or session ended elsewhere.'); window.top.location.href='Default.aspx';</script>")
                page.Response.End()
            End If

        Catch ex As Exception
            ' It's better not to catch everything silently, but keeping your original logic
        End Try
    End Sub

    Public Shared Function FetchRawGitFile() As GitFileResponse
        Dim request As HttpWebRequest = WebRequest.Create(GitURL)
        request.Headers.Add("Authorization", "token " & GitToken)
        request.UserAgent = "ASP.NET_App"
        Using response As HttpWebResponse = request.GetResponse()
            Using reader As New StreamReader(response.GetResponseStream())
                Return New JavaScriptSerializer().Deserialize(Of GitFileResponse)(reader.ReadToEnd())
            End Using
        End Using
    End Function

    Public Shared Function DeserialiseGitData(ByVal base64 As String) As GitHubData
        Dim jsonString As String = Encoding.UTF8.GetString(Convert.FromBase64String(base64))
        Return New JavaScriptSerializer().Deserialize(Of GitHubData)(jsonString)
    End Function

    Public Shared Sub UpdateRawGitFile(ByVal dataObject As GitHubData, ByVal sha As String)
        Dim json = New JavaScriptSerializer().Serialize(dataObject)
        Dim request As HttpWebRequest = WebRequest.Create(GitURL)
        request.Method = "PUT"
        request.Headers.Add("Authorization", "token " & GitToken)
        request.UserAgent = "ASP.NET_App"
        request.ContentType = "application/json"
        Dim payload = New With {.message = "Status Update", .content = Convert.ToBase64String(Encoding.UTF8.GetBytes(json)), .sha = sha}
        Dim body = Encoding.UTF8.GetBytes(New JavaScriptSerializer().Serialize(payload))
        Using stream = request.GetRequestStream() : stream.Write(body, 0, body.Length) : End Using
        request.GetResponse().Close()
    End Sub
    Public Shared Sub UpdateStatus(ByVal uCode As String, ByVal status As String)
        If String.IsNullOrEmpty(uCode) Then Return
        Try
            System.Net.ServicePointManager.SecurityProtocol = DirectCast(3072, System.Net.SecurityProtocolType)

            Dim gitFile = FetchRawGitFile()
            Dim data = DeserialiseGitData(gitFile.content)

            Dim user = data.users.FirstOrDefault(Function(x) x.User_Code.Equals(uCode, StringComparison.OrdinalIgnoreCase))
            If user IsNot Nothing Then
                user.Active = status

                Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
                Dim json = serializer.Serialize(data)

                Dim request As HttpWebRequest = WebRequest.Create(GitURL)
                request.Method = "PUT"
                request.Headers.Add("Authorization", "token " & GitToken)
                request.UserAgent = "ASP.NET_App"
                request.ContentType = "application/json"

                Dim payload = New With {
                .message = "Status update for " & uCode,
                .content = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json)),
                .sha = gitFile.sha
            }

                Dim body = System.Text.Encoding.UTF8.GetBytes(serializer.Serialize(payload))
                Using stream = request.GetRequestStream()
                    stream.Write(body, 0, body.Length)
                End Using
                request.GetResponse().Close()
            End If
        Catch ex As Exception
            ' Log error or handle failure
        End Try
    End Sub
End Class
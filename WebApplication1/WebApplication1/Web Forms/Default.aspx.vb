Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Script.Serialization ' Requires reference to System.Web.Extensions

Public Class _Default
    Inherits System.Web.UI.Page

    Public Class SqlConfig
        Public Property server As String
        Public Property db As String
        Public Property user As String
        Public Property pass As String
    End Class

    Private ReadOnly Property MySystemID As String
        Get
            Return System.Environment.MachineName
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1))
        Response.Cache.SetNoStore()

        If Not IsPostBack Then
            Dim loadedFromJson As Boolean = LoadConfigFromJson()

            If Not loadedFromJson Then
                If Request.Cookies("SQL_Server") IsNot Nothing Then
                    txtServer.Text = Server.UrlDecode(Request.Cookies("SQL_Server").Value)
                    txtDB.Text = Request.Cookies("SQL_DB").Value
                    txtSqlUser.Text = Request.Cookies("SQL_User").Value
                    txtSqlPass.Attributes.Add("value", Server.UrlDecode(Request.Cookies("SQL_Pass").Value))
                End If
            End If

            If Session("GitHubAuth") IsNot Nothing AndAlso CBool(Session("GitHubAuth")) = True Then
                ShowSqlPanel()
            End If
        End If
    End Sub

    Private Function LoadConfigFromJson() As Boolean
        Try
            Dim path As String = Server.MapPath("~/App_Data/dbconfig.json")
            If File.Exists(path) Then
                Dim json As String = File.ReadAllText(path)
                Dim js As New JavaScriptSerializer()
                Dim cfg = js.Deserialize(Of SqlConfig)(json)
                If cfg IsNot Nothing Then
                    txtServer.Text = cfg.server
                    txtDB.Text = cfg.db
                    txtSqlUser.Text = cfg.user
                    txtSqlPass.Attributes.Add("value", cfg.pass)
                    Return True
                End If
            End If
        Catch
            ' Fail silently and use cookies/manual entry
        End Try
        Return False
    End Function

    Private Function GetWebFingerprint() As String
        Try
            Dim ip As String = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
            If String.IsNullOrEmpty(ip) Then ip = Request.UserHostAddress

            Dim agent As String = Request.UserAgent
            Dim rawId As String = ip & agent
            Using md5 As System.Security.Cryptography.MD5 = System.Security.Cryptography.MD5.Create()
                Dim hashBytes As Byte() = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(rawId))
                Return BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 12)
            End Using
        Catch
            Return "WEB_ID_UNKNOWN"
        End Try
    End Function

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Dim uCode As String = txtUserCode.Text.Trim()
        Dim uPass As String = txtPassword.Text.Trim()
        Try
            System.Net.ServicePointManager.SecurityProtocol = DirectCast(3072, System.Net.SecurityProtocolType)
            Dim gitFile = UserGuard.FetchRawGitFile()
            Dim data = UserGuard.DeserialiseGitData(gitFile.content)

            Dim user = data.users.FirstOrDefault(Function(x) x.User_Code.Equals(uCode, StringComparison.OrdinalIgnoreCase) AndAlso x.Password = uPass)

            If user IsNot Nothing Then
                Dim currentWebID As String = GetWebFingerprint()
                Dim storedID As String = Convert.ToString(user.Active).Trim()

                If Not String.IsNullOrEmpty(storedID) AndAlso storedID <> "0" AndAlso storedID <> currentWebID Then
                    Dim otp = New Random().Next(1000, 9999).ToString()
                    Session("Temp_User") = uCode
                    Session("Generated_OTP") = otp

                    lblOTP.Text = otp
                    pnlLogin.Visible = False : pnlOTP.Visible = True

                    ShowMsg($"Active elsewhere (Web ID: {storedID}). Use OTP to reset.", "text-warning")
                Else
                    Session("UserName") = uCode
                    Session("WebID") = currentWebID

                    UserGuard.UpdateStatus(uCode, currentWebID)
                    Session("GitHubAuth") = True

                    Dim s = txtServer.Text.Trim(), d = txtDB.Text.Trim(), u = txtSqlUser.Text.Trim(), p = txtSqlPass.Text.Trim()

                    'If Not String.IsNullOrEmpty(s) AndAlso Tools.CheckConnection(s, d, u, p) Then

                    '    SaveCookie("SQL_Server", s)
                    '    SaveCookie("SQL_DB", d)
                    '    SaveCookie("SQL_User", u)
                    '    SaveCookie("SQL_Pass", p)

                    '    pnlLogin.Visible = False : pnlOTP.Visible = False : pnlSqlConfig.Visible = False
                    '    pnlSelection.Visible = True
                    '    LoadCompanies(Tools.GetConnectionString(s, d, u, p))
                    'Else
                    '    ShowSqlPanel()
                    'End If
                    ShowSqlPanel()
                End If
            Else
                ShowMsg("Invalid Identity Code or Security Key.", "text-danger")
            End If
        Catch ex As Exception
            ShowMsg("Authentication Error: " & ex.Message, "text-danger")
        End Try
    End Sub

    Protected Sub btnVerifyOTP_Click(sender As Object, e As EventArgs) Handles btnVerifyOTP.Click
        Try
            If Session("Generated_OTP") Is Nothing OrElse Session("Temp_User") Is Nothing Then
                ShowMsg("Session expired. Please login again.", "text-danger")
                pnlOTP.Visible = False
                pnlLogin.Visible = True
                Return
            End If

            Dim savedOTP As String = Session("Generated_OTP").ToString()
            Dim uCode As String = Session("Temp_User").ToString()
            Dim enteredOTP As String = txtOTP.Text.Trim()

            If enteredOTP = savedOTP Then
                UserGuard.UpdateStatus(uCode, "")
                Session.Remove("Generated_OTP")
                Session.Remove("Temp_User")
                pnlOTP.Visible = False
                pnlLogin.Visible = True
                txtOTP.Text = ""
                ShowMsg("System reset successful. You can now login.", "text-success")
            Else
                ShowMsg("Invalid OTP Code. Please try again.", "text-danger")
            End If
        Catch ex As Exception
            ShowMsg("Error: " & ex.Message, "text-danger")
        End Try
    End Sub

    Private Sub ShowSqlPanel()
        pnlLogin.Visible = False : pnlOTP.Visible = False : pnlSelection.Visible = False
        pnlSqlConfig.Visible = True

        If String.IsNullOrEmpty(txtServer.Text) AndAlso Request.Cookies("SQL_Server") IsNot Nothing Then
            txtServer.Text = Server.UrlDecode(Request.Cookies("SQL_Server").Value)
            txtDB.Text = Request.Cookies("SQL_DB").Value
            txtSqlUser.Text = Request.Cookies("SQL_User").Value
            txtSqlPass.Attributes.Add("value", Server.UrlDecode(Request.Cookies("SQL_Pass").Value))
        End If
    End Sub

    Protected Sub btnSaveSql_Click(sender As Object, e As EventArgs) Handles btnSaveSql.Click
        Dim s = txtServer.Text.Trim(), d = txtDB.Text.Trim(), u = txtSqlUser.Text.Trim(), p = txtSqlPass.Text.Trim()

        If Tools.CheckConnection(s, d, u, p) Then
            SaveCookie("SQL_Server", s)
            SaveCookie("SQL_DB", d)
            SaveCookie("SQL_User", u)
            SaveCookie("SQL_Pass", p)

            pnlSqlConfig.Visible = False : pnlSelection.Visible = True
            LoadCompanies(Tools.GetConnectionString(s, d, u, p))
        Else
            ShowMsg("SQL Connection Failed.", "text-danger")
        End If
    End Sub

    Private Sub LoadCompanies(Optional specificConn As String = "")
        Dim connStr As String = If(specificConn <> "", specificConn, GetDBConn())
        Try
            Using conn As New SqlConnection(connStr)
                Dim da As New SqlDataAdapter("SELECT Comp_name, Comp_sno FROM Company_table where Grp_Company=0 ORDER BY Comp_name", conn)
                Dim dt As New DataTable()
                da.Fill(dt)
                ddlCompany.DataSource = dt : ddlCompany.DataTextField = "Comp_name" : ddlCompany.DataValueField = "Comp_sno" : ddlCompany.DataBind()
                ddlCompany.Items.Insert(0, New ListItem("-- Select Company --", ""))
            End Using
        Catch ex As Exception
            ShowMsg("Database Error: " & ex.Message, "text-danger")
        End Try
    End Sub

    Protected Sub ddlCompany_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCompany.SelectedIndexChanged
        If ddlCompany.SelectedIndex > 0 Then
            Try
                Using conn As New SqlConnection(GetDBConn())
                    Dim cmd As New SqlCommand("SELECT Period_from, Period_to FROM Company_table WHERE Comp_sno=@cp", conn)
                    cmd.Parameters.AddWithValue("@cp", ddlCompany.SelectedValue)
                    conn.Open()
                    Dim rdr = cmd.ExecuteReader()
                    ddlYear.Items.Clear()
                    While rdr.Read()
                        Dim val = CDate(rdr(0)).ToString("yyyy-MM-dd") & "|" & CDate(rdr(1)).ToString("yyyy-MM-dd")
                        ddlYear.Items.Add(New ListItem(CDate(rdr(0)).ToString("dd/MM/yyyy") & " to " & CDate(rdr(1)).ToString("dd/MM/yyyy"), val))
                    End While
                End Using
            Catch ex As Exception
                ShowMsg("Error: " & ex.Message, "text-danger")
            End Try
        End If
    End Sub

    Protected Sub btnEnterMain_Click(sender As Object, e As EventArgs) Handles btnEnterMain.Click
        If ddlCompany.SelectedIndex > 0 And ddlYear.SelectedValue <> "" Then
            Dim dates = ddlYear.SelectedValue.Split("|"c)
            SaveCookie("Selected_CompNo", ddlCompany.SelectedValue)
            SaveCookie("Selected_CompName", ddlCompany.SelectedItem.Text)
            SaveCookie("Selected_PeriodFrom", dates(0))
            SaveCookie("Selected_PeriodTo", dates(1))
            Response.Redirect("MainForm.aspx")
        End If
    End Sub

    Private Sub SaveCookie(n As String, v As String)
        Dim c As New HttpCookie(n, Server.UrlEncode(v))
        c.Expires = DateTime.Now.AddDays(30)
        c.Path = "/"
        Response.Cookies.Set(c)
    End Sub

    Private Function GetDBConn() As String
        Dim s = If(Request.Cookies("SQL_Server") IsNot Nothing, Server.UrlDecode(Request.Cookies("SQL_Server").Value), txtServer.Text)
        Dim d = If(Request.Cookies("SQL_DB") IsNot Nothing, Request.Cookies("SQL_DB").Value, txtDB.Text)
        Dim u = If(Request.Cookies("SQL_User") IsNot Nothing, Request.Cookies("SQL_User").Value, txtSqlUser.Text)
        Dim p = If(Request.Cookies("SQL_Pass") IsNot Nothing, Server.UrlDecode(Request.Cookies("SQL_Pass").Value), txtSqlPass.Text)

        Dim baseConn = Tools.GetConnectionString(s, d, u, p)
        If Not baseConn.ToLower().Contains("trustservercertificate") Then
            baseConn &= ";TrustServerCertificate=True;"
        End If

        Return baseConn
    End Function

    Private Sub ShowMsg(m As String, c As String)
        lblMsg.Text = m : lblMsg.CssClass = "small d-block fw-bold " & c
    End Sub
End Class
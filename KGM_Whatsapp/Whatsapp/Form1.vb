Imports System.Data.SqlClient
Imports Microsoft.Web.WebView2.Core
Imports System.IO
Imports Newtonsoft.Json

Public Class Form
    Private Const MillisecondsDelay As Integer = 8000
    Dim isProcessing As Boolean = False
    Dim canCloseApp As Boolean = False
    Dim trayMenu As New ContextMenuStrip()
    Dim isBrowserInitialized As Boolean = False

    Private Async Sub Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ApplyDarkTheme()
        SetupGridDesign()
        trayMenu.Items.Add("Open Dashboard", Nothing, AddressOf RestoreFromTray)
        trayMenu.Items.Add("Exit App", Nothing, AddressOf SystemExit)
        niTray.ContextMenuStrip = trayMenu
        niTray.Text = "WhatsApp Automation"
        If niTray.Icon Is Nothing Then niTray.Icon = SystemIcons.Application

        Await InitializeApplication()

        lblversion.Text = "System Version : 1.01"
        Timer1.Interval = 5000

        Guna2ToggleSwitch1.Checked = True
        MinimizeToTray()
    End Sub

    Private Async Function InitializeApplication() As Task
        Tools.LoadConfiguration(True)
        Try
            If Not isBrowserInitialized AndAlso Tools.IsWebviewEnabled() Then
                Dim webPath As String = Path.Combine(Application.StartupPath, "WBSession")
                If Not Directory.Exists(webPath) Then Directory.CreateDirectory(webPath)

                Dim options As New CoreWebView2EnvironmentOptions("--disable-background-timer-throttling --disable-backgrounding-occluded-windows")

                Dim env = Await CoreWebView2Environment.CreateAsync(Nothing, webPath, options)
                Await WebView2.EnsureCoreWebView2Async(env)

                WebView2.ZoomFactor = 0.5
                isBrowserInitialized = True
            End If

            If Tools.IsWebviewEnabled() Then
                WebView2.Visible = True
                WebView2.Source = New Uri("https://web.whatsapp.com")

                Guna2DataGridView1.Width = 752
                'Msgtxt.Width = 752
                Guna2Panel1.Visible = True
                Guna2HtmlLabel2.Visible = True
                Guna2HtmlLabel3.Visible = True
                Guna2ShadowPanel10.Visible = True

                RefreshBtn.Anchor = AnchorStyles.None
                RefreshBtn.Location = New Point(590, 80)
                Setupbtn.Anchor = AnchorStyles.None
                Setupbtn.Location = New Point(590, 130)
            Else
                WebView2.Visible = True
                WebView2.Location = New Point(-10000, -10000)
                WebView2.Source = New Uri("https://web.whatsapp.com")

                Guna2Panel1.Visible = False
                Guna2HtmlLabel2.Visible = False
                Guna2HtmlLabel3.Visible = False
                Guna2ShadowPanel10.Visible = False
                Guna2DataGridView1.Width = 1345
                Msgtxt.Width = 510
                Guna2Panel11.Width = 1345

                RefreshBtn.Anchor = AnchorStyles.None
                RefreshBtn.Location = New Point(1180, 80)
                Setupbtn.Anchor = AnchorStyles.None
                Setupbtn.Location = New Point(1180, 130)
            End If

        Catch ex As Exception
            MessageBox.Show("Initialization Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        LoadDataToGrid()
        UpdateStats()
    End Function

    Private Sub SystemExit(sender As Object, e As EventArgs)
        canCloseApp = True
        Application.Exit()
    End Sub

    Private Sub MinimizeToTray()
        Me.WindowState = FormWindowState.Minimized
        Me.Location = New Point(-10000, -10000)
        'Me.Hide()
        Me.ShowInTaskbar = False
        niTray.Visible = True
    End Sub

    Private Sub RestoreFromTray()
        'Me.Show()
        Me.WindowState = FormWindowState.Normal
        Me.CenterToScreen()
        Me.ShowInTaskbar = True
        Me.BringToFront()
    End Sub

    Private Sub niTray_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles niTray.MouseDoubleClick
        RestoreFromTray()
    End Sub

    Private Sub Guna2ControlBox1_Click(sender As Object, e As EventArgs) Handles Guna2ControlBox1.Click
        MinimizeToTray()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If Not canCloseApp Then
            e.Cancel = True
            MinimizeToTray()
        Else
            niTray.Visible = False
            niTray.Dispose()
        End If
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Not isProcessing AndAlso WebView2.CoreWebView2 IsNot Nothing Then
            LoadDataToGrid()
            ProcessNextMessage()
        End If
    End Sub

    Private Sub ProcessNextMessage()
        If isProcessing Then Exit Sub
        Dim dt As New DataTable()

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()

                Dim findCmd As New SqlCommand("SELECT TOP 1 SmsSno FROM What_Table WHERE Status = 1 ORDER BY SmsSno ASC", conn)
                Dim findId = findCmd.ExecuteScalar()

                If findId IsNot Nothing Then
                    isProcessing = True
                    Timer1.Stop()

                    Dim smsId As Integer = CInt(findId)

                    Dim lockCmd As New SqlCommand("UPDATE What_Table SET ErrorString = 'Sending...' WHERE SmsSno = @id", conn)
                    lockCmd.Parameters.AddWithValue("@id", smsId)
                    lockCmd.ExecuteNonQuery()

                    Dim adapter As New SqlDataAdapter("SELECT SmsSno, Mobile, Message, Attachment, FileName FROM What_Table WHERE SmsSno = @id", conn)
                    adapter.SelectCommand.Parameters.AddWithValue("@id", smsId)
                    adapter.Fill(dt)
                End If
            End Using

            If dt.Rows.Count > 0 Then
                Dim row As DataRow = dt.Rows(0)
                Dim attachData As Byte() = Nothing
                Dim fName As String = ""

                If row("Attachment") IsNot DBNull.Value Then
                    attachData = DirectCast(row("Attachment"), Byte())
                    fName = row("FileName").ToString()
                End If

                SendViaBrowser(CInt(row("SmsSno")), row("Mobile").ToString().Trim(), row("Message").ToString(), attachData, fName)
            Else
                LoadDataToGrid()
                UpdateStats()
                If Guna2ToggleSwitch1.Checked Then Timer1.Start()
            End If
        Catch ex As Exception
            isProcessing = False
            If Guna2ToggleSwitch1.Checked Then Timer1.Start()
            Debug.WriteLine("ProcessNextMessage Error: " & ex.Message)
        End Try
    End Sub
    Private Async Function WakeUpBrowser() As Task

        Me.Invoke(Sub()
                      Me.Show()
                      Me.WindowState = FormWindowState.Normal
                      Me.ShowInTaskbar = True
                      Me.TopMost = True
                      Me.BringToFront()
                  End Sub)
        Await Task.Delay(500)

        Me.Invoke(Sub()
                      Dim pt As Point = WebView2.PointToScreen(New Point(WebView2.Width \ 2, WebView2.Height \ 2))
                      Cursor.Position = pt
                      Dim MOUSEEVENTF_LEFTDOWN As Integer = &H2
                      Dim MOUSEEVENTF_LEFTUP As Integer = &H4
                      mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0)
                      mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0)
                  End Sub)
        Await Task.Delay(500)

        Me.Invoke(Sub() WebView2.Focus())
        Await Task.Delay(300)

        Me.Invoke(Sub() Me.TopMost = False)
    End Function

    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Sub mouse_event(dwFlags As Integer, dx As Integer, dy As Integer, dwData As Integer, dwExtraInfo As Integer)
    End Sub

    <System.Runtime.InteropServices.DllImport("user32.dll")>
    Private Shared Function SetForegroundWindow(ByVal hWnd As IntPtr) As Byte
    End Function

    Private Async Sub SendViaBrowser(id As Integer, phone As String, msg As String, fileBytes As Byte(), fileName As String)
        Dim wasInTray As Boolean = Not Me.Visible

        Try
            WebView2.CoreWebView2.Navigate("https://web.whatsapp.com/send?phone=" & phone)
            Await Task.Delay(15000)

            Dim checkStatusScript As String = "
            (function() {
                var invalid = document.querySelector('div[data-animate-modal-popup=""true""]');
                if (invalid && (invalid.innerText.toLowerCase().includes('invalid') || invalid.innerText.toLowerCase().includes('ok'))) return 'INVALID';
                var chatBox = document.querySelector('div[contenteditable=""true""]');
                return chatBox ? 'READY' : 'LOADING';
            })();"

            Dim statusResult = Await WebView2.ExecuteScriptAsync(checkStatusScript)
            If statusResult.Contains("INVALID") Then
                UpdateSqlStatus(id, 2, "Invalid Phone Number")
                Exit Try
            End If

            Dim jsonMsg As String = JsonConvert.SerializeObject(msg)
            Dim injectScript As String = "
            (function() {
                var boxes = document.querySelectorAll('div[contenteditable=""true""]');
                if (boxes.length > 0) {
                    var targetBox = boxes[boxes.length - 1]; 
                    targetBox.focus();
                    document.execCommand('insertText', false, " & jsonMsg & ");
                    targetBox.dispatchEvent(new Event('input', { bubbles: true }));
                    return 'SUCCESS';
                }
                return 'BOX_NOT_FOUND';
            })();"

            Await WebView2.ExecuteScriptAsync(injectScript)
            Await Task.Delay(2000)

            Dim hasAttachment As Boolean = (fileBytes IsNot Nothing AndAlso fileBytes.Length > 0)

            If hasAttachment Then
                Dim tempPath As String = Path.Combine(Path.GetTempPath(), fileName)
                File.WriteAllBytes(tempPath, fileBytes)
                Dim fileList As New System.Collections.Specialized.StringCollection()
                fileList.Add(tempPath)
                Me.Invoke(Sub() Clipboard.SetFileDropList(fileList))

                Me.Invoke(Sub()
                              Me.Show()
                              Me.WindowState = FormWindowState.Normal
                              Me.TopMost = True
                              Me.Activate()
                              SetForegroundWindow(Me.Handle)
                              WebView2.Focus()
                          End Sub)

                Await Task.Delay(1500)
                SendKeys.SendWait("^v")

                Await Task.Delay(8000)

                If wasInTray Then
                    Me.Invoke(Sub() MinimizeToTray())
                End If
            End If

            Dim clickSendScript As String = "
            (function() {
                // Find the Send Icon (Works for both text and media)
                var sendBtn = document.querySelector('span[data-icon=""send""]') || 
                              document.querySelector('div[aria-label=""Send""]') ||
                              document.querySelector('button[data-testid=""compose-btn-send""]');
                
                if (sendBtn) {
                    // Click the actual button containing the icon
                    var btn = sendBtn.closest('div[role=""button""]') || sendBtn.closest('button') || sendBtn;
                    btn.click();
                    return 'SENT';
                }
                
                // Fallback for some Media Previews
                var allButtons = document.querySelectorAll('div[role=""button""]');
                for (var b of allButtons) {
                   if (b.querySelector('span[data-icon=""send""]')) {
                       b.click();
                       return 'SENT';
                   }
                }
                return 'NOT_FOUND';
            })();"

            Dim sendResult = Await WebView2.ExecuteScriptAsync(clickSendScript)

            If sendResult.Contains("SENT") Then
                Await Task.Delay(3000)
                UpdateSqlStatus(id, 3, "Delivered Successfully")
            Else
                UpdateSqlStatus(id, 2, "Failed to trigger send button")
            End If

        Catch ex As Exception
            UpdateSqlStatus(id, 2, "System Error: " & ex.Message)
            If wasInTray Then Me.Invoke(Sub() MinimizeToTray())
        Finally
            isProcessing = False
            LoadDataToGrid()
            UpdateStats()
            If Guna2ToggleSwitch1.Checked Then Timer1.Start()
        End Try
    End Sub
    Sub UpdateSqlStatus(id As Integer, statusCode As Integer, errorInfo As String)
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim cmd As New SqlCommand(
                "UPDATE What_Table SET Status = @s, SentTime = GETDATE(), ErrorString = @e WHERE SmsSno = @id", conn)

                cmd.Parameters.AddWithValue("@s", statusCode)
                cmd.Parameters.AddWithValue("@e", errorInfo)
                cmd.Parameters.AddWithValue("@id", id)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            Debug.WriteLine("UpdateSqlStatus Error: " & ex.Message)
        End Try
    End Sub

    Sub LoadDataToGrid()
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                Dim query As String =
                    "SELECT TOP 50 SmsSno, Mobile, PartyName, Message AS [Message], " &
                    "CASE WHEN Status = 1 THEN 'Pending' " &
                         "WHEN Status = 2 THEN 'Failed' " &
                         "WHEN Status = 3 THEN 'Sent' " &
                         "ELSE 'Unknown' END AS [Status], " &
                    "SentTime, ErrorString FROM What_Table ORDER BY SmsSno DESC"

                Dim adapter As New SqlDataAdapter(query, conn)
                Dim dt As New DataTable()
                adapter.Fill(dt)
                Guna2DataGridView1.DataSource = dt

                If Guna2DataGridView1.Columns.Contains("SmsSno") Then Guna2DataGridView1.Columns("SmsSno").Visible = False
                If Guna2DataGridView1.Columns.Contains("ErrorString") Then Guna2DataGridView1.Columns("ErrorString").Visible = False
            End Using
        Catch ex As Exception
            Debug.WriteLine("LoadDataToGrid Error: " & ex.Message)
        End Try
    End Sub

    Sub UpdateStats()
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim query As String =
                    "SELECT ISNULL(SUM(CASE WHEN Status = 3 THEN 1 ELSE 0 END), 0), " &
                           "ISNULL(SUM(CASE WHEN Status = 1 THEN 1 ELSE 0 END), 0), " &
                           "ISNULL(SUM(CASE WHEN Status = 2 THEN 1 ELSE 0 END), 0) FROM What_Table"
                Dim reader = New SqlCommand(query, conn).ExecuteReader()
                If reader.Read() Then
                    'lblTotalSent.Text = "Sent: " & reader(0).ToString()
                    'lblTotalPending.Text = "Pending: " & reader(1).ToString()
                    'lblTotalFailed.Text = "Failed: " & reader(2).ToString()

                    lblTotalSent.Text = reader(0).ToString()
                    lblTotalPending.Text = reader(1).ToString()
                    lblTotalFailed.Text = reader(2).ToString()
                End If
            End Using
        Catch ex As Exception
            Debug.WriteLine("UpdateStats Error: " & ex.Message)
        End Try
    End Sub

    Private Sub ApplyDarkTheme()
        Me.BackColor = Color.FromArgb(15, 23, 42)

        'lblTotalSent.ForeColor = Color.FromArgb(16, 185, 129)

        'lblTotalPending.ForeColor = Color.FromArgb(245, 158, 11)

        'lblTotalFailed.ForeColor = Color.FromArgb(239, 68, 68)

        lblStatus.ForeColor = Color.FromArgb(148, 163, 184)
        lblversion.ForeColor = Color.FromArgb(71, 85, 105)

        Guna2ToggleSwitch1.CheckedState.FillColor = Color.FromArgb(16, 185, 129)
        'Guna2ToggleSwitch1.UncheckedState.FillColor = Color.FromArgb(71, 85, 105)
        Guna2ToggleSwitch1.UncheckedState.FillColor = Color.FromArgb(239, 68, 68)

        SetupGridDesign()
    End Sub

    Sub SetupGridDesign()
        Guna2DataGridView1.BackgroundColor = Color.FromArgb(15, 23, 42)
        Guna2DataGridView1.GridColor = Color.FromArgb(30, 41, 59)
        Guna2DataGridView1.BorderStyle = BorderStyle.None
        Guna2DataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal

        Guna2DataGridView1.ColumnHeadersHeight = 45
        Guna2DataGridView1.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(30, 41, 59)
        Guna2DataGridView1.ThemeStyle.HeaderStyle.ForeColor = Color.FromArgb(148, 163, 184)
        Guna2DataGridView1.ThemeStyle.HeaderStyle.Font = New Font("Segoe UI Semibold", 10)
        Guna2DataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None

        Guna2DataGridView1.RowTemplate.Height = 40
        Guna2DataGridView1.ThemeStyle.RowsStyle.BackColor = Color.FromArgb(15, 23, 42)
        Guna2DataGridView1.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(203, 213, 225)
        Guna2DataGridView1.ThemeStyle.RowsStyle.Font = New Font("Segoe UI", 9)

        Guna2DataGridView1.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(51, 65, 85)
        Guna2DataGridView1.ThemeStyle.RowsStyle.SelectionForeColor = Color.White

        Guna2DataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(20, 30, 50)

        Guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        Guna2DataGridView1.AllowUserToResizeRows = False
        Guna2DataGridView1.EnableHeadersVisualStyles = False
        Guna2DataGridView1.ReadOnly = True


        Msgtxt.ReadOnly = True
        Msgtxt.BackColor = Color.FromArgb(30, 41, 59)
        Msgtxt.ForeColor = Color.FromArgb(248, 250, 252)
        Msgtxt.BorderThickness = 1
        Msgtxt.BorderColor = Color.FromArgb(51, 65, 85)
        Msgtxt.FocusedState.BorderColor = Color.FromArgb(16, 185, 129)
    End Sub
    Private Sub Guna2DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles Guna2DataGridView1.CellFormatting
        If Guna2DataGridView1.Columns(e.ColumnIndex).Name = "Status" AndAlso e.Value IsNot Nothing Then

            Dim statusValue As String = e.Value.ToString()

            Select Case statusValue
                Case "Sent"
                    e.CellStyle.ForeColor = Color.FromArgb(16, 185, 129)
                    e.CellStyle.Font = New Font(Guna2DataGridView1.Font, FontStyle.Bold)
                Case "Pending"
                    e.CellStyle.ForeColor = Color.FromArgb(245, 158, 11)
                Case "Failed"
                    e.CellStyle.ForeColor = Color.FromArgb(239, 68, 68)
                    e.CellStyle.Font = New Font(Guna2DataGridView1.Font, FontStyle.Bold)
            End Select
        End If
    End Sub

    Private Sub Guna2DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles Guna2DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            Try
                Dim row As DataGridViewRow = Guna2DataGridView1.Rows(e.RowIndex)

                Dim msgVal = row.Cells("Message").Value
                Dim msg As String = If(msgVal IsNot DBNull.Value AndAlso msgVal IsNot Nothing, msgVal.ToString(), "No message content.")

                Dim errVal = row.Cells("ErrorString").Value
                Dim err As String = If(errVal IsNot DBNull.Value AndAlso errVal IsNot Nothing, errVal.ToString(), "None")

                Msgtxt.Text = "--- MESSAGE CONTENT ---" & vbCrLf &
                              msg & vbCrLf & vbCrLf &
                              "--- ERROR / STATUS DETAILS ---" & vbCrLf &
                              err
            Catch ex As Exception
                Msgtxt.Text = "Error loading details: " & ex.Message
            End Try
        End If
    End Sub

    Private Sub RefreshBtn_Click(sender As Object, e As EventArgs) Handles RefreshBtn.Click
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim cmd As New SqlCommand("UPDATE What_Table SET Status = 1, ErrorString = '' WHERE Status = 2", conn)
                cmd.ExecuteNonQuery()
                LoadDataToGrid()
                UpdateStats()
            End Using
        Catch ex As Exception
            Debug.WriteLine("RefreshBtn Error: " & ex.Message)
        End Try
    End Sub

    Private Sub Guna2ToggleSwitch1_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2ToggleSwitch1.CheckedChanged
        If Guna2ToggleSwitch1.Checked Then
            Timer1.Start()
            lblStatus.Text = "Online"
            StatusLight.FillColor = Color.Green
            StatusLight.ShadowDecoration.Color = Color.Green
        Else
            Timer1.Stop()
            lblStatus.Text = "Offline"
            StatusLight.FillColor = Color.Red
            StatusLight.ShadowDecoration.Color = Color.Red
        End If
    End Sub

    Private Async Sub Setupbtn_Click(sender As Object, e As EventArgs) Handles Setupbtn.Click
        Timer1.Stop()
        Guna2ToggleSwitch1.Checked = False
        Using setupForm As New connect()
            If setupForm.ShowDialog() = DialogResult.OK Then Await InitializeApplication()
        End Using
    End Sub

End Class
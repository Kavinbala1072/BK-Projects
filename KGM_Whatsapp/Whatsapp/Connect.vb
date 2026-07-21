Imports System.IO

Public Class connect
    Private filePath As String = Path.Combine(Application.StartupPath, "DBConnect.txt")

    Private Sub connect_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadConfigurationToUI()
    End Sub

    Private Sub LoadConfigurationToUI()
        Try
            If File.Exists(filePath) Then
                Dim lines = File.ReadAllLines(filePath)
                For Each line In lines
                    Dim parts = line.Split("="c)
                    If parts.Length < 2 Then Continue For
                    Dim key As String = parts(0).Trim()
                    Dim value As String = parts(1).Trim()

                    Select Case key
                        Case "SQLServer" : servertxt.Text = value
                        Case "SQLDBName" : dbtxt.Text = value
                        Case "Webview" : Guna2ToggleSwitch1.Checked = If(value.ToLower() = "true", True, False)
                    End Select
                Next
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading config: " & ex.Message)
        End Try
    End Sub

    Private Sub NextButton_Click(sender As Object, e As EventArgs) Handles NextButton.Click
        Try
            Dim configLines As New List(Of String)
            configLines.Add("SQLServerType=True")
            configLines.Add("SQLServer=" & servertxt.Text.Trim())
            configLines.Add("SQLDBName=" & dbtxt.Text.Trim())
            configLines.Add("Webview=" & Guna2ToggleSwitch1.Checked.ToString())

            File.WriteAllLines(filePath, configLines)
            MessageBox.Show("Configuration saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub
End Class
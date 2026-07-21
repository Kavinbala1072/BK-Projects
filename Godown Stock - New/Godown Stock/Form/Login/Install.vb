Imports System.Net
Imports System.IO
Imports System.IO.Compression
Imports Guna.UI2.WinForms

Public Class Install
    Private Const AppUrl As String = "https://github.com/Kavinbala1072/GodownStock/archive/refs/heads/main.zip"
    Private Const AppName As String = "latest GS"

    Private WithEvents client As New WebClient()

    Private tempZipPath As String = Path.Combine(Path.GetTempPath(), "AppInstaller.zip")
    Private tempExtractPath As String = Path.Combine(Path.GetTempPath(), "AppInstallerTemp")
    Private finalPath As String = Path.Combine(Application.StartupPath, AppName)

    Private Sub Install_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DownloadProgressBar.Minimum = 0
        DownloadProgressBar.Maximum = 100
        DownloadProgressBar.Value = 0

        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me
    End Sub

    Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12

        Try
            If Directory.Exists(tempExtractPath) Then Directory.Delete(tempExtractPath, True)
            If Directory.Exists(finalPath) Then Directory.Delete(finalPath, True)
            If File.Exists(tempZipPath) Then File.Delete(tempZipPath)

            DownloadProgressBar.Value = 0
            client.DownloadFileAsync(New Uri(AppUrl), tempZipPath)
        Catch ex As Exception
            MessageBox.Show("Download failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub client_DownloadProgressChanged(sender As Object, e As DownloadProgressChangedEventArgs) Handles client.DownloadProgressChanged
        If DownloadProgressBar.InvokeRequired Then
            DownloadProgressBar.Invoke(Sub() DownloadProgressBar.Value = e.ProgressPercentage)
        Else
            DownloadProgressBar.Value = e.ProgressPercentage
        End If
    End Sub

    Private Sub client_DownloadFileCompleted(sender As Object, e As System.ComponentModel.AsyncCompletedEventArgs) Handles client.DownloadFileCompleted
        If e.Cancelled Then
            MessageBox.Show("Download was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If e.Error IsNot Nothing Then
            MessageBox.Show("Download failed: " & e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Try
            ZipFile.ExtractToDirectory(tempZipPath, tempExtractPath)
            Dim extractedFolder = Directory.GetDirectories(tempExtractPath).FirstOrDefault()

            If extractedFolder IsNot Nothing Then
                CopyDirectory(extractedFolder, finalPath)

                Dim configPath As String = Path.Combine(Application.StartupPath, "DBConnect.txt")
                If File.Exists(configPath) Then
                    Dim lines As List(Of String) = File.ReadAllLines(configPath).ToList()
                    Dim isFound As Boolean = False
                    For i As Integer = 0 To lines.Count - 1
                        If lines(i).Trim().StartsWith("CurrentVersion=") Then
                            lines(i) = "CurrentVersion=" & DBConnect.Version
                            isFound = True
                            Exit For
                        End If
                    Next
                    If Not isFound Then lines.Add("CurrentVersion=" & DBConnect.Version)
                    File.WriteAllLines(configPath, lines)
                End If

                MessageBox.Show("Installation completed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Application.Exit()
            Else
                MessageBox.Show("Extraction failed: No folder found in archive.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Installation failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            Try
                If File.Exists(tempZipPath) Then File.Delete(tempZipPath)
                If Directory.Exists(tempExtractPath) Then Directory.Delete(tempExtractPath, True)
            Catch cleanupEx As Exception
                MessageBox.Show("Cleanup failed: " & cleanupEx.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End Try
            DownloadProgressBar.Value = 0
        End Try
    End Sub

    Private Sub CopyDirectory(sourceDir As String, destinationDir As String)
        If Not Directory.Exists(destinationDir) Then
            Directory.CreateDirectory(destinationDir)
        End If

        For Each filePath In Directory.GetFiles(sourceDir)
            Dim fileName = Path.GetFileName(filePath)
            Dim destFile = Path.Combine(destinationDir, fileName)
            File.Copy(filePath, destFile, True)
        Next

        For Each subDir In Directory.GetDirectories(sourceDir)
            Dim dirName = Path.GetFileName(subDir)
            Dim destSubDir = Path.Combine(destinationDir, dirName)
            CopyDirectory(subDir, destSubDir)
        Next
    End Sub

    Private Sub SkipButton_Click(sender As Object, e As EventArgs) Handles SkipButton.Click
        Me.Close()
    End Sub
End Class

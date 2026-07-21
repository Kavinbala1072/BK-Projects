Imports Guna.UI2.WinForms
Imports System.Data.SqlClient
Public Class Repost
    Public Property Sett As Setting

    Private Sub Repost_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Tools.LoadConfiguration()
        Themeload()
        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me
    End Sub

    Private Function GenerateKey() As String
        Return DateTime.Now.ToString("yyyyMMdd")
    End Function

    Private Sub ValidBtn_Click_1(sender As Object, e As EventArgs) Handles ValidBtn.Click
        Dim enterKey As String = KeyTxt.Text.Trim()
        Dim expectedKey As String = GenerateKey()

        If enterKey.Equals(expectedKey, StringComparison.OrdinalIgnoreCase) Then
            lblresult.Text = "CORRECT"
            lblresult.ForeColor = Color.Green
            If Sett IsNot Nothing Then
                Sett.ItemPostButton.Visible = True
                Sett.JCPostButton.Visible = True
                Sett.ActiveRepostButton.Visible = True
                Sett.PtPostButton.Visible = True
                Sett.Label66.Visible = True
                Sett.Label65.Visible = True
                Sett.DateTimePickerFrom.Visible = True
                Sett.DateTimePickerTo.Visible = True
                Sett.OldDBName.Visible = True
                Sett.Remove.Visible = True
            End If
            Me.Close()
        Else
            lblresult.Text = "INVALID KEY"
            lblresult.ForeColor = Color.Red
        End If
    End Sub

    Private Sub Themeload()
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'HeaderColor'", sqlconnect)
                Using reader As SqlDataReader = Query.ExecuteReader()
                    If reader.Read() Then
                        Dim colorString As String = reader("Ctl_Value").ToString()
                        If Not String.IsNullOrEmpty(colorString) Then
                            Try
                                HeaderPanel.BackColor = ColorTranslator.FromHtml(colorString)
                            Catch
                                HeaderPanel.BackColor = Color.FromArgb(34, 40, 49)
                            End Try
                        Else
                            HeaderPanel.BackColor = Color.FromArgb(34, 40, 49)
                        End If
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading HeaderColor: " & ex.Message)
            End Try
        End Using

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'ScreenColor'", sqlconnect)
                Using reader As SqlDataReader = Query.ExecuteReader()
                    If reader.Read() Then
                        Dim colorString As String = reader("Ctl_Value").ToString()
                        Dim screenColor As Color = Color.FromArgb(232, 232, 232)

                        If Not String.IsNullOrEmpty(colorString) Then
                            Try
                                screenColor = ColorTranslator.FromHtml(colorString)
                            Catch
                                Me.BackColor = Color.FromArgb(232, 232, 232)
                            End Try
                        End If

                        Me.BackColor = screenColor
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading ScreenColor: " & ex.Message)
            End Try
        End Using

    End Sub
End Class

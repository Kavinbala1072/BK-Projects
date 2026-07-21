Imports System.Data.SqlClient
Imports Guna.UI2.WinForms
Imports System.Security.Cryptography
Imports System.Text

Public Class PSDForm

    Private Sub PSDForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Resettxt.ReadOnly = True
        Tools.LoadConfiguration()
        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me
    End Sub
    Private Function GenerateKey() As String
        Return DateTime.Now.ToString("yyyyMMdd")
    End Function
    Private Sub ValidBtn_Click(sender As Object, e As EventArgs) Handles ValidBtn.Click
        Dim enteredKey As String = KeyTxt.Text.Trim()
        Dim expectedKey As String = GenerateKey()

        If enteredKey = expectedKey Then
            lblresult.Text = "Done"
            lblresult.ForeColor = Color.Green
            Resettxt.ReadOnly = False
        Else
            lblresult.Text = "INVALID KEY"
            lblresult.ForeColor = Color.Red
        End If
    End Sub
    Private Function HashPassword(password As String) As String
        Using sha As SHA256 = SHA256.Create()
            Dim bytes = Encoding.UTF8.GetBytes(password)
            Dim hash = sha.ComputeHash(bytes)
            Return Convert.ToBase64String(hash)
        End Using
    End Function
    Private Sub ResetButton_Click(sender As Object, e As EventArgs) Handles ResetButton.Click
        Dim Username As String = "Admin"
        Dim newPassword As String = HashPassword(Resettxt.Text.Trim())

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Dim checkExistCommand As New SqlCommand(
                "SELECT COUNT(*) FROM User_Table WHERE User_Name = @User_Name", sqlconnect)
            checkExistCommand.Parameters.AddWithValue("@User_Name", Username)

            Try
                sqlconnect.Open()

                If Convert.ToInt32(checkExistCommand.ExecuteScalar()) > 0 Then
                    Dim updateCommand As New SqlCommand(
                        "UPDATE User_Table SET User_Password = @Password WHERE User_Name = @User_Name", sqlconnect)

                    updateCommand.Parameters.AddWithValue("@User_Name", Username)
                    updateCommand.Parameters.AddWithValue("@Password", newPassword)
                    updateCommand.ExecuteNonQuery()

                    MessageBox.Show("Password updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("User does not exist.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
        Me.Close()
    End Sub

End Class

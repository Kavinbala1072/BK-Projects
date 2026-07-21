Public Class Greeting

    Public Sub SetMessage(title As String, message As String, img As Image)
        lblTitle.Text = title
        lblMessage.Text = message

        If img IsNot Nothing Then
            Me.BackgroundImage = img
            Me.BackgroundImageLayout = ImageLayout.Stretch
        End If
    End Sub

    Private Sub Logoutbtn_Click(sender As Object, e As EventArgs) Handles Logoutbtn.Click
        Me.Close()
    End Sub
End Class


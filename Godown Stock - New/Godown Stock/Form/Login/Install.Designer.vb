<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Install
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.DownloadProgressBar = New System.Windows.Forms.ProgressBar()
        Me.Guna2HtmlLabel1 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2Panel2 = New Guna.UI2.WinForms.Guna2Panel()
        Me.UpdateButton = New Guna.UI2.WinForms.Guna2Button()
        Me.SkipButton = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2Elipse = New Guna.UI2.WinForms.Guna2Elipse(Me.components)
        Me.Guna2Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'DownloadProgressBar
        '
        Me.DownloadProgressBar.BackColor = System.Drawing.Color.White
        Me.DownloadProgressBar.Location = New System.Drawing.Point(17, 44)
        Me.DownloadProgressBar.Name = "DownloadProgressBar"
        Me.DownloadProgressBar.Size = New System.Drawing.Size(387, 15)
        Me.DownloadProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.DownloadProgressBar.TabIndex = 20
        '
        'Guna2HtmlLabel1
        '
        Me.Guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel1.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel1.ForeColor = System.Drawing.Color.White
        Me.Guna2HtmlLabel1.Location = New System.Drawing.Point(132, 6)
        Me.Guna2HtmlLabel1.Name = "Guna2HtmlLabel1"
        Me.Guna2HtmlLabel1.Size = New System.Drawing.Size(146, 19)
        Me.Guna2HtmlLabel1.TabIndex = 21
        Me.Guna2HtmlLabel1.Text = "Latest Version Available"
        '
        'Guna2Panel2
        '
        Me.Guna2Panel2.Controls.Add(Me.Guna2HtmlLabel1)
        Me.Guna2Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Guna2Panel2.FillColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.Guna2Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Guna2Panel2.Name = "Guna2Panel2"
        Me.Guna2Panel2.ShadowDecoration.Parent = Me.Guna2Panel2
        Me.Guna2Panel2.Size = New System.Drawing.Size(421, 31)
        Me.Guna2Panel2.TabIndex = 23
        '
        'UpdateButton
        '
        Me.UpdateButton.BorderRadius = 8
        Me.UpdateButton.CheckedState.Parent = Me.UpdateButton
        Me.UpdateButton.CustomImages.Parent = Me.UpdateButton
        Me.UpdateButton.FillColor = System.Drawing.Color.RoyalBlue
        Me.UpdateButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.UpdateButton.ForeColor = System.Drawing.Color.White
        Me.UpdateButton.HoverState.Parent = Me.UpdateButton
        Me.UpdateButton.Image = Global.Godown_Stock.My.Resources.Resources.icons8_download_64
        Me.UpdateButton.Location = New System.Drawing.Point(214, 67)
        Me.UpdateButton.Name = "UpdateButton"
        Me.UpdateButton.ShadowDecoration.Parent = Me.UpdateButton
        Me.UpdateButton.Size = New System.Drawing.Size(101, 31)
        Me.UpdateButton.TabIndex = 22
        Me.UpdateButton.Text = "Update"
        '
        'SkipButton
        '
        Me.SkipButton.BorderRadius = 8
        Me.SkipButton.CheckedState.Parent = Me.SkipButton
        Me.SkipButton.CustomImages.Parent = Me.SkipButton
        Me.SkipButton.FillColor = System.Drawing.Color.Red
        Me.SkipButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.SkipButton.ForeColor = System.Drawing.Color.White
        Me.SkipButton.HoverState.Parent = Me.SkipButton
        Me.SkipButton.Image = Global.Godown_Stock.My.Resources.Resources.icons8_skip_50
        Me.SkipButton.Location = New System.Drawing.Point(106, 67)
        Me.SkipButton.Name = "SkipButton"
        Me.SkipButton.ShadowDecoration.Parent = Me.SkipButton
        Me.SkipButton.Size = New System.Drawing.Size(101, 31)
        Me.SkipButton.TabIndex = 19
        Me.SkipButton.Text = "Skip Version"
        '
        'Install
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(421, 110)
        Me.Controls.Add(Me.Guna2Panel2)
        Me.Controls.Add(Me.UpdateButton)
        Me.Controls.Add(Me.DownloadProgressBar)
        Me.Controls.Add(Me.SkipButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Install"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Install"
        Me.Guna2Panel2.ResumeLayout(False)
        Me.Guna2Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents DownloadProgressBar As ProgressBar
    Friend WithEvents SkipButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2HtmlLabel1 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents UpdateButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2Panel2 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2Elipse As Guna.UI2.WinForms.Guna2Elipse
End Class

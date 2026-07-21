<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Greeting
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lblTitle = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.lblMessage = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Logoutbtn = New Guna.UI2.WinForms.Guna2Button()
        Me.PictureBox1 = New Guna.UI2.WinForms.Guna2PictureBox()
        Me.Guna2PictureBox1 = New Guna.UI2.WinForms.Guna2PictureBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.Guna2PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.White
        Me.lblTitle.Font = New System.Drawing.Font("Calibri", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(12, 353)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(113, 20)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Guna2HtmlLabel1"
        Me.lblTitle.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMessage
        '
        Me.lblMessage.BackColor = System.Drawing.Color.White
        Me.lblMessage.Font = New System.Drawing.Font("Calibri", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMessage.Location = New System.Drawing.Point(12, 373)
        Me.lblMessage.Name = "lblMessage"
        Me.lblMessage.Size = New System.Drawing.Size(145, 25)
        Me.lblMessage.TabIndex = 1
        Me.lblMessage.Text = "Guna2HtmlLabel2"
        Me.lblMessage.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Logoutbtn
        '
        Me.Logoutbtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Logoutbtn.CheckedState.Parent = Me.Logoutbtn
        Me.Logoutbtn.CustomImages.Parent = Me.Logoutbtn
        Me.Logoutbtn.FillColor = System.Drawing.Color.Firebrick
        Me.Logoutbtn.Font = New System.Drawing.Font("Segoe UI Semilight", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Logoutbtn.ForeColor = System.Drawing.Color.White
        Me.Logoutbtn.HoverState.Parent = Me.Logoutbtn
        Me.Logoutbtn.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.Logoutbtn.Location = New System.Drawing.Point(565, 368)
        Me.Logoutbtn.Margin = New System.Windows.Forms.Padding(0)
        Me.Logoutbtn.Name = "Logoutbtn"
        Me.Logoutbtn.Padding = New System.Windows.Forms.Padding(15)
        Me.Logoutbtn.PressedColor = System.Drawing.Color.FromArgb(CType(CType(241, Byte), Integer), CType(CType(246, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Logoutbtn.ShadowDecoration.Parent = Me.Logoutbtn
        Me.Logoutbtn.Size = New System.Drawing.Size(81, 30)
        Me.Logoutbtn.TabIndex = 11
        Me.Logoutbtn.Text = "Close"
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.Transparent
        Me.PictureBox1.Location = New System.Drawing.Point(150, 71)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.ShadowDecoration.Parent = Me.PictureBox1
        Me.PictureBox1.Size = New System.Drawing.Size(300, 200)
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'Guna2PictureBox1
        '
        Me.Guna2PictureBox1.BackColor = System.Drawing.Color.White
        Me.Guna2PictureBox1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Guna2PictureBox1.Location = New System.Drawing.Point(0, 343)
        Me.Guna2PictureBox1.Name = "Guna2PictureBox1"
        Me.Guna2PictureBox1.ShadowDecoration.Parent = Me.Guna2PictureBox1
        Me.Guna2PictureBox1.Size = New System.Drawing.Size(655, 67)
        Me.Guna2PictureBox1.TabIndex = 3
        Me.Guna2PictureBox1.TabStop = False
        '
        'Greeting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(655, 410)
        Me.Controls.Add(Me.Logoutbtn)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.lblMessage)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.Guna2PictureBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Greeting"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Greeting"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.Guna2PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblTitle As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents lblMessage As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2PictureBox1 As Guna.UI2.WinForms.Guna2PictureBox
    Friend WithEvents Logoutbtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents PictureBox1 As Guna.UI2.WinForms.Guna2PictureBox
End Class

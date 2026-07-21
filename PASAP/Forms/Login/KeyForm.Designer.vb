<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class KeyForm
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
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2Button1 = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2HtmlLabel1 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.ActivateBtn = New Guna.UI2.WinForms.Guna2Button()
        Me.LblKey = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.KeyTxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.LblError = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.LaterBtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2Panel2 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2HtmlLabel2 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2Elipse1 = New Guna.UI2.WinForms.Guna2Elipse(Me.components)
        Me.Guna2Panel1.SuspendLayout()
        Me.Guna2Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.Controls.Add(Me.Guna2Button1)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel1)
        Me.Guna2Panel1.Controls.Add(Me.ActivateBtn)
        Me.Guna2Panel1.Controls.Add(Me.LblKey)
        Me.Guna2Panel1.Controls.Add(Me.KeyTxt)
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Guna2Panel1.FillColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.Guna2Panel1.Location = New System.Drawing.Point(0, 30)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.ShadowDecoration.Parent = Me.Guna2Panel1
        Me.Guna2Panel1.Size = New System.Drawing.Size(533, 124)
        Me.Guna2Panel1.TabIndex = 10
        '
        'Guna2Button1
        '
        Me.Guna2Button1.BackColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.Guna2Button1.BorderRadius = 8
        Me.Guna2Button1.CheckedState.Parent = Me.Guna2Button1
        Me.Guna2Button1.CustomImages.Parent = Me.Guna2Button1
        Me.Guna2Button1.FillColor = System.Drawing.SystemColors.ActiveBorder
        Me.Guna2Button1.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.Guna2Button1.ForeColor = System.Drawing.Color.White
        Me.Guna2Button1.HoverState.FillColor = System.Drawing.Color.White
        Me.Guna2Button1.HoverState.ForeColor = System.Drawing.Color.Green
        Me.Guna2Button1.HoverState.Parent = Me.Guna2Button1
        Me.Guna2Button1.Image = Global.PASAP.My.Resources.Resources.Copy
        Me.Guna2Button1.Location = New System.Drawing.Point(345, 12)
        Me.Guna2Button1.Name = "Guna2Button1"
        Me.Guna2Button1.PressedColor = System.Drawing.Color.Green
        Me.Guna2Button1.ShadowDecoration.Parent = Me.Guna2Button1
        Me.Guna2Button1.Size = New System.Drawing.Size(28, 28)
        Me.Guna2Button1.TabIndex = 12
        '
        'Guna2HtmlLabel1
        '
        Me.Guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel1.Font = New System.Drawing.Font("Segoe UI Light", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel1.ForeColor = System.Drawing.Color.LightGray
        Me.Guna2HtmlLabel1.Location = New System.Drawing.Point(128, 51)
        Me.Guna2HtmlLabel1.Name = "Guna2HtmlLabel1"
        Me.Guna2HtmlLabel1.Size = New System.Drawing.Size(245, 15)
        Me.Guna2HtmlLabel1.TabIndex = 4
        Me.Guna2HtmlLabel1.Text = "To activate your license. Please insert the license key."
        '
        'ActivateBtn
        '
        Me.ActivateBtn.BackColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.ActivateBtn.BorderRadius = 3
        Me.ActivateBtn.CheckedState.Parent = Me.ActivateBtn
        Me.ActivateBtn.CustomImages.Parent = Me.ActivateBtn
        Me.ActivateBtn.FillColor = System.Drawing.Color.Green
        Me.ActivateBtn.Font = New System.Drawing.Font("Segoe UI", 8.0!)
        Me.ActivateBtn.ForeColor = System.Drawing.Color.White
        Me.ActivateBtn.HoverState.FillColor = System.Drawing.Color.White
        Me.ActivateBtn.HoverState.ForeColor = System.Drawing.Color.Green
        Me.ActivateBtn.HoverState.Parent = Me.ActivateBtn
        Me.ActivateBtn.Location = New System.Drawing.Point(444, 74)
        Me.ActivateBtn.Name = "ActivateBtn"
        Me.ActivateBtn.PressedColor = System.Drawing.Color.Green
        Me.ActivateBtn.ShadowDecoration.Parent = Me.ActivateBtn
        Me.ActivateBtn.Size = New System.Drawing.Size(78, 28)
        Me.ActivateBtn.TabIndex = 11
        Me.ActivateBtn.Text = "Activate Now"
        '
        'LblKey
        '
        Me.LblKey.BackColor = System.Drawing.Color.Transparent
        Me.LblKey.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblKey.ForeColor = System.Drawing.Color.White
        Me.LblKey.IsSelectionEnabled = False
        Me.LblKey.Location = New System.Drawing.Point(188, 12)
        Me.LblKey.Name = "LblKey"
        Me.LblKey.Size = New System.Drawing.Size(146, 23)
        Me.LblKey.TabIndex = 9
        Me.LblKey.Text = "00000 00000 00000"
        '
        'KeyTxt
        '
        Me.KeyTxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.KeyTxt.DefaultText = ""
        Me.KeyTxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.KeyTxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.KeyTxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.KeyTxt.DisabledState.Parent = Me.KeyTxt
        Me.KeyTxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.KeyTxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.KeyTxt.FocusedState.Parent = Me.KeyTxt
        Me.KeyTxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.KeyTxt.HoverState.Parent = Me.KeyTxt
        Me.KeyTxt.Location = New System.Drawing.Point(14, 74)
        Me.KeyTxt.Name = "KeyTxt"
        Me.KeyTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.KeyTxt.PlaceholderText = ""
        Me.KeyTxt.SelectedText = ""
        Me.KeyTxt.ShadowDecoration.Parent = Me.KeyTxt
        Me.KeyTxt.Size = New System.Drawing.Size(427, 29)
        Me.KeyTxt.TabIndex = 10
        Me.KeyTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'LblError
        '
        Me.LblError.BackColor = System.Drawing.Color.Transparent
        Me.LblError.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblError.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.LblError.Location = New System.Drawing.Point(30, 6)
        Me.LblError.Name = "LblError"
        Me.LblError.Size = New System.Drawing.Size(162, 19)
        Me.LblError.TabIndex = 3
        Me.LblError.Text = "License Key to Activate GS" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'LaterBtn
        '
        Me.LaterBtn.BorderRadius = 8
        Me.LaterBtn.CheckedState.Parent = Me.LaterBtn
        Me.LaterBtn.CustomImages.Parent = Me.LaterBtn
        Me.LaterBtn.FillColor = System.Drawing.Color.White
        Me.LaterBtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LaterBtn.ForeColor = System.Drawing.Color.Red
        Me.LaterBtn.HoverState.FillColor = System.Drawing.Color.Red
        Me.LaterBtn.HoverState.ForeColor = System.Drawing.Color.White
        Me.LaterBtn.HoverState.Parent = Me.LaterBtn
        Me.LaterBtn.Location = New System.Drawing.Point(459, 1)
        Me.LaterBtn.Name = "LaterBtn"
        Me.LaterBtn.ShadowDecoration.Parent = Me.LaterBtn
        Me.LaterBtn.Size = New System.Drawing.Size(72, 28)
        Me.LaterBtn.TabIndex = 12
        Me.LaterBtn.Text = "Later"
        '
        'Guna2Panel2
        '
        Me.Guna2Panel2.BackColor = System.Drawing.Color.White
        Me.Guna2Panel2.Controls.Add(Me.Guna2HtmlLabel2)
        Me.Guna2Panel2.Controls.Add(Me.LblError)
        Me.Guna2Panel2.Controls.Add(Me.LaterBtn)
        Me.Guna2Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Guna2Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Guna2Panel2.Name = "Guna2Panel2"
        Me.Guna2Panel2.ShadowDecoration.Parent = Me.Guna2Panel2
        Me.Guna2Panel2.Size = New System.Drawing.Size(533, 30)
        Me.Guna2Panel2.TabIndex = 11
        '
        'Guna2HtmlLabel2
        '
        Me.Guna2HtmlLabel2.AutoSize = False
        Me.Guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel2.BackgroundImage = Global.PASAP.My.Resources.Resources.key
        Me.Guna2HtmlLabel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Guna2HtmlLabel2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.Guna2HtmlLabel2.Location = New System.Drawing.Point(8, 6)
        Me.Guna2HtmlLabel2.Name = "Guna2HtmlLabel2"
        Me.Guna2HtmlLabel2.Size = New System.Drawing.Size(18, 18)
        Me.Guna2HtmlLabel2.TabIndex = 13
        Me.Guna2HtmlLabel2.Text = Nothing
        '
        'KeyForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(533, 154)
        Me.Controls.Add(Me.Guna2Panel1)
        Me.Controls.Add(Me.Guna2Panel2)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "KeyForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "KeyForm"
        Me.Guna2Panel1.ResumeLayout(False)
        Me.Guna2Panel1.PerformLayout()
        Me.Guna2Panel2.ResumeLayout(False)
        Me.Guna2Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2Button1 As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2HtmlLabel1 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents ActivateBtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents LblKey As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents KeyTxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents LblError As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents LaterBtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2Panel2 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2HtmlLabel2 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2Elipse1 As Guna.UI2.WinForms.Guna2Elipse
End Class

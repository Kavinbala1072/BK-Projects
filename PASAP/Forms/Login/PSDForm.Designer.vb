<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PSDForm
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
        Me.Guna2Panel2 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2ControlBox1 = New Guna.UI2.WinForms.Guna2ControlBox()
        Me.Guna2HtmlLabel1 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.ValidBtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ResetButton = New Guna.UI2.WinForms.Guna2Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Resettxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.KeyTxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.lblresult = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2Elipse = New Guna.UI2.WinForms.Guna2Elipse(Me.components)
        Me.Guna2Panel2.SuspendLayout()
        Me.Guna2Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Guna2Panel2
        '
        Me.Guna2Panel2.Controls.Add(Me.Guna2ControlBox1)
        Me.Guna2Panel2.Controls.Add(Me.Guna2HtmlLabel1)
        Me.Guna2Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Guna2Panel2.FillColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.Guna2Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Guna2Panel2.Name = "Guna2Panel2"
        Me.Guna2Panel2.ShadowDecoration.Parent = Me.Guna2Panel2
        Me.Guna2Panel2.Size = New System.Drawing.Size(371, 36)
        Me.Guna2Panel2.TabIndex = 17
        '
        'Guna2ControlBox1
        '
        Me.Guna2ControlBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Guna2ControlBox1.FillColor = System.Drawing.Color.Red
        Me.Guna2ControlBox1.HoverState.Parent = Me.Guna2ControlBox1
        Me.Guna2ControlBox1.IconColor = System.Drawing.Color.White
        Me.Guna2ControlBox1.Location = New System.Drawing.Point(336, 7)
        Me.Guna2ControlBox1.Name = "Guna2ControlBox1"
        Me.Guna2ControlBox1.ShadowDecoration.Parent = Me.Guna2ControlBox1
        Me.Guna2ControlBox1.Size = New System.Drawing.Size(23, 24)
        Me.Guna2ControlBox1.TabIndex = 2
        '
        'Guna2HtmlLabel1
        '
        Me.Guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel1.Font = New System.Drawing.Font("Cambria", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel1.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Guna2HtmlLabel1.Location = New System.Drawing.Point(12, 8)
        Me.Guna2HtmlLabel1.Name = "Guna2HtmlLabel1"
        Me.Guna2HtmlLabel1.Size = New System.Drawing.Size(119, 21)
        Me.Guna2HtmlLabel1.TabIndex = 1
        Me.Guna2HtmlLabel1.Text = "Reset Password"
        '
        'ValidBtn
        '
        Me.ValidBtn.CheckedState.Parent = Me.ValidBtn
        Me.ValidBtn.CustomImages.Parent = Me.ValidBtn
        Me.ValidBtn.FillColor = System.Drawing.Color.Green
        Me.ValidBtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ValidBtn.ForeColor = System.Drawing.Color.White
        Me.ValidBtn.HoverState.Parent = Me.ValidBtn
        Me.ValidBtn.Location = New System.Drawing.Point(222, 75)
        Me.ValidBtn.Name = "ValidBtn"
        Me.ValidBtn.PressedColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ValidBtn.ShadowDecoration.Parent = Me.ValidBtn
        Me.ValidBtn.Size = New System.Drawing.Size(101, 36)
        Me.ValidBtn.TabIndex = 15
        Me.ValidBtn.Text = "Reset Now"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 49)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(226, 13)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Enter the Valid Password to Change Password"
        '
        'ResetButton
        '
        Me.ResetButton.CheckedState.Parent = Me.ResetButton
        Me.ResetButton.CustomImages.Parent = Me.ResetButton
        Me.ResetButton.FillColor = System.Drawing.Color.Green
        Me.ResetButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ResetButton.ForeColor = System.Drawing.Color.White
        Me.ResetButton.HoverState.Parent = Me.ResetButton
        Me.ResetButton.Location = New System.Drawing.Point(222, 152)
        Me.ResetButton.Name = "ResetButton"
        Me.ResetButton.PressedColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ResetButton.ShadowDecoration.Parent = Me.ResetButton
        Me.ResetButton.Size = New System.Drawing.Size(101, 36)
        Me.ResetButton.TabIndex = 13
        Me.ResetButton.Text = "Update"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 127)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 13)
        Me.Label1.TabIndex = 11
        Me.Label1.Text = "Enter new Password"
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.Guna2Panel1.Controls.Add(Me.Resettxt)
        Me.Guna2Panel1.Controls.Add(Me.KeyTxt)
        Me.Guna2Panel1.Controls.Add(Me.Guna2Panel2)
        Me.Guna2Panel1.Controls.Add(Me.lblresult)
        Me.Guna2Panel1.Controls.Add(Me.ValidBtn)
        Me.Guna2Panel1.Controls.Add(Me.Label2)
        Me.Guna2Panel1.Controls.Add(Me.ResetButton)
        Me.Guna2Panel1.Controls.Add(Me.Label1)
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Guna2Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.ShadowDecoration.Parent = Me.Guna2Panel1
        Me.Guna2Panel1.Size = New System.Drawing.Size(371, 207)
        Me.Guna2Panel1.TabIndex = 1
        '
        'Resettxt
        '
        Me.Resettxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Resettxt.DefaultText = ""
        Me.Resettxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Resettxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Resettxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Resettxt.DisabledState.Parent = Me.Resettxt
        Me.Resettxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Resettxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Resettxt.FocusedState.Parent = Me.Resettxt
        Me.Resettxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Resettxt.HoverState.Parent = Me.Resettxt
        Me.Resettxt.Location = New System.Drawing.Point(12, 152)
        Me.Resettxt.Name = "Resettxt"
        Me.Resettxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Resettxt.PlaceholderText = ""
        Me.Resettxt.SelectedText = ""
        Me.Resettxt.ShadowDecoration.Parent = Me.Resettxt
        Me.Resettxt.Size = New System.Drawing.Size(200, 36)
        Me.Resettxt.TabIndex = 19
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
        Me.KeyTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.KeyTxt.HoverState.Parent = Me.KeyTxt
        Me.KeyTxt.Location = New System.Drawing.Point(12, 73)
        Me.KeyTxt.Name = "KeyTxt"
        Me.KeyTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.KeyTxt.PlaceholderText = ""
        Me.KeyTxt.SelectedText = ""
        Me.KeyTxt.ShadowDecoration.Parent = Me.KeyTxt
        Me.KeyTxt.Size = New System.Drawing.Size(200, 36)
        Me.KeyTxt.TabIndex = 18
        '
        'lblresult
        '
        Me.lblresult.BackColor = System.Drawing.Color.Transparent
        Me.lblresult.Location = New System.Drawing.Point(80, 107)
        Me.lblresult.Name = "lblresult"
        Me.lblresult.Size = New System.Drawing.Size(3, 2)
        Me.lblresult.TabIndex = 16
        Me.lblresult.Text = Nothing
        '
        'PSDForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(371, 207)
        Me.Controls.Add(Me.Guna2Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "PSDForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "PSDForm"
        Me.Guna2Panel2.ResumeLayout(False)
        Me.Guna2Panel2.PerformLayout()
        Me.Guna2Panel1.ResumeLayout(False)
        Me.Guna2Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Guna2Panel2 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2ControlBox1 As Guna.UI2.WinForms.Guna2ControlBox
    Friend WithEvents Guna2HtmlLabel1 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents ValidBtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Label2 As Label
    Friend WithEvents ResetButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents lblresult As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2Elipse As Guna.UI2.WinForms.Guna2Elipse
    Friend WithEvents Resettxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents KeyTxt As Guna.UI2.WinForms.Guna2TextBox
End Class

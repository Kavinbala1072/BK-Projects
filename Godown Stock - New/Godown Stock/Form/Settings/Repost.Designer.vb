<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Repost
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
        Me.ValidBtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.KeyTxt = New ComponentFactory.Krypton.Toolkit.KryptonTextBox()
        Me.HeaderPanel = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2ControlBox1 = New Guna.UI2.WinForms.Guna2ControlBox()
        Me.Guna2HtmlLabel1 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.lblresult = New System.Windows.Forms.Label()
        Me.HeaderPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'ValidBtn
        '
        Me.ValidBtn.CheckedState.Parent = Me.ValidBtn
        Me.ValidBtn.CustomImages.Parent = Me.ValidBtn
        Me.ValidBtn.FillColor = System.Drawing.Color.Green
        Me.ValidBtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ValidBtn.ForeColor = System.Drawing.Color.White
        Me.ValidBtn.HoverState.Parent = Me.ValidBtn
        Me.ValidBtn.Location = New System.Drawing.Point(228, 81)
        Me.ValidBtn.Name = "ValidBtn"
        Me.ValidBtn.PressedColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.ValidBtn.ShadowDecoration.Parent = Me.ValidBtn
        Me.ValidBtn.Size = New System.Drawing.Size(59, 24)
        Me.ValidBtn.TabIndex = 18
        Me.ValidBtn.Text = "OK"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(18, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(174, 13)
        Me.Label2.TabIndex = 17
        Me.Label2.Text = "Enter the Valid Password to Repost"
        '
        'KeyTxt
        '
        Me.KeyTxt.Location = New System.Drawing.Point(21, 82)
        Me.KeyTxt.Name = "KeyTxt"
        Me.KeyTxt.Size = New System.Drawing.Size(192, 23)
        Me.KeyTxt.TabIndex = 16
        Me.KeyTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'HeaderPanel
        '
        Me.HeaderPanel.Controls.Add(Me.Guna2ControlBox1)
        Me.HeaderPanel.Controls.Add(Me.Guna2HtmlLabel1)
        Me.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.HeaderPanel.Location = New System.Drawing.Point(0, 0)
        Me.HeaderPanel.Name = "HeaderPanel"
        Me.HeaderPanel.ShadowDecoration.Parent = Me.HeaderPanel
        Me.HeaderPanel.Size = New System.Drawing.Size(309, 36)
        Me.HeaderPanel.TabIndex = 19
        '
        'Guna2ControlBox1
        '
        Me.Guna2ControlBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Guna2ControlBox1.FillColor = System.Drawing.Color.Red
        Me.Guna2ControlBox1.HoverState.Parent = Me.Guna2ControlBox1
        Me.Guna2ControlBox1.IconColor = System.Drawing.Color.White
        Me.Guna2ControlBox1.Location = New System.Drawing.Point(274, 7)
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
        Me.Guna2HtmlLabel1.Size = New System.Drawing.Size(130, 21)
        Me.Guna2HtmlLabel1.TabIndex = 1
        Me.Guna2HtmlLabel1.Text = "Repost Password"
        '
        'lblresult
        '
        Me.lblresult.AutoSize = True
        Me.lblresult.Location = New System.Drawing.Point(113, 118)
        Me.lblresult.Name = "lblresult"
        Me.lblresult.Size = New System.Drawing.Size(10, 13)
        Me.lblresult.TabIndex = 20
        Me.lblresult.Text = " "
        '
        'Repost
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(309, 136)
        Me.Controls.Add(Me.lblresult)
        Me.Controls.Add(Me.HeaderPanel)
        Me.Controls.Add(Me.ValidBtn)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.KeyTxt)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Repost"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Repost"
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ValidBtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Label2 As Label
    Friend WithEvents KeyTxt As ComponentFactory.Krypton.Toolkit.KryptonTextBox
    Friend WithEvents HeaderPanel As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2ControlBox1 As Guna.UI2.WinForms.Guna2ControlBox
    Friend WithEvents Guna2HtmlLabel1 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents lblresult As Label
End Class

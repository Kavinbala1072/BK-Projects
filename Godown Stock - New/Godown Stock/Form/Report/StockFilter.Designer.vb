<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class StockFilter
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
        Me.components = New System.ComponentModel.Container()
        Me.grpbox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Brandbox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Modelbox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.OKButton = New Guna.UI2.WinForms.Guna2Button()
        Me.HeaderPanel = New Guna.UI2.WinForms.Guna2Panel()
        Me.Guna2ControlBox1 = New Guna.UI2.WinForms.Guna2ControlBox()
        Me.Label = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel1 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel2 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel3 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.KryptonListBox = New ComponentFactory.Krypton.Toolkit.KryptonListBox()
        Me.ClearButton = New Guna.UI2.WinForms.Guna2Button()
        Me.TimerFocusCheck = New System.Windows.Forms.Timer(Me.components)
        Me.HeaderPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpbox
        '
        Me.grpbox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.grpbox.DefaultText = ""
        Me.grpbox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.grpbox.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.grpbox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.grpbox.DisabledState.Parent = Me.grpbox
        Me.grpbox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.grpbox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.grpbox.FocusedState.Parent = Me.grpbox
        Me.grpbox.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.grpbox.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.grpbox.HoverState.Parent = Me.grpbox
        Me.grpbox.Location = New System.Drawing.Point(101, 41)
        Me.grpbox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpbox.Name = "grpbox"
        Me.grpbox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.grpbox.PlaceholderText = ""
        Me.grpbox.SelectedText = ""
        Me.grpbox.ShadowDecoration.Parent = Me.grpbox
        Me.grpbox.Size = New System.Drawing.Size(200, 36)
        Me.grpbox.TabIndex = 0
        '
        'Brandbox
        '
        Me.Brandbox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Brandbox.DefaultText = ""
        Me.Brandbox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Brandbox.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Brandbox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Brandbox.DisabledState.Parent = Me.Brandbox
        Me.Brandbox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Brandbox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Brandbox.FocusedState.Parent = Me.Brandbox
        Me.Brandbox.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Brandbox.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Brandbox.HoverState.Parent = Me.Brandbox
        Me.Brandbox.Location = New System.Drawing.Point(101, 87)
        Me.Brandbox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Brandbox.Name = "Brandbox"
        Me.Brandbox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Brandbox.PlaceholderText = ""
        Me.Brandbox.SelectedText = ""
        Me.Brandbox.ShadowDecoration.Parent = Me.Brandbox
        Me.Brandbox.Size = New System.Drawing.Size(200, 36)
        Me.Brandbox.TabIndex = 1
        '
        'Modelbox
        '
        Me.Modelbox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Modelbox.DefaultText = ""
        Me.Modelbox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Modelbox.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Modelbox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Modelbox.DisabledState.Parent = Me.Modelbox
        Me.Modelbox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Modelbox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Modelbox.FocusedState.Parent = Me.Modelbox
        Me.Modelbox.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Modelbox.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Modelbox.HoverState.Parent = Me.Modelbox
        Me.Modelbox.Location = New System.Drawing.Point(101, 133)
        Me.Modelbox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Modelbox.Name = "Modelbox"
        Me.Modelbox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Modelbox.PlaceholderText = ""
        Me.Modelbox.SelectedText = ""
        Me.Modelbox.ShadowDecoration.Parent = Me.Modelbox
        Me.Modelbox.Size = New System.Drawing.Size(200, 36)
        Me.Modelbox.TabIndex = 2
        '
        'OKButton
        '
        Me.OKButton.CheckedState.Parent = Me.OKButton
        Me.OKButton.CustomImages.Parent = Me.OKButton
        Me.OKButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.OKButton.ForeColor = System.Drawing.Color.White
        Me.OKButton.HoverState.Parent = Me.OKButton
        Me.OKButton.Location = New System.Drawing.Point(186, 345)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.ShadowDecoration.Parent = Me.OKButton
        Me.OKButton.Size = New System.Drawing.Size(115, 40)
        Me.OKButton.TabIndex = 3
        Me.OKButton.Text = "Ok "
        '
        'HeaderPanel
        '
        Me.HeaderPanel.Controls.Add(Me.Guna2ControlBox1)
        Me.HeaderPanel.Controls.Add(Me.Label)
        Me.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.HeaderPanel.Location = New System.Drawing.Point(0, 0)
        Me.HeaderPanel.Name = "HeaderPanel"
        Me.HeaderPanel.ShadowDecoration.Parent = Me.HeaderPanel
        Me.HeaderPanel.Size = New System.Drawing.Size(313, 36)
        Me.HeaderPanel.TabIndex = 5
        '
        'Guna2ControlBox1
        '
        Me.Guna2ControlBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Guna2ControlBox1.FillColor = System.Drawing.Color.Red
        Me.Guna2ControlBox1.HoverState.Parent = Me.Guna2ControlBox1
        Me.Guna2ControlBox1.IconColor = System.Drawing.Color.White
        Me.Guna2ControlBox1.Location = New System.Drawing.Point(286, 5)
        Me.Guna2ControlBox1.Name = "Guna2ControlBox1"
        Me.Guna2ControlBox1.ShadowDecoration.Parent = Me.Guna2ControlBox1
        Me.Guna2ControlBox1.Size = New System.Drawing.Size(22, 25)
        Me.Guna2ControlBox1.TabIndex = 3
        '
        'Label
        '
        Me.Label.BackColor = System.Drawing.Color.Transparent
        Me.Label.Font = New System.Drawing.Font("Tahoma", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label.ForeColor = System.Drawing.Color.White
        Me.Label.Location = New System.Drawing.Point(12, 9)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(32, 18)
        Me.Label.TabIndex = 2
        Me.Label.Text = "Filter"
        '
        'Guna2HtmlLabel1
        '
        Me.Guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel1.Location = New System.Drawing.Point(12, 51)
        Me.Guna2HtmlLabel1.Name = "Guna2HtmlLabel1"
        Me.Guna2HtmlLabel1.Size = New System.Drawing.Size(40, 16)
        Me.Guna2HtmlLabel1.TabIndex = 6
        Me.Guna2HtmlLabel1.Text = "Group"
        '
        'Guna2HtmlLabel2
        '
        Me.Guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel2.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel2.Location = New System.Drawing.Point(12, 97)
        Me.Guna2HtmlLabel2.Name = "Guna2HtmlLabel2"
        Me.Guna2HtmlLabel2.Size = New System.Drawing.Size(39, 16)
        Me.Guna2HtmlLabel2.TabIndex = 7
        Me.Guna2HtmlLabel2.Text = "Brand"
        '
        'Guna2HtmlLabel3
        '
        Me.Guna2HtmlLabel3.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel3.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Guna2HtmlLabel3.Location = New System.Drawing.Point(12, 143)
        Me.Guna2HtmlLabel3.Name = "Guna2HtmlLabel3"
        Me.Guna2HtmlLabel3.Size = New System.Drawing.Size(40, 16)
        Me.Guna2HtmlLabel3.TabIndex = 8
        Me.Guna2HtmlLabel3.Text = "Model"
        '
        'KryptonListBox
        '
        Me.KryptonListBox.Location = New System.Drawing.Point(12, 176)
        Me.KryptonListBox.Name = "KryptonListBox"
        Me.KryptonListBox.Size = New System.Drawing.Size(289, 163)
        Me.KryptonListBox.TabIndex = 9
        '
        'ClearButton
        '
        Me.ClearButton.CheckedState.Parent = Me.ClearButton
        Me.ClearButton.CustomImages.Parent = Me.ClearButton
        Me.ClearButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ClearButton.ForeColor = System.Drawing.Color.White
        Me.ClearButton.HoverState.Parent = Me.ClearButton
        Me.ClearButton.Location = New System.Drawing.Point(65, 345)
        Me.ClearButton.Name = "ClearButton"
        Me.ClearButton.ShadowDecoration.Parent = Me.ClearButton
        Me.ClearButton.Size = New System.Drawing.Size(115, 40)
        Me.ClearButton.TabIndex = 4
        Me.ClearButton.TabStop = False
        Me.ClearButton.Text = "Clear"
        '
        'StockFilter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(313, 397)
        Me.Controls.Add(Me.ClearButton)
        Me.Controls.Add(Me.KryptonListBox)
        Me.Controls.Add(Me.Guna2HtmlLabel3)
        Me.Controls.Add(Me.Guna2HtmlLabel2)
        Me.Controls.Add(Me.Guna2HtmlLabel1)
        Me.Controls.Add(Me.HeaderPanel)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.Modelbox)
        Me.Controls.Add(Me.Brandbox)
        Me.Controls.Add(Me.grpbox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "StockFilter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "StockFilter"
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents grpbox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Brandbox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Modelbox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents OKButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents HeaderPanel As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2ControlBox1 As Guna.UI2.WinForms.Guna2ControlBox
    Friend WithEvents Label As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel1 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel2 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel3 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents KryptonListBox As ComponentFactory.Krypton.Toolkit.KryptonListBox
    Friend WithEvents ClearButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents TimerFocusCheck As Timer
End Class

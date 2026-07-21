<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class WeeklyReport
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.HeaderPanel = New Guna.UI2.WinForms.Guna2ShadowPanel()
        Me.Label = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2DataGridView1 = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.FromDateTextBox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.PrintButton = New Guna.UI2.WinForms.Guna2Button()
        Me.ToDateTextBox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.RefreshButton = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.FlterButton = New Guna.UI2.WinForms.Guna2Button()
        Me.ProgressBar = New Guna.UI2.WinForms.Guna2ProgressBar()
        Me.HeaderPanel.SuspendLayout()
        CType(Me.Guna2DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Guna2Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'HeaderPanel
        '
        Me.HeaderPanel.BackColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.Controls.Add(Me.Label)
        Me.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.HeaderPanel.FillColor = System.Drawing.Color.Empty
        Me.HeaderPanel.ForeColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.Location = New System.Drawing.Point(0, 0)
        Me.HeaderPanel.Name = "HeaderPanel"
        Me.HeaderPanel.ShadowColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.ShadowDepth = 0
        Me.HeaderPanel.Size = New System.Drawing.Size(872, 42)
        Me.HeaderPanel.TabIndex = 85
        '
        'Label
        '
        Me.Label.BackColor = System.Drawing.Color.Transparent
        Me.Label.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label.ForeColor = System.Drawing.Color.White
        Me.Label.Location = New System.Drawing.Point(12, 7)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(222, 27)
        Me.Label.TabIndex = 1
        Me.Label.Text = "Stock Weekly Report"
        '
        'Guna2DataGridView1
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Guna2DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.Guna2DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Guna2DataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.Guna2DataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Guna2DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.Guna2DataGridView1.ColumnHeadersHeight = 4
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Guna2DataGridView1.DefaultCellStyle = DataGridViewCellStyle3
        Me.Guna2DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Guna2DataGridView1.EnableHeadersVisualStyles = False
        Me.Guna2DataGridView1.GridColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2DataGridView1.Location = New System.Drawing.Point(0, 85)
        Me.Guna2DataGridView1.Name = "Guna2DataGridView1"
        Me.Guna2DataGridView1.RowHeadersVisible = False
        Me.Guna2DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Guna2DataGridView1.Size = New System.Drawing.Size(872, 393)
        Me.Guna2DataGridView1.TabIndex = 92
        Me.Guna2DataGridView1.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.[Default]
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.Font = Nothing
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty
        Me.Guna2DataGridView1.ThemeStyle.BackColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2DataGridView1.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2DataGridView1.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.Guna2DataGridView1.ThemeStyle.HeaderStyle.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        Me.Guna2DataGridView1.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        Me.Guna2DataGridView1.ThemeStyle.HeaderStyle.Height = 4
        Me.Guna2DataGridView1.ThemeStyle.ReadOnly = False
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.Height = 22
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        '
        'FromDateTextBox
        '
        Me.FromDateTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.FromDateTextBox.DefaultText = ""
        Me.FromDateTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.FromDateTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.FromDateTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.FromDateTextBox.DisabledState.Parent = Me.FromDateTextBox
        Me.FromDateTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.FromDateTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.FromDateTextBox.FocusedState.Parent = Me.FromDateTextBox
        Me.FromDateTextBox.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.FromDateTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.FromDateTextBox.HoverState.Parent = Me.FromDateTextBox
        Me.FromDateTextBox.Location = New System.Drawing.Point(86, 4)
        Me.FromDateTextBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.FromDateTextBox.Name = "FromDateTextBox"
        Me.FromDateTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.FromDateTextBox.PlaceholderText = ""
        Me.FromDateTextBox.SelectedText = ""
        Me.FromDateTextBox.ShadowDecoration.Parent = Me.FromDateTextBox
        Me.FromDateTextBox.Size = New System.Drawing.Size(110, 36)
        Me.FromDateTextBox.TabIndex = 0
        '
        'PrintButton
        '
        Me.PrintButton.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.PrintButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.PrintButton.CheckedState.Parent = Me.PrintButton
        Me.PrintButton.CustomImages.Parent = Me.PrintButton
        Me.PrintButton.FillColor = System.Drawing.SystemColors.ButtonShadow
        Me.PrintButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.PrintButton.ForeColor = System.Drawing.Color.White
        Me.PrintButton.HoverState.Parent = Me.PrintButton
        Me.PrintButton.Image = Global.Godown_Stock.My.Resources.Resources.printer_9654640
        Me.PrintButton.Location = New System.Drawing.Point(792, 6)
        Me.PrintButton.Name = "PrintButton"
        Me.PrintButton.ShadowDecoration.Parent = Me.PrintButton
        Me.PrintButton.Size = New System.Drawing.Size(72, 32)
        Me.PrintButton.TabIndex = 3
        Me.PrintButton.TabStop = False
        Me.PrintButton.Text = "Print"
        '
        'ToDateTextBox
        '
        Me.ToDateTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ToDateTextBox.DefaultText = ""
        Me.ToDateTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.ToDateTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.ToDateTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.ToDateTextBox.DisabledState.Parent = Me.ToDateTextBox
        Me.ToDateTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.ToDateTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ToDateTextBox.FocusedState.Parent = Me.ToDateTextBox
        Me.ToDateTextBox.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.ToDateTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ToDateTextBox.HoverState.Parent = Me.ToDateTextBox
        Me.ToDateTextBox.Location = New System.Drawing.Point(200, 4)
        Me.ToDateTextBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ToDateTextBox.Name = "ToDateTextBox"
        Me.ToDateTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.ToDateTextBox.PlaceholderText = ""
        Me.ToDateTextBox.SelectedText = ""
        Me.ToDateTextBox.ShadowDecoration.Parent = Me.ToDateTextBox
        Me.ToDateTextBox.Size = New System.Drawing.Size(110, 36)
        Me.ToDateTextBox.TabIndex = 1
        '
        'RefreshButton
        '
        Me.RefreshButton.CheckedState.Parent = Me.RefreshButton
        Me.RefreshButton.CustomImages.Parent = Me.RefreshButton
        Me.RefreshButton.FillColor = System.Drawing.Color.RoyalBlue
        Me.RefreshButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.RefreshButton.ForeColor = System.Drawing.Color.White
        Me.RefreshButton.HoverState.Parent = Me.RefreshButton
        Me.RefreshButton.Image = Global.Godown_Stock.My.Resources.Resources.Refresh
        Me.RefreshButton.Location = New System.Drawing.Point(315, 6)
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.ShadowDecoration.Parent = Me.RefreshButton
        Me.RefreshButton.Size = New System.Drawing.Size(72, 32)
        Me.RefreshButton.TabIndex = 2
        Me.RefreshButton.Text = "Refresh"
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.Controls.Add(Me.FlterButton)
        Me.Guna2Panel1.Controls.Add(Me.RefreshButton)
        Me.Guna2Panel1.Controls.Add(Me.ToDateTextBox)
        Me.Guna2Panel1.Controls.Add(Me.PrintButton)
        Me.Guna2Panel1.Controls.Add(Me.FromDateTextBox)
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Guna2Panel1.Location = New System.Drawing.Point(0, 42)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.ShadowDecoration.Parent = Me.Guna2Panel1
        Me.Guna2Panel1.Size = New System.Drawing.Size(872, 43)
        Me.Guna2Panel1.TabIndex = 91
        '
        'FlterButton
        '
        Me.FlterButton.CheckedState.Parent = Me.FlterButton
        Me.FlterButton.CustomImages.Parent = Me.FlterButton
        Me.FlterButton.FillColor = System.Drawing.Color.RoyalBlue
        Me.FlterButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.FlterButton.ForeColor = System.Drawing.Color.White
        Me.FlterButton.HoverState.Parent = Me.FlterButton
        Me.FlterButton.Location = New System.Drawing.Point(9, 6)
        Me.FlterButton.Name = "FlterButton"
        Me.FlterButton.ShadowDecoration.Parent = Me.FlterButton
        Me.FlterButton.Size = New System.Drawing.Size(72, 32)
        Me.FlterButton.TabIndex = 4
        Me.FlterButton.TabStop = False
        Me.FlterButton.Text = "Filter"
        '
        'ProgressBar
        '
        Me.ProgressBar.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.ProgressBar.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal
        Me.ProgressBar.Location = New System.Drawing.Point(0, 448)
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.ShadowDecoration.Parent = Me.ProgressBar
        Me.ProgressBar.Size = New System.Drawing.Size(872, 30)
        Me.ProgressBar.TabIndex = 95
        Me.ProgressBar.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        '
        'WeeklyReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(872, 478)
        Me.Controls.Add(Me.ProgressBar)
        Me.Controls.Add(Me.Guna2DataGridView1)
        Me.Controls.Add(Me.Guna2Panel1)
        Me.Controls.Add(Me.HeaderPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "WeeklyReport"
        Me.Text = "WeeklyReport"
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        CType(Me.Guna2DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Guna2Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents HeaderPanel As Guna.UI2.WinForms.Guna2ShadowPanel
    Friend WithEvents Label As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2DataGridView1 As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents FromDateTextBox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents PrintButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents ToDateTextBox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents RefreshButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents FlterButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents ProgressBar As Guna.UI2.WinForms.Guna2ProgressBar
End Class

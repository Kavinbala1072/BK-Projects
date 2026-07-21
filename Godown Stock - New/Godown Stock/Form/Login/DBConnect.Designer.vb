<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class DBConnect
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
        Me.Guna2Panel2 = New Guna.UI2.WinForms.Guna2Panel()
        Me.LatestUpdateButton = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2ControlBox1 = New Guna.UI2.WinForms.Guna2ControlBox()
        Me.Guna2HtmlLabel1 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.BackupProgressBar = New Guna.UI2.WinForms.Guna2ProgressBar()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.servertxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.dbtxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Psdtxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.UserText = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.dblocationtxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.BackupPathtxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.RestoreTxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.LogoPanelControl = New Guna.UI2.WinForms.Guna2Panel()
        Me.lblresult = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.ResetButton = New Guna.UI2.WinForms.Guna2Button()
        Me.FileCButton = New Guna.UI2.WinForms.Guna2Button()
        Me.RestoreButton = New Guna.UI2.WinForms.Guna2Button()
        Me.UpdateButton = New Guna.UI2.WinForms.Guna2Button()
        Me.BackupButton = New Guna.UI2.WinForms.Guna2Button()
        Me.DatabaseButton = New Guna.UI2.WinForms.Guna2Button()
        Me.NextButton = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2Elipse = New Guna.UI2.WinForms.Guna2Elipse(Me.components)
        Me.Guna2Panel2.SuspendLayout()
        Me.Guna2Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Guna2Panel2
        '
        Me.Guna2Panel2.Controls.Add(Me.LatestUpdateButton)
        Me.Guna2Panel2.Controls.Add(Me.Guna2ControlBox1)
        Me.Guna2Panel2.Controls.Add(Me.Guna2HtmlLabel1)
        Me.Guna2Panel2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Guna2Panel2.FillColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.Guna2Panel2.Location = New System.Drawing.Point(0, 0)
        Me.Guna2Panel2.Name = "Guna2Panel2"
        Me.Guna2Panel2.ShadowDecoration.Parent = Me.Guna2Panel2
        Me.Guna2Panel2.Size = New System.Drawing.Size(495, 36)
        Me.Guna2Panel2.TabIndex = 0
        '
        'LatestUpdateButton
        '
        Me.LatestUpdateButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LatestUpdateButton.BackColor = System.Drawing.Color.Transparent
        Me.LatestUpdateButton.BorderColor = System.Drawing.Color.Transparent
        Me.LatestUpdateButton.CheckedState.Parent = Me.LatestUpdateButton
        Me.LatestUpdateButton.CustomImages.Parent = Me.LatestUpdateButton
        Me.LatestUpdateButton.FillColor = System.Drawing.Color.Transparent
        Me.LatestUpdateButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.LatestUpdateButton.ForeColor = System.Drawing.Color.White
        Me.LatestUpdateButton.HoverState.Parent = Me.LatestUpdateButton
        Me.LatestUpdateButton.Image = Global.Godown_Stock.My.Resources.Resources.icons8_update_64
        Me.LatestUpdateButton.Location = New System.Drawing.Point(348, 3)
        Me.LatestUpdateButton.Name = "LatestUpdateButton"
        Me.LatestUpdateButton.PressedColor = System.Drawing.Color.Transparent
        Me.LatestUpdateButton.ShadowDecoration.Parent = Me.LatestUpdateButton
        Me.LatestUpdateButton.Size = New System.Drawing.Size(109, 30)
        Me.LatestUpdateButton.TabIndex = 129
        Me.LatestUpdateButton.Text = "Latest Update"
        '
        'Guna2ControlBox1
        '
        Me.Guna2ControlBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Guna2ControlBox1.FillColor = System.Drawing.Color.Red
        Me.Guna2ControlBox1.HoverState.Parent = Me.Guna2ControlBox1
        Me.Guna2ControlBox1.IconColor = System.Drawing.Color.White
        Me.Guna2ControlBox1.Location = New System.Drawing.Point(466, 6)
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
        Me.Guna2HtmlLabel1.Size = New System.Drawing.Size(44, 21)
        Me.Guna2HtmlLabel1.TabIndex = 1
        Me.Guna2HtmlLabel1.Text = "Login"
        '
        'BackupProgressBar
        '
        Me.BackupProgressBar.BorderColor = System.Drawing.Color.Gray
        Me.BackupProgressBar.BorderThickness = 1
        Me.BackupProgressBar.FillColor = System.Drawing.Color.White
        Me.BackupProgressBar.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal
        Me.BackupProgressBar.Location = New System.Drawing.Point(254, 60)
        Me.BackupProgressBar.Name = "BackupProgressBar"
        Me.BackupProgressBar.ProgressColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.BackupProgressBar.ProgressColor2 = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.BackupProgressBar.ShadowDecoration.Parent = Me.BackupProgressBar
        Me.BackupProgressBar.Size = New System.Drawing.Size(234, 35)
        Me.BackupProgressBar.TabIndex = 121
        Me.BackupProgressBar.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(253, 154)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 15)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "Login User"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(253, 211)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(61, 15)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Password"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(10, 156)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(61, 15)
        Me.Label6.TabIndex = 22
        Me.Label6.Text = "DB Name"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(10, 98)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 15)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "Server Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(10, 42)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(74, 15)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "DB Location"
        '
        'servertxt
        '
        Me.servertxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.servertxt.DefaultText = ""
        Me.servertxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.servertxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.servertxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.servertxt.DisabledState.Parent = Me.servertxt
        Me.servertxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.servertxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.servertxt.FocusedState.Parent = Me.servertxt
        Me.servertxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.servertxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.servertxt.HoverState.Parent = Me.servertxt
        Me.servertxt.Location = New System.Drawing.Point(10, 116)
        Me.servertxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.servertxt.Name = "servertxt"
        Me.servertxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.servertxt.PlaceholderText = ""
        Me.servertxt.SelectedText = ""
        Me.servertxt.ShadowDecoration.Parent = Me.servertxt
        Me.servertxt.Size = New System.Drawing.Size(233, 35)
        Me.servertxt.TabIndex = 8
        Me.servertxt.TabStop = False
        '
        'dbtxt
        '
        Me.dbtxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.dbtxt.DefaultText = ""
        Me.dbtxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.dbtxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.dbtxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.dbtxt.DisabledState.Parent = Me.dbtxt
        Me.dbtxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.dbtxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dbtxt.FocusedState.Parent = Me.dbtxt
        Me.dbtxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.dbtxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dbtxt.HoverState.Parent = Me.dbtxt
        Me.dbtxt.Location = New System.Drawing.Point(10, 174)
        Me.dbtxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dbtxt.Name = "dbtxt"
        Me.dbtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.dbtxt.PlaceholderText = ""
        Me.dbtxt.SelectedText = ""
        Me.dbtxt.ShadowDecoration.Parent = Me.dbtxt
        Me.dbtxt.Size = New System.Drawing.Size(144, 35)
        Me.dbtxt.TabIndex = 6
        Me.dbtxt.TabStop = False
        '
        'Psdtxt
        '
        Me.Psdtxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Psdtxt.DefaultText = ""
        Me.Psdtxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Psdtxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Psdtxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Psdtxt.DisabledState.Parent = Me.Psdtxt
        Me.Psdtxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Psdtxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Psdtxt.FocusedState.Parent = Me.Psdtxt
        Me.Psdtxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Psdtxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Psdtxt.HoverState.Parent = Me.Psdtxt
        Me.Psdtxt.Location = New System.Drawing.Point(253, 232)
        Me.Psdtxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Psdtxt.Name = "Psdtxt"
        Me.Psdtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Psdtxt.PlaceholderText = ""
        Me.Psdtxt.SelectedText = ""
        Me.Psdtxt.ShadowDecoration.Parent = Me.Psdtxt
        Me.Psdtxt.Size = New System.Drawing.Size(234, 35)
        Me.Psdtxt.TabIndex = 0
        '
        'UserText
        '
        Me.UserText.BackColor = System.Drawing.Color.Transparent
        Me.UserText.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.UserText.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.UserText.FocusedColor = System.Drawing.Color.Empty
        Me.UserText.FocusedState.Parent = Me.UserText
        Me.UserText.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.UserText.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(112, Byte), Integer))
        Me.UserText.FormattingEnabled = True
        Me.UserText.HoverState.Parent = Me.UserText
        Me.UserText.ItemHeight = 30
        Me.UserText.ItemsAppearance.Parent = Me.UserText
        Me.UserText.Location = New System.Drawing.Point(253, 172)
        Me.UserText.Name = "UserText"
        Me.UserText.ShadowDecoration.Parent = Me.UserText
        Me.UserText.Size = New System.Drawing.Size(233, 36)
        Me.UserText.TabIndex = 5
        Me.UserText.TabStop = False
        '
        'dblocationtxt
        '
        Me.dblocationtxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.dblocationtxt.DefaultText = ""
        Me.dblocationtxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.dblocationtxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.dblocationtxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.dblocationtxt.DisabledState.Parent = Me.dblocationtxt
        Me.dblocationtxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.dblocationtxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dblocationtxt.FocusedState.Parent = Me.dblocationtxt
        Me.dblocationtxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.dblocationtxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.dblocationtxt.HoverState.Parent = Me.dblocationtxt
        Me.dblocationtxt.Location = New System.Drawing.Point(10, 60)
        Me.dblocationtxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.dblocationtxt.Name = "dblocationtxt"
        Me.dblocationtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.dblocationtxt.PlaceholderText = ""
        Me.dblocationtxt.SelectedText = ""
        Me.dblocationtxt.ShadowDecoration.Parent = Me.dblocationtxt
        Me.dblocationtxt.Size = New System.Drawing.Size(234, 35)
        Me.dblocationtxt.TabIndex = 7
        Me.dblocationtxt.TabStop = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(253, 42)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(76, 15)
        Me.Label7.TabIndex = 37
        Me.Label7.Text = "Backup Path"
        '
        'BackupPathtxt
        '
        Me.BackupPathtxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.BackupPathtxt.DefaultText = ""
        Me.BackupPathtxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.BackupPathtxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.BackupPathtxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.BackupPathtxt.DisabledState.Parent = Me.BackupPathtxt
        Me.BackupPathtxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.BackupPathtxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BackupPathtxt.FocusedState.Parent = Me.BackupPathtxt
        Me.BackupPathtxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.BackupPathtxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BackupPathtxt.HoverState.Parent = Me.BackupPathtxt
        Me.BackupPathtxt.Location = New System.Drawing.Point(254, 60)
        Me.BackupPathtxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BackupPathtxt.Name = "BackupPathtxt"
        Me.BackupPathtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.BackupPathtxt.PlaceholderText = ""
        Me.BackupPathtxt.SelectedText = ""
        Me.BackupPathtxt.ShadowDecoration.Parent = Me.BackupPathtxt
        Me.BackupPathtxt.Size = New System.Drawing.Size(233, 35)
        Me.BackupPathtxt.TabIndex = 7
        Me.BackupPathtxt.TabStop = False
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(10, 210)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(94, 15)
        Me.Label8.TabIndex = 123
        Me.Label8.Text = "Backup Restore"
        '
        'RestoreTxt
        '
        Me.RestoreTxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.RestoreTxt.DefaultText = ""
        Me.RestoreTxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.RestoreTxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.RestoreTxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.RestoreTxt.DisabledState.Parent = Me.RestoreTxt
        Me.RestoreTxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.RestoreTxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RestoreTxt.FocusedState.Parent = Me.RestoreTxt
        Me.RestoreTxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.RestoreTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RestoreTxt.HoverState.Parent = Me.RestoreTxt
        Me.RestoreTxt.Location = New System.Drawing.Point(10, 232)
        Me.RestoreTxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RestoreTxt.Name = "RestoreTxt"
        Me.RestoreTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.RestoreTxt.PlaceholderText = ""
        Me.RestoreTxt.SelectedText = ""
        Me.RestoreTxt.ShadowDecoration.Parent = Me.RestoreTxt
        Me.RestoreTxt.Size = New System.Drawing.Size(189, 35)
        Me.RestoreTxt.TabIndex = 11
        Me.RestoreTxt.TabStop = False
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.Controls.Add(Me.BackupProgressBar)
        Me.Guna2Panel1.Controls.Add(Me.LogoPanelControl)
        Me.Guna2Panel1.Controls.Add(Me.lblresult)
        Me.Guna2Panel1.Controls.Add(Me.ResetButton)
        Me.Guna2Panel1.Controls.Add(Me.FileCButton)
        Me.Guna2Panel1.Controls.Add(Me.RestoreButton)
        Me.Guna2Panel1.Controls.Add(Me.RestoreTxt)
        Me.Guna2Panel1.Controls.Add(Me.Label8)
        Me.Guna2Panel1.Controls.Add(Me.UpdateButton)
        Me.Guna2Panel1.Controls.Add(Me.BackupButton)
        Me.Guna2Panel1.Controls.Add(Me.BackupPathtxt)
        Me.Guna2Panel1.Controls.Add(Me.Label7)
        Me.Guna2Panel1.Controls.Add(Me.dblocationtxt)
        Me.Guna2Panel1.Controls.Add(Me.UserText)
        Me.Guna2Panel1.Controls.Add(Me.Psdtxt)
        Me.Guna2Panel1.Controls.Add(Me.dbtxt)
        Me.Guna2Panel1.Controls.Add(Me.servertxt)
        Me.Guna2Panel1.Controls.Add(Me.DatabaseButton)
        Me.Guna2Panel1.Controls.Add(Me.Label1)
        Me.Guna2Panel1.Controls.Add(Me.Label5)
        Me.Guna2Panel1.Controls.Add(Me.Label6)
        Me.Guna2Panel1.Controls.Add(Me.Label4)
        Me.Guna2Panel1.Controls.Add(Me.Label3)
        Me.Guna2Panel1.Controls.Add(Me.NextButton)
        Me.Guna2Panel1.Controls.Add(Me.Guna2Panel2)
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Guna2Panel1.FillColor = System.Drawing.Color.White
        Me.Guna2Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.ShadowDecoration.Parent = Me.Guna2Panel1
        Me.Guna2Panel1.Size = New System.Drawing.Size(495, 336)
        Me.Guna2Panel1.TabIndex = 0
        '
        'LogoPanelControl
        '
        Me.LogoPanelControl.BackColor = System.Drawing.Color.White
        Me.LogoPanelControl.BackgroundImage = Global.Godown_Stock.My.Resources.Resources.Logo
        Me.LogoPanelControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.LogoPanelControl.BorderColor = System.Drawing.Color.White
        Me.LogoPanelControl.CustomBorderColor = System.Drawing.Color.Transparent
        Me.LogoPanelControl.FillColor = System.Drawing.Color.Transparent
        Me.LogoPanelControl.Location = New System.Drawing.Point(10, 42)
        Me.LogoPanelControl.Name = "LogoPanelControl"
        Me.LogoPanelControl.ShadowDecoration.Parent = Me.LogoPanelControl
        Me.LogoPanelControl.Size = New System.Drawing.Size(234, 284)
        Me.LogoPanelControl.TabIndex = 28
        '
        'lblresult
        '
        Me.lblresult.BackColor = System.Drawing.Color.Transparent
        Me.lblresult.Location = New System.Drawing.Point(309, 272)
        Me.lblresult.Name = "lblresult"
        Me.lblresult.Size = New System.Drawing.Size(123, 15)
        Me.lblresult.TabIndex = 127
        Me.lblresult.Text = "INVALID CREDENTIALS"
        '
        'ResetButton
        '
        Me.ResetButton.BackColor = System.Drawing.Color.Transparent
        Me.ResetButton.CheckedState.Parent = Me.ResetButton
        Me.ResetButton.CustomImages.Parent = Me.ResetButton
        Me.ResetButton.FillColor = System.Drawing.Color.Transparent
        Me.ResetButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ResetButton.ForeColor = System.Drawing.Color.Red
        Me.ResetButton.HoverState.Parent = Me.ResetButton
        Me.ResetButton.Image = Global.Godown_Stock.My.Resources.Resources.icons8_reset_password_66
        Me.ResetButton.Location = New System.Drawing.Point(367, 211)
        Me.ResetButton.Name = "ResetButton"
        Me.ResetButton.PressedColor = System.Drawing.Color.Transparent
        Me.ResetButton.ShadowDecoration.Color = System.Drawing.Color.Transparent
        Me.ResetButton.ShadowDecoration.Parent = Me.ResetButton
        Me.ResetButton.Size = New System.Drawing.Size(120, 19)
        Me.ResetButton.TabIndex = 17
        Me.ResetButton.Text = "Reset Password"
        '
        'FileCButton
        '
        Me.FileCButton.BackColor = System.Drawing.Color.Red
        Me.FileCButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.FileCButton.CheckedState.Parent = Me.FileCButton
        Me.FileCButton.CustomImages.Parent = Me.FileCButton
        Me.FileCButton.FillColor = System.Drawing.Color.Transparent
        Me.FileCButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.FileCButton.ForeColor = System.Drawing.Color.White
        Me.FileCButton.HoverState.Parent = Me.FileCButton
        Me.FileCButton.Image = Global.Godown_Stock.My.Resources.Resources.icons8_document_in_folder_50
        Me.FileCButton.ImageAlign = System.Windows.Forms.HorizontalAlignment.Right
        Me.FileCButton.Location = New System.Drawing.Point(204, 232)
        Me.FileCButton.Name = "FileCButton"
        Me.FileCButton.PressedColor = System.Drawing.Color.White
        Me.FileCButton.ShadowDecoration.Parent = Me.FileCButton
        Me.FileCButton.Size = New System.Drawing.Size(39, 35)
        Me.FileCButton.TabIndex = 12
        Me.FileCButton.TabStop = False
        Me.FileCButton.Text = " "
        '
        'RestoreButton
        '
        Me.RestoreButton.CheckedState.Parent = Me.RestoreButton
        Me.RestoreButton.CustomImages.Parent = Me.RestoreButton
        Me.RestoreButton.FillColor = System.Drawing.Color.SteelBlue
        Me.RestoreButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.RestoreButton.ForeColor = System.Drawing.Color.White
        Me.RestoreButton.HoverState.Parent = Me.RestoreButton
        Me.RestoreButton.Image = Global.Godown_Stock.My.Resources.Resources.icons8_database_restore_50
        Me.RestoreButton.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.RestoreButton.Location = New System.Drawing.Point(130, 290)
        Me.RestoreButton.Name = "RestoreButton"
        Me.RestoreButton.PressedColor = System.Drawing.Color.White
        Me.RestoreButton.ShadowDecoration.Parent = Me.RestoreButton
        Me.RestoreButton.Size = New System.Drawing.Size(114, 35)
        Me.RestoreButton.TabIndex = 13
        Me.RestoreButton.TabStop = False
        Me.RestoreButton.Text = "Restore "
        Me.RestoreButton.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        '
        'UpdateButton
        '
        Me.UpdateButton.CheckedState.Parent = Me.UpdateButton
        Me.UpdateButton.CustomImages.Parent = Me.UpdateButton
        Me.UpdateButton.FillColor = System.Drawing.Color.SteelBlue
        Me.UpdateButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.UpdateButton.ForeColor = System.Drawing.Color.White
        Me.UpdateButton.HoverState.Parent = Me.UpdateButton
        Me.UpdateButton.Image = Global.Godown_Stock.My.Resources.Resources.icons8_database_administrator_50
        Me.UpdateButton.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.UpdateButton.Location = New System.Drawing.Point(10, 290)
        Me.UpdateButton.Name = "UpdateButton"
        Me.UpdateButton.PressedColor = System.Drawing.Color.White
        Me.UpdateButton.ShadowDecoration.Parent = Me.UpdateButton
        Me.UpdateButton.Size = New System.Drawing.Size(114, 35)
        Me.UpdateButton.TabIndex = 13
        Me.UpdateButton.TabStop = False
        Me.UpdateButton.Text = "Update DB"
        Me.UpdateButton.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        '
        'BackupButton
        '
        Me.BackupButton.CheckedState.Parent = Me.BackupButton
        Me.BackupButton.CustomImages.Parent = Me.BackupButton
        Me.BackupButton.FillColor = System.Drawing.Color.SteelBlue
        Me.BackupButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.BackupButton.ForeColor = System.Drawing.Color.White
        Me.BackupButton.HoverState.Parent = Me.BackupButton
        Me.BackupButton.Image = Global.Godown_Stock.My.Resources.Resources.icons8_backup_50
        Me.BackupButton.Location = New System.Drawing.Point(254, 116)
        Me.BackupButton.Name = "BackupButton"
        Me.BackupButton.ShadowDecoration.Parent = Me.BackupButton
        Me.BackupButton.Size = New System.Drawing.Size(233, 35)
        Me.BackupButton.TabIndex = 2
        Me.BackupButton.Text = "Backup / Check"
        '
        'DatabaseButton
        '
        Me.DatabaseButton.CheckedState.Parent = Me.DatabaseButton
        Me.DatabaseButton.CustomImages.Parent = Me.DatabaseButton
        Me.DatabaseButton.FillColor = System.Drawing.Color.Green
        Me.DatabaseButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.DatabaseButton.ForeColor = System.Drawing.Color.White
        Me.DatabaseButton.HoverState.Parent = Me.DatabaseButton
        Me.DatabaseButton.Image = Global.Godown_Stock.My.Resources.Resources.icons8_database_50
        Me.DatabaseButton.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left
        Me.DatabaseButton.Location = New System.Drawing.Point(161, 174)
        Me.DatabaseButton.Name = "DatabaseButton"
        Me.DatabaseButton.PressedColor = System.Drawing.Color.White
        Me.DatabaseButton.ShadowDecoration.Parent = Me.DatabaseButton
        Me.DatabaseButton.Size = New System.Drawing.Size(83, 35)
        Me.DatabaseButton.TabIndex = 10
        Me.DatabaseButton.TabStop = False
        Me.DatabaseButton.Text = "Create "
        Me.DatabaseButton.TextAlign = System.Windows.Forms.HorizontalAlignment.Left
        '
        'NextButton
        '
        Me.NextButton.CheckedState.Parent = Me.NextButton
        Me.NextButton.CustomImages.Parent = Me.NextButton
        Me.NextButton.FillColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(134, Byte), Integer), CType(CType(3, Byte), Integer))
        Me.NextButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.NextButton.ForeColor = System.Drawing.Color.White
        Me.NextButton.HoverState.Parent = Me.NextButton
        Me.NextButton.Image = Global.Godown_Stock.My.Resources.Resources.icons8_log_in_50
        Me.NextButton.Location = New System.Drawing.Point(253, 291)
        Me.NextButton.Name = "NextButton"
        Me.NextButton.PressedColor = System.Drawing.Color.White
        Me.NextButton.ShadowDecoration.Parent = Me.NextButton
        Me.NextButton.Size = New System.Drawing.Size(234, 35)
        Me.NextButton.TabIndex = 1
        Me.NextButton.Text = "Next"
        '
        'DBConnect
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(495, 336)
        Me.Controls.Add(Me.Guna2Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "DBConnect"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "DBConnect"
        Me.Guna2Panel2.ResumeLayout(False)
        Me.Guna2Panel2.PerformLayout()
        Me.Guna2Panel1.ResumeLayout(False)
        Me.Guna2Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Guna2Panel2 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2ControlBox1 As Guna.UI2.WinForms.Guna2ControlBox
    Friend WithEvents Guna2HtmlLabel1 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents NextButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents DatabaseButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents servertxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents dbtxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Psdtxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents UserText As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents dblocationtxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents BackupPathtxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents BackupButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents UpdateButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Label8 As Label
    Friend WithEvents RestoreTxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents RestoreButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents LogoPanelControl As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents BackupProgressBar As Guna.UI2.WinForms.Guna2ProgressBar
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents FileCButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents ResetButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents lblresult As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents LatestUpdateButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2Elipse As Guna.UI2.WinForms.Guna2Elipse

End Class

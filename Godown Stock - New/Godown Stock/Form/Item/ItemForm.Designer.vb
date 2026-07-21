<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ItemForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.HeaderPanel = New Guna.UI2.WinForms.Guna2ShadowPanel()
        Me.Refreshbtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Label = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Searchbtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Killbtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Savebtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Guna2DataGridView1 = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.Snotxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Itemtxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.OpeningText = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Unitbox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Modelbox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Brandbox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.grpbox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.KryptonListBox = New ComponentFactory.Krypton.Toolkit.KryptonListBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.MinStockTxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.TimerFocusDelay = New System.Windows.Forms.Timer(Me.components)
        Me.HeaderPanel.SuspendLayout()
        CType(Me.Guna2DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HeaderPanel
        '
        Me.HeaderPanel.BackColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.Controls.Add(Me.Refreshbtn)
        Me.HeaderPanel.Controls.Add(Me.Label)
        Me.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.HeaderPanel.FillColor = System.Drawing.Color.Empty
        Me.HeaderPanel.ForeColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.Location = New System.Drawing.Point(0, 0)
        Me.HeaderPanel.Name = "HeaderPanel"
        Me.HeaderPanel.ShadowColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.ShadowDepth = 0
        Me.HeaderPanel.Size = New System.Drawing.Size(1078, 42)
        Me.HeaderPanel.TabIndex = 85
        '
        'Refreshbtn
        '
        Me.Refreshbtn.CheckedState.Parent = Me.Refreshbtn
        Me.Refreshbtn.CustomImages.Parent = Me.Refreshbtn
        Me.Refreshbtn.FillColor = System.Drawing.Color.Empty
        Me.Refreshbtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Refreshbtn.ForeColor = System.Drawing.Color.White
        Me.Refreshbtn.HoverState.Parent = Me.Refreshbtn
        Me.Refreshbtn.Image = Global.Godown_Stock.My.Resources.Resources.Refresh
        Me.Refreshbtn.Location = New System.Drawing.Point(167, 9)
        Me.Refreshbtn.Name = "Refreshbtn"
        Me.Refreshbtn.PressedColor = System.Drawing.Color.DarkSlateBlue
        Me.Refreshbtn.ShadowDecoration.Parent = Me.Refreshbtn
        Me.Refreshbtn.Size = New System.Drawing.Size(48, 25)
        Me.Refreshbtn.TabIndex = 1
        Me.Refreshbtn.Text = " "
        '
        'Label
        '
        Me.Label.BackColor = System.Drawing.Color.Transparent
        Me.Label.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label.ForeColor = System.Drawing.Color.White
        Me.Label.Location = New System.Drawing.Point(12, 7)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(149, 27)
        Me.Label.TabIndex = 1
        Me.Label.Text = "Item Creation"
        '
        'Label6
        '
        Me.Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label6.Location = New System.Drawing.Point(12, 45)
        Me.Label6.MinimumSize = New System.Drawing.Size(260, 500)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(260, 549)
        Me.Label6.TabIndex = 103
        Me.Label6.Text = " "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(25, 108)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 13)
        Me.Label1.TabIndex = 86
        Me.Label1.Text = "Item Name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(25, 169)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(30, 13)
        Me.Label2.TabIndex = 87
        Me.Label2.Text = "Unit"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(25, 230)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 13)
        Me.Label3.TabIndex = 88
        Me.Label3.Text = "Item Group"
        '
        'Searchbtn
        '
        Me.Searchbtn.CheckedState.Parent = Me.Searchbtn
        Me.Searchbtn.CustomImages.Parent = Me.Searchbtn
        Me.Searchbtn.FillColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(125, Byte), Integer), CType(CType(205, Byte), Integer))
        Me.Searchbtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Searchbtn.ForeColor = System.Drawing.Color.White
        Me.Searchbtn.HoverState.Parent = Me.Searchbtn
        Me.Searchbtn.Location = New System.Drawing.Point(210, 68)
        Me.Searchbtn.Name = "Searchbtn"
        Me.Searchbtn.ShadowDecoration.Parent = Me.Searchbtn
        Me.Searchbtn.Size = New System.Drawing.Size(49, 36)
        Me.Searchbtn.TabIndex = 105
        Me.Searchbtn.Text = "Search"
        '
        'Killbtn
        '
        Me.Killbtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Killbtn.CheckedState.Parent = Me.Killbtn
        Me.Killbtn.CustomImages.Parent = Me.Killbtn
        Me.Killbtn.FillColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(28, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.Killbtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Killbtn.ForeColor = System.Drawing.Color.White
        Me.Killbtn.HoverState.Parent = Me.Killbtn
        Me.Killbtn.Location = New System.Drawing.Point(25, 554)
        Me.Killbtn.Name = "Killbtn"
        Me.Killbtn.ShadowDecoration.Parent = Me.Killbtn
        Me.Killbtn.Size = New System.Drawing.Size(91, 29)
        Me.Killbtn.TabIndex = 106
        Me.Killbtn.TabStop = False
        Me.Killbtn.Text = "Inactive"
        '
        'Savebtn
        '
        Me.Savebtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Savebtn.CheckedState.Parent = Me.Savebtn
        Me.Savebtn.CustomImages.Parent = Me.Savebtn
        Me.Savebtn.FillColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(134, Byte), Integer), CType(CType(3, Byte), Integer))
        Me.Savebtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Savebtn.ForeColor = System.Drawing.Color.White
        Me.Savebtn.HoverState.Parent = Me.Savebtn
        Me.Savebtn.Location = New System.Drawing.Point(167, 554)
        Me.Savebtn.Name = "Savebtn"
        Me.Savebtn.ShadowDecoration.Parent = Me.Savebtn
        Me.Savebtn.Size = New System.Drawing.Size(91, 29)
        Me.Savebtn.TabIndex = 8
        Me.Savebtn.Text = "Save"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.Black
        Me.Label7.Location = New System.Drawing.Point(25, 291)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(71, 13)
        Me.Label7.TabIndex = 110
        Me.Label7.Text = "Item Brand"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Black
        Me.Label8.Location = New System.Drawing.Point(25, 352)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(72, 13)
        Me.Label8.TabIndex = 112
        Me.Label8.Text = "Item Model"
        '
        'Guna2DataGridView1
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Guna2DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Guna2DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.Guna2DataGridView1.BackgroundColor = System.Drawing.Color.Gainsboro
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
        Me.Guna2DataGridView1.ColumnHeadersHeight = 21
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Guna2DataGridView1.DefaultCellStyle = DataGridViewCellStyle3
        Me.Guna2DataGridView1.EnableHeadersVisualStyles = False
        Me.Guna2DataGridView1.GridColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2DataGridView1.Location = New System.Drawing.Point(276, 45)
        Me.Guna2DataGridView1.Name = "Guna2DataGridView1"
        Me.Guna2DataGridView1.ReadOnly = True
        Me.Guna2DataGridView1.RowHeadersVisible = False
        Me.Guna2DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Guna2DataGridView1.Size = New System.Drawing.Size(796, 549)
        Me.Guna2DataGridView1.TabIndex = 110
        Me.Guna2DataGridView1.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.[Default]
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.Font = Nothing
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty
        Me.Guna2DataGridView1.ThemeStyle.BackColor = System.Drawing.Color.Gainsboro
        Me.Guna2DataGridView1.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2DataGridView1.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2DataGridView1.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        Me.Guna2DataGridView1.ThemeStyle.HeaderStyle.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        Me.Guna2DataGridView1.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing
        Me.Guna2DataGridView1.ThemeStyle.HeaderStyle.Height = 21
        Me.Guna2DataGridView1.ThemeStyle.ReadOnly = True
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.Height = 22
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2DataGridView1.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        '
        'Snotxt
        '
        Me.Snotxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Snotxt.DefaultText = ""
        Me.Snotxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Snotxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Snotxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Snotxt.DisabledState.Parent = Me.Snotxt
        Me.Snotxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Snotxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Snotxt.FocusedState.Parent = Me.Snotxt
        Me.Snotxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Snotxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Snotxt.HoverState.Parent = Me.Snotxt
        Me.Snotxt.Location = New System.Drawing.Point(21, 68)
        Me.Snotxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Snotxt.Name = "Snotxt"
        Me.Snotxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Snotxt.PlaceholderText = ""
        Me.Snotxt.SelectedText = ""
        Me.Snotxt.ShadowDecoration.Parent = Me.Snotxt
        Me.Snotxt.Size = New System.Drawing.Size(183, 36)
        Me.Snotxt.TabIndex = 0
        '
        'Itemtxt
        '
        Me.Itemtxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Itemtxt.DefaultText = ""
        Me.Itemtxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Itemtxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Itemtxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Itemtxt.DisabledState.Parent = Me.Itemtxt
        Me.Itemtxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Itemtxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Itemtxt.FocusedState.Parent = Me.Itemtxt
        Me.Itemtxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Itemtxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Itemtxt.HoverState.Parent = Me.Itemtxt
        Me.Itemtxt.Location = New System.Drawing.Point(25, 127)
        Me.Itemtxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Itemtxt.Name = "Itemtxt"
        Me.Itemtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Itemtxt.PlaceholderText = ""
        Me.Itemtxt.SelectedText = ""
        Me.Itemtxt.ShadowDecoration.Parent = Me.Itemtxt
        Me.Itemtxt.Size = New System.Drawing.Size(234, 36)
        Me.Itemtxt.TabIndex = 1
        '
        'OpeningText
        '
        Me.OpeningText.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.OpeningText.DefaultText = ""
        Me.OpeningText.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.OpeningText.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.OpeningText.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.OpeningText.DisabledState.Parent = Me.OpeningText
        Me.OpeningText.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.OpeningText.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.OpeningText.FocusedState.Parent = Me.OpeningText
        Me.OpeningText.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.OpeningText.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.OpeningText.HoverState.Parent = Me.OpeningText
        Me.OpeningText.Location = New System.Drawing.Point(25, 493)
        Me.OpeningText.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.OpeningText.Name = "OpeningText"
        Me.OpeningText.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.OpeningText.PlaceholderText = ""
        Me.OpeningText.SelectedText = ""
        Me.OpeningText.ShadowDecoration.Parent = Me.OpeningText
        Me.OpeningText.Size = New System.Drawing.Size(234, 36)
        Me.OpeningText.TabIndex = 7
        Me.OpeningText.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.Black
        Me.Label9.Location = New System.Drawing.Point(25, 474)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(76, 13)
        Me.Label9.TabIndex = 122
        Me.Label9.Text = "Opening Qty"
        '
        'Unitbox
        '
        Me.Unitbox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Unitbox.DefaultText = ""
        Me.Unitbox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Unitbox.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Unitbox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Unitbox.DisabledState.Parent = Me.Unitbox
        Me.Unitbox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Unitbox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Unitbox.FocusedState.Parent = Me.Unitbox
        Me.Unitbox.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Unitbox.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Unitbox.HoverState.Parent = Me.Unitbox
        Me.Unitbox.Location = New System.Drawing.Point(25, 188)
        Me.Unitbox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Unitbox.Name = "Unitbox"
        Me.Unitbox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Unitbox.PlaceholderText = ""
        Me.Unitbox.SelectedText = ""
        Me.Unitbox.ShadowDecoration.Parent = Me.Unitbox
        Me.Unitbox.Size = New System.Drawing.Size(234, 36)
        Me.Unitbox.TabIndex = 2
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
        Me.Modelbox.Location = New System.Drawing.Point(25, 371)
        Me.Modelbox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Modelbox.Name = "Modelbox"
        Me.Modelbox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Modelbox.PlaceholderText = ""
        Me.Modelbox.SelectedText = ""
        Me.Modelbox.ShadowDecoration.Parent = Me.Modelbox
        Me.Modelbox.Size = New System.Drawing.Size(234, 36)
        Me.Modelbox.TabIndex = 5
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
        Me.Brandbox.Location = New System.Drawing.Point(25, 310)
        Me.Brandbox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Brandbox.Name = "Brandbox"
        Me.Brandbox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Brandbox.PlaceholderText = ""
        Me.Brandbox.SelectedText = ""
        Me.Brandbox.ShadowDecoration.Parent = Me.Brandbox
        Me.Brandbox.Size = New System.Drawing.Size(234, 36)
        Me.Brandbox.TabIndex = 4
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
        Me.grpbox.Location = New System.Drawing.Point(25, 249)
        Me.grpbox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.grpbox.Name = "grpbox"
        Me.grpbox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.grpbox.PlaceholderText = ""
        Me.grpbox.SelectedText = ""
        Me.grpbox.ShadowDecoration.Parent = Me.grpbox
        Me.grpbox.Size = New System.Drawing.Size(234, 36)
        Me.grpbox.TabIndex = 3
        '
        'KryptonListBox
        '
        Me.KryptonListBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.KryptonListBox.Location = New System.Drawing.Point(276, 45)
        Me.KryptonListBox.Name = "KryptonListBox"
        Me.KryptonListBox.Size = New System.Drawing.Size(238, 162)
        Me.KryptonListBox.TabIndex = 126
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.Black
        Me.Label5.Location = New System.Drawing.Point(25, 413)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(94, 13)
        Me.Label5.TabIndex = 128
        Me.Label5.Text = "Minimum Stock"
        '
        'MinStockTxt
        '
        Me.MinStockTxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.MinStockTxt.DefaultText = ""
        Me.MinStockTxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.MinStockTxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.MinStockTxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.MinStockTxt.DisabledState.Parent = Me.MinStockTxt
        Me.MinStockTxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.MinStockTxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.MinStockTxt.FocusedState.Parent = Me.MinStockTxt
        Me.MinStockTxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.MinStockTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.MinStockTxt.HoverState.Parent = Me.MinStockTxt
        Me.MinStockTxt.Location = New System.Drawing.Point(25, 432)
        Me.MinStockTxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MinStockTxt.Name = "MinStockTxt"
        Me.MinStockTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.MinStockTxt.PlaceholderText = ""
        Me.MinStockTxt.SelectedText = ""
        Me.MinStockTxt.ShadowDecoration.Parent = Me.MinStockTxt
        Me.MinStockTxt.Size = New System.Drawing.Size(234, 36)
        Me.MinStockTxt.TabIndex = 6
        Me.MinStockTxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'TimerFocusDelay
        '
        Me.TimerFocusDelay.Interval = 200
        '
        'ItemForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1078, 612)
        Me.Controls.Add(Me.KryptonListBox)
        Me.Controls.Add(Me.MinStockTxt)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.grpbox)
        Me.Controls.Add(Me.Brandbox)
        Me.Controls.Add(Me.Modelbox)
        Me.Controls.Add(Me.Unitbox)
        Me.Controls.Add(Me.OpeningText)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Itemtxt)
        Me.Controls.Add(Me.Snotxt)
        Me.Controls.Add(Me.HeaderPanel)
        Me.Controls.Add(Me.Guna2DataGridView1)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Savebtn)
        Me.Controls.Add(Me.Killbtn)
        Me.Controls.Add(Me.Searchbtn)
        Me.Controls.Add(Me.Label6)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "ItemForm"
        Me.Text = "ItemForm"
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        CType(Me.Guna2DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents HeaderPanel As Guna.UI2.WinForms.Guna2ShadowPanel
    Friend WithEvents Label As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Savebtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Killbtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Searchbtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Guna2DataGridView1 As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents Refreshbtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Snotxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Itemtxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents OpeningText As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Unitbox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Modelbox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Brandbox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents grpbox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents KryptonListBox As ComponentFactory.Krypton.Toolkit.KryptonListBox
    Friend WithEvents Label5 As Label
    Friend WithEvents MinStockTxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents TimerFocusDelay As Timer
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LedgerForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.HeaderPanel = New Guna.UI2.WinForms.Guna2ShadowPanel()
        Me.Refreshbtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Label = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2DataGridView1 = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Savebtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Killbtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Searchbtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Searchtxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Partytxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Mobiletxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.underbox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.ListBox1 = New ComponentFactory.Krypton.Toolkit.KryptonListBox()
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
        Me.HeaderPanel.TabIndex = 114
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
        Me.Refreshbtn.Location = New System.Drawing.Point(184, 9)
        Me.Refreshbtn.Name = "Refreshbtn"
        Me.Refreshbtn.PressedColor = System.Drawing.Color.DarkSlateBlue
        Me.Refreshbtn.ShadowDecoration.Parent = Me.Refreshbtn
        Me.Refreshbtn.Size = New System.Drawing.Size(48, 25)
        Me.Refreshbtn.TabIndex = 2
        Me.Refreshbtn.Text = " "
        '
        'Label
        '
        Me.Label.BackColor = System.Drawing.Color.Transparent
        Me.Label.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label.ForeColor = System.Drawing.Color.White
        Me.Label.Location = New System.Drawing.Point(12, 7)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(169, 27)
        Me.Label.TabIndex = 1
        Me.Label.Text = "Ledger Creation"
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
        Me.Guna2DataGridView1.Location = New System.Drawing.Point(298, 45)
        Me.Guna2DataGridView1.Name = "Guna2DataGridView1"
        Me.Guna2DataGridView1.ReadOnly = True
        Me.Guna2DataGridView1.RowHeadersVisible = False
        Me.Guna2DataGridView1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.Guna2DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Guna2DataGridView1.Size = New System.Drawing.Size(773, 549)
        Me.Guna2DataGridView1.TabIndex = 132
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
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(36, 176)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(83, 13)
        Me.Label2.TabIndex = 116
        Me.Label2.Text = "Ledger Under"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(33, 117)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(81, 13)
        Me.Label1.TabIndex = 115
        Me.Label1.Text = "Ledger Name"
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
        Me.Savebtn.Location = New System.Drawing.Point(178, 550)
        Me.Savebtn.Name = "Savebtn"
        Me.Savebtn.ShadowDecoration.Parent = Me.Savebtn
        Me.Savebtn.Size = New System.Drawing.Size(91, 29)
        Me.Savebtn.TabIndex = 4
        Me.Savebtn.Text = "Save"
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
        Me.Killbtn.Location = New System.Drawing.Point(36, 550)
        Me.Killbtn.Name = "Killbtn"
        Me.Killbtn.ShadowDecoration.Parent = Me.Killbtn
        Me.Killbtn.Size = New System.Drawing.Size(91, 29)
        Me.Killbtn.TabIndex = 7
        Me.Killbtn.Text = "Inactive"
        '
        'Searchbtn
        '
        Me.Searchbtn.CheckedState.Parent = Me.Searchbtn
        Me.Searchbtn.CustomImages.Parent = Me.Searchbtn
        Me.Searchbtn.FillColor = System.Drawing.Color.RoyalBlue
        Me.Searchbtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Searchbtn.ForeColor = System.Drawing.Color.White
        Me.Searchbtn.HoverState.Parent = Me.Searchbtn
        Me.Searchbtn.Location = New System.Drawing.Point(226, 77)
        Me.Searchbtn.Name = "Searchbtn"
        Me.Searchbtn.ShadowDecoration.Parent = Me.Searchbtn
        Me.Searchbtn.Size = New System.Drawing.Size(45, 36)
        Me.Searchbtn.TabIndex = 6
        Me.Searchbtn.Text = "Search"
        '
        'Label6
        '
        Me.Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label6.Location = New System.Drawing.Point(12, 45)
        Me.Label6.MinimumSize = New System.Drawing.Size(280, 500)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(280, 549)
        Me.Label6.TabIndex = 126
        Me.Label6.Text = " "
        '
        'Searchtxt
        '
        Me.Searchtxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Searchtxt.DefaultText = ""
        Me.Searchtxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Searchtxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Searchtxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Searchtxt.DisabledState.Parent = Me.Searchtxt
        Me.Searchtxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Searchtxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Searchtxt.FocusedState.Parent = Me.Searchtxt
        Me.Searchtxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Searchtxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Searchtxt.HoverState.Parent = Me.Searchtxt
        Me.Searchtxt.Location = New System.Drawing.Point(32, 77)
        Me.Searchtxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Searchtxt.Name = "Searchtxt"
        Me.Searchtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Searchtxt.PlaceholderText = ""
        Me.Searchtxt.SelectedText = ""
        Me.Searchtxt.ShadowDecoration.Parent = Me.Searchtxt
        Me.Searchtxt.Size = New System.Drawing.Size(188, 36)
        Me.Searchtxt.TabIndex = 5
        '
        'Partytxt
        '
        Me.Partytxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Partytxt.DefaultText = ""
        Me.Partytxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Partytxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Partytxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Partytxt.DisabledState.Parent = Me.Partytxt
        Me.Partytxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Partytxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Partytxt.FocusedState.Parent = Me.Partytxt
        Me.Partytxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Partytxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Partytxt.HoverState.Parent = Me.Partytxt
        Me.Partytxt.Location = New System.Drawing.Point(34, 135)
        Me.Partytxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Partytxt.Name = "Partytxt"
        Me.Partytxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Partytxt.PlaceholderText = ""
        Me.Partytxt.SelectedText = ""
        Me.Partytxt.ShadowDecoration.Parent = Me.Partytxt
        Me.Partytxt.Size = New System.Drawing.Size(235, 36)
        Me.Partytxt.TabIndex = 1
        '
        'Mobiletxt
        '
        Me.Mobiletxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Mobiletxt.DefaultText = ""
        Me.Mobiletxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Mobiletxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Mobiletxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Mobiletxt.DisabledState.Parent = Me.Mobiletxt
        Me.Mobiletxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Mobiletxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Mobiletxt.FocusedState.Parent = Me.Mobiletxt
        Me.Mobiletxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Mobiletxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Mobiletxt.HoverState.Parent = Me.Mobiletxt
        Me.Mobiletxt.Location = New System.Drawing.Point(36, 251)
        Me.Mobiletxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Mobiletxt.Name = "Mobiletxt"
        Me.Mobiletxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Mobiletxt.PlaceholderText = ""
        Me.Mobiletxt.SelectedText = ""
        Me.Mobiletxt.ShadowDecoration.Parent = Me.Mobiletxt
        Me.Mobiletxt.Size = New System.Drawing.Size(235, 36)
        Me.Mobiletxt.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(35, 233)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 13)
        Me.Label3.TabIndex = 136
        Me.Label3.Text = "Ledger Mobile"
        '
        'underbox
        '
        Me.underbox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.underbox.DefaultText = ""
        Me.underbox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.underbox.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.underbox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.underbox.DisabledState.Parent = Me.underbox
        Me.underbox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.underbox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.underbox.FocusedState.Parent = Me.underbox
        Me.underbox.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.underbox.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.underbox.HoverState.Parent = Me.underbox
        Me.underbox.Location = New System.Drawing.Point(34, 194)
        Me.underbox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.underbox.Name = "underbox"
        Me.underbox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.underbox.PlaceholderText = ""
        Me.underbox.SelectedText = ""
        Me.underbox.ShadowDecoration.Parent = Me.underbox
        Me.underbox.Size = New System.Drawing.Size(235, 36)
        Me.underbox.TabIndex = 2
        '
        'ListBox1
        '
        Me.ListBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListBox1.Location = New System.Drawing.Point(298, 45)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(245, 154)
        Me.ListBox1.TabIndex = 140
        '
        'TimerFocusDelay
        '
        Me.TimerFocusDelay.Interval = 200
        '
        'LedgerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1078, 612)
        Me.Controls.Add(Me.ListBox1)
        Me.Controls.Add(Me.underbox)
        Me.Controls.Add(Me.Mobiletxt)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Partytxt)
        Me.Controls.Add(Me.Searchtxt)
        Me.Controls.Add(Me.HeaderPanel)
        Me.Controls.Add(Me.Guna2DataGridView1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Savebtn)
        Me.Controls.Add(Me.Killbtn)
        Me.Controls.Add(Me.Searchbtn)
        Me.Controls.Add(Me.Label6)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "LedgerForm"
        Me.Text = "LedgerForm"
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        CType(Me.Guna2DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents HeaderPanel As Guna.UI2.WinForms.Guna2ShadowPanel
    Friend WithEvents Label As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2DataGridView1 As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Savebtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Killbtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Searchbtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Label6 As Label
    Friend WithEvents Searchtxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Partytxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Mobiletxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Refreshbtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents underbox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents ListBox1 As ComponentFactory.Krypton.Toolkit.KryptonListBox
    Friend WithEvents TimerFocusDelay As Timer
End Class

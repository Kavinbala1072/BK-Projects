<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class JCDisplay
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
        Me.CancelBttn = New Guna.UI2.WinForms.Guna2Button()
        Me.ToDateTextBox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.FromDateTextBox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.RefreshButton = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.DateTxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.AddonsButton = New Guna.UI2.WinForms.Guna2Button()
        Me.PrintButton = New ComponentFactory.Krypton.Toolkit.KryptonDropButton()
        Me.PrintMenu = New ComponentFactory.Krypton.Toolkit.KryptonContextMenu()
        Me.KryptonContextMenuItems1 = New ComponentFactory.Krypton.Toolkit.KryptonContextMenuItems()
        Me.KryptonContextMenuItem1 = New ComponentFactory.Krypton.Toolkit.KryptonContextMenuItem()
        Me.KryptonContextMenuItem2 = New ComponentFactory.Krypton.Toolkit.KryptonContextMenuItem()
        Me.UpdateButton = New Guna.UI2.WinForms.Guna2Button()
        Me.UpdateTextBox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Guna2DataGridView1 = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.HeaderPanel.SuspendLayout()
        Me.Guna2Panel1.SuspendLayout()
        CType(Me.Guna2DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.HeaderPanel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.HeaderPanel.Name = "HeaderPanel"
        Me.HeaderPanel.ShadowColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.ShadowDepth = 0
        Me.HeaderPanel.Size = New System.Drawing.Size(1251, 52)
        Me.HeaderPanel.TabIndex = 87
        '
        'Label
        '
        Me.Label.BackColor = System.Drawing.Color.Transparent
        Me.Label.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label.ForeColor = System.Drawing.Color.White
        Me.Label.Location = New System.Drawing.Point(16, 9)
        Me.Label.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(116, 33)
        Me.Label.TabIndex = 1
        Me.Label.Text = "Job Card"
        '
        'CancelBttn
        '
        Me.CancelBttn.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.CancelBttn.CheckedState.Parent = Me.CancelBttn
        Me.CancelBttn.CustomImages.Parent = Me.CancelBttn
        Me.CancelBttn.FillColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(28, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.CancelBttn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.CancelBttn.ForeColor = System.Drawing.Color.White
        Me.CancelBttn.HoverState.Parent = Me.CancelBttn
        Me.CancelBttn.Location = New System.Drawing.Point(1143, 6)
        Me.CancelBttn.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.CancelBttn.Name = "CancelBttn"
        Me.CancelBttn.ShadowDecoration.Parent = Me.CancelBttn
        Me.CancelBttn.Size = New System.Drawing.Size(96, 39)
        Me.CancelBttn.TabIndex = 8
        Me.CancelBttn.TabStop = False
        Me.CancelBttn.Text = "Cancel"
        '
        'ToDateTextBox
        '
        Me.ToDateTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left
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
        Me.ToDateTextBox.Location = New System.Drawing.Point(143, 4)
        Me.ToDateTextBox.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.ToDateTextBox.Name = "ToDateTextBox"
        Me.ToDateTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.ToDateTextBox.PlaceholderText = ""
        Me.ToDateTextBox.SelectedText = ""
        Me.ToDateTextBox.ShadowDecoration.Parent = Me.ToDateTextBox
        Me.ToDateTextBox.Size = New System.Drawing.Size(133, 44)
        Me.ToDateTextBox.TabIndex = 1
        '
        'FromDateTextBox
        '
        Me.FromDateTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left
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
        Me.FromDateTextBox.Location = New System.Drawing.Point(4, 4)
        Me.FromDateTextBox.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.FromDateTextBox.Name = "FromDateTextBox"
        Me.FromDateTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.FromDateTextBox.PlaceholderText = ""
        Me.FromDateTextBox.SelectedText = ""
        Me.FromDateTextBox.ShadowDecoration.Parent = Me.FromDateTextBox
        Me.FromDateTextBox.Size = New System.Drawing.Size(133, 44)
        Me.FromDateTextBox.TabIndex = 0
        '
        'RefreshButton
        '
        Me.RefreshButton.BackColor = System.Drawing.Color.RoyalBlue
        Me.RefreshButton.CheckedState.Parent = Me.RefreshButton
        Me.RefreshButton.CustomImages.Parent = Me.RefreshButton
        Me.RefreshButton.FillColor = System.Drawing.Color.RoyalBlue
        Me.RefreshButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.RefreshButton.ForeColor = System.Drawing.Color.White
        Me.RefreshButton.HoverState.Parent = Me.RefreshButton
        Me.RefreshButton.Image = Global.Godown_Stock.My.Resources.Resources.Refresh
        Me.RefreshButton.Location = New System.Drawing.Point(283, 6)
        Me.RefreshButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.ShadowDecoration.Parent = Me.RefreshButton
        Me.RefreshButton.Size = New System.Drawing.Size(101, 39)
        Me.RefreshButton.TabIndex = 2
        Me.RefreshButton.Text = "Refresh"
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.Controls.Add(Me.DateTxt)
        Me.Guna2Panel1.Controls.Add(Me.AddonsButton)
        Me.Guna2Panel1.Controls.Add(Me.PrintButton)
        Me.Guna2Panel1.Controls.Add(Me.UpdateButton)
        Me.Guna2Panel1.Controls.Add(Me.UpdateTextBox)
        Me.Guna2Panel1.Controls.Add(Me.CancelBttn)
        Me.Guna2Panel1.Controls.Add(Me.RefreshButton)
        Me.Guna2Panel1.Controls.Add(Me.FromDateTextBox)
        Me.Guna2Panel1.Controls.Add(Me.ToDateTextBox)
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Guna2Panel1.Location = New System.Drawing.Point(0, 52)
        Me.Guna2Panel1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.ShadowDecoration.Parent = Me.Guna2Panel1
        Me.Guna2Panel1.Size = New System.Drawing.Size(1251, 53)
        Me.Guna2Panel1.TabIndex = 186
        '
        'DateTxt
        '
        Me.DateTxt.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DateTxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.DateTxt.DefaultText = ""
        Me.DateTxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.DateTxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.DateTxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.DateTxt.DisabledState.Parent = Me.DateTxt
        Me.DateTxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.DateTxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.DateTxt.FocusedState.Parent = Me.DateTxt
        Me.DateTxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.DateTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.DateTxt.HoverState.Parent = Me.DateTxt
        Me.DateTxt.Location = New System.Drawing.Point(564, 5)
        Me.DateTxt.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.DateTxt.Name = "DateTxt"
        Me.DateTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.DateTxt.PlaceholderText = ""
        Me.DateTxt.SelectedText = ""
        Me.DateTxt.ShadowDecoration.Parent = Me.DateTxt
        Me.DateTxt.Size = New System.Drawing.Size(123, 44)
        Me.DateTxt.TabIndex = 3
        '
        'AddonsButton
        '
        Me.AddonsButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddonsButton.BackColor = System.Drawing.Color.RoyalBlue
        Me.AddonsButton.CheckedState.Parent = Me.AddonsButton
        Me.AddonsButton.CustomImages.Parent = Me.AddonsButton
        Me.AddonsButton.FillColor = System.Drawing.Color.RoyalBlue
        Me.AddonsButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.AddonsButton.ForeColor = System.Drawing.Color.White
        Me.AddonsButton.HoverState.Parent = Me.AddonsButton
        Me.AddonsButton.Location = New System.Drawing.Point(929, 6)
        Me.AddonsButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.AddonsButton.Name = "AddonsButton"
        Me.AddonsButton.ShadowDecoration.Parent = Me.AddonsButton
        Me.AddonsButton.Size = New System.Drawing.Size(101, 39)
        Me.AddonsButton.TabIndex = 6
        Me.AddonsButton.Text = "Addons"
        '
        'PrintButton
        '
        Me.PrintButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PrintButton.ButtonStyle = ComponentFactory.Krypton.Toolkit.ButtonStyle.Custom1
        Me.PrintButton.Cursor = System.Windows.Forms.Cursors.Default
        Me.PrintButton.KryptonContextMenu = Me.PrintMenu
        Me.PrintButton.Location = New System.Drawing.Point(1039, 6)
        Me.PrintButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PrintButton.Name = "PrintButton"
        Me.PrintButton.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalOffice2003
        Me.PrintButton.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.PrintButton.Size = New System.Drawing.Size(96, 39)
        Me.PrintButton.TabIndex = 7
        Me.PrintButton.TabStop = False
        Me.PrintButton.Values.Image = Global.Godown_Stock.My.Resources.Resources.printer_9654640
        Me.PrintButton.Values.Text = "Print"
        '
        'PrintMenu
        '
        Me.PrintMenu.Items.AddRange(New ComponentFactory.Krypton.Toolkit.KryptonContextMenuItemBase() {Me.KryptonContextMenuItems1})
        Me.PrintMenu.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Custom
        '
        'KryptonContextMenuItems1
        '
        Me.KryptonContextMenuItems1.Items.AddRange(New ComponentFactory.Krypton.Toolkit.KryptonContextMenuItemBase() {Me.KryptonContextMenuItem1, Me.KryptonContextMenuItem2})
        '
        'KryptonContextMenuItem1
        '
        Me.KryptonContextMenuItem1.ImageTransparentColor = System.Drawing.Color.Transparent
        Me.KryptonContextMenuItem1.Text = "Multi Print"
        '
        'KryptonContextMenuItem2
        '
        Me.KryptonContextMenuItem2.Text = "All Print"
        '
        'UpdateButton
        '
        Me.UpdateButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UpdateButton.BackColor = System.Drawing.Color.RoyalBlue
        Me.UpdateButton.CheckedState.Parent = Me.UpdateButton
        Me.UpdateButton.CustomImages.Parent = Me.UpdateButton
        Me.UpdateButton.FillColor = System.Drawing.Color.RoyalBlue
        Me.UpdateButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.UpdateButton.ForeColor = System.Drawing.Color.White
        Me.UpdateButton.HoverState.Parent = Me.UpdateButton
        Me.UpdateButton.Location = New System.Drawing.Point(825, 6)
        Me.UpdateButton.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.UpdateButton.Name = "UpdateButton"
        Me.UpdateButton.ShadowDecoration.Parent = Me.UpdateButton
        Me.UpdateButton.Size = New System.Drawing.Size(96, 39)
        Me.UpdateButton.TabIndex = 5
        Me.UpdateButton.Tag = "Update Fininshing Note"
        Me.UpdateButton.Text = "Update"
        '
        'UpdateTextBox
        '
        Me.UpdateTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UpdateTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.UpdateTextBox.DefaultText = ""
        Me.UpdateTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.UpdateTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.UpdateTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.UpdateTextBox.DisabledState.Parent = Me.UpdateTextBox
        Me.UpdateTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.UpdateTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.UpdateTextBox.FocusedState.Parent = Me.UpdateTextBox
        Me.UpdateTextBox.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.UpdateTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.UpdateTextBox.HoverState.Parent = Me.UpdateTextBox
        Me.UpdateTextBox.Location = New System.Drawing.Point(695, 4)
        Me.UpdateTextBox.Margin = New System.Windows.Forms.Padding(5, 5, 5, 5)
        Me.UpdateTextBox.Name = "UpdateTextBox"
        Me.UpdateTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.UpdateTextBox.PlaceholderText = ""
        Me.UpdateTextBox.SelectedText = ""
        Me.UpdateTextBox.ShadowDecoration.Parent = Me.UpdateTextBox
        Me.UpdateTextBox.Size = New System.Drawing.Size(123, 44)
        Me.UpdateTextBox.TabIndex = 4
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
        Me.Guna2DataGridView1.Location = New System.Drawing.Point(0, 105)
        Me.Guna2DataGridView1.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Guna2DataGridView1.Name = "Guna2DataGridView1"
        Me.Guna2DataGridView1.RowHeadersVisible = False
        Me.Guna2DataGridView1.RowHeadersWidth = 51
        Me.Guna2DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Guna2DataGridView1.Size = New System.Drawing.Size(1251, 486)
        Me.Guna2DataGridView1.TabIndex = 187
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
        'JCDisplay
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1251, 591)
        Me.Controls.Add(Me.Guna2DataGridView1)
        Me.Controls.Add(Me.Guna2Panel1)
        Me.Controls.Add(Me.HeaderPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "JCDisplay"
        Me.Text = "JCDisplay"
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        Me.Guna2Panel1.ResumeLayout(False)
        CType(Me.Guna2DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents HeaderPanel As Guna.UI2.WinForms.Guna2ShadowPanel
    Friend WithEvents Label As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents RefreshButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents CancelBttn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents ToDateTextBox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents FromDateTextBox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2DataGridView1 As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents UpdateButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents UpdateTextBox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents PrintButton As ComponentFactory.Krypton.Toolkit.KryptonDropButton
    Friend WithEvents PrintMenu As ComponentFactory.Krypton.Toolkit.KryptonContextMenu
    Friend WithEvents KryptonContextMenuItems1 As ComponentFactory.Krypton.Toolkit.KryptonContextMenuItems
    Friend WithEvents KryptonContextMenuItem1 As ComponentFactory.Krypton.Toolkit.KryptonContextMenuItem
    Friend WithEvents AddonsButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents DateTxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents KryptonContextMenuItem2 As ComponentFactory.Krypton.Toolkit.KryptonContextMenuItem
End Class

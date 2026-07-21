<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Printing
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
        Me.Label = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Savebtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Guna2DataGridView1 = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.PCancelBtn = New Guna.UI2.WinForms.Guna2Button()
        Me.RefreshButton = New Guna.UI2.WinForms.Guna2Button()
        Me.FromDateTextBox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.ToDateTextBox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.PrintingDate = New Guna.UI2.WinForms.Guna2TextBox()
        Me.BillNoTxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.PartynameTxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Papertxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.QtyTxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Colourtxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Detailstxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.PMethodTxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Status = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.PrintButton = New Guna.UI2.WinForms.Guna2Button()
        Me.PrintingTxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.PMachineTxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.KryptonListBox = New ComponentFactory.Krypton.Toolkit.KryptonListBox()
        Me.CompletedButton = New Guna.UI2.WinForms.Guna2Button()
        Me.DateTxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.UpdateTextBox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.PrintingItemtxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Weighttxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Brandtxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.TimerFocusDelay = New System.Windows.Forms.Timer(Me.components)
        Me.HeaderPanel.SuspendLayout()
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
        Me.HeaderPanel.Name = "HeaderPanel"
        Me.HeaderPanel.ShadowColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.ShadowDepth = 0
        Me.HeaderPanel.Size = New System.Drawing.Size(1096, 42)
        Me.HeaderPanel.TabIndex = 87
        '
        'Label
        '
        Me.Label.BackColor = System.Drawing.Color.Transparent
        Me.Label.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label.ForeColor = System.Drawing.Color.White
        Me.Label.Location = New System.Drawing.Point(12, 7)
        Me.Label.Name = "Label"
        Me.Label.Size = New System.Drawing.Size(86, 27)
        Me.Label.TabIndex = 1
        Me.Label.Text = "Printing"
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
        Me.Savebtn.Location = New System.Drawing.Point(239, 586)
        Me.Savebtn.Name = "Savebtn"
        Me.Savebtn.ShadowDecoration.Parent = Me.Savebtn
        Me.Savebtn.Size = New System.Drawing.Size(103, 37)
        Me.Savebtn.TabIndex = 16
        Me.Savebtn.Text = "Save"
        '
        'Label6
        '
        Me.Label6.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label6.BackColor = System.Drawing.Color.Transparent
        Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label6.Location = New System.Drawing.Point(7, 48)
        Me.Label6.MinimumSize = New System.Drawing.Size(260, 450)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(348, 585)
        Me.Label6.TabIndex = 132
        Me.Label6.Text = " "
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
        Me.Guna2DataGridView1.Location = New System.Drawing.Point(361, 97)
        Me.Guna2DataGridView1.Name = "Guna2DataGridView1"
        Me.Guna2DataGridView1.ReadOnly = True
        Me.Guna2DataGridView1.RowHeadersVisible = False
        Me.Guna2DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Guna2DataGridView1.Size = New System.Drawing.Size(726, 536)
        Me.Guna2DataGridView1.TabIndex = 148
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
        'PCancelBtn
        '
        Me.PCancelBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PCancelBtn.CheckedState.Parent = Me.PCancelBtn
        Me.PCancelBtn.CustomImages.Parent = Me.PCancelBtn
        Me.PCancelBtn.FillColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(28, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.PCancelBtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.PCancelBtn.ForeColor = System.Drawing.Color.White
        Me.PCancelBtn.HoverState.Parent = Me.PCancelBtn
        Me.PCancelBtn.Location = New System.Drawing.Point(1006, 55)
        Me.PCancelBtn.Name = "PCancelBtn"
        Me.PCancelBtn.ShadowDecoration.Parent = Me.PCancelBtn
        Me.PCancelBtn.Size = New System.Drawing.Size(72, 31)
        Me.PCancelBtn.TabIndex = 3
        Me.PCancelBtn.Text = "Cancel"
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
        Me.RefreshButton.Location = New System.Drawing.Point(575, 55)
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.ShadowDecoration.Parent = Me.RefreshButton
        Me.RefreshButton.Size = New System.Drawing.Size(76, 31)
        Me.RefreshButton.TabIndex = 4
        Me.RefreshButton.Text = "Refresh"
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
        Me.FromDateTextBox.Location = New System.Drawing.Point(366, 55)
        Me.FromDateTextBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.FromDateTextBox.Name = "FromDateTextBox"
        Me.FromDateTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.FromDateTextBox.PlaceholderText = ""
        Me.FromDateTextBox.SelectedText = ""
        Me.FromDateTextBox.ShadowDecoration.Parent = Me.FromDateTextBox
        Me.FromDateTextBox.Size = New System.Drawing.Size(100, 31)
        Me.FromDateTextBox.TabIndex = 0
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
        Me.ToDateTextBox.Location = New System.Drawing.Point(470, 55)
        Me.ToDateTextBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.ToDateTextBox.Name = "ToDateTextBox"
        Me.ToDateTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.ToDateTextBox.PlaceholderText = ""
        Me.ToDateTextBox.SelectedText = ""
        Me.ToDateTextBox.ShadowDecoration.Parent = Me.ToDateTextBox
        Me.ToDateTextBox.Size = New System.Drawing.Size(100, 31)
        Me.ToDateTextBox.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label4.Location = New System.Drawing.Point(361, 48)
        Me.Label4.MinimumSize = New System.Drawing.Size(260, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(726, 46)
        Me.Label4.TabIndex = 190
        Me.Label4.Text = " "
        '
        'PrintingDate
        '
        Me.PrintingDate.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.PrintingDate.DefaultText = ""
        Me.PrintingDate.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.PrintingDate.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.PrintingDate.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.PrintingDate.DisabledState.Parent = Me.PrintingDate
        Me.PrintingDate.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.PrintingDate.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PrintingDate.FocusedState.Parent = Me.PrintingDate
        Me.PrintingDate.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.PrintingDate.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PrintingDate.HoverState.Parent = Me.PrintingDate
        Me.PrintingDate.Location = New System.Drawing.Point(54, 54)
        Me.PrintingDate.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PrintingDate.Name = "PrintingDate"
        Me.PrintingDate.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.PrintingDate.PlaceholderText = ""
        Me.PrintingDate.SelectedText = ""
        Me.PrintingDate.ShadowDecoration.Parent = Me.PrintingDate
        Me.PrintingDate.Size = New System.Drawing.Size(120, 36)
        Me.PrintingDate.TabIndex = 3
        '
        'BillNoTxt
        '
        Me.BillNoTxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.BillNoTxt.DefaultText = ""
        Me.BillNoTxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.BillNoTxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.BillNoTxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.BillNoTxt.DisabledState.Parent = Me.BillNoTxt
        Me.BillNoTxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.BillNoTxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BillNoTxt.FocusedState.Parent = Me.BillNoTxt
        Me.BillNoTxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.BillNoTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BillNoTxt.HoverState.Parent = Me.BillNoTxt
        Me.BillNoTxt.Location = New System.Drawing.Point(224, 54)
        Me.BillNoTxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.BillNoTxt.Name = "BillNoTxt"
        Me.BillNoTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.BillNoTxt.PlaceholderText = ""
        Me.BillNoTxt.SelectedText = ""
        Me.BillNoTxt.ShadowDecoration.Parent = Me.BillNoTxt
        Me.BillNoTxt.Size = New System.Drawing.Size(120, 36)
        Me.BillNoTxt.TabIndex = 4
        Me.BillNoTxt.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(178, 65)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 15)
        Me.Label1.TabIndex = 192
        Me.Label1.Text = "Bill No"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(16, 65)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 15)
        Me.Label3.TabIndex = 191
        Me.Label3.Text = "Date "
        '
        'PartynameTxt
        '
        Me.PartynameTxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.PartynameTxt.DefaultText = ""
        Me.PartynameTxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.PartynameTxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.PartynameTxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.PartynameTxt.DisabledState.Parent = Me.PartynameTxt
        Me.PartynameTxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.PartynameTxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PartynameTxt.FocusedState.Parent = Me.PartynameTxt
        Me.PartynameTxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.PartynameTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PartynameTxt.HoverState.Parent = Me.PartynameTxt
        Me.PartynameTxt.Location = New System.Drawing.Point(17, 110)
        Me.PartynameTxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PartynameTxt.Name = "PartynameTxt"
        Me.PartynameTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.PartynameTxt.PlaceholderText = ""
        Me.PartynameTxt.SelectedText = ""
        Me.PartynameTxt.ShadowDecoration.Parent = Me.PartynameTxt
        Me.PartynameTxt.Size = New System.Drawing.Size(328, 36)
        Me.PartynameTxt.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(14, 92)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(151, 15)
        Me.Label5.TabIndex = 195
        Me.Label5.Text = "School / Wrapper Name"
        '
        'Papertxt
        '
        Me.Papertxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Papertxt.DefaultText = ""
        Me.Papertxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Papertxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Papertxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Papertxt.DisabledState.Parent = Me.Papertxt
        Me.Papertxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Papertxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Papertxt.FocusedState.Parent = Me.Papertxt
        Me.Papertxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Papertxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Papertxt.HoverState.Parent = Me.Papertxt
        Me.Papertxt.Location = New System.Drawing.Point(17, 338)
        Me.Papertxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Papertxt.Name = "Papertxt"
        Me.Papertxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Papertxt.PlaceholderText = ""
        Me.Papertxt.SelectedText = ""
        Me.Papertxt.ShadowDecoration.Parent = Me.Papertxt
        Me.Papertxt.Size = New System.Drawing.Size(109, 36)
        Me.Papertxt.TabIndex = 12
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(15, 319)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(109, 15)
        Me.Label2.TabIndex = 197
        Me.Label2.Text = "Paper Size / GSM"
        '
        'QtyTxt
        '
        Me.QtyTxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.QtyTxt.DefaultText = ""
        Me.QtyTxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.QtyTxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.QtyTxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.QtyTxt.DisabledState.Parent = Me.QtyTxt
        Me.QtyTxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.QtyTxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.QtyTxt.FocusedState.Parent = Me.QtyTxt
        Me.QtyTxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.QtyTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.QtyTxt.HoverState.Parent = Me.QtyTxt
        Me.QtyTxt.Location = New System.Drawing.Point(238, 338)
        Me.QtyTxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.QtyTxt.Name = "QtyTxt"
        Me.QtyTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.QtyTxt.PlaceholderText = ""
        Me.QtyTxt.SelectedText = ""
        Me.QtyTxt.ShadowDecoration.Parent = Me.QtyTxt
        Me.QtyTxt.Size = New System.Drawing.Size(109, 36)
        Me.QtyTxt.TabIndex = 14
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label10.Location = New System.Drawing.Point(239, 318)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(60, 15)
        Me.Label10.TabIndex = 199
        Me.Label10.Text = "Quantity"
        '
        'Colourtxt
        '
        Me.Colourtxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Colourtxt.DefaultText = ""
        Me.Colourtxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Colourtxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Colourtxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Colourtxt.DisabledState.Parent = Me.Colourtxt
        Me.Colourtxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Colourtxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Colourtxt.FocusedState.Parent = Me.Colourtxt
        Me.Colourtxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Colourtxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Colourtxt.HoverState.Parent = Me.Colourtxt
        Me.Colourtxt.Location = New System.Drawing.Point(18, 281)
        Me.Colourtxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Colourtxt.Name = "Colourtxt"
        Me.Colourtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Colourtxt.PlaceholderText = ""
        Me.Colourtxt.SelectedText = ""
        Me.Colourtxt.ShadowDecoration.Parent = Me.Colourtxt
        Me.Colourtxt.Size = New System.Drawing.Size(161, 36)
        Me.Colourtxt.TabIndex = 10
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(14, 263)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(99, 15)
        Me.Label7.TabIndex = 201
        Me.Label7.Text = "Printing Colour"
        '
        'Detailstxt
        '
        Me.Detailstxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Detailstxt.DefaultText = ""
        Me.Detailstxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Detailstxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Detailstxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Detailstxt.DisabledState.Parent = Me.Detailstxt
        Me.Detailstxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Detailstxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Detailstxt.FocusedState.Parent = Me.Detailstxt
        Me.Detailstxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Detailstxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Detailstxt.HoverState.Parent = Me.Detailstxt
        Me.Detailstxt.Location = New System.Drawing.Point(18, 407)
        Me.Detailstxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Detailstxt.Multiline = True
        Me.Detailstxt.Name = "Detailstxt"
        Me.Detailstxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Detailstxt.PlaceholderText = ""
        Me.Detailstxt.SelectedText = ""
        Me.Detailstxt.ShadowDecoration.Parent = Me.Detailstxt
        Me.Detailstxt.Size = New System.Drawing.Size(327, 161)
        Me.Detailstxt.TabIndex = 15
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.Transparent
        Me.Label13.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label13.Location = New System.Drawing.Point(17, 389)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(102, 15)
        Me.Label13.TabIndex = 203
        Me.Label13.Text = "Printing Details"
        '
        'PMethodTxt
        '
        Me.PMethodTxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.PMethodTxt.DefaultText = ""
        Me.PMethodTxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.PMethodTxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.PMethodTxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.PMethodTxt.DisabledState.Parent = Me.PMethodTxt
        Me.PMethodTxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.PMethodTxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PMethodTxt.FocusedState.Parent = Me.PMethodTxt
        Me.PMethodTxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.PMethodTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PMethodTxt.HoverState.Parent = Me.PMethodTxt
        Me.PMethodTxt.Location = New System.Drawing.Point(18, 167)
        Me.PMethodTxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PMethodTxt.Name = "PMethodTxt"
        Me.PMethodTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.PMethodTxt.PlaceholderText = ""
        Me.PMethodTxt.SelectedText = ""
        Me.PMethodTxt.ShadowDecoration.Parent = Me.PMethodTxt
        Me.PMethodTxt.Size = New System.Drawing.Size(161, 36)
        Me.PMethodTxt.TabIndex = 6
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(15, 149)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(106, 15)
        Me.Label8.TabIndex = 205
        Me.Label8.Text = "Printing Method"
        '
        'Status
        '
        Me.Status.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Status.BackColor = System.Drawing.Color.Transparent
        Me.Status.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.Status.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Status.FocusedColor = System.Drawing.Color.Empty
        Me.Status.FocusedState.Parent = Me.Status
        Me.Status.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Status.ForeColor = System.Drawing.Color.FromArgb(CType(CType(68, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(112, Byte), Integer))
        Me.Status.FormattingEnabled = True
        Me.Status.HoverState.Parent = Me.Status
        Me.Status.IntegralHeight = False
        Me.Status.ItemHeight = 25
        Me.Status.ItemsAppearance.Parent = Me.Status
        Me.Status.Location = New System.Drawing.Point(65, 588)
        Me.Status.Name = "Status"
        Me.Status.ShadowDecoration.Parent = Me.Status
        Me.Status.Size = New System.Drawing.Size(143, 31)
        Me.Status.TabIndex = 208
        '
        'Label14
        '
        Me.Label14.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(15, 596)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(45, 15)
        Me.Label14.TabIndex = 207
        Me.Label14.Text = "Status"
        '
        'PrintButton
        '
        Me.PrintButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PrintButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.PrintButton.CheckedState.Parent = Me.PrintButton
        Me.PrintButton.CustomImages.Parent = Me.PrintButton
        Me.PrintButton.FillColor = System.Drawing.SystemColors.ButtonShadow
        Me.PrintButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.PrintButton.ForeColor = System.Drawing.Color.White
        Me.PrintButton.HoverState.Parent = Me.PrintButton
        Me.PrintButton.Image = Global.Godown_Stock.My.Resources.Resources.printer_9654640
        Me.PrintButton.Location = New System.Drawing.Point(928, 55)
        Me.PrintButton.Name = "PrintButton"
        Me.PrintButton.ShadowDecoration.Parent = Me.PrintButton
        Me.PrintButton.Size = New System.Drawing.Size(72, 31)
        Me.PrintButton.TabIndex = 209
        Me.PrintButton.Text = "Print"
        '
        'PrintingTxt
        '
        Me.PrintingTxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.PrintingTxt.DefaultText = ""
        Me.PrintingTxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.PrintingTxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.PrintingTxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.PrintingTxt.DisabledState.Parent = Me.PrintingTxt
        Me.PrintingTxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.PrintingTxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PrintingTxt.FocusedState.Parent = Me.PrintingTxt
        Me.PrintingTxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.PrintingTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PrintingTxt.HoverState.Parent = Me.PrintingTxt
        Me.PrintingTxt.Location = New System.Drawing.Point(184, 165)
        Me.PrintingTxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PrintingTxt.Name = "PrintingTxt"
        Me.PrintingTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.PrintingTxt.PlaceholderText = ""
        Me.PrintingTxt.SelectedText = ""
        Me.PrintingTxt.ShadowDecoration.Parent = Me.PrintingTxt
        Me.PrintingTxt.Size = New System.Drawing.Size(161, 36)
        Me.PrintingTxt.TabIndex = 7
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(180, 147)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(90, 15)
        Me.Label9.TabIndex = 212
        Me.Label9.Text = "Printing Type"
        '
        'PMachineTxt
        '
        Me.PMachineTxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.PMachineTxt.DefaultText = ""
        Me.PMachineTxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.PMachineTxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.PMachineTxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.PMachineTxt.DisabledState.Parent = Me.PMachineTxt
        Me.PMachineTxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.PMachineTxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PMachineTxt.FocusedState.Parent = Me.PMachineTxt
        Me.PMachineTxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.PMachineTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PMachineTxt.HoverState.Parent = Me.PMachineTxt
        Me.PMachineTxt.Location = New System.Drawing.Point(18, 223)
        Me.PMachineTxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PMachineTxt.Name = "PMachineTxt"
        Me.PMachineTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.PMachineTxt.PlaceholderText = ""
        Me.PMachineTxt.SelectedText = ""
        Me.PMachineTxt.ShadowDecoration.Parent = Me.PMachineTxt
        Me.PMachineTxt.Size = New System.Drawing.Size(162, 36)
        Me.PMachineTxt.TabIndex = 8
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(19, 205)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(111, 15)
        Me.Label11.TabIndex = 210
        Me.Label11.Text = "Printing Machine"
        '
        'KryptonListBox
        '
        Me.KryptonListBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.KryptonListBox.Location = New System.Drawing.Point(956, 97)
        Me.KryptonListBox.Name = "KryptonListBox"
        Me.KryptonListBox.Size = New System.Drawing.Size(131, 194)
        Me.KryptonListBox.TabIndex = 214
        '
        'CompletedButton
        '
        Me.CompletedButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CompletedButton.BackColor = System.Drawing.Color.RoyalBlue
        Me.CompletedButton.CheckedState.Parent = Me.CompletedButton
        Me.CompletedButton.CustomImages.Parent = Me.CompletedButton
        Me.CompletedButton.FillColor = System.Drawing.Color.RoyalBlue
        Me.CompletedButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.CompletedButton.ForeColor = System.Drawing.Color.White
        Me.CompletedButton.HoverState.Parent = Me.CompletedButton
        Me.CompletedButton.Location = New System.Drawing.Point(850, 55)
        Me.CompletedButton.Name = "CompletedButton"
        Me.CompletedButton.ShadowDecoration.Parent = Me.CompletedButton
        Me.CompletedButton.Size = New System.Drawing.Size(72, 31)
        Me.CompletedButton.TabIndex = 19
        Me.CompletedButton.Text = "Update"
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
        Me.DateTxt.Location = New System.Drawing.Point(658, 55)
        Me.DateTxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.DateTxt.Name = "DateTxt"
        Me.DateTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.DateTxt.PlaceholderText = ""
        Me.DateTxt.SelectedText = ""
        Me.DateTxt.ShadowDecoration.Parent = Me.DateTxt
        Me.DateTxt.Size = New System.Drawing.Size(92, 31)
        Me.DateTxt.TabIndex = 17
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
        Me.UpdateTextBox.Location = New System.Drawing.Point(753, 55)
        Me.UpdateTextBox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.UpdateTextBox.Name = "UpdateTextBox"
        Me.UpdateTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.UpdateTextBox.PlaceholderText = ""
        Me.UpdateTextBox.SelectedText = ""
        Me.UpdateTextBox.ShadowDecoration.Parent = Me.UpdateTextBox
        Me.UpdateTextBox.Size = New System.Drawing.Size(92, 31)
        Me.UpdateTextBox.TabIndex = 18
        '
        'PrintingItemtxt
        '
        Me.PrintingItemtxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.PrintingItemtxt.DefaultText = ""
        Me.PrintingItemtxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.PrintingItemtxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.PrintingItemtxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.PrintingItemtxt.DisabledState.Parent = Me.PrintingItemtxt
        Me.PrintingItemtxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.PrintingItemtxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PrintingItemtxt.FocusedState.Parent = Me.PrintingItemtxt
        Me.PrintingItemtxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.PrintingItemtxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.PrintingItemtxt.HoverState.Parent = Me.PrintingItemtxt
        Me.PrintingItemtxt.Location = New System.Drawing.Point(183, 223)
        Me.PrintingItemtxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.PrintingItemtxt.Name = "PrintingItemtxt"
        Me.PrintingItemtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.PrintingItemtxt.PlaceholderText = ""
        Me.PrintingItemtxt.SelectedText = ""
        Me.PrintingItemtxt.ShadowDecoration.Parent = Me.PrintingItemtxt
        Me.PrintingItemtxt.Size = New System.Drawing.Size(161, 36)
        Me.PrintingItemtxt.TabIndex = 9
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.BackColor = System.Drawing.Color.Transparent
        Me.Label12.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label12.Location = New System.Drawing.Point(183, 205)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(89, 15)
        Me.Label12.TabIndex = 219
        Me.Label12.Text = "Printing Item"
        '
        'Weighttxt
        '
        Me.Weighttxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Weighttxt.DefaultText = ""
        Me.Weighttxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Weighttxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Weighttxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Weighttxt.DisabledState.Parent = Me.Weighttxt
        Me.Weighttxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Weighttxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Weighttxt.FocusedState.Parent = Me.Weighttxt
        Me.Weighttxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Weighttxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Weighttxt.HoverState.Parent = Me.Weighttxt
        Me.Weighttxt.Location = New System.Drawing.Point(128, 338)
        Me.Weighttxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Weighttxt.Name = "Weighttxt"
        Me.Weighttxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Weighttxt.PlaceholderText = ""
        Me.Weighttxt.SelectedText = ""
        Me.Weighttxt.ShadowDecoration.Parent = Me.Weighttxt
        Me.Weighttxt.Size = New System.Drawing.Size(109, 36)
        Me.Weighttxt.TabIndex = 13
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.BackColor = System.Drawing.Color.Transparent
        Me.Label15.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label15.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label15.Location = New System.Drawing.Point(134, 318)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(49, 15)
        Me.Label15.TabIndex = 221
        Me.Label15.Text = "Weight"
        '
        'Brandtxt
        '
        Me.Brandtxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Brandtxt.DefaultText = ""
        Me.Brandtxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Brandtxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Brandtxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Brandtxt.DisabledState.Parent = Me.Brandtxt
        Me.Brandtxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Brandtxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Brandtxt.FocusedState.Parent = Me.Brandtxt
        Me.Brandtxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Brandtxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Brandtxt.HoverState.Parent = Me.Brandtxt
        Me.Brandtxt.Location = New System.Drawing.Point(183, 281)
        Me.Brandtxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Brandtxt.Name = "Brandtxt"
        Me.Brandtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Brandtxt.PlaceholderText = ""
        Me.Brandtxt.SelectedText = ""
        Me.Brandtxt.ShadowDecoration.Parent = Me.Brandtxt
        Me.Brandtxt.Size = New System.Drawing.Size(161, 36)
        Me.Brandtxt.TabIndex = 11
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.BackColor = System.Drawing.Color.Transparent
        Me.Label16.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label16.Location = New System.Drawing.Point(179, 263)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(83, 15)
        Me.Label16.TabIndex = 223
        Me.Label16.Text = "Paper Brand"
        '
        'TimerFocusDelay
        '
        Me.TimerFocusDelay.Interval = 200
        '
        'Printing
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1096, 642)
        Me.Controls.Add(Me.KryptonListBox)
        Me.Controls.Add(Me.Brandtxt)
        Me.Controls.Add(Me.Label16)
        Me.Controls.Add(Me.Weighttxt)
        Me.Controls.Add(Me.Label15)
        Me.Controls.Add(Me.PrintingItemtxt)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.DateTxt)
        Me.Controls.Add(Me.UpdateTextBox)
        Me.Controls.Add(Me.CompletedButton)
        Me.Controls.Add(Me.PrintingTxt)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.PMachineTxt)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.PrintButton)
        Me.Controls.Add(Me.Status)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Savebtn)
        Me.Controls.Add(Me.PMethodTxt)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Detailstxt)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Colourtxt)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.QtyTxt)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Papertxt)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.PartynameTxt)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.PrintingDate)
        Me.Controls.Add(Me.BillNoTxt)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PCancelBtn)
        Me.Controls.Add(Me.RefreshButton)
        Me.Controls.Add(Me.Guna2DataGridView1)
        Me.Controls.Add(Me.FromDateTextBox)
        Me.Controls.Add(Me.ToDateTextBox)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.HeaderPanel)
        Me.Controls.Add(Me.Label4)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Printing"
        Me.Text = "Printing"
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        CType(Me.Guna2DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents HeaderPanel As Guna.UI2.WinForms.Guna2ShadowPanel
    Friend WithEvents Label As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Savebtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Label6 As Label
    Friend WithEvents Guna2DataGridView1 As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents PCancelBtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents RefreshButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents FromDateTextBox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents ToDateTextBox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label4 As Label
    Friend WithEvents PrintingDate As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents BillNoTxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents PartynameTxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Papertxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents QtyTxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents Colourtxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Detailstxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label13 As Label
    Friend WithEvents PMethodTxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents Status As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents Label14 As Label
    Friend WithEvents PrintButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents PrintingTxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents PMachineTxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents KryptonListBox As ComponentFactory.Krypton.Toolkit.KryptonListBox
    Friend WithEvents CompletedButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents DateTxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents UpdateTextBox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents PrintingItemtxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents Weighttxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label15 As Label
    Friend WithEvents Brandtxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents TimerFocusDelay As Timer
End Class

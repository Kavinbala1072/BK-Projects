<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Sales
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
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Addbtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Datesales = New Guna.UI2.WinForms.Guna2TextBox()
        Me.BillNoTxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Totamttxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.ratetxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.qtytxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.LabelNetAmt = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Clearbtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Savebtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2DataGridView1 = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.HeaderPanel = New Guna.UI2.WinForms.Guna2ShadowPanel()
        Me.lblSales = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.RemarksTextBox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.KryptonListBox1 = New ComponentFactory.Krypton.Toolkit.KryptonListBox()
        Me.Itembox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Partyname = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ManualBillnoText = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Guna2Elipse = New Guna.UI2.WinForms.Guna2Elipse(Me.components)
        Me.TimerFocusDelay = New System.Windows.Forms.Timer(Me.components)
        CType(Me.Guna2DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.HeaderPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'Addbtn
        '
        Me.Addbtn.CheckedState.Parent = Me.Addbtn
        Me.Addbtn.CustomImages.Parent = Me.Addbtn
        Me.Addbtn.FillColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(125, Byte), Integer), CType(CType(205, Byte), Integer))
        Me.Addbtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Addbtn.ForeColor = System.Drawing.Color.White
        Me.Addbtn.HoverState.Parent = Me.Addbtn
        Me.Addbtn.Location = New System.Drawing.Point(1016, 137)
        Me.Addbtn.Name = "Addbtn"
        Me.Addbtn.ShadowDecoration.Parent = Me.Addbtn
        Me.Addbtn.Size = New System.Drawing.Size(49, 36)
        Me.Addbtn.TabIndex = 8
        Me.Addbtn.Text = "Add"
        '
        'Datesales
        '
        Me.Datesales.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Datesales.DefaultText = ""
        Me.Datesales.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Datesales.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Datesales.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Datesales.DisabledState.Parent = Me.Datesales
        Me.Datesales.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Datesales.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Datesales.FocusedState.Parent = Me.Datesales
        Me.Datesales.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Datesales.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Datesales.HoverState.Parent = Me.Datesales
        Me.Datesales.Location = New System.Drawing.Point(61, 62)
        Me.Datesales.Name = "Datesales"
        Me.Datesales.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Datesales.PlaceholderText = ""
        Me.Datesales.SelectedText = ""
        Me.Datesales.ShadowDecoration.Parent = Me.Datesales
        Me.Datesales.Size = New System.Drawing.Size(138, 36)
        Me.Datesales.TabIndex = 0
        Me.Datesales.TabStop = False
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
        Me.BillNoTxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BillNoTxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BillNoTxt.HoverState.Parent = Me.BillNoTxt
        Me.BillNoTxt.Location = New System.Drawing.Point(265, 62)
        Me.BillNoTxt.Name = "BillNoTxt"
        Me.BillNoTxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.BillNoTxt.PlaceholderText = ""
        Me.BillNoTxt.ReadOnly = True
        Me.BillNoTxt.SelectedText = ""
        Me.BillNoTxt.ShadowDecoration.Parent = Me.BillNoTxt
        Me.BillNoTxt.Size = New System.Drawing.Size(138, 36)
        Me.BillNoTxt.TabIndex = 1
        Me.BillNoTxt.TabStop = False
        '
        'Totamttxt
        '
        Me.Totamttxt.Cursor = System.Windows.Forms.Cursors.No
        Me.Totamttxt.DefaultText = ""
        Me.Totamttxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Totamttxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Totamttxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Totamttxt.DisabledState.Parent = Me.Totamttxt
        Me.Totamttxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Totamttxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Totamttxt.FocusedState.Parent = Me.Totamttxt
        Me.Totamttxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Totamttxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Totamttxt.HoverState.Parent = Me.Totamttxt
        Me.Totamttxt.Location = New System.Drawing.Point(810, 137)
        Me.Totamttxt.Name = "Totamttxt"
        Me.Totamttxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Totamttxt.PlaceholderText = ""
        Me.Totamttxt.ReadOnly = True
        Me.Totamttxt.SelectedText = ""
        Me.Totamttxt.ShadowDecoration.Parent = Me.Totamttxt
        Me.Totamttxt.Size = New System.Drawing.Size(200, 36)
        Me.Totamttxt.TabIndex = 7
        Me.Totamttxt.TabStop = False
        Me.Totamttxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'ratetxt
        '
        Me.ratetxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ratetxt.DefaultText = ""
        Me.ratetxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.ratetxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.ratetxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.ratetxt.DisabledState.Parent = Me.ratetxt
        Me.ratetxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.ratetxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ratetxt.FocusedState.Parent = Me.ratetxt
        Me.ratetxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ratetxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ratetxt.HoverState.Parent = Me.ratetxt
        Me.ratetxt.Location = New System.Drawing.Point(600, 137)
        Me.ratetxt.Name = "ratetxt"
        Me.ratetxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.ratetxt.PlaceholderText = ""
        Me.ratetxt.SelectedText = ""
        Me.ratetxt.ShadowDecoration.Parent = Me.ratetxt
        Me.ratetxt.Size = New System.Drawing.Size(200, 36)
        Me.ratetxt.TabIndex = 6
        Me.ratetxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'qtytxt
        '
        Me.qtytxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.qtytxt.DefaultText = ""
        Me.qtytxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.qtytxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.qtytxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.qtytxt.DisabledState.Parent = Me.qtytxt
        Me.qtytxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.qtytxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.qtytxt.FocusedState.Parent = Me.qtytxt
        Me.qtytxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.qtytxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.qtytxt.HoverState.Parent = Me.qtytxt
        Me.qtytxt.Location = New System.Drawing.Point(392, 137)
        Me.qtytxt.Name = "qtytxt"
        Me.qtytxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.qtytxt.PlaceholderText = ""
        Me.qtytxt.SelectedText = ""
        Me.qtytxt.ShadowDecoration.Parent = Me.qtytxt
        Me.qtytxt.Size = New System.Drawing.Size(200, 36)
        Me.qtytxt.TabIndex = 5
        Me.qtytxt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'LabelNetAmt
        '
        Me.LabelNetAmt.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LabelNetAmt.AutoSize = True
        Me.LabelNetAmt.BackColor = System.Drawing.Color.Transparent
        Me.LabelNetAmt.Cursor = System.Windows.Forms.Cursors.No
        Me.LabelNetAmt.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LabelNetAmt.ForeColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(125, Byte), Integer), CType(CType(205, Byte), Integer))
        Me.LabelNetAmt.Location = New System.Drawing.Point(866, 485)
        Me.LabelNetAmt.Name = "LabelNetAmt"
        Me.LabelNetAmt.Size = New System.Drawing.Size(84, 37)
        Me.LabelNetAmt.TabIndex = 149
        Me.LabelNetAmt.Text = "0.00"
        Me.LabelNetAmt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label14
        '
        Me.Label14.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label14.AutoSize = True
        Me.Label14.BackColor = System.Drawing.Color.Transparent
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(5, Byte), Integer), CType(CType(125, Byte), Integer), CType(CType(205, Byte), Integer))
        Me.Label14.Location = New System.Drawing.Point(547, 485)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(198, 37)
        Me.Label14.TabIndex = 148
        Me.Label14.Text = "Net Amount"
        '
        'Clearbtn
        '
        Me.Clearbtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Clearbtn.CheckedState.Parent = Me.Clearbtn
        Me.Clearbtn.CustomImages.Parent = Me.Clearbtn
        Me.Clearbtn.FillColor = System.Drawing.Color.Orange
        Me.Clearbtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Clearbtn.ForeColor = System.Drawing.Color.White
        Me.Clearbtn.HoverState.Parent = Me.Clearbtn
        Me.Clearbtn.Location = New System.Drawing.Point(820, 538)
        Me.Clearbtn.Name = "Clearbtn"
        Me.Clearbtn.ShadowDecoration.Parent = Me.Clearbtn
        Me.Clearbtn.Size = New System.Drawing.Size(125, 34)
        Me.Clearbtn.TabIndex = 11
        Me.Clearbtn.TabStop = False
        Me.Clearbtn.Text = "Clear"
        '
        'Savebtn
        '
        Me.Savebtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Savebtn.CheckedState.Parent = Me.Savebtn
        Me.Savebtn.CustomImages.Parent = Me.Savebtn
        Me.Savebtn.FillColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(134, Byte), Integer), CType(CType(3, Byte), Integer))
        Me.Savebtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Savebtn.ForeColor = System.Drawing.Color.White
        Me.Savebtn.HoverState.Parent = Me.Savebtn
        Me.Savebtn.Location = New System.Drawing.Point(951, 538)
        Me.Savebtn.Name = "Savebtn"
        Me.Savebtn.ShadowDecoration.Parent = Me.Savebtn
        Me.Savebtn.Size = New System.Drawing.Size(125, 34)
        Me.Savebtn.TabIndex = 10
        Me.Savebtn.Text = "Save"
        '
        'Guna2DataGridView1
        '
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle4
        Me.Guna2DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Guna2DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.Guna2DataGridView1.BackgroundColor = System.Drawing.Color.Silver
        Me.Guna2DataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.Guna2DataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal
        Me.Guna2DataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(CType(CType(100, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle5.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        DataGridViewCellStyle5.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Guna2DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.Guna2DataGridView1.ColumnHeadersHeight = 4
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle6.Font = New System.Drawing.Font("Segoe UI", 10.5!)
        DataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(CType(CType(71, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(94, Byte), Integer))
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.Guna2DataGridView1.DefaultCellStyle = DataGridViewCellStyle6
        Me.Guna2DataGridView1.EnableHeadersVisualStyles = False
        Me.Guna2DataGridView1.GridColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.Guna2DataGridView1.Location = New System.Drawing.Point(12, 185)
        Me.Guna2DataGridView1.Name = "Guna2DataGridView1"
        Me.Guna2DataGridView1.RowHeadersVisible = False
        Me.Guna2DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Guna2DataGridView1.Size = New System.Drawing.Size(1072, 280)
        Me.Guna2DataGridView1.TabIndex = 145
        Me.Guna2DataGridView1.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.[Default]
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.Font = Nothing
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty
        Me.Guna2DataGridView1.ThemeStyle.BackColor = System.Drawing.Color.Silver
        Me.Guna2DataGridView1.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(249, Byte), Integer))
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
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.Transparent
        Me.Label11.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label11.Location = New System.Drawing.Point(810, 119)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(55, 15)
        Me.Label11.TabIndex = 144
        Me.Label11.Text = "Amount"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.BackColor = System.Drawing.Color.Transparent
        Me.Label8.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label8.Location = New System.Drawing.Point(601, 119)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(35, 15)
        Me.Label8.TabIndex = 143
        Me.Label8.Text = "Rate"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.BackColor = System.Drawing.Color.Transparent
        Me.Label7.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label7.Location = New System.Drawing.Point(397, 119)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(28, 15)
        Me.Label7.TabIndex = 142
        Me.Label7.Text = "Qty"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.Transparent
        Me.Label5.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label5.Location = New System.Drawing.Point(20, 119)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(74, 15)
        Me.Label5.TabIndex = 141
        Me.Label5.Text = "Item Name"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(417, 73)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(78, 15)
        Me.Label1.TabIndex = 139
        Me.Label1.Text = "Party Name"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(213, 73)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(46, 15)
        Me.Label4.TabIndex = 138
        Me.Label4.Text = "Bill No"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(20, 73)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(38, 15)
        Me.Label3.TabIndex = 137
        Me.Label3.Text = "Date "
        '
        'Label17
        '
        Me.Label17.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label17.BackColor = System.Drawing.Color.Transparent
        Me.Label17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label17.Location = New System.Drawing.Point(12, 51)
        Me.Label17.MinimumSize = New System.Drawing.Size(1070, 130)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(1072, 130)
        Me.Label17.TabIndex = 140
        Me.Label17.Text = " "
        '
        'HeaderPanel
        '
        Me.HeaderPanel.BackColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.Controls.Add(Me.lblSales)
        Me.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.HeaderPanel.FillColor = System.Drawing.Color.Empty
        Me.HeaderPanel.ForeColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.Location = New System.Drawing.Point(0, 0)
        Me.HeaderPanel.Name = "HeaderPanel"
        Me.HeaderPanel.ShadowColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.ShadowDepth = 0
        Me.HeaderPanel.Size = New System.Drawing.Size(1096, 42)
        Me.HeaderPanel.TabIndex = 136
        '
        'lblSales
        '
        Me.lblSales.BackColor = System.Drawing.Color.Transparent
        Me.lblSales.Font = New System.Drawing.Font("Tahoma", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSales.ForeColor = System.Drawing.Color.White
        Me.lblSales.Location = New System.Drawing.Point(12, 7)
        Me.lblSales.Name = "lblSales"
        Me.lblSales.Size = New System.Drawing.Size(153, 27)
        Me.lblSales.TabIndex = 1
        Me.lblSales.Text = "Sales Creation"
        '
        'RemarksTextBox
        '
        Me.RemarksTextBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RemarksTextBox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.RemarksTextBox.DefaultText = ""
        Me.RemarksTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.RemarksTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.RemarksTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.RemarksTextBox.DisabledState.Parent = Me.RemarksTextBox
        Me.RemarksTextBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.RemarksTextBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RemarksTextBox.FocusedState.Parent = Me.RemarksTextBox
        Me.RemarksTextBox.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RemarksTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.RemarksTextBox.HoverState.Parent = Me.RemarksTextBox
        Me.RemarksTextBox.Location = New System.Drawing.Point(21, 503)
        Me.RemarksTextBox.Multiline = True
        Me.RemarksTextBox.Name = "RemarksTextBox"
        Me.RemarksTextBox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.RemarksTextBox.PlaceholderText = ""
        Me.RemarksTextBox.SelectedText = ""
        Me.RemarksTextBox.ShadowDecoration.Parent = Me.RemarksTextBox
        Me.RemarksTextBox.Size = New System.Drawing.Size(496, 69)
        Me.RemarksTextBox.TabIndex = 9
        '
        'Label9
        '
        Me.Label9.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label9.AutoSize = True
        Me.Label9.BackColor = System.Drawing.Color.Transparent
        Me.Label9.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label9.Location = New System.Drawing.Point(24, 483)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 15)
        Me.Label9.TabIndex = 162
        Me.Label9.Text = "Remarks"
        '
        'Label10
        '
        Me.Label10.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label10.BackColor = System.Drawing.Color.Transparent
        Me.Label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label10.Location = New System.Drawing.Point(12, 469)
        Me.Label10.MinimumSize = New System.Drawing.Size(515, 95)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(1072, 112)
        Me.Label10.TabIndex = 161
        Me.Label10.Text = " "
        '
        'KryptonListBox1
        '
        Me.KryptonListBox1.Location = New System.Drawing.Point(21, 184)
        Me.KryptonListBox1.Name = "KryptonListBox1"
        Me.KryptonListBox1.Size = New System.Drawing.Size(365, 280)
        Me.KryptonListBox1.TabIndex = 164
        Me.KryptonListBox1.Visible = False
        '
        'Itembox
        '
        Me.Itembox.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Itembox.DefaultText = ""
        Me.Itembox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Itembox.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Itembox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Itembox.DisabledState.Parent = Me.Itembox
        Me.Itembox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Itembox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Itembox.FocusedState.Parent = Me.Itembox
        Me.Itembox.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Itembox.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Itembox.HoverState.Parent = Me.Itembox
        Me.Itembox.Location = New System.Drawing.Point(21, 137)
        Me.Itembox.Name = "Itembox"
        Me.Itembox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Itembox.PlaceholderText = ""
        Me.Itembox.SelectedText = ""
        Me.Itembox.ShadowDecoration.Parent = Me.Itembox
        Me.Itembox.Size = New System.Drawing.Size(365, 36)
        Me.Itembox.TabIndex = 4
        '
        'Partyname
        '
        Me.Partyname.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Partyname.DefaultText = ""
        Me.Partyname.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Partyname.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Partyname.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Partyname.DisabledState.Parent = Me.Partyname
        Me.Partyname.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Partyname.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Partyname.FocusedState.Parent = Me.Partyname
        Me.Partyname.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Partyname.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Partyname.HoverState.Parent = Me.Partyname
        Me.Partyname.Location = New System.Drawing.Point(504, 62)
        Me.Partyname.Name = "Partyname"
        Me.Partyname.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Partyname.PlaceholderText = ""
        Me.Partyname.SelectedText = ""
        Me.Partyname.ShadowDecoration.Parent = Me.Partyname
        Me.Partyname.Size = New System.Drawing.Size(306, 36)
        Me.Partyname.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Cambria", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(820, 73)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(94, 15)
        Me.Label2.TabIndex = 168
        Me.Label2.Text = "Manual Bill No"
        '
        'ManualBillnoText
        '
        Me.ManualBillnoText.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.ManualBillnoText.DefaultText = ""
        Me.ManualBillnoText.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.ManualBillnoText.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.ManualBillnoText.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.ManualBillnoText.DisabledState.Parent = Me.ManualBillnoText
        Me.ManualBillnoText.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.ManualBillnoText.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ManualBillnoText.FocusedState.Parent = Me.ManualBillnoText
        Me.ManualBillnoText.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ManualBillnoText.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ManualBillnoText.HoverState.Parent = Me.ManualBillnoText
        Me.ManualBillnoText.Location = New System.Drawing.Point(920, 62)
        Me.ManualBillnoText.Name = "ManualBillnoText"
        Me.ManualBillnoText.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.ManualBillnoText.PlaceholderText = ""
        Me.ManualBillnoText.SelectedText = ""
        Me.ManualBillnoText.ShadowDecoration.Parent = Me.ManualBillnoText
        Me.ManualBillnoText.Size = New System.Drawing.Size(138, 36)
        Me.ManualBillnoText.TabIndex = 3
        '
        'TimerFocusDelay
        '
        Me.TimerFocusDelay.Interval = 200
        '
        'Sales
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(244, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(242, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1096, 586)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ManualBillnoText)
        Me.Controls.Add(Me.Partyname)
        Me.Controls.Add(Me.Itembox)
        Me.Controls.Add(Me.KryptonListBox1)
        Me.Controls.Add(Me.RemarksTextBox)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Addbtn)
        Me.Controls.Add(Me.Datesales)
        Me.Controls.Add(Me.BillNoTxt)
        Me.Controls.Add(Me.Totamttxt)
        Me.Controls.Add(Me.ratetxt)
        Me.Controls.Add(Me.qtytxt)
        Me.Controls.Add(Me.LabelNetAmt)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.Clearbtn)
        Me.Controls.Add(Me.Savebtn)
        Me.Controls.Add(Me.Guna2DataGridView1)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label17)
        Me.Controls.Add(Me.HeaderPanel)
        Me.Controls.Add(Me.Label10)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Sales"
        Me.Text = "Sales"
        CType(Me.Guna2DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Addbtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Datesales As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents BillNoTxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Totamttxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents ratetxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents qtytxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents LabelNetAmt As Label
    Friend WithEvents Label14 As Label
    Friend WithEvents Clearbtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Savebtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2DataGridView1 As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents Label11 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label17 As Label
    Friend WithEvents HeaderPanel As Guna.UI2.WinForms.Guna2ShadowPanel
    Friend WithEvents lblSales As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents RemarksTextBox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents KryptonListBox1 As ComponentFactory.Krypton.Toolkit.KryptonListBox
    Friend WithEvents Itembox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Partyname As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents ManualBillnoText As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Guna2Elipse As Guna.UI2.WinForms.Guna2Elipse
    Friend WithEvents TimerFocusDelay As Timer
End Class

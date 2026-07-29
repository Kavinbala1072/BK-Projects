<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Voucher
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
        Me.lblSales = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.ClearButton = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2ToggleSwitch1 = New Guna.UI2.WinForms.Guna2ToggleSwitch()
        Me.AccountCombo = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.Guna2HtmlLabel11 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.AddBankButton = New Guna.UI2.WinForms.Guna2Button()
        Me.ToDate = New Guna.UI2.WinForms.Guna2DateTimePicker()
        Me.FromDate = New Guna.UI2.WinForms.Guna2DateTimePicker()
        Me.Guna2HtmlLabel10 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.txtBillNo = New Guna.UI2.WinForms.Guna2TextBox()
        Me.dtpJoiningDate = New Guna.UI2.WinForms.Guna2DateTimePicker()
        Me.Guna2HtmlLabel9 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.btnPrint = New Guna.UI2.WinForms.Guna2Button()
        Me.DTypeCombo = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.btnRefresh = New Guna.UI2.WinForms.Guna2Button()
        Me.PurposeCombo = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.PaymentCombo = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TypeCombo = New Guna.UI2.WinForms.Guna2ComboBox()
        Me.txtAmount = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtMemberName = New Guna.UI2.WinForms.Guna2TextBox()
        Me.txtMNo = New Guna.UI2.WinForms.Guna2TextBox()
        Me.btnCancel = New Guna.UI2.WinForms.Guna2Button()
        Me.btnSave = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2HtmlLabel7 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.txtRemarks = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Guna2HtmlLabel6 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel5 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel4 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel3 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel2 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2HtmlLabel1 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.Guna2DataGridView1 = New Guna.UI2.WinForms.Guna2DataGridView()
        Me.Guna2HtmlLabel8 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.btnPDF = New Guna.UI2.WinForms.Guna2Button()
        Me.HeaderPanel.SuspendLayout()
        Me.Guna2Panel1.SuspendLayout()
        CType(Me.Guna2DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'HeaderPanel
        '
        Me.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.HeaderPanel.Controls.Add(Me.lblSales)
        Me.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.HeaderPanel.FillColor = System.Drawing.Color.Empty
        Me.HeaderPanel.ForeColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.Location = New System.Drawing.Point(0, 0)
        Me.HeaderPanel.Name = "HeaderPanel"
        Me.HeaderPanel.ShadowColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.ShadowDepth = 0
        Me.HeaderPanel.Size = New System.Drawing.Size(1280, 42)
        Me.HeaderPanel.TabIndex = 138
        '
        'lblSales
        '
        Me.lblSales.BackColor = System.Drawing.Color.Transparent
        Me.lblSales.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSales.ForeColor = System.Drawing.Color.White
        Me.lblSales.Location = New System.Drawing.Point(12, 10)
        Me.lblSales.Name = "lblSales"
        Me.lblSales.Size = New System.Drawing.Size(144, 21)
        Me.lblSales.TabIndex = 1
        Me.lblSales.Text = "Voucher Creation"
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.Guna2Panel1.Controls.Add(Me.btnPDF)
        Me.Guna2Panel1.Controls.Add(Me.ClearButton)
        Me.Guna2Panel1.Controls.Add(Me.Guna2ToggleSwitch1)
        Me.Guna2Panel1.Controls.Add(Me.AccountCombo)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel11)
        Me.Guna2Panel1.Controls.Add(Me.AddBankButton)
        Me.Guna2Panel1.Controls.Add(Me.ToDate)
        Me.Guna2Panel1.Controls.Add(Me.FromDate)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel10)
        Me.Guna2Panel1.Controls.Add(Me.txtBillNo)
        Me.Guna2Panel1.Controls.Add(Me.dtpJoiningDate)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel9)
        Me.Guna2Panel1.Controls.Add(Me.btnPrint)
        Me.Guna2Panel1.Controls.Add(Me.DTypeCombo)
        Me.Guna2Panel1.Controls.Add(Me.btnRefresh)
        Me.Guna2Panel1.Controls.Add(Me.PurposeCombo)
        Me.Guna2Panel1.Controls.Add(Me.PaymentCombo)
        Me.Guna2Panel1.Controls.Add(Me.Label4)
        Me.Guna2Panel1.Controls.Add(Me.TypeCombo)
        Me.Guna2Panel1.Controls.Add(Me.txtAmount)
        Me.Guna2Panel1.Controls.Add(Me.txtMemberName)
        Me.Guna2Panel1.Controls.Add(Me.txtMNo)
        Me.Guna2Panel1.Controls.Add(Me.btnCancel)
        Me.Guna2Panel1.Controls.Add(Me.btnSave)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel7)
        Me.Guna2Panel1.Controls.Add(Me.txtRemarks)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel6)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel5)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel4)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel3)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel2)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel1)
        Me.Guna2Panel1.Controls.Add(Me.Guna2DataGridView1)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel8)
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Guna2Panel1.Location = New System.Drawing.Point(0, 42)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.ShadowDecoration.Parent = Me.Guna2Panel1
        Me.Guna2Panel1.Size = New System.Drawing.Size(1280, 678)
        Me.Guna2Panel1.TabIndex = 139
        '
        'ClearButton
        '
        Me.ClearButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ClearButton.CheckedState.Parent = Me.ClearButton
        Me.ClearButton.CustomImages.Parent = Me.ClearButton
        Me.ClearButton.FillColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ClearButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.ClearButton.ForeColor = System.Drawing.Color.White
        Me.ClearButton.HoverState.Parent = Me.ClearButton
        Me.ClearButton.Location = New System.Drawing.Point(139, 620)
        Me.ClearButton.Name = "ClearButton"
        Me.ClearButton.ShadowDecoration.Parent = Me.ClearButton
        Me.ClearButton.Size = New System.Drawing.Size(110, 36)
        Me.ClearButton.TabIndex = 210
        Me.ClearButton.Text = "CLEAR"
        '
        'Guna2ToggleSwitch1
        '
        Me.Guna2ToggleSwitch1.CheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2ToggleSwitch1.CheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2ToggleSwitch1.CheckedState.InnerBorderColor = System.Drawing.Color.White
        Me.Guna2ToggleSwitch1.CheckedState.InnerColor = System.Drawing.Color.White
        Me.Guna2ToggleSwitch1.CheckedState.Parent = Me.Guna2ToggleSwitch1
        Me.Guna2ToggleSwitch1.Location = New System.Drawing.Point(137, 156)
        Me.Guna2ToggleSwitch1.Name = "Guna2ToggleSwitch1"
        Me.Guna2ToggleSwitch1.ShadowDecoration.Parent = Me.Guna2ToggleSwitch1
        Me.Guna2ToggleSwitch1.Size = New System.Drawing.Size(35, 20)
        Me.Guna2ToggleSwitch1.TabIndex = 209
        Me.Guna2ToggleSwitch1.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        Me.Guna2ToggleSwitch1.UncheckedState.FillColor = System.Drawing.Color.FromArgb(CType(CType(125, Byte), Integer), CType(CType(137, Byte), Integer), CType(CType(149, Byte), Integer))
        Me.Guna2ToggleSwitch1.UncheckedState.InnerBorderColor = System.Drawing.Color.White
        Me.Guna2ToggleSwitch1.UncheckedState.InnerColor = System.Drawing.Color.White
        Me.Guna2ToggleSwitch1.UncheckedState.Parent = Me.Guna2ToggleSwitch1
        '
        'AccountCombo
        '
        Me.AccountCombo.BackColor = System.Drawing.Color.Transparent
        Me.AccountCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.AccountCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.AccountCombo.FocusedColor = System.Drawing.Color.Empty
        Me.AccountCombo.FocusedState.Parent = Me.AccountCombo
        Me.AccountCombo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.AccountCombo.ForeColor = System.Drawing.Color.Black
        Me.AccountCombo.FormattingEnabled = True
        Me.AccountCombo.HoverState.Parent = Me.AccountCombo
        Me.AccountCombo.ItemHeight = 30
        Me.AccountCombo.ItemsAppearance.Parent = Me.AccountCombo
        Me.AccountCombo.Location = New System.Drawing.Point(182, 358)
        Me.AccountCombo.Name = "AccountCombo"
        Me.AccountCombo.ShadowDecoration.Parent = Me.AccountCombo
        Me.AccountCombo.Size = New System.Drawing.Size(380, 36)
        Me.AccountCombo.TabIndex = 208
        '
        'Guna2HtmlLabel11
        '
        Me.Guna2HtmlLabel11.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel11.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Guna2HtmlLabel11.Location = New System.Drawing.Point(24, 366)
        Me.Guna2HtmlLabel11.Name = "Guna2HtmlLabel11"
        Me.Guna2HtmlLabel11.Size = New System.Drawing.Size(157, 19)
        Me.Guna2HtmlLabel11.TabIndex = 207
        Me.Guna2HtmlLabel11.Text = "TRANSACTION ACCOUNT"
        '
        'AddBankButton
        '
        Me.AddBankButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.AddBankButton.CheckedState.Parent = Me.AddBankButton
        Me.AddBankButton.CustomImages.Parent = Me.AddBankButton
        Me.AddBankButton.FillColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.AddBankButton.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.AddBankButton.ForeColor = System.Drawing.Color.White
        Me.AddBankButton.HoverState.Parent = Me.AddBankButton
        Me.AddBankButton.Location = New System.Drawing.Point(23, 620)
        Me.AddBankButton.Name = "AddBankButton"
        Me.AddBankButton.ShadowDecoration.Parent = Me.AddBankButton
        Me.AddBankButton.Size = New System.Drawing.Size(110, 36)
        Me.AddBankButton.TabIndex = 206
        Me.AddBankButton.Text = "ADD BANK"
        '
        'ToDate
        '
        Me.ToDate.BorderColor = System.Drawing.Color.FromArgb(CType(CType(213, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.ToDate.BorderThickness = 1
        Me.ToDate.CheckedState.Parent = Me.ToDate
        Me.ToDate.FillColor = System.Drawing.Color.White
        Me.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.[Long]
        Me.ToDate.HoverState.Parent = Me.ToDate
        Me.ToDate.Location = New System.Drawing.Point(868, 16)
        Me.ToDate.MaxDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.ToDate.MinDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.ToDate.Name = "ToDate"
        Me.ToDate.ShadowDecoration.Parent = Me.ToDate
        Me.ToDate.Size = New System.Drawing.Size(134, 36)
        Me.ToDate.TabIndex = 204
        Me.ToDate.Value = New Date(2026, 3, 12, 22, 45, 26, 351)
        '
        'FromDate
        '
        Me.FromDate.BorderColor = System.Drawing.Color.FromArgb(CType(CType(213, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.FromDate.BorderThickness = 1
        Me.FromDate.CheckedState.Parent = Me.FromDate
        Me.FromDate.FillColor = System.Drawing.Color.White
        Me.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.[Long]
        Me.FromDate.HoverState.Parent = Me.FromDate
        Me.FromDate.Location = New System.Drawing.Point(730, 16)
        Me.FromDate.MaxDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.FromDate.MinDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.FromDate.Name = "FromDate"
        Me.FromDate.ShadowDecoration.Parent = Me.FromDate
        Me.FromDate.Size = New System.Drawing.Size(134, 36)
        Me.FromDate.TabIndex = 203
        Me.FromDate.Value = New Date(2026, 3, 12, 22, 45, 26, 351)
        '
        'Guna2HtmlLabel10
        '
        Me.Guna2HtmlLabel10.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel10.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Guna2HtmlLabel10.Location = New System.Drawing.Point(282, 50)
        Me.Guna2HtmlLabel10.Name = "Guna2HtmlLabel10"
        Me.Guna2HtmlLabel10.Size = New System.Drawing.Size(51, 19)
        Me.Guna2HtmlLabel10.TabIndex = 202
        Me.Guna2HtmlLabel10.Text = "BILL NO"
        '
        'txtBillNo
        '
        Me.txtBillNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtBillNo.DefaultText = ""
        Me.txtBillNo.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtBillNo.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtBillNo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtBillNo.DisabledState.Parent = Me.txtBillNo
        Me.txtBillNo.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtBillNo.Enabled = False
        Me.txtBillNo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtBillNo.FocusedState.Parent = Me.txtBillNo
        Me.txtBillNo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtBillNo.ForeColor = System.Drawing.Color.Black
        Me.txtBillNo.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtBillNo.HoverState.Parent = Me.txtBillNo
        Me.txtBillNo.Location = New System.Drawing.Point(357, 42)
        Me.txtBillNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtBillNo.Name = "txtBillNo"
        Me.txtBillNo.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtBillNo.PlaceholderText = ""
        Me.txtBillNo.SelectedText = ""
        Me.txtBillNo.ShadowDecoration.Parent = Me.txtBillNo
        Me.txtBillNo.Size = New System.Drawing.Size(162, 36)
        Me.txtBillNo.TabIndex = 201
        '
        'dtpJoiningDate
        '
        Me.dtpJoiningDate.BorderColor = System.Drawing.Color.FromArgb(CType(CType(213, Byte), Integer), CType(CType(218, Byte), Integer), CType(CType(223, Byte), Integer))
        Me.dtpJoiningDate.BorderThickness = 1
        Me.dtpJoiningDate.CheckedState.Parent = Me.dtpJoiningDate
        Me.dtpJoiningDate.FillColor = System.Drawing.Color.White
        Me.dtpJoiningDate.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.dtpJoiningDate.ForeColor = System.Drawing.Color.Black
        Me.dtpJoiningDate.Format = System.Windows.Forms.DateTimePickerFormat.[Long]
        Me.dtpJoiningDate.HoverState.Parent = Me.dtpJoiningDate
        Me.dtpJoiningDate.Location = New System.Drawing.Point(82, 42)
        Me.dtpJoiningDate.MaxDate = New Date(9998, 12, 31, 0, 0, 0, 0)
        Me.dtpJoiningDate.MinDate = New Date(1753, 1, 1, 0, 0, 0, 0)
        Me.dtpJoiningDate.Name = "dtpJoiningDate"
        Me.dtpJoiningDate.ShadowDecoration.Parent = Me.dtpJoiningDate
        Me.dtpJoiningDate.Size = New System.Drawing.Size(159, 36)
        Me.dtpJoiningDate.TabIndex = 200
        Me.dtpJoiningDate.Value = New Date(2026, 3, 12, 22, 45, 26, 351)
        '
        'Guna2HtmlLabel9
        '
        Me.Guna2HtmlLabel9.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel9.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Guna2HtmlLabel9.Location = New System.Drawing.Point(24, 50)
        Me.Guna2HtmlLabel9.Name = "Guna2HtmlLabel9"
        Me.Guna2HtmlLabel9.Size = New System.Drawing.Size(35, 19)
        Me.Guna2HtmlLabel9.TabIndex = 199
        Me.Guna2HtmlLabel9.Text = "DATE"
        '
        'btnPrint
        '
        Me.btnPrint.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnPrint.CheckedState.Parent = Me.btnPrint
        Me.btnPrint.CustomImages.Parent = Me.btnPrint
        Me.btnPrint.FillColor = System.Drawing.SystemColors.ButtonShadow
        Me.btnPrint.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnPrint.ForeColor = System.Drawing.Color.White
        Me.btnPrint.HoverState.Parent = Me.btnPrint
        Me.btnPrint.Location = New System.Drawing.Point(1192, 16)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.ShadowDecoration.Parent = Me.btnPrint
        Me.btnPrint.Size = New System.Drawing.Size(72, 36)
        Me.btnPrint.TabIndex = 198
        Me.btnPrint.TabStop = False
        Me.btnPrint.Text = "PRINT"
        '
        'DTypeCombo
        '
        Me.DTypeCombo.BackColor = System.Drawing.Color.Transparent
        Me.DTypeCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.DTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.DTypeCombo.FocusedColor = System.Drawing.Color.Empty
        Me.DTypeCombo.FocusedState.Parent = Me.DTypeCombo
        Me.DTypeCombo.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.DTypeCombo.ForeColor = System.Drawing.Color.Black
        Me.DTypeCombo.FormattingEnabled = True
        Me.DTypeCombo.HoverState.Parent = Me.DTypeCombo
        Me.DTypeCombo.ItemHeight = 30
        Me.DTypeCombo.ItemsAppearance.Parent = Me.DTypeCombo
        Me.DTypeCombo.Location = New System.Drawing.Point(593, 16)
        Me.DTypeCombo.Name = "DTypeCombo"
        Me.DTypeCombo.ShadowDecoration.Parent = Me.DTypeCombo
        Me.DTypeCombo.Size = New System.Drawing.Size(133, 36)
        Me.DTypeCombo.TabIndex = 197
        '
        'btnRefresh
        '
        Me.btnRefresh.CheckedState.Parent = Me.btnRefresh
        Me.btnRefresh.CustomImages.Parent = Me.btnRefresh
        Me.btnRefresh.FillColor = System.Drawing.Color.RoyalBlue
        Me.btnRefresh.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnRefresh.ForeColor = System.Drawing.Color.White
        Me.btnRefresh.HoverState.Parent = Me.btnRefresh
        Me.btnRefresh.Location = New System.Drawing.Point(1005, 16)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.ShadowDecoration.Parent = Me.btnRefresh
        Me.btnRefresh.Size = New System.Drawing.Size(72, 36)
        Me.btnRefresh.TabIndex = 196
        Me.btnRefresh.Text = "REFRESH"
        '
        'PurposeCombo
        '
        Me.PurposeCombo.BackColor = System.Drawing.Color.Transparent
        Me.PurposeCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.PurposeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PurposeCombo.FocusedColor = System.Drawing.Color.Empty
        Me.PurposeCombo.FocusedState.Parent = Me.PurposeCombo
        Me.PurposeCombo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.PurposeCombo.ForeColor = System.Drawing.Color.Black
        Me.PurposeCombo.FormattingEnabled = True
        Me.PurposeCombo.HoverState.Parent = Me.PurposeCombo
        Me.PurposeCombo.ItemHeight = 30
        Me.PurposeCombo.ItemsAppearance.Parent = Me.PurposeCombo
        Me.PurposeCombo.Location = New System.Drawing.Point(182, 307)
        Me.PurposeCombo.Name = "PurposeCombo"
        Me.PurposeCombo.ShadowDecoration.Parent = Me.PurposeCombo
        Me.PurposeCombo.Size = New System.Drawing.Size(380, 36)
        Me.PurposeCombo.TabIndex = 193
        '
        'PaymentCombo
        '
        Me.PaymentCombo.BackColor = System.Drawing.Color.Transparent
        Me.PaymentCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.PaymentCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PaymentCombo.FocusedColor = System.Drawing.Color.Empty
        Me.PaymentCombo.FocusedState.Parent = Me.PaymentCombo
        Me.PaymentCombo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.PaymentCombo.ForeColor = System.Drawing.Color.Black
        Me.PaymentCombo.FormattingEnabled = True
        Me.PaymentCombo.HoverState.Parent = Me.PaymentCombo
        Me.PaymentCombo.ItemHeight = 30
        Me.PaymentCombo.ItemsAppearance.Parent = Me.PaymentCombo
        Me.PaymentCombo.Location = New System.Drawing.Point(182, 407)
        Me.PaymentCombo.Name = "PaymentCombo"
        Me.PaymentCombo.ShadowDecoration.Parent = Me.PaymentCombo
        Me.PaymentCombo.Size = New System.Drawing.Size(380, 36)
        Me.PaymentCombo.TabIndex = 192
        '
        'Label4
        '
        Me.Label4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label4.Location = New System.Drawing.Point(588, 11)
        Me.Label4.MinimumSize = New System.Drawing.Size(260, 40)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(680, 46)
        Me.Label4.TabIndex = 191
        Me.Label4.Text = " "
        '
        'TypeCombo
        '
        Me.TypeCombo.BackColor = System.Drawing.Color.Transparent
        Me.TypeCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.TypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TypeCombo.FocusedColor = System.Drawing.Color.Empty
        Me.TypeCombo.FocusedState.Parent = Me.TypeCombo
        Me.TypeCombo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.TypeCombo.ForeColor = System.Drawing.Color.Black
        Me.TypeCombo.FormattingEnabled = True
        Me.TypeCombo.HoverState.Parent = Me.TypeCombo
        Me.TypeCombo.ItemHeight = 30
        Me.TypeCombo.ItemsAppearance.Parent = Me.TypeCombo
        Me.TypeCombo.Location = New System.Drawing.Point(182, 95)
        Me.TypeCombo.Name = "TypeCombo"
        Me.TypeCombo.ShadowDecoration.Parent = Me.TypeCombo
        Me.TypeCombo.Size = New System.Drawing.Size(380, 36)
        Me.TypeCombo.TabIndex = 19
        '
        'txtAmount
        '
        Me.txtAmount.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtAmount.DefaultText = ""
        Me.txtAmount.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtAmount.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtAmount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtAmount.DisabledState.Parent = Me.txtAmount
        Me.txtAmount.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtAmount.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtAmount.FocusedState.Parent = Me.txtAmount
        Me.txtAmount.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtAmount.ForeColor = System.Drawing.Color.Black
        Me.txtAmount.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtAmount.HoverState.Parent = Me.txtAmount
        Me.txtAmount.Location = New System.Drawing.Point(182, 254)
        Me.txtAmount.Margin = New System.Windows.Forms.Padding(4)
        Me.txtAmount.Name = "txtAmount"
        Me.txtAmount.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtAmount.PlaceholderText = ""
        Me.txtAmount.SelectedText = ""
        Me.txtAmount.ShadowDecoration.Parent = Me.txtAmount
        Me.txtAmount.Size = New System.Drawing.Size(380, 36)
        Me.txtAmount.TabIndex = 4
        '
        'txtMemberName
        '
        Me.txtMemberName.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMemberName.DefaultText = ""
        Me.txtMemberName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtMemberName.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtMemberName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtMemberName.DisabledState.Parent = Me.txtMemberName
        Me.txtMemberName.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtMemberName.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtMemberName.FocusedState.Parent = Me.txtMemberName
        Me.txtMemberName.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtMemberName.ForeColor = System.Drawing.Color.Black
        Me.txtMemberName.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtMemberName.HoverState.Parent = Me.txtMemberName
        Me.txtMemberName.Location = New System.Drawing.Point(182, 201)
        Me.txtMemberName.Margin = New System.Windows.Forms.Padding(4)
        Me.txtMemberName.Name = "txtMemberName"
        Me.txtMemberName.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtMemberName.PlaceholderText = ""
        Me.txtMemberName.SelectedText = ""
        Me.txtMemberName.ShadowDecoration.Parent = Me.txtMemberName
        Me.txtMemberName.Size = New System.Drawing.Size(380, 36)
        Me.txtMemberName.TabIndex = 3
        '
        'txtMNo
        '
        Me.txtMNo.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtMNo.DefaultText = ""
        Me.txtMNo.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtMNo.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtMNo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtMNo.DisabledState.Parent = Me.txtMNo
        Me.txtMNo.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtMNo.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtMNo.FocusedState.Parent = Me.txtMNo
        Me.txtMNo.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtMNo.ForeColor = System.Drawing.Color.Black
        Me.txtMNo.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtMNo.HoverState.Parent = Me.txtMNo
        Me.txtMNo.Location = New System.Drawing.Point(182, 148)
        Me.txtMNo.Margin = New System.Windows.Forms.Padding(4)
        Me.txtMNo.Name = "txtMNo"
        Me.txtMNo.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtMNo.PlaceholderText = ""
        Me.txtMNo.SelectedText = ""
        Me.txtMNo.ShadowDecoration.Parent = Me.txtMNo
        Me.txtMNo.Size = New System.Drawing.Size(380, 36)
        Me.txtMNo.TabIndex = 2
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnCancel.CheckedState.Parent = Me.btnCancel
        Me.btnCancel.CustomImages.Parent = Me.btnCancel
        Me.btnCancel.FillColor = System.Drawing.Color.Red
        Me.btnCancel.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnCancel.ForeColor = System.Drawing.Color.White
        Me.btnCancel.HoverState.Parent = Me.btnCancel
        Me.btnCancel.Location = New System.Drawing.Point(336, 620)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.ShadowDecoration.Parent = Me.btnCancel
        Me.btnCancel.Size = New System.Drawing.Size(110, 36)
        Me.btnCancel.TabIndex = 17
        Me.btnCancel.Text = "CANCEL"
        '
        'btnSave
        '
        Me.btnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnSave.CheckedState.Parent = Me.btnSave
        Me.btnSave.CustomImages.Parent = Me.btnSave
        Me.btnSave.FillColor = System.Drawing.Color.Green
        Me.btnSave.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnSave.ForeColor = System.Drawing.Color.White
        Me.btnSave.HoverState.Parent = Me.btnSave
        Me.btnSave.Location = New System.Drawing.Point(452, 620)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.ShadowDecoration.Parent = Me.btnSave
        Me.btnSave.Size = New System.Drawing.Size(110, 36)
        Me.btnSave.TabIndex = 16
        Me.btnSave.Text = "SAVE"
        '
        'Guna2HtmlLabel7
        '
        Me.Guna2HtmlLabel7.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel7.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Guna2HtmlLabel7.Location = New System.Drawing.Point(24, 459)
        Me.Guna2HtmlLabel7.Name = "Guna2HtmlLabel7"
        Me.Guna2HtmlLabel7.Size = New System.Drawing.Size(62, 19)
        Me.Guna2HtmlLabel7.TabIndex = 15
        Me.Guna2HtmlLabel7.Text = "REMARKS"
        '
        'txtRemarks
        '
        Me.txtRemarks.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.txtRemarks.DefaultText = ""
        Me.txtRemarks.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.txtRemarks.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.txtRemarks.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtRemarks.DisabledState.Parent = Me.txtRemarks
        Me.txtRemarks.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.txtRemarks.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtRemarks.FocusedState.Parent = Me.txtRemarks
        Me.txtRemarks.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.txtRemarks.ForeColor = System.Drawing.Color.Black
        Me.txtRemarks.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.txtRemarks.HoverState.Parent = Me.txtRemarks
        Me.txtRemarks.Location = New System.Drawing.Point(24, 488)
        Me.txtRemarks.Margin = New System.Windows.Forms.Padding(4)
        Me.txtRemarks.Multiline = True
        Me.txtRemarks.Name = "txtRemarks"
        Me.txtRemarks.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.txtRemarks.PlaceholderText = ""
        Me.txtRemarks.SelectedText = ""
        Me.txtRemarks.ShadowDecoration.Parent = Me.txtRemarks
        Me.txtRemarks.Size = New System.Drawing.Size(538, 120)
        Me.txtRemarks.TabIndex = 14
        '
        'Guna2HtmlLabel6
        '
        Me.Guna2HtmlLabel6.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel6.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Guna2HtmlLabel6.Location = New System.Drawing.Point(24, 416)
        Me.Guna2HtmlLabel6.Name = "Guna2HtmlLabel6"
        Me.Guna2HtmlLabel6.Size = New System.Drawing.Size(126, 19)
        Me.Guna2HtmlLabel6.TabIndex = 13
        Me.Guna2HtmlLabel6.Text = "TRANSACTION TYPE"
        '
        'Guna2HtmlLabel5
        '
        Me.Guna2HtmlLabel5.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel5.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Guna2HtmlLabel5.Location = New System.Drawing.Point(24, 315)
        Me.Guna2HtmlLabel5.Name = "Guna2HtmlLabel5"
        Me.Guna2HtmlLabel5.Size = New System.Drawing.Size(60, 19)
        Me.Guna2HtmlLabel5.TabIndex = 11
        Me.Guna2HtmlLabel5.Text = "PURPOSE"
        '
        'Guna2HtmlLabel4
        '
        Me.Guna2HtmlLabel4.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel4.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Guna2HtmlLabel4.Location = New System.Drawing.Point(24, 262)
        Me.Guna2HtmlLabel4.Name = "Guna2HtmlLabel4"
        Me.Guna2HtmlLabel4.Size = New System.Drawing.Size(60, 19)
        Me.Guna2HtmlLabel4.TabIndex = 10
        Me.Guna2HtmlLabel4.Text = "AMOUNT"
        '
        'Guna2HtmlLabel3
        '
        Me.Guna2HtmlLabel3.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel3.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Guna2HtmlLabel3.Location = New System.Drawing.Point(24, 209)
        Me.Guna2HtmlLabel3.Name = "Guna2HtmlLabel3"
        Me.Guna2HtmlLabel3.Size = New System.Drawing.Size(99, 19)
        Me.Guna2HtmlLabel3.TabIndex = 9
        Me.Guna2HtmlLabel3.Text = "MEMBER NAME"
        '
        'Guna2HtmlLabel2
        '
        Me.Guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Guna2HtmlLabel2.Location = New System.Drawing.Point(24, 157)
        Me.Guna2HtmlLabel2.Name = "Guna2HtmlLabel2"
        Me.Guna2HtmlLabel2.Size = New System.Drawing.Size(81, 19)
        Me.Guna2HtmlLabel2.TabIndex = 8
        Me.Guna2HtmlLabel2.Text = "MEMBER NO"
        '
        'Guna2HtmlLabel1
        '
        Me.Guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel1.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Guna2HtmlLabel1.Location = New System.Drawing.Point(24, 104)
        Me.Guna2HtmlLabel1.Name = "Guna2HtmlLabel1"
        Me.Guna2HtmlLabel1.Size = New System.Drawing.Size(33, 19)
        Me.Guna2HtmlLabel1.TabIndex = 7
        Me.Guna2HtmlLabel1.Text = "TYPE"
        '
        'Guna2DataGridView1
        '
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.Guna2DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Guna2DataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.Guna2DataGridView1.BackgroundColor = System.Drawing.Color.Silver
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
        Me.Guna2DataGridView1.EnableHeadersVisualStyles = False
        Me.Guna2DataGridView1.GridColor = System.Drawing.Color.FromArgb(CType(CType(231, Byte), Integer), CType(CType(229, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Guna2DataGridView1.Location = New System.Drawing.Point(588, 60)
        Me.Guna2DataGridView1.Name = "Guna2DataGridView1"
        Me.Guna2DataGridView1.RowHeadersVisible = False
        Me.Guna2DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.Guna2DataGridView1.Size = New System.Drawing.Size(682, 606)
        Me.Guna2DataGridView1.TabIndex = 6
        Me.Guna2DataGridView1.Theme = Guna.UI2.WinForms.Enums.DataGridViewPresetThemes.[Default]
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.Font = Nothing
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty
        Me.Guna2DataGridView1.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty
        Me.Guna2DataGridView1.ThemeStyle.BackColor = System.Drawing.Color.Silver
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
        'Guna2HtmlLabel8
        '
        Me.Guna2HtmlLabel8.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Guna2HtmlLabel8.AutoSize = False
        Me.Guna2HtmlLabel8.BackColor = System.Drawing.Color.Transparent
        Me.Guna2HtmlLabel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Guna2HtmlLabel8.Location = New System.Drawing.Point(12, 11)
        Me.Guna2HtmlLabel8.Name = "Guna2HtmlLabel8"
        Me.Guna2HtmlLabel8.Size = New System.Drawing.Size(564, 655)
        Me.Guna2HtmlLabel8.TabIndex = 18
        Me.Guna2HtmlLabel8.Text = "  "
        '
        'btnPDF
        '
        Me.btnPDF.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPDF.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnPDF.CheckedState.Parent = Me.btnPDF
        Me.btnPDF.CustomImages.Parent = Me.btnPDF
        Me.btnPDF.FillColor = System.Drawing.SystemColors.ActiveCaption
        Me.btnPDF.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.btnPDF.ForeColor = System.Drawing.Color.White
        Me.btnPDF.HoverState.Parent = Me.btnPDF
        Me.btnPDF.Location = New System.Drawing.Point(1114, 16)
        Me.btnPDF.Name = "btnPDF"
        Me.btnPDF.ShadowDecoration.Parent = Me.btnPDF
        Me.btnPDF.Size = New System.Drawing.Size(72, 36)
        Me.btnPDF.TabIndex = 211
        Me.btnPDF.TabStop = False
        Me.btnPDF.Text = "PDF"
        '
        'Voucher
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1280, 720)
        Me.Controls.Add(Me.Guna2Panel1)
        Me.Controls.Add(Me.HeaderPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "Voucher"
        Me.Text = "Voucher"
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        Me.Guna2Panel1.ResumeLayout(False)
        Me.Guna2Panel1.PerformLayout()
        CType(Me.Guna2DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents HeaderPanel As Guna.UI2.WinForms.Guna2ShadowPanel
    Friend WithEvents lblSales As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents btnCancel As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnSave As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Guna2HtmlLabel7 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents txtRemarks As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Guna2HtmlLabel6 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel5 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel4 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel3 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel2 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel1 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2DataGridView1 As Guna.UI2.WinForms.Guna2DataGridView
    Friend WithEvents txtAmount As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtMemberName As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents txtMNo As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Guna2HtmlLabel8 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents TypeCombo As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents PurposeCombo As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents PaymentCombo As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents DTypeCombo As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents btnRefresh As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnPrint As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents dtpJoiningDate As Guna.UI2.WinForms.Guna2DateTimePicker
    Friend WithEvents Guna2HtmlLabel9 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2HtmlLabel10 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents txtBillNo As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents ToDate As Guna.UI2.WinForms.Guna2DateTimePicker
    Friend WithEvents FromDate As Guna.UI2.WinForms.Guna2DateTimePicker
    Friend WithEvents AddBankButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents AccountCombo As Guna.UI2.WinForms.Guna2ComboBox
    Friend WithEvents Guna2HtmlLabel11 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents Guna2ToggleSwitch1 As Guna.UI2.WinForms.Guna2ToggleSwitch
    Friend WithEvents ClearButton As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents btnPDF As Guna.UI2.WinForms.Guna2Button
End Class

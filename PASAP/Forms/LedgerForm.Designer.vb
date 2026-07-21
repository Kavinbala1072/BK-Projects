<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LedgerForm
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
        Me.Guna2Panel1 = New Guna.UI2.WinForms.Guna2Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ListBox1 = New ComponentFactory.Krypton.Toolkit.KryptonListBox()
        Me.underbox = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Openingtxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Partytxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Searchtxt = New Guna.UI2.WinForms.Guna2TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Savebtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Killbtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Searchbtn = New Guna.UI2.WinForms.Guna2Button()
        Me.Guna2HtmlLabel8 = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.lblSales = New Guna.UI2.WinForms.Guna2HtmlLabel()
        Me.HeaderPanel = New Guna.UI2.WinForms.Guna2ShadowPanel()
        Me.Guna2ControlBox1 = New Guna.UI2.WinForms.Guna2ControlBox()
        Me.TimerHideList = New System.Windows.Forms.Timer(Me.components)
        Me.Guna2Panel1.SuspendLayout()
        Me.HeaderPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'Guna2Panel1
        '
        Me.Guna2Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(240, Byte), Integer))
        Me.Guna2Panel1.Controls.Add(Me.Label4)
        Me.Guna2Panel1.Controls.Add(Me.ListBox1)
        Me.Guna2Panel1.Controls.Add(Me.underbox)
        Me.Guna2Panel1.Controls.Add(Me.Openingtxt)
        Me.Guna2Panel1.Controls.Add(Me.Label3)
        Me.Guna2Panel1.Controls.Add(Me.Partytxt)
        Me.Guna2Panel1.Controls.Add(Me.Searchtxt)
        Me.Guna2Panel1.Controls.Add(Me.Label2)
        Me.Guna2Panel1.Controls.Add(Me.Label1)
        Me.Guna2Panel1.Controls.Add(Me.Savebtn)
        Me.Guna2Panel1.Controls.Add(Me.Killbtn)
        Me.Guna2Panel1.Controls.Add(Me.Searchbtn)
        Me.Guna2Panel1.Controls.Add(Me.Guna2HtmlLabel8)
        Me.Guna2Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Guna2Panel1.Location = New System.Drawing.Point(0, 42)
        Me.Guna2Panel1.Name = "Guna2Panel1"
        Me.Guna2Panel1.ShadowDecoration.Parent = Me.Guna2Panel1
        Me.Guna2Panel1.Size = New System.Drawing.Size(588, 368)
        Me.Guna2Panel1.TabIndex = 141
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Transparent
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(33, 37)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(96, 13)
        Me.Label4.TabIndex = 152
        Me.Label4.Text = "LEDGER SEARCH"
        '
        'ListBox1
        '
        Me.ListBox1.Location = New System.Drawing.Point(30, 236)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(245, 107)
        Me.ListBox1.TabIndex = 151
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
        Me.underbox.Location = New System.Drawing.Point(160, 132)
        Me.underbox.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.underbox.Name = "underbox"
        Me.underbox.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.underbox.PlaceholderText = ""
        Me.underbox.SelectedText = ""
        Me.underbox.ShadowDecoration.Parent = Me.underbox
        Me.underbox.Size = New System.Drawing.Size(235, 36)
        Me.underbox.TabIndex = 142
        '
        'Openingtxt
        '
        Me.Openingtxt.Cursor = System.Windows.Forms.Cursors.IBeam
        Me.Openingtxt.DefaultText = ""
        Me.Openingtxt.DisabledState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(208, Byte), Integer))
        Me.Openingtxt.DisabledState.FillColor = System.Drawing.Color.FromArgb(CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(226, Byte), Integer))
        Me.Openingtxt.DisabledState.ForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Openingtxt.DisabledState.Parent = Me.Openingtxt
        Me.Openingtxt.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer), CType(CType(138, Byte), Integer))
        Me.Openingtxt.FocusedState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Openingtxt.FocusedState.Parent = Me.Openingtxt
        Me.Openingtxt.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold)
        Me.Openingtxt.HoverState.BorderColor = System.Drawing.Color.FromArgb(CType(CType(94, Byte), Integer), CType(CType(148, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Openingtxt.HoverState.Parent = Me.Openingtxt
        Me.Openingtxt.Location = New System.Drawing.Point(160, 189)
        Me.Openingtxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Openingtxt.Name = "Openingtxt"
        Me.Openingtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Openingtxt.PlaceholderText = ""
        Me.Openingtxt.SelectedText = ""
        Me.Openingtxt.ShadowDecoration.Parent = Me.Openingtxt
        Me.Openingtxt.Size = New System.Drawing.Size(235, 36)
        Me.Openingtxt.TabIndex = 143
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BackColor = System.Drawing.Color.Transparent
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(33, 199)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(107, 13)
        Me.Label3.TabIndex = 150
        Me.Label3.Text = "OPENING BALANCE"
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
        Me.Partytxt.Location = New System.Drawing.Point(160, 85)
        Me.Partytxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Partytxt.Name = "Partytxt"
        Me.Partytxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Partytxt.PlaceholderText = ""
        Me.Partytxt.SelectedText = ""
        Me.Partytxt.ShadowDecoration.Parent = Me.Partytxt
        Me.Partytxt.Size = New System.Drawing.Size(235, 36)
        Me.Partytxt.TabIndex = 141
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
        Me.Searchtxt.Location = New System.Drawing.Point(160, 24)
        Me.Searchtxt.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Searchtxt.Name = "Searchtxt"
        Me.Searchtxt.PasswordChar = Global.Microsoft.VisualBasic.ChrW(0)
        Me.Searchtxt.PlaceholderText = ""
        Me.Searchtxt.SelectedText = ""
        Me.Searchtxt.ShadowDecoration.Parent = Me.Searchtxt
        Me.Searchtxt.Size = New System.Drawing.Size(235, 36)
        Me.Searchtxt.TabIndex = 145
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.Transparent
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(33, 143)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(89, 13)
        Me.Label2.TabIndex = 149
        Me.Label2.Text = "LEDGER UNDER"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Transparent
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(33, 96)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 13)
        Me.Label1.TabIndex = 148
        Me.Label1.Text = "LEDGER NAME"
        '
        'Savebtn
        '
        Me.Savebtn.CheckedState.Parent = Me.Savebtn
        Me.Savebtn.CustomImages.Parent = Me.Savebtn
        Me.Savebtn.FillColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(134, Byte), Integer), CType(CType(3, Byte), Integer))
        Me.Savebtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Savebtn.ForeColor = System.Drawing.Color.White
        Me.Savebtn.HoverState.Parent = Me.Savebtn
        Me.Savebtn.Location = New System.Drawing.Point(435, 305)
        Me.Savebtn.Name = "Savebtn"
        Me.Savebtn.ShadowDecoration.Parent = Me.Savebtn
        Me.Savebtn.Size = New System.Drawing.Size(116, 38)
        Me.Savebtn.TabIndex = 144
        Me.Savebtn.Text = "SAVE"
        '
        'Killbtn
        '
        Me.Killbtn.CheckedState.Parent = Me.Killbtn
        Me.Killbtn.CustomImages.Parent = Me.Killbtn
        Me.Killbtn.FillColor = System.Drawing.Color.FromArgb(CType(CType(237, Byte), Integer), CType(CType(28, Byte), Integer), CType(CType(35, Byte), Integer))
        Me.Killbtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Killbtn.ForeColor = System.Drawing.Color.White
        Me.Killbtn.HoverState.Parent = Me.Killbtn
        Me.Killbtn.Location = New System.Drawing.Point(293, 305)
        Me.Killbtn.Name = "Killbtn"
        Me.Killbtn.ShadowDecoration.Parent = Me.Killbtn
        Me.Killbtn.Size = New System.Drawing.Size(116, 38)
        Me.Killbtn.TabIndex = 147
        Me.Killbtn.Text = "INACTIVE"
        '
        'Searchbtn
        '
        Me.Searchbtn.CheckedState.Parent = Me.Searchbtn
        Me.Searchbtn.CustomImages.Parent = Me.Searchbtn
        Me.Searchbtn.FillColor = System.Drawing.Color.RoyalBlue
        Me.Searchbtn.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.Searchbtn.ForeColor = System.Drawing.Color.White
        Me.Searchbtn.HoverState.Parent = Me.Searchbtn
        Me.Searchbtn.Location = New System.Drawing.Point(404, 24)
        Me.Searchbtn.Name = "Searchbtn"
        Me.Searchbtn.ShadowDecoration.Parent = Me.Searchbtn
        Me.Searchbtn.Size = New System.Drawing.Size(81, 36)
        Me.Searchbtn.TabIndex = 146
        Me.Searchbtn.Text = "Search"
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
        Me.Guna2HtmlLabel8.Size = New System.Drawing.Size(564, 345)
        Me.Guna2HtmlLabel8.TabIndex = 18
        Me.Guna2HtmlLabel8.Text = "  "
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
        'HeaderPanel
        '
        Me.HeaderPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(34, Byte), Integer), CType(CType(40, Byte), Integer), CType(CType(49, Byte), Integer))
        Me.HeaderPanel.Controls.Add(Me.Guna2ControlBox1)
        Me.HeaderPanel.Controls.Add(Me.lblSales)
        Me.HeaderPanel.Dock = System.Windows.Forms.DockStyle.Top
        Me.HeaderPanel.FillColor = System.Drawing.Color.Empty
        Me.HeaderPanel.ForeColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.Location = New System.Drawing.Point(0, 0)
        Me.HeaderPanel.Name = "HeaderPanel"
        Me.HeaderPanel.ShadowColor = System.Drawing.Color.Transparent
        Me.HeaderPanel.ShadowDepth = 0
        Me.HeaderPanel.Size = New System.Drawing.Size(588, 42)
        Me.HeaderPanel.TabIndex = 140
        '
        'Guna2ControlBox1
        '
        Me.Guna2ControlBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Guna2ControlBox1.FillColor = System.Drawing.Color.FromArgb(CType(CType(139, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.Guna2ControlBox1.HoverState.Parent = Me.Guna2ControlBox1
        Me.Guna2ControlBox1.IconColor = System.Drawing.Color.White
        Me.Guna2ControlBox1.Location = New System.Drawing.Point(546, 7)
        Me.Guna2ControlBox1.Name = "Guna2ControlBox1"
        Me.Guna2ControlBox1.ShadowDecoration.Parent = Me.Guna2ControlBox1
        Me.Guna2ControlBox1.Size = New System.Drawing.Size(30, 29)
        Me.Guna2ControlBox1.TabIndex = 2
        '
        'TimerHideList
        '
        '
        'LedgerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(588, 410)
        Me.Controls.Add(Me.Guna2Panel1)
        Me.Controls.Add(Me.HeaderPanel)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "LedgerForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LedgerForm"
        Me.Guna2Panel1.ResumeLayout(False)
        Me.Guna2Panel1.PerformLayout()
        Me.HeaderPanel.ResumeLayout(False)
        Me.HeaderPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Guna2Panel1 As Guna.UI2.WinForms.Guna2Panel
    Friend WithEvents Guna2HtmlLabel8 As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents lblSales As Guna.UI2.WinForms.Guna2HtmlLabel
    Friend WithEvents HeaderPanel As Guna.UI2.WinForms.Guna2ShadowPanel
    Friend WithEvents ListBox1 As ComponentFactory.Krypton.Toolkit.KryptonListBox
    Friend WithEvents underbox As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Openingtxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Partytxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Searchtxt As Guna.UI2.WinForms.Guna2TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Savebtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Killbtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Searchbtn As Guna.UI2.WinForms.Guna2Button
    Friend WithEvents Label4 As Label
    Friend WithEvents TimerHideList As Timer
    Friend WithEvents Guna2ControlBox1 As Guna.UI2.WinForms.Guna2ControlBox
End Class

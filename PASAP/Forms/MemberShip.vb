Imports System.Data.SqlClient
Imports System.IO
Imports System.Drawing.Imaging
Imports System.Drawing.Printing

Public Class MemberShip

    Private SelectedMemberID As Guid = Guid.Empty
    Private PrintMemberID As Guid = Guid.Empty

    Private IsActiveStatus As Boolean = True
    Private FinStartDate As DateTime
    Private FinEndDate As DateTime

    Private Sub MemberShip_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeDesign()
        LoadFinancialPeriod()
        LoadMemberData()
        ClearFields()
    End Sub

    Private Sub InitializeDesign()

        Dim dtps As Guna.UI2.WinForms.Guna2DateTimePicker() = {dtpJoiningDate}
        For Each dtp In dtps
            dtp.Format = DateTimePickerFormat.Custom
            dtp.CustomFormat = "dd-MM-yyyy"
            dtp.FillColor = Color.White
            dtp.ForeColor = Color.Black
            dtp.CheckedState.FillColor = Color.FromArgb(255, 128, 64)
            dtp.CheckedState.ForeColor = Color.White
        Next

        pbMemberPhoto.SizeMode = PictureBoxSizeMode.Zoom
        pbMemberPhoto.BackColor = Color.WhiteSmoke
        pbMemberPhoto.BorderStyle = BorderStyle.FixedSingle

        dtpJoiningDate.Format = DateTimePickerFormat.Custom
        dtpJoiningDate.CustomFormat = "dd-MM-yyyy"
        dtpJoiningDate.Value = DateTime.Today

        With Guna2DataGridView1
            .AllowUserToAddRows = False
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245)
            .ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(34, 40, 49)
            .ThemeStyle.HeaderStyle.ForeColor = Color.White
            .ThemeStyle.HeaderStyle.Font = New Font("Segoe UI", 10, FontStyle.Bold)
            .ColumnHeadersHeight = 40
            .RowTemplate.Height = 32
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .GridColor = Color.FromArgb(231, 229, 255)
            .ThemeStyle.GridColor = Color.FromArgb(231, 229, 255)
            .ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single
            .ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.Single
            .Padding = New Padding(0, 0, 0, 35)
        End With
    End Sub



    Private Sub LoadFinancialPeriod()
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim sql As String = "SELECT Ctl_Desc, Ctl_Value FROM Control_Table WHERE Ctl_Desc IN ('fromDate', 'toDate')"
                Using cmd As New SqlCommand(sql, conn)
                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        While rdr.Read()
                            Dim desc As String = rdr("Ctl_Desc").ToString()
                            Dim val As String = rdr("Ctl_Value").ToString()
                            If desc = "fromDate" Then FinStartDate = DateTime.ParseExact(val, "dd-MM-yyyy", Nothing)
                            If desc = "toDate" Then FinEndDate = DateTime.ParseExact(val, "dd-MM-yyyy", Nothing)
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading financial period. Check Settings.")
        End Try
    End Sub

    Private Sub LoadMemberData()
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                Dim query As String = "SELECT ID, M_No, Member_Name, Mobile_No, Address_Text, Aadhar_No, Remarks, Is_Active, Member_Photo, Joining_Date FROM Member_Table where M_No != '0' ORDER BY Created_Date DESC"
                Dim adapter As New SqlDataAdapter(query, conn)
                Dim dt As New DataTable()
                adapter.Fill(dt)

                dt.Columns.Add("SNo", GetType(Integer)).SetOrdinal(0)
                For i As Integer = 0 To dt.Rows.Count - 1
                    dt.Rows(i)("SNo") = i + 1
                Next

                Guna2DataGridView1.DataSource = dt

                If Guna2DataGridView1.Columns.Count > 0 Then
                    Guna2DataGridView1.Columns("ID").Visible = False
                    Guna2DataGridView1.Columns("Member_Photo").Visible = False
                    Guna2DataGridView1.Columns("Is_Active").Visible = False
                    Guna2DataGridView1.Columns("Remarks").Visible = False
                    Guna2DataGridView1.Columns("Address_Text").Visible = False

                    Guna2DataGridView1.Columns("SNo").HeaderText = "S.No"
                    Guna2DataGridView1.Columns("M_No").HeaderText = "M.No"
                    Guna2DataGridView1.Columns("Member_Name").HeaderText = "Member Name"
                    Guna2DataGridView1.Columns("Mobile_No").HeaderText = "Mobile No"
                    Guna2DataGridView1.Columns("Aadhar_No").HeaderText = "Aadhar No"
                    Guna2DataGridView1.Columns("Joining_Date").HeaderText = "Joining Date"

                    Guna2DataGridView1.Columns("SNo").Width = 50
                    Guna2DataGridView1.Columns("M_No").Width = 100
                    Guna2DataGridView1.Columns("Joining_Date").Width = 120

                    Guna2DataGridView1.Columns("Joining_Date").DefaultCellStyle.Format = "dd-MM-yyyy"

                    Guna2DataGridView1.Columns("SNo").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Guna2DataGridView1.Columns("M_No").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Guna2DataGridView1.Columns("Joining_Date").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        End Try
    End Sub

    Private Sub Guna2DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles Guna2DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = Guna2DataGridView1.Rows(e.RowIndex)
            If row.Cells("ID").Value IsNot DBNull.Value Then
                PrintMemberID = DirectCast(row.Cells("ID").Value, Guid)
            End If
        End If
    End Sub

    Private Sub Guna2DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles Guna2DataGridView1.CellDoubleClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = Guna2DataGridView1.Rows(e.RowIndex)
            SelectedMemberID = DirectCast(row.Cells("ID").Value, Guid)

            txtMNo.Text = row.Cells("M_No").Value.ToString()
            txtMemberName.Text = row.Cells("Member_Name").Value.ToString()
            txtMobile.Text = row.Cells("Mobile_No").Value.ToString()
            txtAddress.Text = row.Cells("Address_Text").Value.ToString()
            txtAadhar.Text = row.Cells("Aadhar_No").Value.ToString()
            txtRemarks.Text = If(IsDBNull(row.Cells("Remarks").Value), "", row.Cells("Remarks").Value.ToString())
            dtpJoiningDate.Value = Convert.ToDateTime(row.Cells("Joining_Date").Value)

            If Not IsDBNull(row.Cells("Member_Photo").Value) Then
                pbMemberPhoto.Image = GetImageFromBytes(DirectCast(row.Cells("Member_Photo").Value, Byte()))
            Else
                pbMemberPhoto.Image = Nothing
            End If

            IsActiveStatus = Convert.ToBoolean(row.Cells("Is_Active").Value)
            btnInactive.Visible = True
            UpdateStatusButtonUI()
            btnSave.Text = "UPDATE"
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        If dtpJoiningDate.Value.Date < FinStartDate OrElse dtpJoiningDate.Value.Date > FinEndDate Then
            MessageBox.Show("Date is outside current financial period.", "Restricted", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrWhiteSpace(txtMNo.Text) OrElse String.IsNullOrWhiteSpace(txtMemberName.Text) Then
            MessageBox.Show("Required: Member No and Name.")
            Return
        End If

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim sql As String
                If SelectedMemberID = Guid.Empty Then
                    sql = "INSERT INTO Member_Table (ID, M_No, Member_Name, Mobile_No, Address_Text, Aadhar_No, Remarks, Member_Photo, Is_Active, User_ID, Joining_Date) " &
                          "VALUES (NEWID(), @MNo, @Name, @Mobile, @Address, @Aadhar, @Remarks, @Photo, @Active, @UID, @Date)"
                Else
                    sql = "UPDATE Member_Table SET M_No=@MNo, Member_Name=@Name, Mobile_No=@Mobile, Address_Text=@Address, " &
                          "Aadhar_No=@Aadhar, Remarks=@Remarks, Member_Photo=@Photo, Is_Active=@Active, Joining_Date=@Date, Modified_Date=GETDATE() WHERE ID=@ID"
                End If

                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@MNo", txtMNo.Text.Trim())
                    cmd.Parameters.AddWithValue("@Name", txtMemberName.Text.Trim())
                    cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text.Trim())
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim())
                    cmd.Parameters.AddWithValue("@Aadhar", txtAadhar.Text.Trim())
                    cmd.Parameters.AddWithValue("@Remarks", txtRemarks.Text.Trim())
                    cmd.Parameters.AddWithValue("@Active", IsActiveStatus)
                    cmd.Parameters.AddWithValue("@Date", dtpJoiningDate.Value.Date)
                    cmd.Parameters.AddWithValue("@UID", Tools.GetStoredUsername())

                    Dim imgBytes As Byte() = GetPhotoBytes()
                    cmd.Parameters.Add("@Photo", SqlDbType.VarBinary).Value = If(imgBytes IsNot Nothing, imgBytes, DBNull.Value)

                    If SelectedMemberID <> Guid.Empty Then cmd.Parameters.AddWithValue("@ID", SelectedMemberID)
                    cmd.ExecuteNonQuery()
                End Using
            End Using

            MessageBox.Show("Successfully Saved/Updated.")
            LoadMemberData()
            ClearFields()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Sub btnInactive_Click(sender As Object, e As EventArgs) Handles btnInactive.Click
        If SelectedMemberID = Guid.Empty Then Return

        Dim nextStatus As Boolean = Not IsActiveStatus
        Dim msg As String = If(nextStatus, "Activate this member?", "Deactivate this member?")

        If MessageBox.Show(msg, "Confirm", MessageBoxButtons.YesNo) = DialogResult.Yes Then
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim cmd As New SqlCommand("UPDATE Member_Table SET Is_Active=@Act WHERE ID=@ID", conn)
                cmd.Parameters.AddWithValue("@Act", nextStatus)
                cmd.Parameters.AddWithValue("@ID", SelectedMemberID)
                cmd.ExecuteNonQuery()
            End Using
            IsActiveStatus = nextStatus
            UpdateStatusButtonUI()
            LoadMemberData()
        End If
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If PrintMemberID = Guid.Empty Then
            MessageBox.Show("Select a member from grid (Single Click) to print.")
            Return
        End If

        Try
            Dim pd As New PrintDocument()
            pd.DefaultPageSettings.PaperSize = New PaperSize("A4 Half", 827, 585)
            pd.DefaultPageSettings.Margins = New Margins(20, 20, 20, 20)

            AddHandler pd.PrintPage, AddressOf PrintDocument_PrintPage

            Dim dlg As New PrintDialog()
            dlg.Document = pd
            dlg.AllowSomePages = False
            dlg.AllowSelection = False

            If dlg.ShowDialog() = DialogResult.OK Then
                pd.Print()
            End If

        Catch ex As Exception
            MessageBox.Show("Printing failed: " & ex.Message)
        End Try
    End Sub

    Private Sub PrintDocument_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics
        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        Dim mainPen As New Pen(Color.Black, 1.5)
        Dim lightPen As New Pen(Color.LightGray, 1)

        Dim fHeader As New Font("Segoe UI", 18, FontStyle.Bold)
        Dim fSubHeader As New Font("Segoe UI", 10, FontStyle.Regular)
        Dim fLabel As New Font("Segoe UI", 10, FontStyle.Bold)
        Dim fValue As New Font("Segoe UI", 10, FontStyle.Regular)

        Dim primaryBrush As Brush = Brushes.Black
        Dim secondaryBrush As Brush = Brushes.DimGray

        Dim mNo = "", mName = "", mMob = "", mAad = "", mAddr = "", mDate = ""
        Dim mPhoto As Image = Nothing

        Using conn As SqlConnection = Tools.GetConnection()
            conn.Open()
            Dim cmd As New SqlCommand("SELECT * FROM Member_Table WHERE ID=@ID", conn)
            cmd.Parameters.AddWithValue("@ID", PrintMemberID)

            Using rdr = cmd.ExecuteReader()
                If rdr.Read() Then
                    mNo = rdr("M_No").ToString()
                    mName = rdr("Member_Name").ToString().ToUpper()
                    mMob = rdr("Mobile_No").ToString()
                    mAad = rdr("Aadhar_No").ToString()
                    mAddr = rdr("Address_Text").ToString()
                    mDate = Convert.ToDateTime(rdr("Joining_Date")).ToString("dd-MMM-yyyy")

                    If Not IsDBNull(rdr("Member_Photo")) Then
                        mPhoto = GetImageFromBytes(DirectCast(rdr("Member_Photo"), Byte()))
                    End If
                End If
            End Using
        End Using

        Dim cName = "COMPANY NAME", cAddr = "", cMob = ""

        Using conn As SqlConnection = Tools.GetConnection()
            conn.Open()
            Dim cmd As New SqlCommand("SELECT Comp_Name, Comp_Address1, Comp_Address2, Mobile FROM Company_Table WHERE Comp_No='BK0002'", conn)

            Using rdr = cmd.ExecuteReader()
                If rdr.Read() Then
                    cName = rdr("Comp_Name").ToString().ToUpper()
                    cAddr = $"{rdr("Comp_Address1")}, {rdr("Comp_Address2")}"
                    cMob = rdr("Mobile").ToString()
                End If
            End Using
        End Using


        Dim pageWidth As Integer = 800
        Dim margin As Integer = 30

        g.DrawRectangle(mainPen, margin, margin, 740, 520)

        Dim titleWidth = g.MeasureString(cName, fHeader).Width
        g.DrawString(cName, fHeader, primaryBrush, (pageWidth - titleWidth) / 2, 50)

        Dim subWidth = g.MeasureString(cAddr, fSubHeader).Width
        g.DrawString(cAddr, fSubHeader, secondaryBrush, (pageWidth - subWidth) / 2, 85)

        g.DrawLine(lightPen, 50, 120, 750, 120)

        g.DrawRectangle(lightPen, 50, 140, 700, 280)

        g.DrawRectangle(lightPen, 580, 160, 140, 160)
        If mPhoto IsNot Nothing Then
            g.DrawImage(mPhoto, 585, 165, 130, 150)
        End If

        Dim xLabel As Integer = 70
        Dim xValue As Integer = 220
        Dim yStart As Integer = 170
        Dim gap As Integer = 35

        Dim drawRow = Sub(lbl As String, val As String, y As Integer)
                          g.DrawString(lbl, fLabel, primaryBrush, xLabel, y)
                          g.DrawString(val, fValue, primaryBrush, xValue, y)
                      End Sub

        drawRow("Member No", mNo, yStart)
        drawRow("Name", mName, yStart + gap)
        drawRow("Mobile", mMob, yStart + gap * 2)
        drawRow("Join Date", mDate, yStart + gap * 3)

        g.DrawString("Address", fLabel, primaryBrush, xLabel, yStart + gap * 4)
        g.DrawString(mAddr, fValue, primaryBrush,
                 New RectangleF(xValue, yStart + gap * 4, 320, 60))

        g.DrawLine(lightPen, 50, 450, 750, 450)
        g.DrawLine(mainPen, 80, 500, 220, 500)
        g.DrawString("Member Signature", fSubHeader, secondaryBrush, 90, 510)

        g.DrawLine(mainPen, 550, 500, 690, 500)
        g.DrawString("Authorized Signatory", fSubHeader, secondaryBrush, 550, 510)

    End Sub
    Private Sub UpdateStatusButtonUI()
        If IsActiveStatus Then
            btnInactive.Text = "INACTIVE" : btnInactive.FillColor = Color.Red
        Else
            btnInactive.Text = "ACTIVE" : btnInactive.FillColor = Color.Green
        End If
    End Sub

    Private Sub ClearFields()
        SelectedMemberID = Guid.Empty
        txtMNo.Clear() : txtMemberName.Clear() : txtMobile.Clear() : txtAadhar.Clear()
        txtAddress.Clear() : txtRemarks.Clear()
        pbMemberPhoto.Image = Nothing
        IsActiveStatus = True
        btnSave.Text = "SAVE"
        btnInactive.Visible = False
        UpdateStatusButtonUI()
    End Sub

    Private Function GetPhotoBytes() As Byte()
        If pbMemberPhoto.Image Is Nothing Then Return Nothing
        Using ms As New MemoryStream()
            Using bmp As New Bitmap(pbMemberPhoto.Image)
                bmp.Save(ms, ImageFormat.Jpeg)
            End Using
            Return ms.ToArray()
        End Using
    End Function

    Private Function GetImageFromBytes(ByVal bytes As Byte()) As Image
        If bytes Is Nothing OrElse bytes.Length = 0 Then Return Nothing
        Using ms As New MemoryStream(bytes)
            Return Image.FromStream(ms).Clone()
        End Using
    End Function

    Private Sub Guna2DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles Guna2DataGridView1.CellFormatting
        If e.RowIndex >= 0 Then
            Dim row = Guna2DataGridView1.Rows(e.RowIndex)
            If row.Cells("Is_Active").Value IsNot DBNull.Value AndAlso Not CBool(row.Cells("Is_Active").Value) Then
                e.CellStyle.ForeColor = Color.Red
            End If
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Using ofd As New OpenFileDialog()
            If ofd.ShowDialog() = DialogResult.OK Then
                Using tempImg = Image.FromFile(ofd.FileName)
                    pbMemberPhoto.Image = New Bitmap(tempImg)
                End Using
            End If
        End Using
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles btnClear.Click
        ClearFields()
    End Sub

    Private Sub txtMobile_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtMobile.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    Private Sub txtMobile_TextChanged(sender As Object, e As EventArgs) Handles txtMobile.TextChanged

        Dim digitsOnly As String = ""
        For Each c As Char In txtMobile.Text
            If Char.IsDigit(c) Then
                digitsOnly &= c
            End If
        Next

        If digitsOnly.Length > 10 Then
            digitsOnly = digitsOnly.Substring(0, 10)
        End If

        If txtMobile.Text <> digitsOnly Then
            txtMobile.Text = digitsOnly
            txtMobile.SelectionStart = txtMobile.Text.Length
        End If
    End Sub
    Private Sub txtAadhar_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAadhar.KeyPress
        If Not Char.IsDigit(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub
    Private Sub txtAadhar_TextChanged(sender As Object, e As EventArgs) Handles txtAadhar.TextChanged
        Dim digitsOnly As String = ""
        For Each c As Char In txtAadhar.Text
            If Char.IsDigit(c) Then
                digitsOnly &= c
            End If
        Next

        If digitsOnly.Length > 12 Then
            digitsOnly = digitsOnly.Substring(0, 12)
        End If

        If txtAadhar.Text <> digitsOnly Then
            txtAadhar.Text = digitsOnly
            txtAadhar.SelectionStart = txtAadhar.Text.Length
        End If
    End Sub

    Private Sub DownloadButton_Click(sender As Object, e As EventArgs) Handles DownloadButton.Click

        If pbMemberPhoto.Image Is Nothing Then
            MessageBox.Show("No photo available to download.", "Download", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Using sfd As New SaveFileDialog()
            sfd.Title = "Download Member Photo"
            Dim safeName As String = txtMemberName.Text.Trim().Replace(" ", "_")
            sfd.FileName = If(safeName <> "", safeName, "MemberPhoto")

            sfd.Filter = "JPEG Image|*.jpg|PNG Image|*.png|Bitmap Image|*.bmp"

            If sfd.ShowDialog() = DialogResult.OK Then
                Try
                    Dim format As ImageFormat = ImageFormat.Jpeg
                    Select Case Path.GetExtension(sfd.FileName).ToLower()
                        Case ".png"
                            format = ImageFormat.Png
                        Case ".bmp"
                            format = ImageFormat.Bmp
                        Case ".jpg", ".jpeg"
                            format = ImageFormat.Jpeg
                    End Select

                    Using tempBitmap As New Bitmap(pbMemberPhoto.Image)
                        tempBitmap.Save(sfd.FileName, format)
                    End Using

                    MessageBox.Show("Photo downloaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                Catch ex As Exception
                    MessageBox.Show("Failed to save image: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Sub ClearButton_Click_1(sender As Object, e As EventArgs) Handles ClearButton.Click
        ClearFields()
    End Sub

End Class
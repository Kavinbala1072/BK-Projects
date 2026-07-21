Imports System.Data.SqlClient
Imports System.Drawing.Printing

Public Class Voucher

    Private SelectedVoucherID As Guid = Guid.Empty
    Private LinkedMemberID As Guid = Guid.Empty
    Private PrintVoucherID As Guid = Guid.Empty

    Private IsRecordCancelled As Integer = 0
    Private FinStartDate As DateTime
    Private FinEndDate As DateTime

    Private Sub Voucher_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FromDate.Value = DateTime.Today
        ToDate.Value = DateTime.Today
        SetupCombos()
        InitializeDesign()
        LoadFinancialPeriod()
        ClearFields()
        LoadVoucherData()
        LoadPaymentAccounts()

        Guna2ToggleSwitch1.Checked = False
        Guna2HtmlLabel2.Text = "MEMBER NO"
        Guna2HtmlLabel2.ForeColor = Color.Red
        btnCancel.Visible = False
    End Sub

    Private Sub Guna2ToggleSwitch1_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2ToggleSwitch1.CheckedChanged
        If Guna2ToggleSwitch1.Checked Then
            Guna2HtmlLabel2.Text = "UNMEMBER"
            Guna2HtmlLabel2.ForeColor = Color.Green

            txtMNo.Text = "0"
            txtMNo.Enabled = False

            txtMemberName.ReadOnly = False
            txtMemberName.Enabled = True
            txtMemberName.Clear()
            txtMemberName.Focus()
        Else
            Guna2HtmlLabel2.Text = "MEMBER NO"
            Guna2HtmlLabel2.ForeColor = Color.Red

            txtMNo.Enabled = True
            txtMNo.Clear()
            txtMemberName.ReadOnly = True
            txtMemberName.Enabled = False
        End If
    End Sub
    Private Function GetUnMemberID() As Guid
        Dim tempID As Guid = Guid.Empty
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim sql As String = "SELECT ID FROM Member_Table WHERE M_No = '0'"
                Using cmd As New SqlCommand(sql, conn)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then tempID = DirectCast(result, Guid)
                End Using
            End Using
        Catch ex As Exception
        End Try
        Return tempID
    End Function
    Private Sub SetupCombos()
        TypeCombo.Items.Clear()
        TypeCombo.Items.AddRange({"RECEIPT", "VOUCHER"})
        TypeCombo.StartIndex = 0

        PaymentCombo.Items.Clear()
        PaymentCombo.Items.AddRange({"Cash", "Phone Pay", "Google Pay", "NEFT", "Cheque"})
        PaymentCombo.StartIndex = 0

        DTypeCombo.Items.Clear()
        DTypeCombo.Items.AddRange({"ALL", "RECEIPT", "VOUCHER"})
        DTypeCombo.StartIndex = 0
    End Sub
    Private Sub TypeCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TypeCombo.SelectedIndexChanged
        LoadAutoBillNo()
        PurposeCombo.Items.Clear()

        If TypeCombo.Text = "RECEIPT" Then
            PurposeCombo.Items.AddRange({"Member Ship", "Donation", "Maintenance", "Event Fee", "Other"})
        ElseIf TypeCombo.Text = "VOUCHER" Then
            PurposeCombo.Items.AddRange({"Maintenance", "Event Fee", "Other"})
        End If

        If PurposeCombo.Items.Count > 0 Then
            PurposeCombo.StartIndex = 0
        End If
    End Sub

    Private Sub LoadPaymentAccounts()
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                Dim query As String = "SELECT ID, Partyname FROM Ledger_Table WHERE Active = 0 ORDER BY Partyname ASC"
                Dim adapter As New SqlDataAdapter(query, conn)
                Dim dt As New DataTable()
                adapter.Fill(dt)

                AccountCombo.DataSource = dt
                AccountCombo.DisplayMember = "Partyname"
                AccountCombo.ValueMember = "ID"
                'AccountCombo.SelectedIndex = 0
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading accounts: " & ex.Message)
        End Try
    End Sub

    Private Sub InitializeDesign()
        Dim dtps As Guna.UI2.WinForms.Guna2DateTimePicker() = {dtpJoiningDate, FromDate, ToDate}
        For Each dtp In dtps
            dtp.Format = DateTimePickerFormat.Custom
            dtp.CustomFormat = "dd-MM-yyyy"
            dtp.FillColor = Color.White
            dtp.ForeColor = Color.Black
            dtp.CheckedState.FillColor = Color.FromArgb(255, 128, 64)
            dtp.CheckedState.ForeColor = Color.White
        Next

        With Guna2DataGridView1
            ' --- Existing Properties ---
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

    Private Sub LoadAutoBillNo()

        If SelectedVoucherID <> Guid.Empty Then Exit Sub

        Dim dbType As String = If(TypeCombo.Text = "VOUCHER", "PAYMENT", "RECEIPT")

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim sql As String = "SELECT Vt_Prefix, Vt_Suffix, Vt_Billno FROM v_table WHERE Vt_Name = @Type"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@Type", dbType)
                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        If rdr.Read() Then
                            Dim prefix As String = rdr("Vt_Prefix").ToString()
                            Dim suffix As String = rdr("Vt_Suffix").ToString()
                            Dim billNo As String = rdr("Vt_Billno").ToString()
                            txtBillNo.Text = prefix & billNo & suffix
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            ' Log error if needed
        End Try
    End Sub

    Private Sub txtMNo_TextChanged(sender As Object, e As EventArgs) Handles txtMNo.TextChanged
        If txtMNo.Text.Trim().Length > 0 Then
            GetMemberDetails(txtMNo.Text.Trim())
        Else
            LinkedMemberID = Guid.Empty
            txtMemberName.Clear()
        End If
    End Sub

    Private Sub GetMemberDetails(ByVal mNo As String)
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim sql As String = "SELECT ID, Member_Name FROM Member_Table WHERE M_No = @MNo"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@MNo", mNo)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            LinkedMemberID = DirectCast(reader("ID"), Guid)
                            txtMemberName.Text = reader("Member_Name").ToString()
                            txtMemberName.ForeColor = Color.Black
                        Else
                            LinkedMemberID = Guid.Empty
                            txtMemberName.Text = "NOT FOUND"
                            txtMemberName.ForeColor = Color.Red
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            LinkedMemberID = Guid.Empty
        End Try
    End Sub

    Private Sub Guna2DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles Guna2DataGridView1.CellClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = Guna2DataGridView1.Rows(e.RowIndex)
            If row.Cells("ID").Value IsNot DBNull.Value Then
                PrintVoucherID = DirectCast(row.Cells("ID").Value, Guid)
            End If
        End If
    End Sub

    Private Sub Guna2DataGridView1_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles Guna2DataGridView1.CellDoubleClick
        If e.RowIndex >= 0 Then
            Dim row As DataGridViewRow = Guna2DataGridView1.Rows(e.RowIndex)
            SelectedVoucherID = DirectCast(row.Cells("ID").Value, Guid)
            IsRecordCancelled = Convert.ToInt32(row.Cells("Is_Cancelled").Value)

            dtpJoiningDate.Value = Convert.ToDateTime(row.Cells("V_Date").Value)
            txtBillNo.Text = row.Cells("Bill_No").Value.ToString()
            TypeCombo.Text = row.Cells("V_Type").Value.ToString()
            txtMNo.Text = row.Cells("M_No").Value.ToString()
            txtAmount.Text = row.Cells("Amount").Value.ToString()
            PurposeCombo.Text = row.Cells("Purpose").Value.ToString()
            PaymentCombo.Text = row.Cells("Payment_Method").Value.ToString()
            txtRemarks.Text = row.Cells("Remarks").Value.ToString()

            If IsRecordCancelled = 1 Then
                btnSave.Enabled = False
                btnSave.Text = "CANCELLED"
                btnSave.FillColor = Color.Gray
            Else
                btnSave.Enabled = True
                btnSave.Text = "UPDATE"
                btnSave.FillColor = Color.Green
                'btnCancel.Text = "Cancel Bill"
            End If
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        'Dim entryDate As DateTime = dtpJoiningDate.Value.Date
        'If entryDate < FinStartDate OrElse entryDate > FinEndDate Then
        '    MessageBox.Show($"Access Denied: The date must be within the financial period ({FinStartDate:dd-MM-yyyy} to {FinEndDate:dd-MM-yyyy})", "Date Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop)
        '    Return
        'End If

        'Dim finalMemberID As Guid = Guid.Empty
        'Dim finalMemberName As String = txtMemberName.Text.Trim()

        'If Guna2ToggleSwitch1.Checked Then
        '    ' UNMEMBER MODE: Get the ID for M_No '0'
        '    finalMemberID = GetUnMemberID()
        '    If finalMemberID = Guid.Empty Then
        '        MessageBox.Show("Error: UnMember record (M_No: 0) not found in Member_Table. Please create it first.", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        '        Return
        '    End If
        '    ' Validation: UnMember must have a name typed
        '    If String.IsNullOrWhiteSpace(finalMemberName) Or finalMemberName = "NOT FOUND" Then
        '        MessageBox.Show("Please enter the UnMember's name.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '        Return
        '    End If
        'Else
        '    ' MEMBER MODE: Use the ID found during M_No lookup
        '    finalMemberID = LinkedMemberID
        '    If finalMemberID = Guid.Empty Then
        '        MessageBox.Show("Please enter a valid Member Number.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '        Return
        '    End If
        'End If

        'If String.IsNullOrWhiteSpace(txtBillNo.Text) Then
        '    MessageBox.Show("Bill Number is required.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    Return
        'End If

        If String.IsNullOrWhiteSpace(txtBillNo.Text) Then
            MessageBox.Show("Bill Number is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtBillNo.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(txtMemberName.Text) OrElse txtMemberName.Text = "NOT FOUND" Then
            MessageBox.Show("A valid Member or UnMember name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If String.IsNullOrWhiteSpace(PurposeCombo.Text) Then
            MessageBox.Show("Please select a Purpose.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            PurposeCombo.Focus()
            Return
        End If

        If String.IsNullOrWhiteSpace(PaymentCombo.Text) Then
            MessageBox.Show("Please select a Payment Method.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            PaymentCombo.Focus()
            Return
        End If

        If AccountCombo.SelectedValue Is Nothing Then
            MessageBox.Show("Please select a Payment Account/Ledger.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            AccountCombo.Focus()
            Return
        End If

        Dim dblAmount As Decimal = 0

        If String.IsNullOrWhiteSpace(txtAmount.Text) Then
            MessageBox.Show("Amount cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtAmount.Focus()
            Return
        End If

        If Not Decimal.TryParse(txtAmount.Text, dblAmount) Then
            MessageBox.Show("Please enter a valid numeric amount.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtAmount.Focus()
            Return
        End If

        If dblAmount <= 0 Then
            MessageBox.Show("Amount must be greater than zero.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            txtAmount.Focus()
            Return
        End If

        If dblAmount > 10000000 Then
            MessageBox.Show("Transaction restricted! Amount cannot exceed 1,00,00,000 (10 Million).", "Limit Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            txtAmount.Focus()
            Return
        End If

        Dim entryDate As DateTime = dtpJoiningDate.Value.Date
        If entryDate < FinStartDate OrElse entryDate > FinEndDate Then
            MessageBox.Show($"Access Denied: Date must be within financial period ({FinStartDate:dd-MM-yyyy} to {FinEndDate:dd-MM-yyyy})", "Date Restriction", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Return
        End If

        Dim finalMemberID As Guid = Guid.Empty
        Dim finalMemberName As String = txtMemberName.Text.Trim()

        If Guna2ToggleSwitch1.Checked Then
            finalMemberID = GetUnMemberID()
            If finalMemberID = Guid.Empty Then
                MessageBox.Show("System Error: UnMember record (M_No: 0) missing in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
        Else
            finalMemberID = LinkedMemberID
            If finalMemberID = Guid.Empty Then
                MessageBox.Show("Please enter a valid Member Number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If
        End If

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim sql As String
                Dim selectedLedgerID As Object = If(AccountCombo.SelectedValue Is Nothing, DBNull.Value, AccountCombo.SelectedValue)

                If SelectedVoucherID = Guid.Empty Then
                    sql = "INSERT INTO Voucher_Table (ID, Bill_No, V_Date, V_Type, Member_ID, Member_Name, Amount, Purpose, Payment_Method, Remarks, User_ID, Is_Cancelled, Ledger_ID, Created_Date) " &
                      "VALUES (NEWID(), @Bill, @Date, @Type, @MID, @MName, @Amt, @Purpose, @Method, @Rem, @UID, 0, @LedgerID, GETDATE())"
                Else
                    sql = "UPDATE Voucher_Table SET Bill_No=@Bill, V_Date=@Date, V_Type=@Type, Member_ID=@MID, Member_Name=@MName, " &
                      "Amount=@Amt, Purpose=@Purpose, Payment_Method=@Method, Remarks=@Rem, Ledger_ID=@LedgerID, Modified_Date=GETDATE() WHERE ID=@ID"
                End If

                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@Bill", txtBillNo.Text.Trim())
                    cmd.Parameters.AddWithValue("@Date", entryDate)
                    cmd.Parameters.AddWithValue("@Type", TypeCombo.Text)
                    cmd.Parameters.AddWithValue("@MID", finalMemberID)
                    cmd.Parameters.AddWithValue("@MName", finalMemberName)
                    cmd.Parameters.AddWithValue("@Amt", Decimal.Parse(If(txtAmount.Text = "", "0", txtAmount.Text)))
                    cmd.Parameters.AddWithValue("@Purpose", PurposeCombo.Text)
                    cmd.Parameters.AddWithValue("@Method", PaymentCombo.Text)
                    cmd.Parameters.AddWithValue("@Rem", txtRemarks.Text.Trim())
                    cmd.Parameters.AddWithValue("@UID", Tools.GetStoredUsername())
                    cmd.Parameters.AddWithValue("@LedgerID", selectedLedgerID)

                    If SelectedVoucherID <> Guid.Empty Then
                        cmd.Parameters.AddWithValue("@ID", SelectedVoucherID)
                    End If

                    cmd.ExecuteNonQuery()
                End Using

                If SelectedVoucherID = Guid.Empty Then
                    Dim dbType As String = If(TypeCombo.Text = "VOUCHER", "PAYMENT", "RECEIPT")
                    Using upCmd As New SqlCommand("UPDATE v_table SET Vt_Billno = Vt_Billno + 1 WHERE Vt_Name = @Type", conn)
                        upCmd.Parameters.AddWithValue("@Type", dbType)
                        upCmd.ExecuteNonQuery()
                    End Using
                End If
            End Using

            MessageBox.Show("Transaction saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ClearFields()
            LoadVoucherData()

        Catch ex As Exception
            MessageBox.Show("Save Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        If PrintVoucherID = Guid.Empty Then
            MessageBox.Show("Click once on a bill in the grid to select it for printing.")
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

        Dim fHeader As New Font("Segoe UI", 18, FontStyle.Bold)
        Dim fTitle As New Font("Segoe UI", 12, FontStyle.Bold)
        Dim fLabel As New Font("Segoe UI", 10, FontStyle.Bold)
        Dim fText As New Font("Segoe UI", 10, FontStyle.Regular)

        Dim mainPen As New Pen(Color.Black, 1.5)
        Dim lightPen As New Pen(Color.LightGray, 1)
        Dim primaryBrush As Brush = Brushes.Black
        Dim secondaryBrush As Brush = Brushes.DimGray

        Dim vNo = "", vDate = "", vType = "", mName = "", mNo_ = ""
        Dim vAmt As Decimal = 0, vPurp = "", vMeth = "", vRem = "", vStat = 0

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim sql = "SELECT V.*, M.Member_Name, M.M_No FROM Voucher_Table V LEFT JOIN Member_Table M ON V.Member_ID = M.ID WHERE V.ID = @ID"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@ID", PrintVoucherID)
                    Using rdr = cmd.ExecuteReader()
                        If rdr.Read() Then
                            vNo = rdr("Bill_No").ToString()
                            vDate = Convert.ToDateTime(rdr("V_Date")).ToString("dd-MMM-yyyy")
                            vType = rdr("V_Type").ToString()
                            mName = rdr("Member_Name").ToString().ToUpper()
                            mNo_ = rdr("M_No").ToString()
                            vAmt = Convert.ToDecimal(rdr("Amount"))
                            vPurp = rdr("Purpose").ToString()
                            vMeth = rdr("Payment_Method").ToString()
                            vRem = rdr("Remarks").ToString()
                            vStat = Convert.ToInt32(rdr("Is_Cancelled"))
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
        End Try

        Dim cName = "COMPANY NAME", cAddr = "", cMob = ""

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim cmd = New SqlCommand("SELECT Comp_Name, Comp_Address1, Mobile FROM Company_Table WHERE Comp_No='BK0002'", conn)
                Using rdr = cmd.ExecuteReader()
                    If rdr.Read() Then
                        cName = rdr("Comp_Name").ToString().ToUpper()
                        cAddr = rdr("Comp_Address1").ToString()
                        cMob = rdr("Mobile").ToString()
                    End If
                End Using
            End Using
        Catch ex As Exception
        End Try

        Dim margin As Integer = 30
        Dim width As Integer = 740

        g.DrawRectangle(mainPen, margin, margin, width, 520)

        Dim centerX = 400
        g.DrawString(cName, fHeader, primaryBrush, centerX - g.MeasureString(cName, fHeader).Width / 2, 45)
        g.DrawString(cAddr & " | Mob: " & cMob, fText, secondaryBrush,
                 centerX - g.MeasureString(cAddr & " | Mob: " & cMob, fText).Width / 2, 80)

        g.DrawLine(lightPen, 50, 120, 750, 120)

        Dim titleText = If(vType = "RECEIPT", "RECEIPT VOUCHER", "PAYMENT VOUCHER")
        If vStat = 1 Then titleText &= " (CANCELLED)"

        g.DrawString(titleText, fTitle, primaryBrush,
                 centerX - g.MeasureString(titleText, fTitle).Width / 2, 135)

        g.DrawRectangle(lightPen, 50, 170, 700, 220)

        Dim xL = 70
        Dim xR = 420
        Dim y = 190
        Dim gap = 35

        Dim drawRow = Sub(lbl As String, val As String, x As Integer, yy As Integer)
                          g.DrawString(lbl, fLabel, primaryBrush, x, yy)
                          g.DrawString(val, fText, primaryBrush, x + 110, yy)
                      End Sub

        drawRow("Bill No", vNo, xL, y)
        drawRow("Date", vDate, xR, y)

        y += gap
        drawRow("Member", mName & " (" & mNo_ & ")", xL, y)

        y += gap
        drawRow("Purpose", vPurp, xL, y)
        drawRow("Method", vMeth, xR, y)

        y += gap
        g.DrawString("Remarks", fLabel, primaryBrush, xL, y)
        g.DrawString(vRem, fText, primaryBrush,
                 New RectangleF(xL + 110, y, 520, 60))

        Dim amtY = 410
        g.FillRectangle(New SolidBrush(Color.FromArgb(230, 240, 255)), 50, amtY, 300, 50)
        g.DrawRectangle(mainPen, 50, amtY, 300, 50)

        g.DrawString("TOTAL AMOUNT", fLabel, Brushes.DarkBlue, 65, amtY + 5)
        g.DrawString("Rs. " & vAmt.ToString("N2"),
                 New Font("Segoe UI", 14, FontStyle.Bold),
                 Brushes.DarkBlue, 65, amtY + 22)

        g.DrawLine(lightPen, 50, 480, 750, 480)

        g.DrawLine(mainPen, 80, 520, 220, 520)
        g.DrawString("Receiver Signature", fText, secondaryBrush, 85, 530)

        g.DrawLine(mainPen, 550, 520, 690, 520)
        g.DrawString("Authorized Signatory", fText, secondaryBrush, 540, 530)

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
        End Try
    End Sub

    Private Sub LoadVoucherData()
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                Dim query As String = "SELECT V.ID, V.V_Date, V.Bill_No, V.V_Type, M.M_No, " &
             "ISNULL(V.Member_Name, M.Member_Name) as Member_Name, " & ' Use Voucher table name first
             "V.Amount, V.Is_Cancelled, V.Remarks, V.Purpose, V.Payment_Method " &
             "FROM Voucher_Table V " &
             "LEFT JOIN Member_Table M ON V.Member_ID = M.ID " &
             "WHERE V.V_Date BETWEEN @From AND @To ORDER BY V.Created_Date DESC"

                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@From", FromDate.Value.Date)
                cmd.Parameters.AddWithValue("@To", ToDate.Value.Date)

                Dim adapter As New SqlDataAdapter(cmd)
                Dim dt As New DataTable()
                adapter.Fill(dt)

                dt.Columns.Add("SNo", GetType(Integer))

                dt.Columns("SNo").SetOrdinal(0)

                For i As Integer = 0 To dt.Rows.Count - 1
                    dt.Rows(i)("SNo") = i + 1
                Next

                Guna2DataGridView1.DataSource = dt

                If Guna2DataGridView1.Columns.Count > 0 Then
                    Guna2DataGridView1.Columns("ID").Visible = False
                    Guna2DataGridView1.Columns("Is_Cancelled").Visible = False
                    Guna2DataGridView1.Columns("Remarks").Visible = False

                    Guna2DataGridView1.Columns("SNo").HeaderText = "S.No"
                    Guna2DataGridView1.Columns("V_Date").HeaderText = "Date"
                    Guna2DataGridView1.Columns("Bill_No").HeaderText = "Bill No"
                    Guna2DataGridView1.Columns("V_Type").HeaderText = "Type"
                    Guna2DataGridView1.Columns("M_No").HeaderText = "Member No"
                    Guna2DataGridView1.Columns("Member_Name").HeaderText = "Member Name"
                    Guna2DataGridView1.Columns("Amount").HeaderText = "Amount"
                    Guna2DataGridView1.Columns("Purpose").HeaderText = "Purpose"
                    Guna2DataGridView1.Columns("Payment_Method").HeaderText = "Method"

                    Guna2DataGridView1.Columns("SNo").Width = 50
                    Guna2DataGridView1.Columns("SNo").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                    Guna2DataGridView1.Columns("V_Date").DefaultCellStyle.Format = "dd-MM-yyyy"

                    Guna2DataGridView1.Columns("Amount").DefaultCellStyle.Format = "N2"
                    Guna2DataGridView1.Columns("Amount").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                    Guna2DataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                    Guna2DataGridView1.Columns("V_Type").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                    Guna2DataGridView1.Columns("Bill_No").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End If
            End Using
        Catch ex As Exception
            ' Optional: MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    'Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
    '    If SelectedVoucherID = Guid.Empty Then
    '        ClearFields() : Return
    '    End If

    '    If MessageBox.Show("Cancel this bill permanently?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
    '        Try
    '            Using conn As SqlConnection = Tools.GetConnection()
    '                conn.Open()
    '                Dim sql As String = "UPDATE Voucher_Table SET Is_Cancelled = 1, Modified_Date = GETDATE() WHERE ID = @ID"
    '                Using cmd As New SqlCommand(sql, conn)
    '                    cmd.Parameters.AddWithValue("@ID", SelectedVoucherID)
    '                    cmd.ExecuteNonQuery()
    '                End Using
    '            End Using
    '            ClearFields()
    '            LoadVoucherData()
    '        Catch ex As Exception
    '        End Try
    '    End If
    'End Sub

    Private Sub Guna2DataGridView1_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles Guna2DataGridView1.CellFormatting
        If e.RowIndex >= 0 Then
            Dim row = Guna2DataGridView1.Rows(e.RowIndex)
            If row.Cells("Is_Cancelled").Value IsNot DBNull.Value AndAlso Convert.ToInt32(row.Cells("Is_Cancelled").Value) = 1 Then
                e.CellStyle.ForeColor = Color.Red
            End If
        End If
    End Sub

    Private Sub ClearFields()
        SelectedVoucherID = Guid.Empty : LinkedMemberID = Guid.Empty : IsRecordCancelled = 0
        dtpJoiningDate.Value = DateTime.Today : txtMNo.Clear() : txtMemberName.Clear() : txtAmount.Clear() : txtRemarks.Clear()
        TypeCombo.StartIndex = 0
        btnSave.Text = "SAVE" : btnSave.Enabled = True : btnSave.FillColor = Color.FromArgb(0, 128, 0)
        'btnCancel.Text = "Clear"
        LoadAutoBillNo()
        txtMNo.Focus()
        'AccountCombo.SelectedIndex = 0
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        LoadVoucherData()
    End Sub

    Private Sub txtAmount_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtAmount.KeyPress
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso (e.KeyChar <> "."c) Then
            e.Handled = True
        End If
    End Sub

    Private Sub AddBankButton_Click(sender As Object, e As EventArgs) Handles AddBankButton.Click
        LedgerForm.Location = New Point(
        (Me.Width - LedgerForm.Width) / 2,
        (Me.Height - LedgerForm.Height) / 2
    )

        LedgerForm.Visible = True
        LedgerForm.BringToFront()
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        ClearFields()
    End Sub
End Class
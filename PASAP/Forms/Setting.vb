Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.IO
Imports Guna.UI2.WinForms
Imports System.Security.Cryptography
Imports System.Text

Public Class Setting

    Private Const COMP_ID As String = "BK0002"

    Private Sub Setting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CompanyLoad()
        LoadFinancialPeriod()
        NumberLoad()
        'UserRight()
        'LoadUsernames()
        SetupUserTree()
        InitializeUserSecurity()
        CBUserright.ForeColor = Color.Black
        CBEBackup.ForeColor = Color.Black
        settingload()
    End Sub
    Private Sub settingload()
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'UserRight'", sqlconnect)
                Using reader As SqlDataReader = Query.ExecuteReader()
                    If reader.Read() Then
                        CBUserright.Checked = (reader("Ctl_Value").ToString() = "1")
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'EnableBackup'", sqlconnect)
                Using reader As SqlDataReader = Query.ExecuteReader()
                    If reader.Read() Then
                        CBEBackup.Checked = (reader("Ctl_Value").ToString() = "1")
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        Using conn As SqlConnection = Tools.GetConnection()
            Try
                conn.Open()
                Dim sql As String = "
                    IF EXISTS (SELECT 1 FROM Company_Table WHERE Comp_No = @CompNo)
                    BEGIN
                        UPDATE Company_Table SET Comp_Name=@Name, Comp_Address1=@A1, Comp_Address2=@A2, Comp_Address3=@A3, Mobile=@Mob 
                        WHERE Comp_No = @CompNo
                    END
                    ELSE
                    BEGIN
                        INSERT INTO Company_Table (Comp_No, Comp_Name, Comp_Address1, Comp_Address2, Comp_Address3, Mobile) 
                        VALUES (@CompNo, @Name, @A1, @A2, @A3, @Mob)
                    END"

                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@CompNo", COMP_ID)
                    cmd.Parameters.AddWithValue("@Name", CompanyNameText.Text.Trim())
                    cmd.Parameters.AddWithValue("@A1", Address1TextBox.Text.Trim())
                    cmd.Parameters.AddWithValue("@A2", Address2TextBox.Text.Trim())
                    cmd.Parameters.AddWithValue("@A3", Address3TextBox.Text.Trim())
                    cmd.Parameters.AddWithValue("@Mob", MobileTextBox.Text.Trim())
                    cmd.ExecuteNonQuery()
                End Using

                MessageBox.Show("Company details saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Error saving company details: " & ex.Message)
            End Try
        End Using

        Dim CBUser As Boolean = CBUserright.Checked

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                Dim UserRightCommand As New SqlCommand("UPDATE Control_Table SET Ctl_Value = @UserRight WHERE Ctl_Desc = 'UserRight'", sqlconnect)
                UserRightCommand.Parameters.AddWithValue("@UserRight", CBUser)
                UserRightCommand.ExecuteNonQuery()
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using


        Dim CBBackup As Boolean = CBEBackup.Checked

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                Dim sql As String = "UPDATE Control_Table SET Ctl_Value = @Backup WHERE Ctl_Desc = 'EnableBackup'"
                Using BackupCommand As New SqlCommand(sql, sqlconnect)
                    BackupCommand.Parameters.Add("@Backup", SqlDbType.Bit).Value = CBBackup
                    BackupCommand.ExecuteNonQuery()
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        Refresh()
    End Sub

    Private Sub CompanyLoad()
        Using conn As SqlConnection = Tools.GetConnection()
            Try
                conn.Open()
                Dim sql As String = "SELECT Comp_Name, Comp_Address1, Comp_Address2, Comp_Address3, Mobile FROM Company_Table WHERE Comp_No = @CompNo"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@CompNo", COMP_ID)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            CompanyNameText.Text = reader("Comp_Name").ToString()
                            Address1TextBox.Text = reader("Comp_Address1").ToString()
                            Address2TextBox.Text = reader("Comp_Address2").ToString()
                            Address3TextBox.Text = reader("Comp_Address3").ToString()
                            MobileTextBox.Text = reader("Mobile").ToString()
                        End If
                    End Using
                End Using
            Catch ex As Exception
                MessageBox.Show("Error loading company: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub LoadFinancialPeriod()
        Try
            Tools.LoadConfiguration()
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim sql As String = "SELECT Ctl_Desc, Ctl_Value FROM Control_Table WHERE Ctl_Desc IN ('fromDate', 'toDate')"
                Using cmd As New SqlCommand(sql, conn)
                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        While rdr.Read()
                            Dim desc As String = rdr("Ctl_Desc").ToString()
                            Dim val As String = rdr("Ctl_Value").ToString()

                            If desc = "fromDate" Then Periodstart.Text = val
                            If desc = "toDate" Then PeriodEnd.Text = val
                        End While
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading period: " & ex.Message)
        End Try
    End Sub

    Private Sub PeriodChange_Click(sender As Object, e As EventArgs) Handles PeriodChange.Click
        If String.IsNullOrWhiteSpace(Periodstart.Text) OrElse String.IsNullOrWhiteSpace(PeriodEnd.Text) Then
            MessageBox.Show("Please enter valid dates.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim sql As String = "
                    UPDATE Control_Table SET Ctl_Value = @fd WHERE Ctl_Desc = 'fromDate';
                    UPDATE Control_Table SET Ctl_Value = @td WHERE Ctl_Desc = 'toDate';"

                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@fd", Periodstart.Text)
                    cmd.Parameters.AddWithValue("@td", PeriodEnd.Text)
                    cmd.ExecuteNonQuery()
                End Using
                MessageBox.Show("Financial period updated successfully.")
            End Using
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub
    Private Sub ExecuteBtn_Click(sender As Object, e As EventArgs) Handles ExecuteBtn.Click
        Dim query As String = ExecuteText.Text.Trim()
        If String.IsNullOrEmpty(query) Then Return

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Using cmd As New SqlCommand(query, conn)
                    If query.ToUpper().StartsWith("SELECT") Then
                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            Dim sb As New Text.StringBuilder()
                            While reader.Read()
                                For i As Integer = 0 To reader.FieldCount - 1
                                    sb.Append(reader.GetName(i) & ": " & reader(i).ToString() & " | ")
                                Next
                                sb.AppendLine()
                            End While
                            MessageBox.Show(If(sb.Length > 0, sb.ToString(), "No results."), "Query Result")
                        End Using
                    Else
                        Dim rows As Integer = cmd.ExecuteNonQuery()
                        MessageBox.Show(rows & " row(s) affected.", "Success")
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("SQL Error: " & ex.Message)
        End Try
    End Sub

    Private Sub SaveBillnoButton_Click(sender As Object, e As EventArgs) Handles SaveBillnoButton.Click
        Using conn As SqlConnection = Tools.GetConnection()
            Try
                conn.Open()
                UpdateBillPrefixSuffix(conn, "RECEIPT", ReceiptNO.Text, ReceiptPrefix.Text, ReceiptSuffix.Text)
                UpdateBillPrefixSuffix(conn, "PAYMENT", PaymentNo.Text, PaymentPrefix.Text, PaymentSuffix.Text)

                Dim updateCmd As New SqlCommand("UPDATE Control_Table SET Ctl_Value = @NewDate WHERE Ctl_Desc = 'LastNoUpdated'", conn)
                updateCmd.Parameters.AddWithValue("@NewDate", DateTime.Now.ToString("dd-MM-yyyy"))
                updateCmd.ExecuteNonQuery()

                MessageBox.Show("Bill numbering updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Error updating Bill No: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub UpdateBillPrefixSuffix(conn As SqlConnection, BillType As String, BillNo As String, Prefix As String, Suffix As String)
        Dim sql As String = "UPDATE v_table SET Vt_Prefix = @Prefix, Vt_Suffix = @Suffix, Vt_Billno = @No WHERE Vt_Name = @Type"
        Using cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@No", BillNo)
            cmd.Parameters.AddWithValue("@Prefix", Prefix)
            cmd.Parameters.AddWithValue("@Suffix", Suffix)
            cmd.Parameters.AddWithValue("@Type", BillType)
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub NumberLoad()
        Using conn As SqlConnection = Tools.GetConnection()
            Try
                conn.Open()
                LoadSingleNumberType(conn, "RECEIPT", ReceiptNO, ReceiptPrefix, ReceiptSuffix)
                LoadSingleNumberType(conn, "PAYMENT", PaymentNo, PaymentPrefix, PaymentSuffix)
            Catch ex As Exception
                MessageBox.Show("Error loading numbers: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub LoadSingleNumberType(conn As SqlConnection, typeName As String, txtNo As Guna2TextBox, txtPre As Guna2TextBox, txtSuf As Guna2TextBox)
        Dim sql As String = "SELECT Vt_Prefix, Vt_Suffix, Vt_Billno FROM v_table WHERE Vt_Name = @Name"
        Using cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@Name", typeName)
            Using reader As SqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    txtNo.Text = reader("Vt_Billno").ToString()
                    txtPre.Text = reader("Vt_Prefix").ToString()
                    txtSuf.Text = reader("Vt_Suffix").ToString()
                End If
            End Using
        End Using
    End Sub

    Private Function HashPassword(password As String) As String
        Using sha As SHA256 = SHA256.Create()
            Dim bytes = Encoding.UTF8.GetBytes(password)
            Dim hash = sha.ComputeHash(bytes)
            Return Convert.ToBase64String(hash)
        End Using
    End Function
    Private Sub Userbtn_Click(sender As Object, e As EventArgs) Handles Userbtn.Click
        Dim Username As String = Usertxt.Text.Trim()
        Dim enteredPassword As String = Psdtxt.Text.Trim()

        If String.IsNullOrWhiteSpace(Username) Then
            MessageBox.Show("User name empty.")
            Return
        End If

        If String.IsNullOrWhiteSpace(enteredPassword) Then
            MessageBox.Show("User password empty.")
            Return
        End If

        Dim hashedPassword As String = HashPassword(enteredPassword)

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim checkExistCommand As New SqlCommand(
                "SELECT ID FROM user_table WHERE User_Name = @Username", sqlconnect)
                checkExistCommand.Parameters.AddWithValue("@Username", Username)

                Dim result = checkExistCommand.ExecuteScalar()

                If result IsNot Nothing Then
                    Dim ID As Integer = Convert.ToInt32(result)
                    Dim updateCommand As New SqlCommand("UPDATE user_table SET User_Name = @Username, User_Password = @Password WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@Username", Username)
                    updateCommand.Parameters.AddWithValue("@Password", hashedPassword)
                    updateCommand.Parameters.AddWithValue("@ID", ID)
                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("User details updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Dim insertCommand As New SqlCommand("INSERT INTO user_table (User_Name, User_Password) VALUES (@Username, @Password)", sqlconnect)
                    insertCommand.Parameters.AddWithValue("@Username", Username)
                    insertCommand.Parameters.AddWithValue("@Password", hashedPassword)
                    insertCommand.ExecuteNonQuery()
                    MessageBox.Show("User details saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using

        InitializeUserSecurity()

    End Sub
    'Public Shared Function GetStoredUsername() As String
    '    Dim ctlDesc As String = "UserName"
    '    Dim storedUsername As String = ""

    '    Using sqlconnect As SqlConnection = Tools.GetConnection()
    '        sqlconnect.Open()

    '        Dim command As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = @CtlDesc", sqlconnect)
    '        command.Parameters.AddWithValue("@CtlDesc", ctlDesc)

    '        Dim result = command.ExecuteScalar()
    '        If result IsNot Nothing Then
    '            storedUsername = result.ToString()
    '        End If
    '    End Using

    '    Return storedUsername
    'End Function

    'Private Sub ushowshower()
    '    Using sqlconnect As SqlConnection = Tools.GetConnection()
    '        Try
    '            sqlconnect.Open()
    '            Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'UserRight'", sqlconnect)
    '            Using reader As SqlDataReader = Query.ExecuteReader()
    '                If reader.Read() Then
    '                    Dim userRightValue As String = reader("Ctl_Value").ToString().Trim()
    '                    If userRightValue <> "0" Then
    '                        If GetStoredUsername() <> "Admin" Then
    '                            Label49.Visible = False
    '                            Label52.Visible = False
    '                            Label53.Visible = False
    '                            Label54.Visible = False
    '                            Usertxt.Visible = False
    '                            Psdtxt.Visible = False
    '                            Userbtn.Visible = False
    '                            Label55.Visible = False
    '                            Label56.Visible = False
    '                            UserText.Visible = False
    '                            Usertree.Visible = False
    '                            SRSavebtn.Visible = False
    '                        End If
    '                    Else
    '                        Label49.Visible = False
    '                        Label52.Visible = False
    '                        Label53.Visible = False
    '                        Label54.Visible = False
    '                        Usertxt.Visible = False
    '                        Psdtxt.Visible = False
    '                        Userbtn.Visible = False
    '                        Label55.Visible = False
    '                        Label56.Visible = False
    '                        UserText.Visible = False
    '                        Usertree.Visible = False
    '                        SRSavebtn.Visible = False
    '                    End If
    '                End If
    '            End Using
    '        Catch ex As Exception
    '            MessageBox.Show("Error: " & ex.Message)
    '        End Try
    '    End Using
    'End Sub

    Private Sub InitializeUserSecurity()
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()

                Dim currentUser As String = ""
                Dim userRightValue As String = ""

                Using cmdControl = New SqlCommand("SELECT Ctl_Desc, Ctl_Value FROM Control_Table WHERE Ctl_Desc IN ('UserName', 'UserRight')", conn)
                    Using reader = cmdControl.ExecuteReader()
                        While reader.Read()
                            If reader("Ctl_Desc").ToString() = "UserName" Then currentUser = reader("Ctl_Value").ToString()
                            If reader("Ctl_Desc").ToString() = "UserRight" Then userRightValue = reader("Ctl_Value").ToString()
                        End While
                    End Using
                End Using

                Dim isAdmin As Boolean = currentUser.Equals("Admin", StringComparison.OrdinalIgnoreCase)

                Dim shouldHide As Boolean = (userRightValue = "0") Or (Not isAdmin)

                Dim adminControls As Control() = {Label49, Label52, Label53, Label54, Usertxt, Psdtxt, Userbtn,
                                             Label55, Label56, UserText, Usertree, SRSavebtn}

                For Each ctrl In adminControls
                    If ctrl IsNot Nothing Then
                        ctrl.Visible = Not shouldHide
                    End If
                Next

                If Not shouldHide Then
                    UserText.Items.Clear()
                    Using cmdUsers = New SqlCommand("SELECT User_name FROM user_table WHERE ID != 1", conn)
                        Using rdrUsers = cmdUsers.ExecuteReader()
                            While rdrUsers.Read()
                                If Not rdrUsers.IsDBNull(0) Then
                                    UserText.Items.Add(rdrUsers.GetString(0))
                                End If
                            End While
                        End Using
                    End Using

                    If UserText.Items.Count > 0 Then
                        UserText.SelectedIndex = 0
                    End If
                End If

            End Using
        Catch ex As SqlException
            MsgBox("Database Error: " & ex.Message, MsgBoxStyle.Critical)
        Catch ex As Exception
            MsgBox("General Error: " & ex.Message, MsgBoxStyle.Exclamation)
        End Try
    End Sub

    Public Shared Function GetStoredUsername() As String
        Using conn As SqlConnection = Tools.GetConnection()
            conn.Open()
            Dim cmd As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'UserName'", conn)
            Return cmd.ExecuteScalar()?.ToString()
        End Using
    End Function

    Private Sub SetupUserTree()
        Usertree.CheckBoxes = True
        Usertree.Nodes.Clear()

        Dim rootNode As TreeNode = Usertree.Nodes.Add("BK0", "ATTMA SYSTEM")

        Dim MemberNode As TreeNode = rootNode.Nodes.Add("BK1", "MEMBER SHIP")
        MemberNode.Nodes.Add("BK1_EDIT", "EDIT")
        MemberNode.Nodes.Add("BK1_CANCEL", "CANCEL")

        Dim masterNode As TreeNode = rootNode.Nodes.Add("BK2", "VOUCHER")
        masterNode.Nodes.Add("BK2_PAY", "PAYMENT")
        masterNode.Nodes.Add("BK2_REC", "RECEIPT")
        masterNode.Nodes.Add("BK2_EDIT", "EDIT")
        masterNode.Nodes.Add("BK2_CANCEL", "CANCEL")

        Dim ReportNode As TreeNode = rootNode.Nodes.Add("BK6", "REPORT")
        ReportNode.Nodes.Add("BK7", "MS REPORT")
        ReportNode.Nodes.Add("BK8", "VOUCHER REPORT")

        rootNode.Nodes.Add("BK9", "SETTING")
        Usertree.ExpandAll()
    End Sub
    Private Sub CheckAllNodes(node As TreeNode)
        node.Checked = True
        For Each child As TreeNode In node.Nodes
            CheckAllNodes(child)
        Next
    End Sub

    Private Sub SRSavebtn_Click(sender As Object, e As EventArgs) Handles SRSavebtn.Click
        If UserText.SelectedItem Is Nothing Then
            MessageBox.Show("Please select a user.")
            Return
        End If

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()

                Dim cmdId As New SqlCommand("SELECT ID FROM User_table WHERE User_Name = @Name", conn)
                cmdId.Parameters.AddWithValue("@Name", UserText.SelectedItem.ToString())
                Dim userId = cmdId.ExecuteScalar()

                If userId IsNot Nothing Then

                    Dim del As New SqlCommand("DELETE FROM UserRight_Table WHERE User_ID = @UID", conn)
                    del.Parameters.AddWithValue("@UID", userId)
                    del.ExecuteNonQuery()

                    SaveAllNodes(Usertree.Nodes, Convert.ToInt32(userId), conn)

                    MessageBox.Show("Rights updated successfully.")
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("Save Error: " & ex.Message)
        End Try
    End Sub
    Private Sub UserText_SelectedIndexChanged(sender As Object, e As EventArgs) Handles UserText.SelectedIndexChanged
        LoadUserRights()
    End Sub

    Private Function FindNodeByKey(nodes As TreeNodeCollection, key As String) As TreeNode
        For Each n As TreeNode In nodes
            If n.Name = key Then Return n
            Dim found = FindNodeByKey(n.Nodes, key)
            If found IsNot Nothing Then Return found
        Next
        Return Nothing
    End Function
    Private Sub SaveAllNodes(nodes As TreeNodeCollection, uid As Integer, conn As SqlConnection)
        For Each node As TreeNode In nodes
            If node.Checked Then
                Dim sql As String = "INSERT INTO UserRight_Table (User_ID, Menu_ID, Menu_Name, IsAllowed) VALUES (@UID, @MID, @MName, 1)"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@UID", uid)
                    cmd.Parameters.AddWithValue("@MID", node.Name) ' Unique Key
                    cmd.Parameters.AddWithValue("@MName", node.Text)
                    cmd.ExecuteNonQuery()
                End Using
            End If
            SaveAllNodes(node.Nodes, uid, conn)
        Next
    End Sub
    Private Sub SaveUserRights(userId As Integer)
        For Each node As TreeNode In Usertree.Nodes
            SaveNodeRights(node, userId, "")
        Next
    End Sub
    Private Sub SaveNodeRights(node As TreeNode, userId As Integer, path As String)
        Dim menuName As String = node.Text
        Dim menuID As String = node.Name

        If node.Checked Then
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()

                Dim cmd As New SqlCommand("INSERT INTO UserRight_Table (User_ID, Menu_ID, Menu_Name, IsAllowed) VALUES (@UserID, @MenuID, @MenuName, 1)", conn)
                cmd.Parameters.AddWithValue("@UserID", userId)
                cmd.Parameters.AddWithValue("@MenuID", menuID)
                cmd.Parameters.AddWithValue("@MenuName", menuName)
                cmd.ExecuteNonQuery()
            End Using
        End If

        For Each child As TreeNode In node.Nodes
            SaveNodeRights(child, userId, "")
        Next
    End Sub
    Private Sub LoadUserRights()
        Try
            If UserText.SelectedItem Is Nothing Then Exit Sub

            Dim username As String = UserText.SelectedItem.ToString()
            Dim userId As Integer = -1

            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()

                Using cmd As New SqlCommand("SELECT ID FROM User_table WHERE User_Name = @UserName", conn)
                    cmd.Parameters.AddWithValue("@UserName", username)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        userId = Convert.ToInt32(result)
                    Else
                        MessageBox.Show("User not found.", "Error")
                        Exit Sub
                    End If
                End Using

                UncheckAllNodes(Usertree.Nodes)

                Dim rightsCmd As New SqlCommand("SELECT Menu_ID FROM UserRight_Table WHERE User_ID = @UserID", conn)
                rightsCmd.Parameters.AddWithValue("@UserID", userId)

                Using reader As SqlDataReader = rightsCmd.ExecuteReader()
                    While reader.Read()
                        Dim menuID As String = reader.GetString(0)
                        Dim node As TreeNode = FindNodeByName(Usertree.Nodes, menuID)
                        If node IsNot Nothing Then
                            node.Checked = True
                        End If
                    End While
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading user rights: " & ex.Message)
        End Try
    End Sub
    Private Sub UncheckAllNodes(nodes As TreeNodeCollection)
        For Each n As TreeNode In nodes
            n.Checked = False
            UncheckAllNodes(n.Nodes)
        Next
    End Sub
    Private Function FindNodeByName(nodes As TreeNodeCollection, name As String) As TreeNode
        For Each node As TreeNode In nodes
            If node.Name = name Then
                Return node
            End If
            Dim found As TreeNode = FindNodeByName(node.Nodes, name)
            If found IsNot Nothing Then
                Return found
            End If
        Next
        Return Nothing
    End Function

    Private Sub Guna2Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Guna2Panel1.Paint

    End Sub
    'Private Sub userrights()
    '    Try
    '        Dim username As String = GetStoredUsername()
    '        Dim userId As Integer = -1

    '        Using conn As SqlConnection = Tools.GetConnection()
    '            conn.Open()

    '            Dim cmd As New SqlCommand("SELECT ID FROM User_table WHERE User_Name = @UserName", conn)
    '            cmd.Parameters.AddWithValue("@UserName", username)

    '            Dim result = cmd.ExecuteScalar()
    '            If result IsNot Nothing Then
    '                userId = Convert.ToInt32(result)
    '            Else
    '                MessageBox.Show("User not found in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    '                Exit Sub
    '            End If
    '        End Using

    '        Dim allowedMenus As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    '        Using conn As SqlConnection = Tools.GetConnection()
    '            conn.Open()

    '            Dim cmd As New SqlCommand("SELECT Menu_ID FROM UserRight_Table WHERE User_ID = @ID AND IsAllowed = 1", conn)
    '            cmd.Parameters.AddWithValue("@ID", userId)

    '            Using reader As SqlDataReader = cmd.ExecuteReader()
    '                While reader.Read()
    '                    allowedMenus.Add(reader("Menu_ID").ToString())
    '                End While
    '            End Using
    '        End Using

    '    Catch ex As Exception
    '        MessageBox.Show("Error: " & ex.Message)
    '    End Try
    'End Sub
End Class
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.IO
Imports Guna.UI2.WinForms
Imports System.Security.Cryptography
Imports System.Text

Public Class Setting
    Private dtnpm, dtns, dtnt, dtaddons As DataTable
    Private dtpm, dtpt, dtpme, dtpi As DataTable
    Private activeTextbox As Guna.UI2.WinForms.Guna2TextBox
    Private PactiveTextbox As Guna.UI2.WinForms.Guna2TextBox
    Private listBoxSelectionMade As Boolean = False
    Private suppressSelection As Boolean = False
    Private currentItemList As DataTable
    Private filePath As String = Path.Combine(Application.StartupPath, "DBConnect.txt")

    Private Sub Setting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
            Tools.LoadConfiguration()
            LoadRefresh()
            Themeload()
            CBUserright.ForeColor = Color.Black
            CBEBackup.ForeColor = Color.Black
            CBTheme.ForeColor = Color.Black
            Dim elipse As New Guna2Elipse()
            elipse.BorderRadius = 20
            elipse.TargetControl = Me

            AddHandler KryptonListBox.Click, AddressOf KryptonListBox_Click
            AddHandler KryptonListBox.KeyDown, AddressOf KryptonListBox_KeyDown
            AddHandler KryptonListBox.LostFocus, AddressOf KryptonListBox_LostFocus

            AddHandler KryptonListBox1.Click, AddressOf KryptonListBox1_Click
            AddHandler KryptonListBox1.KeyDown, AddressOf KryptonListBox1_KeyDown
            AddHandler KryptonListBox1.LostFocus, AddressOf KryptonListBox1_LostFocus

            AddHandler ItemListBox.KeyDown, AddressOf ItemListBox_KeyDown
            ItemCombo.Items.AddRange({"GROUP", "MODEL", "BRAND", "UNIT"})


            DateTimePickerFrom.Format = DateTimePickerFormat.Custom
            DateTimePickerFrom.CustomFormat = "dd/MM/yyyy"

            DateTimePickerTo.Format = DateTimePickerFormat.Custom
            DateTimePickerTo.CustomFormat = "dd/MM/yyyy"

            ' Optional: default financial year
            DateTimePickerFrom.Value = New Date(Date.Now.Year - 1, 4, 1)
        DateTimePickerTo.Value = New Date(Date.Now.Year, 3, 31)
        LoadFinancialPeriod()

    End Sub

        Private Sub Themeload()
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    sqlconnect.Open()
                    Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'HeaderColor'", sqlconnect)
                    Using reader As SqlDataReader = Query.ExecuteReader()
                        If reader.Read() Then
                            Dim colorString As String = reader("Ctl_Value").ToString()
                            If Not String.IsNullOrEmpty(colorString) Then
                                Try
                                    HeaderPanel.BackColor = ColorTranslator.FromHtml(colorString)
                                Catch
                                    HeaderPanel.BackColor = Color.FromArgb(34, 40, 49)
                                End Try
                            Else
                                HeaderPanel.BackColor = Color.FromArgb(34, 40, 49)
                            End If
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error loading HeaderColor: " & ex.Message)
                End Try
            End Using

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    sqlconnect.Open()
                    Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'ScreenColor'", sqlconnect)
                    Using reader As SqlDataReader = Query.ExecuteReader()
                        If reader.Read() Then
                            Dim colorString As String = reader("Ctl_Value").ToString()
                            Dim screenColor As Color = Color.FromArgb(232, 232, 232)

                            If Not String.IsNullOrEmpty(colorString) Then
                                Try
                                    screenColor = ColorTranslator.FromHtml(colorString)
                                Catch
                                    Me.BackColor = Color.FromArgb(232, 232, 232)
                                End Try
                            End If

                            Me.BackColor = screenColor
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error loading ScreenColor: " & ex.Message)
                End Try
            End Using

        End Sub

        Private Sub LoadRefresh()
            CompanyLoad()
            NumberLoad()
            PrintLoad()
            LoadAutoCompleteData()
            LoadPrintingData()
            LoadUsernames()
            repostbtn()
            ushowshower()
            UserRight()
            userrights()
            LoadUserRights()
            settingload()
        End Sub

    'Comapny Page Details Stared
    Private Sub SaveBillnoButton_Click(sender As Object, e As EventArgs) Handles SaveBillnoButton.Click

        Dim PurchaseNO As String = Me.PurchaseNO.Text
        Dim PurchasePrefix As String = Me.PurchasePrefix.Text
        Dim PurchaseSuffix As String = Me.PurchaseSuffix.Text
        Dim Purchase As String = "Purchase"

        Dim SalesNo As String = Me.SalesNo.Text
        Dim SalesPrefix As String = Me.SalesPrefix.Text
        Dim SalesSuffix As String = Me.SalesSuffix.Text
        Dim Sales As String = "Sales"

        Dim JCNo As String = Me.JCNo.Text
        Dim JCPrefix As String = Me.JCPrefix.Text
        Dim JCSuffix As String = Me.JCSuffix.Text
        Dim JobCard As String = "JobCard"

        Dim PNo As String = Me.PNo.Text
        Dim PPrefix As String = Me.PPrefix.Text
        Dim PSuffix As String = Me.PSuffix.Text
        Dim Printing As String = "Printing"

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                UpdateBillPrefixSuffix(sqlconnect, Purchase, PurchaseNO, PurchasePrefix, PurchaseSuffix)
                UpdateBillPrefixSuffix(sqlconnect, Sales, SalesNo, SalesPrefix, SalesSuffix)
                UpdateBillPrefixSuffix(sqlconnect, JobCard, JCNo, JCPrefix, JCSuffix)
                UpdateBillPrefixSuffix(sqlconnect, Printing, PNo, PPrefix, PSuffix)

                Dim updateCmd As New SqlCommand("UPDATE Control_Table SET Ctl_Value = @NewDate WHERE Ctl_Desc = 'LastNoUpdated'", sqlconnect)
                updateCmd.Parameters.AddWithValue("@NewDate", Today.ToString("dd-MM-yyyy"))
                updateCmd.ExecuteNonQuery()
                MessageBox.Show("Bill No Updated.")
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
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

        LoadUsernames()
    End Sub
    Public Shared Function GetStoredUsername() As String
        Dim ctlDesc As String = "UserName"
        Dim storedUsername As String = ""

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()

            Dim command As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = @CtlDesc", sqlconnect)
            command.Parameters.AddWithValue("@CtlDesc", ctlDesc)

            Dim result = command.ExecuteScalar()
            If result IsNot Nothing Then
                storedUsername = result.ToString()
            End If
        End Using

        Return storedUsername
    End Function
    Private Sub ushowshower()
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'UserRight'", sqlconnect)
                Using reader As SqlDataReader = Query.ExecuteReader()
                    If reader.Read() Then
                        Dim userRightValue As String = reader("Ctl_Value").ToString().Trim()
                        If userRightValue <> "0" Then
                            If GetStoredUsername() <> "Admin" Then
                                Label49.Visible = False
                                Label52.Visible = False
                                Label53.Visible = False
                                Label54.Visible = False
                                Usertxt.Visible = False
                                Psdtxt.Visible = False
                                Userbtn.Visible = False
                                Label55.Visible = False
                                Label56.Visible = False
                                UserText.Visible = False
                                Usertree.Visible = False
                                SRSavebtn.Visible = False
                            End If
                        Else
                            Label49.Visible = False
                            Label52.Visible = False
                            Label53.Visible = False
                            Label54.Visible = False
                            Usertxt.Visible = False
                            Psdtxt.Visible = False
                            Userbtn.Visible = False
                            Label55.Visible = False
                            Label56.Visible = False
                            UserText.Visible = False
                            Usertree.Visible = False
                            SRSavebtn.Visible = False
                        End If
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub LoadUsernames()
        Try
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()

                Dim query As String = "SELECT User_name FROM user_table where ID != 1"
                Using cmd As New SqlCommand(query, sqlconnect)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        UserText.Items.Clear()
                        While reader.Read()
                            If Not reader.IsDBNull(0) Then
                                UserText.Items.Add(reader.GetString(0))
                            End If
                        End While
                    End Using
                End Using
            End Using

            If UserText.Items.Count > 0 Then
                UserText.SelectedIndex = 0
            End If

        Catch ex As SqlException
            MsgBox("SQL Error: " & ex.Message)
        Catch ex As Exception
            MsgBox("General Error: " & ex.Message)
        End Try
    End Sub

    Private Sub UserRight()
        Try
            Usertree.CheckBoxes = True
            Usertree.Nodes.Clear()

            Dim rootNode As TreeNode = Usertree.Nodes.Add("GS0", "Godown Stock")

            Usertree.ExpandAll()

            Dim salesNode As TreeNode = rootNode.Nodes.Add("GS1", "Sales")

            Dim purchaseNode As TreeNode = rootNode.Nodes.Add("GS2", "Purchase")

            rootNode.Nodes.Add("GS3", "Display")

            Dim masterNode As TreeNode = rootNode.Nodes.Add("GS4", "Master")
            Dim itemNode As TreeNode = masterNode.Nodes.Add("GS5", "Item")

            Dim ledgerNode As TreeNode = masterNode.Nodes.Add("GS6", "Ledger")

            Dim ReportNode As TreeNode = rootNode.Nodes.Add("GS7", "Report")
            ReportNode.Nodes.Add("GS15", "Weekly Report")
            ReportNode.Nodes.Add("GS16", "Item Wise")
            ReportNode.Nodes.Add("GS17", "Stock Summary")

            Dim jobcardNode As TreeNode = rootNode.Nodes.Add("GS8", "Jobcard")
            jobcardNode.Nodes.Add("GS18", "Jobcard Create")
            jobcardNode.Nodes.Add("GS19", "JobCard Display")
            jobcardNode.Nodes.Add("GS20", "JobCard Report")

            Dim printingNode As TreeNode = rootNode.Nodes.Add("GS9", "Printing")
            printingNode.Nodes.Add("GS21", "Printing Create")
            printingNode.Nodes.Add("GS22", "Printing Report")

            Dim settingNode As TreeNode = rootNode.Nodes.Add("GS10", "Setting")
            settingNode.Nodes.Add("GS11", "Company Details")
            settingNode.Nodes.Add("GS12", "Item Details")
            settingNode.Nodes.Add("GS13", "Jobcard Details")
            settingNode.Nodes.Add("GS14", "Printing Details")

            For Each node As TreeNode In Usertree.Nodes
                CheckAllNodes(node)
            Next

        Catch ex As SqlException
            MsgBox("SQL Error: " & ex.Message)
        Catch ex As Exception
            MsgBox("General Error: " & ex.Message)
        End Try
    End Sub

    Private Sub CheckAllNodes(node As TreeNode)
        node.Checked = True
        For Each child As TreeNode In node.Nodes
            CheckAllNodes(child)
        Next
    End Sub

    Private Sub SRSavebtn_Click(sender As Object, e As EventArgs) Handles SRSavebtn.Click
        Try
            If UserText.SelectedItem Is Nothing Then
                MessageBox.Show("Please select a user.")
                Exit Sub
            End If

            Dim username As String = UserText.SelectedItem.ToString()

            Dim userId As Integer = -1
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()

                Dim cmd As New SqlCommand("SELECT ID FROM User_table WHERE User_Name = @UserName", conn)
                cmd.Parameters.AddWithValue("@UserName", username)

                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    userId = Convert.ToInt32(result)
                Else
                    MessageBox.Show("User not found in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
            End Using

            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim deleteCmd As New SqlCommand("DELETE FROM UserRight_Table WHERE User_ID = @UserID", conn)
                deleteCmd.Parameters.AddWithValue("@UserID", userId)
                deleteCmd.ExecuteNonQuery()
            End Using

            SaveUserRights(userId)

            MessageBox.Show("User rights saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As SqlException
            MessageBox.Show("SQL Error: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
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
    Private Sub UserText_SelectedIndexChanged(sender As Object, e As EventArgs)
        LoadUserRights()
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
            For Each node As TreeNode In nodes
                node.Checked = False
                UncheckAllNodes(node.Nodes)
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

        Private Sub userrights()
            Try
                Dim username As String = GetStoredUsername()
                Dim userId As Integer = -1

                Using conn As SqlConnection = Tools.GetConnection()
                    conn.Open()

                    Dim cmd As New SqlCommand("SELECT ID FROM User_table WHERE User_Name = @UserName", conn)
                    cmd.Parameters.AddWithValue("@UserName", username)

                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        userId = Convert.ToInt32(result)
                    Else
                        MessageBox.Show("User not found in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End If
                End Using

                If userId = 1 Then
                    ShowAllButtons()
                    Exit Sub
                End If

                Dim allowedMenus As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
                Using conn As SqlConnection = Tools.GetConnection()
                    conn.Open()

                    Dim cmd As New SqlCommand("SELECT Menu_ID FROM UserRight_Table WHERE User_ID = @ID AND IsAllowed = 1", conn)
                    cmd.Parameters.AddWithValue("@ID", userId)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            allowedMenus.Add(reader("Menu_ID").ToString())
                        End While
                    End Using
                End Using

                ToggleButtonAccess(allowedMenus)

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Sub
        Private Sub ToggleButtonAccess(allowedMenus As HashSet(Of String))
            JobCard.Visible = allowedMenus.Contains("GS13")
            Printing.Visible = allowedMenus.Contains("GS14")
            Company.Visible = allowedMenus.Contains("GS11")
            Item.Visible = allowedMenus.Contains("GS12")
        End Sub

        Private Sub ShowAllButtons()
            JobCard.Visible = True
            Printing.Visible = True
            Company.Visible = True
            Item.Visible = True
        End Sub

        Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        Dim CompNo As String = "BK0001"
        Dim CompanyName As String = CompanyNameText.Text
            Dim Address1 As String = Address1TextBox.Text
            Dim Address2 As String = Address2TextBox.Text
            Dim Address3 As String = Address3TextBox.Text
            Dim Mobile As String = MobileTextBox.Text

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    sqlconnect.Open()

                    Dim checkExistCommand As New SqlCommand("SELECT COUNT(*) FROM Company_Table WHERE Comp_No = @CompNo", sqlconnect)
                    checkExistCommand.Parameters.AddWithValue("@CompNo", CompNo)

                    Dim count As Integer = Convert.ToInt32(checkExistCommand.ExecuteScalar())

                    If count > 0 Then
                        Dim updateCommand As New SqlCommand("UPDATE Company_Table SET Comp_Name = @CompanyName, Comp_Address1 = @Address1, Comp_Address2 = @Address2, Comp_Address3 = @Address3, Mobile = @Mobile WHERE Comp_No = @CompNo", sqlconnect)
                        updateCommand.Parameters.AddWithValue("@CompanyName", CompanyName)
                        updateCommand.Parameters.AddWithValue("@Address1", Address1)
                        updateCommand.Parameters.AddWithValue("@Address2", Address2)
                        updateCommand.Parameters.AddWithValue("@Address3", Address3)
                        updateCommand.Parameters.AddWithValue("@Mobile", Mobile)
                        updateCommand.Parameters.AddWithValue("@CompNo", CompNo)

                        updateCommand.ExecuteNonQuery()

                        'MessageBox.Show("Company details updated.")
                    Else
                        Dim insertCommand As New SqlCommand("INSERT INTO Company_Table (Comp_No, Comp_Name, Comp_Address1, Comp_Address2, Comp_Address3, Mobile) VALUES (@CompNo, @CompanyName, @Address1, @Address2, @Address3, @Mobile)", sqlconnect)
                        insertCommand.Parameters.AddWithValue("@CompNo", CompNo)
                        insertCommand.Parameters.AddWithValue("@CompanyName", CompanyName)
                        insertCommand.Parameters.AddWithValue("@Address1", Address1)
                        insertCommand.Parameters.AddWithValue("@Address2", Address2)
                        insertCommand.Parameters.AddWithValue("@Address3", Address3)
                        insertCommand.Parameters.AddWithValue("@Mobile", Mobile)

                        insertCommand.ExecuteNonQuery()
                        MessageBox.Show("Company details saved.")
                    End If
                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using

            Dim PageWidth As Integer = Pagetxt.Text
            Dim Paperwidth As Integer = PaperTxt.Text

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    sqlconnect.Open()
                    Dim PageWidthCommand As New SqlCommand("update Control_Table set Ctl_Value = @Paperwidth  where Ctl_Desc = 'Print_BoxWidth'", sqlconnect)
                    PageWidthCommand.Parameters.AddWithValue("@Paperwidth", Paperwidth)
                    PageWidthCommand.ExecuteNonQuery()
                    Dim PaperwidthCommand As New SqlCommand("update Control_Table set Ctl_Value = @PageWidth  where Ctl_Desc = 'Print_PageWidth'", sqlconnect)
                    PaperwidthCommand.Parameters.AddWithValue("@PageWidth", PageWidth)
                    PaperwidthCommand.ExecuteNonQuery()

                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
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
            'MsgBox("Setting Saved")
            Refresh()

            Dim CBBackup As Boolean = CBEBackup.Checked

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    sqlconnect.Open()
                    Dim sql As String = "UPDATE Control_Table SET Ctl_Value = @Backup WHERE Ctl_Desc = 'EnableBackup'"
                    Using BackupCommand As New SqlCommand(sql, sqlconnect)
                        BackupCommand.Parameters.Add("@Backup", SqlDbType.Bit).Value = CBBackup
                        BackupCommand.ExecuteNonQuery()
                    End Using
                    'MsgBox("Setting Saved Successfully")
                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using

            Dim CBETheme As Boolean = CBTheme.Checked

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    sqlconnect.Open()
                    Dim sql As String = "UPDATE Control_Table SET Ctl_Value = @Backup WHERE Ctl_Desc = 'EnableTheme'"
                    Using ThemeCommand As New SqlCommand(sql, sqlconnect)
                        ThemeCommand.Parameters.Add("@Backup", SqlDbType.Bit).Value = CBETheme
                        ThemeCommand.ExecuteNonQuery()
                    End Using
                    MsgBox("Setting Saved Successfully")

                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using

            Refresh()


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

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    sqlconnect.Open()
                    Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'EnableTheme'", sqlconnect)
                    Using reader As SqlDataReader = Query.ExecuteReader()
                        If reader.Read() Then
                            Dim enabled As Boolean = (reader("Ctl_Value").ToString() = "1")
                            CBTheme.Checked = enabled

                            If enabled Then
                                Label64.Show()
                                ResetTheme.Show()
                                lblheaderColorPicker.Show()
                                lblscreenColorPicker.Show()
                                ColorPicker.Show()
                                Screencolorpicker.Show()
                            Else
                                Label64.Hide()
                                ResetTheme.Hide()
                                lblheaderColorPicker.Hide()
                                lblscreenColorPicker.Hide()
                                ColorPicker.Hide()
                                Screencolorpicker.Hide()
                            End If
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using

            ' Load HeaderColor
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    sqlconnect.Open()
                    Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'HeaderColor'", sqlconnect)
                    Using reader As SqlDataReader = Query.ExecuteReader()
                        If reader.Read() Then
                            Dim colorString As String = reader("Ctl_Value").ToString()
                            If Not String.IsNullOrEmpty(colorString) Then
                                Try
                                    lblheaderColorPicker.BackColor = ColorTranslator.FromHtml(colorString)
                                Catch
                                    ' If color conversion fails, set default color
                                    lblheaderColorPicker.BackColor = Color.White
                                End Try
                            Else
                                lblheaderColorPicker.BackColor = Color.White
                            End If
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error loading HeaderColor: " & ex.Message)
                End Try
            End Using

            ' Load ScreenColor
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    sqlconnect.Open()
                    Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'ScreenColor'", sqlconnect)
                    Using reader As SqlDataReader = Query.ExecuteReader()
                        If reader.Read() Then
                            Dim colorString As String = reader("Ctl_Value").ToString()
                            If Not String.IsNullOrEmpty(colorString) Then
                                Try
                                    lblscreenColorPicker.BackColor = ColorTranslator.FromHtml(colorString)
                                Catch
                                    lblscreenColorPicker.BackColor = Color.White
                                End Try
                            Else
                                lblscreenColorPicker.BackColor = Color.White
                            End If
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error loading ScreenColor: " & ex.Message)
                End Try
            End Using

        End Sub
        Private Sub ResetTheme_Click(sender As Object, e As EventArgs) Handles ResetTheme.Click

            Dim colorString As String = ""

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()

                Using transaction = sqlconnect.BeginTransaction()
                    Try
                        Dim sql As String = "UPDATE Control_Table SET Ctl_Value = @ColorValue WHERE Ctl_Desc IN ('HeaderColor', 'ScreenColor')"
                        Using cmd As New SqlCommand(sql, sqlconnect, transaction)
                            cmd.Parameters.Add("@ColorValue", SqlDbType.VarChar, 50).Value = colorString
                            cmd.ExecuteNonQuery()
                        End Using

                        transaction.Commit()
                        MessageBox.Show("Theme reset successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex As Exception
                        transaction.Rollback()
                        MessageBox.Show("Error: " & ex.Message)
                    End Try
                End Using
            End Using

        End Sub

        Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles ColorPicker.Click

            Using colorDialog As New ColorDialog()
                colorDialog.AllowFullOpen = True
                colorDialog.AnyColor = True
                colorDialog.SolidColorOnly = False
                colorDialog.Color = lblheaderColorPicker.BackColor

                If colorDialog.ShowDialog() = DialogResult.OK Then

                    Dim selectedColor As Color = colorDialog.Color

                    lblheaderColorPicker.BackColor = selectedColor

                    Dim colorString As String = ColorTranslator.ToHtml(selectedColor)

                    Using sqlconnect As SqlConnection = Tools.GetConnection()
                        Try
                            sqlconnect.Open()
                            Dim sql As String = "UPDATE Control_Table SET Ctl_Value = @ColorValue WHERE Ctl_Desc = 'HeaderColor'"
                            Using ThemeCommand As New SqlCommand(sql, sqlconnect)
                                ThemeCommand.Parameters.Add("@ColorValue", SqlDbType.VarChar).Value = colorString
                                ThemeCommand.ExecuteNonQuery()
                            End Using
                            MsgBox("Setting Saved Successfully")

                        Catch ex As Exception
                            MessageBox.Show("Error: " & ex.Message)
                        End Try
                    End Using
                End If
            End Using
        End Sub
        Private Sub Screencolorpicker_Click(sender As Object, e As EventArgs) Handles Screencolorpicker.Click

            Using colorDialog As New ColorDialog()
                colorDialog.AllowFullOpen = True
                colorDialog.AnyColor = True
                colorDialog.SolidColorOnly = False
                colorDialog.Color = lblheaderColorPicker.BackColor

                If colorDialog.ShowDialog() = DialogResult.OK Then

                    Dim selectedColor As Color = colorDialog.Color

                    lblscreenColorPicker.BackColor = selectedColor

                    Dim colorString As String = ColorTranslator.ToHtml(selectedColor)

                    Using sqlconnect As SqlConnection = Tools.GetConnection()
                        Try
                            sqlconnect.Open()
                            Dim sql As String = "UPDATE Control_Table SET Ctl_Value = @ColorValue WHERE Ctl_Desc = 'ScreenColor'"
                            Using ThemeCommand As New SqlCommand(sql, sqlconnect)
                                ThemeCommand.Parameters.Add("@ColorValue", SqlDbType.VarChar).Value = colorString
                                ThemeCommand.ExecuteNonQuery()
                            End Using
                            MsgBox("Setting Saved Successfully")

                        Catch ex As Exception
                            MessageBox.Show("Error: " & ex.Message)
                        End Try
                    End Using
                End If
            End Using
        End Sub
        Private Sub CompanyLoad()

        Dim CompNo As String = "BK0001"

        Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    sqlconnect.Open()

                    Dim checkExistCommand As New SqlCommand("SELECT COUNT(*) FROM Company_Table WHERE Comp_No = @CompNo", sqlconnect)
                    checkExistCommand.Parameters.AddWithValue("@CompNo", CompNo)

                    Dim count As Integer = Convert.ToInt32(checkExistCommand.ExecuteScalar())

                    If count > 0 Then
                        Dim selectCommand As New SqlCommand("SELECT Comp_Name, Comp_Address1, Comp_Address2, Comp_Address3, Mobile FROM Company_Table WHERE Comp_No = @CompNo", sqlconnect)
                        selectCommand.Parameters.AddWithValue("@CompNo", CompNo)

                        Using reader As SqlDataReader = selectCommand.ExecuteReader()
                            If reader.Read() Then
                                CompanyNameText.Text = reader("Comp_Name").ToString()
                                Address1TextBox.Text = reader("Comp_Address1").ToString()
                                Address2TextBox.Text = reader("Comp_Address2").ToString()
                                Address3TextBox.Text = reader("Comp_Address3").ToString()
                                MobileTextBox.Text = reader("Mobile").ToString()
                            End If
                        End Using
                    Else
                        CompanyNameText.Text = ""
                        Address1TextBox.Text = ""
                        Address2TextBox.Text = ""
                        Address3TextBox.Text = ""
                        MobileTextBox.Text = ""
                    End If

                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using

        End Sub
        Private Sub PrintLoad()

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    sqlconnect.Open()

                    Dim selectCommand As New SqlCommand("select ctl_value from Control_Table where Ctl_Desc = 'Print_BoxWidth'", sqlconnect)
                    Using reader As SqlDataReader = selectCommand.ExecuteReader()
                        If reader.Read() Then
                            PaperTxt.Text = reader("ctl_value").ToString()
                        End If
                    End Using
                    Dim PagetxtCommand As New SqlCommand("select ctl_value from Control_Table where Ctl_Desc = 'Print_PageWidth'", sqlconnect)
                    Using reader As SqlDataReader = PagetxtCommand.ExecuteReader()
                        If reader.Read() Then
                            Pagetxt.Text = reader("ctl_value").ToString()
                        End If
                    End Using

                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message)
                End Try
            End Using

        End Sub

        Private Sub NumberLoad()
            Dim PurchaseTxt As String = PurchaseNO.Text
            Dim SalesTxt As String = SalesNo.Text
            Dim Purchase As String = "Purchase"
            Dim Sales As String = "Sales"
            Dim Jobcard As String = "JobCard"
            Dim Printing As String = "Printing"

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    Dim checkExistCommand As New SqlCommand("select count(*) from v_table where Vt_Name = 'Purchase'", sqlconnect)

                    sqlconnect.Open()
                    Dim count As Integer = Convert.ToInt32(checkExistCommand.ExecuteScalar())

                    If count > 0 Then

                        Dim selectCommand As New SqlCommand("select Vt_Prefix, Vt_Suffix, Vt_Billno from v_table where Vt_Name = 'Purchase'", sqlconnect)
                        selectCommand.Parameters.AddWithValue("@Vt_Name", Purchase)

                        Using reader As SqlDataReader = selectCommand.ExecuteReader()
                            If reader.Read() Then
                                PurchaseNO.Text = reader("Vt_Billno").ToString()
                                PurchasePrefix.Text = reader("Vt_Prefix").ToString()
                                PurchaseSuffix.Text = reader("Vt_Suffix").ToString()
                            End If
                        End Using
                    Else
                        PurchaseNO.Text = String.Empty
                        PurchasePrefix.Text = String.Empty
                        PurchaseSuffix.Text = String.Empty
                    End If

                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message & vbCrLf & "Stack Trace: " & ex.StackTrace)
                End Try
            End Using

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    Dim checkExistCommand As New SqlCommand("select count(*) from v_table where Vt_Name = 'Sales'", sqlconnect)

                    sqlconnect.Open()
                    Dim count As Integer = Convert.ToInt32(checkExistCommand.ExecuteScalar())

                    If count > 0 Then

                        Dim selectCommand As New SqlCommand("select Vt_Prefix, Vt_Suffix, Vt_Billno from v_table where Vt_Name = 'Sales'", sqlconnect)
                        selectCommand.Parameters.AddWithValue("@Vt_Name", Sales)

                        Using reader As SqlDataReader = selectCommand.ExecuteReader()
                            If reader.Read() Then
                                SalesNo.Text = reader("Vt_Billno").ToString()
                                SalesPrefix.Text = reader("Vt_Prefix").ToString()
                                SalesSuffix.Text = reader("Vt_Suffix").ToString()
                            End If
                        End Using
                    Else
                        SalesNo.Text = String.Empty
                        SalesPrefix.Text = String.Empty
                        SalesSuffix.Text = String.Empty
                    End If

                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message & vbCrLf & "Stack Trace: " & ex.StackTrace)
                End Try
            End Using

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    Dim checkExistCommand As New SqlCommand("select count(*) from v_table where Vt_Name = 'JobCard'", sqlconnect)

                    sqlconnect.Open()
                    Dim count As Integer = Convert.ToInt32(checkExistCommand.ExecuteScalar())

                    If count > 0 Then

                        Dim selectCommand As New SqlCommand("select Vt_Prefix, Vt_Suffix, Vt_Billno from v_table where Vt_Name = 'JobCard'", sqlconnect)
                        selectCommand.Parameters.AddWithValue("@Vt_Name", Jobcard)

                        Using reader As SqlDataReader = selectCommand.ExecuteReader()
                            If reader.Read() Then
                                JCNo.Text = reader("Vt_Billno").ToString()
                                JCPrefix.Text = reader("Vt_Prefix").ToString()
                                JCSuffix.Text = reader("Vt_Suffix").ToString()
                            End If
                        End Using
                    Else
                        JCNo.Text = String.Empty
                        JCPrefix.Text = String.Empty
                        JCSuffix.Text = String.Empty
                    End If

                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message & vbCrLf & "Stack Trace: " & ex.StackTrace)
                End Try
            End Using

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    Dim checkExistCommand As New SqlCommand("select count(*) from v_table where Vt_Name = 'Printing'", sqlconnect)

                    sqlconnect.Open()
                    Dim count As Integer = Convert.ToInt32(checkExistCommand.ExecuteScalar())

                    If count > 0 Then

                        Dim selectCommand As New SqlCommand("select Vt_Prefix, Vt_Suffix, Vt_Billno from v_table where Vt_Name = 'Printing'", sqlconnect)
                        selectCommand.Parameters.AddWithValue("@Vt_Name", Printing)

                        Using reader As SqlDataReader = selectCommand.ExecuteReader()
                            If reader.Read() Then
                                PNo.Text = reader("Vt_Billno").ToString()
                                PPrefix.Text = reader("Vt_Prefix").ToString()
                                PSuffix.Text = reader("Vt_Suffix").ToString()
                            End If
                        End Using
                    Else
                        PNo.Text = String.Empty
                        PPrefix.Text = String.Empty
                        PSuffix.Text = String.Empty
                    End If

                Catch ex As Exception
                    MessageBox.Show("Error: " & ex.Message & vbCrLf & "Stack Trace: " & ex.StackTrace)
                End Try
            End Using

        End Sub

        Private Sub UpdateBillPrefixSuffix(ByRef sqlconnect As SqlConnection, ByVal BillType As String, ByVal BillNo As String, ByVal Prefix As String, ByVal Suffix As String)
            Try
                Dim checkExistCommand As New SqlCommand("select count(*) from v_table where Vt_Name = @Vt_Name", sqlconnect)
                checkExistCommand.Parameters.AddWithValue("@Vt_Name", BillType)

                Dim count As Integer = Convert.ToInt32(checkExistCommand.ExecuteScalar())

                If count > 0 Then
                    Dim updateCommand As New SqlCommand("UPDATE v_table SET Vt_Prefix = @Vt_Prefix, Vt_Suffix = @Vt_Suffix, Vt_Billno = @Vt_Billno from v_table where Vt_Name = @Vt_Name", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@Vt_Billno", BillNo)
                    updateCommand.Parameters.AddWithValue("@Vt_Prefix", Prefix)
                    updateCommand.Parameters.AddWithValue("@Vt_Suffix", Suffix)
                    updateCommand.Parameters.AddWithValue("@Vt_Name", BillType)

                    updateCommand.ExecuteNonQuery()
                End If
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Sub

        Private Sub repostbtn()
            ItemPostButton.Visible = False
            JCPostButton.Visible = False
            ActiveRepostButton.Visible = False
        PtPostButton.Visible = False
        Label66.Visible = False
        DateTimePickerFrom.Visible = False
        Label70.Visible = False
        DateTimePickerTo.Visible = False
        OldDBName.Visible = False
        Remove.Visible = False
    End Sub

        Private Function GetIdMapping(connection As SqlConnection, tableName As String, nameColumn As String) As Dictionary(Of String, Integer)
            Dim dict As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
            Dim cmd As New SqlCommand($"SELECT ID, {nameColumn} FROM {tableName}", connection)

            Using reader As SqlDataReader = cmd.ExecuteReader()
                While reader.Read()
                    Dim name = reader(nameColumn).ToString().Trim()
                    Dim id = Convert.ToInt32(reader("ID"))
                    If Not dict.ContainsKey(name) Then
                        dict(name) = id
                    End If
                End While
            End Using

            Return dict
        End Function

    Private Sub ItemPostButton_Click(sender As Object, e As EventArgs) Handles ItemPostButton.Click

        Try
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()

                Dim groupDict = GetIdMapping(sqlconnect, "ItemGroup_Table", "ItemGroup_Name")
                Dim brandDict = GetIdMapping(sqlconnect, "ItemBrand_Table", "ItemBrand_Name")
                Dim modelDict = GetIdMapping(sqlconnect, "ItemModel_Table", "ItemModel_Name")
                Dim unitDict = GetIdMapping(sqlconnect, "ItemUnit_Table", "ItemUnit_Name")

                Dim itemTable As New DataTable()
                Using getItemsCmd As New SqlCommand("SELECT ID, Itemgroup, Itembrand, Itemmodel, Unit FROM Item_Table", sqlconnect)
                    Using reader As SqlDataReader = getItemsCmd.ExecuteReader()
                        itemTable.Load(reader)
                    End Using
                End Using

                For Each row As DataRow In itemTable.Rows
                    Dim itemId As Integer = row("ID")
                    Dim groupName As String = row("Itemgroup").ToString().Trim()
                    Dim brandName As String = row("Itembrand").ToString().Trim()
                    Dim modelName As String = row("Itemmodel").ToString().Trim()
                    Dim unitName As String = row("Unit").ToString().Trim()

                    Dim groupId = If(groupDict.ContainsKey(groupName), groupDict(groupName), CType(Nothing, Integer?))
                    Dim brandId = If(brandDict.ContainsKey(brandName), brandDict(brandName), CType(Nothing, Integer?))
                    Dim modelId = If(modelDict.ContainsKey(modelName), modelDict(modelName), CType(Nothing, Integer?))
                    Dim unitId = If(unitDict.ContainsKey(unitName), unitDict(unitName), CType(Nothing, Integer?))

                    If groupId.HasValue OrElse brandId.HasValue OrElse modelId.HasValue OrElse unitId.HasValue Then
                        Dim updateCmd As New SqlCommand(" UPDATE Item_Table SET ItemGroup_ID = @GroupID, ItemBrand_ID = @BrandID, ItemModel_ID = @ModelID, ItemUnit_ID = @UnitID 
                                                          WHERE ID = @ItemID", sqlconnect)

                        updateCmd.Parameters.AddWithValue("@GroupID", If(groupId.HasValue, CType(groupId, Object), DBNull.Value))
                        updateCmd.Parameters.AddWithValue("@BrandID", If(brandId.HasValue, CType(brandId, Object), DBNull.Value))
                        updateCmd.Parameters.AddWithValue("@ModelID", If(modelId.HasValue, CType(modelId, Object), DBNull.Value))
                        updateCmd.Parameters.AddWithValue("@UnitID", If(unitId.HasValue, CType(unitId, Object), DBNull.Value))
                        updateCmd.Parameters.AddWithValue("@ItemID", itemId)

                        updateCmd.ExecuteNonQuery()
                    End If
                Next

                MessageBox.Show("Item_Table IDs updated successfully.")
            End Using
        Catch ex As Exception
            MessageBox.Show("Error updating IDs: " & ex.Message)
        End Try
    End Sub
    Private Sub JCPostButton_Click(sender As Object, e As EventArgs) Handles JCPostButton.Click
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()
            Using transaction = sqlconnect.BeginTransaction()
                Try
                    Dim updateQuery As String = "update JobCard_table set WorkingStatus='PENDING' where WorkingStatus<>'COMPLETED' and WorkingStatus<>'CANCEL'"
                    Using sqlcommand As New SqlCommand(updateQuery, sqlconnect, transaction)
                        sqlcommand.ExecuteNonQuery()
                    End Using
                    transaction.Commit()
                    MessageBox.Show("Repost Completed")
                Catch ex As Exception
                    transaction.Rollback()
                    MessageBox.Show("An error occurred while cancelling the entry: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub
    Private Sub PtPostButton_Click(sender As Object, e As EventArgs) Handles PtPostButton.Click
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()
            Using transaction = sqlconnect.BeginTransaction()
                Try
                    Dim updateQuery As String = "update Printing_table set WorkingStatus='PENDING' where WorkingStatus<>'COMPLETED' and WorkingStatus<>'CANCEL'"
                    Dim FinishQuery As String = "update Printing_table set Finish=0 "
                    Using sqlcommand As New SqlCommand(updateQuery, sqlconnect, transaction)
                        sqlcommand.ExecuteNonQuery()
                    End Using
                    Using sqlcommand As New SqlCommand(FinishQuery, sqlconnect, transaction)
                        sqlcommand.ExecuteNonQuery()
                    End Using
                    transaction.Commit()
                    MessageBox.Show("Repost Completed")
                Catch ex As Exception
                    transaction.Rollback()
                    MessageBox.Show("An error occurred while cancelling the entry: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub
    Private Sub ActiveRepostButton_Click(sender As Object, e As EventArgs) Handles ActiveRepostButton.Click
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()
            Using transaction = sqlconnect.BeginTransaction()
                Try
                    Dim tableList As String() = {"Addons_Master", "ItemBrand_Table", "ItemGroup_Table", "ItemModel_Table", "ItemUnit_Table", "NoteProcessing_table",
                    "Notesize_table", "NoteType_table", "PrintingMachine_table", "PrintingMethod_table", "PrintingType_table", "PrintingItem_table"}
                    For Each tableName As String In tableList
                        Dim updateActiveQuery As String = $"UPDATE {tableName} SET active = 0"
                        Using sqlcommand As New SqlCommand(updateActiveQuery, sqlconnect, transaction)
                            sqlcommand.ExecuteNonQuery()
                        End Using
                    Next

                    transaction.Commit()
                    MessageBox.Show("Repost Completed and Active Flags Updated")
                Catch ex As Exception
                    transaction.Rollback()
                    MessageBox.Show("An error occurred: " & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    Private Sub RepostButton_Click(sender As Object, e As EventArgs) Handles RepostButton.Click
        Dim repostForm As New Repost()
        repostForm.Sett = Me
        repostForm.ShowDialog()
    End Sub

    'Comapny Page Details End

    'Item Page Details Stared
    Private Sub UnitSaveButton_Click(sender As Object, e As EventArgs) Handles UnitSaveButton.Click
        Dim UnitName As String = UnitTextBox.Text.Trim()

        If String.IsNullOrWhiteSpace(UnitName) Then
            MessageBox.Show("Unit name cannot be empty.")
            Exit Sub
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Dim checkExistCommand As New SqlCommand("SELECT COUNT(*) FROM ItemUnit_Table WHERE ItemUnit_Name = @ItemUnit_Name", sqlconnect)
            checkExistCommand.Parameters.AddWithValue("@ItemUnit_Name", UnitName)

            Try
                sqlconnect.Open()

                Dim count As Integer = Convert.ToInt32(checkExistCommand.ExecuteScalar())

                If count > 0 Then
                    MessageBox.Show("This unit already exists.")
                    Exit Sub
                End If

                Dim insertCommand As New SqlCommand("INSERT INTO ItemUnit_Table (ItemUnit_Name) VALUES (@ItemUnit_Name)", sqlconnect)
                insertCommand.Parameters.AddWithValue("@ItemUnit_Name", UnitName)

                insertCommand.ExecuteNonQuery()
                MessageBox.Show("Unit saved successfully.")

                UnitTextBox.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub


    Private Sub BrandSaveButton_Click(sender As Object, e As EventArgs) Handles BrandSaveButton.Click
        Dim BrandName As String = BrandTextBox.Text.Trim()

        If String.IsNullOrWhiteSpace(BrandName) Then
            MessageBox.Show("Brand name cannot be empty.")
            Exit Sub
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Dim checkExistCommand As New SqlCommand("SELECT COUNT(*) FROM ItemBrand_Table WHERE ItemBrand_Name = @ItemBrand_Name", sqlconnect)
            checkExistCommand.Parameters.AddWithValue("@ItemBrand_Name", BrandName)

            Try
                sqlconnect.Open()
                Dim count As Integer = Convert.ToInt32(checkExistCommand.ExecuteScalar())

                If count > 0 Then
                    MessageBox.Show("This Brand already exists.")
                    Exit Sub
                End If

                Dim insertCommand As New SqlCommand("INSERT INTO ItemBrand_Table (ItemBrand_Name) VALUES (@ItemBrand_Name)", sqlconnect)
                insertCommand.Parameters.AddWithValue("@ItemBrand_Name", BrandName)
                insertCommand.ExecuteNonQuery()
                MessageBox.Show("Brand saved.")


                BrandTextBox.Text = ""
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub ModelSaveButton_Click(sender As Object, e As EventArgs) Handles ModelSaveButton.Click

        Dim ModelName As String = ModelTextBox.Text.Trim()

        If String.IsNullOrWhiteSpace(ModelName) Then
            MessageBox.Show("Model name cannot be empty.")
            Exit Sub
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Dim checkExistCommand As New SqlCommand("SELECT COUNT(*) FROM ItemModel_Table WHERE ItemModel_Name = @ItemModel_Name", sqlconnect)
            checkExistCommand.Parameters.AddWithValue("@ItemModel_Name", ModelName)

            Try
                sqlconnect.Open()
                Dim count As Integer = Convert.ToInt32(checkExistCommand.ExecuteScalar())

                If count > 0 Then
                    MessageBox.Show("This Model already exists.")
                    Exit Sub
                End If

                Dim insertCommand As New SqlCommand("INSERT INTO ItemModel_Table (ItemModel_Name) VALUES (@ItemModel_Name)", sqlconnect)
                insertCommand.Parameters.AddWithValue("@ItemModel_Name", ModelName)
                insertCommand.ExecuteNonQuery()
                MessageBox.Show("Model saved.")


                ModelTextBox.Text = ""
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub GSaveButton_Click(sender As Object, e As EventArgs) Handles GSaveButton.Click
        Dim GroupName As String = GroupTextBox.Text.Trim()

        If String.IsNullOrWhiteSpace(GroupName) Then
            MessageBox.Show("Group name cannot be empty.")
            Exit Sub
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Dim checkExistCommand As New SqlCommand("SELECT COUNT(*) FROM ItemGroup_Table WHERE ItemGroup_Name = @ItemGroup_Name", sqlconnect)
            checkExistCommand.Parameters.AddWithValue("@ItemGroup_Name", GroupName)

            Try
                sqlconnect.Open()
                Dim count As Integer = Convert.ToInt32(checkExistCommand.ExecuteScalar())

                If count > 0 Then
                    MessageBox.Show("This Group already exists.")
                    Exit Sub
                End If

                Dim insertCommand As New SqlCommand("INSERT INTO ItemGroup_Table (ItemGroup_Name) VALUES (@ItemGroup_Name)", sqlconnect)
                insertCommand.Parameters.AddWithValue("@ItemGroup_Name", GroupName)

                insertCommand.ExecuteNonQuery()
                MessageBox.Show("Group saved successfully.")

                GroupTextBox.Text = ""
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub SaveDtlsButton_Click(sender As Object, e As EventArgs) Handles SaveDtlsButton.Click
        Dim oldValue As String = SearchDtlsText.Text.Trim()
        Dim newValue As String = UpdateDtlsText.Text.Trim()
        Dim selectedItem As String = ItemCombo.SelectedItem?.ToString()

        If String.IsNullOrWhiteSpace(oldValue) OrElse String.IsNullOrWhiteSpace(newValue) Then
            MessageBox.Show("Both fields must be filled.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(selectedItem) Then
            MessageBox.Show("Please select a category from the combo box.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim tableColumnMap As New Dictionary(Of String, Tuple(Of String, String)) From {
                        {"GROUP", Tuple.Create("ItemGroup_Table", "ItemGroup_Name")},
                        {"MODEL", Tuple.Create("ItemModel_Table", "ItemModel_Name")},
                        {"BRAND", Tuple.Create("ItemBrand_Table", "ItemBrand_Name")},
                        {"UNIT", Tuple.Create("ItemUnit_Table", "ItemUnit_Name")}
                    }


        If Not tableColumnMap.ContainsKey(selectedItem) Then
            MessageBox.Show("Invalid selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim tableName = tableColumnMap(selectedItem).Item1
        Dim columnName = tableColumnMap(selectedItem).Item2

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()

            Try
                Dim checkCmd As New SqlCommand($"SELECT COUNT(*) FROM {tableName} WHERE {columnName} = @OldValue", sqlconnect)
                checkCmd.Parameters.AddWithValue("@OldValue", oldValue)

                Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                If exists = 0 Then
                    MessageBox.Show($"The value '{oldValue}' was not found in the selected table.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Exit Sub
                End If

                Dim updateCmd As New SqlCommand($"UPDATE {tableName} SET {columnName} = @NewValue WHERE {columnName} = @OldValue", sqlconnect)
                updateCmd.Parameters.AddWithValue("@OldValue", oldValue)
                updateCmd.Parameters.AddWithValue("@NewValue", newValue)
                updateCmd.ExecuteNonQuery()

                MessageBox.Show($"{selectedItem} updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

                SearchDtlsText.Text = ""
                UpdateDtlsText.Text = ""
                ItemCombo.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show("Error during update: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub IDInactive_Click(sender As Object, e As EventArgs) Handles IDInactive.Click
        Dim oldValue As String = SearchDtlsText.Text.Trim()
        Dim selectedItem As String = ItemCombo.SelectedItem?.ToString().Trim().ToUpper()

        If String.IsNullOrWhiteSpace(oldValue) Then
            MessageBox.Show("Old Values field must be filled.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(selectedItem) Then
            MessageBox.Show("Please select a category.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim tableColumnMap As New Dictionary(Of String, Tuple(Of String, String)) From {
        {"GROUP", Tuple.Create("ItemGroup_Table", "ItemGroup_Name")},
        {"MODEL", Tuple.Create("ItemModel_Table", "ItemModel_Name")},
        {"BRAND", Tuple.Create("ItemBrand_Table", "ItemBrand_Name")},
        {"UNIT", Tuple.Create("ItemUnit_Table", "ItemUnit_Name")}
    }

        Dim itemUsageColumnMap As New Dictionary(Of String, String) From {
        {"GROUP", "ItemGroup_ID"},
        {"MODEL", "ItemModel_ID"},
        {"BRAND", "ItemBrand_ID"},
        {"UNIT", "ItemUnit_ID"}
    }

        If Not tableColumnMap.ContainsKey(selectedItem) Then
            MessageBox.Show("Invalid selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If

        Dim tableName = tableColumnMap(selectedItem).Item1
        Dim columnName = tableColumnMap(selectedItem).Item2
        Dim itemColumn = itemUsageColumnMap(selectedItem)

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()

            Try
                Dim getInfoCmd As New SqlCommand(
                $"SELECT ID, Active FROM {tableName} WHERE {columnName} = @OldValue", sqlconnect)

                getInfoCmd.Parameters.AddWithValue("@OldValue", oldValue)

                Using rdr = getInfoCmd.ExecuteReader()
                    If Not rdr.Read() Then
                        MessageBox.Show($"'{oldValue}' not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Exit Sub
                    End If

                    Dim masterID As Integer = Convert.ToInt32(rdr("ID"))
                    Dim currentActive As Integer = Convert.ToInt32(rdr("Active")) ' 0 or 1
                    rdr.Close()

                    If currentActive = 0 Then
                        Dim usageCmd As New SqlCommand(
                        $"SELECT COUNT(*) FROM Item_Table WHERE {itemColumn} = @ID", sqlconnect)

                        usageCmd.Parameters.AddWithValue("@ID", masterID)

                        Dim usageCount As Integer = Convert.ToInt32(usageCmd.ExecuteScalar())

                        If usageCount > 0 Then
                            MessageBox.Show(
                            $"{selectedItem} '{oldValue}' is already used in Item records and cannot be inactivated.",
                            "Action Blocked",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
                            Exit Sub
                        End If
                    End If

                    ' Toggle status
                    Dim newActive As Integer = If(currentActive = 0, 1, 0)

                    Dim updateCmd As New SqlCommand(
                    $"UPDATE {tableName} SET Active = @NewActive WHERE ID = @ID", sqlconnect)

                    updateCmd.Parameters.AddWithValue("@NewActive", newActive)
                    updateCmd.Parameters.AddWithValue("@ID", masterID)
                    updateCmd.ExecuteNonQuery()

                    Dim statusText As String = If(newActive = 0, "activated", "inactivated")
                    MessageBox.Show(
                    $"{selectedItem} '{oldValue}' has been {statusText}.",
                    "Success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information)
                End Using

                SearchDtlsText.Clear()
                UpdateDtlsText.Clear()
                ItemCombo.SelectedIndex = -1

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub



    Private Sub ItemCombo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ItemCombo.SelectedIndexChanged
        Dim selectedCategory As String = ItemCombo.SelectedItem?.ToString().Trim()

        If String.IsNullOrWhiteSpace(selectedCategory) Then
            ItemListBox.Visible = False
            ItemListBox.DataSource = Nothing
            Exit Sub
        End If

        Dim query As String = ""

        Select Case selectedCategory.ToUpper()
            Case "GROUP"
                query = "SELECT ItemGroup_Name FROM ItemGroup_Table"
            Case "BRAND"
                query = "SELECT ItemBrand_Name FROM ItemBrand_Table"
            Case "MODEL"
                query = "SELECT ItemModel_Name FROM ItemModel_Table"
            Case "UNIT"
                query = "SELECT ItemUnit_Name FROM ItemUnit_Table"
            Case Else
                ItemListBox.Visible = False
                ItemListBox.DataSource = Nothing
                Exit Sub
        End Select

        Try
            currentItemList = LoadItemDataTable(query)
            SearchDtlsText.Text = ""

            If currentItemList IsNot Nothing AndAlso currentItemList.Rows.Count > 0 Then
                FilterItemList(SearchDtlsText.Text.Trim())
                ItemListBox.Visible = True
            Else
                ItemListBox.DataSource = Nothing
                ItemListBox.Visible = False
            End If
        Catch ex As Exception
            MessageBox.Show("Error loading list: " & ex.Message)
        End Try
    End Sub

    Private Sub FilterItemList(filterText As String)
        If currentItemList Is Nothing OrElse currentItemList.Rows.Count = 0 Then
            ItemListBox.DataSource = Nothing
            ItemListBox.Visible = False
            Exit Sub
        End If

        Dim dv As New DataView(currentItemList)
        Dim columnName As String = currentItemList.Columns(0).ColumnName
        filterText = filterText.Replace("'", "''")

        If Not String.IsNullOrWhiteSpace(filterText) Then
            dv.RowFilter = $"[{columnName}] LIKE '%{filterText}%'"
        End If

        suppressSelection = True
        If dv.Count > 0 Then
            ItemListBox.DataSource = dv
            ItemListBox.DisplayMember = columnName
            ItemListBox.Visible = True
            ItemListBox.SelectedIndex = 0
        Else
            ItemListBox.DataSource = Nothing
            ItemListBox.Visible = False
        End If
        suppressSelection = False
    End Sub

    Private Sub ItemListBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ItemListBox.SelectedIndexChanged
        If Not suppressSelection AndAlso ItemListBox.Focused Then
            SetSelectedListItem()
        End If
    End Sub

    Private Sub SearchDtlsText_TextChanged(sender As Object, e As EventArgs) Handles SearchDtlsText.TextChanged
        FilterItemList(SearchDtlsText.Text.Trim())
    End Sub

    Private Sub SearchDtlsText_KeyDown(sender As Object, e As KeyEventArgs) Handles SearchDtlsText.KeyDown
        If ItemListBox.Visible Then
            If e.KeyCode = Keys.Down AndAlso ItemListBox.Items.Count > 0 Then
                ItemListBox.Focus()
                If ItemListBox.SelectedIndex < 0 AndAlso ItemListBox.Items.Count > 0 Then
                    ItemListBox.SelectedIndex = 0
                End If
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub ItemListBox_KeyDown(sender As Object, e As KeyEventArgs) Handles ItemListBox.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                SetSelectedListItem()
                SearchDtlsText.Focus()
                e.Handled = True

            Case Keys.Escape
                ItemListBox.Visible = False
                SearchDtlsText.Focus()
                e.Handled = True
        End Select
    End Sub

    Private Sub ItemListBox_Click(sender As Object, e As EventArgs) Handles ItemListBox.Click
        SetSelectedListItem()
    End Sub

    Private Sub SetSelectedListItem()
        If ItemListBox.SelectedItem IsNot Nothing Then
            Dim selectedValue As String = ""

            If TypeOf ItemListBox.SelectedItem Is DataRowView Then
                selectedValue = DirectCast(ItemListBox.SelectedItem, DataRowView)(0).ToString()
            Else
                selectedValue = ItemListBox.SelectedItem.ToString()
            End If

            SearchDtlsText.Text = selectedValue
            ItemListBox.Visible = False
        End If
    End Sub

    Private Function LoadItemDataTable(query As String) As DataTable
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                Using da As New SqlDataAdapter(query, conn)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    Return dt
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading data table: " & ex.Message)
            Return New DataTable()
        End Try
    End Function

    'Item Page Details End

    'Jobcard Page Details Stared

    Private Function GetNoteProcessIDByName(searchName As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM NoteProcessing_Table WHERE Name = @Name"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Name", searchName)
                    sqlconnect.Open()
                    Dim result = sqlcommand.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return Convert.ToInt32(result)
                    Else
                        Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error getting Note Process ID: " & ex.Message)
            Return 0
        End Try
    End Function
    Private Sub NoteProcessSave_Click(sender As Object, e As EventArgs) Handles NoteProcessSave.Click
        Dim NoteProcessEng As String = NoteProcessEnglish.Text.Trim()
        Dim NoteProcessTam As String = NoteProcessTamil.Text.Trim()
        Dim searchName As String = NPMTextBox.Text.Trim()

        If String.IsNullOrWhiteSpace(NoteProcessEng) Then
            MessageBox.Show("Note Process name cannot be empty.")
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(NoteProcessTam) Then
            MessageBox.Show("Note Process Tamil name cannot be empty.")
            Exit Sub
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim processId As Integer = GetNoteProcessIDByName(searchName)

                If processId > 0 Then
                    ' Update
                    Dim updateCommand As New SqlCommand("UPDATE NoteProcessing_Table SET Name = @Name, TamilName = @TamilName WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@Name", NoteProcessEng)
                    updateCommand.Parameters.AddWithValue("@TamilName", NoteProcessTam)
                    updateCommand.Parameters.AddWithValue("@ID", processId)

                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("Note Process updated successfully.")
                Else
                    ' Insert
                    Dim insertCommand As New SqlCommand("INSERT INTO NoteProcessing_Table (Name, TamilName) VALUES (@Name, @TamilName)", sqlconnect)
                    insertCommand.Parameters.AddWithValue("@Name", NoteProcessEng)
                    insertCommand.Parameters.AddWithValue("@TamilName", NoteProcessTam)

                    insertCommand.ExecuteNonQuery()
                    MessageBox.Show("Note Process saved successfully.")
                End If
                NoteProcessEnglish.Text = ""
                NoteProcessTamil.Text = ""
                NPMTextBox.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadAutoCompleteData()
    End Sub

    Private Function GetNoteSizeIDByName(searchName As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM NoteSize_Table WHERE Name = @Name"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Name", searchName)
                    sqlconnect.Open()
                    Dim result = sqlcommand.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return Convert.ToInt32(result)
                    Else
                        Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error getting Note Size ID: " & ex.Message)
            Return 0
        End Try
    End Function
    Private Function GetNoteTypeIDByName(searchName As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM NoteType_table WHERE Name = @Name"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Name", searchName)
                    sqlconnect.Open()
                    Dim result = sqlcommand.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return Convert.ToInt32(result)
                    Else
                        Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error getting Note Size ID: " & ex.Message)
            Return 0
        End Try
    End Function

    Private Sub NoteSizeSave_Click(sender As Object, e As EventArgs) Handles NoteSizeSave.Click
        Dim NoteSizeEng As String = NoteSizeEnglish.Text.Trim()
        Dim NoteSizeTam As String = NoteSizeTamil.Text.Trim()
        Dim searchName As String = NSTextBox.Text.Trim()

        If String.IsNullOrWhiteSpace(NoteSizeEng) Then
            MessageBox.Show("Note Size name cannot be empty.")
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(NoteSizeTam) Then
            MessageBox.Show("Note Size Tamil name cannot be empty.")
            Exit Sub
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim noteSizeId As Integer = GetNoteSizeIDByName(searchName)

                If noteSizeId > 0 Then
                    Dim updateCommand As New SqlCommand("UPDATE NoteSize_Table SET Name = @Name, TamilName = @TamilName WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@Name", NoteSizeEng)
                    updateCommand.Parameters.AddWithValue("@TamilName", NoteSizeTam)
                    updateCommand.Parameters.AddWithValue("@ID", noteSizeId)

                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("Note Size updated.")
                Else
                    Dim insertCommand As New SqlCommand("INSERT INTO NoteSize_Table (Name, TamilName) VALUES (@Name, @TamilName)", sqlconnect)
                    insertCommand.Parameters.AddWithValue("@Name", NoteSizeEng)
                    insertCommand.Parameters.AddWithValue("@TamilName", NoteSizeTam)

                    insertCommand.ExecuteNonQuery()
                    MessageBox.Show("Note Size saved.")
                End If

                NoteSizeEnglish.Text = ""
                NoteSizeTamil.Text = ""
                NSTextBox.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadAutoCompleteData()
    End Sub
    Private Sub NoteTypeSave_Click(sender As Object, e As EventArgs) Handles NoteTypeSave.Click
        Dim NoteTypeEng As String = NoteTypeEnglish.Text.Trim()
        Dim NoteTypeTam As String = NoteTypeTamil.Text.Trim()
        Dim searchName As String = NTTextBox.Text.Trim()

        If String.IsNullOrWhiteSpace(NoteTypeEng) Then
            MessageBox.Show("Note Size name cannot be empty.")
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(NoteTypeTam) Then
            MessageBox.Show("Note Size Tamil name cannot be empty.")
            Exit Sub
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim noteTypeId As Integer = GetNoteTypeIDByName(searchName)

                If noteTypeId > 0 Then
                    Dim updateCommand As New SqlCommand("UPDATE NoteType_table SET Name = @Name, TamilName = @TamilName WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@Name", NoteTypeEng)
                    updateCommand.Parameters.AddWithValue("@TamilName", NoteTypeTam)
                    updateCommand.Parameters.AddWithValue("@ID", noteTypeId)

                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("Note Type updated.")
                Else
                    Dim insertCommand As New SqlCommand("INSERT INTO NoteType_table (Name, TamilName) VALUES (@Name, @TamilName)", sqlconnect)
                    insertCommand.Parameters.AddWithValue("@Name", NoteTypeEng)
                    insertCommand.Parameters.AddWithValue("@TamilName", NoteTypeTam)

                    insertCommand.ExecuteNonQuery()
                    MessageBox.Show("Note Type saved.")
                End If

                NoteTypeEnglish.Text = ""
                NoteTypeTamil.Text = ""
                NTTextBox.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadAutoCompleteData()
    End Sub

    Private Sub LoadAutoCompleteData()
        Try
            dtnpm = LoadDataTable("SELECT Name FROM NoteProcessing_table where Active = 0 order by Name Asc")
            dtns = LoadDataTable("SELECT Name FROM Notesize_table  where Active = 0  order by Name Asc")
            dtnt = LoadDataTable("SELECT Name FROM NoteType_table  where Active = 0  order by Name Asc")
            dtaddons = LoadDataTable("Select Processing_Method from Addons_Master  where Active = 0  order by Processing_Method Asc")
            KryptonListBox.Visible = False
        Catch ex As Exception
            MessageBox.Show("Error loading autocomplete data: " & ex.Message)
        End Try
    End Sub
    Private Function LoadDataTable(query As String) As DataTable
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                Using da As New SqlDataAdapter(query, conn)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    Return dt
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading data table: " & ex.Message)
            Return New DataTable()
        End Try
    End Function
    Private Sub ShowSuggestionsForTextBox(textBox As Guna.UI2.WinForms.Guna2TextBox)
        Try
            activeTextbox = textBox

            Dim source As DataTable = If(textBox Is NPMTextBox, dtnpm,
                                  If(textBox Is NSTextBox, dtns,
                                  If(textBox Is NTTextBox, dtnt,
                                   If(textBox Is JCSearchtxt, dtaddons, Nothing))))

            If source Is Nothing Then
                KryptonListBox.Visible = False
                Exit Sub
            End If

            Dim dv As New DataView(source)
            Dim filterText As String = textBox.Text.Replace("'", "''")
            Dim columnName As String = source.Columns(0).ColumnName

            If Not String.IsNullOrWhiteSpace(filterText) Then
                dv.RowFilter = $"[{columnName}] LIKE '%{filterText}%'"
            End If

            If dv.Count = 0 Then
                KryptonListBox.Visible = False
                Exit Sub
            End If

            KryptonListBox.DataSource = dv
            KryptonListBox.DisplayMember = columnName
            KryptonListBox.Visible = True
            KryptonListBox.Location = New Point(textBox.Left, textBox.Top + textBox.Height)
            KryptonListBox.Width = textBox.Width
            KryptonListBox.SelectedIndex = 0
        Catch ex As Exception
            MessageBox.Show("Error showing suggestions: " & ex.Message)
        End Try
    End Sub

    Private Sub Guna2TextBox_TextChanged(sender As Object, e As EventArgs) _
    Handles NPMTextBox.TextChanged, NSTextBox.TextChanged, JCSearchtxt.TextChanged, NTTextBox.TextChanged

        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            ShowSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If

    End Sub
    Private Sub Guna2TextBox_Click(sender As Object, e As EventArgs) _
    Handles NPMTextBox.Click, NSTextBox.Click, JCSearchtxt.Click, NTTextBox.Click

        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            ShowSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If
    End Sub
    Private Function IsValidInput(textBox As Guna.UI2.WinForms.Guna2TextBox, source As DataTable) As Boolean
        Dim filterText As String = textBox.Text.Trim().Replace("'", "''")
        If source Is Nothing OrElse source.Rows.Count = 0 Then Return False

        For Each row As DataRow In source.Rows
            If String.Compare(row(0).ToString(), filterText, True) = 0 Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub KryptonListBox_Click(sender As Object, e As EventArgs)
        listBoxSelectionMade = True
        If KryptonListBox.SelectedItem IsNot Nothing AndAlso activeTextbox IsNot Nothing Then
            activeTextbox.Text = DirectCast(KryptonListBox.SelectedItem, DataRowView)(0).ToString()
            KryptonListBox.Visible = False
            activeTextbox.SelectionStart = activeTextbox.Text.Length
            activeTextbox.Focus()
        End If
    End Sub

    Private Sub KryptonListBox_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            KryptonListBox_Click(sender, e)
        ElseIf e.KeyCode = Keys.Escape Then
            KryptonListBox.Visible = False
        End If
    End Sub

    Private Sub AnyTextBox_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles NPMTextBox.KeyDown, NSTextBox.KeyDown, JCSearchtxt.KeyDown, NTTextBox.KeyDown

        If KryptonListBox.Visible Then
            If e.KeyCode = Keys.Down Then
                KryptonListBox.Focus()
                If KryptonListBox.Items.Count > 0 Then KryptonListBox.SelectedIndex = 0
                e.Handled = True
            End If
        End If

    End Sub
    Private Async Sub TextBox_LostFocus(sender As Object, e As EventArgs) _
    Handles NPMTextBox.LostFocus, NSTextBox.LostFocus, JCSearchtxt.LostFocus, NTTextBox.LostFocus
        Await Task.Delay(200)

        If Not KryptonListBox.Focused AndAlso Not listBoxSelectionMade Then
            KryptonListBox.Visible = False
            If Not IsValidInput(activeTextbox, GetCorrespondingDataTable(activeTextbox)) Then
                activeTextbox.Clear()
            End If
        End If

        listBoxSelectionMade = False
    End Sub
    Private Function GetCorrespondingDataTable(tb As Guna.UI2.WinForms.Guna2TextBox) As DataTable
        If tb Is NPMTextBox Then
            Return dtnpm
        ElseIf tb Is NSTextBox Then
            Return dtns
        ElseIf tb Is NTTextBox Then
            Return dtnt
        Else
            Return Nothing
        End If
    End Function

    Private Sub KryptonListBox_LostFocus(sender As Object, e As EventArgs)
        KryptonListBox.Visible = False
    End Sub

    Private Sub AnyTextBox_GotFocus(sender As Object, e As EventArgs) _
    Handles NSTextBox.GotFocus, NPMTextBox.GotFocus, JCSearchtxt.GotFocus

        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            ShowSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If

    End Sub

    Private Sub NPMSearchbtn_Click(sender As Object, e As EventArgs) Handles NPMSearchbtn.Click
        Dim NPMName As String = NPMTextBox.Text.Trim()

        If String.IsNullOrWhiteSpace(NPMName) Then
            MessageBox.Show("Please enter an item name to search.")
            Return
        End If

        Using conn As SqlConnection = Tools.GetConnection()
            Dim cmd As New SqlCommand("SELECT * FROM NoteProcessing_table WHERE name = @name", conn)
            cmd.Parameters.AddWithValue("@name", NPMName)

            Try
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        While reader.Read()
                            NoteProcessEnglish.Text = reader("Name").ToString()
                            NoteProcessTamil.Text = reader("TamilName").ToString()
                        End While
                    Else
                        MessageBox.Show("NPM Name not found.")
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub NSSearchButton_Click(sender As Object, e As EventArgs) Handles NSSearchButton.Click
        Dim NSName As String = NSTextBox.Text.Trim()

        If String.IsNullOrWhiteSpace(NSName) Then
            MessageBox.Show("Please enter an item name to search.")
            Return
        End If

        Using conn As SqlConnection = Tools.GetConnection()
            Dim cmd As New SqlCommand("SELECT * FROM Notesize_table WHERE name = @name", conn)
            cmd.Parameters.AddWithValue("@name", NSName)

            Try
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        While reader.Read()
                            NoteSizeEnglish.Text = reader("Name").ToString()
                            NoteSizeTamil.Text = reader("TamilName").ToString()
                        End While
                    Else
                        MessageBox.Show("NS Name not found.")
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Sub NTSearchButton_Click(sender As Object, e As EventArgs) Handles NTSearchButton.Click
        Dim NTName As String = NTTextBox.Text.Trim()

        If String.IsNullOrWhiteSpace(NTName) Then
            MessageBox.Show("Please enter an item name to search.")
            Return
        End If

        Using conn As SqlConnection = Tools.GetConnection()
            Dim cmd As New SqlCommand("SELECT * FROM NoteType_table WHERE name = @name", conn)
            cmd.Parameters.AddWithValue("@name", NTName)

            Try
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        While reader.Read()
                            NoteTypeEnglish.Text = reader("Name").ToString()
                            NoteTypeTamil.Text = reader("TamilName").ToString()
                        End While
                    Else
                        MessageBox.Show("NT Name not found.")
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Function GetAddonsName(AddonsName As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM Addons_Master WHERE Processing_Method = @Processing_Method"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Processing_Method", AddonsName.Trim())
                    sqlconnect.Open()
                    Dim result = sqlcommand.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return Convert.ToInt32(result)
                    Else
                        Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error getting Addons ID: " & ex.Message)
            Return 0
        End Try
    End Function

    Private Sub AddonsButton_Click(sender As Object, e As EventArgs) Handles AddonsButton.Click
        Dim Addonstxt As String = JCupdatetxt.Text.Trim()
        Dim AddonsName As String = JCSearchtxt.Text.Trim()

        If String.IsNullOrWhiteSpace(Addonstxt) Then
            MessageBox.Show("Note Process name cannot be empty.")
            Exit Sub
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                Dim processId As Integer = GetAddonsName(AddonsName)

                If processId > 0 Then
                    Dim updateCommand As New SqlCommand("UPDATE Addons_Master SET Processing_Method = @Name WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@Name", Addonstxt)
                    updateCommand.Parameters.AddWithValue("@ID", processId)
                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("Note Process updated successfully.")
                Else
                    Dim insertCommand As New SqlCommand("INSERT INTO Addons_Master (Processing_Method) VALUES (@Name)", sqlconnect)
                    insertCommand.Parameters.AddWithValue("@Name", Addonstxt)
                    insertCommand.ExecuteNonQuery()
                    MessageBox.Show("Note Process saved successfully.")
                End If

                JCupdatetxt.Text = ""
                JCSearchtxt.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using

        LoadAutoCompleteData()
    End Sub

    Private Sub AddonsSearch_Click(sender As Object, e As EventArgs) Handles AddonsSearch.Click
        Dim AddonsName As String = JCSearchtxt.Text.Trim()

        If String.IsNullOrWhiteSpace(AddonsName) Then
            MessageBox.Show("Please enter an Addons name to search.")
            Return
        End If

        Using conn As SqlConnection = Tools.GetConnection()
            Dim cmd As New SqlCommand("SELECT * FROM Addons_Master WHERE Processing_Method = @name", conn)
            cmd.Parameters.AddWithValue("@name", AddonsName)

            Try
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        While reader.Read()
                            JCupdatetxt.Text = reader("Processing_Method").ToString()
                        End While
                    Else
                        MessageBox.Show("Addons Name not found.")
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    'Jobcard Page Details End

    'Printing Page Details Stared

    Private Sub LoadPrintingData()
        Try
            dtpm = PrintingLoadDataTable("SELECT Name FROM PrintingMethod_table")
            dtpt = PrintingLoadDataTable("SELECT Name FROM PrintingType_table")
            dtpme = PrintingLoadDataTable("Select Name from PrintingMachine_table")
            dtpi = PrintingLoadDataTable("Select Name from PrintingItem_table")
            KryptonListBox1.Visible = False
        Catch ex As Exception
            MessageBox.Show("Error loading autocomplete data: " & ex.Message)
        End Try
    End Sub
    Private Function PrintingLoadDataTable(query As String) As DataTable
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                Using da As New SqlDataAdapter(query, conn)
                    Dim dt As New DataTable()
                    da.Fill(dt)
                    Return dt
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading data table: " & ex.Message)
            Return New DataTable()
        End Try
    End Function
    Private Sub PrintingSuggestionsForTextBox(PrinttextBox As Guna.UI2.WinForms.Guna2TextBox)
        Try
            PactiveTextbox = PrinttextBox

            Dim source As DataTable = If(PrinttextBox Is PMSearchTxt, dtpm,
                                  If(PrinttextBox Is PTSearchTxt, dtpt,
                                   If(PrinttextBox Is PMESearchTxt, dtpme,
                                   If(PrinttextBox Is PISearchTxt, dtpi, Nothing))))

            If source Is Nothing Then
                KryptonListBox1.Visible = False
                Exit Sub
            End If

            Dim dv As New DataView(source)
            Dim filterText As String = PrinttextBox.Text.Replace("'", "''")
            Dim columnName As String = source.Columns(0).ColumnName

            If Not String.IsNullOrWhiteSpace(filterText) Then
                dv.RowFilter = $"[{columnName}] LIKE '%{filterText}%'"
            End If

            If dv.Count = 0 Then
                KryptonListBox1.Visible = False
                Exit Sub
            End If

            KryptonListBox1.DataSource = dv
            KryptonListBox1.DisplayMember = columnName
            KryptonListBox1.Visible = True
            KryptonListBox1.Location = New Point(PrinttextBox.Left, PrinttextBox.Top + PrinttextBox.Height)
            KryptonListBox1.Width = PrinttextBox.Width
            KryptonListBox1.SelectedIndex = 0
        Catch ex As Exception
            MessageBox.Show("Error showing suggestions: " & ex.Message)
        End Try
    End Sub


    Private Sub Guna2TextBox1_TextChanged(sender As Object, e As EventArgs) _
    Handles PMSearchTxt.TextChanged, PTSearchTxt.TextChanged, PMESearchTxt.TextChanged, PISearchTxt.TextChanged

        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            PrintingSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If

    End Sub
    Private Sub Guna2TextBox1_Click(sender As Object, e As EventArgs) _
    Handles PMSearchTxt.Click, PTSearchTxt.Click, PMESearchTxt.Click, PISearchTxt.Click

        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            PrintingSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If
    End Sub
    Private Function PrintIsValidInput(PrinttextBox As Guna.UI2.WinForms.Guna2TextBox, source As DataTable) As Boolean
        Dim filterText As String = PrinttextBox.Text.Trim().Replace("'", "''")
        If source Is Nothing OrElse source.Rows.Count = 0 Then Return False

        For Each row As DataRow In source.Rows
            If String.Compare(row(0).ToString(), filterText, True) = 0 Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Sub KryptonListBox1_Click(sender As Object, e As EventArgs)
        listBoxSelectionMade = True
        If KryptonListBox1.SelectedItem IsNot Nothing AndAlso PactiveTextbox IsNot Nothing Then
            PactiveTextbox.Text = DirectCast(KryptonListBox1.SelectedItem, DataRowView)(0).ToString()
            KryptonListBox1.Visible = False
            PactiveTextbox.SelectionStart = PactiveTextbox.Text.Length
            PactiveTextbox.Focus()
        End If
    End Sub

    Private Sub KryptonListBox1_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            KryptonListBox1_Click(sender, e)
        ElseIf e.KeyCode = Keys.Escape Then
            KryptonListBox1.Visible = False
        End If
    End Sub

    Private Sub PrintTextBox_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles PMSearchTxt.KeyDown, PTSearchTxt.KeyDown, PMESearchTxt.KeyDown, PISearchTxt.KeyDown

        If KryptonListBox1.Visible Then
            If e.KeyCode = Keys.Down Then
                KryptonListBox1.Focus()
                If KryptonListBox1.Items.Count > 0 Then KryptonListBox1.SelectedIndex = 0
                e.Handled = True
            End If
        End If

    End Sub

    Private Async Sub TextBox1_LostFocus(sender As Object, e As EventArgs) _
    Handles PMSearchTxt.LostFocus, PTSearchTxt.LostFocus, PMESearchTxt.LostFocus, PISearchTxt.LostFocus
        Await Task.Delay(200)

        If Not KryptonListBox1.Focused AndAlso Not listBoxSelectionMade Then
            KryptonListBox1.Visible = False
            If Not PrintIsValidInput(PactiveTextbox, GetPrintingDataTable(PactiveTextbox)) Then
                PactiveTextbox.Clear()
            End If
        End If

        listBoxSelectionMade = False
    End Sub

    Private Function GetPrintingDataTable(tb As Guna.UI2.WinForms.Guna2TextBox) As DataTable
        If tb Is PMSearchTxt Then
            Return dtpm
        ElseIf tb Is PTSearchTxt Then
            Return dtpt
        ElseIf tb Is PMESearchTxt Then
            Return dtpme
        ElseIf tb Is PISearchTxt Then
            Return dtpi
        Else
            Return Nothing
        End If
    End Function

    Private Sub KryptonListBox1_LostFocus(sender As Object, e As EventArgs)
        KryptonListBox1.Visible = False
    End Sub

    Private Sub PrintTextBox_GotFocus(sender As Object, e As EventArgs) _
    Handles PMSearchTxt.GotFocus, PTSearchTxt.GotFocus, PMESearchTxt.GotFocus, PISearchTxt.GotFocus

        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            PrintingSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If
    End Sub
    Private Sub PMSBtn_Click(sender As Object, e As EventArgs) Handles PMSBtn.Click
        Dim PMName As String = PMSearchTxt.Text.Trim()

        If String.IsNullOrWhiteSpace(PMName) Then
            MessageBox.Show("Please enter an item name to search.")
            Return
        End If

        Using conn As SqlConnection = Tools.GetConnection()
            Dim cmd As New SqlCommand("SELECT * FROM PrintingMethod_table WHERE name = @name", conn)
            cmd.Parameters.AddWithValue("@name", PMName)

            Try
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        While reader.Read()
                            PMETxt.Text = reader("Name").ToString()
                            PMTTxt.Text = reader("TamilName").ToString()
                        End While
                    Else
                        MessageBox.Show("PM Name not found.")
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub PISBtn_Click(sender As Object, e As EventArgs) Handles PISBtn.Click
        Dim PIName As String = PISearchTxt.Text.Trim()

        If String.IsNullOrWhiteSpace(PIName) Then
            MessageBox.Show("Please enter an item name to search.")
            Return
        End If

        Using conn As SqlConnection = Tools.GetConnection()
            Dim cmd As New SqlCommand("SELECT * FROM PrintingItem_table WHERE name = @name", conn)
            cmd.Parameters.AddWithValue("@name", PIName)

            Try
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        While reader.Read()
                            PIETxt.Text = reader("Name").ToString()
                            PITTxt.Text = reader("TamilName").ToString()
                        End While
                    Else
                        MessageBox.Show("PI Name not found.")
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub PTSBtn_Click(sender As Object, e As EventArgs) Handles PTSBtn.Click
        Dim PTName As String = PTSearchTxt.Text.Trim()

        If String.IsNullOrWhiteSpace(PTName) Then
            MessageBox.Show("Please enter an item name to search.")
            Return
        End If

        Using conn As SqlConnection = Tools.GetConnection()
            Dim cmd As New SqlCommand("SELECT * FROM PrintingType_table WHERE name = @name", conn)
            cmd.Parameters.AddWithValue("@name", PTName)

            Try
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        While reader.Read()
                            PTETxt.Text = reader("Name").ToString()
                            PTTTxt.Text = reader("TamilName").ToString()
                        End While
                    Else
                        MessageBox.Show("PT Name not found.")
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub

    Private Sub PMESBtn_Click(sender As Object, e As EventArgs) Handles PMESBtn.Click
        Dim PMEName As String = PMESearchTxt.Text.Trim()

        If String.IsNullOrWhiteSpace(PMEName) Then
            MessageBox.Show("Please enter an item name to search.")
            Return
        End If

        Using conn As SqlConnection = Tools.GetConnection()
            Dim cmd As New SqlCommand("SELECT * FROM PrintingMachine_table WHERE name = @name", conn)
            cmd.Parameters.AddWithValue("@name", PMEName)

            Try
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        While reader.Read()
                            PMEETxt.Text = reader("Name").ToString()
                            PMETTxt.Text = reader("TamilName").ToString()
                        End While
                    Else
                        MessageBox.Show("PME Name not found.")
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
    End Sub
    Private Function GetPMIDByName(searchName As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM PrintingMethod_table WHERE Name = @Name"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Name", searchName)
                    sqlconnect.Open()
                    Dim result = sqlcommand.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return Convert.ToInt32(result)
                    Else
                        Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error getting Note Process ID: " & ex.Message)
            Return 0
        End Try
    End Function

    Private Sub NTInactiveBtn_Click(sender As Object, e As EventArgs) Handles NTInactiveBtn.Click
        Dim searchName As String = NTTextBox.Text.Trim()

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim noteTypeId As Integer = GetNoteTypeIDByName(searchName)

                If noteTypeId > 0 Then
                    Dim updateCommand As New SqlCommand("UPDATE NoteType_table SET active = 1 WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@ID", noteTypeId)
                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("InActive successfully.")
                Else
                    MessageBox.Show("No Value Found")
                End If

                NoteTypeEnglish.Text = ""
                NoteTypeTamil.Text = ""
                NTTextBox.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadAutoCompleteData()
    End Sub

    Private Sub NPMInactivebtn_Click(sender As Object, e As EventArgs) Handles NPMInactivebtn.Click
        Dim searchName As String = NPMTextBox.Text.Trim()
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim processId As Integer = GetNoteProcessIDByName(searchName)

                If processId > 0 Then
                    Dim updateCommand As New SqlCommand("UPDATE NoteProcessing_table SET active = 1 WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@ID", processId)

                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("InActive successfully.")
                Else
                    MessageBox.Show("No Value Found")
                End If
                NoteProcessEnglish.Text = ""
                NoteProcessTamil.Text = ""
                NPMTextBox.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadAutoCompleteData()
    End Sub

    Private Sub NSInactiveBtn_Click(sender As Object, e As EventArgs) Handles NSInactiveBtn.Click
        Dim searchName As String = NSTextBox.Text.Trim()

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim noteSizeId As Integer = GetNoteSizeIDByName(searchName)

                If noteSizeId > 0 Then
                    Dim updateCommand As New SqlCommand("UPDATE Notesize_table SET active = 1 WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@ID", noteSizeId)
                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("InActive successfully.")
                Else
                    MessageBox.Show("No Value Found")
                End If

                NoteSizeEnglish.Text = ""
                NoteSizeTamil.Text = ""
                NSTextBox.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadAutoCompleteData()
    End Sub

    Private Sub AddInactiveBtn_Click(sender As Object, e As EventArgs) Handles AddInactiveBtn.Click
        Dim AddonsName As String = JCSearchtxt.Text.Trim()
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()
                Dim processId As Integer = GetAddonsName(AddonsName)

                If processId > 0 Then
                    Dim updateCommand As New SqlCommand("UPDATE Addons_Master SET active = 1 WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@ID", processId)
                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("InActive successfully.")
                Else
                    MessageBox.Show("No Value Found")
                End If

                JCupdatetxt.Text = ""
                JCSearchtxt.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using

        LoadAutoCompleteData()
    End Sub

    Private Sub PMSActiveBtn_Click(sender As Object, e As EventArgs) Handles PMSActiveBtn.Click
        Dim searchName As String = PMSearchTxt.Text.Trim()

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim processId As Integer = GetPMIDByName(searchName)

                If processId > 0 Then
                    Dim updateCommand As New SqlCommand("UPDATE PrintingMethod_table SET Active = 1 WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@ID", processId)

                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("InActive successfully.")
                Else
                    MessageBox.Show("No Value Found")
                End If
                PMSearchTxt.Text = ""
                PMETxt.Text = ""
                PMTTxt.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadPrintingData()
    End Sub

    Private Sub PISActiveBtn_Click(sender As Object, e As EventArgs) Handles PISActiveBtn.Click
        Dim searchName As String = PISearchTxt.Text.Trim()

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim processId As Integer = GetPIEIDByName(searchName)

                If processId > 0 Then
                    Dim updateCommand As New SqlCommand("UPDATE PrintingItem_table SET Active = 1 WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@ID", processId)

                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("InActive successfully.")
                Else
                    MessageBox.Show("No Value Found")
                End If
                PISearchTxt.Text = ""
                PIETxt.Text = ""
                PITTxt.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadPrintingData()
    End Sub

    Private Sub PTActiveBtn_Click(sender As Object, e As EventArgs) Handles PTActiveBtn.Click
        Dim searchName As String = PTSearchTxt.Text.Trim()

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim processId As Integer = GetPTIDByName(searchName)

                If processId > 0 Then
                    Dim updateCommand As New SqlCommand("UPDATE PrintingType_table SET Active = 1 WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@ID", processId)

                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("InActive successfully.")
                Else
                    MessageBox.Show("No Value Found")
                End If
                PTSearchTxt.Text = ""
                PTETxt.Text = ""
                PTTTxt.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadPrintingData()
    End Sub

    Private Sub PMActiveBtn_Click(sender As Object, e As EventArgs) Handles PMActiveBtn.Click
        Dim searchName As String = PMESearchTxt.Text.Trim()

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim processId As Integer = GetPMEIDByName(searchName)

                If processId > 0 Then
                    Dim updateCommand As New SqlCommand("UPDATE PrintingMachine_table SET Active = 1 WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@ID", processId)

                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("InActive successfully.")
                Else
                    MessageBox.Show("No Value Found")
                End If
                PMESearchTxt.Text = ""
                PMEETxt.Text = ""
                PMETTxt.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadPrintingData()
    End Sub

    Private Sub PMSave_Click(sender As Object, e As EventArgs) Handles PMSave.Click
        Dim PMEng As String = PMETxt.Text.Trim()
        Dim PMTam As String = PMTTxt.Text.Trim()
        Dim searchName As String = PMSearchTxt.Text.Trim()

        If String.IsNullOrWhiteSpace(PMEng) Then
            MessageBox.Show("Note Process name cannot be empty.")
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(PMTam) Then
            MessageBox.Show("Note Process Tamil name cannot be empty.")
            Exit Sub
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim processId As Integer = GetPMIDByName(searchName)

                If processId > 0 Then
                    ' Update
                    Dim updateCommand As New SqlCommand("UPDATE PrintingMethod_table SET Name = @Name, TamilName = @TamilName WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@Name", PMEng)
                    updateCommand.Parameters.AddWithValue("@TamilName", PMTam)
                    updateCommand.Parameters.AddWithValue("@ID", processId)

                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("Printing Method updated successfully.")
                Else
                    ' Insert
                    Dim insertCommand As New SqlCommand("INSERT INTO PrintingMethod_table (Name, TamilName) VALUES (@Name, @TamilName)", sqlconnect)
                    insertCommand.Parameters.AddWithValue("@Name", PMEng)
                    insertCommand.Parameters.AddWithValue("@TamilName", PMTam)

                    insertCommand.ExecuteNonQuery()
                    MessageBox.Show("Printing Method saved successfully.")
                End If
                PMSearchTxt.Text = ""
                PMETxt.Text = ""
                PMTTxt.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadPrintingData()
    End Sub

    Private Sub PISave_Click(sender As Object, e As EventArgs) Handles PISave.Click
        Dim PIEng As String = PIETxt.Text.Trim()
        Dim PITam As String = PITTxt.Text.Trim()
        Dim searchName As String = PISearchTxt.Text.Trim()

        If String.IsNullOrWhiteSpace(PIEng) Then
            MessageBox.Show("Printing Item name cannot be empty.")
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(PITam) Then
            MessageBox.Show("Printing Item Tamil name cannot be empty.")
            Exit Sub
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim processId As Integer = GetPIEIDByName(searchName)

                If processId > 0 Then
                    ' Update
                    Dim updateCommand As New SqlCommand("UPDATE PrintingItem_table SET Name = @Name, TamilName = @TamilName WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@Name", PIEng)
                    updateCommand.Parameters.AddWithValue("@TamilName", PITam)
                    updateCommand.Parameters.AddWithValue("@ID", processId)

                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("Printing Item updated successfully.")
                Else
                    ' Insert
                    Dim insertCommand As New SqlCommand("INSERT INTO PrintingItem_table (Name, TamilName) VALUES (@Name, @TamilName)", sqlconnect)
                    insertCommand.Parameters.AddWithValue("@Name", PIEng)
                    insertCommand.Parameters.AddWithValue("@TamilName", PITam)

                    insertCommand.ExecuteNonQuery()
                    MessageBox.Show("Printing Item saved successfully.")
                End If
                PISearchTxt.Text = ""
                PIETxt.Text = ""
                PITTxt.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadPrintingData()
    End Sub

    Private Function GetPTIDByName(searchName As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM PrintingType_table WHERE Name = @Name"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Name", searchName)
                    sqlconnect.Open()
                    Dim result = sqlcommand.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return Convert.ToInt32(result)
                    Else
                        Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error getting Note Process ID: " & ex.Message)
            Return 0
        End Try
    End Function
    Private Sub PTSave_Click(sender As Object, e As EventArgs) Handles PTSave.Click
        Dim PTEng As String = PTETxt.Text.Trim()
        Dim PTTam As String = PTTTxt.Text.Trim()
        Dim searchName As String = PTSearchTxt.Text.Trim()

        If String.IsNullOrWhiteSpace(PTEng) Then
            MessageBox.Show("Note Process name cannot be empty.")
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(PTTam) Then
            MessageBox.Show("Note Process Tamil name cannot be empty.")
            Exit Sub
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim processId As Integer = GetPTIDByName(searchName)

                If processId > 0 Then
                    ' Update
                    Dim updateCommand As New SqlCommand("UPDATE PrintingType_table SET Name = @Name, TamilName = @TamilName WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@Name", PTEng)
                    updateCommand.Parameters.AddWithValue("@TamilName", PTTam)
                    updateCommand.Parameters.AddWithValue("@ID", processId)

                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("Printing Type updated successfully.")
                Else
                    ' Insert
                    Dim insertCommand As New SqlCommand("INSERT INTO PrintingType_table (Name, TamilName) VALUES (@Name, @TamilName)", sqlconnect)
                    insertCommand.Parameters.AddWithValue("@Name", PTEng)
                    insertCommand.Parameters.AddWithValue("@TamilName", PTTam)

                    insertCommand.ExecuteNonQuery()
                    MessageBox.Show("Printing Type saved successfully.")
                End If
                PTSearchTxt.Text = ""
                PTETxt.Text = ""
                PTTTxt.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadPrintingData()
    End Sub

    Private Function GetPMEIDByName(searchName As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM PrintingMachine_table WHERE Name = @Name"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Name", searchName)
                    sqlconnect.Open()
                    Dim result = sqlcommand.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return Convert.ToInt32(result)
                    Else
                        Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error getting Note Process ID: " & ex.Message)
            Return 0
        End Try
    End Function

    Private Function GetPIEIDByName(searchName As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM PrintingItem_table WHERE Name = @Name"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Name", searchName)
                    sqlconnect.Open()
                    Dim result = sqlcommand.ExecuteScalar()
                    If result IsNot Nothing Then
                        Return Convert.ToInt32(result)
                    Else
                        Return 0
                    End If
                End Using
            End Using
        Catch ex As Exception
            MessageBox.Show("Error getting Note Process ID: " & ex.Message)
            Return 0
        End Try
    End Function

    Private Sub PMESave_Click(sender As Object, e As EventArgs) Handles PMESave.Click
        Dim PMEEng As String = PMEETxt.Text.Trim()
        Dim PMETam As String = PMETTxt.Text.Trim()
        Dim searchName As String = PMESearchTxt.Text.Trim()

        If String.IsNullOrWhiteSpace(PMEEng) Then
            MessageBox.Show("Note Process name cannot be empty.")
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(PMETam) Then
            MessageBox.Show("Note Process Tamil name cannot be empty.")
            Exit Sub
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Try
                sqlconnect.Open()

                Dim processId As Integer = GetPMEIDByName(searchName)

                If processId > 0 Then
                    ' Update
                    Dim updateCommand As New SqlCommand("UPDATE PrintingMachine_table SET Name = @Name, TamilName = @TamilName WHERE ID = @ID", sqlconnect)
                    updateCommand.Parameters.AddWithValue("@Name", PMEEng)
                    updateCommand.Parameters.AddWithValue("@TamilName", PMETam)
                    updateCommand.Parameters.AddWithValue("@ID", processId)

                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("Printing Machine updated successfully.")
                Else
                    ' Insert
                    Dim insertCommand As New SqlCommand("INSERT INTO PrintingMachine_table (Name, TamilName) VALUES (@Name, @TamilName)", sqlconnect)
                    insertCommand.Parameters.AddWithValue("@Name", PMEEng)
                    insertCommand.Parameters.AddWithValue("@TamilName", PMETam)

                    insertCommand.ExecuteNonQuery()
                    MessageBox.Show("Printing Machine saved successfully.")
                End If
                PMESearchTxt.Text = ""
                PMEETxt.Text = ""
                PMETTxt.Text = ""

            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
            End Try
        End Using
        LoadPrintingData()
    End Sub
    Private Sub ExecuteBtn_Click(sender As Object, e As EventArgs) Handles ExecuteBtn.Click
        Dim query As String = ExecuteText.Text.Trim()

        If String.IsNullOrEmpty(query) Then
            MessageBox.Show("Please enter a SQL query.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                Using cmd As New SqlCommand(query, conn)
                    conn.Open()

                    If query.Trim().ToUpper().StartsWith("SELECT") Then
                        Using reader As SqlDataReader = cmd.ExecuteReader()
                            Dim results As String = ""

                            While reader.Read()
                                For i As Integer = 0 To reader.FieldCount - 1
                                    results &= reader.GetName(i) & ": " & reader(i).ToString() & vbTab
                                Next
                                results &= vbCrLf
                            End While

                            If String.IsNullOrEmpty(results) Then
                                MessageBox.Show("No rows returned.", "Results", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Else
                                MessageBox.Show(results, "Results", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            End If
                        End Using
                    Else
                        Dim affectedRows As Integer = cmd.ExecuteNonQuery()
                        MessageBox.Show(affectedRows & " row(s) affected.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error executing query: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PeriodChange_Click(sender As Object, e As EventArgs) Handles PeriodChange.Click
        If String.IsNullOrWhiteSpace(Periodstart.Text) OrElse String.IsNullOrWhiteSpace(PeriodEnd.Text) Then
            MsgBox("Please enter the From and To dates first.", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        Try
            Tools.LoadConfiguration()
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()

                Dim sql As String = "
            IF EXISTS (SELECT 1 FROM Control_Table WHERE Ctl_Desc = 'fromDate')
                UPDATE Control_Table SET Ctl_Value = @fd WHERE Ctl_Desc = 'fromDate'
            ELSE
                INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('fromDate', @fd);

            IF EXISTS (SELECT 1 FROM Control_Table WHERE Ctl_Desc = 'toDate')
                UPDATE Control_Table SET Ctl_Value = @td WHERE Ctl_Desc = 'toDate'
            ELSE
                INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('toDate', @td);"

                Using cmd As New SqlCommand(sql, sqlconnect)
                    cmd.Parameters.AddWithValue("@fd", Periodstart.Text)
                    cmd.Parameters.AddWithValue("@td", PeriodEnd.Text)

                    cmd.ExecuteNonQuery()
                End Using

                MsgBox("Financial period updated successfully.", MsgBoxStyle.Information)
            End Using
        Catch ex As Exception
            MsgBox("Error updating period: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
    Private Sub LoadFinancialPeriod()
        Try
            Tools.LoadConfiguration()

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Dim sql As String = "SELECT Ctl_Desc, Ctl_Value FROM Control_Table WHERE Ctl_Desc IN ('fromDate', 'toDate')"

                sqlconnect.Open()
                Using cmd As New SqlCommand(sql, sqlconnect)
                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        While rdr.Read()
                            Dim desc As String = rdr("Ctl_Desc").ToString()
                            Dim val As String = rdr("Ctl_Value").ToString()
                            Dim dt As DateTime

                            If DateTime.TryParseExact(val, "dd-MM-yyyy", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, dt) Then
                                If desc = "fromDate" Then
                                    Periodstart.Text = dt.ToString("dd-MM-yyyy")
                                ElseIf desc = "toDate" Then
                                    PeriodEnd.Text = dt.ToString("dd-MM-yyyy")
                                End If

                            Else
                                If desc = "fromDate" Then Periodstart.Text = val
                                If desc = "toDate" Then PeriodEnd.Text = val
                            End If
                        End While
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading financial period: " & ex.Message, "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub SplitFinancialYear_ByBackup(fromDate As Date, toDate As Date, oldDbName As String)

        Dim currentDb As String
        Dim backupFolder As String = "C:\Project\Backup\"
        Dim backupPath As String

        Using con As SqlConnection = Tools.GetConnection()
            con.Open()
            currentDb = con.Database

            Try
                If File.Exists(filePath) Then
                    Dim lines As String() = File.ReadAllLines(filePath)
                    For Each line As String In lines
                        If line.StartsWith("BackupPath=", StringComparison.OrdinalIgnoreCase) Then
                            backupFolder = line.Substring("BackupPath=".Length).Trim()
                            Exit For
                        End If
                    Next
                Else
                    MessageBox.Show("Configuration file not found at: " & filePath, "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            Catch ex As Exception
                MessageBox.Show("Error reading configuration: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try

            backupPath = IO.Path.Combine(backupFolder, $"{currentDb}_{DateTime.Now:yyyyMMddHHmmss}.bak")

            Try
                Using cmd As New SqlCommand($"BACKUP DATABASE [{currentDb}] TO DISK = '{backupPath}' WITH INIT", con)
                    cmd.ExecuteNonQuery()
                End Using

                Using cmd As New SqlCommand($"
                IF DB_ID('{oldDbName}') IS NOT NULL
                BEGIN
                    ALTER DATABASE [{oldDbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    DROP DATABASE [{oldDbName}];
                END", con)
                    cmd.ExecuteNonQuery()
                End Using

                Dim fileMoveList As New List(Of String)
                Using cmd As New SqlCommand("
                SELECT name, physical_name
                FROM sys.master_files
                WHERE database_id = DB_ID(@dbName)", con)

                    cmd.Parameters.AddWithValue("@dbName", currentDb)
                    Using reader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim logicalName = reader("name").ToString()
                            Dim physicalPath = reader("physical_name").ToString()
                            Dim dir = IO.Path.GetDirectoryName(physicalPath)
                            Dim ext = IO.Path.GetExtension(physicalPath)

                            Dim newName As String =
                            If(ext.ToLower() = ".ldf",
                               oldDbName & "_log" & ext,
                               oldDbName & "_" & logicalName & ext)

                            fileMoveList.Add($"MOVE '{logicalName}' TO '{IO.Path.Combine(dir, newName)}'")
                        End While
                    End Using
                End Using

                Using cmd As New SqlCommand(
                $"RESTORE DATABASE [{oldDbName}]
                  FROM DISK = '{backupPath}'
                  WITH REPLACE, {String.Join(", ", fileMoveList)}", con)
                    cmd.ExecuteNonQuery()
                End Using

                DeleteYearData(con, currentDb, fromDate, toDate, deleteOldYear:=True)
                DeleteYearData(con, oldDbName, fromDate, toDate, deleteOldYear:=False)

                MsgBox("Financial year split completed successfully.", MsgBoxStyle.Information)

            Catch ex As Exception
                MsgBox("Error: " & ex.Message, MsgBoxStyle.Critical)
            End Try
        End Using
    End Sub
    Private Sub DeleteYearData(con As SqlConnection, dbName As String, fromDate As Date, toDate As Date, deleteOldYear As Boolean)

        Dim condition As String = If(deleteOldYear, "BETWEEN @FromDate AND @ToDate", "NOT BETWEEN @FromDate AND @ToDate")

        Using cmd As New SqlCommand()
            cmd.Connection = con
            cmd.CommandType = CommandType.Text

            cmd.Parameters.Add("@FromDate", SqlDbType.Date).Value = fromDate
            cmd.Parameters.Add("@ToDate", SqlDbType.Date).Value = toDate

            cmd.CommandText = $"DELETE FROM [{dbName}].dbo.Sales_table WHERE sale_date {condition};
                                DELETE FROM [{dbName}].dbo.Purchase_table WHERE Purchase_date {condition};
                                DELETE FROM [{dbName}].dbo.JobCard_table WHERE JobCard_date {condition};
                                DELETE FROM [{dbName}].dbo.Printing_table WHERE Printing_date {condition};
                                
                                DELETE FROM [{dbName}].dbo.AutoBackup_table WHERE BackupDate {condition};"
            'DELETE FROM [{dbName}].dbo.Stock_table WHERE Stock_date {condition};

            cmd.ExecuteNonQuery()
        End Using
    End Sub
    Private Sub UpdateControlTableDates(con As SqlConnection, fromDate As Date, toDate As Date)
        Using cmd As New SqlCommand()
            cmd.Connection = con
            cmd.CommandType = CommandType.Text

            cmd.CommandText = "
        IF EXISTS (SELECT 1 FROM Control_Table WHERE Ctl_Desc = 'fromDate')
            UPDATE Control_Table SET Ctl_Value = @FromDate WHERE Ctl_Desc = 'fromDate'
        ELSE
            INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('fromDate', @FromDate);"
            cmd.Parameters.AddWithValue("@FromDate", fromDate.ToString("dd-MM-yyyy"))
            cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()

            cmd.CommandText = "
        IF EXISTS (SELECT 1 FROM Control_Table WHERE Ctl_Desc = 'toDate')
            UPDATE Control_Table SET Ctl_Value = @ToDate WHERE Ctl_Desc = 'toDate'
        ELSE
            INSERT INTO Control_Table (Ctl_Desc, Ctl_Value) VALUES ('toDate', @ToDate);"
            cmd.Parameters.AddWithValue("@ToDate", toDate.ToString("dd-MM-yyyy"))
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Function MapToOldFinancialYear(selectedDate As Date) As Date
        Dim fyStartYear As Integer

        If selectedDate.Month >= 4 Then
            fyStartYear = selectedDate.Year
        Else
            fyStartYear = selectedDate.Year - 1
        End If

        Return New Date(fyStartYear, selectedDate.Month, selectedDate.Day)
    End Function
    Private Sub Remove_Click(sender As Object, e As EventArgs) Handles Remove.Click

        Dim oldFromDate = MapToOldFinancialYear(DateTimePickerFrom.Value.Date)
        Dim oldToDate = MapToOldFinancialYear(DateTimePickerTo.Value.Date)

        Dim newFromDate As Date
        Dim newToDate As Date

        newFromDate = oldToDate.AddDays(1)
        newToDate = New Date(newFromDate.Year + 1, 3, 31)

        If oldFromDate > oldToDate Then
            MsgBox("From date cannot be greater than To date", MsgBoxStyle.Exclamation)
            Exit Sub
        End If

        If OldDBName.Text.Trim() = "" Then
            MsgBox("Enter old database name", MsgBoxStyle.Exclamation)
            Exit Sub
        End If


        If MsgBox("This will BACKUP, RESTORE and SPLIT data. Continue?", MsgBoxStyle.YesNo + MessageBoxIcon.Warning) = MsgBoxResult.Yes Then
            SplitFinancialYear_ByBackup(oldFromDate, oldToDate, OldDBName.Text.Trim())
        End If

        Using con As SqlConnection = Tools.GetConnection()
            con.Open()

            UpdateControlTableDates(con, newFromDate, newToDate)

            Using cmdNew As New SqlCommand()
                cmdNew.Connection = con
                cmdNew.CommandType = CommandType.Text
                cmdNew.CommandText = $"USE [{OldDBName.Text.Trim()}];"
                cmdNew.ExecuteNonQuery()
            End Using
            UpdateControlTableDates(con, oldFromDate, oldToDate)
        End Using
    End Sub
End Class
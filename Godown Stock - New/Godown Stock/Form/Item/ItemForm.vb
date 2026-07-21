Imports System.Data.SqlClient
Imports System.IO
Imports System.Reflection
Imports Guna.UI2.WinForms

Public Class ItemForm
    Private dtUnits, dtGroup, dtBrand, dtModel, dtname As DataTable
    Private activeTextbox As Guna.UI2.WinForms.Guna2TextBox = Nothing
    Private listBoxSelectionMade As Boolean = False
    Private isPopulatingText As Boolean = False

    Private Sub ItemForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Itemtxt.Focus()
    End Sub
    Private Sub ItemForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me
        Try
            Themeload()
            Tools.LoadConfiguration()
            InitializeDataGridView()
            RefreshItemList()
            LoadAutoCompleteData()
            OpeningText.Text = 0
            MinStockTxt.Text = 0
            AddHandler KryptonListBox.Click, AddressOf KryptonListBox_Click
            AddHandler KryptonListBox.KeyDown, AddressOf KryptonListBox_KeyDown
        Catch ex As TargetInvocationException
            MessageBox.Show(If(ex.InnerException IsNot Nothing, ex.InnerException.Message, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
    'Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) _
    'Handles Itemtxt.KeyDown, Unitbox.KeyDown, grpbox.KeyDown, Brandbox.KeyDown, Modelbox.KeyDown,
    'OpeningText.KeyDown, MinStockTxt.KeyDown, Searchbtn.KeyDown, Savebtn.KeyDown

    '    If e.KeyCode = Keys.Enter Then
    '        e.SuppressKeyPress = True
    '        Me.SelectNextControl(CType(sender, Control), forward:=True, tabStopOnly:=True, nested:=True, wrap:=True)
    '    End If
    'End Sub
    Private Sub RefreshItemList()
        OpeningText.Text = 0
        MinStockTxt.Text = 0

        Dim query As String = "SELECT  I.Itemname,U.ItemUnit_Name AS Unit,G.ItemGroup_Name AS Itemgroup,B.ItemBrand_Name AS Itembrand,M.ItemModel_Name AS Itemmodel,I.Quantity,I.MinStock
                                FROM Item_Table I
                                LEFT JOIN ItemGroup_Table G ON G.ID = I.Itemgroup_id
                                LEFT JOIN ItemBrand_Table B ON B.ID = I.ItemBrand_ID
                                LEFT JOIN ItemModel_Table M ON M.ID = I.ItemModel_ID
                                LEFT JOIN ItemUnit_Table U ON U.ID = I.ItemUnit_ID
                                WHERE I.Active = 0 ORDER BY I.Itemname ASC;"
        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Dim command As New SqlCommand(query, sqlconnect)
            Try
                sqlconnect.Open()
                Dim reader As SqlDataReader = command.ExecuteReader()
                Dim dt As New DataTable()
                dt.Load(reader)
                dt.Columns.Add("SNo", GetType(Integer))
                For i As Integer = 0 To dt.Rows.Count - 1
                    dt.Rows(i)("SNo") = i + 1
                Next
                Guna2DataGridView1.DataSource = dt
                Guna2DataGridView1.Columns("SNo").DisplayIndex = 0
                Guna2DataGridView1.Columns("SNo").HeaderText = "S.No"
                Guna2DataGridView1.Columns("SNo").Width = 25
                Guna2DataGridView1.Columns("ItemName").Width = 200
                Guna2DataGridView1.Columns("Unit").Width = 80
                Guna2DataGridView1.Columns("ItemGroup").Width = 100
                Guna2DataGridView1.Columns("ItemBrand").Width = 100
                Guna2DataGridView1.Columns("ItemModel").Width = 100
                Guna2DataGridView1.Columns("Quantity").Width = 80
                Guna2DataGridView1.Columns("MinStock").Width = 80

                ' Guna2DataGridView1.Columns("ID").HeaderText = "Code"
                Guna2DataGridView1.Columns("ItemName").HeaderText = "Item Name"
                Guna2DataGridView1.Columns("Unit").HeaderText = "Unit"
                Guna2DataGridView1.Columns("ItemGroup").HeaderText = "Group"
                Guna2DataGridView1.Columns("ItemBrand").HeaderText = "Brand"
                Guna2DataGridView1.Columns("ItemModel").HeaderText = "Model"
                Guna2DataGridView1.Columns("Quantity").HeaderText = "Opening Qty"
                Guna2DataGridView1.Columns("MinStock").HeaderText = "Min Stock"
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub InitializeDataGridView()
        With Guna2DataGridView1
            '.Dock = DockStyle.Fill
            .ColumnHeadersVisible = True
            .AllowUserToAddRows = False
            .AllowUserToOrderColumns = True
            '.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 40, 49)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersHeight = 35
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect

            .ReadOnly = True
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
            .Margin = New Padding(20, 20, 20, 20)
            .MultiSelect = False
            .CellBorderStyle = DataGridViewCellBorderStyle.Single

            .DefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Regular)

            .ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Regular)

            Dim headerColor As Color = Color.FromArgb(34, 40, 49)
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Try
                    sqlconnect.Open()
                    Dim Query As New SqlCommand("SELECT Ctl_Value FROM Control_Table WHERE Ctl_Desc = 'HeaderColor'", sqlconnect)
                    Using reader As SqlDataReader = Query.ExecuteReader()
                        If reader.Read() Then
                            Dim colorString As String = reader("Ctl_Value").ToString()
                            If Not String.IsNullOrEmpty(colorString) Then
                                Try
                                    headerColor = ColorTranslator.FromHtml(colorString)
                                Catch
                                    ' keep default if conversion fails
                                End Try
                            End If
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error loading HeaderColor: " & ex.Message)
                End Try
            End Using

            .ColumnHeadersDefaultCellStyle.BackColor = headerColor
        End With
    End Sub

    Private Sub ClearItemFields()
        Itemtxt.Clear()
        Unitbox.Text = ""
        grpbox.Text = ""
        Brandbox.Text = ""
        Modelbox.Text = ""
        OpeningText.Clear()
        MinStockTxt.Clear()
        Snotxt.Clear()
    End Sub
    Private Function GetItemIdByName(itemName As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM Item_table WHERE Itemname = @Itemname"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@Itemname", itemName)
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
            MessageBox.Show("Error getting item ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        End Try
    End Function

    Private Function GetUnitIdByName(unit As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM ItemUnit_Table WHERE ItemUnit_Name = @unit"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@unit", unit)
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
            MessageBox.Show("Error getting unit ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        End Try
    End Function

    Private Function GetGroupIdByName(itemGroup As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM ItemGroup_Table WHERE ItemGroup_Name = @group"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@group", itemGroup)
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
            MessageBox.Show("Error getting group ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        End Try
    End Function
    Private Function GetBrandIdByName(itemBrand As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM ItemBrand_Table WHERE ItemBrand_Name = @brand"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@brand", itemBrand)
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
            MessageBox.Show("Error getting brand ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        End Try
    End Function
    Private Function GetModelIdByName(itemModel As String) As Integer
        Try
            Dim query As String = "SELECT ID FROM ItemModel_Table WHERE ItemModel_Name = @model"
            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using sqlcommand As New SqlCommand(query, sqlconnect)
                    sqlcommand.Parameters.AddWithValue("@model", itemModel)
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
            MessageBox.Show("Error getting model ID: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return 0
        End Try
    End Function

    Private Sub Savebtn_Click(sender As Object, e As EventArgs) Handles Savebtn.Click
        Dim itemName As String = Itemtxt.Text.Trim()
        Dim unit As String = Unitbox.Text.Trim()
        Dim itemGroup As String = grpbox.Text.Trim()
        Dim itemBrand As String = Brandbox.Text.Trim()
        Dim itemModel As String = Modelbox.Text.Trim()
        Dim searchItemName As String = Snotxt.Text.Trim()
        Dim quantity As Integer
        Dim MinStock As Integer

        If String.IsNullOrWhiteSpace(itemName) OrElse
       String.IsNullOrWhiteSpace(unit) OrElse
       String.IsNullOrWhiteSpace(itemGroup) OrElse
       String.IsNullOrWhiteSpace(itemBrand) OrElse
       String.IsNullOrWhiteSpace(itemModel) Then
            MessageBox.Show("All fields are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not Integer.TryParse(OpeningText.Text.Trim(), quantity) Then
            MessageBox.Show("Please enter a valid integer for Quantity.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        If Not Integer.TryParse(MinStockTxt.Text.Trim(), MinStock) Then
            MessageBox.Show("Please enter a valid integer for MinStock.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Dim unitId As Integer = GetUnitIdByName(unit)
        Dim groupId As Integer = GetGroupIdByName(itemGroup)
        Dim brandId As Integer = GetBrandIdByName(itemBrand)
        Dim modelId As Integer = GetModelIdByName(itemModel)
        Dim UserId As Integer = Tools.GetStoredUsername()

        If unitId = 0 OrElse groupId = 0 OrElse brandId = 0 OrElse modelId = 0 Then
            MessageBox.Show("One or more reference values are invalid or missing from lookup tables.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using conn As SqlConnection = Tools.GetConnection()
            Try
                conn.Open()
                Dim itemId As Integer = GetItemIdByName(searchItemName)

                If itemId > 0 Then
                    Dim updateCommand As New SqlCommand("UPDATE Item_Table SET ItemName = @ItemName, Unit = @Unit, ItemGroup = @ItemGroup, ItemBrand = @ItemBrand, ItemModel = @ItemModel,
                        Quantity = @Quantity, MinStock = @MinStock, ItemUnit_ID = @UnitID, ItemGroup_ID = @GroupID, ItemBrand_ID = @BrandID, ItemModel_ID = @ModelID, UserID = @UserID WHERE ID = @ID", conn)

                    updateCommand.Parameters.AddWithValue("@ItemName", itemName)
                    updateCommand.Parameters.AddWithValue("@Unit", unit)
                    updateCommand.Parameters.AddWithValue("@ItemGroup", itemGroup)
                    updateCommand.Parameters.AddWithValue("@ItemBrand", itemBrand)
                    updateCommand.Parameters.AddWithValue("@ItemModel", itemModel)
                    updateCommand.Parameters.AddWithValue("@Quantity", quantity)
                    updateCommand.Parameters.AddWithValue("@MinStock", MinStock)
                    updateCommand.Parameters.AddWithValue("@UnitID", unitId)
                    updateCommand.Parameters.AddWithValue("@GroupID", groupId)
                    updateCommand.Parameters.AddWithValue("@BrandID", brandId)
                    updateCommand.Parameters.AddWithValue("@ModelID", modelId)
                    updateCommand.Parameters.AddWithValue("@ID", itemId)
                    updateCommand.Parameters.AddWithValue("@UserID", UserId)

                    updateCommand.ExecuteNonQuery()
                    MessageBox.Show("Item updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    Dim insertCommand As New SqlCommand("INSERT INTO Item_Table (ItemName, Unit, ItemGroup, ItemBrand, ItemModel, Quantity, MinStock, ItemUnit_ID, ItemGroup_ID, ItemBrand_ID, ItemModel_ID, UserId)
                                                         VALUES (@ItemName, @Unit, @ItemGroup, @ItemBrand, @ItemModel, @Quantity, @MinStock, @UnitID, @GroupID, @BrandID, @ModelID, @UserId)", conn)

                    insertCommand.Parameters.AddWithValue("@ItemName", itemName)
                    insertCommand.Parameters.AddWithValue("@Unit", unit)
                    insertCommand.Parameters.AddWithValue("@ItemGroup", itemGroup)
                    insertCommand.Parameters.AddWithValue("@ItemBrand", itemBrand)
                    insertCommand.Parameters.AddWithValue("@ItemModel", itemModel)
                    insertCommand.Parameters.AddWithValue("@Quantity", quantity)
                    insertCommand.Parameters.AddWithValue("@MinStock", MinStock)
                    insertCommand.Parameters.AddWithValue("@UnitID", unitId)
                    insertCommand.Parameters.AddWithValue("@GroupID", groupId)
                    insertCommand.Parameters.AddWithValue("@BrandID", brandId)
                    insertCommand.Parameters.AddWithValue("@ModelID", modelId)
                    insertCommand.Parameters.AddWithValue("@UserId", UserId)

                    insertCommand.ExecuteNonQuery()
                    MessageBox.Show("Item saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

                ClearItemFields()
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Finally
                RefreshItemList()
                LoadAutoCompleteData()
            End Try
        End Using
    End Sub
    Private Sub Searchbtn_Click(sender As Object, e As EventArgs) Handles Searchbtn.Click
        Dim itemName As String = Snotxt.Text.Trim()

        If String.IsNullOrWhiteSpace(itemName) Then
            MessageBox.Show("Please enter an item name to search.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        Using conn As SqlConnection = Tools.GetConnection()
            Dim cmd As New SqlCommand("SELECT * FROM Item_Table WHERE ItemName = @ItemName and active = 0", conn)
            cmd.Parameters.AddWithValue("@ItemName", itemName)

            Try
                conn.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.HasRows Then
                        While reader.Read()
                            Itemtxt.Text = reader("ItemName").ToString()
                            Unitbox.Text = reader("Unit").ToString()
                            grpbox.Text = reader("ItemGroup").ToString()
                            Brandbox.Text = reader("ItemBrand").ToString()
                            Modelbox.Text = reader("ItemModel").ToString()
                            OpeningText.Text = reader("Quantity").ToString()
                            MinStockTxt.Text = reader("MinStock").ToString()

                        End While
                    Else
                        MessageBox.Show("Item not found.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub Killbtn_Click(sender As Object, e As EventArgs) Handles Killbtn.Click

        Dim itemName As String = Snotxt.Text.Trim()

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Dim cmd As New SqlCommand("UPDATE Item_Table SET Active = 1 WHERE Itemname = @Itemname", sqlconnect)
            cmd.Parameters.AddWithValue("@Itemname", itemName)
            Try
                sqlconnect.Open()
                Dim rows = cmd.ExecuteNonQuery()
                MessageBox.Show(If(rows > 0, "Item inactive successfully.", "No record found."), "Status", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using

        ClearItemFields()
        RefreshItemList()
        LoadAutoCompleteData()
    End Sub

    Private Sub Refreshbtn_Click(sender As Object, e As EventArgs) Handles Refreshbtn.Click
        RefreshItemList()
        LoadAutoCompleteData()
    End Sub
    Private Sub LoadAutoCompleteData()
        dtname = LoadDataTable("SELECT Itemname FROM Item_table Where active = 0")
        dtUnits = LoadDataTable("SELECT ItemUnit_Name FROM ItemUnit_Table Where active = 0")
        dtGroup = LoadDataTable("SELECT ItemGroup_Name FROM ItemGroup_Table Where active = 0")
        dtBrand = LoadDataTable("SELECT ItemBrand_Name FROM ItemBrand_Table Where active = 0")
        dtModel = LoadDataTable("SELECT ItemModel_Name FROM ItemModel_Table Where active = 0")
    End Sub

    Private Function LoadDataTable(query As String) As DataTable
        Using conn As SqlConnection = Tools.GetConnection()
            Using da As New SqlDataAdapter(query, conn)
                Dim dt As New DataTable()
                da.Fill(dt)
                Return dt
            End Using
        End Using
    End Function
    Private Sub ValidateTyping(sender As Object, e As KeyPressEventArgs) Handles _
        Unitbox.KeyPress, grpbox.KeyPress, Brandbox.KeyPress, Modelbox.KeyPress, Snotxt.KeyPress

        If Char.IsControl(e.KeyChar) Then Exit Sub
        Dim tb As Guna2TextBox = CType(sender, Guna2TextBox)
        Dim predictedText As String = tb.Text.Remove(tb.SelectionStart, tb.SelectionLength).Insert(tb.SelectionStart, e.KeyChar.ToString())

        Dim source As DataTable = GetCorrespondingDataTable(tb)
        If source IsNot Nothing Then
            Dim exists As Boolean = source.AsEnumerable().Any(Function(row) row.Field(Of String)(0).StartsWith(predictedText, StringComparison.OrdinalIgnoreCase))
            If Not exists Then e.Handled = True
        End If
    End Sub

    Private Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles _
        Snotxt.TextChanged, Unitbox.TextChanged, grpbox.TextChanged, Brandbox.TextChanged, Modelbox.TextChanged

        Dim tb As Guna2TextBox = CType(sender, Guna2TextBox)
        activeTextbox = tb

        Dim source As DataTable = GetCorrespondingDataTable(tb)
        If source Is Nothing OrElse String.IsNullOrEmpty(tb.Text) Then
            KryptonListBox.Visible = False
            Exit Sub
        End If

        Dim dv As New DataView(source)
        dv.RowFilter = $"[{source.Columns(0).ColumnName}] LIKE '{tb.Text.Replace("'", "''")}%'"

        If dv.Count > 0 AndAlso tb.Focused Then
            KryptonListBox.DataSource = dv
            KryptonListBox.DisplayMember = source.Columns(0).ColumnName

            ' Position ListBox
            Dim p As Point = tb.Parent.PointToScreen(tb.Location)
            Dim localPoint As Point = Me.PointToClient(p)
            KryptonListBox.Location = New Point(localPoint.X, localPoint.Y + tb.Height)
            KryptonListBox.Width = tb.Width
            KryptonListBox.Visible = True
            KryptonListBox.BringToFront()
        Else
            KryptonListBox.Visible = False
        End If
    End Sub

    Private Sub SelectItemFromList()
        If KryptonListBox.Visible AndAlso KryptonListBox.SelectedItem IsNot Nothing AndAlso activeTextbox IsNot Nothing Then
            Dim rowView As DataRowView = DirectCast(KryptonListBox.SelectedItem, DataRowView)
            activeTextbox.Text = rowView(0).ToString()
            KryptonListBox.Visible = False
            activeTextbox.Focus()
            activeTextbox.SelectionStart = activeTextbox.Text.Length
        End If
    End Sub
    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) Handles _
        Itemtxt.KeyDown, Unitbox.KeyDown, grpbox.KeyDown, Brandbox.KeyDown, Modelbox.KeyDown,
        OpeningText.KeyDown, MinStockTxt.KeyDown, Snotxt.KeyDown, Searchbtn.KeyDown, Savebtn.KeyDown

        Dim tb As Control = CType(sender, Control)

        If e.KeyCode = Keys.F2 Then
            If TypeOf tb Is Guna2TextBox AndAlso (tb Is Snotxt Or tb Is Unitbox Or tb Is grpbox Or tb Is Brandbox Or tb Is Modelbox) Then
                e.Handled = True
                ShowAllSuggestions(CType(tb, Guna2TextBox))
                Exit Sub
            End If
        End If

        If KryptonListBox.Visible AndAlso (tb Is Snotxt Or tb Is Unitbox Or tb Is grpbox Or tb Is Brandbox Or tb Is Modelbox) Then
            If e.KeyCode = Keys.Down Then
                KryptonListBox.Focus()
                If KryptonListBox.Items.Count > 0 Then KryptonListBox.SelectedIndex = 0
                e.Handled = True
                Exit Sub
            ElseIf e.KeyCode = Keys.Enter Then
                SelectItemFromList()
                e.Handled = True : e.SuppressKeyPress = True
                Exit Sub
            End If
        End If

        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.SelectNextControl(tb, True, True, True, True)
        End If
    End Sub
    Private Sub ShowAllSuggestions(tb As Guna2TextBox)
        activeTextbox = tb
        Dim source As DataTable = GetCorrespondingDataTable(tb)

        If source Is Nothing OrElse source.Rows.Count = 0 Then
            KryptonListBox.Visible = False
            Exit Sub
        End If

        Dim dv As New DataView(source)
        KryptonListBox.DataSource = dv
        KryptonListBox.DisplayMember = source.Columns(0).ColumnName

        Dim p As Point = tb.Parent.PointToScreen(tb.Location)
        Dim localPoint As Point = Me.PointToClient(p)
        KryptonListBox.Location = New Point(localPoint.X, localPoint.Y + tb.Height)
        KryptonListBox.Width = tb.Width
        KryptonListBox.Visible = True
        KryptonListBox.BringToFront()

        If KryptonListBox.Items.Count > 0 Then
            KryptonListBox.Focus()
            KryptonListBox.SelectedIndex = 0
        End If
    End Sub
    Private Sub KryptonListBox_KeyDown(sender As Object, e As KeyEventArgs) Handles KryptonListBox.KeyDown
        If e.KeyCode = Keys.Enter Then
            SelectItemFromList()
            e.Handled = True
        ElseIf e.KeyCode = Keys.Escape Then
            KryptonListBox.Visible = False
            activeTextbox?.Focus()
        End If
    End Sub

    Private Sub KryptonListBox_Click(sender As Object, e As EventArgs) Handles KryptonListBox.Click
        SelectItemFromList()
    End Sub

    Private Sub TextBox_LostFocus(sender As Object, e As EventArgs) Handles _
        Snotxt.LostFocus, Unitbox.LostFocus, grpbox.LostFocus, Brandbox.LostFocus, Modelbox.LostFocus
        TimerFocusDelay.Start()
    End Sub

    Private Sub TimerFocusDelay_Tick(sender As Object, e As EventArgs) Handles TimerFocusDelay.Tick
        TimerFocusDelay.Stop()
        If Not KryptonListBox.Focused Then
            KryptonListBox.Visible = False
            ' Final Validation: Clear field if text isn't a 100% match (except for Item Name)
            If activeTextbox IsNot Nothing AndAlso activeTextbox IsNot Itemtxt Then
                Dim source As DataTable = GetCorrespondingDataTable(activeTextbox)
                If Not IsValidInput(activeTextbox, source) Then activeTextbox.Clear()
            End If
        End If
    End Sub

    Private Function IsValidInput(tb As Guna2TextBox, source As DataTable) As Boolean
        If String.IsNullOrWhiteSpace(tb.Text) Then Return True
        If source Is Nothing Then Return False
        Return source.AsEnumerable().Any(Function(row) row.Field(Of String)(0).Equals(tb.Text, StringComparison.OrdinalIgnoreCase))
    End Function

    Private Function GetCorrespondingDataTable(tb As Guna2TextBox) As DataTable
        If tb Is Unitbox Then Return dtUnits
        If tb Is grpbox Then Return dtGroup
        If tb Is Brandbox Then Return dtBrand
        If tb Is Modelbox Then Return dtModel
        If tb Is Snotxt Then Return dtname
        Return Nothing
    End Function
    Private Sub TextBox_FocusTrigger(sender As Object, e As EventArgs) Handles _
        Snotxt.Click, Unitbox.Click, grpbox.Click, Brandbox.Click, Modelbox.Click,
        Snotxt.GotFocus, Unitbox.GotFocus, grpbox.GotFocus, Brandbox.GotFocus, Modelbox.GotFocus

        TextBox_TextChanged(CType(sender, Guna2TextBox), EventArgs.Empty)
    End Sub
End Class

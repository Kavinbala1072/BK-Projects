Imports System.Data.SqlClient
Imports Guna.UI2.WinForms
Public Class Display

    Private Sub SalesDisplayForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FromDateTextBox.text = DateTime.Now.ToString("dd/MM/yyyy")
        ToDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy")
        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me

        DisplayEntryTypes()
        InitializeDataGridView()
        CancelBtn.Visible = False
        If DisplayComboBox.Items.Count > 0 Then
            DisplayComboBox.SelectedIndex = 0
        End If
        Themeload()
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

    Private Sub ItemForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        FromDateTextBox.Focus()
    End Sub
    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles RefreshButton.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.SelectNextControl(CType(sender, Control), forward:=True, tabStopOnly:=True, nested:=True, wrap:=True)
        End If
    End Sub

    Private Sub LoadSalesData()
        Dim fromDate As Date
        Dim toDate As Date

        If Not Date.TryParse(FromDateTextBox.Text, fromDate) OrElse Not Date.TryParse(ToDateTextBox.Text, toDate) Then
            MessageBox.Show("Please enter valid From and To dates.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(DisplayComboBox.Text) Then
            MessageBox.Show("Please select a Godown Type.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim entryType As String = DisplayComboBox.Text
        Dim query As String = String.Empty
        Dim dateColumn As String
        Dim tableName As String

        If entryType = "Purchase" Then
            dateColumn = "Purchase_date"
            tableName = "purchase_table"
        ElseIf entryType = "Sales" Then
            dateColumn = "Sale_date"
            tableName = "Sales_Table"
        Else
            MessageBox.Show("Invalid entry type selected.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        query = $"WITH OrderedData AS (SELECT t.Manual_BillNo, t.Bill_No, t.{dateColumn}, t.Partyname, t.quantity, t.Total_Amount, t.Remarks, t.Cancel, t.EntryType,
                    i.Itemname AS Itemname,DENSE_RANK() OVER (ORDER BY CASE WHEN t.Bill_No IS NULL THEN 1 ELSE 0 END, t.Bill_No) AS BillGroup,
                    ROW_NUMBER() OVER (PARTITION BY t.Bill_No ORDER BY i.Itemname) AS rn
                    FROM {tableName} t
                    LEFT JOIN Item_Table i ON i.ID = t.item_id
                    WHERE t.EntryType = @EntryType  AND t.{dateColumn} BETWEEN @FromDate AND @ToDate)
                    SELECT 
                        CASE WHEN rn = 1 THEN Manual_BillNo ELSE NULL END AS Manual_BillNo,
                        CASE WHEN rn = 1 THEN Bill_No ELSE NULL END AS Bill_No,
                        CASE WHEN rn = 1 THEN FORMAT({dateColumn}, 'dd/MM/yyyy') ELSE NULL END AS Bill_Date,
                        CASE WHEN rn = 1 THEN Partyname ELSE NULL END AS Partyname,
                        Itemname,quantity,
                        Total_Amount AS Amount,
                        CASE WHEN rn = 1 THEN Remarks ELSE NULL END AS Remarks,
                        CASE WHEN rn = 1 THEN Cancel ELSE 0 END AS Cancel
                    FROM OrderedData
                    ORDER BY BillGroup, rn, Bill_No;"



        'query = $"WITH OrderedData AS (SELECT *, DENSE_RANK() OVER (ORDER BY 
        '        CASE WHEN Bill_No IS NULL THEN 1 ELSE 0 END, Bill_No) AS BillGroup,ROW_NUMBER() OVER (PARTITION BY Bill_No ORDER BY Itemname) AS rn
        '        FROM {tableName}
        '        WHERE EntryType = @EntryType  AND {dateColumn} BETWEEN @FromDate AND @ToDate)
        '        SELECT 
        '            CASE WHEN rn = 1 THEN Manual_BillNo ELSE NULL END AS Manual_BillNo,
        '            CASE WHEN rn = 1 THEN Bill_No ELSE NULL END AS Bill_No,
        '            CASE WHEN rn = 1 THEN FORMAT({dateColumn}, 'dd/MM/yyyy') ELSE NULL END AS Bill_Date,
        '            CASE WHEN rn = 1 THEN Partyname ELSE NULL END AS Partyname,
        '            Itemname,quantity,Total_Amount AS Amount,
        '            CASE WHEN rn = 1 THEN Remarks ELSE NULL END AS Remarks,
        '            CASE WHEN rn = 1 THEN Cancel ELSE 0 END AS Cancel
        '        FROM OrderedData
        '        ORDER BY BillGroup,rn;"


        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Using command As New SqlCommand(query, sqlconnect)
                command.Parameters.AddWithValue("@EntryType", If(entryType = "Purchase", 1, 2))
                command.Parameters.AddWithValue("@FromDate", fromDate)
                command.Parameters.AddWithValue("@ToDate", toDate)

                Try
                    sqlconnect.Open()
                    Using reader As SqlDataReader = command.ExecuteReader()
                        Dim dt As New DataTable()
                        dt.Load(reader)

                        ' Add serial number column
                        dt.Columns.Add("SNo", GetType(Integer))

                        Dim currentSNo As Integer = 1
                        Dim previousBillNo As String = Nothing

                        For i As Integer = 0 To dt.Rows.Count - 1
                            Dim currentBillNo As String = If(IsDBNull(dt.Rows(i)("Bill_No")), "", dt.Rows(i)("Bill_No").ToString())

                            If Not String.IsNullOrEmpty(currentBillNo) AndAlso currentBillNo <> previousBillNo Then
                                dt.Rows(i)("SNo") = currentSNo
                                previousBillNo = currentBillNo
                                currentSNo += 1
                            Else
                                dt.Rows(i)("SNo") = DBNull.Value
                            End If
                        Next


                        Guna2DataGridView1.DataSource = dt
                        If Guna2DataGridView1.Columns.Contains("Cancel") Then
                            Guna2DataGridView1.Columns("Cancel").Visible = False
                        End If

                        ' Color Bill_No red if Cancel = 1
                        For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                            If Not row.IsNewRow Then
                                Dim isCancelled As Boolean = False
                                If Not IsDBNull(row.Cells("Cancel").Value) Then
                                    isCancelled = Convert.ToBoolean(row.Cells("Cancel").Value)
                                End If

                                If isCancelled Then
                                    'row.DefaultCellStyle.BackColor = Color.FromArgb(255, 127, 127)
                                    row.Cells("Bill_No").Style.ForeColor = Color.Red
                                    'row.Cells("WorkingStatus").Style.ForeColor = Color.Red
                                Else
                                    row.DefaultCellStyle.BackColor = Color.White
                                End If
                            End If
                        Next

                        ' Set column headers
                        Guna2DataGridView1.Columns("SNo").HeaderText = "S.No"
                        Guna2DataGridView1.Columns("Bill_No").HeaderText = "Bill No"
                        Guna2DataGridView1.Columns("Bill_Date").HeaderText = "Date"
                        Guna2DataGridView1.Columns("Manual_BillNo").HeaderText = "Manual BillNo"
                        Guna2DataGridView1.Columns("Partyname").HeaderText = "Party Name"
                        Guna2DataGridView1.Columns("Itemname").HeaderText = "Item Name"
                        Guna2DataGridView1.Columns("quantity").HeaderText = "Quantity"
                        Guna2DataGridView1.Columns("Amount").HeaderText = "Net Amount"
                        Guna2DataGridView1.Columns("Remarks").HeaderText = "Remarks"

                        ' Set column widths
                        Guna2DataGridView1.Columns("SNo").Width = 25
                        Guna2DataGridView1.Columns("Bill_No").Width = 60
                        Guna2DataGridView1.Columns("Bill_Date").Width = 70
                        Guna2DataGridView1.Columns("Manual_BillNo").Width = 60
                        Guna2DataGridView1.Columns("Partyname").Width = 150
                        Guna2DataGridView1.Columns("Itemname").Width = 150
                        Guna2DataGridView1.Columns("quantity").Width = 80
                        Guna2DataGridView1.Columns("Amount").Width = 80
                        Guna2DataGridView1.Columns("Remarks").Width = 80

                        Guna2DataGridView1.Columns("SNo").DisplayIndex = 0
                        Guna2DataGridView1.Columns("Bill_No").DisplayIndex = 1
                        Guna2DataGridView1.Columns("Bill_Date").DisplayIndex = 2
                        Guna2DataGridView1.Columns("Manual_BillNo").DisplayIndex = 3
                        Guna2DataGridView1.Columns("Partyname").DisplayIndex = 4
                        Guna2DataGridView1.Columns("Itemname").DisplayIndex = 5
                        Guna2DataGridView1.Columns("quantity").DisplayIndex = 6
                        Guna2DataGridView1.Columns("Amount").DisplayIndex = 7
                        Guna2DataGridView1.Columns("Remarks").DisplayIndex = 8

                        ' Set column order
                        Guna2DataGridView1.Columns("SNo").DisplayIndex = 0
                    End Using
                Catch ex As Exception
                    MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub

    Private Sub InitializeDataGridView()
        With Guna2DataGridView1
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
            .MultiSelect = False
            .CellBorderStyle = DataGridViewCellBorderStyle.Single

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

    Private Sub DisplayEntryTypes()
        Dim query As String = "SELECT DISTINCT CASE EntryType WHEN 1 THEN 'Purchase' WHEN 2 THEN 'Sales' ELSE 'Unknown' END AS EntryType FROM purchase_table
                                UNION
                                SELECT DISTINCT CASE EntryType WHEN 1 THEN 'Purchase' WHEN 2 THEN 'Sales' ELSE 'Unknown' END AS EntryType FROM Sales_Table;"

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Using sqlcommand As New SqlCommand(query, sqlconnect)
                Try
                    sqlconnect.Open()
                    Using reader As SqlDataReader = sqlcommand.ExecuteReader()
                        DisplayComboBox.Items.Clear()
                        While reader.Read()
                            DisplayComboBox.Items.Add(reader("EntryType").ToString())
                        End While
                    End Using
                Catch ex As Exception
                    MessageBox.Show("An error occurred while loading Godown Types: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub

    Private Sub DisplayComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DisplayComboBox.SelectedIndexChanged
        LoadSalesData()
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        LoadSalesData()
    End Sub
    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelBtn.Click
        If Guna2DataGridView1.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a row to cancel.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)
        Dim billNoObj As Object = selectedRow.Cells("Bill_No").Value

        If billNoObj Is Nothing OrElse IsDBNull(billNoObj) Then
            MessageBox.Show("No Bill No found in the selected row.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Dim result As DialogResult = MessageBox.Show("Are you sure you want to cancel this entry?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result = DialogResult.No Then
            Exit Sub
        End If

        Dim billNo As String = billNoObj.ToString()
        Dim entryType As String = DisplayComboBox.Text
        Dim tableName As String = String.Empty

        If entryType = "Purchase" Then
            tableName = "purchase_table"
        ElseIf entryType = "Sales" Then
            tableName = "Sales_table"
        Else
            MessageBox.Show("Invalid entry type selected.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()
            Using transaction = sqlconnect.BeginTransaction()
                Try
                    Dim updateQuery As String = $"UPDATE {tableName} SET Cancel = 1 WHERE Bill_No = @BillNo"
                    Using sqlcommand As New SqlCommand(updateQuery, sqlconnect, transaction)
                        sqlcommand.Parameters.AddWithValue("@BillNo", billNo)
                        sqlcommand.ExecuteNonQuery()
                    End Using

                    Dim updateStockQuery As String = "UPDATE Stock_table SET Cancel = 1 WHERE Bill_No = @BillNo"
                    Using sqlcommandStock As New SqlCommand(updateStockQuery, sqlconnect, transaction)
                        sqlcommandStock.Parameters.AddWithValue("@BillNo", billNo)
                        sqlcommandStock.ExecuteNonQuery()
                    End Using

                    transaction.Commit()
                    MessageBox.Show("Entry successfully cancelled.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    LoadSalesData()
                Catch ex As Exception
                    transaction.Rollback()
                    MessageBox.Show("An error occurred while cancelling the entry: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub

    Private Sub Guna2DataGridView1_DoubleClick(sender As Object, e As EventArgs) Handles Guna2DataGridView1.DoubleClick
        Try
            If Guna2DataGridView1.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a row to alter.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim selectedRow As DataGridViewRow = Guna2DataGridView1.SelectedRows(0)
            Dim billNoObj As Object = selectedRow.Cells("Bill_No").Value
            If billNoObj Is Nothing OrElse IsDBNull(billNoObj) Then
                MessageBox.Show("No Bill No found in the selected row.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim billNo As String = billNoObj.ToString()
            Dim entryType As String = DisplayComboBox.Text

            Dim isCancelled As Boolean = False
            Dim tableName As String = If(entryType = "Sales", "Sales_table", "Purchase_table")

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                sqlconnect.Open()
                Using cmd As New SqlCommand($"SELECT Cancel FROM {tableName} WHERE Bill_No = @BillNo", sqlconnect)
                    cmd.Parameters.AddWithValue("@BillNo", billNo)
                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                        isCancelled = Convert.ToBoolean(result)
                    End If
                End Using
            End Using

            If isCancelled Then
                MessageBox.Show("This entry has been cancelled and cannot be altered.", "Entry Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            Dim stockForm As Stock = CType(Application.OpenForms("Stock"), Stock)
            If stockForm Is Nothing Then
                MessageBox.Show("Stock form is not open.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Exit Sub
            End If

            If entryType = "Sales" Then
                stockForm.LoadFormToKryptonNavigator(Of Sales)("Sales")
                For Each page As ComponentFactory.Krypton.Navigator.KryptonPage In stockForm.KryptonDockableNavigator1.Pages
                    If page.Text = "Sales" Then
                        Dim salesForm As Sales = TryCast(page.Controls.OfType(Of Sales).FirstOrDefault(), Sales)
                        If salesForm IsNot Nothing Then
                            salesForm.AlterBillNo = billNo
                            salesForm.LoadSalesEntry(billNo)
                            salesForm.lblSales.Text = "Sales Alteration"
                            salesForm.Savebtn.Text = "Update"
                        End If
                        Exit For
                    End If
                Next
            ElseIf entryType = "Purchase" Then
                stockForm.LoadFormToKryptonNavigator(Of Purchase)("Purchase")
                For Each page As ComponentFactory.Krypton.Navigator.KryptonPage In stockForm.KryptonDockableNavigator1.Pages
                    If page.Text = "Purchase" Then
                        Dim purchaseForm As Purchase = TryCast(page.Controls.OfType(Of Purchase).FirstOrDefault(), Purchase)
                        If purchaseForm IsNot Nothing Then
                            purchaseForm.AlterBillNo = billNo
                            purchaseForm.LoadPurchaseEntry(billNo)
                            purchaseForm.lblPurchase.Text = "Purchase Alteration"
                            purchaseForm.Savebtn.Text = "Update"
                        End If
                        Exit For
                    End If
                Next
            Else
                MessageBox.Show("Invalid entry type selected.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading entry: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End Try
    End Sub



End Class

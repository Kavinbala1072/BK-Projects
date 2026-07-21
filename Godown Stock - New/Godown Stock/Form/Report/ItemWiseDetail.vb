Imports System.Data.SqlClient
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports Guna.UI2.WinForms

Public Class ItemWiseDetail
    Private dtname As DataTable
    Private activeTextbox As Guna.UI2.WinForms.Guna2TextBox

    Private Sub ItemWiseDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FromDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy")
        ToDateTextBox.Text = DateTime.Now.ToString("dd/MM/yyyy")

        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me

        LoadAutoCompleteData()
        InitializeDataGridView()
        AddHandler ListBox1.Click, AddressOf ListBox1_Click
        AddHandler ListBox1.KeyDown, AddressOf ListBox1_KeyDown
        AddHandler ListBox1.LostFocus, AddressOf ListBox1_LostFocus

        ItemComboBox.Text = ""
        Themeload()
        ReportLoad()
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
        ItemComboBox.Focus()
    End Sub
    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles FromDateTextBox.KeyDown, ToDateTextBox.KeyDown, RefreshButton.KeyDown, ItemComboBox.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.SelectNextControl(CType(sender, Control), forward:=True, tabStopOnly:=True, nested:=True, wrap:=True)
        End If
    End Sub

    Private Sub LoadAutoCompleteData()
        dtname = LoadDataTable("SELECT Itemname FROM Item_table where active = 0")
        ListBox1.Visible = False
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

    Private Sub ShowSuggestionsForTextBox(textBox As Guna.UI2.WinForms.Guna2TextBox)
        activeTextbox = textBox

        Dim source As DataTable = If(textBox Is ItemComboBox, dtname, Nothing)

        If source Is Nothing Then
            ListBox1.Visible = False
            Exit Sub
        End If

        Dim dv As New DataView(source)
        Dim filterText As String = textBox.Text.Replace("'", "''")
        Dim columnName As String = source.Columns(0).ColumnName

        If Not String.IsNullOrWhiteSpace(filterText) Then
            dv.RowFilter = $"[{columnName}] LIKE '%{filterText}%'"
        End If

        If dv.Count = 0 Then
            ListBox1.Visible = False
            Exit Sub
        End If

        'If String.IsNullOrWhiteSpace(filterText) Then
        '    textBox.Text = dv(0)(columnName).ToString()
        '    textBox.SelectionStart = textBox.Text.Length
        'End If

        ListBox1.DataSource = dv
        ListBox1.DisplayMember = columnName
        ListBox1.Visible = True
        'ListBox1.Location = New Point(textBox.Left, textBox.Top + textBox.Height)
        ListBox1.Width = textBox.Width

        ListBox1.SelectedIndex = 0
    End Sub

    Private Sub Guna2TextBox_TextChanged(sender As Object, e As EventArgs) Handles ItemComboBox.TextChanged

        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            ShowSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If

    End Sub
    Private Sub Guna2TextBox_Click(sender As Object, e As EventArgs) Handles ItemComboBox.TextChanged

        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            ShowSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If
    End Sub


    Private Sub ListBox1_Click(sender As Object, e As EventArgs)

        If ListBox1.SelectedItem IsNot Nothing AndAlso activeTextbox IsNot Nothing Then
            activeTextbox.Text = DirectCast(ListBox1.SelectedItem, DataRowView)(0).ToString()
            ListBox1.Visible = False
            activeTextbox.SelectionStart = activeTextbox.Text.Length
            activeTextbox.Focus()
        End If

    End Sub

    Private Sub ListBox1_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            ListBox1_Click(sender, e)
        ElseIf e.KeyCode = Keys.Escape Then
            ListBox1.Visible = False
        End If
    End Sub

    Private Sub AnyTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles ItemComboBox.KeyDown

        If ListBox1.Visible Then
            If e.KeyCode = Keys.Down Then
                ListBox1.Focus()
                If ListBox1.Items.Count > 0 Then ListBox1.SelectedIndex = 0
                e.Handled = True
            End If
        End If

    End Sub

    Private Sub TextBox_LostFocus(sender As Object, e As EventArgs) Handles ItemComboBox.LostFocus

        Task.Delay(200).ContinueWith(Sub()
                                         Me.Invoke(Sub()
                                                       If Not ListBox1.Focused Then
                                                           ListBox1.Visible = False
                                                       End If
                                                   End Sub)
                                     End Sub)
    End Sub

    Private Sub ListBox1_LostFocus(sender As Object, e As EventArgs)
        ListBox1.Visible = False
    End Sub

    Private Sub AnyTextBox_GotFocus(sender As Object, e As EventArgs) Handles ItemComboBox.GotFocus

        If TypeOf sender Is Guna.UI2.WinForms.Guna2TextBox Then
            ShowSuggestionsForTextBox(CType(sender, Guna.UI2.WinForms.Guna2TextBox))
        End If

    End Sub
    Private Sub DisplayItem()
        Dim query As String = "SELECT ItemName FROM Item_table"

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Dim command As New SqlCommand(query, sqlconnect)

            Try
                sqlconnect.Open()
                Using reader As SqlDataReader = command.ExecuteReader()
                    Dim itemList As New List(Of String)
                    While reader.Read()
                        itemList.Add(reader("ItemName").ToString())
                    End While
                    ItemComboBox.Text = String.Join(Environment.NewLine, itemList)
                End Using
            Catch ex As Exception
                MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Function GetItemIdByName(itemName As String) As Integer

        Dim query As String = "SELECT ID FROM Item_table WHERE Itemname = @Itemname"

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Using sqlcommand As New SqlCommand(query, sqlconnect)
                sqlcommand.Parameters.AddWithValue("@Itemname", itemName)
                sqlconnect.Open()
                Return Convert.ToInt32(sqlcommand.ExecuteScalar())
            End Using
        End Using
    End Function

    Private Sub ReportLoad()
        Dim fromDate As Date = FromDateTextBox.Text
        Dim toDate As Date = ToDateTextBox.Text

        If Not Date.TryParse(FromDateTextBox.Text, fromDate) OrElse Not Date.TryParse(ToDateTextBox.Text, toDate) Then
            MessageBox.Show("Please enter valid From and To dates.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Exit Sub
        End If

        If String.IsNullOrWhiteSpace(ItemComboBox.Text) Then
            ' MessageBox.Show("Please select an item.")
            Exit Sub
        End If

        Dim itemId As Integer = GetItemIdByName(ItemComboBox.Text)

        Dim query As String = "SET DATEFORMAT DMY;
                                WITH OpeningCalc AS (SELECT 
                                SUM(CASE WHEN EntryType = 1 THEN Quantity ELSE 0 END) -
                                SUM(CASE WHEN EntryType = 2 THEN Quantity ELSE 0 END) AS MovementQty
                                FROM Stock_table WHERE Item_id = @ItemId AND Cancel = 0 AND Stock_date < @FromDate),
                                Opening AS (SELECT i.Itemname, 'Opening Stock' AS PartyName, DATEADD(DAY, -1, @FromDate) AS Stock_date_raw,
                                FORMAT(DATEADD(DAY, -1, @FromDate), 'dd/MM/yyyy') AS Stock_date,
                                NULL AS EntryType,NULL AS Receipt_Quantity,NULL AS Issue_Quantity,
                                ISNULL(i.Quantity, 0) + ISNULL(o.MovementQty, 0) AS Closing_Stock,0 AS RowNum
                                FROM Item_Table i
                                LEFT JOIN OpeningCalc o ON 1=1
                                WHERE i.ID = @ItemId),
                                Transactions AS (SELECT s.ID,i.Itemname,l.PartyName,s.Stock_date AS Stock_date_raw,
                                FORMAT(s.Stock_date, 'dd/MM/yyyy') AS Stock_date,
                                CASE s.EntryType WHEN 1 THEN 'Purchase' WHEN 2 THEN 'Sales' ELSE 'Unknown' END AS EntryType,
                                CASE WHEN s.EntryType = 1 THEN s.Quantity ELSE 0 END AS Receipt_Quantity,
                                CASE WHEN s.EntryType = 2 THEN s.Quantity ELSE 0 END AS Issue_Quantity,
                                CASE WHEN s.EntryType = 1 THEN s.Quantity 
                                WHEN s.EntryType = 2 THEN -s.Quantity ELSE 0 END AS MovementQty,
                                ROW_NUMBER() OVER (ORDER BY s.Stock_date, s.ID) AS RowNum
                                FROM Stock_table s
                                INNER JOIN Item_Table i ON s.Item_id = i.ID
                                INNER JOIN Ledger_Table l ON s.ledger_id = l.ID
                                WHERE s.Item_id = @ItemId AND s.Cancel = 0 AND s.Stock_date BETWEEN @FromDate AND @ToDate),
                                RunningStock AS (SELECT t.Itemname,t.PartyName, t.Stock_date_raw,t.Stock_date,t.EntryType,
                                t.Receipt_Quantity, t.Issue_Quantity,t.RowNum,
                                 o.Closing_Stock + SUM(t.MovementQty) OVER (ORDER BY t.Stock_date_raw, t.ID ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS Closing_Stock
                                FROM Transactions t CROSS JOIN (SELECT Closing_Stock FROM Opening) o)
                                SELECT * FROM (SELECT Itemname, PartyName, Stock_date_raw, Stock_date, EntryType, 
                                Receipt_Quantity, Issue_Quantity, Closing_Stock, RowNum
                                FROM Opening
                                UNION ALL
                                SELECT Itemname, PartyName, Stock_date_raw, Stock_date, EntryType,Receipt_Quantity, Issue_Quantity, Closing_Stock, RowNum
                                FROM RunningStock) AS Combined
                                ORDER BY Stock_date_raw, RowNum;"


        '                       WITH Opening AS (SELECT i.itemname,
        ''Opening Stock' AS partyname,DATEADD(DAY, -1, @FromDate) AS Stock_date_raw,FORMAT(DATEADD(DAY, -1, @FromDate), 'dd/MM/yyyy') AS Stock_date,
        '                           NULL AS EntryType,NULL AS Receipt_Quantity,NULL AS Issue_Quantity,i.Quantity AS Closing_Stock,0 AS RowNum FROM item_table i WHERE i.ID = @ItemId),
        '                           Transactions AS (SELECT s.ID,i.itemname,l.partyname,s.Stock_date AS Stock_date_raw,FORMAT(s.Stock_date, 'dd/MM/yyyy') AS Stock_date,
        '                           CASE s.EntryType WHEN 1 THEN 'Purchase' WHEN 2 THEN 'Sales' ELSE 'Unknown' END AS EntryType,
        '                           CASE WHEN s.EntryType = 1 THEN s.quantity ELSE 0 END AS Receipt_Quantity,
        '                           CASE WHEN s.EntryType = 2 THEN s.quantity ELSE 0 END AS Issue_Quantity,
        '                           CASE WHEN s.EntryType = 1 THEN s.quantity WHEN s.EntryType = 2 THEN -s.quantity ELSE 0 END AS MovementQty,
        '                           i.Quantity AS Opening_Stock,ROW_NUMBER() OVER (ORDER BY s.Stock_date, s.ID) AS RowNum
        '                           FROM Stock_table s
        '                           JOIN Ledger_Table l ON s.ledger_id = l.ID
        '                           JOIN item_table i ON s.item_id = i.ID
        '                           WHERE s.item_id = @ItemId AND s.Stock_date BETWEEN @FromDate AND @ToDate AND s.cancel = 0 ),
        '                           RunningStock AS (SELECT t.itemname,t.partyname,t.Stock_date_raw, t.Stock_date,t.EntryType,t.Receipt_Quantity,t.Issue_Quantity,t.RowNum,t.Opening_Stock +
        '                           SUM(t.MovementQty) OVER (ORDER BY t.Stock_date_raw, t.ID ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW) AS Closing_Stock
        '                           FROM Transactions t)
        '                           SELECT * FROM (SELECT itemname,partyname,Stock_date_raw,Stock_date,EntryType,Receipt_Quantity,Issue_Quantity,Closing_Stock,RowNum FROM Opening
        '                           UNION ALL
        '                           SELECT itemname,partyname,Stock_date_raw,Stock_date,EntryType,Receipt_Quantity,Issue_Quantity,Closing_Stock,RowNum FROM RunningStock) AS Combined
        '                           ORDER BY Stock_date_raw, RowNum;


        '"Select * FROM (Select item_table.itemname,'Opening Stock' AS partyname,DATEADD(DAY, -1, @FromDate) AS Stock_date_raw,
        '  FORMAT(DATEADD(DAY, -1, @FromDate), 'dd/MM/yyyy') AS Stock_date,
        '  NULL AS EntryType,NULL AS Receipt_Quantity,NULL AS Issue_Quantity,item_table.Quantity AS Closing_Stock
        '  FROM item_table WHERE item_table.ID = @ItemId
        '  UNION ALL 
        '  SELECT item_table.itemname,Ledger_Table.partyname,Stock_table.Stock_date AS Stock_date_raw,
        '  FORMAT(Stock_table.Stock_date, 'dd/MM/yyyy') AS Stock_date,
        '  CASE Stock_table.EntryType  WHEN 1 THEN 'Purchase' WHEN 2 THEN 'Sales' ELSE 'Unknown' END AS EntryType,
        '  CASE WHEN Stock_table.EntryType = 1 THEN Stock_table.quantity ELSE 0 END AS Receipt_Quantity,
        '  CASE WHEN Stock_table.EntryType = 2 THEN Stock_table.quantity ELSE 0 END AS Issue_Quantity,
        '  (SUM(CASE 
        '      WHEN Stock_table.EntryType = 1 THEN Stock_table.quantity 
        '      WHEN Stock_table.EntryType = 2 THEN -Stock_table.quantity 
        '      ELSE 0 
        '  END ) 
        '  OVER (PARTITION BY Stock_table.item_id ORDER BY Stock_table.Stock_date, Stock_table.ID ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW)
        '  + item_table.Quantity) AS Closing_Stock
        '  FROM Stock_table
        '  JOIN Ledger_Table ON Stock_table.ledger_id = Ledger_Table.ID
        '  JOIN item_table ON Stock_table.item_id = item_table.ID
        '  WHERE Stock_table.item_id = @ItemId 
        '      AND Stock_table.Stock_date BETWEEN @FromDate AND @ToDate) AS Combined
        '  ORDER BY Stock_date_raw, partyname;"


        Using sqlconnect As SqlConnection = Tools.GetConnection()
            Using command As New SqlCommand(query, sqlconnect)
                command.Parameters.AddWithValue("@ItemId", itemId)
                command.Parameters.AddWithValue("@FromDate", fromDate)
                command.Parameters.AddWithValue("@ToDate", toDate)

                Try
                    sqlconnect.Open()
                    Using reader As SqlDataReader = command.ExecuteReader()
                        Dim dt As New DataTable()
                        dt.Load(reader)

                        If dt.Rows.Count = 0 Then
                            ' MessageBox.Show("No data found for the selected item and date range.")
                            Exit Sub
                        End If

                        dt.Columns.Add("SNo", GetType(Integer))
                        For i As Integer = 0 To dt.Rows.Count - 1
                            dt.Rows(i)("SNo") = i + 1
                        Next

                        Guna2DataGridView1.DataSource = dt

                        If Guna2DataGridView1.Columns.Contains("Stock_date_raw") Then
                            Guna2DataGridView1.Columns("Stock_date_raw").Visible = False
                        End If

                        If Guna2DataGridView1.Columns.Contains("RowNum") Then
                            Guna2DataGridView1.Columns("RowNum").Visible = False
                        End If


                        Guna2DataGridView1.Columns("SNo").HeaderText = "S.No"
                        Guna2DataGridView1.Columns("itemname").HeaderText = "Item Name"
                        Guna2DataGridView1.Columns("partyname").HeaderText = "Party Name"
                        Guna2DataGridView1.Columns("EntryType").HeaderText = "Entry Type"
                        Guna2DataGridView1.Columns("Stock_date").HeaderText = "Date"
                        Guna2DataGridView1.Columns("Receipt_Quantity").HeaderText = "Receipts"
                        Guna2DataGridView1.Columns("Issue_Quantity").HeaderText = "Issues"
                        Guna2DataGridView1.Columns("Closing_Stock").HeaderText = "Closing Stock"

                        Guna2DataGridView1.Columns("SNo").Width = 40
                        Guna2DataGridView1.Columns("itemname").Width = 220
                        Guna2DataGridView1.Columns("partyname").Width = 220

                        Guna2DataGridView1.Columns("SNo").DisplayIndex = 0
                        Guna2DataGridView1.Columns("itemname").DisplayIndex = 1
                        Guna2DataGridView1.Columns("partyname").DisplayIndex = 2
                        Guna2DataGridView1.Columns("EntryType").DisplayIndex = 3
                        Guna2DataGridView1.Columns("Stock_date").DisplayIndex = 4
                        Guna2DataGridView1.Columns("Receipt_Quantity").DisplayIndex = 5
                        Guna2DataGridView1.Columns("Issue_Quantity").DisplayIndex = 6
                        Guna2DataGridView1.Columns("Closing_Stock").DisplayIndex = 7
                    End Using
                Catch ex As Exception
                    MessageBox.Show("An error occurred while loading the data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .ColumnHeadersHeight = 35
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            .ReadOnly = True
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
            .Margin = New Padding(20, 20, 20, 20)
            .MultiSelect = False

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

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        ReportLoad()
    End Sub

    Private Sub PrintButton_Click(sender As Object, e As EventArgs) Handles PrintButton.Click
        ExportToPDF()
    End Sub
    Private Sub ExportToPDF()
        Try
            Dim folderPath As String = Path.Combine(Application.StartupPath, "GS Report\")
            '"C:\GS\GS Report\"



            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If

            Dim filePath As String = Path.Combine(folderPath, $"ItemWiseDetails_{DateTime.Now:yyyyMMdd_HHmmss}.pdf")

            Using fs As New FileStream(filePath, FileMode.Create)
                Dim document As New Document(PageSize.A4, 20, 20, 20, 20) ' Portrait with better margins
                Dim writer As PdfWriter = PdfWriter.GetInstance(document, fs)
                document.Open()

                Dim font As New Font(Font.FontFamily.HELVETICA, 10)
                Dim boldFont As New Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)
                Dim headerFont As New Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)

                Dim compName As String = "", address1 As String = "", address2 As String = "", address3 As String = "", mobile As String = ""
                Using conn As SqlConnection = Tools.GetConnection()
                    conn.Open()
                    Dim cmd As New SqlCommand("SELECT TOP 1 Comp_Name, Comp_Address1, Comp_Address2, Comp_Address3, Mobile FROM company_table", conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            compName = reader("Comp_Name").ToString()
                            address1 = reader("Comp_Address1").ToString()
                            address2 = reader("Comp_Address2").ToString()
                            address3 = reader("Comp_Address3").ToString()
                            mobile = reader("Mobile").ToString()
                        End If
                    End Using
                End Using

                Dim para As Paragraph
                para = New Paragraph(compName.ToUpper(), boldFont) : para.Alignment = Element.ALIGN_CENTER : document.Add(para)
                para = New Paragraph(address1, font) : para.Alignment = Element.ALIGN_CENTER : document.Add(para)
                para = New Paragraph(address2, font) : para.Alignment = Element.ALIGN_CENTER : document.Add(para)
                para = New Paragraph(address3, font) : para.Alignment = Element.ALIGN_CENTER : document.Add(para)
                para = New Paragraph("Mobile: " & mobile, font) : para.Alignment = Element.ALIGN_CENTER : document.Add(para)
                document.Add(New Paragraph(" "))
                para = New Paragraph("ITEM WISE DETAILS REPORT", boldFont) : para.Alignment = Element.ALIGN_CENTER : document.Add(para)
                document.Add(New Paragraph(" "))

                ' Column headers (manually defined for order and control)
                Dim columnHeaders As String() = {"S.No", "Item Name", "Party Name", "Entry Type", "Date", "Receipts", "Issues", "Closing Stock"}
                Dim columnNames As String() = {"SNo", "itemname", "partyname", "EntryType", "Stock_date", "Receipt_Quantity", "Issue_Quantity", "Closing_Stock"}

                Dim table As New PdfPTable(columnNames.Length)
                table.WidthPercentage = 100
                table.SetWidths(New Single() {5, 20, 20, 15, 15, 10, 10, 15}) ' Custom widths

                ' Add headers
                For Each header As String In columnHeaders
                    Dim cell As New PdfPCell(New Phrase(header, headerFont)) With {
                        .BackgroundColor = BaseColor.LIGHT_GRAY,
                        .HorizontalAlignment = Element.ALIGN_CENTER,
                        .Padding = 4
                    }
                    table.AddCell(cell)
                Next

                ' Add rows
                For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                    If Not row.IsNewRow Then
                        For Each colName As String In columnNames
                            Dim value As String = If(row.Cells(colName).Value IsNot Nothing, row.Cells(colName).Value.ToString(), "")
                            Dim align As Integer = If(colName = "itemname" Or colName = "partyname", Element.ALIGN_LEFT, Element.ALIGN_CENTER)
                            Dim cell As New PdfPCell(New Phrase(value, font)) With {
                                .HorizontalAlignment = align,
                                .NoWrap = False,
                                .Padding = 4
                            }
                            table.AddCell(cell)
                        Next
                    End If
                Next

                document.Add(table)
                document.Close()
            End Using

            MessageBox.Show("PDF saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error creating PDF: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


End Class

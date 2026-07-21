Imports System.Data.SqlClient
Imports Guna.UI2.WinForms

Public Class JCDprocessing
    Public Property ManualBillNo As String
    Public Property BillNo As String

    Private Sub JCDprocessing_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeDataGridView()
        LoadProcessingMethods()
        Themeload()
        LoadAddonInputValuesFromDatabase()
        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me
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
                                FooterPanel.BackColor = ColorTranslator.FromHtml(colorString)
                            Catch
                                HeaderPanel.BackColor = Color.FromArgb(34, 40, 49)
                                FooterPanel.BackColor = Color.FromArgb(34, 40, 49)
                            End Try
                        Else
                            HeaderPanel.BackColor = Color.FromArgb(34, 40, 49)
                            FooterPanel.BackColor = Color.FromArgb(34, 40, 49)
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
    Private Sub DBConnect_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        Guna2DataGridView1.Focus()
    End Sub
    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles Guna2DataGridView1.KeyDown, OKButton.KeyDown, ClearButton.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.SelectNextControl(CType(sender, Control), forward:=True, tabStopOnly:=True, nested:=True, wrap:=True)
        End If
    End Sub
    Private Sub LoadAddonInputValuesFromDatabase()
        If String.IsNullOrWhiteSpace(ManualBillNo) OrElse String.IsNullOrWhiteSpace(BillNo) Then
            Return
        End If

        Dim combinedBill As String = $"{ManualBillNo}-{BillNo}"
        Dim addonValues As New Dictionary(Of String, String)

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()

                Dim query As String = "SELECT Processing_Method_Name, Value_Name FROM Addons_Table WHERE JC_BillNO = @combinedBill"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@combinedBill", combinedBill)

                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Dim methodName As String = reader("Processing_Method_Name").ToString()
                            Dim valueName As String = reader("Value_Name").ToString()
                            addonValues(methodName) = valueName
                        End While
                    End Using
                End Using
            End Using

            If addonValues.Count = 0 Then
                ' MessageBox.Show("No addon values found for this bill. You can now enter new values.", "New Addon", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                If row.IsNewRow Then Continue For
                Dim method As String = row.Cells("ProcessingMethod").Value?.ToString().Trim()
                If addonValues.ContainsKey(method) Then
                    row.Cells("InputValue").Value = addonValues(method)
                Else
                    row.Cells("InputValue").Value = ""
                End If
            Next

        Catch ex As Exception
            MessageBox.Show("Error loading addon values from database: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadProcessingMethods()
        Dim query As String = "SELECT Processing_Method FROM Addons_Master where Active = 0 order by Processing_Method Asc"

        Using conn As SqlConnection = Tools.GetConnection()
            Using cmd As New SqlCommand(query, conn)
                Try
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Guna2DataGridView1.Rows.Add(reader("Processing_Method").ToString())
                        End While
                    End Using
                Catch ex As Exception
                    MessageBox.Show("Error loading data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        End Using
    End Sub

    Private Sub InitializeDataGridView()
        With Guna2DataGridView1
            .ColumnHeadersVisible = False
            .AllowUserToAddRows = False
            .AllowUserToOrderColumns = True

            .Columns.Add("ProcessingMethod", "Processing Method")
            .Columns.Add("InputValue", "Input Value")

            .Columns(0).ReadOnly = True
            .Columns(1).ReadOnly = False

            .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 40, 49)
            .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
            .ColumnHeadersHeight = 40

            .ScrollBars = ScrollBars.Both
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect

            .GridColor = Color.FromArgb(100, 100, 100)
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .RowTemplate.DividerHeight = 1
        End With
    End Sub
    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click
        Dim combinedBill As String = $"{ManualBillNo}-{BillNo}"

        Using sqlconnect As SqlConnection = Tools.GetConnection()
            sqlconnect.Open()
            Dim transaction As SqlTransaction = sqlconnect.BeginTransaction()

            Try
                For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                    If row.IsNewRow Then Continue For

                    Dim method As String = row.Cells("ProcessingMethod").Value?.ToString().Trim()
                    Dim inputVal As String = row.Cells("InputValue").Value?.ToString().Trim()

                    If String.IsNullOrWhiteSpace(method) OrElse String.IsNullOrWhiteSpace(inputVal) Then
                        Continue For
                    End If

                    Using getIdCmd As New SqlCommand("SELECT ID FROM Addons_Master WHERE Processing_Method = @method", sqlconnect, transaction)
                        getIdCmd.Parameters.AddWithValue("@method", method)
                        Dim masterIdObj = getIdCmd.ExecuteScalar()

                        If masterIdObj IsNot Nothing Then
                            Dim masterId As Integer = Convert.ToInt32(masterIdObj)

                            Dim recordExists As Boolean
                            Using checkCmd As New SqlCommand("SELECT COUNT(*) FROM Addons_Table WHERE JC_BillNO = @bill AND Processing_Method_Name = @mname", sqlconnect, transaction)
                                checkCmd.Parameters.AddWithValue("@bill", combinedBill)
                                checkCmd.Parameters.AddWithValue("@mname", method)
                                recordExists = (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                            End Using

                            If recordExists Then
                                Using updateCmd As New SqlCommand("UPDATE Addons_Table SET Value_Name = @val, Master_ID = @mid 
                                WHERE JC_BillNO = @bill AND Processing_Method_Name = @mname", sqlconnect, transaction)

                                    updateCmd.Parameters.AddWithValue("@val", inputVal)
                                    updateCmd.Parameters.AddWithValue("@mid", masterId)
                                    updateCmd.Parameters.AddWithValue("@bill", combinedBill)
                                    updateCmd.Parameters.AddWithValue("@mname", method)

                                    updateCmd.ExecuteNonQuery()
                                End Using
                            Else
                                Using insertCmd As New SqlCommand("INSERT INTO Addons_Table (JC_BillNO, Master_ID, Processing_Method_Name, Value_Name)
                                VALUES (@bill, @mid, @mname, @val)", sqlconnect, transaction)

                                    insertCmd.Parameters.AddWithValue("@bill", combinedBill)
                                    insertCmd.Parameters.AddWithValue("@mid", masterId)
                                    insertCmd.Parameters.AddWithValue("@mname", method)
                                    insertCmd.Parameters.AddWithValue("@val", inputVal)

                                    insertCmd.ExecuteNonQuery()
                                End Using
                            End If
                        End If
                    End Using
                Next

                transaction.Commit()
                MessageBox.Show("Saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Me.Close()
            Catch dbEx As Exception
                transaction.Rollback()
                MessageBox.Show("Database error: " & dbEx.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Using
    End Sub

    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        For Each row As DataGridViewRow In Guna2DataGridView1.Rows
            If row.IsNewRow Then Continue For
            row.Cells("InputValue").Value = String.Empty
        Next
    End Sub

    Private Sub Guna2ControlBox1_Click(sender As Object, e As EventArgs) Handles Guna2ControlBox1.Click
        Me.Close()
    End Sub
End Class


'Public Class JCDprocessing
'    Public Property ManualBillNo As String
'    Public Property BillNo As String

'    Private Sub JCDprocessing_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'        'GenerateTextBoxes(5)
'        InitializeDataGridView()
'        LoadProcessingMethods()
'        'LoadAddonInputValuesFromFile()
'        LoadAddonInputValuesFromDatabase()
'    End Sub

'    Private Sub LoadAddonInputValuesFromFile()
'            Dim filePath As String = Path.Combine(Application.StartupPath, "addons.xml")

'            If Not File.Exists(filePath) Then Exit Sub

'            Dim addonDict As New Dictionary(Of String, String)

'            Try
'                Dim xmlDoc As New XmlDocument()
'                xmlDoc.Load(filePath)

'                Dim addonNodes As XmlNodeList = xmlDoc.SelectNodes("/Addons/Addon")

'                For Each addonNode As XmlNode In addonNodes
'                    Dim methodNode As XmlNode = addonNode.SelectSingleNode("Method")
'                    Dim valueNode As XmlNode = addonNode.SelectSingleNode("Value")

'                    If methodNode IsNot Nothing AndAlso valueNode IsNot Nothing Then
'                        Dim method As String = methodNode.InnerText.Trim()
'                        Dim value As String = valueNode.InnerText.Trim()
'                        addonDict(method) = value
'                    End If
'                Next

'                For Each row As DataGridViewRow In Guna2DataGridView1.Rows
'                    If row.IsNewRow Then Continue For
'                    Dim method As String = row.Cells("ProcessingMethod").Value?.ToString().Trim()

'                    If addonDict.ContainsKey(method) Then
'                        row.Cells("InputValue").Value = addonDict(method)
'                    End If
'                Next

'            Catch ex As Exception
'                MessageBox.Show("Error reading XML file: " & ex.Message)
'            End Try
'        End Sub
'    Private Sub LoadAddonInputValuesFromDatabase()
'        If String.IsNullOrWhiteSpace(ManualBillNo) OrElse String.IsNullOrWhiteSpace(BillNo) Then
'            Return
'        End If

'        Dim combinedBill As String = $"{ManualBillNo}-{BillNo}"

'        Dim addonValues As New Dictionary(Of String, String)

'        Try
'            Using conn As SqlConnection = Tools.GetConnection()
'                conn.Open()

'                Dim query As String = "SELECT Processing_Method_Name, Value_Name FROM Addons_Table WHERE JC_BillNO = @combinedBill"
'                Using cmd As New SqlCommand(query, conn)
'                    cmd.Parameters.AddWithValue("@combinedBill", combinedBill)

'                    Using reader As SqlDataReader = cmd.ExecuteReader()
'                        While reader.Read()
'                            Dim methodName As String = reader("Processing_Method_Name").ToString()
'                            Dim valueName As String = reader("Value_Name").ToString()
'                            addonValues(methodName) = valueName
'                        End While
'                    End Using
'                End Using
'            End Using

'            ' Now update the grid input values where processing method matches
'            For Each row As DataGridViewRow In Guna2DataGridView1.Rows
'                If row.IsNewRow Then Continue For
'                Dim method As String = row.Cells("ProcessingMethod").Value?.ToString().Trim()
'                If addonValues.ContainsKey(method) Then
'                    row.Cells("InputValue").Value = addonValues(method)
'                Else
'                    row.Cells("InputValue").Value = "" ' clear if no data in DB
'                End If
'            Next

'        Catch ex As Exception
'            MessageBox.Show("Error loading addon values from database: " & ex.Message)
'        End Try
'    End Sub

'    Private Sub LoadProcessingMethods()
'            Dim query As String = "SELECT Processing_Method FROM Addons_Master"

'            Using conn As SqlConnection = Tools.GetConnection()
'                Using cmd As New SqlCommand(query, conn)
'                    Try
'                        conn.Open()
'                        Using reader As SqlDataReader = cmd.ExecuteReader()
'                            While reader.Read()
'                                Guna2DataGridView1.Rows.Add(
'                                reader("Processing_Method").ToString())
'                                'If(reader("Input_Value") IsNot DBNull.Value, reader("Input_Value").ToString(), "")

'                            End While
'                        End Using
'                    Catch ex As Exception
'                        MessageBox.Show("Error loading data: " & ex.Message)
'                    End Try
'                End Using
'            End Using
'        End Sub
'        Private Sub InitializeDataGridView()
'            With Guna2DataGridView1
'                .ColumnHeadersVisible = False
'                .AllowUserToAddRows = False
'                .AllowUserToOrderColumns = True

'                .Columns.Add("ProcessingMethod", "Processing Method")
'                .Columns.Add("InputValue", "Input Value")

'                .Columns(0).ReadOnly = True
'                .Columns(1).ReadOnly = False

'                .ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(34, 40, 49)
'                .ColumnHeadersDefaultCellStyle.ForeColor = Color.White
'                .ColumnHeadersHeight = 40

'                .ScrollBars = ScrollBars.Both
'                .SelectionMode = DataGridViewSelectionMode.FullRowSelect

'                .GridColor = Color.FromArgb(100, 100, 100)
'                .CellBorderStyle = DataGridViewCellBorderStyle.Single
'                .RowTemplate.DividerHeight = 1
'            End With
'        End Sub
'    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click
'        Dim filePath As String = Path.Combine(Application.StartupPath, "addons.xml")
'        Dim jcd As New JCDisplay
'        Try

'            'If jcd.Guna2DataGridView1.SelectedRows.Count = 0 Then
'            '    MessageBox.Show("Please select a row to get the Manual Bill No.")
'            '    Exit Sub
'            'End If

'            'Dim selectedRow As DataGridViewRow = jcd.Guna2DataGridView1.SelectedRows(0)
'            'Dim BillNo As String = selectedRow.Cells("Manual_BillNo").Value?.ToString().Trim()

'            'If String.IsNullOrWhiteSpace(BillNo) Then
'            '    MessageBox.Show("Manual Bill No is empty in the selected row.")
'            '    Exit Sub
'            'End If
'            Dim combinedBill As String = $"{ManualBillNo}-{BillNo}"

'            ' STEP 2: Write the XML file
'            Dim settings As New XmlWriterSettings() With {
'            .Indent = True,
'            .IndentChars = vbTab,
'            .NewLineOnAttributes = False
'        }

'            Using writer As XmlWriter = XmlWriter.Create(filePath, settings)
'                writer.WriteStartDocument()
'                writer.WriteStartElement("Addons")

'                For Each row As DataGridViewRow In Guna2DataGridView1.Rows
'                    If row.IsNewRow Then Continue For

'                    Dim method As String = row.Cells("ProcessingMethod").Value?.ToString().Trim()
'                    Dim inputVal As String = row.Cells("InputValue").Value?.ToString().Trim()

'                    If Not String.IsNullOrWhiteSpace(method) AndAlso Not String.IsNullOrWhiteSpace(inputVal) Then
'                        writer.WriteStartElement("Addon")
'                        writer.WriteElementString("Method", method)
'                        writer.WriteElementString("Value", inputVal)
'                        writer.WriteEndElement() ' </Addon>
'                    End If
'                Next

'                writer.WriteEndElement() ' </Addons>
'                writer.WriteEndDocument()
'            End Using

'            Using sqlconnect As SqlConnection = Tools.GetConnection()
'                sqlconnect.Open()
'                Dim transaction As SqlTransaction = sqlconnect.BeginTransaction()

'                Try
'                    Dim xmlDoc As New XmlDocument()
'                    xmlDoc.Load(filePath)

'                    Dim addonNodes As XmlNodeList = xmlDoc.SelectNodes("/Addons/Addon")

'                    For Each addonNode As XmlNode In addonNodes
'                        Dim methodNode = addonNode.SelectSingleNode("Method")
'                        Dim valueNode = addonNode.SelectSingleNode("Value")

'                        Dim method As String = If(methodNode IsNot Nothing, methodNode.InnerText.Trim(), "")
'                        Dim value As String = If(valueNode IsNot Nothing, valueNode.InnerText.Trim(), "")

'                        If String.IsNullOrWhiteSpace(method) OrElse String.IsNullOrWhiteSpace(value) Then Continue For

'                        Using getIdCmd As New SqlCommand("SELECT ID FROM Addons_Master WHERE Processing_Method = @method", sqlconnect, transaction)
'                            getIdCmd.Parameters.AddWithValue("@method", method)
'                            Dim masterIdObj = getIdCmd.ExecuteScalar()

'                            If masterIdObj IsNot Nothing Then
'                                Dim masterId As Integer = Convert.ToInt32(masterIdObj)

'                                Using insertCmd As New SqlCommand("
'                                INSERT INTO Addons_Table (JC_BillNO, Master_ID, Processing_Method_Name, Value_Name)
'                                VALUES (@bill, @mid, @mname, @val)", sqlconnect, transaction)

'                                    insertCmd.Parameters.AddWithValue("@bill", combinedBill)
'                                    insertCmd.Parameters.AddWithValue("@mid", masterId)
'                                    insertCmd.Parameters.AddWithValue("@mname", method)
'                                    insertCmd.Parameters.AddWithValue("@val", value)

'                                    insertCmd.ExecuteNonQuery()
'                                End Using
'                            End If
'                        End Using
'                    Next

'                    transaction.Commit()
'                    MessageBox.Show("Saved successfully.")
'                    Me.Close()
'                Catch dbEx As Exception
'                    transaction.Rollback()
'                    MessageBox.Show("Database error: " & dbEx.Message)
'                End Try
'            End Using

'        Catch ex As Exception
'            MessageBox.Show("Error: " & ex.Message)
'        End Try
'    End Sub


'    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
'            Dim filePath As String = Path.Combine(Application.StartupPath, "addons.xml")

'            Try
'                Dim settings As New XmlWriterSettings()
'                settings.Indent = True
'                settings.IndentChars = vbTab
'                settings.NewLineOnAttributes = False

'                Using writer As XmlWriter = XmlWriter.Create(filePath, settings)
'                    writer.WriteStartDocument()
'                    writer.WriteStartElement("Addons")
'                    writer.WriteEndElement()
'                    writer.WriteEndDocument()
'                End Using

'                ' MessageBox.Show("XML content cleared successfully.")
'                Me.Close()
'            Catch ex As Exception
'                MessageBox.Show("Error clearing XML: " & ex.Message)
'            End Try
'        End Sub

'    'Private Sub Guna2ControlBox1_Click(sender As Object, e As EventArgs) Handles Guna2ControlBox1.Click
'    '    Dim filePath As String = Path.Combine(Application.StartupPath, "addons.xml")

'    '    Try
'    '        Dim settings As New XmlWriterSettings()
'    '        settings.Indent = True
'    '        settings.IndentChars = vbTab
'    '        settings.NewLineOnAttributes = False

'    '        Using writer As XmlWriter = XmlWriter.Create(filePath, settings)
'    '            writer.WriteStartDocument()
'    '            writer.WriteStartElement("Addons")
'    '            writer.WriteEndElement()
'    '            writer.WriteEndDocument()
'    '        End Using

'    '        ' MessageBox.Show("XML content cleared successfully.")
'    '        Me.Close()
'    '    Catch ex As Exception
'    '        MessageBox.Show("Error clearing XML: " & ex.Message)
'    '    End Try
'    'End Sub
'End Class



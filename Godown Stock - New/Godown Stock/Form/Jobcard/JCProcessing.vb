Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml
Public Class JCProcessing
    Private Sub JCProcessing_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'GenerateTextBoxes(5)
        InitializeDataGridView()
        LoadProcessingMethods()
        LoadAddonInputValuesFromFile()
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
    Private Sub LoadAddonInputValuesFromFile()
        Dim filePath As String = Path.Combine(Application.StartupPath, "addons.xml")

        If Not File.Exists(filePath) Then Exit Sub

        Dim addonDict As New Dictionary(Of String, String)

        Try
            Dim xmlDoc As New XmlDocument()
            xmlDoc.Load(filePath)

            Dim addonNodes As XmlNodeList = xmlDoc.SelectNodes("/Addons/Addon")

            For Each addonNode As XmlNode In addonNodes
                Dim methodNode As XmlNode = addonNode.SelectSingleNode("Method")
                Dim valueNode As XmlNode = addonNode.SelectSingleNode("Value")

                If methodNode IsNot Nothing AndAlso valueNode IsNot Nothing Then
                    Dim method As String = methodNode.InnerText.Trim()
                    Dim value As String = valueNode.InnerText.Trim()
                    addonDict(method) = value
                End If
            Next

            For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                If row.IsNewRow Then Continue For
                Dim method As String = row.Cells("ProcessingMethod").Value?.ToString().Trim()

                If addonDict.ContainsKey(method) Then
                    row.Cells("InputValue").Value = addonDict(method)
                End If
            Next

        Catch ex As Exception
            MessageBox.Show("Error reading XML file: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadProcessingMethods()
        Dim query As String = "SELECT Processing_Method FROM Addons_Master"

        Using conn As SqlConnection = Tools.GetConnection()
            Using cmd As New SqlCommand(query, conn)
                Try
                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        While reader.Read()
                            Guna2DataGridView1.Rows.Add(
                            reader("Processing_Method").ToString())
                            'If(reader("Input_Value") IsNot DBNull.Value, reader("Input_Value").ToString(), "")

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
        Dim filePath As String = Path.Combine(Application.StartupPath, "addons.xml")

        Try
            Dim settings As New XmlWriterSettings()
            settings.Indent = True
            settings.IndentChars = vbTab
            settings.NewLineOnAttributes = False

            Using writer As XmlWriter = XmlWriter.Create(filePath, settings)
                writer.WriteStartDocument()
                writer.WriteStartElement("Addons")

                For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                    If row.IsNewRow Then Continue For

                    Dim method As String = row.Cells("ProcessingMethod").Value?.ToString().Trim()
                    Dim inputVal As String = row.Cells("InputValue").Value?.ToString().Trim()

                    If Not String.IsNullOrWhiteSpace(method) AndAlso Not String.IsNullOrWhiteSpace(inputVal) Then
                        writer.WriteStartElement("Addon")
                        writer.WriteElementString("Method", method)
                        writer.WriteElementString("Value", inputVal)
                        writer.WriteEndElement() ' </Addon>
                    End If
                Next

                writer.WriteEndElement() ' </Addons>
                writer.WriteEndDocument()
            End Using

            '  MessageBox.Show("Addon data saved to XML file.")
            '  Me.DialogResult = DialogResult.OK
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error writing XML: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        Dim filePath As String = Path.Combine(Application.StartupPath, "addons.xml")

        Try
            Dim settings As New XmlWriterSettings()
            settings.Indent = True
            settings.IndentChars = vbTab
            settings.NewLineOnAttributes = False

            Using writer As XmlWriter = XmlWriter.Create(filePath, settings)
                writer.WriteStartDocument()
                writer.WriteStartElement("Addons")
                writer.WriteEndElement()
                writer.WriteEndDocument()
            End Using

            ' MessageBox.Show("XML content cleared successfully.")
            Me.Close()
        Catch ex As Exception
            MessageBox.Show("Error clearing XML: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class



'Private Sub GenerateTextBoxes(inputCount As Integer)
'    Dim startY As Integer = 10 ' Initial Y position for textboxes
'    For i As Integer = 1 To inputCount
'        Dim txtBox As New TextBox()
'        txtBox.Name = "TextBox" & i ' Assign unique name
'        txtBox.TabIndex = i ' Set tab index
'        txtBox.Location = New Point(10, startY) ' Position the textbox
'        txtBox.Width = 200 ' Set width
'        startY += 30 ' Move the next textbox down
'        Me.Controls.Add(txtBox) ' Add textbox to form controls
'    Next
'End Sub

'Private Sub GenerateTextBoxes(inputCount As Integer)
'    Dim startY As Integer = 40
'    Using sqlconnect As SqlConnection = Tools.GetConnection()
'        Dim checkExistCommand As New SqlCommand("SELECT * FROM ItemGroup_Table", sqlconnect)
'        Try
'            sqlconnect.Open()
'            Dim reader As SqlDataReader = checkExistCommand.ExecuteReader()
'            Dim dt As New DataTable()
'            dt.Load(reader)
'            For i As Integer = 0 To dt.Rows.Count - 1
'                ' Create label
'                Dim lbl As New Label()
'                lbl.Name = dt.Rows(i)("id")
'                lbl.Text = dt.Rows(i)("id")
'                lbl.Location = New Point(10, startY)
'                lbl.AutoSize = True
'                ' Create text box
'                Dim txtBox As New TextBox()
'                txtBox.Name = "Tbox" & dt.Rows(i)("id")
'                txtBox.TabIndex = i
'                txtBox.Location = New Point(100, startY)
'                txtBox.Width = 200
'                startY += 30 ' Move both label and text box down for the next one
'                ' Add controls to form
'                Me.Controls.Add(lbl)
'                Me.Controls.Add(txtBox)
'            Next
'        Catch ex As Exception
'            MessageBox.Show("Error: " & ex.Message)
'        End Try
'    End Using
'End Sub
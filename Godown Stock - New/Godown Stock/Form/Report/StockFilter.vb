Imports System.Data.SqlClient
Imports System.IO
Imports System.Xml
Imports System.Reflection
Imports Guna.UI2.WinForms

Public Class StockFilter
    Private dtGroup, dtBrand, dtModel As DataTable
    Private activeTextbox As Guna2TextBox
    Private isPopulatingText As Boolean = False

    Private selectedGroupID As Integer = -1
    Private selectedBrandID As Integer = -1
    Private selectedModelID As Integer = -1

    Private Sub StockFilter_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim elipse As New Guna2Elipse()
        elipse.BorderRadius = 20
        elipse.TargetControl = Me

        Tools.LoadConfiguration()
        LoadAutoCompleteData()
        LoadFilterValuesFromFile()
        Themeload()
        AddHandler KryptonListBox.KeyDown, AddressOf KryptonListBox_KeyDown
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
        grpbox.Focus()
    End Sub
    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs) _
    Handles grpbox.KeyDown, Brandbox.KeyDown, Modelbox.KeyDown, OKButton.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.SuppressKeyPress = True
            Me.SelectNextControl(CType(sender, Control), forward:=True, tabStopOnly:=True, nested:=True, wrap:=True)
        End If
    End Sub
    Private Sub LoadAutoCompleteData()
        dtGroup = LoadDataTable("SELECT ID, ItemGroup_Name FROM ItemGroup_Table WHERE Active = 0 ORDER BY ItemGroup_Name")
        dtBrand = LoadDataTable("SELECT ID, ItemBrand_Name FROM ItemBrand_Table WHERE Active = 0 ORDER BY ItemBrand_Name")
        dtModel = LoadDataTable("SELECT ID, ItemModel_Name FROM ItemModel_Table WHERE Active = 0 ORDER BY ItemModel_Name")
        KryptonListBox.Visible = False
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

    Private Sub ShowSuggestionsForTextBox(tb As Guna2TextBox)

        If tb Is Nothing OrElse Not tb.Focused Then
            KryptonListBox.Visible = False
            Exit Sub
        End If

        Dim source As DataTable = Nothing
        Dim filterColumn As String = ""

        If tb Is grpbox Then
            source = dtGroup
            filterColumn = "ItemGroup_Name"
        ElseIf tb Is Brandbox Then
            source = dtBrand
            filterColumn = "ItemBrand_Name"
        ElseIf tb Is Modelbox Then
            source = dtModel
            filterColumn = "ItemModel_Name"
        End If

        If source Is Nothing Then Exit Sub

        activeTextbox = tb

        Dim dv As New DataView(source)
        If tb.Text.Trim <> "" Then
            dv.RowFilter = $"[{filterColumn}] LIKE '%{tb.Text.Replace("'", "''")}%'"
        End If

        KryptonListBox.DataSource = dv
        KryptonListBox.DisplayMember = filterColumn

        'Dim screenPos = tb.Parent.PointToScreen(tb.Location)
        'Dim clientPos = Me.PointToClient(screenPos)

        'KryptonListBox.Location = New Point(clientPos.X, clientPos.Y + tb.Height)
        'KryptonListBox.Width = tb.Width
        KryptonListBox.BringToFront()
        KryptonListBox.Visible = dv.Count > 0

        If dv.Count > 0 Then KryptonListBox.SelectedIndex = 0
    End Sub

    Private Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles _
        grpbox.TextChanged, Brandbox.TextChanged, Modelbox.TextChanged

        If Not isPopulatingText Then
            ShowSuggestionsForTextBox(CType(sender, Guna2TextBox))
        End If
    End Sub

    Private Sub TextBox_Interacted(sender As Object, e As EventArgs) Handles _
        grpbox.Click, Brandbox.Click, Modelbox.Click,
        grpbox.GotFocus, Brandbox.GotFocus, Modelbox.GotFocus

        ShowSuggestionsForTextBox(CType(sender, Guna2TextBox))
    End Sub

    Private Sub TextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles _
        grpbox.KeyDown, Brandbox.KeyDown, Modelbox.KeyDown

        If e.KeyCode = Keys.Down AndAlso KryptonListBox.Visible Then
            KryptonListBox.Focus()
            e.Handled = True
        ElseIf e.KeyCode = Keys.Enter AndAlso KryptonListBox.Visible Then
            e.SuppressKeyPress = True
            KryptonListBox_Click(KryptonListBox, EventArgs.Empty)
        End If
    End Sub

    Private Sub KryptonListBox_KeyDown(sender As Object, e As KeyEventArgs) Handles KryptonListBox.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            KryptonListBox_Click(sender, EventArgs.Empty)
        ElseIf e.KeyCode = Keys.Escape Then
            KryptonListBox.Visible = False
            activeTextbox?.Focus()
        End If
    End Sub

    Private Sub KryptonListBox_Click(sender As Object, e As EventArgs) Handles _
        KryptonListBox.Click, KryptonListBox.DoubleClick

        If KryptonListBox.SelectedItem Is Nothing OrElse activeTextbox Is Nothing Then Exit Sub

        Dim row As DataRowView = DirectCast(KryptonListBox.SelectedItem, DataRowView)
        isPopulatingText = True

        If activeTextbox Is grpbox Then
            grpbox.Text = row("ItemGroup_Name").ToString()
            selectedGroupID = CInt(row("ID"))
        ElseIf activeTextbox Is Brandbox Then
            Brandbox.Text = row("ItemBrand_Name").ToString()
            selectedBrandID = CInt(row("ID"))
        ElseIf activeTextbox Is Modelbox Then
            Modelbox.Text = row("ItemModel_Name").ToString()
            selectedModelID = CInt(row("ID"))
        End If

        KryptonListBox.Visible = False
        activeTextbox.SelectionStart = activeTextbox.Text.Length
        activeTextbox.Focus()

        isPopulatingText = False
        Me.SelectNextControl(activeTextbox, True, True, True, True)
    End Sub

    Private Sub Global_LostFocus(sender As Object, e As EventArgs) Handles _
        grpbox.LostFocus, Brandbox.LostFocus, Modelbox.LostFocus, KryptonListBox.LostFocus

        TimerFocusCheck.Start()
    End Sub

    Private Sub TimerFocusCheck_Tick(sender As Object, e As EventArgs) Handles TimerFocusCheck.Tick
        TimerFocusCheck.Stop()

        If activeTextbox IsNot Nothing AndAlso
           Not activeTextbox.Focused AndAlso
           Not KryptonListBox.Focused Then
            KryptonListBox.Visible = False
        End If
    End Sub
    Private Sub ClearButton_Click(sender As Object, e As EventArgs) Handles ClearButton.Click
        Dim filePath As String = Path.Combine(Application.StartupPath, "StockFilter.xml")

        Try
            Using writer As XmlWriter = XmlWriter.Create(filePath, New XmlWriterSettings With {.Indent = True})
                writer.WriteStartDocument()
                writer.WriteStartElement("StockFilter")
                writer.WriteStartElement("Group")
                writer.WriteElementString("Name", "")
                writer.WriteElementString("ID", "")
                writer.WriteEndElement()

                writer.WriteStartElement("Brand")
                writer.WriteElementString("Name", "")
                writer.WriteElementString("ID", "")
                writer.WriteEndElement()

                writer.WriteStartElement("Model")
                writer.WriteElementString("Name", "")
                writer.WriteElementString("ID", "")
                writer.WriteEndElement()

                writer.WriteEndElement()
                writer.WriteEndDocument()
            End Using

            grpbox.Clear()
            Brandbox.Clear()
            Modelbox.Clear()
            selectedGroupID = -1
            selectedBrandID = -1
            selectedModelID = -1

            'MessageBox.Show("Filter values cleared.")
        Catch ex As Exception
            MessageBox.Show("Error clearing XML file: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub LoadFilterValuesFromFile()
        Dim filePath As String = Path.Combine(Application.StartupPath, "StockFilter.xml")

        If Not File.Exists(filePath) Then Exit Sub

        Try
            Dim xmlDoc As New XmlDocument()
            xmlDoc.Load(filePath)

            Dim groupNode As XmlNode = xmlDoc.SelectSingleNode("/StockFilter/Group")
            If groupNode IsNot Nothing Then
                Dim nameNode = groupNode.SelectSingleNode("Name")
                Dim idNode = groupNode.SelectSingleNode("ID")
                If nameNode IsNot Nothing Then grpbox.Text = nameNode.InnerText
                If idNode IsNot Nothing Then selectedGroupID = Convert.ToInt32(idNode.InnerText)
            End If

            Dim brandNode As XmlNode = xmlDoc.SelectSingleNode("/StockFilter/Brand")
            If brandNode IsNot Nothing Then
                Dim nameNode = brandNode.SelectSingleNode("Name")
                Dim idNode = brandNode.SelectSingleNode("ID")
                If nameNode IsNot Nothing Then Brandbox.Text = nameNode.InnerText
                If idNode IsNot Nothing Then selectedBrandID = Convert.ToInt32(idNode.InnerText)
            End If

            Dim modelNode As XmlNode = xmlDoc.SelectSingleNode("/StockFilter/Model")
            If modelNode IsNot Nothing Then
                Dim nameNode = modelNode.SelectSingleNode("Name")
                Dim idNode = modelNode.SelectSingleNode("ID")
                If nameNode IsNot Nothing Then Modelbox.Text = nameNode.InnerText
                If idNode IsNot Nothing Then selectedModelID = Convert.ToInt32(idNode.InnerText)
            End If

        Catch ex As Exception
            MessageBox.Show("Error loading filter values: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub OKButton_Click(sender As Object, e As EventArgs) Handles OKButton.Click
        Dim filePath As String = Path.Combine(Application.StartupPath, "StockFilter.xml")

        Try
            Dim settings As New XmlWriterSettings()
            settings.Indent = True
            settings.IndentChars = vbTab
            settings.NewLineOnAttributes = False

            Using writer As XmlWriter = XmlWriter.Create(filePath, settings)
                writer.WriteStartDocument()
                writer.WriteStartElement("StockFilter")

                If Not String.IsNullOrWhiteSpace(grpbox.Text) Then
                    writer.WriteStartElement("Group")
                    writer.WriteElementString("ID", selectedGroupID.ToString())
                    writer.WriteElementString("Name", grpbox.Text.Trim())
                    writer.WriteEndElement()
                End If

                If Not String.IsNullOrWhiteSpace(Brandbox.Text) Then
                    writer.WriteStartElement("Brand")
                    writer.WriteElementString("ID", selectedBrandID.ToString())
                    writer.WriteElementString("Name", Brandbox.Text.Trim())
                    writer.WriteEndElement()
                End If
                If Not String.IsNullOrWhiteSpace(Modelbox.Text) Then
                    writer.WriteStartElement("Model")
                    writer.WriteElementString("ID", selectedModelID.ToString())
                    writer.WriteElementString("Name", Modelbox.Text.Trim())
                    writer.WriteEndElement()
                End If

                writer.WriteEndElement()
                writer.WriteEndDocument()
            End Using

            '  MessageBox.Show("Filter settings saved to XML file.")
            MessageBox.Show("Filter settings have been saved successfully. Please refresh to apply changes.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)

            Me.DialogResult = DialogResult.OK
            Me.Close()

            'WR.ReportLoad()

        Catch ex As Exception
            MessageBox.Show("Error writing XML: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class
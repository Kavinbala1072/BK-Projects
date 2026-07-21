Imports System.Data.SqlClient
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports Guna.UI2.WinForms

Public Class StockSummary
    Private Sub StockSummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReportLoad()
        Tools.LoadConfiguration()
        InitializeDataGridView()
        Themeload()
        ProgressBar.Visible = False

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
    Private Sub ReportLoad()
        ProgressBar.Visible = True
        ProgressBar.Style = ProgressBarStyle.Marquee

        Task.Run(Sub()
                     Dim query As String = "SELECT i.Itemname, i.Itemgroup,i.Itembrand,i.Itemmodel,i.Quantity AS Opening_Quantity,
                               SUM(CASE WHEN s.cancel = 0 AND s.EntryType = 1 THEN s.quantity WHEN s.cancel = 0 AND s.EntryType = 2 THEN - s.quantity ELSE 0 END) AS Stock_Movement,
                               COALESCE(i.Quantity, 0) + SUM(
                                CASE 
                                    WHEN s.cancel = 0 AND s.EntryType = 1 THEN s.quantity
                                    WHEN s.cancel = 0 AND s.EntryType = 2 THEN -s.quantity
                                    ELSE 0 
                                END) AS Current_Stock
                                FROM Item_Table i
                                LEFT JOIN  Stock_table s ON i.ID = s.item_id
                                where active=0
                                GROUP BY i.ID, i.Itemname, i.Unit, i.Itemgroup, i.Itembrand, i.Itemmodel, i.Quantity
                                ORDER BY i.Itemname;"

                     '"SELECT i.Itemname,i.Itemgroup,i.Itembrand, i.Itemmodel, i.Quantity AS Opening_Quantity,
                     '                        SUM(CASE WHEN s.EntryType = 1 THEN s.quantity WHEN s.EntryType = 2 THEN -s.quantity ELSE 0 END) AS Stock_Movement,
                     '                        COALESCE(i.Quantity, 0) + 
                     '                        SUM(CASE WHEN s.EntryType = 1 THEN s.quantity WHEN s.EntryType = 2 THEN -s.quantity ELSE 0 END) AS Current_Stock
                     '                        FROM Item_Table i
                     '                        LEFT JOIN Stock_table s ON i.ID = s.item_id
                     '                        GROUP BY i.ID, i.Itemname, i.Unit, i.Itemgroup, i.Itembrand, i.Itemmodel, i.Quantity
                     '                        ORDER BY i.Itemname;"

                     Try
                         Using sqlconnect As SqlConnection = Tools.GetConnection()
                             Dim command As New SqlCommand(query, sqlconnect)
                             sqlconnect.Open()

                             Using reader As SqlDataReader = command.ExecuteReader()
                                 Dim dt As New DataTable()
                                 dt.Load(reader)

                                 dt.Columns.Add("SNo", GetType(Integer))
                                 For i As Integer = 0 To dt.Rows.Count - 1
                                     dt.Rows(i)("SNo") = i + 1
                                 Next

                                 Invoke(Sub()
                                            Guna2DataGridView1.DataSource = dt

                                            Guna2DataGridView1.Columns("SNo").DisplayIndex = 0
                                            Guna2DataGridView1.Columns("SNo").HeaderText = "S.No"
                                            Guna2DataGridView1.Columns("SNo").Width = 40

                                            Guna2DataGridView1.Columns("ItemName").HeaderText = "Item Name"
                                            Guna2DataGridView1.Columns("ItemGroup").HeaderText = "Item Group"
                                            Guna2DataGridView1.Columns("ItemBrand").HeaderText = "Item Brand"
                                            Guna2DataGridView1.Columns("ItemModel").HeaderText = "Item Model"
                                            Guna2DataGridView1.Columns("Opening_Quantity").HeaderText = "Opening Stock"
                                            Guna2DataGridView1.Columns("Stock_Movement").HeaderText = "Stock Movement"
                                            Guna2DataGridView1.Columns("Current_Stock").HeaderText = "Current Stock"

                                            Guna2DataGridView1.Columns("ItemName").Width = 250
                                            Guna2DataGridView1.Columns("ItemGroup").Width = 110
                                            Guna2DataGridView1.Columns("ItemBrand").Width = 110
                                            Guna2DataGridView1.Columns("ItemModel").Width = 110

                                            ProgressBar.Visible = False
                                        End Sub)
                             End Using
                         End Using
                     Catch ex As Exception
                         Invoke(Sub()
                                    ProgressBar.Visible = False
                                    MessageBox.Show("An error occurred: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                End Sub)
                     End Try
                 End Sub)
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
            .CellBorderStyle = DataGridViewCellBorderStyle.Single
            .ReadOnly = True
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .AllowUserToResizeColumns = False
            .Margin = New Padding(20, 20, 20, 20)
            .MultiSelect = False
            '.DefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Regular)
            '.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 9, FontStyle.Regular)

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
            Dim companyName As String = "COMPANY"
            Dim titleQuery As String = "SELECT TOP 1 comp_name FROM company_table"

            Using sqlconnect As SqlConnection = Tools.GetConnection()
                Using command As New SqlCommand(titleQuery, sqlconnect)
                    sqlconnect.Open()
                    Dim result = command.ExecuteScalar()
                    If result IsNot Nothing Then
                        companyName = result.ToString()
                    End If
                End Using
            End Using

            Dim folderPath As String = Path.Combine(Application.StartupPath, "GS Report\")
            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If

            Dim filePath As String = Path.Combine(folderPath, $"StockSummary_{DateTime.Now:yyyyMMdd_HHmmss}.pdf")

            Using fs As New FileStream(filePath, FileMode.Create)
                Dim document As New Document(PageSize.A4.Rotate(), 20, 20, 20, 20)
                Dim writer As PdfWriter = PdfWriter.GetInstance(document, fs)
                document.Open()

                Dim titleFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 14, iTextSharp.text.Font.BOLD)
                Dim headerFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD)
                Dim bodyFont As New iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10)

                Dim titleText As String = $"{companyName} - STOCK SUMMARY REPORT".ToUpper()
                Dim titlePara As New Paragraph(titleText, titleFont) With {.Alignment = Element.ALIGN_CENTER}
                document.Add(titlePara)

                document.Add(New Paragraph(" "))

                Dim columnHeaders As String() = {
                    "S.No", "Item Name", "Item Group", "Item Brand", "Item Model",
                    "Opening Stock", "Stock Movement", "Current Stock"
                }

                Dim table As New PdfPTable(columnHeaders.Length)
                table.WidthPercentage = 100
                table.SetWidths(New Single() {5, 25, 15, 15, 15, 10, 10, 10})

                For Each header As String In columnHeaders
                    Dim cell As New PdfPCell(New Phrase(header, headerFont)) With {
                        .HorizontalAlignment = Element.ALIGN_CENTER,
                        .BackgroundColor = BaseColor.LIGHT_GRAY,
                        .Padding = 5
                    }
                    table.AddCell(cell)
                Next

                ' Add rows
                For Each row As DataGridViewRow In Guna2DataGridView1.Rows
                    If Not row.IsNewRow Then
                        table.AddCell(New PdfPCell(New Phrase(row.Cells("SNo").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(row.Cells("ItemName").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_LEFT, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(row.Cells("ItemGroup").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_LEFT, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(row.Cells("ItemBrand").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_LEFT, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(row.Cells("ItemModel").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_LEFT, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(row.Cells("Opening_Quantity").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(row.Cells("Stock_Movement").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
                        table.AddCell(New PdfPCell(New Phrase(row.Cells("Current_Stock").Value?.ToString(), bodyFont)) With {.HorizontalAlignment = Element.ALIGN_CENTER, .Padding = 4})
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
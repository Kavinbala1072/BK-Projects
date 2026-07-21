Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports System.IO

Public Class MSReport

    Private mRow As Integer = 0
    Private PageNumber As Integer = 1

    Private Sub MSReport_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeGridDesign()
        LoadMemberData()
    End Sub

    Private Sub InitializeGridDesign()
        With Guna2DataGridView1
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

    Private Sub LoadMemberData()
        ProgressBar.Value = 0
        ProgressBar.Visible = True

        Dim dt As New DataTable()
        dt.Columns.Add("SNo", GetType(Integer))
        dt.Columns.Add("M_No", GetType(String))
        dt.Columns.Add("Member_Name", GetType(String))
        dt.Columns.Add("Mobile_No", GetType(String))
        dt.Columns.Add("Address_Text", GetType(String))
        dt.Columns.Add("Aadhar_No", GetType(String))
        dt.Columns.Add("Member_Photo", GetType(Byte()))
        dt.Columns.Add("Joining_Date", GetType(DateTime))

        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()

                Dim totalCount As Integer = 0
                Using cmdCount As New SqlCommand("SELECT COUNT(*) FROM Member_Table", conn)
                    totalCount = Convert.ToInt32(cmdCount.ExecuteScalar())
                End Using

                If totalCount > 0 Then
                    ProgressBar.Maximum = totalCount
                Else
                    ProgressBar.Maximum = 100
                End If

                Dim query As String = "SELECT M_No, Member_Name, Mobile_No, Address_Text, Aadhar_No, Member_Photo, Joining_Date FROM Member_Table where M_No != '0' ORDER BY M_No ASC"
                Using cmd As New SqlCommand(query, conn)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        Dim count As Integer = 0
                        While reader.Read()
                            count += 1

                            Dim row As DataRow = dt.NewRow()
                            row("SNo") = count
                            row("M_No") = reader("M_No")
                            row("Member_Name") = reader("Member_Name")
                            row("Mobile_No") = reader("Mobile_No")
                            row("Address_Text") = reader("Address_Text")
                            row("Aadhar_No") = reader("Aadhar_No")
                            row("Member_Photo") = reader("Member_Photo")
                            row("Joining_Date") = reader("Joining_Date")
                            dt.Rows.Add(row)

                            ProgressBar.Value = count

                            If count Mod 5 = 0 Then Application.DoEvents()
                        End While
                    End Using
                End Using

                Guna2DataGridView1.DataSource = dt


                If Guna2DataGridView1.Columns.Count > 0 Then
                    Guna2DataGridView1.Columns("Member_Photo").Visible = False
                    Guna2DataGridView1.Columns("Address_Text").Visible = False

                    Guna2DataGridView1.Columns("SNo").HeaderText = "S.No"
                    Guna2DataGridView1.Columns("M_No").HeaderText = "M.No"
                    Guna2DataGridView1.Columns("Joining_Date").DefaultCellStyle.Format = "dd-MM-yyyy"
                End If

            End Using
        Catch ex As Exception
            MessageBox.Show("Error loading data: " & ex.Message)
        Finally
            ProgressBar.Value = ProgressBar.Maximum

            ProgressBar.Visible = False
        End Try
    End Sub

    Private Function GetImageFromBytes(ByVal bytes As Byte()) As Image
        If bytes Is Nothing OrElse bytes.Length = 0 Then Return Nothing
        Try
            Using ms As New MemoryStream(bytes)
                Return Image.FromStream(ms).Clone()
            End Using
        Catch
            Return Nothing
        End Try
    End Function

    Private Sub PrintButton_Click(sender As Object, e As EventArgs) Handles PrintButton.Click
        If Guna2DataGridView1.Rows.Count = 0 Then
            MessageBox.Show("No data to save.")
            Return
        End If

        Try
            Dim folderPath As String = Path.Combine(Application.StartupPath, "Report")
            If Not Directory.Exists(folderPath) Then
                Directory.CreateDirectory(folderPath)
            End If

            Dim fileName As String = "Member_Register_" & DateTime.Now.ToString("ddMMyyyy_HHmmss") & ".pdf"
            Dim fullPath As String = Path.Combine(folderPath, fileName)

            Dim pd As New PrintDocument
            pd.PrintController = New StandardPrintController()

            pd.DefaultPageSettings.PaperSize = New PaperSize("A4", 827, 1169)
            pd.DefaultPageSettings.Margins = New Margins(40, 40, 40, 40)

            pd.PrinterSettings.PrinterName = "Microsoft Print to PDF"
            pd.PrinterSettings.PrintToFile = True
            pd.PrinterSettings.PrintFileName = fullPath

            AddHandler pd.PrintPage, AddressOf PrintDocument_PrintPage

            mRow = 0
            PageNumber = 1

            pd.Print()
            MessageBox.Show("Report saved successfully in 'Report' folder." & vbCrLf & "File: " & fileName, "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub PrintDocument_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim g As Graphics = e.Graphics

        ' Fonts
        Dim fInst As New Font("Arial", 14, FontStyle.Bold)
        Dim fHeader As New Font("Arial", 10, FontStyle.Bold)
        Dim fBody As New Font("Arial", 9, FontStyle.Regular)
        Dim fSno As New Font("Arial", 10, FontStyle.Bold)

        Dim left As Integer = e.MarginBounds.Left
        Dim y As Integer = e.MarginBounds.Top
        Dim centerX As Integer = e.PageBounds.Width / 2

        ' 1. Institution Header
        Dim compName As String = "ATTMA SEVA ARAKKATTALAI"
        Try
            Using conn As SqlConnection = Tools.GetConnection()
                conn.Open()
                Dim cmd = New SqlCommand("SELECT Comp_Name FROM Company_Table WHERE Comp_No='KR1'", conn)
                Dim res = cmd.ExecuteScalar()
                If res IsNot Nothing Then compName = res.ToString().ToUpper()
            End Using
        Catch : End Try

        g.DrawString(compName, fInst, Brushes.Black, centerX - (g.MeasureString(compName, fInst).Width / 2), y)
        y += 30
        g.DrawString("MEMBER SHIP REGISTER", fHeader, Brushes.Black, centerX - (g.MeasureString("MEMBER SHIP REGISTER", fHeader).Width / 2), y)
        y += 40

        ' 2. Table Headers
        Dim colW As Integer() = {50, 180, 510}
        Dim colN As String() = {"S.No", "Member Name", "Member Details & Photo"}

        g.FillRectangle(Brushes.LightGray, left, y, colW.Sum, 30)
        Dim curX As Integer = left
        For i As Integer = 0 To colN.Length - 1
            g.DrawRectangle(Pens.Black, curX, y, colW(i), 30)
            g.DrawString(colN(i), fHeader, Brushes.Black, curX + 5, y + 7)
            curX += colW(i)
        Next
        y += 30

        ' 3. Data Rows (Box style)
        Dim rowH As Integer = 130

        While mRow < Guna2DataGridView1.Rows.Count
            Dim row As DataGridViewRow = Guna2DataGridView1.Rows(mRow)
            curX = left

            ' Box 1: S.No
            g.DrawRectangle(Pens.Black, curX, y, colW(0), rowH)
            g.DrawString((mRow + 1).ToString & ".", fSno, Brushes.Black, curX + 10, y + 15)
            curX += colW(0)

            ' Box 2: Member Name
            g.DrawRectangle(Pens.Black, curX, y, colW(1), rowH)
            Dim nameStr As String = row.Cells("Member_Name").Value.ToString().ToUpper()
            g.DrawString(nameStr, fHeader, Brushes.Black, New RectangleF(curX + 5, y + 15, colW(1) - 10, rowH - 20))
            curX += colW(1)

            ' Box 3: Photo and Multi-line Details
            g.DrawRectangle(Pens.Black, curX, y, colW(2), rowH)

            ' Photo
            If Not IsDBNull(row.Cells("Member_Photo").Value) Then
                Dim img As Image = GetImageFromBytes(DirectCast(row.Cells("Member_Photo").Value, Byte()))
                If img IsNot Nothing Then
                    g.DrawRectangle(Pens.Gray, curX + 390, y + 10, 110, 110)
                    g.DrawImage(img, curX + 395, y + 15, 100, 100)
                End If
            End If

            ' Details
            Dim detX As Integer = curX + 10
            Dim detY As Integer = y + 10
            Dim addr As String = "Address: " & row.Cells("Address_Text").Value.ToString()
            Dim aadhar As String = "Aadhar No: " & row.Cells("Aadhar_No").Value.ToString()
            Dim mobile As String = "Mobile: " & row.Cells("Mobile_No").Value.ToString()
            Dim jDate As String = "Join Date: " & Convert.ToDateTime(row.Cells("Joining_Date").Value).ToString("dd-MM-yyyy")

            Dim addrRect As New RectangleF(detX, detY, 370, 65)
            g.DrawString(addr, fBody, Brushes.Black, addrRect)
            g.DrawString(aadhar, fBody, Brushes.Black, detX, detY + 65)
            g.DrawString(mobile, fBody, Brushes.Black, detX, detY + 82)
            g.DrawString(jDate, fBody, Brushes.Black, detX, detY + 99)

            y += rowH
            mRow += 1

            ' Page Break
            If y > e.MarginBounds.Bottom - rowH Then
                g.DrawString("Page " & PageNumber, fBody, Brushes.Black, e.MarginBounds.Right - 50, e.MarginBounds.Bottom + 10)
                PageNumber += 1
                e.HasMorePages = True
                Return
            End If
        End While

        g.DrawString("Page " & PageNumber, fBody, Brushes.Black, e.MarginBounds.Right - 50, e.MarginBounds.Bottom + 10)
        e.HasMorePages = False
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        LoadMemberData()
    End Sub

    Private Sub ProgressBar_ValueChanged(sender As Object, e As EventArgs) Handles ProgressBar.ValueChanged
        If ProgressBar.Value < 30 Then
            ProgressBar.ProgressColor = Color.Green
        ElseIf ProgressBar.Value < 70 Then
            ProgressBar.ProgressColor = Color.Green
        Else
            ProgressBar.ProgressColor = Color.Green
        End If
    End Sub

End Class
Imports System.IO
Imports System.Xml.Linq
Imports System.Management
Imports System.Security.Cryptography
Imports System.Text
Imports System.Data.SqlClient

Public Class KeyForm
    Private base1 As Integer
    Private base2 As Integer
    Private base3 As Integer

    Private Structure KeyParts
        Public Part1 As Integer
        Public Part2 As Integer
        Public Part3 As Integer
    End Structure

    Private Function GetSystemIdentifier() As String
        Dim processorId As String = ""
        Dim motherboardSerial As String = ""

        Try
            Dim searcher1 As New ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor")
            For Each obj In searcher1.Get()
                processorId = obj("ProcessorId").ToString()
                Exit For
            Next

            Dim searcher2 As New ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard")
            For Each obj In searcher2.Get()
                motherboardSerial = obj("SerialNumber").ToString()
                Exit For
            Next
        Catch ex As Exception
            MsgBox("Error retrieving system ID: " & ex.Message, MsgBoxStyle.Critical)
            Me.Close()
        End Try

        Return processorId & motherboardSerial
    End Function

    Private Function GetHashedNumbers(identifier As String) As KeyParts
        Dim sha256 As SHA256 = SHA256.Create()
        Dim hashBytes() As Byte = sha256.ComputeHash(Encoding.UTF8.GetBytes(identifier))

        Dim kp As KeyParts
        kp.Part1 = 10000 + (BitConverter.ToUInt16(hashBytes, 0) Mod 90000)
        kp.Part2 = 10000 + (BitConverter.ToUInt16(hashBytes, 4) Mod 90000)
        kp.Part3 = 10000 + (BitConverter.ToUInt16(hashBytes, 8) Mod 90000)
        Return kp
    End Function

    Private Sub KeyForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim folderPath As String = Path.Combine(Application.StartupPath, "BK Key")
        Dim filePath As String = Path.Combine(folderPath, "activation.xml")

        Dim identifier As String = GetSystemIdentifier()
        Dim hashed = GetHashedNumbers(identifier)

        base1 = hashed.Part1
        base2 = hashed.Part2
        base3 = hashed.Part3

        LblKey.Text = $"{base1} {base2} {base3}"
        LblKey.ForeColor = Color.White

        If File.Exists(filePath) Then
            Try
                Dim xml = XElement.Load(filePath)
                Dim key1 = Integer.Parse(xml.Element("KeyPart1").Value)
                Dim key2 = Integer.Parse(xml.Element("KeyPart2").Value)
                Dim key3 = Integer.Parse(xml.Element("KeyPart3").Value)

                Dim day = Date.Today.Day
                Dim month = Date.Today.Month
                Dim year = Date.Today.Year

                If key1 = base1 + day AndAlso key2 = base2 + month AndAlso key3 = base3 + year Then
                    LblError.Text = "Already activated."
                    LblError.ForeColor = Color.Green
                    Me.Close()
                End If
            Catch
                ' Ignore errors and allow re-activation
            End Try
        End If
    End Sub

    Private Sub ActivateBtn_Click(sender As Object, e As EventArgs) Handles ActivateBtn.Click
        Dim input = KeyTxt.Text.Trim()
        Dim parts() As String = input.Split(" "c)

        If parts.Length <> 3 Then
            LblError.Text = "Invalid format. Use: 12345 67890 11123"
            LblError.ForeColor = Color.Red
            Return
        End If

        Dim input1, input2, input3 As Integer
        If Not Integer.TryParse(parts(0), input1) OrElse Not Integer.TryParse(parts(1), input2) OrElse Not Integer.TryParse(parts(2), input3) Then
            LblError.Text = "Invalid numbers."
            LblError.ForeColor = Color.Red
            Return
        End If

        Dim day = Date.Today.Day
        Dim month = Date.Today.Month
        Dim year = Date.Today.Year

        If input1 = base1 + day AndAlso input2 = base2 + month AndAlso input3 = base3 + year Then
            Dim folderPath = Path.Combine(Application.StartupPath, "BK Key")
            Dim filePath = Path.Combine(folderPath, "activation.xml")
            If Not Directory.Exists(folderPath) Then Directory.CreateDirectory(folderPath)

            Dim activationDate As Date = Date.Today
            Dim expiryDate As Date = activationDate.AddDays(365)

            Dim xml As New XElement("Activation",
                New XElement("Date", activationDate.ToString("yyyy-MM-dd")),
                New XElement("KeyPart1", input1),
                New XElement("KeyPart2", input2),
                New XElement("KeyPart3", input3),
                New XElement("ToDate", expiryDate.ToString("yyyy-MM-dd"))
            )

            xml.Save(filePath)
            MessageBox.Show("Key activated successfully!", "Activation", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Me.Close()
        Else
            LblError.Text = "Incorrect activation key."
            LblError.ForeColor = Color.Red
        End If
    End Sub

    Private Sub LaterBtn_Click(sender As Object, e As EventArgs) Handles LaterBtn.Click
        Me.Close()
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        Clipboard.SetText(LblKey.Text)
    End Sub
End Class

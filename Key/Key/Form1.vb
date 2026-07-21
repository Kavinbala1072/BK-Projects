Imports System.Text
Imports System.Text.RegularExpressions
Imports kgmtools
Imports Microsoft.VisualBasic

Public Class Form1
    Private stf As New kgmtools.STDFunc()
    Private comp As New kgmtools.clsComputerInfo()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            cmbSystemType.Items.Clear()
            cmbSystemType.Items.AddRange({"0 - PC", "1 - Server", "2 - Node", "3 - Laptop"})
            cmbSystemType.SelectedIndex = 0

            cmbPackage.Items.Clear()
            cmbPackage.Items.AddRange({"StoresERP", "PetrolERP", "CustomerERP", "Others", "EduERP", "HpERP", "TransportERP", "TextileERP", "KGMShoppy", "KGMCRM", "KGMSilk", "KGMJewel", "KGMPos"})
            cmbPackage.SelectedIndex = 0

            txtCustId.Text = "2453"
            txtValidity.Text = "35"
            txtVersion.Text = "01"

            txtValidity.Enabled = False
            txtVersion.Enabled = False
            'txtResult.Enabled = False

            Dim hwid As String = comp.Get_HWID()
            txtUKey.Text = FormatKey(comp.Get_Key(hwid))

        Catch ex As Exception
            MessageBox.Show("Initialization Error: " & ex.Message)
        End Try
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click

        If String.IsNullOrWhiteSpace(txtCustId.Text) Then
            MessageBox.Show("Customer ID cannot be empty. Please enter a valid ID.")
            txtCustId.Focus()
            Exit Sub
        End If
        If String.IsNullOrWhiteSpace(txtUKey.Text) Then
            MessageBox.Show("Customer ID cannot be empty. Please enter a valid ID.")
            txtCustId.Focus()
            Exit Sub
        End If

        Try
            Dim uKeyRaw As String = stf.MtriM(txtUKey.Text)

            If uKeyRaw.Length <> 60 Then
                MessageBox.Show("System Key must be exactly 60 digits.")
                Exit Sub
            End If

            Dim r1 As String = StrReverse(uKeyRaw.Substring(0, 12))
            Dim r2 As String = StrReverse(uKeyRaw.Substring(12, 12))
            Dim r3 As String = StrReverse(uKeyRaw.Substring(24, 12))
            Dim r4 As String = StrReverse(uKeyRaw.Substring(36, 12))
            Dim r5 As String = StrReverse(uKeyRaw.Substring(48, 12))

            Dim p9 As String = r1.Substring(0, 5)
            Dim p10 As String = r1.Substring(5, 7)
            Dim p11 As String = r2.Substring(0, 4)
            Dim p12 As String = r2.Substring(4, 8)
            Dim p13 As String = r3.Substring(0, 3)
            Dim p14 As String = r3.Substring(3, 9)
            Dim p15 As String = r4.Substring(0, 8)
            Dim p16 As String = r4.Substring(8, 4)
            Dim p17 As String = r5.Substring(0, 7)
            Dim p18 As String = r5.Substring(7, 5)

            Dim scrambled As String = p13 & p16 & p9 & p18 & p12 & p15 & p14 & p17 & p10 & p11
            Dim keyArray As Char() = scrambled.ToCharArray()

            Inject(keyArray, 13, stf.PADL(txtCustId.Text, 6, "0"))          ' CustID
            Inject(keyArray, 23, stf.PADL(txtValidity.Text, 2, "0"))        ' Validity
            Inject(keyArray, 41, cmbSystemType.SelectedIndex.ToString())    ' SysType
            Inject(keyArray, 51, stf.PADL(txtVersion.Text, 2, "0"))         ' Version

            Dim finalKey As String = New String(keyArray)
            txtResult.Text = FormatKey(finalKey)

            If Check_UserKey() Then
                Label7.Text = "Key is valid."
                Label7.ForeColor = Color.Green
            Else
                Label7.Text = "Key is invalid."
                Label7.ForeColor = Color.Red
            End If

        Catch ex As Exception
            MessageBox.Show("Generation Error: " & ex.Message)
        End Try
    End Sub

    Private Function Check_UserKey() As Boolean
        Dim text As String = stf.MtriM(txtResult.Text)
        Dim text2 As String = text.Substring(13, 6)   ' Customer ID
        Dim num As Integer = Integer.Parse(text.Substring(23, 2)) ' Validity/Days
        Dim num2 As Integer = Integer.Parse(text.Substring(41, 1)) ' System Type
        Dim num3 As Integer = Integer.Parse(text.Substring(51, 2)) ' Version

        Dim text3 As String = stf.MtriM(txtUKey.Text)
        Dim text4 As String = StrReverse(text3.Substring(0, 12))
        Dim text5 As String = StrReverse(text3.Substring(12, 12))
        Dim text6 As String = StrReverse(text3.Substring(24, 12))
        Dim text7 As String = StrReverse(text3.Substring(36, 12))
        Dim text8 As String = StrReverse(text3.Substring(48, 12))

        Dim text9 As String = text4.Substring(0, 5)
        Dim text10 As String = text4.Substring(5, 7)
        Dim text11 As String = text5.Substring(0, 4)
        Dim text12 As String = text5.Substring(4, 8)
        Dim text13 As String = text6.Substring(0, 3)
        Dim text14 As String = text6.Substring(3, 9)
        Dim text15 As String = text7.Substring(0, 8)
        Dim text16 As String = text7.Substring(8, 4)
        Dim text17 As String = text8.Substring(0, 7)
        Dim text18 As String = text8.Substring(7, 5)

        text3 = String.Concat(New String() {text13, text16, text9, text18, text12, text15, text14, text17, text10, text11})

        text3 = text3.Remove(13, 6).Insert(13, text2)
        text3 = text3.Remove(23, 2).Insert(23, stf.PADL(num.ToString(), 2, "0"))
        text3 = text3.Remove(41, 1).Insert(41, num2.ToString())
        text3 = text3.Remove(51, 2).Insert(51, stf.PADL(num3.ToString(), 2, "0"))

        'Dim pkgCode As Integer = Integer.Parse(text.Substring(57, 2))

        '' Rebuild scrambled hardware key
        'text3 = text3.Remove(13, 6).Insert(13, text2)
        'text3 = text3.Remove(23, 2).Insert(23, stf.PADL(num.ToString(), 2, "0"))
        'text3 = text3.Remove(41, 1).Insert(41, num2.ToString())
        'text3 = text3.Remove(51, 2).Insert(51, stf.PADL(num3.ToString(), 2, "0"))
        'text3 = text3.Remove(57, 2).Insert(57, stf.PADL(pkgCode.ToString(), 2, "0"))


        Return String.Compare(text, text3, True) = 0
    End Function

    Private Sub Inject(ByRef target As Char(), index As Integer, value As String)
        Dim vChars = value.ToCharArray()
        For i As Integer = 0 To vChars.Length - 1
            If (index + i) < target.Length Then
                target(index + i) = vChars(i)
            End If
        Next
    End Sub

    'Private Function FormatKey(input As String) As String
    '    Dim raw As String = Regex.Replace(input, "[^A-Z0-9]", "")
    '    If raw.Length <> 60 Then Return input

    '    Dim sb As New StringBuilder()
    '    For i As Integer = 0 To 59 Step 6
    '        If i > 0 Then sb.Append("-")
    '        sb.Append(raw.Substring(i, 6))
    '    Next
    '    Return sb.ToString()
    'End Function


    Private Function FormatKey(input As String) As String
        Dim raw As String = Regex.Replace(input, "[^A-Z0-9]", "")
        If raw.Length <> 60 Then Return input

        Dim sb As New StringBuilder()
        For i As Integer = 0 To 59 Step 6
            sb.Append(raw.Substring(i, 6))
            If i < 54 Then
                sb.Append(" - ")
            End If
        Next
        Return sb.ToString()
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Clipboard.SetText(txtResult.Text)
        'MessageBox.Show("Text copied to clipboard!")
    End Sub

End Class

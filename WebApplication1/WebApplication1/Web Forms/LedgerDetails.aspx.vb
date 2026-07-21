Imports System.Data
Imports System.Data.SqlClient

Public Class LedgerDetail
    Inherits System.Web.UI.Page

    Dim compNo As String = "ED1"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Cache Control
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1))
        Response.Cache.SetNoStore()

        If Session("UserName") Is Nothing OrElse Request.Cookies("SQL_Server") Is Nothing Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        ' Get Company Code from Cookie
        If Request.Cookies("Selected_CompNo") IsNot Nothing Then
            compNo = Request.Cookies("Selected_CompNo").Value
        End If

        If Not IsPostBack Then
            LoadLedgerList()
        End If
    End Sub

    Private Sub LoadLedgerList()
        Using conn As New SqlConnection(GetDBConn())
            Dim sql As String = "SELECT led_name, ledsel_mtname FROM ledsel_table " &
                                "INNER JOIN ledger_table ON ledger_sno=ledger_no " &
                                "WHERE ledger_table.comp_no = @cp " &
                                "GROUP BY led_name, ledsel_mtname ORDER BY led_name"

            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@cp", compNo)

            Try
                conn.Open()
                lstLedgers.DataSource = cmd.ExecuteReader()
                lstLedgers.DataTextField = "led_name"
                lstLedgers.DataValueField = "ledsel_mtname"
                lstLedgers.DataBind()
            Catch ex As Exception
                lblStatus.Text = "List Error: " & ex.Message
            End Try
        End Using
    End Sub

    Protected Sub lstLedgers_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstLedgers.SelectedIndexChanged
        If lstLedgers.SelectedItem IsNot Nothing Then
            ' Update top bar
            txtSelectedLedger.Text = lstLedgers.SelectedItem.Text
            ' Load details
            LoadLedgerDetails(lstLedgers.SelectedValue)
        End If
    End Sub

    Private Sub LoadLedgerDetails(mtName As String)
        Using conn As New SqlConnection(GetDBConn())
            conn.Open()

            ' 1. Load Profile Data
            Dim sqlDetail As String = "Select L.ledger_name, L.ledger_Add1, L.ledger_Add2, L.ledger_Add3, L.Ledger_GST, L.Ledger_TIN, L.Ledger_AadharNo, L.Limit_amount, " &
                "isnull(ledgergroup_name,'') as ledgrpName, isnull(Area_Name,'') as Area_Name, isnull(Contact_Mobile,'') as Mobile, isnull(Contact_EmailId,'') as Email, " &
                "IsNull(Agent.Ledger_Name,'') As AgentName, IsNull(SalMan.Ledger_Name,'') As SalManName " &
                "from Ledger_table L " &
                "LEFT JOIN Area_Table On Area_Table.Area_Sno = L.Area_No AND Area_Table.Comp_No=L.Comp_No " &
                "LEFT JOIN ledgergroup_Table On ledgergroup_Table.Ledgergroup_sno = L.ledger_groupno AND ledgergroup_Table.Comp_No=L.Comp_No " &
                "Left Join Contact_Details on L.ledger_sno=Contact_Sno " &
                "Left Join LEdger_Table Agent on L.Ledger_Broker = Agent.Ledger_Sno " &
                "Left Join LEdger_Table SalMan on L.ledger_SalMan = SalMan.ledger_Sno " &
                "Where L.comp_no = @cp AND L.ledger_mtname = @mt"

            Dim cmdDetail As New SqlCommand(sqlDetail, conn)
            cmdDetail.Parameters.AddWithValue("@cp", compNo)
            cmdDetail.Parameters.AddWithValue("@mt", mtName)

            Dim dr As SqlDataReader = cmdDetail.ExecuteReader()
            If dr.Read() Then
                litMainName.Text = dr("ledger_name").ToString()
                litGroupName.Text = dr("ledgrpName").ToString()
                litArea.Text = dr("Area_Name").ToString()
                litMobile.Text = dr("Mobile").ToString()
                litEmail.Text = dr("Email").ToString()
                litAdd1.Text = dr("ledger_Add1").ToString()
                litAdd2.Text = dr("ledger_Add2").ToString()
                litAdd3.Text = dr("ledger_Add3").ToString()
                litGSTNo.Text = dr("Ledger_GST").ToString()
                litTIN.Text = dr("Ledger_TIN").ToString()
                'litAadhar.Text = dr("Ledger_AadharNo").ToString()
                litSalesman.Text = dr("SalManName").ToString()
                litAgent.Text = dr("AgentName").ToString()

                Dim limitAmt As Decimal = If(IsDBNull(dr("Limit_amount")), 0, Convert.ToDecimal(dr("Limit_amount")))
                litLimit.Text = limitAmt.ToString("N2")

                pnlDetails.Visible = True
                pnlEmpty.Visible = False
            End If
            dr.Close()

            ' 2. Calculate Balance (Sum of Dr - Cr)
            Dim sqlBal As String = "SELECT ISNULL(SUM(dr_amount - cr_amount), 0) FROM Vch_table v " &
                                   "INNER JOIN ledger_table l ON l.ledger_sno=v.ledger_no " &
                                   "WHERE v.Vch_cancel='False' AND v.comp_no=@cp AND l.ledger_mtname=@mt"

            Dim cmdBal As New SqlCommand(sqlBal, conn)
            cmdBal.Parameters.AddWithValue("@cp", compNo)
            cmdBal.Parameters.AddWithValue("@mt", mtName)

            Dim result = cmdBal.ExecuteScalar()
            litBalance.Text = Convert.ToDecimal(If(IsDBNull(result), 0, result)).ToString("N2")
        End Using
    End Sub

    Private Function GetDBConn() As String
        Dim s = Server.UrlDecode(Request.Cookies("SQL_Server").Value).Replace("\\", "\")
        Dim d = Request.Cookies("SQL_DB").Value
        Dim u = Request.Cookies("SQL_User").Value
        Dim p = Server.UrlDecode(Request.Cookies("SQL_Pass").Value)
        Return String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3};", s, d, u, p)
    End Function

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Write("<script>window.top.location.href='MainForm.aspx';</script>")
    End Sub
End Class
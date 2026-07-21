Imports System.Data
Imports System.Data.SqlClient

Public Class OutstandingPayable
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        UserGuard.ValidateSession(Me)
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1))
        Response.Cache.SetNoStore()

        If Session("UserName") Is Nothing OrElse Request.Cookies("SQL_Server") Is Nothing Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Not IsPostBack Then
            Dim fyEnd As String = DateTime.Now.ToString("yyyy-MM-dd")
            If Request.Cookies("Selected_PeriodTo") IsNot Nothing Then
                fyEnd = Request.Cookies("Selected_PeriodTo").Value
            End If

            txtToDate.Text = DateTime.Now.ToString("yyyy-MM-dd")

            txtToDate.Attributes("max") = fyEnd

            BindFilterLists()

            BindReportData()
        End If
    End Sub
    Private Sub BindFilterLists()
        Try
            Dim s = Server.UrlDecode(Request.Cookies("SQL_Server").Value).Replace("\\", "\")
            Dim d = Request.Cookies("SQL_DB").Value
            Dim u = Request.Cookies("SQL_User").Value
            Dim p = Server.UrlDecode(Request.Cookies("SQL_Pass").Value)

            Dim connStr = Tools.GetConnectionString(s, d, u, p)
            Dim compNo As String = If(Request.Cookies("Selected_CompNo") IsNot Nothing, Request.Cookies("Selected_CompNo").Value, "ED1")

            Using conn As New SqlConnection(connStr)
                conn.Open()

                Dim cmdG As New SqlCommand("SELECT DISTINCT Ledgergroup_name FROM Ledgergroup_Table WHERE comp_no=@cp ORDER BY Ledgergroup_name", conn)
                cmdG.Parameters.AddWithValue("@cp", compNo)
                ddlGroup.DataSource = cmdG.ExecuteReader()
                ddlGroup.DataTextField = "Ledgergroup_name"
                ddlGroup.DataBind()
                ddlGroup.Items.Insert(0, New ListItem("All Groups", ""))
                conn.Close() : conn.Open()

                Dim cmdA As New SqlCommand("SELECT DISTINCT Area_name FROM Area_Table WHERE comp_no=@cp ORDER BY Area_name", conn)
                cmdA.Parameters.AddWithValue("@cp", compNo)
                ddlArea.DataSource = cmdA.ExecuteReader()
                ddlArea.DataTextField = "Area_name"
                ddlArea.DataBind()
                ddlArea.Items.Insert(0, New ListItem("All Areas", ""))
            End Using
        Catch : End Try
    End Sub

    Private Sub BindReportData()
        Try
            Dim s = Server.UrlDecode(Request.Cookies("SQL_Server").Value).Replace("\\", "\")
            Dim d = Request.Cookies("SQL_DB").Value
            Dim u = Request.Cookies("SQL_User").Value
            Dim p = Server.UrlDecode(Request.Cookies("SQL_Pass").Value)

            Dim connStr = Tools.GetConnectionString(s, d, u, p)
            Dim compNo As String = If(Request.Cookies("Selected_CompNo") IsNot Nothing, Request.Cookies("Selected_CompNo").Value, "ED1")

            Using conn As New SqlConnection(connStr)
                Dim query As String = "
                    SELECT 
                        CASE WHEN ledger_Name = '' THEN 'Empty Ledger' ELSE ledger_name END AS ledger_Name,
                        ISNULL(Area_name,'') AS Area_name,
                        Ledgergroup_name,
                        Ledger_Table.comp_no,
                        SUM(Dr_amount - Cr_amount) AS TotalBalance 
                    FROM vch_table 
                    LEFT JOIN Ledger_Table ON Vch_Table.Ledger_No = Ledger_Table.Ledger_Sno AND Vch_Table.Comp_No = Ledger_Table.Comp_No  
                    LEFT JOIN Area_Table ON ledger_table.Area_No = Area_Table.Area_Sno AND Area_Table.Comp_No = Ledger_Table.Comp_No  
                    LEFT JOIN Ledgergroup_Table ON ledgergroup_Table.Ledgergroup_sNo = Ledger_Table.Ledger_groupno AND ledgergroup_Table.Comp_No = Ledger_Table.Comp_No  
                    WHERE 
                        Vch_Table.Comp_No = @comp 
                        AND Vch_Cancel = 'False' 
                        AND Vch_Date <= @to 
                        AND (Ledger_GNo IN (36, 201, 203, 205, 43, 208, 209)) "

                If ddlGroup.SelectedIndex > 0 Then query &= " AND Ledgergroup_name = @group "
                If ddlArea.SelectedIndex > 0 Then query &= " AND Area_name = @area "

                query &= " GROUP BY ledgergroup_name, area_name, Ledger_Name, Ledger_Table.comp_no "

                Dim typeFilter As String = ddlType.SelectedValue
                If typeFilter = "DR" Then
                    query &= " HAVING SUM(Dr_amount - Cr_amount) > 0 "
                ElseIf typeFilter = "CR" Then
                    query &= " HAVING SUM(Dr_amount - Cr_amount) < 0 "
                Else
                    query &= " HAVING SUM(Dr_amount - Cr_amount) <> 0 "
                End If

                query &= " ORDER BY Ledger_name"

                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@to", DateTime.Parse(txtToDate.Text))
                cmd.Parameters.AddWithValue("@comp", compNo)
                If ddlGroup.SelectedIndex > 0 Then cmd.Parameters.AddWithValue("@group", ddlGroup.SelectedValue)
                If ddlArea.SelectedIndex > 0 Then cmd.Parameters.AddWithValue("@area", ddlArea.SelectedValue)

                Dim da As New SqlDataAdapter(cmd)
                Dim dt As New DataTable()
                da.Fill(dt)

                gvOutstanding.DataSource = dt
                gvOutstanding.DataBind()
            End Using
        Catch ex As Exception
            ' Error Handling
        End Try
    End Sub

    'Private Sub BindReportData()
    '    Try
    '        Dim s = Server.UrlDecode(Request.Cookies("SQL_Server").Value).Replace("\\", "\")
    '        Dim d = Request.Cookies("SQL_DB").Value
    '        Dim u = Request.Cookies("SQL_User").Value
    '        Dim p = Server.UrlDecode(Request.Cookies("SQL_Pass").Value)

    '        Dim connStr = Tools.GetConnectionString(s, d, u, p)

    '        Dim compNo As String = If(Request.Cookies("Selected_CompNo") IsNot Nothing, Request.Cookies("Selected_CompNo").Value, "ED1")

    '        Using conn As New SqlConnection(connStr)
    '            Dim query As String = "
    '                Select 
    '                    case when ledger_Name = '' then 'Empty Ledger' else ledger_name end as ledger_Name,
    '                    isNull(Area_name,'') as Area_name,
    '                    Ledgergroup_name,
    '                    Ledger_Table.comp_no,
    '                    sum(Dr_amount - Cr_amount) as TotalBalance 
    '                from vch_table 
    '                LEFT JOIN Ledger_Table on Vch_Table.Ledger_No = Ledger_Table.Ledger_Sno and Vch_Table.Comp_No = Ledger_Table.Comp_No  
    '                LEFT JOIN Area_Table on ledger_table.Area_No = Area_Table.Area_Sno and Area_Table.Comp_No = Ledger_Table.Comp_No  
    '                LEFT JOIN Ledgergroup_Table on ledgergroup_Table.Ledgergroup_sNo = Ledger_Table.Ledger_groupno and ledgergroup_Table.Comp_No = Ledger_Table.Comp_No  
    '                Where 
    '                    Vch_Table.Comp_No = @comp 
    '                    and Vch_Cancel = 'False' 
    '                    and Vch_Date <= @to 
    '                    And (Ledger_GNo IN (36, 201, 203, 205, 43, 208, 209)) 
    '                group by 
    '                    ledgergroup_name, area_name, Ledger_Name, ledger_Code, ledger_PName, ledger_Aliasname, ledger_serialpos, ledger_sno, Ledger_Table.comp_no  
    '                Order by Ledger_name"

    '            Dim cmd As New SqlCommand(query, conn)

    '            cmd.Parameters.AddWithValue("@to", DateTime.Parse(txtToDate.Text))
    '            cmd.Parameters.AddWithValue("@comp", compNo)

    '            Dim da As New SqlDataAdapter(cmd)
    '            Dim dt As New DataTable()
    '            da.Fill(dt)

    '            gvOutstanding.DataSource = dt
    '            gvOutstanding.DataBind()
    '        End Using
    '    Catch ex As Exception
    '        Response.Write("<script>console.log('SQL Error: " & ex.Message.Replace("'", "") & "');</script>")
    '    End Try
    'End Sub

    Protected Sub gvOutstanding_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim rawValue As Object = DataBinder.Eval(e.Row.DataItem, "TotalBalance")

            If rawValue IsNot DBNull.Value AndAlso rawValue IsNot Nothing Then
                Dim balance As Decimal = Convert.ToDecimal(rawValue)
                Dim formattedNumber As String = Math.Abs(balance).ToString("N2")

                Dim balanceCell As TableCell = e.Row.Cells(4)

                If balance > 0 Then
                    balanceCell.Text = formattedNumber & " Dr"
                    balanceCell.ForeColor = Drawing.Color.Black
                ElseIf balance < 0 Then
                    balanceCell.Text = formattedNumber & " Cr"
                    balanceCell.ForeColor = Drawing.Color.Red
                Else
                    balanceCell.Text = "0.00"
                    balanceCell.ForeColor = Drawing.Color.Gray
                End If
            End If
        End If
    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        BindReportData()
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Write("<script>window.top.location.href='MainForm.aspx';</script>")
    End Sub
End Class
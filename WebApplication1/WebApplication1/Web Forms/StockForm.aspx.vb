Imports System.Data
Imports System.Data.SqlClient

Public Class StockForm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1))
        Response.Cache.SetNoStore()

        UserGuard.ValidateSession(Me)

        If Session("UserName") Is Nothing OrElse Request.Cookies("SQL_Server") Is Nothing Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Not IsPostBack Then

            Dim fyEnd As String = If(Request.Cookies("Selected_PeriodTo") IsNot Nothing, Request.Cookies("Selected_PeriodTo").Value, DateTime.Now.ToString("yyyy-MM-dd"))

            Dim todayDate As DateTime = DateTime.Now
            Dim limitDate As DateTime = DateTime.Parse(fyEnd)

            If todayDate > limitDate Then
                txtToDate.Text = limitDate.ToString("yyyy-MM-dd")
            Else
                txtToDate.Text = todayDate.ToString("yyyy-MM-dd")
            End If

            txtToDate.Attributes("max") = fyEnd

            LoadFilterLists()
            BindStockData()
        End If
    End Sub

    Private Sub LoadFilterLists()
        Dim connStr As String = GetDBConn()
        Dim compNo As String = If(Request.Cookies("Selected_CompNo") IsNot Nothing, Request.Cookies("Selected_CompNo").Value, "ED1")

        Using conn As New SqlConnection(connStr)
            conn.Open()

            FillDDL(conn, "SELECT ItemGroup_Sno, ItemGroup_Name FROM ItemGroup_Table WHERE comp_no = @cp ORDER BY ItemGroup_Name", ddlGroup, "ItemGroup_Name", "ItemGroup_Sno", compNo)

            FillDDL(conn, "SELECT Brand_Sno, Brand_Name FROM Brand_Table WHERE comp_no = @cp ORDER BY Brand_Name", ddlBrand, "Brand_Name", "Brand_Sno", compNo)

            FillDDL(conn, "SELECT Model_Sno, Model_Name FROM Model_Table WHERE comp_no = @cp ORDER BY Model_Name", ddlModel, "Model_Name", "Model_Sno", compNo)

            FillDDL(conn, "SELECT Rack_Sno, Rack_Name FROM Rack_Table WHERE comp_no = @cp ORDER BY Rack_Name", ddlRack, "Rack_Name", "Rack_Sno", compNo)
        End Using
    End Sub

    Private Sub FillDDL(conn As SqlConnection, sql As String, ddl As DropDownList, textCol As String, valCol As String, compNo As String)
        Dim cmd As New SqlCommand(sql, conn)

        cmd.Parameters.AddWithValue("@cp", compNo)

        Dim dt As New DataTable()
        Using rdr As SqlDataReader = cmd.ExecuteReader()
            dt.Load(rdr)
        End Using

        ddl.DataSource = dt
        ddl.DataTextField = textCol
        ddl.DataValueField = valCol
        ddl.DataBind()

        ddl.Items.Insert(0, New ListItem("-- All --", "0"))
    End Sub

    Private Sub BindStockData()
        lblError.Text = ""
        Dim connStr As String = GetDBConn()
        Dim compNo As String = If(Request.Cookies("Selected_CompNo") IsNot Nothing, Request.Cookies("Selected_CompNo").Value, "ED1")

        Using conn As New SqlConnection(connStr)
            Dim whereClause As String = " WHERE Item_Table.comp_no = @compNo AND Item_Table.Item_Active = 'True' "

            If ddlGroup.SelectedValue <> "0" Then whereClause &= " AND Item_Table.Item_Groupno = @group "
            If ddlBrand.SelectedValue <> "0" Then whereClause &= " AND Item_Table.Brand_No = @brand "
            If ddlModel.SelectedValue <> "0" Then whereClause &= " AND Item_Table.Model_No = @model "
            If ddlRack.SelectedValue <> "0" Then whereClause &= " AND Item_Table.Rack_No = @rack "

            Dim query As String = "
                WITH cte AS (
                    SELECT 
                        Item_Table.Item_Name, ItemGroup_Table.ItemGroup_Name, Item_Table.Item_Srate, 
                        Item_Table.Item_Mrp, Model_Table.Model_Name, Brand_Table.Brand_Name, 
                        Rack_Table.Rack_Name, Item_Table.item_mtname, Item_Table.Item_mtCode,
                        ISNULL(SUM(CASE WHEN stocktran_type IN (0,2,3,7,12,21,38,39,73,89,10,35) THEN stocktran_Qty ELSE stocktran_Qty * -1 END),0) AS 'Qty',
                        ISNULL(SUM(CASE WHEN stocktran_type IN (0,2,3,7,12,21,38,39,73,89,10,35) THEN stocktran_pcs ELSE stocktran_pcs * -1 END),0) AS 'Pcs'
                    FROM Item_Table 
                    LEFT JOIN StockTran_Table ON Item_Table.Item_Sno = StockTran_Table.StockTran_ItemSno AND (stocktran_date <= @to OR stocktran_date IS NULL)
                    INNER JOIN Model_Table ON Model_Table.Model_Sno = Item_Table.Model_No  
                    INNER JOIN Brand_Table ON Brand_Table.Brand_Sno = Item_Table.Brand_No  
                    INNER JOIN Rack_Table ON Rack_Table.Rack_Sno = Item_Table.Rack_No  
                    INNER JOIN ItemGroup_Table ON ItemGroup_Table.ItemGroup_Sno = Item_Table.Item_Groupno " &
                    whereClause & "
                    GROUP BY 
                        Item_Table.Item_Name, ItemGroup_Table.ItemGroup_Name, Item_Table.Item_Srate, Item_Table.Item_Mrp, 
                        Model_Table.Model_Name, Brand_Table.Brand_Name, Rack_Table.Rack_Name, Item_Table.item_mtname, Item_Table.Item_mtCode
                ) 
                SELECT * FROM cte WHERE 1=1 "

            Select Case ddlStockType.SelectedValue
                Case "POS" : query &= " AND Qty > 0 "
                Case "ZERO" : query &= " AND Qty = 0 "
                Case "NEG" : query &= " AND Qty < 0 "
            End Select

            query &= " ORDER BY item_mtname, Item_mtCode"

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@to", DateTime.Parse(txtToDate.Text))
            cmd.Parameters.AddWithValue("@compNo", compNo)
            If ddlGroup.SelectedValue <> "0" Then cmd.Parameters.AddWithValue("@group", ddlGroup.SelectedValue)
            If ddlBrand.SelectedValue <> "0" Then cmd.Parameters.AddWithValue("@brand", ddlBrand.SelectedValue)
            If ddlModel.SelectedValue <> "0" Then cmd.Parameters.AddWithValue("@model", ddlModel.SelectedValue)
            If ddlRack.SelectedValue <> "0" Then cmd.Parameters.AddWithValue("@rack", ddlRack.SelectedValue)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()

            Try
                conn.Open()
                da.Fill(dt)
                gvStock.DataSource = dt
                gvStock.DataBind()

                If dt.Rows.Count > 0 Then
                    litCount.Text = dt.Rows.Count.ToString()
                    litTotalQty.Text = Convert.ToDecimal(If(IsDBNull(dt.Compute("SUM(Qty)", "")), 0, dt.Compute("SUM(Qty)", ""))).ToString("N2")
                    litTotalPcs.Text = Convert.ToDecimal(If(IsDBNull(dt.Compute("SUM(Pcs)", "")), 0, dt.Compute("SUM(Pcs)", ""))).ToString("N0")
                Else
                    litCount.Text = "0" : litTotalQty.Text = "0.00" : litTotalPcs.Text = "0"
                End If
            Catch ex As Exception
                lblError.Text = "SQL Error: " & ex.Message
            End Try
        End Using
    End Sub

    Private Function GetDBConn() As String
        Dim s = Server.UrlDecode(Request.Cookies("SQL_Server").Value).Replace("\\", "\")
        Dim d = Request.Cookies("SQL_DB").Value
        Dim u = Request.Cookies("SQL_User").Value
        Dim p = Server.UrlDecode(Request.Cookies("SQL_Pass").Value)
        Return Tools.GetConnectionString(s, d, u, p)
    End Function

    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        BindStockData()
    End Sub

    Protected Sub btnExit_Click(sender As Object, e As EventArgs) Handles btnExit.Click
        Response.Write("<script>window.top.location.href='MainForm.aspx';</script>")
    End Sub
End Class
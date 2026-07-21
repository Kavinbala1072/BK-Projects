Imports System.Data
Imports System.Data.SqlClient

Public Class ItemDetails
    Inherits System.Web.UI.Page

    Dim compNo As String = "ED1"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1))
        Response.Cache.SetNoStore()

        If Session("UserName") Is Nothing OrElse Request.Cookies("SQL_Server") Is Nothing Then
            Response.Redirect("Default.aspx")
            Exit Sub
        End If

        If Request.Cookies("Selected_CompNo") IsNot Nothing Then
            compNo = Request.Cookies("Selected_CompNo").Value
        End If

        If Not IsPostBack Then
            LoadItemList()
        End If
    End Sub

    Private Sub LoadItemList()
        Using conn As New SqlConnection(GetDBConn())
            Dim sql As String = "SELECT Item_SelTable.Name, Item_SelTable.Code, " &
                                "Cast(Item_SelTable.Item_no As Varchar(20)) + ';' + Cast(Item_SelTable.Comp_no as Varchar(20)) AS Val " &
                                "FROM Item_SelTable " &
                                "INNER JOIN Item_table ON Item_SelTable.Item_no = Item_table.Item_sno AND Item_SelTable.Comp_no = Item_table.Comp_no " &
                                "WHERE Item_SelTable.Comp_No = @cp " &
                                "Group by Item_SelTable.Name, Item_SelTable.Code, Item_SelTable.Item_no, Item_SelTable.Comp_no " &
                                "Order by Item_SelTable.Name"

            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@cp", compNo)

            Try
                conn.Open()
                lstItems.DataSource = cmd.ExecuteReader()
                lstItems.DataTextField = "Name"
                lstItems.DataValueField = "Val"
                lstItems.DataBind()
            Catch ex As Exception
                lblStatus.Text = "List Error: " & ex.Message
            End Try
        End Using
    End Sub

    Protected Sub lstItems_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstItems.SelectedIndexChanged
        If lstItems.SelectedItem IsNot Nothing Then
            txtSelectedItem.Text = lstItems.SelectedItem.Text
            Dim val As String = lstItems.SelectedValue
            Dim parts As String() = val.Split(";"c)
            LoadItemDetails(parts(0), parts(1))
        End If
    End Sub

    Private Sub LoadItemDetails(itemSno As String, itemComp As String)
        Using conn As New SqlConnection(GetDBConn())
            ' Note: Query already selects a.* which includes the rate fields
            Dim sql As String = "Select a.*, ISNULL(Brand_table.Brand_Name,'') AS Brand_Name, " &
                "ISNULL(ItemSize_Table.ItemSize_Name,'') AS ItemSize_Name, ISNULL(Model_table.Model_Name,'') AS Model_Name, " &
                "ISNULL(ItemGroup_table.ItemGroup_Name,'') AS ItemGroup_Name, ISNULL(Rack_table.Rack_Name,'') AS Rack_Name, " &
                "isnull(U1.Unit_Name,'') as UnitName, IsNull(U2.Unit_Name,'') as PurUnitName, IsNull(U3.Unit_Name,'') as PurUnitName2, " &
                "ISNULL(Margin_table.Margin_name,'') AS Margin_name, ISNULL(Tax_table.Tax_name,'') AS Tax_name, " &
                "IsNull(Currency_Table.Currency_Name,'') as CurrencyName, isnull(comp_name,'') as Company, " &
                "isnull(G.Tax_Name,'') as GstName, IsNull(User_table.User_Name,'ADMIN') as User_Name, " &
                "IsNull((Select SUM(Stocktran_Pcs) from StockTran_Table Where StockTran_Type = 0 And StockTran_ItemSno = a.Item_Sno And Comp_No = a.Comp_No), 0) As OpnPcs, " &
                "IsNull((Select SUM(Stocktran_Qty) from StockTran_Table Where StockTran_Type = 0 And StockTran_ItemSno = a.Item_Sno And Comp_No = a.Comp_No), 0) As OpnQty, " &
                "IsNull((Select SUM(Stocktran_Qty*stocktran_factor) from StockTran_Table Where StockTran_ItemSno = a.Item_Sno And stocktran_cancel='false' And Comp_No = a.Comp_No), 0) as ClosQty " &
                "from Item_Table a " &
                "LEFT JOIN ItemGroup_table ON a.item_Groupno = ItemGroup_table.Itemgroup_Sno and a.Comp_no=ItemGroup_table.Comp_no " &
                "LEFT JOIN Brand_table ON a.Brand_no = Brand_table.Brand_Sno and a.Comp_no=Brand_table.Comp_no " &
                "LEFT JOIN Model_table ON a.Model_no = Model_table.Model_Sno and a.Comp_no=Model_table.Comp_no " &
                "LEFT JOIN ItemSize_Table on a.Item_SizeNo = ItemSize_Table.ItemSize_SNo and a.Comp_no = ItemSize_Table.Comp_no " &
                "LEFT JOIN unit_table U1 ON a.Unit_no = U1.unit_Sno and a.Comp_no=U1.Comp_no " &
                "LEFT JOIN unit_table U2 ON a.PurUnit = U2.unit_Sno and a.Comp_no=U2.Comp_no " &
                "LEFT JOIN unit_table U3 ON a.PurUnit2 = U3.unit_Sno and a.Comp_no=U3.Comp_no " &
                "LEFT JOIN Margin_table ON a.Margin_no = Margin_table.Margin_Sno and a.Comp_no=Margin_table.Comp_no " &
                "LEFT JOIN Tax_table ON a.Tax_no = Tax_table.Tax_Sno and a.Comp_no=Tax_table.Comp_no " &
                "LEFT JOIN Tax_table as G ON a.Gst_no = G.Tax_Sno and a.Comp_no=G.Comp_no " &
                "LEFT JOIN Rack_table ON a.Rack_no = Rack_table.Rack_Sno and a.Comp_no=Rack_table.Comp_no " &
                "LEFT JOIN Currency_table ON a.CurrencyNo = Currency_Table.Currency_Sno and a.Comp_No = Currency_Table.Comp_No " &
                "LEFT JOIN Company_Table ON a.Comp_no = Company_table.Comp_Sno " &
                "Left Join User_table On a.User_no = User_table.User_Sno " &
                "Where a.Comp_no = @cp AND a.Item_Sno = @sno"

            Dim cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@cp", itemComp)
            cmd.Parameters.AddWithValue("@sno", itemSno)

            Try
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader()
                If dr.Read() Then
                    litItemName.Text = dr("Item_Name").ToString()
                    litItemCode.Text = dr("Item_Code").ToString()
                    litGroup.Text = dr("ItemGroup_Name").ToString()
                    litBrand.Text = dr("Brand_Name").ToString()
                    litModel.Text = dr("Model_Name").ToString()
                    litSize.Text = dr("ItemSize_Name").ToString()
                    litRack.Text = dr("Rack_Name").ToString()
                    litBaseUnit.Text = dr("UnitName").ToString()
                    litMargin.Text = dr("Margin_name").ToString()
                    litGst.Text = dr("GstName").ToString()
                    litCompany.Text = dr("Company").ToString()

                    ' ASSIGNING THE RATE FIELDS
                    litPRate.Text = Convert.ToDecimal(If(IsDBNull(dr("Item_PRate")), 0, dr("Item_PRate"))).ToString("N2")
                    litCost.Text = Convert.ToDecimal(If(IsDBNull(dr("Item_cost")), 0, dr("Item_cost"))).ToString("N2")
                    litMRP.Text = Convert.ToDecimal(If(IsDBNull(dr("Item_mrp")), 0, dr("Item_mrp"))).ToString("N2")
                    litSRate.Text = Convert.ToDecimal(If(IsDBNull(dr("Item_srate")), 0, dr("Item_srate"))).ToString("N2")

                    litOpnQty.Text = Convert.ToDecimal(dr("OpnQty")).ToString("N2")
                    litOpnPcs.Text = Convert.ToDecimal(dr("OpnPcs")).ToString("N0")
                    litClosQty.Text = Convert.ToDecimal(dr("ClosQty")).ToString("N2")

                    pnlDetails.Visible = True
                    pnlEmpty.Visible = False
                End If
            Catch ex As Exception
                lblStatus.Text = "Detail Error: " & ex.Message
            End Try
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
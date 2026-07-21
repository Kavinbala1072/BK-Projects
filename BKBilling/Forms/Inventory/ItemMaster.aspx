<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemMaster.aspx.cs" Inherits="BKBilling.Forms.Master.ItemMaster" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Advanced Item Master | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body { background: #f8fafc; background-image: radial-gradient(#cbd5e1 0.7px, transparent 0.7px); background-size: 24px 24px; min-height: 100vh; padding: 20px; font-family: 'Inter', sans-serif; }
        .card-custom { background: white; border-radius: 16px; border: 1px solid #e2e8f0; box-shadow: 0 10px 15px -3px rgba(0,0,0,0.1); margin: 0 auto;}
        .form-header { padding: 25px 40px; border-bottom: 1px solid #f1f5f9; }
        .section-title { font-size: 0.75rem; font-weight: 800; color: #6366f1; text-transform: uppercase; letter-spacing: 1px; border-bottom: 1px solid #f1f5f9; padding-bottom: 8px; margin-bottom: 15px; display: flex; align-items: center; }
        .form-label { font-weight: 600; font-size: 0.78rem; color: #475569; }
        .form-control-sm, .form-select-sm { border-radius: 8px; border: 1px solid #cbd5e1; padding: 8px 12px; }
        .gv-style th { background: #f8fafc; color: #64748b; font-size: 0.75rem; text-transform: uppercase; padding: 12px; border-bottom: 1px solid #e2e8f0; }
        .gv-style td { vertical-align: middle; padding: 12px; border-bottom: 1px solid #f1f5f9; }
        .bg-setup { background-color: #f8fafc; border-radius: 12px; padding: 20px; border: 1px solid #e2e8f0; }
        .img-preview { width: 100px; height: 100px; object-fit: cover; border-radius: 12px; border: 2px solid #e2e8f0; background: #fff; }
        .modern-switch { display: flex; align-items: center; background: #f8fafc; padding: 10px 15px 10px 45px; border-radius: 12px; border: 1px solid #e2e8f0; position: relative; min-height: 45px; width: 100%; transition: all 0.2s; }
        .modern-switch span { position: absolute; left: 10px; display: flex; align-items: center; }
        .modern-switch .form-check-input { width: 2.5em !important; height: 1.25em !important; margin: 0 !important; cursor: pointer; appearance: none; -webkit-appearance: none; background-color: #cbd5e1; border: none !important; }
        .modern-switch .form-check-input:checked { background-color: #10b981 !important; }
        .modern-switch label { font-weight: 600; color: #334155; cursor: pointer; font-size: 0.85rem; margin: 0 0 0 5px; }
    </style>
    <script>
        function showNotification(message, type) {
            var toastEl = document.getElementById('msgToast');
            document.getElementById('msgText').innerText = message;
            toastEl.classList.remove('bg-danger', 'bg-success');
            toastEl.classList.add(type === 'error' ? 'bg-danger' : 'bg-success');
            new bootstrap.Toast(toastEl).show();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <asp:HiddenField ID="hfItemID" runat="server" />

        <div class="container-fluid">
            <!-- VIEW 1: LIST -->
            <asp:Panel ID="pnlList" runat="server">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h4 class="fw-bold m-0"><i class="fas fa-boxes-stacked me-2 text-primary"></i>Product Directory</h4>
                        <div class="d-flex gap-3">
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" placeholder="Search..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" style="width:250px;"></asp:TextBox>
                            <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn btn-primary px-4 fw-bold shadow-sm" OnClick="btnOpenCreate_Click">+ New Item</asp:LinkButton>
                        </div>
                    </div>
                    <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="false" CssClass="table gv-style" GridLines="None" OnRowCommand="gvItems_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Item_Type" HeaderText="Type" ItemStyle-Font-Size="Smaller" />
                            <asp:BoundField DataField="Item_Code" HeaderText="Code" />
                            <asp:BoundField DataField="Item_Name" HeaderText="Item Name" ItemStyle-CssClass="fw-bold" />
                            <asp:BoundField DataField="ItemGroup_Name" HeaderText="Category" />
                            <asp:BoundField DataField="Unit_Sname" HeaderText="Unit" />
                            <asp:BoundField DataField="Selling_Price" HeaderText="Price" DataFormatString="{0:N2}" />
                            <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-end">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="EditRecord" CommandArgument='<%# Eval("Item_Sno") %>' CssClass="btn btn-sm btn-outline-primary"><i class="fas fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>

            <!-- VIEW 2: FORM -->
            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4 border-bottom pb-3">
                        <div>
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="text-decoration-none small text-muted fw-bold" OnClick="btnBack_Click"><i class="fas fa-arrow-left me-1"></i> BACK</asp:LinkButton>
                            <h3 class="fw-bold m-0 mt-1 text-dark">Item Configuration</h3>
                        </div>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success px-5 fw-bold shadow-sm" OnClick="btnSave_Click">SAVE ITEM</asp:LinkButton>
                    </div>

                    <div class="row g-4">
                        <!-- Col 1 -->
                        <div class="col-md-4 border-end">
                            <p class="section-title"><i class="fas fa-barcode"></i> 1. Identification</p>
                            
                            <div class="mb-3">
                                <label class="form-label">Item Type *</label>
                                <asp:DropDownList ID="ddlItemType" runat="server" CssClass="form-select form-select-sm fw-bold border-primary">
                                    <asp:ListItem Value="Item">Item</asp:ListItem>
                                    <asp:ListItem Value="Material">Material</asp:ListItem>
                                </asp:DropDownList>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Product Name *</label>
                                <asp:TextBox ID="txtItemName" runat="server" CssClass="form-control form-control-sm" placeholder="Enter name"></asp:TextBox>
                            </div>
                            
                            <div class="row g-2 mb-3">
                                <div class="col-6"><label class="form-label">Item Code</label><asp:TextBox ID="txtItemCode" runat="server" CssClass="form-control form-control-sm"></asp:TextBox></div>
                                <div class="col-6"><label class="form-label">HSN Code</label><asp:TextBox ID="txtHSN" runat="server" CssClass="form-control form-control-sm"></asp:TextBox></div>
                            </div>
                            
                            <div class="mb-3"><label class="form-label">Barcode / EAN</label><asp:TextBox ID="txtBarcode" runat="server" CssClass="form-control form-control-sm"></asp:TextBox></div>
                            <div class="mb-3"><label class="form-label">Main Category</label><asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></div>
                            <div class="mb-3"><label class="form-label">Sub-Category</label><asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></div>
                        </div>

                        <!-- Col 2 -->
                        <div class="col-md-4 border-end">
                            <p class="section-title"><i class="fas fa-gears"></i> 2. Attributes & Logic</p>
                            <div class="row g-2 mb-3">
                                <div class="col-6"><label class="form-label">Color</label><asp:DropDownList ID="ddlColor" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></div>
                                <div class="col-6"><label class="form-label">Weave Type</label><asp:DropDownList ID="ddlWeave" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></div>
                            </div>
                            
                            <div class="bg-setup mb-3">
                                <div class="row g-2">
                                    <div class="col-6"><div class="modern-switch form-check form-switch"><asp:CheckBox ID="chkBatch" runat="server" CssClass="form-check-input" /><label>Batch Wise</label></div></div>
                                    <div class="col-6"><div class="modern-switch form-check form-switch"><asp:CheckBox ID="chkSerial" runat="server" CssClass="form-check-input" /><label>Serial Track</label></div></div>
                                </div>
                            </div>

                            <div class="bg-setup mb-3">
                                <div class="row g-2 mb-2">
                                    <div class="col-6"><label class="form-label">Primary Unit</label><asp:DropDownList ID="ddlBaseUnit" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></div>
                                    <div class="col-6"><label class="form-label">Alt Unit</label><asp:DropDownList ID="ddlAltUnit" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></div>
                                </div>
                                <label class="form-label">Conv. Factor</label>
                                <asp:TextBox ID="txtConvFactor" runat="server" CssClass="form-control form-control-sm" Text="1.00"></asp:TextBox>
                            </div>

                            <div class="mt-4"><label class="form-label">Item Image</label>
                                <div class="d-flex gap-3 align-items-center bg-light p-2 rounded">
                                    <asp:FileUpload ID="fuImage" runat="server" CssClass="form-control form-control-sm" />
                                    <asp:Image ID="imgPrev" runat="server" CssClass="img-preview" ImageUrl="~/Images/no-image.png" />
                                </div>
                            </div>
                        </div>

                        <!-- Col 3 -->
                        <div class="col-md-4">
                            <p class="section-title"><i class="fas fa-chart-line"></i> 3. Stock & Pricing</p>
                            <div class="row g-2 mb-3">
                                <div class="col-6"><label class="form-label">Min Stock</label><asp:TextBox ID="txtMinStock" runat="server" CssClass="form-control form-control-sm" Text="0"></asp:TextBox></div>
                                <div class="col-6"><label class="form-label">Max Stock</label><asp:TextBox ID="txtMaxStock" runat="server" CssClass="form-control form-control-sm" Text="0"></asp:TextBox></div>
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-6"><label class="form-label text-primary">Selling Price</label><asp:TextBox ID="txtSalesPrice" runat="server" CssClass="form-control form-control-sm fw-bold" Text="0.00"></asp:TextBox></div>
                                <div class="col-6"><label class="form-label text-success">Cost Rate</label><asp:TextBox ID="txtPurRate" runat="server" CssClass="form-control form-control-sm fw-bold" Text="0.00"></asp:TextBox></div>
                            </div>
                            <div class="mb-3"><label class="form-label">GST Tax Rule</label><asp:DropDownList ID="ddlGST" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></div>
                            <div class="bg-setup mb-4">
                                <label class="form-label">Opening Quantity</label>
                                <asp:TextBox ID="txtOpQty" runat="server" CssClass="form-control form-control-sm" Text="0"></asp:TextBox>
                            </div>
                            <div class="modern-switch form-check form-switch">
                                <asp:CheckBox ID="chkActive" runat="server" CssClass="form-check-input" Checked="true" />
                                <label class="form-check-label ms-2" for="<%= chkActive.ClientID %>">Item Active Status</label>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3">
                <div id="msgToast" class="toast align-items-center text-white border-0 shadow-lg" role="alert">
                    <div class="d-flex"><div class="toast-body"><i id="msgIcon"></i> <span id="msgText"></span></div><button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button></div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
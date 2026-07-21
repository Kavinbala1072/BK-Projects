<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PurchaseForm.aspx.cs" Inherits="BKBilling.Forms.Transaction.PurchaseForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Invoice | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <script>
        function showNotification(message, type) {
            var toastEl = document.getElementById('msgToast');
            if (!toastEl) return;
            document.getElementById('msgText').innerText = message;
            toastEl.classList.remove('bg-danger', 'bg-success', 'bg-primary');
            if (type === 'error') toastEl.classList.add('bg-danger');
            else if (type === 'success') toastEl.classList.add('bg-success');
            else toastEl.classList.add('bg-primary');
            new bootstrap.Toast(toastEl, { delay: 3000 }).show();
        }
    </script>
    <style>
        body { background-color: #f1f5f9; padding: 15px; font-family: 'Inter', sans-serif; }
        .card-custom { background: white; border-radius: 12px; border: none; box-shadow: 0 4px 12px rgba(0,0,0,0.05); }
        .section-title { font-size: 0.8rem; font-weight: 700; color: #dc2626; text-transform: uppercase; letter-spacing: 1px; margin-bottom: 15px; }
        .form-label { font-weight: 600; font-size: 0.75rem; color: #475569; margin-bottom: 2px; }
        .table-items thead { background: #475569; color: white; font-size: 0.75rem; }
        .total-box { background: #fef2f2; border-radius: 8px; padding: 15px; border: 1px solid #fee2e2; }
        .grand-total { font-size: 1.5rem; font-weight: 800; color: #dc2626; }
        .supplier-info-box { background: #f8fafc; border-radius: 8px; padding: 10px; border: 1px solid #e2e8f0; min-height: 85px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <asp:HiddenField ID="hfPurchaseID" runat="server" />

        <div class="container-fluid">
            <!-- VIEW 1: PURCHASE REGISTER -->
            <asp:Panel ID="pnlList" runat="server">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h4 class="fw-bold m-0"><i class="fas fa-shopping-cart me-2 text-danger"></i>Purchase Register</h4>
                        <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn btn-danger px-4 fw-bold shadow-sm" OnClick="btnOpenCreate_Click">
                            <i class="fas fa-plus me-1"></i> New Purchase Entry
                        </asp:LinkButton>
                    </div>
                    <asp:GridView ID="gvPurchase" runat="server" AutoGenerateColumns="false" CssClass="table table-hover align-middle gv-style" 
                        GridLines="None" DataKeyNames="Purchase_Sno" OnRowCommand="gvPurchase_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Purchase_No" HeaderText="Pur #" />
                            <asp:BoundField DataField="Bill_Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
                            <asp:BoundField DataField="Supplier_Name" HeaderText="Supplier" />
                            <asp:BoundField DataField="GSTIN" HeaderText="GSTIN" />
                            <asp:BoundField DataField="Grand_Total" HeaderText="Total Amount" DataFormatString="{0:N2}" />
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditRecord" CommandArgument='<%# Eval("Purchase_Sno") %>' CssClass="btn btn-sm btn-outline-secondary"><i class="fas fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>

            <!-- VIEW 2: PURCHASE FORM -->
            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-3 border-bottom pb-2">
                        <asp:LinkButton ID="btnBack" runat="server" CssClass="text-decoration-none fw-bold text-muted" OnClick="btnBack_Click"><i class="fas fa-arrow-left"></i> BACK</asp:LinkButton>
                        <h4 class="fw-bold m-0">Purchase Entry</h4>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-danger px-5 fw-bold" OnClick="btnSave_Click"><i class="fas fa-save me-2"></i>SAVE PURCHASE</asp:LinkButton>
                    </div>

                    <!-- HEADER: SUPPLIER DETAILS -->
                    <div class="row g-3 mb-4">
                        <div class="col-md-2">
                            <label class="form-label">Purchase No</label>
                            <asp:TextBox ID="txtPurNo" runat="server" CssClass="form-control form-control-sm" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="form-label">Purchase Date</label>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">Supplier Selection *</label>
                            <asp:DropDownList ID="ddlSupplier" runat="server" CssClass="form-select form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddlSupplier_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label text-muted">Supplier Address & GST</label>
                            <div class="supplier-info-box small">
                                <asp:Literal ID="litSupplierDetail" runat="server" Text="Select a supplier to view details..."></asp:Literal>
                            </div>
                        </div>
                    </div>

                    <!-- ITEMS TABLE -->
                    <p class="section-title">Purchase Item Details</p>
                    <div class="table-responsive mb-3">
                        <asp:UpdatePanel ID="upItems" runat="server">
                            <ContentTemplate>
                                <table class="table table-bordered table-items align-middle">
                                    <thead>
                                        <tr>
                                            <th style="width: 30%;">Product / Material</th>
                                            <th>HSN</th>
                                            <th>Qty</th>
                                            <th>Rate</th>
                                            <th>GST %</th>
                                            <th>Amount</th>
                                            <th>Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptItems" runat="server" OnItemCommand="rptItems_ItemCommand">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:DropDownList ID="ddlItem" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                                                    <td><asp:TextBox ID="txtHSN" runat="server" CssClass="form-control form-control-sm"></asp:TextBox></td>
                                                    <td><asp:TextBox ID="txtQty" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("Qty") %>'></asp:TextBox></td>
                                                    <td><asp:TextBox ID="txtRate" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("Rate") %>'></asp:TextBox></td>
                                                    <td><asp:TextBox ID="txtTax" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("Tax") %>'></asp:TextBox></td>
                                                    <td><asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount") %>' CssClass="fw-bold"></asp:Label></td>
                                                    <td><asp:LinkButton ID="btnRemove" runat="server" CommandName="Delete" CssClass="text-danger"><i class="fas fa-trash"></i></asp:LinkButton></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                                <asp:LinkButton ID="btnAddRow" runat="server" CssClass="btn btn-sm btn-outline-secondary fw-bold" OnClick="btnAddRow_Click"><i class="fas fa-plus me-1"></i> Add Row</asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <!-- SUMMARY -->
                    <div class="row">
                        <div class="col-md-7">
                            <label class="form-label">Purchase Remarks</label>
                            <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="Reference invoice no, truck details, etc."></asp:TextBox>
                        </div>
                        <div class="col-md-5">
                            <div class="total-box">
                                <div class="d-flex justify-content-between mb-2">
                                    <span>Taxable Value</span>
                                    <asp:Label ID="lblSubTotal" runat="server" Text="0.00" CssClass="fw-bold"></asp:Label>
                                </div>
                                <div class="d-flex justify-content-between mb-2">
                                    <span>Inward Freight</span>
                                    <asp:TextBox ID="txtFreight" runat="server" CssClass="form-control form-control-sm w-50 text-end" Text="0" AutoPostBack="true" OnTextChanged="CalculateTotal"></asp:TextBox>
                                </div>
                                <div class="d-flex justify-content-between mb-2">
                                    <span>Purchase Disc (-)</span>
                                    <asp:TextBox ID="txtDisc" runat="server" CssClass="form-control form-control-sm w-50 text-end" Text="0" AutoPostBack="true" OnTextChanged="CalculateTotal"></asp:TextBox>
                                </div>
                                <div class="d-flex justify-content-between mb-2 text-danger fw-bold border-top pt-2">
                                    <span>Input Tax (GST)</span>
                                    <asp:Label ID="lblTaxTotal" runat="server" Text="0.00"></asp:Label>
                                </div>
                                <div class="d-flex justify-content-between border-top pt-3 mt-2">
                                    <span class="h5 fw-bold">Grand Total</span>
                                    <asp:Label ID="lblGrandTotal" runat="server" Text="0.00" CssClass="grand-total"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3">
                <div id="msgToast" class="toast align-items-center text-white border-0 shadow-lg" role="alert">
                    <div class="d-flex">
                        <div class="toast-body"><i id="msgIcon"></i> <span id="msgText"></span></div>
                        <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SalesReturnForm.aspx.cs" Inherits="BKBilling.Forms.Transaction.SalesReturnForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sales Return | BK Softwares</title>
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
        .section-title { font-size: 0.8rem; font-weight: 700; color: #6366f1; text-transform: uppercase; letter-spacing: 1px; margin-bottom: 15px; }
        .form-label { font-weight: 600; font-size: 0.75rem; color: #475569; margin-bottom: 2px; }
        .table-items thead { background: #6366f1; color: white; font-size: 0.75rem; }
        .total-box { background: #f5f3ff; border-radius: 8px; padding: 15px; border: 1px solid #ddd6fe; }
        .grand-total { font-size: 1.5rem; font-weight: 800; color: #4338ca; }
        .info-box { background: #f8fafc; border-radius: 8px; padding: 10px; border: 1px solid #e2e8f0; min-height: 85px; font-size: 0.85rem; }
        .gv-style th { background: #f8fafc; color: #64748b; font-size: 0.75rem; text-transform: uppercase; padding: 12px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <asp:HiddenField ID="hfReturnID" runat="server" />

        <div class="container-fluid">
            <!-- VIEW 1: LIST -->
            <asp:Panel ID="pnlList" runat="server">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h4 class="fw-bold m-0 text-dark"><i class="fas fa-undo me-2 text-primary"></i>Sales Return Register</h4>
                        <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn btn-primary px-4 fw-bold shadow-sm" OnClick="btnOpenCreate_Click">
                            <i class="fas fa-plus me-1"></i> New Return Entry
                        </asp:LinkButton>
                    </div>
                    <asp:GridView ID="gvReturns" runat="server" AutoGenerateColumns="false" CssClass="table table-hover align-middle gv-style" 
                        GridLines="None" DataKeyNames="Return_Sno" OnRowCommand="gvReturns_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Return_No" HeaderText="Return #" />
                            <asp:BoundField DataField="Return_Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
                            <asp:BoundField DataField="Ref_Sales_No" HeaderText="Sales Ref #" />
                            <asp:BoundField DataField="Customer_Name" HeaderText="Customer" />
                            <asp:BoundField DataField="Grand_Total" HeaderText="Total Amount" DataFormatString="{0:N2}" />
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="EditRecord" CommandArgument='<%# Eval("Return_Sno") %>' CssClass="btn btn-sm btn-outline-secondary"><i class="fas fa-edit"></i></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>

            <!-- VIEW 2: FORM -->
            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-3 border-bottom pb-2">
                        <asp:LinkButton ID="btnBack" runat="server" CssClass="text-decoration-none fw-bold text-muted" OnClick="btnBack_Click"><i class="fas fa-arrow-left"></i> BACK</asp:LinkButton>
                        <h4 class="fw-bold m-0">Sales Return / Credit Note</h4>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success px-5 fw-bold shadow-sm" OnClick="btnSave_Click"><i class="fas fa-check-circle me-2"></i>SAVE RETURN</asp:LinkButton>
                    </div>

                    <div class="row g-3 mb-4">
                        <div class="col-md-2">
                            <label class="form-label">Return No</label>
                            <asp:TextBox ID="txtReturnNo" runat="server" CssClass="form-control form-control-sm" placeholder="Auto"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="form-label">Return Date</label>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label text-primary fw-bold">Link to Sales Invoice #</label>
                            <asp:DropDownList ID="ddlSalesRef" runat="server" CssClass="form-select form-select-sm border-primary" AutoPostBack="true" OnSelectedIndexChanged="ddlSalesRef_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-5">
                            <label class="form-label">Customer Detail</label>
                            <div class="info-box small">
                                <asp:Literal ID="litCustomerDetail" runat="server" Text="Select Sales Invoice to load data..."></asp:Literal>
                            </div>
                        </div>
                    </div>

                    <p class="section-title">Returning Items</p>
                    <asp:UpdatePanel ID="upItems" runat="server">
                        <ContentTemplate>
                            <div class="table-responsive mb-3">
                                <table class="table table-bordered table-items align-middle">
                                    <thead>
                                        <tr>
                                            <th style="width: 35%;">Item Description</th>
                                            <th>Qty Returned</th>
                                            <th>Original Rate</th>
                                            <th>GST %</th>
                                            <th>Credit Amount</th>
                                            <th>#</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater ID="rptItems" runat="server" OnItemCommand="rptItems_ItemCommand">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><asp:DropDownList ID="ddlItem" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                                                    <td><asp:TextBox ID="txtQty" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("Qty") %>' AutoPostBack="true" OnTextChanged="RecalculateRow"></asp:TextBox></td>
                                                    <td><asp:TextBox ID="txtRate" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("Rate") %>' AutoPostBack="true" OnTextChanged="RecalculateRow"></asp:TextBox></td>
                                                    <td><asp:TextBox ID="txtTax" runat="server" CssClass="form-control form-control-sm" Text='<%# Eval("Tax") %>' AutoPostBack="true" OnTextChanged="RecalculateRow"></asp:TextBox></td>
                                                    <td><asp:Label ID="lblAmount" runat="server" Text='<%# Eval("Amount", "{0:N2}") %>' CssClass="fw-bold"></asp:Label></td>
                                                    <td><asp:LinkButton ID="btnDel" runat="server" CommandName="Delete" CommandArgument='<%# Container.ItemIndex %>' CssClass="text-danger"><i class="fas fa-trash"></i></asp:LinkButton></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                                <asp:LinkButton ID="btnAddRow" runat="server" CssClass="btn btn-sm btn-outline-primary fw-bold" OnClick="btnAddRow_Click"><i class="fas fa-plus me-1"></i> Add Manual Row</asp:LinkButton>
                            </div>

                            <div class="row">
                                <div class="col-md-7">
                                    <label class="form-label">Reason for Return</label>
                                    <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="4" placeholder="Damaged goods, wrong item, etc."></asp:TextBox>
                                </div>
                                <div class="col-md-5">
                                    <div class="total-box">
                                        <div class="d-flex justify-content-between mb-2"><span>Taxable Value</span><asp:Label ID="lblSubTotal" runat="server" Text="0.00" CssClass="fw-bold"></asp:Label></div>
                                        <div class="d-flex justify-content-between mb-2"><span>Adj. Charges</span><asp:TextBox ID="txtFreight" runat="server" CssClass="form-control form-control-sm w-50 text-end" Text="0" AutoPostBack="true" OnTextChanged="RecalculateRow"></asp:TextBox></div>
                                        <div class="d-flex justify-content-between mb-2 text-primary fw-bold border-top pt-2"><span>Tax (GST) Credit</span><asp:Label ID="lblTaxTotal" runat="server" Text="0.00"></asp:Label></div>
                                        <div class="d-flex justify-content-between border-top pt-3 mt-2"><span class="h5 fw-bold">Grand Total Credit</span><asp:Label ID="lblGrandTotal" runat="server" Text="0.00" CssClass="grand-total"></asp:Label></div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
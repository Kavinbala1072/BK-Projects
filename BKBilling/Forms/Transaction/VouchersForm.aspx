<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VouchersForm.aspx.cs" Inherits="BKBilling.Forms.Transaction.VouchersForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Voucher Entry | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <script>
        function showNotification(message, type) {
            var toastEl = document.getElementById('msgToast');
            if (!toastEl) return;
            document.getElementById('msgText').innerText = message;
            toastEl.classList.remove('bg-danger', 'bg-success', 'bg-primary', 'bg-info', 'bg-warning');
            if (type === 'error') toastEl.classList.add('bg-danger');
            else if (type === 'success') toastEl.classList.add('bg-success');
            else if (type === 'warning') toastEl.classList.add('bg-warning');
            else toastEl.classList.add('bg-primary');
            new bootstrap.Toast(toastEl, { delay: 3000 }).show();
        }
    </script>
    <style>
        body { background-color: #f1f5f9; padding: 15px; font-family: 'Inter', sans-serif; }
        .card-custom { background: white; border-radius: 12px; border: none; box-shadow: 0 4px 12px rgba(0,0,0,0.05); }
        .section-title { font-size: 0.8rem; font-weight: 700; color: #475569; text-transform: uppercase; letter-spacing: 1px; margin-bottom: 15px; }
        .form-label { font-weight: 600; font-size: 0.75rem; color: #475569; margin-bottom: 2px; }
        .form-control-sm, .form-select-sm { border-radius: 4px; font-size: 0.85rem; }
        
        /* Dynamic Header Colors */
        .type-header { border-left: 5px solid #64748b; padding-left: 15px; }
        .receipt-theme { border-left-color: #10b981; color: #10b981; }
        .payment-theme { border-left-color: #ef4444; color: #ef4444; }
        .contra-theme { border-left-color: #3b82f6; color: #3b82f6; }
        .journal-theme { border-left-color: #6366f1; color: #6366f1; }
        
        .entry-box { background: #f8fafc; border: 1px solid #e2e8f0; border-radius: 8px; padding: 20px; }
        .gv-style th { background: #f8fafc; color: #64748b; font-size: 0.75rem; text-transform: uppercase; padding: 12px; }
        .badge-type { font-size: 0.7rem; padding: 4px 8px; border-radius: 4px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <asp:HiddenField ID="hfVoucherID" runat="server" />

        <div class="container-fluid">
            <!-- VIEW 1: VOUCHER LIST -->
            <asp:Panel ID="pnlList" runat="server">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h4 class="fw-bold m-0"><i class="fas fa-book me-2 text-secondary"></i>Day Book / Vouchers</h4>
                        <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn btn-dark px-4 fw-bold shadow-sm" OnClick="btnOpenCreate_Click">
                            <i class="fas fa-plus me-1"></i> New Voucher Entry
                        </asp:LinkButton>
                    </div>
                    <asp:GridView ID="gvVouchers" runat="server" AutoGenerateColumns="false" CssClass="table table-hover align-middle gv-style" 
                        GridLines="None" DataKeyNames="Voucher_Sno" OnRowCommand="gvVouchers_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Voucher_No" HeaderText="Vch #" />
                            <asp:BoundField DataField="Vch_Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" />
                            <asp:TemplateField HeaderText="Type">
                                <ItemTemplate>
                                    <span class='badge bg-secondary badge-type'><%# Eval("Vch_Type") %></span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Dr_Ledger" HeaderText="Particulars (Dr)" />
                            <asp:BoundField DataField="Cr_Ledger" HeaderText="Particulars (Cr)" />
                            <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:N2}" />
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="EditRecord" CommandArgument='<%# Eval("Voucher_Sno") %>' CssClass="btn btn-sm btn-outline-secondary"><i class="fas fa-edit"></i></asp:LinkButton>
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
                        <h4 class="fw-bold m-0 type-header" id="vchHeader" runat="server">Voucher Entry</h4>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success px-5 fw-bold shadow-sm" OnClick="btnSave_Click"><i class="fas fa-check-circle me-2"></i>POST VOUCHER</asp:LinkButton>
                    </div>

                    <div class="row g-3 mb-4">
                        <div class="col-md-3">
                            <label class="form-label">Voucher Type *</label>
                            <asp:DropDownList ID="ddlVchType" runat="server" CssClass="form-select form-select-sm fw-bold border-dark" AutoPostBack="true" OnSelectedIndexChanged="ddlVchType_SelectedIndexChanged">
                                <asp:ListItem Text="Receipt" Value="Receipt" />
                                <asp:ListItem Text="Payment" Value="Payment" />
                                <asp:ListItem Text="Contra" Value="Contra" />
                                <asp:ListItem Text="Journal" Value="Journal" />
                                <asp:ListItem Text="Credit Note" Value="Cr Note" />
                                <asp:ListItem Text="Debit Note" Value="Dr Note" />
                                <asp:ListItem Text="Others" Value="Others" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Voucher No</label>
                            <asp:TextBox ID="txtVchNo" runat="server" CssClass="form-control form-control-sm" placeholder="Auto"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Date</label>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Reference (Chq/DD/Inv)</label>
                            <asp:TextBox ID="txtRef" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                    </div>

                    <div class="entry-box mb-4">
                        <div class="row g-4">
                            <!-- DEBIT SIDE -->
                            <div class="col-md-6 border-end">
                                <p class="section-title text-primary"><i class="fas fa-arrow-right me-2"></i>Debit Side (By)</p>
                                <div class="mb-3">
                                    <label class="form-label">Select Account (Dr)</label>
                                    <asp:DropDownList ID="ddlDrLedger" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                                <div class="mb-0">
                                    <label class="form-label">Debit Amount</label>
                                    <asp:TextBox ID="txtDrAmount" runat="server" CssClass="form-control form-control-sm fw-bold" placeholder="0.00"></asp:TextBox>
                                </div>
                            </div>

                            <!-- CREDIT SIDE -->
                            <div class="col-md-6">
                                <p class="section-title text-danger"><i class="fas fa-arrow-left me-2"></i>Credit Side (To)</p>
                                <div class="mb-3">
                                    <label class="form-label">Select Account (Cr)</label>
                                    <asp:DropDownList ID="ddlCrLedger" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                                <div class="mb-0">
                                    <label class="form-label">Credit Amount</label>
                                    <asp:TextBox ID="txtCrAmount" runat="server" CssClass="form-control form-control-sm fw-bold" placeholder="0.00"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <label class="form-label">Narration / Being...</label>
                            <asp:TextBox ID="txtNarration" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="3" placeholder="Enter transaction details here..."></asp:TextBox>
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
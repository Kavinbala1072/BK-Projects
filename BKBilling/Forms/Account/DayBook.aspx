<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DayBook.aspx.cs" Inherits="BKBilling.Forms.Account.DayBook" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DayBook | BK Softwares</title>
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
        function printDiv() {
            window.print();
        }
    </script>
    <style>
        body { background-color: #f1f5f9; padding: 15px; font-family: 'Inter', sans-serif; }
        .card-custom { background: white; border-radius: 12px; border: none; box-shadow: 0 4px 12px rgba(0,0,0,0.05); }
        .filter-section { background: #f8fafc; border-bottom: 1px solid #e2e8f0; padding: 15px; border-radius: 12px 12px 0 0; }
        .form-label { font-weight: 600; font-size: 0.75rem; color: #475569; margin-bottom: 2px; }
        .gv-style { font-size: 0.85rem; }
        .gv-style th { background: #334155; color: white; font-weight: 600; text-transform: uppercase; font-size: 0.75rem; padding: 12px; }
        .gv-style td { padding: 10px; border-bottom: 1px solid #f1f5f9; }
        .text-debit { color: #059669; font-weight: 600; }
        .text-credit { color: #dc2626; font-weight: 600; }
        .footer-totals { background: #f1f5f9; font-weight: 800; font-size: 0.9rem; }
        
        @media print {
            .no-print { display: none !important; }
            body { padding: 0; background: white; }
            .card-custom { box-shadow: none; border: none; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        
        <div class="container-fluid">
            <div class="card-custom">
                <!-- FILTER BAR -->
                <div class="filter-section no-print">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h4 class="fw-bold m-0"><i class="fas fa-book-open me-2 text-slate"></i>Day Book Report</h4>
                        <div>
                            <button type="button" class="btn btn-outline-secondary btn-sm me-2" onclick="printDiv()">
                                <i class="fas fa-print me-1"></i> Print
                            </button>
                            <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-outline-success btn-sm" OnClick="btnExport_Click">
                                <i class="fas fa-file-excel me-1"></i> Export
                            </asp:LinkButton>
                        </div>
                    </div>
                    
                    <div class="row g-3 align-items-end">
                        <div class="col-md-2">
                            <label class="form-label">From Date</label>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="form-label">To Date</label>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Voucher Type</label>
                            <asp:DropDownList ID="ddlVchType" runat="server" CssClass="form-select form-select-sm">
                                <asp:ListItem Text="All Transactions" Value="All" />
                                <asp:ListItem Text="Sales" Value="Sales" />
                                <asp:ListItem Text="Purchase" Value="Purchase" />
                                <asp:ListItem Text="Receipt" Value="Receipt" />
                                <asp:ListItem Text="Payment" Value="Payment" />
                                <asp:ListItem Text="Contra" Value="Contra" />
                                <asp:ListItem Text="Journal" Value="Journal" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label">Search Ledger/Particulars</label>
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" placeholder="Search..."></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <asp:LinkButton ID="btnView" runat="server" CssClass="btn btn-primary btn-sm w-100 fw-bold" OnClick="btnView_Click">
                                <i class="fas fa-search me-1"></i> View Report
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>

                <!-- REPORT TABLE -->
                <div class="p-4">
                    <div class="table-responsive">
                        <asp:GridView ID="gvDayBook" runat="server" AutoGenerateColumns="false" CssClass="table table-hover gv-style" 
                            GridLines="None" ShowFooter="true" OnRowDataBound="gvDayBook_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-Width="120px" />
                                <asp:BoundField DataField="Vch_No" HeaderText="Vch No" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="Vch_Type" HeaderText="Type" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="Particulars" HeaderText="Particulars (Account Name)" />
                                <asp:TemplateField HeaderText="Debit (In)" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDebit" runat="server" Text='<%# Eval("Debit", "{0:N2}") %>' CssClass='<%# Convert.ToDecimal(Eval("Debit")) > 0 ? "text-debit" : "" %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Credit (Out)" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCredit" runat="server" Text='<%# Eval("Credit", "{0:N2}") %>' CssClass='<%# Convert.ToDecimal(Eval("Credit")) > 0 ? "text-credit" : "" %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center py-5 text-muted">No transactions found for the selected period.</div>
                            </EmptyDataTemplate>
                            <FooterStyle CssClass="footer-totals" />
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- Toast NOTIFICATION -->
            <div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3 no-print">
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
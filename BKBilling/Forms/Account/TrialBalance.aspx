<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TrialBalance.aspx.cs" Inherits="BKBilling.Forms.Account.TrialBalance" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Trial Balance | BK Softwares</title>
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
        .report-header { background: #1e293b; color: white; padding: 20px; border-radius: 12px 12px 0 0; }
        .form-label { font-weight: 700; font-size: 0.75rem; color: #cbd5e1; text-transform: uppercase; }
        
        .summary-box { padding: 15px; border-radius: 10px; background: #f8fafc; border: 1px solid #e2e8f0; }
        .summary-val { font-size: 1.25rem; font-weight: 800; }
        
        .gv-style { font-size: 0.85rem; }
        .gv-style th { background: #f8fafc; color: #475569; text-transform: uppercase; font-size: 0.75rem; padding: 12px; border-bottom: 2px solid #e2e8f0; }
        .gv-style td { padding: 10px; border-bottom: 1px solid #f1f5f9; }
        
        .row-group { background-color: #f8fafc; font-weight: 700; color: #4f46e5; }
        .diff-warning { background-color: #fef2f2; border: 1px solid #fee2e2; color: #dc2626; border-radius: 8px; padding: 10px; display: none; }
        
        @media print { .no-print { display: none !important; } }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />

        <div class="container-fluid">
            <div class="card-custom">
                <!-- HEADER & FILTERS -->
                <div class="report-header no-print">
                    <div class="row align-items-center">
                        <div class="col-md-6">
                            <h4 class="fw-bold m-0"><i class="fas fa-balance-scale me-2 text-info"></i>Trial Balance Report</h4>
                        </div>
                        <div class="col-md-6 text-end">
                            <div class="d-inline-flex align-items-center">
                                <div class="text-start me-3">
                                    <label class="form-label mb-0">As of Date</label>
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control form-control-sm bg-dark text-white border-secondary" TextMode="Date"></asp:TextBox>
                                </div>
                                <asp:LinkButton ID="btnView" runat="server" CssClass="btn btn-info btn-sm fw-bold mt-3" OnClick="btnView_Click">
                                    <i class="fas fa-sync-alt me-1"></i> Generate
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- VALIDATION ALERT -->
                <div id="divDiff" runat="server" class="m-3 diff-warning">
                    <i class="fas fa-exclamation-circle me-2"></i>
                    <strong>Out of Balance!</strong> There is a difference of <asp:Label ID="lblDiff" runat="server"></asp:Label> between total Debits and Credits.
                </div>

                <!-- SUMMARY CARDS -->
                <div class="p-3 border-bottom no-print">
                    <div class="row g-3">
                        <div class="col-md-4">
                            <div class="summary-box">
                                <small class="text-muted fw-bold text-uppercase">Total Debit</small>
                                <div class="summary-val text-success"><asp:Label ID="lblSumDr" runat="server" Text="0.00"></asp:Label></div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="summary-box">
                                <small class="text-muted fw-bold text-uppercase">Total Credit</small>
                                <div class="summary-val text-danger"><asp:Label ID="lblSumCr" runat="server" Text="0.00"></asp:Label></div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="summary-box">
                                <small class="text-muted fw-bold text-uppercase">Export Report</small>
                                <div class="mt-1">
                                    <button type="button" class="btn btn-sm btn-outline-secondary" onclick="window.print()"><i class="fas fa-print"></i> PDF</button>
                                    <asp:LinkButton ID="btnExcel" runat="server" CssClass="btn btn-sm btn-outline-success ms-2"><i class="fas fa-file-excel"></i> Excel</asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- REPORT DATA -->
                <div class="p-4">
                    <div class="table-responsive">
                        <asp:GridView ID="gvTrialBalance" runat="server" AutoGenerateColumns="false" CssClass="table gv-style" 
                            GridLines="None" ShowHeaderWhenEmpty="true">
                            <Columns>
                                <asp:BoundField DataField="LedgerName" HeaderText="Particulars (Account Name)" />
                                <asp:BoundField DataField="GroupName" HeaderText="Account Group" ItemStyle-CssClass="text-muted" />
                                <asp:TemplateField HeaderText="Debit Amount" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Debit", "{0:N2}") %>' 
                                            CssClass='<%# Convert.ToDecimal(Eval("Debit")) > 0 ? "fw-bold text-dark" : "text-muted" %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Credit Amount" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# Eval("Credit", "{0:N2}") %>' 
                                            CssClass='<%# Convert.ToDecimal(Eval("Credit")) > 0 ? "fw-bold text-dark" : "text-muted" %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center py-5">No ledger balances found for this date.</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- Toast UI -->
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
</body>
</html>
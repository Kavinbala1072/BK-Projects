<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LedgerView.aspx.cs" Inherits="BKBilling.Forms.Account.LedgerView" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ledger View | BK Softwares</title>
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
        .filter-section { background: #ffffff; border-bottom: 2px solid #f1f5f9; padding: 20px; border-radius: 12px 12px 0 0; }
        .form-label { font-weight: 700; font-size: 0.75rem; color: #64748b; text-transform: uppercase; margin-bottom: 5px; }
        
        /* Summary Cards */
        .summary-card { padding: 15px; border-radius: 10px; border: 1px solid #e2e8f0; background: #f8fafc; }
        .summary-label { font-size: 0.7rem; font-weight: 700; color: #64748b; display: block; margin-bottom: 2px; }
        .summary-value { font-size: 1.1rem; font-weight: 800; color: #1e293b; }

        .gv-style { font-size: 0.85rem; }
        .gv-style th { background: #1e293b; color: white; text-transform: uppercase; font-size: 0.75rem; padding: 12px; }
        .gv-style td { padding: 10px; border-bottom: 1px solid #f1f5f9; }
        
        .text-dr { color: #059669; font-weight: 600; }
        .text-cr { color: #dc2626; font-weight: 600; }
        .running-bal { background: #f1f5f9; font-weight: 700; }

        @media print { .no-print { display: none !important; } body { background: white; padding: 0; } }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />

        <div class="container-fluid">
            <div class="card-custom">
                <!-- SELECTION & FILTERS -->
                <div class="filter-section no-print">
                    <div class="d-flex justify-content-between align-items-center mb-3">
                        <h4 class="fw-bold m-0"><i class="fas fa-file-invoice-dollar me-2 text-primary"></i>Account Statement</h4>
                        <div>
                            <button type="button" class="btn btn-outline-dark btn-sm me-2" onclick="window.print()"><i class="fas fa-print me-1"></i> Print</button>
                            <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-outline-success btn-sm"><i class="fas fa-file-excel me-1"></i> Excel</asp:LinkButton>
                        </div>
                    </div>
                    
                    <div class="row g-3">
                        <div class="col-md-4">
                            <label class="form-label">Search Ledger Account</label>
                            <asp:DropDownList ID="ddlLedger" runat="server" CssClass="form-select form-select-sm select2" AutoPostBack="true" OnSelectedIndexChanged="btnView_Click"></asp:DropDownList>
                        </div>
                        <div class="col-md-2">
                            <label class="form-label">From Date</label>
                            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="form-label">To Date</label>
                            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="form-label">&nbsp;</label>
                            <asp:LinkButton ID="btnView" runat="server" CssClass="btn btn-primary btn-sm w-100 fw-bold" OnClick="btnView_Click">
                                <i class="fas fa-sync me-1"></i> Refresh
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>

                <!-- SUMMARY DASHBOARD -->
                <div class="p-3 border-bottom bg-light">
                    <div class="row g-3">
                        <div class="col-md-3">
                            <div class="summary-card">
                                <span class="summary-label">Opening Balance</span>
                                <asp:Label ID="lblOpeningBal" runat="server" CssClass="summary-value" Text="0.00"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="summary-card">
                                <span class="summary-label">Total Debit (+)</span>
                                <asp:Label ID="lblTotalDr" runat="server" CssClass="summary-value text-dr" Text="0.00"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="summary-card">
                                <span class="summary-label">Total Credit (-)</span>
                                <asp:Label ID="lblTotalCr" runat="server" CssClass="summary-value text-cr" Text="0.00"></asp:Label>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="summary-card border-primary bg-primary-subtle">
                                <span class="summary-label text-primary">Closing Balance</span>
                                <asp:Label ID="lblClosingBal" runat="server" CssClass="summary-value text-primary" Text="0.00"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- TRANSACTION GRID -->
                <div class="p-4">
                    <div class="table-responsive">
                        <asp:GridView ID="gvLedgerRows" runat="server" AutoGenerateColumns="false" CssClass="table table-hover gv-style" 
                            GridLines="None" OnRowDataBound="gvLedgerRows_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="Vch_Date" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy}" ItemStyle-Width="120px" />
                                <asp:BoundField DataField="Vch_No" HeaderText="Vch No" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="Vch_Type" HeaderText="Type" ItemStyle-Width="100px" />
                                <asp:BoundField DataField="Narration" HeaderText="Narration / Description" />
                                <asp:TemplateField HeaderText="Debit" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDr" runat="server" Text='<%# Eval("Debit", "{0:N2}") %>' CssClass='<%# Convert.ToDecimal(Eval("Debit")) > 0 ? "text-dr" : "text-muted" %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Credit" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCr" runat="server" Text='<%# Eval("Credit", "{0:N2}") %>' CssClass='<%# Convert.ToDecimal(Eval("Credit")) > 0 ? "text-cr" : "text-muted" %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Balance" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="running-bal">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRunningBal" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center py-5 text-muted">Select a ledger and date range to view statement.</div>
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
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BalanceSheet.aspx.cs" Inherits="BKBilling.Forms.Account.BalanceSheet" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Balance Sheet | BK Softwares</title>
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
        .report-header { background: #0f172a; color: white; padding: 20px; border-radius: 12px 12px 0 0; }
        
        .bs-container { display: flex; border-top: 1px solid #e2e8f0; }
        .bs-side { flex: 1; padding: 0; }
        .bs-side:first-child { border-right: 2px solid #cbd5e1; }
        
        .side-title { background: #f8fafc; padding: 10px 15px; font-weight: 800; font-size: 0.85rem; color: #334155; text-transform: uppercase; border-bottom: 1px solid #e2e8f0; }
        
        .gv-style { font-size: 0.85rem; margin-bottom: 0; }
        .gv-style td { padding: 8px 15px; border-bottom: 1px solid #f1f5f9; }
        .gv-style .group-row { background-color: #f8fafc; font-weight: 700; color: #4f46e5; }
        
        .total-row { background: #1e293b; color: white; font-weight: 800; font-size: 1rem; }
        
        .summary-card { border-radius: 10px; padding: 12px; border: 1px solid #e2e8f0; }
        
        @media print { 
            .no-print { display: none !important; } 
            body { padding: 0; background: white; }
            .card-custom { box-shadow: none; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />

        <div class="container-fluid">
            <div class="card-custom">
                <!-- HEADER -->
                <div class="report-header no-print">
                    <div class="row align-items-center">
                        <div class="col-md-6">
                            <h4 class="fw-bold m-0"><i class="fas fa-chart-pie me-2"></i>Balance Sheet</h4>
                            <small class="text-slate-400">Financial Position Statement</small>
                        </div>
                        <div class="col-md-6 text-end">
                            <div class="d-inline-flex align-items-center gap-2">
                                <div class="text-start">
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control form-control-sm bg-dark text-white border-secondary" TextMode="Date"></asp:TextBox>
                                </div>
                                <asp:LinkButton ID="btnView" runat="server" CssClass="btn btn-primary btn-sm fw-bold" OnClick="btnView_Click">
                                    <i class="fas fa-sync me-1"></i> Generate
                                </asp:LinkButton>
                                <button type="button" class="btn btn-light btn-sm" onclick="window.print()"><i class="fas fa-print"></i></button>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- T-FORMAT CONTENT -->
                <div class="bs-container">
                    <!-- LEFT SIDE: LIABILITIES -->
                    <div class="bs-side">
                        <div class="side-title d-flex justify-content-between">
                            <span>Liabilities & Capital</span>
                            <span>Amount</span>
                        </div>
                        <asp:GridView ID="gvLiabilities" runat="server" AutoGenerateColumns="false" CssClass="table gv-style" GridLines="None" ShowHeader="false">
                            <Columns>
                                <asp:BoundField DataField="Particulars" ItemStyle-Width="70%" />
                                <asp:BoundField DataField="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="fw-bold" />
                            </Columns>
                        </asp:GridView>
                    </div>

                    <!-- RIGHT SIDE: ASSETS -->
                    <div class="bs-side">
                        <div class="side-title d-flex justify-content-between">
                            <span>Assets</span>
                            <span>Amount</span>
                        </div>
                        <asp:GridView ID="gvAssets" runat="server" AutoGenerateColumns="false" CssClass="table gv-style" GridLines="None" ShowHeader="false">
                            <Columns>
                                <asp:BoundField DataField="Particulars" ItemStyle-Width="70%" />
                                <asp:BoundField DataField="Amount" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="fw-bold" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

                <!-- FOOTER TOTALS -->
                <div class="row g-0 border-top">
                    <div class="col-6 bg-dark text-white p-3 d-flex justify-content-between align-items-center border-end border-secondary">
                        <span class="text-uppercase small fw-bold">Total Liabilities</span>
                        <asp:Label ID="lblTotalLiab" runat="server" CssClass="h5 m-0 fw-bold" Text="0.00"></asp:Label>
                    </div>
                    <div class="col-6 bg-dark text-white p-3 d-flex justify-content-between align-items-center">
                        <span class="text-uppercase small fw-bold">Total Assets</span>
                        <asp:Label ID="lblTotalAssets" runat="server" CssClass="h5 m-0 fw-bold" Text="0.00"></asp:Label>
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
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BalanceSheet.aspx.vb" Inherits="WebApplication1.BalanceSheet" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporting - Balance Sheet</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    
    <style>
        :root { --primary: #6366f1; --header-bg: #0f172a; --bg-light: #f8fafc; --border: #e2e8f0; }
        body, html { height: 100%; margin: 0; font-family: 'Inter', sans-serif; background-color: var(--bg-light); overflow: hidden; }

        #loader-wrapper {
            position: fixed; top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(255, 255, 255, 0.9); z-index: 9999; display: none;
            flex-direction: column; justify-content: center; align-items: center;
        }
        .loader-spinner { width: 40px; height: 40px; border: 3px solid #f3f3f3; border-top: 3px solid var(--primary); border-radius: 50%; animation: spin 0.8s linear infinite; }
        @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }

        .report-header { background: var(--header-bg); padding: 0 20px; color: #fff; font-size: 14px; display: flex; justify-content: space-between; height: 45px; align-items: center; }
        .main-container { display: flex; height: calc(100vh - 110px); }
        .table-area { display: flex; flex-direction: column; flex-grow: 1; overflow: hidden; background: #fff; }
        .grid-body-scroll { flex-grow: 1; overflow: auto; }

        .split-container { display: flex; min-width: 1000px; min-height: 100%; }
        .split-side { flex: 1; display: flex; flex-direction: column; border-right: 1px solid var(--border); }
        .side-title { background-color: #f1f5f9; padding: 10px 15px; font-weight: 800; font-size: 11px; text-transform: uppercase; border-bottom: 2px solid var(--border); position: sticky; top: 0; z-index: 20; }

        .report-table { width: 100%; border-collapse: collapse; font-size: 12px; }
        .report-table td { padding: 8px 15px; border-bottom: 1px solid #f8fafc; }
        .amt-col { width: 140px; text-align: right; font-weight: 600; }
        
        .row-group { font-weight: 700; color: #4f46e5; background-color: #fcfcfd; }
        .row-ledger { padding-left: 30px !important; color: #64748b; font-size: 11px; }
        .row-diff { font-weight: 700; color: #f43f5e !important; } /* Rose color for Diff */

        .total-row { background-color: #1e293b; color: #fff; font-weight: 800; margin-top: auto; padding: 12px 15px; }
        .right-sidebar { width: 300px; background: #fff; border-left: 1px solid var(--border); padding: 20px; }
        .footer-toolbar { background: #fff; padding: 10px 20px; border-top: 1px solid var(--border); display: flex; justify-content: space-between; align-items: center; height: 65px; }
        .btn-custom { padding: 8px 18px; font-size: 13px; font-weight: 600; border: 1px solid var(--border); background: #fff; color: var(--text-main); border-radius: 6px; text-decoration: none; display: inline-flex; align-items: center; gap: 8px; transition: all 0.2s; }
        .btn-custom:hover { background-color: #f1f5f9; color: var(--primary); }

        .btn-refresh { background-color: var(--primary); color: #fff !important; border: none; }
        .btn-refresh:hover { background-color: var(--primary-dark); }
    </style>

    <script>
        function showLoader() { document.getElementById('loader-wrapper').style.display = 'flex'; }
        window.onload = function () { document.getElementById('loader-wrapper').style.display = 'none'; };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="loader-wrapper"><div class="loader-spinner">LOADING DATA...</div></div>

        <div class="report-header">
            <span><i class="fas fa-balance-scale me-2"></i> Balance Sheet (Horizontal View)</span>
            <asp:Label ID="lblStatus" runat="server" Font-Size="11px" ForeColor="#fda4af"></asp:Label>
        </div>

        <div class="main-container">
            <div class="table-area">
                <div class="grid-body-scroll">
                    <div class="split-container">
                        <!-- LIABILITIES SIDE -->
                        <div class="split-side">
                            <div class="side-title d-flex justify-content-between"><span>Liabilities & Capital</span><span>Amount</span></div>
                            <table class="report-table">
                                <tbody><asp:Literal ID="litLiabilitiesRows" runat="server"></asp:Literal></tbody>
                            </table>
                            <div class="total-row d-flex justify-content-between">
                                <span>TOTAL LIABILITIES</span>
                                <span><asp:Literal ID="litTotalLiabilities" runat="server">0.00</asp:Literal></span>
                            </div>
                        </div>

                        <!-- ASSETS SIDE -->
                        <div class="split-side">
                            <div class="side-title d-flex justify-content-between"><span>Assets</span><span>Amount</span></div>
                            <table class="report-table">
                                <tbody><asp:Literal ID="litAssetsRows" runat="server"></asp:Literal></tbody>
                            </table>
                            <div class="total-row d-flex justify-content-between">
                                <span>TOTAL ASSETS</span>
                                <span><asp:Literal ID="litTotalAssets" runat="server">0.00</asp:Literal></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

<%--            <div class="right-sidebar">
                <h6 class="fw-bold small mb-3">REPORT OPTIONS</h6>
                <asp:RadioButtonList ID="rblDetail" runat="server" CssClass="small mb-3" AutoPostBack="true">
                    <asp:ListItem Text="&nbsp;Summary (Groups)" Value="S" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="&nbsp;Detailed (Ledgers)" Value="D"></asp:ListItem>
                </asp:RadioButtonList>
                <hr />
                <div class="alert alert-info py-2 small">
                    <i class="fas fa-info-circle me-1"></i> Opening differences and Current Net Profit are auto-calculated.
                </div>
            </div>--%>
        </div>

        <div class="footer-toolbar">
            <div class="d-flex align-items-center gap-2">
                <span class="small fw-bold text-muted">AS ON DATE:</span>
                <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:160px;"></asp:TextBox>
                <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn-custom btn-refresh ms-2" OnClick="btnRefresh_Click" OnClientClick="showLoader();">
                    <i class="fas fa-sync-alt"></i> <span>Refresh</span>
                </asp:LinkButton>
            </div>
            <asp:LinkButton ID="LinkButton3" runat="server" CssClass="btn-custom border-danger text-danger" OnClick="btnExit_Click"><i class="fas fa-sign-out-alt"></i>Exit</asp:LinkButton>
        </div>
    </form>
</body>
</html>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProfitandlossForm.aspx.vb" Inherits="WebApplication1.ProfitandlossForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Profit & Loss - Standard View</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    
    <style>
        :root { --primary: #6366f1; --header-bg: #0f172a; --border: #e2e8f0; }
        body, html { height: 100%; margin: 0; font-family: 'Inter', sans-serif; background-color: #f8fafc; overflow: hidden; }

        .report-header { background: var(--header-bg); padding: 0 20px; color: #fff; font-size: 14px; height: 45px; display: flex; align-items: center; justify-content: space-between; }
        .main-container { display: flex; height: calc(100vh - 110px); }
        .table-area { display: flex; flex-direction: column; flex-grow: 1; background: #fff; overflow: hidden; }
        .grid-body-scroll { flex-grow: 1; overflow: auto; }

        /* PAIRING TABLE STYLE */
        .report-table { width: 100%; border-collapse: collapse; font-size: 12px; min-width: 1100px; table-layout: fixed; }
        .report-table th { 
            background-color: #f1f5f9; position: sticky; top: 0; z-index: 20; 
            padding: 10px 15px; font-weight: 800; border-bottom: 2px solid var(--border); text-transform: uppercase;
        }
        .report-table td { padding: 6px 15px; border-bottom: 1px solid #f1f5f9; vertical-align: middle; height: 32px; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

        /* Column Widths */
        .col-name { width: 35%; }
        .col-amt { width: 15%; text-align: right; font-weight: 600; }
        .border-mid { border-right: 2px solid var(--border) !important; }

        /* Row Styling */
        .row-group { font-weight: 700; color: #4338ca; }
        .row-ledger { padding-left: 30px !important; color: #64748b; }
        .row-balancing { font-weight: 800; color: #10b981; }
        .row-loss { font-weight: 800; color: #ef4444; }

        /* Footer */
        .total-row { background-color: #1e293b; color: #fff; font-weight: 800; padding: 12px 15px; display: flex; min-width: 1100px; }
        .footer-toolbar { background: #fff; padding: 10px 20px; border-top: 1px solid var(--border); display: flex; justify-content: space-between; height: 65px; align-items: center; }
        
        .right-sidebar { width: 300px; background: #fff; border-left: 1px solid var(--border); padding: 20px; }
        .btn-refresh { background-color: var(--primary); color: #fff !important; font-weight: 600; border: none; }
        .btn-custom { padding: 8px 18px; font-size: 13px; font-weight: 600; border: 1px solid var(--border); background: #fff; color: var(--text-main); border-radius: 6px; text-decoration: none; display: inline-flex; align-items: center; gap: 8px; transition: all 0.2s; }
        .btn-custom:hover { background-color: #f1f5f9; color: var(--primary); }

         .btn-refresh { background-color: var(--primary); color: #fff !important; border: none; }
        .btn-refresh:hover { background-color: var(--primary-dark); }

                #loader-wrapper {
            position: fixed; top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(255, 255, 255, 0.9); z-index: 9999;
            display: flex; flex-direction: column; justify-content: center; align-items: center;
        }
        .loader-spinner {
            width: 45px; height: 45px;
            border: 4px solid #f3f3f3; border-top: 4px solid var(--accent);
            border-radius: 50%; animation: spin 0.8s linear infinite;
        }
        .loader-text { margin-top: 15px; font-weight: 600; color: var(--primary-dark1); font-size: 0.85rem; letter-spacing: 0.5px; }
        @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }

    </style>

     <script type="text/javascript">
        function showLoader() {
            document.getElementById('loader-wrapper').style.display = 'flex';
        }
        window.onload = function () {
            document.getElementById('loader-wrapper').style.display = 'none';
        };
     </script>
</head>
<body>
    <form id="form1" runat="server">

        <div id="loader-wrapper">
            <div class="loader-spinner"></div>
            <div class="loader-text">LOADING DATA...</div>
        </div>

        <div class="report-header">
            <span><i class="fas fa-file-invoice-dollar me-2"></i> Profit & Loss Account</span>
            <asp:Label ID="lblStatus" runat="server" Font-Size="11px"></asp:Label>
        </div>

        <div class="main-container">
            <div class="table-area">
                <div class="grid-body-scroll">
                    <table class="report-table">
                        <thead>
                            <tr>
                                <th class="col-name">Particulars (Expenses)</th>
                                <th class="col-amt border-mid">Amount</th>
                                <th class="col-name">Particulars (Income)</th>
                                <th class="col-amt">Amount</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Literal ID="litPLRows" runat="server"></asp:Literal>
                        </tbody>
                    </table>
                </div>

                <div class="total-row">
                    <div class="col-name ps-3">TOTAL</div>
                    <div class="col-amt border-mid pe-3"><asp:Literal ID="litTotalDebit" runat="server">0.00</asp:Literal></div>
                    <div class="col-name ps-3">TOTAL</div>
                    <div class="col-amt pe-3"><asp:Literal ID="litTotalCredit" runat="server">0.00</asp:Literal></div>
                </div>
            </div>

<%--            <div class="right-sidebar">
                <h6 class="fw-bold small mb-3">DISPLAY OPTIONS</h6>
                <asp:DropDownList ID="ddlLevel" runat="server" CssClass="form-select form-select-sm mb-3" AutoPostBack="true">
                    <asp:ListItem Text="Detailed (Include Ledgers)" Value="D" Selected="True"></asp:ListItem>
                    <asp:ListItem Text="Summary (Groups Only)" Value="G"></asp:ListItem>
                </asp:DropDownList>
                <hr />
                <div class="alert alert-light border small text-muted">
                    Trading section is calculated first to determine Gross Profit.
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
            <asp:LinkButton ID="btnExit" runat="server" CssClass="btn-custom border-danger text-danger" OnClick="btnExit_Click"><i class="fas fa-sign-out-alt"></i>Exit</asp:LinkButton>
        </div>
    </form>
</body>
</html>
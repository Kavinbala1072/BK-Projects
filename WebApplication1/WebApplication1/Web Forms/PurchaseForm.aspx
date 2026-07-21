<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PurchaseForm.aspx.vb" Inherits="WebApplication1.PurchaseForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporting - Purchase Summary</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    
    <style>
        :root {
            --primary-bg: #f8fafc;
            --header-dark: #0f172a;
            --accent-blue: #6366f1;
            --border-color: #e2e8f0;
            --summary-bg: #eff6ff;
            --text-main: #1e293b;
            --text-muted: #64748b;
        }

        body, html { 
            height: 100%; margin: 0; 
            font-family: 'Inter', sans-serif; 
            background-color: var(--primary-bg); 
            color: var(--text-main);
            overflow: hidden; 
        }
        
        #loader-wrapper {
            position: fixed; top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(255, 255, 255, 0.9); z-index: 9999;
            display: flex; flex-direction: column; justify-content: center; align-items: center;
        }
        .loader-spinner {
            width: 45px; height: 45px;
            border: 4px solid #f3f3f3; border-top: 4px solid var(--accent-blue);
            border-radius: 50%; animation: spin 0.8s linear infinite;
        }
        @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }
        .loader-text { margin-top: 15px; font-weight: 600; color: var(--header-dark); font-size: 0.9rem; }

        .main-wrapper { display: flex; flex-direction: column; height: 100vh; }

        .report-header { 
            background: var(--header-dark); padding: 10px 20px; 
            font-size: 0.9rem; font-weight: 600; color: #fff; 
            height: 45px; display: flex; align-items: center;
        }

        .grid-content-scroll { 
            flex-grow: 1; 
            overflow: auto; 
            background-color: #fff; 
        }

        .scroll-width-wrapper {
            min-width: 1100px; /* Prevents squishing */
            display: flex;
            flex-direction: column;
            min-height: 100%;
        }

        .report-table { width: 100%; border-collapse: collapse; font-size: 13px; table-layout: fixed; }
        
        .report-table th { 
            background-color: #f1f5f9; 
            border-bottom: 2px solid var(--border-color); 
            padding: 12px 10px; 
            position: sticky; top: 0; 
            text-align: left; color: #475569; 
            font-weight: 700; text-transform: uppercase; font-size: 11px;
            z-index: 40; 
        }

        .report-table td { border-bottom: 1px solid var(--border-color); padding: 10px; white-space: nowrap; }
        .report-table tr:hover { background-color: #f8fafc; }

        .col-bill { width: 90px; }
        .col-date { width: 110px; }
        .col-name { width: auto; min-width: 250px; }
        .col-qty { width: 120px; text-align: right; }
        .col-gross { width: 140px; text-align: right; }
        .col-net { width: 150px; text-align: right; font-weight: 700; color: var(--accent-blue); }

        /* STICKY FOOTER TOTALS */
        .summary-sticky-container { 
            position: sticky; bottom: 0; 
            background-color: var(--summary-bg); 
            border-top: 2px solid #bfdbfe; 
            z-index: 50;
            margin-top: auto;
        }

        .summary-row { font-weight: 700; color: #1e40af; }
        .summary-row td { padding: 12px 10px; border: none; }

        /* FOOTER TOOLBAR */
        .footer-toolbar { 
            flex-shrink: 0; background-color: #fff; 
            padding: 10px 20px; border-top: 1px solid var(--border-color); 
            display: flex; justify-content: space-between; align-items: center; 
            min-height: 65px;
        }

        /* UI BUTTONS */
        .btn-custom { 
            padding: 8px 18px; font-size: 13px; font-weight: 600;
            border: 1px solid var(--border-color); border-radius: 6px;
            background: #fff; color: var(--text-main);
            transition: all 0.2s; display: inline-flex; align-items: center; gap: 8px;
            text-decoration: none;
        }
        .btn-custom:hover { background: #f1f5f9; color: var(--accent-blue); border-color: #cbd5e1; }
        .btn-refresh { background-color: var(--accent-blue) !important; color: #fff !important; border: none; }

        .date-input { width: 150px; height: 38px; border-radius: 6px; border: 1px solid var(--border-color); font-size: 13px; padding: 5px 10px; }

        /* --- RESPONSIVE DESIGN (SMALL SCREEN) --- */
        @media (max-width: 992px) {
            body, html { overflow: auto; }
            .main-wrapper { height: auto; min-height: 100vh; }
            .grid-content-scroll { height: 450px; }
            
            .footer-toolbar { flex-direction: column; padding: 20px; height: auto; gap: 15px; }
            .footer-toolbar .d-flex { flex-direction: column; width: 100%; gap: 12px; }
            
            .date-input-group { display: flex; width: 100%; justify-content: space-between; gap: 5px; }
            .date-input { width: 48% !important; flex-grow: 1; }
            
            .btn-full { width: 100%; justify-content: center; height: 45px; }
            .report-header { justify-content: center; }
        }
    </style>

    <script type="text/javascript">
        function showLoader() { document.getElementById('loader-wrapper').style.display = 'flex'; }
        window.onload = function () { document.getElementById('loader-wrapper').style.display = 'none'; };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        
        <div id="loader-wrapper">
            <div class="loader-spinner"></div>
            <div class="loader-text">FETCHING PURCHASE DATA...</div>
        </div>

        <div class="main-wrapper">
            <!-- HEADER -->
            <div class="report-header">
                <i class="fas fa-shopping-cart me-2 text-info"></i> Purchase Detailed Report
            </div>

            <!-- SCROLLABLE TABLE AREA -->
            <div class="grid-content-scroll">
                <div class="scroll-width-wrapper">
                    <asp:GridView ID="gvPurchase" runat="server" AutoGenerateColumns="False" 
                        CssClass="report-table" GridLines="None" ShowHeader="True">
                        <Columns>
                            <asp:BoundField DataField="Pmas_VchNo" HeaderText="BILL NO" HeaderStyle-CssClass="col-bill" ItemStyle-CssClass="col-bill" />
                            <asp:BoundField DataField="Pmas_BillNo" HeaderText="MANUAL BILL NO" HeaderStyle-CssClass="col-bill" ItemStyle-CssClass="col-bill" />
                            <asp:BoundField DataField="Pmas_BillDate" HeaderText="DATE" DataFormatString="{0:dd-MM-yyyy}" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                            <asp:BoundField DataField="ledger_name" HeaderText="VENDOR NAME" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                            <asp:BoundField DataField="purQty" HeaderText="QTY" HeaderStyle-CssClass="col-qty" ItemStyle-CssClass="col-qty" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="Pmas_gross" HeaderText="GROSS" HeaderStyle-CssClass="col-gross" ItemStyle-CssClass="col-gross" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="Pmas_NetAmount" HeaderText="NET AMOUNT" HeaderStyle-CssClass="col-net" ItemStyle-CssClass="col-net" DataFormatString="{0:N2}" />
                        </Columns>
                    </asp:GridView>

                    <!-- FIXED BOTTOM SUMMARY (Aligns with horizontal scroll) -->
                    <div class="summary-sticky-container">
                        <table class="report-table">
                            <tr class="summary-row">
                                <td colspan="3" class="text-uppercase" style="padding-left: 20px;">
                                    TOTAL BILLS: <span class="badge bg-primary ms-2"><asp:Literal ID="litBillCount" runat="server">0</asp:Literal></span>
                                </td>
                                <td class="col-qty"><asp:Literal ID="litQtyTotal" runat="server">0.00</asp:Literal></td>
                                <td class="col-gross"><asp:Literal ID="litGrossTotal" runat="server">0.00</asp:Literal></td>
                                <td class="col-net"><asp:Literal ID="litAmountTotal" runat="server">0.00</asp:Literal></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>

            <!-- FOOTER TOOLBAR -->
            <div class="footer-toolbar">
                <div class="d-flex align-items-center">
                    <div class="date-input-group">
                        <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" CssClass="date-input"></asp:TextBox>
                        <span class="small text-muted">to</span>
                        <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="date-input"></asp:TextBox>
                    </div>
                    
                    <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn-custom btn-refresh btn-full ms-lg-3" 
                        OnClick="btnRefresh_Click" OnClientClick="showLoader();">
                        <i class="fas fa-sync-alt"></i> Refresh
                    </asp:LinkButton>
                </div>
                
                <div class="d-flex btn-full">
                    <asp:LinkButton ID="btnExit" runat="server" CssClass="btn-custom text-danger border-danger btn-full" OnClick="btnExit_Click">
                        <i class="fas fa-sign-out-alt"></i> Exit
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
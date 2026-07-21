<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TrialBalance.aspx.vb" Inherits="WebApplication1.TrialBalance" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporting - Trial Balance</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    
    <style>
        :root {
            --primary: #6366f1;
            --primary-dark: #4f46e5;
            --sidebar-bg: #1e293b;
            --header-bg: #0f172a;
            --bg-light: #f8fafc;
            --border: #e2e8f0;
            --text-main: #1e293b;
            --text-muted: #64748b;
        }

        body, html { 
            height: 100%; margin: 0; 
            font-family: 'Inter', sans-serif; 
            background-color: var(--bg-light); 
            color: var(--text-main);
            overflow: hidden; 
        }

        /* Modern Loader */
        #loader-wrapper {
            position: fixed; top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(255, 255, 255, 0.9); z-index: 9999; 
            display: flex; flex-direction: column; justify-content: center; align-items: center;
        }
        .loader-spinner {
            width: 40px; height: 40px;
            border: 3px solid #f3f3f3; border-top: 3px solid var(--primary);
            border-radius: 50%; animation: spin 0.8s linear infinite;
        }
        @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }

        .report-header { 
            background: var(--header-bg); padding: 0 20px; 
            color: #fff; font-size: 14px; font-weight: 600; 
            display: flex; justify-content: space-between; height: 45px; align-items: center; 
        }
        
        .info-bar { 
            background-color: #fff; padding: 0 20px; 
            font-size: 13px; border-bottom: 1px solid var(--border); 
            display: flex; align-items: center; height: 55px; 
        }

        .main-container { display: flex; height: calc(100vh - 165px); flex-direction: row; }
        
        .table-area { 
            display: flex; flex-direction: column; flex-grow: 1; 
            overflow: hidden; background: #fff; 
        }

        .grid-body-scroll { 
            flex-grow: 1; 
            overflow: auto; 
            background-color: #fff; 
            position: relative;
        }

        .scroll-content-wrapper {
            min-width: 1100px; /* Increased for more columns */
            display: flex;
            flex-direction: column;
            min-height: 100%;
        }

        .report-table { width: 100%; border-collapse: collapse; font-size: 12px; }
        .report-table th { 
            background-color: #f1f5f9; color: var(--text-muted); 
            text-align: left; position: sticky; top: 0; z-index: 10; 
            padding: 12px 10px; font-weight: 700; text-transform: uppercase;
            font-size: 10px; border-bottom: 2px solid var(--border);
        }
        .report-table td { border-bottom: 1px solid var(--border); padding: 8px 10px; }

        /* FIXED FOOTER LOGIC */
        .fixed-footer-sticky {
            position: sticky;
            bottom: 0;
            background-color: #f1f5f9;
            z-index: 11;
            box-shadow: 0 -4px 10px rgba(0,0,0,0.05);
            margin-top: auto;
        }

        .balance-row { font-weight: 700; }
        .balance-row td { color: var(--primary-dark); padding: 12px 10px; border-top: 2px solid var(--border); }

        .right-sidebar { 
            width: 320px; background-color: #fff; 
            border-left: 1px solid var(--border); display: flex; flex-direction: column; 
        }
        .list-header { 
            background-color: #f1f5f9; color: var(--text-main); 
            padding: 12px 15px; font-size: 11px; font-weight: 800; text-transform: uppercase;
            border-bottom: 1px solid var(--border);
        }

        .footer-toolbar { 
            background-color: #fff; padding: 10px 20px; 
            border-top: 1px solid var(--border); display: flex; 
            justify-content: space-between; align-items: center; height: 65px; 
        }
        .btn-custom { 
            padding: 8px 18px; font-size: 13px; font-weight: 600;
            border: 1px solid var(--border); border-radius: 6px; 
            text-decoration: none; display: inline-flex; align-items: center; gap: 8px;
        }
        .btn-refresh { background-color: var(--primary); color: #fff !important; border: none; }
        .btn-refresh:hover { background-color: var(--primary-dark); }

        /* Column widths */
        .w-name { min-width: 250px; }
        .w-amt { width: 120px; text-align: right; }
        .bg-op { background-color: #fefce8; } /* Light yellow for Opening */
        .bg-tr { background-color: #f0f9ff; } /* Light blue for Transactions */
        .bg-cl { background-color: #f0fdf4; } /* Light green for Closing */

        @media (max-width: 992px) {
            body, html { overflow: auto; }
            .main-container { flex-direction: column; height: auto; }
            .right-sidebar { width: 100%; height: auto; border-left: none; }
            .table-area { height: 500px; }
            .footer-toolbar { height: auto; flex-direction: column; gap: 12px; }
        }
    </style>

    <script type="text/javascript">
        function showLoader() { document.getElementById('loader-wrapper').style.display = 'flex'; }
        window.onload = function () { document.getElementById('loader-wrapper').style.display = 'none'; };

        function filterTable() {
            var input = document.getElementById('txtSearch').value.toLowerCase();
            var rows = document.querySelectorAll('.grid-row');
            rows.forEach(row => {
                var text = row.innerText.toLowerCase();
                row.style.display = text.includes(input) ? "" : "none";
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="loader-wrapper">
            <div class="loader-spinner"></div>
            <div class="loader-text mt-3 fw-bold text-primary">LOADING DATA......</div>
        </div>

        <div class="report-header">
            <span><i class="fas fa-scale-balanced me-2"></i> Trial Balance Statement</span>
            <asp:Label ID="lblStatus" runat="server" Font-Size="11px" ForeColor="#fda4af"></asp:Label>
        </div>

        <div class="info-bar">
            <span class="fw-bold text-muted small text-uppercase me-3">Summary View:</span>
            <div class="badge bg-primary px-3 py-2">Account-wise Totals</div>
        </div>

        <div class="main-container">
            <div class="table-area">
                <div class="grid-body-scroll">
                    <div class="scroll-content-wrapper">
                        <asp:GridView ID="gvTrialBalance" runat="server" AutoGenerateColumns="False" CssClass="report-table" GridLines="None" ShowHeader="True">
                            <HeaderStyle CssClass="trial-header" />
                            <RowStyle CssClass="grid-row" />
                            <Columns>
                                <asp:BoundField DataField="AccountName" HeaderText="Account Particulars" ItemStyle-CssClass="w-name fw-bold" HeaderStyle-CssClass="w-name" />
                                
                                <asp:BoundField DataField="OpDr" HeaderText="Op. Debit" ItemStyle-CssClass="w-amt bg-op" HeaderStyle-CssClass="w-amt bg-op" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="OpCr" HeaderText="Op. Credit" ItemStyle-CssClass="w-amt bg-op" HeaderStyle-CssClass="w-amt bg-op" DataFormatString="{0:N2}" />
                                
                                <asp:BoundField DataField="TrDr" HeaderText="Trans. Debit" ItemStyle-CssClass="w-amt bg-tr" HeaderStyle-CssClass="w-amt bg-tr" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="TrCr" HeaderText="Trans. Credit" ItemStyle-CssClass="w-amt bg-tr" HeaderStyle-CssClass="w-amt bg-tr" DataFormatString="{0:N2}" />
                                
                                <asp:BoundField DataField="ClDr" HeaderText="Cl. Debit" ItemStyle-CssClass="w-amt bg-cl" HeaderStyle-CssClass="w-amt bg-cl" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="ClCr" HeaderText="Cl. Credit" ItemStyle-CssClass="w-amt bg-cl" HeaderStyle-CssClass="w-amt bg-cl" DataFormatString="{0:N2}" />
                            </Columns>
                        </asp:GridView>

                        <!-- Fixed Totals Footer -->
                        <div class="fixed-footer-sticky">
                            <table class="report-table">
                                <tr class="balance-row">
                                    <td class="w-name ps-3">GRAND TOTALS</td>
                                    <td class="w-amt bg-op"><asp:Literal ID="litSumOpDr" runat="server">0.00</asp:Literal></td>
                                    <td class="w-amt bg-op"><asp:Literal ID="litSumOpCr" runat="server">0.00</asp:Literal></td>
                                    <td class="w-amt bg-tr"><asp:Literal ID="litSumTrDr" runat="server">0.00</asp:Literal></td>
                                    <td class="w-amt bg-tr"><asp:Literal ID="litSumTrCr" runat="server">0.00</asp:Literal></td>
                                    <td class="w-amt bg-cl"><asp:Literal ID="litSumClDr" runat="server">0.00</asp:Literal></td>
                                    <td class="w-amt bg-cl"><asp:Literal ID="litSumClCr" runat="server">0.00</asp:Literal></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <div class="right-sidebar">
                <div class="list-header">Filter Controls</div>
                <div class="p-3">
                    <label class="small fw-bold text-muted mb-1">SEARCH ACCOUNT</label>
                    <input type="text" id="txtSearch" class="form-control form-control-sm mb-3" placeholder="Type to filter..." onkeyup="filterTable()" />
                    
                    <label class="small fw-bold text-muted mb-1">REPORT OPTIONS</label>
                    <asp:CheckBox ID="chkHideZero" runat="server" Text="&nbsp;Hide Zero Balances" CssClass="small d-block mb-2" AutoPostBack="true" />
                    <%--<asp:CheckBox ID="chkShowGroups" runat="server" Text="&nbsp;Show Group Totals" CssClass="small d-block mb-2" AutoPostBack="true" />--%>
                </div>
<%--                <div class="mt-auto p-3 border-top bg-light">
                    <p class="x-small text-muted mb-0">Note: Trial Balance is generated based on selected date range.</p>
                </div>--%>
            </div>
        </div>

        <div class="footer-toolbar">
            <div class="d-flex align-items-center gap-2 date-input-group">
                <span class="small fw-bold text-muted">AS ON DATE:</span>
                <asp:TextBox ID="txtDate" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:160px;"></asp:TextBox>
                
                <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn-custom btn-refresh ms-2" OnClick="btnRefresh_Click" OnClientClick="showLoader();">
                    <i class="fas fa-sync-alt"></i> <span>Refresh</span>
                </asp:LinkButton>
            </div>
            <div class="d-flex gap-2">
<%--                <asp:LinkButton ID="btnExport" runat="server" CssClass="btn-custom text-success border-success">
                    <i class="fas fa-file-excel"></i> <span>Excel</span>
                </asp:LinkButton>--%>
                <asp:LinkButton ID="btnExit" runat="server" CssClass="btn-custom text-danger border-danger" OnClick="btnExit_Click">
                    <i class="fas fa-sign-out-alt"></i> <span>Exit</span>
                </asp:LinkButton>
            </div>
        </div>
    </form>
</body>
</html>
<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LedgerForm.aspx.vb" Inherits="WebApplication1.LedgerForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporting - Ledger Statement</title>
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
        
        .ledger-info-bar { 
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
            min-width: 900px;
            display: flex;
            flex-direction: column;
            min-height: 100%;
        }

        .report-table { width: 100%; border-collapse: collapse; font-size: 13px; }
        .report-table th { 
            background-color: #f1f5f9; color: var(--text-muted); 
            text-align: left; position: sticky; top: 0; z-index: 10; 
            padding: 12px 10px; font-weight: 700; text-transform: uppercase;
            font-size: 11px; border-bottom: 2px solid var(--border);
        }
        .report-table td { border-bottom: 1px solid var(--border); padding: 10px; }

        /* FIXED FOOTER LOGIC */
        .fixed-footer-sticky {
            position: sticky;
            bottom: 0;
            background-color: #fff;
            z-index: 11;
            box-shadow: 0 -4px 10px rgba(0,0,0,0.05);
            margin-top: auto; /* Pushes footer to bottom if rows are few */
        }

        .balance-row { font-weight: 700; background-color: #f8fafc !important; }
        .balance-row td { border-top: 1px solid var(--border) !important; color: var(--primary-dark); }

        .right-sidebar { 
            width: 320px; background-color: #fff; 
            border-left: 1px solid var(--border); display: flex; flex-direction: column; 
        }
        .list-header { 
            background-color: #f1f5f9; color: var(--text-main); 
            padding: 12px 15px; font-size: 11px; font-weight: 800; text-transform: uppercase;
            border-bottom: 1px solid var(--border);
        }
        .ledger-list-box { flex-grow: 1; border: none; font-size: 13px; outline: none; padding: 5px; }
        .ledger-list-box option { padding: 8px 12px; border-radius: 4px; }

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

        .w-date { width: 110px; }
        .w-type { width: 120px; }
        .w-inv { width: 100px; }
        .w-amt { width: 130px; text-align: right; }

        /* MOBILE RESPONSIVE DESIGN */
        @media (max-width: 992px) {
            body, html { overflow: auto; }
            .main-container { flex-direction: column; height: auto; }
            .right-sidebar { width: 100%; height: 350px; border-left: none; border-top: 2px solid var(--primary); }
            .table-area { height: 500px; }
            .report-header span { font-size: 12px; }
            .footer-toolbar { 
                height: auto; flex-direction: column; gap: 12px; padding: 15px;
            }
            .footer-toolbar .d-flex { width: 100%; justify-content: center; flex-wrap: wrap; }
            .btn-custom { width: 100%; justify-content: center; }
            .date-input-group { display: flex; gap: 5px; width: 100%; justify-content: center; }
        }
    </style>

    <script type="text/javascript">
        function showLoader() { document.getElementById('loader-wrapper').style.display = 'flex'; }
        window.onload = function () { document.getElementById('loader-wrapper').style.display = 'none'; };

        function filterLedgers() {
            var input = document.getElementById('<%= txtSearch.ClientID %>').value.toLowerCase().trim();
            var listbox = document.getElementById('<%= lstLedgers.ClientID %>');
            var options = listbox.options;

            for (var i = 0; i < options.length; i++) {
                var text = options[i].text.toLowerCase();
                // If input is empty, show all. Otherwise, filter.
                if (input === "" || text.includes(input)) {
                    options[i].style.display = "";
                } else {
                    options[i].style.display = "none";
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="loader-wrapper">
            <div class="loader-spinner"></div>
            <div class="loader-text mt-3 fw-bold text-primary">FETCHING DATA...</div>
        </div>

        <div class="report-header">
            <span><i class="fas fa-book me-2"></i> Ledger Statement</span>
            <asp:Label ID="lblStatus" runat="server" Font-Size="11px" ForeColor="#fda4af"></asp:Label>
        </div>

        <div class="ledger-info-bar">
            <span class="fw-bold text-muted small text-uppercase me-3">Selected:</span>
            <asp:TextBox ID="txtSelectedLedger" runat="server" CssClass="form-control form-control-sm bg-light" 
                style="max-width: 400px; font-weight: 700; color: var(--primary-dark);" ReadOnly="true"></asp:TextBox>
        </div>

        <div class="main-container">
            <div class="table-area">
                <div class="grid-body-scroll">
                    <div class="scroll-content-wrapper">
                        <!-- Data Table -->
                        <asp:GridView ID="gvLedger" runat="server" AutoGenerateColumns="False" CssClass="report-table" GridLines="None" ShowHeader="True">
                            <Columns>
                                <asp:BoundField DataField="vch_date" HeaderText="Date" DataFormatString="{0:dd-MM-yyyy}" ItemStyle-CssClass="w-date" HeaderStyle-CssClass="w-date" />
                                <asp:BoundField DataField="oppledger" HeaderText="Particulars" HeaderStyle-CssClass="ps-3" ItemStyle-CssClass="ps-3" />
                                <asp:BoundField DataField="vt_shortname" HeaderText="Type" ItemStyle-CssClass="w-type" HeaderStyle-CssClass="w-type" />
                                <asp:BoundField DataField="vch_no" HeaderText="Inv No" ItemStyle-CssClass="w-inv" HeaderStyle-CssClass="w-inv" />
                                <asp:BoundField DataField="Cr_Amount" HeaderText="Credit (In)" ItemStyle-CssClass="w-amt" HeaderStyle-CssClass="w-amt" DataFormatString="{0:N2}" />
                                <asp:BoundField DataField="Dr_Amount" HeaderText="Debit (Out)" ItemStyle-CssClass="w-amt" HeaderStyle-CssClass="w-amt" DataFormatString="{0:N2}" />
                            </Columns>
                        </asp:GridView>

                        <!-- Fixed Footer Container -->
                        <div class="fixed-footer-sticky">
                            <table class="report-table">
                                <tr class="balance-row">
                                    <td class="w-date"></td>
                                    <td class="ps-3">Opening Balance</td>
                                    <td class="w-type"></td><td class="w-inv"></td>
                                    <td class="w-amt"><asp:Literal ID="litOpCr" runat="server">0.00</asp:Literal></td>
                                    <td class="w-amt"><asp:Literal ID="litOpDr" runat="server">0.00</asp:Literal></td>
                                </tr>
                                <tr class="balance-row text-primary">
                                    <td class="w-date"></td>
                                    <td class="ps-3"><asp:Literal ID="litTotalCount" runat="server">Total ( 0 )</asp:Literal></td>
                                    <td class="w-type"></td><td class="w-inv"></td>
                                    <td class="w-amt"><asp:Literal ID="litTotalCr" runat="server">0.00</asp:Literal></td>
                                    <td class="w-amt"><asp:Literal ID="litTotalDr" runat="server">0.00</asp:Literal></td>
                                </tr>
                                <tr class="balance-row">
                                    <td class="w-date"></td>
                                    <td class="ps-3">Closing Balance</td>
                                    <td class="w-type"></td><td class="w-inv"></td>
                                    <td class="w-amt"><asp:Literal ID="litClCr" runat="server">0.00</asp:Literal></td>
                                    <td class="w-amt"><asp:Literal ID="litClDr" runat="server">0.00</asp:Literal></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <div class="right-sidebar">
                <div class="list-header">Account Directory</div>
                <div class="p-2 border-bottom">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" 
                        placeholder="Search Account..." onkeyup="filterLedgers()"></asp:TextBox>
                </div>
                <asp:ListBox ID="lstLedgers" runat="server" CssClass="ledger-list-box" 
                    AutoPostBack="true" OnSelectedIndexChanged="lstLedgers_SelectedIndexChanged" onchange="showLoader();"></asp:ListBox>
            </div>
        </div>

        <div class="footer-toolbar">
            <div class="d-flex align-items-center gap-2 date-input-group">
                <%--<span class="small fw-bold text-muted d-none d-md-inline">Date:</span>--%>
                <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:135px;"></asp:TextBox>
                <span class="small text-muted">to</span>
                <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="form-control form-control-sm" style="width:135px;"></asp:TextBox>
                
                <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn-custom btn-refresh ms-md-2" OnClick="btnRefresh_Click" OnClientClick="showLoader();">
                    <i class="fas fa-sync-alt"></i> <span>Refresh</span>
                </asp:LinkButton>
            </div>
            <div class="d-flex gap-2">
                <asp:LinkButton ID="btnExit" runat="server" CssClass="btn-custom text-danger border-danger" OnClick="btnExit_Click">
                    <i class="fas fa-sign-out-alt"></i> <span>Exit</span>
                </asp:LinkButton>
            </div>
        </div>
    </form>
</body>
</html>
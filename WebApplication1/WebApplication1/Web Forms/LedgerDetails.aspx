<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LedgerDetails.aspx.vb" Inherits="WebApplication1.LedgerDetail" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporting - Ledger Details</title>
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
            --success: #10b981;
            --warning: #f59e0b;
        }

        body, html { overflow: auto;  margin: 0; font-family: 'Inter', sans-serif; background-color: var(--bg-light); color: var(--text-main); overflow-x: hidden; }

        /* Loader */
        #loader-wrapper {
            position: fixed; top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(255,255,255,0.9); z-index: 9999;
            display: flex; justify-content: center; align-items: center; flex-direction: column;
        }
        .loader-spinner {
            width: 40px; height: 40px;
            border: 3px solid #f3f3f3; border-top: 3px solid var(--primary);
            border-radius: 50%; animation: spin 0.8s linear infinite;
        }
        @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }
        .loader-text { font-weight: 700; color: var(--primary); margin-top: 10px; }

        /* Header */
        .report-header { background: var(--header-bg); padding: 0 20px; color: #fff; font-size: 14px; font-weight: 600; display: flex; justify-content: space-between; height: 45px; align-items: center; }

        /* Info Bar */
        .ledger-info-bar { background-color: #fff; padding: 0 20px; font-size: 13px; border-bottom: 1px solid var(--border); display: flex; align-items: center; height: 55px; }
        .ledger-label { font-weight: 700; color: var(--text-muted); margin-right: 15px; text-transform: uppercase; font-size: 11px; }

        /* Layout */
        .main-container { display: flex; flex-direction: row; height: calc(100vh - 165px); }

        /* Content */
        .content-area { flex-grow: 1; overflow-y: auto; padding: 20px; background-color: var(--bg-light); display: flex; flex-direction: column; }

        /* Right Sidebar */
        .right-sidebar { width: 320px; background-color: #fff; border-left: 1px solid var(--border); display: flex; flex-direction: column; }
        .list-header { background-color: #f1f5f9; color: var(--text-main); padding: 12px 15px; font-size: 11px; font-weight: 800; text-transform: uppercase; border-bottom: 1px solid var(--border); }
        .ledger-list-box { flex-grow: 1; border: none; font-size: 13px; outline: none; padding: 5px; color: var(--text-main); cursor: pointer; }
        .ledger-list-box option { padding: 8px 12px; border-radius: 4px; }

        /* Detail Cards */
        .detail-card { background: #fff; border-radius: 8px; border: 1px solid var(--border); padding: 20px; margin-bottom: 20px; box-shadow: 0 1px 2px rgba(0,0,0,0.05); }
        .info-group-title { font-size: 11px; font-weight: 800; color: var(--primary); text-transform: uppercase; margin-bottom: 15px; border-bottom: 1px solid #f1f5f9; padding-bottom: 8px; display: flex; align-items: center; gap: 8px; }
        .lbl-title { font-size: 11px; font-weight: 700; color: var(--text-muted); text-transform: uppercase; display: block; }
        .lbl-value { font-size: 13px; font-weight: 600; color: var(--text-main); display: block; margin-bottom: 12px; }

        /* Balance Highlight */
        .balance-highlight { background-color: var(--primary); color: #fff; padding: 15px; border-radius: 8px; text-align: center; margin-bottom: 20px; }
        .balance-highlight .lbl-title { color: rgba(255,255,255,0.7); font-size: 11px; }

        /* Footer */
        .footer-toolbar { background-color: #fff; padding: 10px 20px; border-top: 1px solid var(--border); display: flex; justify-content: space-between; align-items: center; height: 65px; }
        .btn-custom { padding: 8px 18px; font-size: 13px; font-weight: 600; border: 1px solid var(--border); background: #fff; color: var(--text-main); border-radius: 6px; text-decoration: none; display: inline-flex; align-items: center; gap: 8px; transition: all 0.2s; }
        .btn-custom:hover { background-color: #f1f5f9; color: var(--primary); }

        @media(max-width:992px){
            .main-container{flex-direction: column-reverse;height:auto;}
            .right-sidebar{width:100%;height:300px;border-left:none;border-bottom:2px solid var(--primary);}
            .content-area{height:auto;padding:15px;}
            .footer-toolbar{height:auto;padding:15px;position:relative;}
            .btn-custom{width:100%;justify-content:center;height:45px;}
        }
    </style>

    <script type="text/javascript">
        function showLoader() { document.getElementById('loader-wrapper').style.display = 'flex'; }
        window.onload = function () { document.getElementById('loader-wrapper').style.display = 'none'; };

        function filterList() {
            var input = document.getElementById('<%= txtSearch.ClientID %>').value.toLowerCase();
            var listbox = document.getElementById('<%= lstLedgers.ClientID %>');
            var options = listbox.options;
            for (var i = 0; i < options.length; i++) {
                options[i].style.display = options[i].text.toLowerCase().includes(input) ? "" : "none";
            }
        }
    </script>
</head>

<body>
    <form id="form1" runat="server">

        <!-- Loader -->
        <div id="loader-wrapper">
            <div class="loader-spinner"></div>
            <div class="loader-text">LOADING LEDGER DATA...</div>
        </div>

        <!-- Header -->
        <div class="report-header">
            <span><i class="fas fa-id-card me-2 text-primary"></i> Ledger Profile Details</span>
            <asp:Label ID="lblStatus" runat="server" Font-Size="11px" ForeColor="#fda4af" Font-Bold="true"></asp:Label>
        </div>

        <!-- Selected Ledger Bar -->
        <div class="ledger-info-bar">
            <span class="ledger-label">Selected Account</span>
            <asp:TextBox ID="txtSelectedLedger" runat="server" CssClass="form-control form-control-sm" style="max-width:450px;font-weight:700;border-color:var(--primary);color:var(--primary-dark);" ReadOnly="true"></asp:TextBox>
        </div>

        <!-- Main -->
        <div class="main-container">

            <!-- Content Area -->
            <div class="content-area">
                <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                    <div class="balance-highlight">
                        <div class="lbl-title">Current Closing Balance</div>
                        <h3 class="mb-0 fw-bold">₹ <asp:Literal ID="litBalance" runat="server" /></h3>
                    </div>

                    <div class="row">
                        <div class="col-md-4">
                            <div class="detail-card">
                                <div class="info-group-title"><i class="fas fa-user"></i> Basic Profile</div>
                                <span class="lbl-title">Ledger Name</span>
                                <span class="lbl-value"><asp:Literal ID="litMainName" runat="server" /></span>
                                <span class="lbl-title">Group Name</span>
                                <span class="lbl-value"><asp:Literal ID="litGroupName" runat="server" /></span>
                                <span class="lbl-title">Area / Location</span>
                                <span class="lbl-value"><asp:Literal ID="litArea" runat="server" /></span>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="detail-card">
                                <div class="info-group-title"><i class="fas fa-phone"></i> Contact Details</div>
                                <span class="lbl-title">Mobile Number</span>
                                <span class="lbl-value"><asp:Literal ID="litMobile" runat="server" /></span>
                                <span class="lbl-title">Email Address</span>
                                <span class="lbl-value"><asp:Literal ID="litEmail" runat="server" /></span>
                                <span class="lbl-title">Full Address</span>
                                <span class="lbl-value"><asp:Literal ID="litAdd1" runat="server" /> <asp:Literal ID="litAdd2" runat="server" /> <asp:Literal ID="litAdd3" runat="server" /></span>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="detail-card">
                                <div class="info-group-title"><i class="fas fa-file-contract"></i> Statutory & Assignment</div>
                                <span class="lbl-title">GST Number</span>
                                <span class="lbl-value text-success"><asp:Literal ID="litGSTNo" runat="server" /></span>
                                <span class="lbl-title">PAN / TIN</span>
                                <span class="lbl-value"><asp:Literal ID="litTIN" runat="server" /></span>
                                <span class="lbl-title">Salesman</span>
                                <span class="lbl-value"><asp:Literal ID="litSalesman" runat="server" /></span>
                                <span class="lbl-title">Agent / Broker</span>
                                <span class="lbl-value"><asp:Literal ID="litAgent" runat="server" /></span>
                                <span class="lbl-title">Credit Limit</span>
                                <span class="lbl-value text-danger">₹ <asp:Literal ID="litLimit" runat="server" /></span>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlEmpty" runat="server" CssClass="text-center py-5 mt-5 text-muted">
                    <i class="fas fa-user-circle fa-3x mb-3 text-light"></i>
                    <h5>Select an account from the sidebar to view details</h5>
                </asp:Panel>
            </div>

            <!-- Sidebar -->
            <div class="right-sidebar">
                <div class="list-header"><i class="fas fa-users me-2"></i> Account Directory</div>
                <div class="p-2 border-bottom">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" placeholder="Search Account..." onkeyup="filterList()"></asp:TextBox>
                </div>
                <asp:ListBox ID="lstLedgers" runat="server" CssClass="ledger-list-box" AutoPostBack="true" OnSelectedIndexChanged="lstLedgers_SelectedIndexChanged" onchange="showLoader();"></asp:ListBox>
            </div>
        </div>

        <!-- Footer -->
        <div class="footer-toolbar">
            <div></div>
            <div>
                <asp:LinkButton ID="btnExit" runat="server" CssClass="btn-custom border-danger text-danger" OnClick="btnExit_Click">
                    <i class="fas fa-sign-out-alt"></i> Exit
                </asp:LinkButton>
            </div>
        </div>

    </form>
</body>
</html>
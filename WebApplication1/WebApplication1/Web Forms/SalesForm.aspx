<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SalesForm.aspx.vb" Inherits="WebApplication1.SalesForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporting - Sales Report</title>
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
        }

        body, html { height: 100%; margin: 0; font-family: 'Inter', sans-serif; background-color: var(--primary-bg); color: var(--text-main); overflow: hidden; }
        
        #loader-wrapper { position: fixed; top: 0; left: 0; width: 100%; height: 100%; background: rgba(255, 255, 255, 0.9); z-index: 9999; display: flex; flex-direction: column; justify-content: center; align-items: center; }
        .loader-spinner { width: 45px; height: 45px; border: 4px solid #f3f3f3; border-top: 4px solid var(--accent-blue); border-radius: 50%; animation: spin 0.8s linear infinite; }
        @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }

        .main-wrapper { display: flex; flex-direction: column; height: 100vh; }
        .report-header { background: var(--header-dark); padding: 10px 20px; font-size: 0.9rem; font-weight: 600; color: #fff; height: 45px; display: flex; align-items: center; }

        .grid-content-scroll { flex-grow: 1; overflow: auto; background-color: #fff; }
        
        /* Forces columns to stay aligned even during horizontal scrolling */
        .scroll-width-wrapper { min-width: 1100px; display: flex; flex-direction: column; min-height: 100%; }

        .report-table { width: 100%; border-collapse: collapse; font-size: 13px; table-layout: fixed; }
        .report-table th { background-color: #f1f5f9; border-bottom: 2px solid var(--border-color); padding: 12px 10px; position: sticky; top: 0; text-align: left; color: #475569; font-weight: 700; text-transform: uppercase; font-size: 11px; z-index: 40; }
        .report-table td { border-bottom: 1px solid var(--border-color); padding: 10px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }

        /* Column Sizing - Exact sync between Grid and Footer */
        .col-no { width: 70px; }
        .col-date { width: 100px; }
        .col-party { width: 250px; }
        .col-area { width: 120px; }
        .col-gst { width: 140px; }
        .col-mode { width: 80px; }
        .col-qty { width: 100px; text-align: right; }
        .col-gross { width: 120px; text-align: right; }
        .col-net { width: 130px; text-align: right; font-weight: 700; color: var(--accent-blue); }

        /* THE STICKY FOOTER FIX */
        .summary-sticky-container { 
            background-color: var(--summary-bg); 
            border-top: 2px solid #bfdbfe; 
            position: sticky; 
            bottom: 0; 
            z-index: 50; 
            margin-top: auto; /* Pushes footer to bottom if records are few */
        }
        .summary-row { font-weight: 700; color: #1e40af; }

        .footer-toolbar { flex-shrink: 0; background-color: #fff; padding: 10px 20px; border-top: 1px solid var(--border-color); display: flex; justify-content: space-between; align-items: center; min-height: 65px; gap: 15px; }
        .btn-custom { padding: 8px 16px; font-size: 13px; font-weight: 600; border: 1px solid var(--border-color); border-radius: 6px; background: #fff; color: var(--text-main); text-decoration: none; display: inline-flex; align-items: center; gap: 8px; transition: all 0.2s; white-space: nowrap; }
        .btn-custom:hover { background: #f1f5f9; color: var(--accent-blue); }
        .btn-refresh { background-color: var(--accent-blue) !important; color: #fff !important; border: none; }
        
        .date-group { display: flex; align-items: center; gap: 8px; }
        .date-input { width: 145px; height: 38px; border-radius: 6px; border: 1px solid var(--border-color); font-size: 13px; padding: 5px 10px; background: #fff; }

        @media (max-width: 992px) {
            body, html { overflow: auto; }
            .main-wrapper { height: auto; min-height: 100vh; }
            .grid-content-scroll { height: 450px; }
            .footer-toolbar { flex-direction: column; padding: 20px; height: auto; }
            .footer-toolbar .d-flex { flex-direction: column; width: 100%; gap: 12px; }
            .date-group { width: 100%; justify-content: space-between; }
            .date-input { width: 46% !important; flex-grow: 1; }
            .btn-container { width: 100%; }
            .btn-custom { width: 100%; justify-content: center; height: 45px; }
            .report-header { justify-content: center; font-size: 0.8rem; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="loader-wrapper">
            <div class="loader-spinner"></div>
            <div class="loader-text">FETCHING RECORDS...</div>
        </div>

        <div class="main-wrapper">
            <div class="report-header">
                <i class="fas fa-file-invoice me-2"></i> Sales Summary Report
            </div>

            <div class="grid-content-scroll">
                <div class="scroll-width-wrapper">
                    <!-- Data Grid -->
                    <asp:GridView ID="gvSales" runat="server" AutoGenerateColumns="False" CssClass="report-table" GridLines="None" ShowHeader="True">
                        <Columns>
                            <asp:BoundField DataField="Sales No" HeaderText="No" HeaderStyle-CssClass="col-no" ItemStyle-CssClass="col-no" />
                            <asp:BoundField DataField="Sales Date" HeaderText="Date" DataFormatString="{0:dd-MM-yyyy}" HeaderStyle-CssClass="col-date" ItemStyle-CssClass="col-date" />
                            <asp:BoundField DataField="Party Name" HeaderText="Party Name" HeaderStyle-CssClass="col-party" ItemStyle-CssClass="col-party" />
                            <asp:BoundField DataField="Area" HeaderText="Area" HeaderStyle-CssClass="col-area" ItemStyle-CssClass="col-area" />
                            <asp:BoundField DataField="GSTNO" HeaderText="GST No" HeaderStyle-CssClass="col-gst" ItemStyle-CssClass="col-gst" />
                            <asp:BoundField DataField="Cash Mode" HeaderText="Mode" HeaderStyle-CssClass="col-mode" ItemStyle-CssClass="col-mode" />
                            <asp:BoundField DataField="Total Qty" HeaderText="Qty" HeaderStyle-CssClass="col-qty" ItemStyle-CssClass="col-qty" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="Gross" HeaderText="Gross" HeaderStyle-CssClass="col-gross" ItemStyle-CssClass="col-gross" DataFormatString="{0:N2}" />
                            <asp:BoundField DataField="NetAmount" HeaderText="Net Amount" HeaderStyle-CssClass="col-net" ItemStyle-CssClass="col-net" DataFormatString="{0:N2}" />
                        </Columns>
                        <EmptyDataTemplate>
                            <div class="p-5 text-center text-muted">No sales records found for selected dates.</div>
                        </EmptyDataTemplate>
                    </asp:GridView>

                    <!-- STICKY FOOTER -->
                    <div class="summary-sticky-container">
                        <table class="report-table">
                            <tr class="summary-row">
                                <td colspan="6" style="padding-left: 20px;">
                                    TOTAL BILLS: <span class="badge bg-primary ms-2"><asp:Literal ID="litCount" runat="server">0</asp:Literal></span>
                                </td>
                                <td class="col-qty"><asp:Literal ID="litQty" runat="server">0.00</asp:Literal></td>
                                <td class="col-gross"><asp:Literal ID="litGross" runat="server">0.00</asp:Literal></td>
                                <td class="col-net"><asp:Literal ID="litNet" runat="server">0.00</asp:Literal></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>

            <!-- FOOTER TOOLBAR -->
            <div class="footer-toolbar">
                <div class="d-flex align-items-center">
                    <div class="date-group">
                        <asp:TextBox ID="txtFromDate" runat="server" TextMode="Date" CssClass="date-input"></asp:TextBox>
                        <span class="small text-muted">to</span>
                        <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="date-input"></asp:TextBox>
                    </div>
                    
                    <div class="btn-container ms-lg-3">
                        <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn-custom btn-refresh" OnClick="btnRefresh_Click" OnClientClick="showLoader();">
                            <i class="fas fa-sync-alt"></i> Refresh
                        </asp:LinkButton>
                    </div>
                </div>
                
                <div class="btn-container">
                    <asp:LinkButton ID="btnExit" runat="server" CssClass="btn-custom text-danger border-danger" OnClick="btnExit_Click">
                        <i class="fas fa-sign-out-alt"></i> Exit
                    </asp:LinkButton>
                </div>
            </div>
        </div>
    </form>

    <script type="text/javascript">
        function showLoader() { document.getElementById('loader-wrapper').style.display = 'flex'; }
        window.onload = function () { document.getElementById('loader-wrapper').style.display = 'none'; };
    </script>
</body>
</html>
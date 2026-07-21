<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OutstandingPayable.aspx.vb" Inherits="WebApplication1.OutstandingPayable" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporting - Outstanding Report</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    
    <style>
        :root {
            --primary-dark: #1e293b;
            --accent: #6366f1;
            --bg-light: #f8fafc;
            --border-color: #e2e8f0;
            --header-bg: #f1f5f9;
        }

        body, html { 
            height: 100%; margin: 0; 
            font-family: 'Inter', -apple-system, sans-serif; 
            background-color: var(--bg-light); 
            overflow: hidden; 
        }

        /* Modern Loader */
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
        .loader-text { margin-top: 15px; font-weight: 600; color: var(--primary-dark); font-size: 0.85rem; letter-spacing: 0.5px; }
        @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }

        /* Main Layout */
        .main-wrapper { display: flex; flex-direction: column; height: 100vh; }

        /* Modern Header */
        .report-header { 
            background: var(--primary-dark); 
            padding: 10px 20px; 
            font-size: 15px; font-weight: 600; color: #fff; 
            height: 45px !important; 
            display: flex; align-items: center;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        /* Grid Container */
        .grid-content-scroll {
            flex-grow: 1;
            overflow: auto;
            background-color: #fff;
            /*margin: 10px;*/
            border-radius: 8px;
            border: 1px solid var(--border-color);
            box-shadow: 0 1px 3px rgba(0,0,0,0.05);
        }

        /* Table Styling */
        .report-table { width: 100%; border-collapse: collapse; font-size: 13.5px; min-width: 800px; }
        
        .report-table th {
            background-color: var(--header-bg);
            border-bottom: 2px solid var(--border-color);
            padding: 12px 15px;
            position: sticky; top: 0;
            text-align: left;
            color: var(--primary-dark);
            font-weight: 700;
            z-index: 40;
            text-transform: uppercase;
            font-size: 11px;
            letter-spacing: 0.5px;
        }

        .report-table td { 
            border-bottom: 1px solid var(--border-color); 
            padding: 10px 15px; 
            color: #475569;
        }

        .report-table tr:hover { background-color: #f1f5f9; }
        
        /* Column Control */
        .col-ledger { width: auto; font-weight: 500; color: var(--primary-dark); }
        .col-area { width: 150px; }
        .col-group { width: 200px; }
        .col-bal { width: 160px; text-align: right; font-weight: 700; }

        /* Footer Toolbar */
        .footer-toolbar {
            flex-shrink: 0;
            background-color: #fff;
            padding: 10px 20px;
            border-top: 1px solid var(--border-color);
            display: flex;
            justify-content: space-between;
            align-items: center;
            min-height: 65px;
        }

        /* Inputs & Buttons */
        .btn-custom { 
            padding: 6px 16px; font-size: 13px; font-weight: 600;
            border: 1px solid var(--border-color);
            background: white; color: var(--primary-dark);
            transition: all 0.2s; border-radius: 6px;
            display: inline-flex; align-items: center; gap: 8px;
            text-decoration: none;
        }
        .btn-custom:hover { background: #f8fafc; border-color: #cbd5e1; color: var(--accent); }
        
        .date-input, .form-select.date-input { 
            border-radius: 6px; border: 1px solid var(--border-color);
            font-size: 13px; height: 36px; padding: 4px 10px;
            background-color: #fff;
        }

        .date-label { font-size: 12px; font-weight: 700; color: #64748b; margin-right: 8px; text-transform: uppercase; }

        /* Responsive UI */
        @media (max-width: 992px) {
            .footer-toolbar { flex-direction: column; height: auto; gap: 15px; padding: 15px; }
            .grid-content-scroll { margin: 5px; }
            .report-table { font-size: 12px; }
            .d-flex.align-items-center { width: 100%; justify-content: center; }
            .date-input { flex-grow: 1; width: 100% !important; margin-bottom: 5px; }
            .right-buttons { width: 100%; display: flex; justify-content: center; }
            .btn-custom { width: 100%; justify-content: center; }
        }

        /* Large Screen tweaks */
        @media (min-width: 1200px) {
            .report-table { font-size: 14px; }
            .date-input { width: 160px !important; }
        }
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
            <div class="loader-text">FETCHING OUTSTANDING DATA...</div>
        </div>

        <div class="main-wrapper">
            <div class="report-header">
                <i class="fas fa-file-invoice-dollar me-2 text-info"></i> Outstanding Summary Report
            </div>

            <div class="grid-content-scroll">
                <asp:GridView ID="gvOutstanding" runat="server" AutoGenerateColumns="False" 
                    CssClass="report-table" GridLines="None" ShowHeaderWhenEmpty="True" 
                    OnRowDataBound="gvOutstanding_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ledger_Name" HeaderText="Ledger Name" HeaderStyle-CssClass="col-ledger" ItemStyle-CssClass="col-ledger" />
                        <asp:BoundField DataField="Area_name" HeaderText="Area" HeaderStyle-CssClass="col-area" ItemStyle-CssClass="col-area" />
                        <asp:BoundField DataField="Ledgergroup_name" HeaderText="Group" HeaderStyle-CssClass="col-group" ItemStyle-CssClass="col-group" />
                        <asp:BoundField DataField="comp_no" HeaderText="Comp" Visible="false" /> 
                        <asp:BoundField DataField="TotalBalance" HeaderText="Balance" HeaderStyle-CssClass="col-bal" ItemStyle-CssClass="col-bal" ItemStyle-HorizontalAlign="Right" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="p-5 text-center text-muted">
                            <i class="fas fa-folder-open fa-3x mb-3"></i>
                            <p>No outstanding records found for the selected criteria.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>

            <div class="footer-toolbar">
                <div class="d-flex align-items-center flex-wrap gap-2">
                    <div class="d-flex align-items-center">
                        <span class="date-label">To:</span>
                        <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="form-control date-input"></asp:TextBox>
                    </div>
        
                    <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-select date-input" ToolTip="Filter by Group"></asp:DropDownList>
                    <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-select date-input" ToolTip="Filter by Area"></asp:DropDownList>
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select date-input" style="width:110px !important;">
                        <asp:ListItem Text="All Bal" Value="ALL"></asp:ListItem>
                        <asp:ListItem Text="Only Dr" Value="DR"></asp:ListItem>
                        <asp:ListItem Text="Only Cr" Value="CR"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn-custom text-primary" 
                        OnClick="btnRefresh_Click" OnClientClick="showLoader();">
                        <i class="fas fa-sync-alt"></i> Refresh
                    </asp:LinkButton>
                </div>
                
                <div class="right-buttons">
                    <asp:LinkButton ID="btnExit" runat="server" CssClass="btn-custom border-danger" OnClick="btnExit_Click">
                        <i class="fas fa-sign-out-alt text-danger"></i> <span class="text-danger">Exit Report</span>
                    </asp:LinkButton>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
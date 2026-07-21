<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="OutstandingPayable.aspx.vb" Inherits="WebApplication1.OutstandingPayable" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporting - Outstanding Report</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    
    <style>
        :root {
            --primary-dark: #0f172a;
            --accent: #6366f1;
            --bg-light: #f8fafc;
            --border-color: #e2e8f0;
            --header-bg: #f1f5f9;
        }

        body, html { 
            height: 100%; margin: 0; 
            font-family: 'Inter', sans-serif; 
            background-color: var(--bg-light); 
            overflow: hidden; /* Keeps the app-like feel */
        }

        /* Modern Loader */
        #loader-wrapper {
            position: fixed; top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(255, 255, 255, 0.9); z-index: 9999;
            display: flex; flex-direction: column; justify-content: center; align-items: center;
        }
        .loader-spinner {
            width: 40px; height: 40px;
            border: 3px solid #f3f3f3; border-top: 3px solid var(--accent);
            border-radius: 50%; animation: spin 0.8s linear infinite;
        }
        @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }

        /* Main Layout */
        .main-wrapper { 
            display: flex; 
            flex-direction: column; 
            height: 100vh; /* Exactly fill the screen */
            width: 100%;
        }

        /* Header matched to MainForm theme */
        .report-header { 
            background: var(--primary-dark); 
            padding: 0 15px; 
            font-size: 0.9rem; font-weight: 600; color: #fff; 
            height: 50px; flex-shrink: 0;
            display: flex; align-items: center;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        /* Grid Container - The magic part that handles scrolling */
        .grid-content-scroll {
            flex-grow: 1;
            overflow: auto; /* vertical and horizontal scroll */
            background-color: #fff;
            margin: 10px;
            border-radius: 8px;
            border: 1px solid var(--border-color);
        }

        /* Table Styling */
        .report-table { width: 100%; border-collapse: collapse; font-size: 13px; min-width: 700px; }
        
        .report-table th {
            background-color: var(--header-bg);
            border-bottom: 2px solid var(--border-color);
            padding: 12px 10px;
            position: sticky; top: 0;
            text-align: left;
            color: #475569;
            font-weight: 700;
            z-index: 10;
            text-transform: uppercase;
            font-size: 11px;
        }

        .report-table td { 
            border-bottom: 1px solid var(--border-color); 
            padding: 10px; 
            color: #1e293b;
            white-space: nowrap;
        }
        .report-table tr:hover { background-color: #f8fafc; }
        
        .col-ledger { font-weight: 600; }
        .col-bal { text-align: right; font-weight: 700; color: var(--accent); }

        /* Footer Toolbar */
        .footer-toolbar {
            flex-shrink: 0;
            background-color: #fff;
            padding: 12px 15px;
            border-top: 1px solid var(--border-color);
            display: flex;
            justify-content: space-between;
            align-items: center;
            gap: 10px;
        }

        /* Inputs & Buttons */
        .btn-custom { 
            padding: 8px 16px; font-size: 13px; font-weight: 600;
            border: 1px solid var(--border-color);
            background: white; color: #334155;
            transition: all 0.2s; border-radius: 8px;
            display: inline-flex; align-items: center; gap: 8px;
            text-decoration: none;
        }
        .btn-custom:hover { background: #f1f5f9; border-color: #cbd5e1; }
        
        .date-input, .form-select { 
            border-radius: 8px; border: 1px solid var(--border-color);
            font-size: 13px; height: 38px;
        }

        .date-label { font-size: 11px; font-weight: 700; color: #64748b; text-transform: uppercase; margin-right: 5px; }

        /* Small Screen Fixes */
        @media (max-width: 768px) {
            .main-wrapper { height: 100dvh; } /* use dynamic viewport height for mobile */
            .grid-content-scroll { margin: 5px; }
            .footer-toolbar { 
                flex-direction: column; 
                align-items: stretch; 
                padding: 15px;
                max-height: 50vh; /* prevents footer from taking over screen */
                overflow-y: auto;
            }
            .right-buttons { margin-top: 5px; }
            .btn-custom { width: 100%; justify-content: center; }
            .date-input-group { width: 100%; display: flex; align-items: center; }
            .date-input { flex-grow: 1; }
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
                        <asp:BoundField DataField="ledger_Name" HeaderText="Ledger Name" ItemStyle-CssClass="col-ledger" />
                        <asp:BoundField DataField="Area_name" HeaderText="Area" />
                        <asp:BoundField DataField="Ledgergroup_name" HeaderText="Group" />
                        <asp:BoundField DataField="comp_no" HeaderText="Comp" Visible="false" /> 
                        <asp:BoundField DataField="TotalBalance" HeaderText="Balance" ItemStyle-CssClass="col-bal" ItemStyle-HorizontalAlign="Right" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="p-5 text-center text-muted">
                            <i class="fas fa-search fa-2x mb-2 opacity-25"></i>
                            <p>No records found.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>

            <div class="footer-toolbar">
                <!-- Grouped inputs for mobile stacking -->
                <div class="d-flex flex-column flex-md-row flex-wrap gap-2 flex-grow-1">
                    <div class="d-flex align-items-center">
                        <span class="date-label">To:</span>
                        <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="form-control date-input"></asp:TextBox>
                    </div>
        
                    <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-select"></asp:DropDownList>
                    <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-select"></asp:DropDownList>
                    <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select" style="min-width: 100px;">
                        <asp:ListItem Text="All Bal" Value="ALL"></asp:ListItem>
                        <asp:ListItem Text="Only Dr" Value="DR"></asp:ListItem>
                        <asp:ListItem Text="Only Cr" Value="CR"></asp:ListItem>
                    </asp:DropDownList>

                    <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn-custom bg-primary text-white border-0" 
                        OnClick="btnRefresh_Click" OnClientClick="showLoader();">
                        <i class="fas fa-sync-alt"></i> Refresh
                    </asp:LinkButton>
                </div>
                
                <div class="right-buttons">
                    <asp:LinkButton ID="btnExit" runat="server" CssClass="btn-custom border-danger text-danger" OnClick="btnExit_Click">
                        <i class="fas fa-sign-out-alt"></i> <span>Exit</span>
                    </asp:LinkButton>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
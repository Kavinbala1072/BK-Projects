<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="StockForm.aspx.vb" Inherits="WebApplication1.StockForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporting - Stock Summary</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    
<style>
    :root {
        --primary: #6366f1;
        --primary-dark: #4f46e5;
        --bg-body: #f8fafc;
        --header-dark: #0f172a;
        --border-color: #e2e8f0;
        --summary-bg: #f1f5f9;
        --text-main: #1e293b;
    }

    body, html { 
        height: 100%; margin: 0; 
        font-family: 'Inter', sans-serif; 
        background-color: var(--bg-body); 
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
        border: 4px solid #f3f3f3; border-top: 4px solid var(--primary);
        border-radius: 50%; animation: spin 0.8s linear infinite;
    }
    @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }
    .loader-text { margin-top: 15px; font-weight: 600; color: var(--header-dark); font-size: 0.85rem; letter-spacing: 0.5px; }

    /* --- LAYOUT --- */
    .main-wrapper { display: flex; flex-direction: column; height: 100vh; }

    .report-header { 
        background: var(--header-dark); padding: 5px 15px; 
        font-size: 0.9rem; font-weight: 600; color: #fff; 
        height: 48px; display: flex; align-items: center; justify-content: space-between;
        box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .grid-content-scroll { flex-grow: 1; overflow: auto; background-color: #fff; }

    .report-table { width: 100%; border-collapse: separate; border-spacing: 0; font-size: 13px; min-width: 1000px; }
    
    .report-table th { 
        background-color: #f1f5f9; border-bottom: 2px solid var(--border-color); 
        padding: 12px 10px; position: sticky; top: 0; 
        text-align: left; color: #475569; font-weight: 700; text-transform: uppercase; font-size: 11px; z-index: 40; 
    }

    .report-table td { border-bottom: 1px solid var(--border-color); padding: 10px; white-space: nowrap; }
    .report-table tr:hover { background-color: #f8fafc; }

    /* --- SUMMARY --- */
    .fixed-summary-container { flex-shrink: 0; background-color: var(--summary-bg); border-top: 2px solid var(--border-color); z-index: 30; }
    .summary-row { font-weight: 700; color: var(--primary-dark); }
    .summary-row td { padding: 12px 15px; border: none !important; }

    /* --- FOOTER TOOLBAR --- */
    .footer-toolbar { 
        flex-shrink: 0; background-color: #fff; padding: 10px 20px; 
        border-top: 1px solid var(--border-color); display: flex; justify-content: space-between; align-items: center; 
    }

    .btn-custom { 
        padding: 8px 16px; font-size: 13px; font-weight: 600; border: 1px solid var(--border-color); border-radius: 6px;
        background: #fff; color: var(--text-main); transition: all 0.2s ease; display: inline-flex; align-items: center; gap: 8px; text-decoration: none;
    }
    .btn-refresh { background-color: var(--primary) !important; color: #fff !important; border: none; }
    .date-input { width: 160px; height: 38px; border-radius: 6px; border: 1px solid var(--border-color); font-size: 13px; padding: 5px; }

    /* --- EXIT BUTTON IN HEADER --- */
    .report-header .btn-custom { 
        padding: 2px 10px; height: 30px; background: rgba(255,255,255,0.1); border: 1px solid rgba(255,255,255,0.2); color: white; 
    }

    /* --- SMALL SCREEN DESIGN (SHRINK) --- */
    @media (max-width: 768px) {
        /* 1. Header Shrink */
        .report-header { height: 42px; padding: 0 10px; font-size: 0.8rem; }
        
        /* 2. Table Density */
        .report-table { font-size: 11px; }
        .report-table th { padding: 8px 5px; font-size: 9px; }
        .report-table td { padding: 6px 5px; }

        /* 3. Footer Shrink */
        .footer-toolbar { 
            padding: 8px; 
            flex-direction: column; 
            gap: 8px;
        }

        /* 4. Date Row (Left Aligned) */
        .footer-toolbar .d-flex.align-items-center.flex-wrap {
            display: grid !important;
            grid-template-columns: 1fr 1fr; /* Buttons side by side */
            width: 100%;
            gap: 8px;
        }

        /* Force the Date input to occupy the first row fully */
        .footer-toolbar .d-flex.align-items-center:first-child {
            width: 100%;
            justify-content: flex-start;
            order: -1; /* Keep at top */
            margin-bottom: 2px;
        }

        /* 5. Compact Elements */
        .date-input { 
            flex-grow: 1;
            width: 100% !important; 
            height: 34px; 
            font-size: 12px; 
        }

        .btn-custom { 
            width: 100%; 
            justify-content: center; 
            padding: 7px 5px; 
            font-size: 11px; 
            gap: 5px;
        }

        .summary-row td { padding: 6px 10px !important; font-size: 11px; }
        
        /* Total row doesn't need to be 1000px on mobile container */
        .fixed-summary-container table { min-width: 100% !important; width: 100%; }
        
        /* Hide text on icons if screen is very small to avoid overflow */
        @media (max-width: 380px) {
            .btn-custom span, .btn-custom i { font-size: 10px; }
        }
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
            <div class="loader-text">GENERATING STOCK SUMMARY...</div>
        </div>

        <div class="main-wrapper">
            
            <!-- HEADER -->
            <div class="report-header">
                <span><i class="fas fa-boxes me-2 text-info"></i> Stock Summary Report</span>
                <asp:Label ID="lblError" runat="server" ForeColor="#fda4af" Font-Size="11px" Font-Bold="true"></asp:Label>
            </div>

            <!-- SCROLLABLE DATA AREA -->
            <div class="grid-content-scroll">
                <asp:GridView ID="gvStock" runat="server" AutoGenerateColumns="False" 
                    CssClass="report-table" GridLines="None" ShowHeader="True" ShowHeaderWhenEmpty="True">
                    <Columns>
                        <asp:TemplateField HeaderText="S.No" HeaderStyle-CssClass="col-sno" ItemStyle-CssClass="col-sno">
                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Item_Name" HeaderText="Item Name" HeaderStyle-CssClass="col-name" ItemStyle-CssClass="col-name" />
                        <asp:BoundField DataField="ItemGroup_Name" HeaderText="Group" />
                        <asp:BoundField DataField="Model_Name" HeaderText="Model" />
                        <asp:BoundField DataField="Brand_Name" HeaderText="Brand" />
                        <asp:BoundField DataField="Rack_Name" HeaderText="Rack" />
                        <asp:BoundField DataField="Item_Srate" HeaderText="S.Rate" HeaderStyle-CssClass="col-rate" ItemStyle-CssClass="col-rate" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="Item_Mrp" HeaderText="MRP" HeaderStyle-CssClass="col-mrp" ItemStyle-CssClass="col-mrp" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="Qty" HeaderText="Qty" HeaderStyle-CssClass="col-qty" ItemStyle-CssClass="col-qty" DataFormatString="{0:N2}" />
                        <asp:BoundField DataField="Pcs" HeaderText="Pcs" HeaderStyle-CssClass="col-pcs" ItemStyle-CssClass="col-pcs" DataFormatString="{0:N0}" />
                    </Columns>
                    <EmptyDataTemplate>
                        <div class="p-5 text-center text-muted">
                            <i class="fas fa-folder-open fa-3x mb-3"></i>
                            <p>No stock data found for the selected criteria.</p>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>

            <div class="fixed-summary-container">
                <table class="report-table" style="min-width: 1000px;">
                    <tr class="summary-row">
                        <td style="width: auto;">
                            <span class="text-muted small text-uppercase">Total Items:</span> 
                            <span class="ms-2"><asp:Literal ID="litCount" runat="server">0</asp:Literal></span>
                        </td>
                        <td class="col-qty">
                            <asp:Literal ID="litTotalQty" runat="server">0.00</asp:Literal>
                        </td>
                        <td class="col-pcs">
                            <asp:Literal ID="litTotalPcs" runat="server">0</asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>

            <!-- FILTER MODAL -->
            <div class="modal fade" id="filterModal" tabindex="-1" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title"><i class="fas fa-sliders-h me-2"></i> Report Filters</h5>
                            <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div class="modal-body p-4">
                            <div class="row g-3">
                                <div class="col-6">
                                    <label class="filter-label">Category Group</label>
                                    <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                                <div class="col-6">
                                    <label class="filter-label">Brand Name</label>
                                    <asp:DropDownList ID="ddlBrand" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                                <div class="col-6">
                                    <label class="filter-label">Product Model</label>
                                    <asp:DropDownList ID="ddlModel" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                                <div class="col-6">
                                    <label class="filter-label">Storage Rack</label>
                                    <asp:DropDownList ID="ddlRack" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                                <div class="col-12">
                                    <label class="filter-label">Inventory Status</label>
                                    <asp:DropDownList ID="ddlStockType" runat="server" CssClass="form-select form-select-sm">
                                        <asp:ListItem Text="All Items" Value="ALL"></asp:ListItem>
                                        <asp:ListItem Text="+ Positive Stock" Value="POS"></asp:ListItem>
                                        <asp:ListItem Text="0 Zero Stock" Value="ZERO"></asp:ListItem>
                                        <asp:ListItem Text="- Negative Stock" Value="NEG"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer border-0 p-3 bg-light">
                            <button type="button" class="btn btn-outline-secondary btn-sm" data-bs-dismiss="modal">Cancel</button>
                            <asp:LinkButton ID="btnApplyFilter" runat="server" CssClass="btn btn-primary btn-sm px-4" OnClick="btnRefresh_Click" OnClientClick="showLoader();">
                                Apply Search
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>

            <!-- FOOTER TOOLBAR -->
            <div class="footer-toolbar">
                <div class="d-flex align-items-center flex-wrap gap-2">
                    <div class="d-flex align-items-center">
                        <span class="text-muted small fw-bold text-uppercase me-2">AS ON DATE:</span>
                        <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="form-control date-input"></asp:TextBox>
                    </div>

                    <button type="button" class="btn-custom" data-bs-toggle="modal" data-bs-target="#filterModal">
                        <i class="fas fa-filter"></i> More Filters
                    </button>
                    
                    <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn-custom btn-refresh" 
                        OnClick="btnRefresh_Click" OnClientClick="showLoader();">
                        <i class="fas fa-sync-alt"></i> Refresh
                    </asp:LinkButton>
                </div>
                <div class="d-flex gap-2">
                    <asp:LinkButton ID="btnExit" runat="server" CssClass="btn-custom text-danger border-danger" OnClick="btnExit_Click">
                        <i class="fas fa-sign-out-alt"></i> <span>Exit</span>
                    </asp:LinkButton>
                </div>
            </div>

        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
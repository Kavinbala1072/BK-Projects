<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ItemDetails.aspx.vb" Inherits="WebApplication1.ItemDetails" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporting - Item Details</title>
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

        body, html { 
            height: 100%; margin: 0; 
            font-family: 'Inter', sans-serif; 
            background-color: var(--bg-light); 
            color: var(--text-main);
            overflow-x: hidden; /* allow vertical scroll on mobile */
        }

        /* Header Components */
        .report-header { 
            background: var(--header-bg); padding: 0 20px; 
            color: #fff; font-size: 14px; font-weight: 600; 
            display: flex; justify-content: space-between; height: 45px; align-items: center; 
        }
        
        .item-info-bar { 
            background-color: #fff; padding: 0 20px; 
            font-size: 13px; border-bottom: 1px solid var(--border); 
            display: flex; align-items: center; height: 55px; 
        }
        .item-label { font-weight: 700; color: var(--text-muted); margin-right: 15px; text-transform: uppercase; font-size: 11px; }

        /* Main Layout */
        .main-container { display: flex; flex-direction: row; min-height: calc(100vh - 165px); }
        
        /* Content Area */
        .content-area { 
            display: flex; flex-direction: column; flex-grow: 1; 
            overflow-y: auto; background: var(--bg-light); padding: 20px;
        }

        /* Right Sidebar */
        .right-sidebar { 
            width: 320px; background-color: #fff; 
            border-left: 1px solid var(--border); display: flex; flex-direction: column; 
        }
        .list-header { 
            background-color: #f1f5f9; color: var(--text-main); 
            padding: 12px 15px; font-size: 11px; font-weight: 800; text-transform: uppercase;
            border-bottom: 1px solid var(--border);
        }
        .item-list-box { 
            flex-grow: 1; border: none; font-size: 13px; outline: none; padding: 5px;
            color: var(--text-main); cursor: pointer;
        }
        .item-list-box option { padding: 8px 12px; border-radius: 4px; }

        /* Detail Cards */
        .detail-card {
            background: #fff; border-radius: 8px; border: 1px solid var(--border);
            padding: 20px; margin-bottom: 20px; box-shadow: 0 1px 2px rgba(0,0,0,0.05);
        }
        .info-group-title {
            font-size: 11px; font-weight: 800; color: var(--primary);
            text-transform: uppercase; margin-bottom: 15px; border-bottom: 1px solid #f1f5f9;
            padding-bottom: 8px; display: flex; align-items: center; gap: 8px;
        }
        .lbl-title { font-size: 11px; font-weight: 700; color: var(--text-muted); text-transform: uppercase; display: block; }
        .lbl-value { font-size: 13px; font-weight: 600; color: var(--text-main); display: block; margin-bottom: 12px; }

        /* Stock Stats Highlight Row */
        .stock-highlight-row { margin-bottom: 20px; }
        .stock-box {
            padding: 15px; border-radius: 8px; color: #fff; text-align: center;
        }
        .bg-opn { background-color: var(--primary); }
        .bg-pcs { background-color: var(--warning); }
        .bg-cls { background-color: var(--success); }

        /* Footer Toolbar */
        .footer-toolbar { 
            background-color: #fff; padding: 10px 20px; 
            border-top: 1px solid var(--border); display: flex; 
            justify-content: space-between; align-items: center; height: 65px; 
        }
        .btn-custom { 
            padding: 8px 18px; font-size: 13px; font-weight: 600;
            border: 1px solid var(--border); background: #fff; 
            color: var(--text-main); border-radius: 6px; text-decoration: none;
            transition: all 0.2s; display: inline-flex; align-items: center; gap: 8px;
        }
        .btn-custom:hover { background-color: #f1f5f9; color: var(--primary); }

        /* Responsive Design */
        @media (max-width: 992px) {
            .main-container { flex-direction: column-reverse; height: auto; }
            .item-info-bar { height: auto; padding: 10px 20px; }
            .item-info-bar .form-control { width: 100% !important; }
            .right-sidebar { width: 100%; height: 300px; border-left: none; border-bottom: 2px solid var(--primary); }
            .content-area { height: auto; flex-grow: 0; padding: 15px; }
            .footer-toolbar { height: auto; padding: 15px; position: relative; }
            .btn-custom { width: 100%; justify-content: center; height: 45px; }
            .stock-box { margin-bottom: 10px; }
        }
    </style>

    <script type="text/javascript">
        function filterList() {
            var input = document.getElementById('<%= txtSearch.ClientID %>').value.toLowerCase();
            var listbox = document.getElementById('<%= lstItems.ClientID %>');
            var options = listbox.options;
            for (var i = 0; i < options.length; i++) {
                var text = options[i].text.toLowerCase();
                options[i].style.display = text.indexOf(input) > -1 ? "" : "none";
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <div class="report-header">
            <span><i class="fas fa-boxes me-2 text-primary"></i> Item Profile Details</span>
            <asp:Label ID="lblStatus" runat="server" Font-Size="11px" ForeColor="#fda4af" Font-Bold="true"></asp:Label>
        </div>

        <div class="item-info-bar">
            <span class="item-label">Selected Item</span>
            <asp:TextBox ID="txtSelectedItem" runat="server" CssClass="form-control form-control-sm" 
                style="max-width: 450px; font-weight: 700; border-color: var(--primary); color: var(--primary-dark);" ReadOnly="true"></asp:TextBox>
        </div>

        <div class="main-container">
            <!-- CONTENT AREA -->
            <div class="content-area">
                <asp:Panel ID="pnlDetails" runat="server" Visible="false">
                    
                    <!-- Stock Summary Stat Boxes -->
                    <div class="row g-3 stock-highlight-row">
                        <div class="col-6 col-md-4">
                            <div class="stock-box bg-opn">
                                <div class="lbl-title text-white-50">Opening Qty</div>
                                <h4 class="mb-0 fw-bold"><asp:Literal ID="litOpnQty" runat="server" /></h4>
                            </div>
                        </div>
                        <div class="col-6 col-md-4">
                            <div class="stock-box bg-pcs">
                                <div class="lbl-title text-white-50">Opening Pcs</div>
                                <h4 class="mb-0 fw-bold"><asp:Literal ID="litOpnPcs" runat="server" /></h4>
                            </div>
                        </div>
                        <div class="col-12 col-md-4">
                            <div class="stock-box bg-cls">
                                <div class="lbl-title text-white-50">Current Closing Stock</div>
                                <h4 class="mb-0 fw-bold"><asp:Literal ID="litClosQty" runat="server" /></h4>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12 col-md-4">
                            <div class="detail-card">
                                <div class="info-group-title"><i class="fas fa-info-circle"></i> General Specs</div>
                                <span class="lbl-title">Item Name</span>
                                <span class="lbl-value"><asp:Literal ID="litItemName" runat="server" /></span>
                                <span class="lbl-title">Item Code</span>
                                <span class="lbl-value"><asp:Literal ID="litItemCode" runat="server" /></span>
                                <span class="lbl-title">Group / Category</span>
                                <span class="lbl-value"><asp:Literal ID="litGroup" runat="server" /></span>
                                <span class="lbl-title">Brand & Model</span>
                                <span class="lbl-value"><asp:Literal ID="litBrand" runat="server" /> / <asp:Literal ID="litModel" runat="server" /></span>
                            </div>
                        </div>
                        <div class="col-12 col-md-4">
                            <div class="detail-card">
                                <div class="info-group-title"><i class="fas fa-tags"></i> Pricing & Units</div>
                                <div class="row mb-2">
                                    <div class="col-6">
                                        <span class="lbl-title">Pur. Rate</span><span class="lbl-value text-primary">₹<asp:Literal ID="litPRate" runat="server" /></span>
                                    </div>
                                    <div class="col-6">
                                        <span class="lbl-title">Cost Rate</span><span class="lbl-value">₹<asp:Literal ID="litCost" runat="server" /></span>
                                    </div>
                                    <div class="col-6">
                                        <span class="lbl-title">M.R.P.</span><span class="lbl-value text-danger">₹<asp:Literal ID="litMRP" runat="server" /></span>
                                    </div>
                                    <div class="col-6">
                                        <span class="lbl-title">Sales Rate</span><span class="lbl-value text-success">₹<asp:Literal ID="litSRate" runat="server" /></span>
                                    </div>
                                </div>
                                <span class="lbl-title">Base Unit</span>
                                <span class="lbl-value"><asp:Literal ID="litBaseUnit" runat="server" /></span>
                                <span class="lbl-title">Margin Group</span>
                                <span class="lbl-value"><asp:Literal ID="litMargin" runat="server" /></span>
                            </div>
                        </div>
                        <div class="col-12 col-md-4">
                            <div class="detail-card">
                                <div class="info-group-title"><i class="fas fa-warehouse"></i> Storage & Tax</div>
                                <span class="lbl-title">GST Tax Name</span>
                                <span class="lbl-value text-success"><asp:Literal ID="litGst" runat="server" /></span>
                                <span class="lbl-title">Rack Location</span>
                                <span class="lbl-value"><asp:Literal ID="litRack" runat="server" /></span>
                                <span class="lbl-title">Size / Dimension</span>
                                <span class="lbl-value"><asp:Literal ID="litSize" runat="server" /></span>
                                <span class="lbl-title">Company Name</span>
                                <span class="lbl-value"><asp:Literal ID="litCompany" runat="server" /></span>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlEmpty" runat="server" CssClass="text-center py-5 mt-5 text-muted">
                    <i class="fas fa-box fa-3x mb-3 text-light"></i>
                    <h5>Select an item from the sidebar to view details</h5>
                </asp:Panel>
            </div>

            <!-- RIGHT SIDEBAR -->
            <div class="right-sidebar">
                <div class="list-header"><i class="fas fa-boxes me-2"></i> Item Inventory</div>
                <div class="p-2 border-bottom">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control form-control-sm" 
                        placeholder="Search Item..." onkeyup="filterList()"></asp:TextBox>
                </div>
                <asp:ListBox ID="lstItems" runat="server" CssClass="item-list-box" 
                    AutoPostBack="true" OnSelectedIndexChanged="lstItems_SelectedIndexChanged"></asp:ListBox>
            </div>
        </div>

        <!-- FOOTER TOOLBAR -->
        <div class="footer-toolbar">
            <div class="d-flex align-items-center gap-2"></div>
            <div>
                <asp:LinkButton ID="btnExit" runat="server" CssClass="btn-custom border-danger text-danger" OnClick="btnExit_Click">
                    <i class="fas fa-sign-out-alt"></i> Exit
                </asp:LinkButton>
            </div>
        </div>
    </form>
</body>
</html>
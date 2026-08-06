<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="BKBilling.Forms.MainForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard | BK Softwares</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.13.1/font/bootstrap-icons.min.css">
    <style>
        :root {
            --sidebar-bg: #ffffff;
            --sidebar-hover: #f1f5fb;
            --accent-blue: #2563eb;
            --accent-blue-dark: #1d4ed8;
            --text-main: #334155;
            --text-dim: #64748b;
            --navbar-bg: #ffffff;
            --body-bg: #f4f6fb;
            --border-col: #e9edf5;
        }

        body, html { height: 100%; margin: 0; background-color: var(--body-bg); font-family: 'Inter', sans-serif; overflow: hidden; }

        #wrapper { display: flex; width: 100%; height: 100vh; align-items: stretch; position: relative; }

        /* ---------- SIDEBAR (light theme) ---------- */
        #sidebar {
            min-width: 260px;
            max-width: 260px;
            background: var(--sidebar-bg);
            color: var(--text-main);
            transition: all 0.25s cubic-bezier(0.4, 0, 0.2, 1);
            z-index: 2000;
            display: flex;
            flex-direction: column;
            border-right: 1px solid var(--border-col);
            overflow-y: auto;
            overflow-x: hidden;
        }
        #sidebar::-webkit-scrollbar { width: 5px; }
        #sidebar::-webkit-scrollbar-thumb { background: #dbe3f0; border-radius: 10px; }

        .sidebar-header {
            height: 65px;
            display: flex;
            align-items: center;
            padding: 0 20px;
            background: var(--sidebar-bg);
            border-bottom: 1px solid var(--border-col);
            flex-shrink: 0;
        }
        .sidebar-header .brand-full { color: var(--accent-blue-dark); letter-spacing: 0.5px; }

        .sidebar-search { padding: 12px 15px; border-bottom: 1px solid var(--border-col); }
        .sidebar-search .input-group { background: var(--body-bg); border-radius: 8px; border: 1px solid var(--border-col); }
        .sidebar-search input { background: transparent !important; border: none !important; color: var(--text-main) !important; font-size: 0.85rem; }
        .sidebar-search input::placeholder { color: #94a3b8; }

        .nav-btn {
            padding: 10px 20px;
            display: flex;
            align-items: center;
            color: var(--text-main);
            text-decoration: none;
            background: none;
            width: 100%;
            text-align: left;
            border: none;
            transition: 0.15s;
            white-space: nowrap;
            border-radius: 8px;
            cursor: pointer;
            font-size: 0.9rem;
            position: relative;
        }
        .nav-btn:hover { background: var(--sidebar-hover); color: var(--accent-blue-dark); }
        .nav-btn.active-link { background: var(--accent-blue); color: #ffffff; font-weight: 600; }
        .nav-btn.active-link i.main-icon { color: #ffffff; }

        .nav-btn i.main-icon { width: 22px; font-size: 1.05rem; margin-right: 12px; text-align: center; color: var(--text-dim); flex-shrink: 0; }
        .nav-btn:hover i.main-icon { color: var(--accent-blue-dark); }

        #mainMenu > li { padding: 2px 12px; }

        .sub-group-btn { padding: 8px 12px 8px 46px; font-size: 0.8rem; font-weight: 600; color: var(--text-dim); display: flex; align-items: center; cursor: pointer; text-decoration: none; border-radius: 8px; }
        .sub-group-btn:hover { color: var(--accent-blue-dark); background: var(--sidebar-hover); }

        .submenu-inner { list-style: none; padding: 0; margin: 0 0 4px 0; }
        .submenu-inner .nav-btn { padding-left: 58px; font-size: 0.82rem; color: #52607a; border-radius: 8px; }

        .has-submenu[data-bs-toggle="collapse"]::after { display: inline-block; margin-left: auto; font-family: "Font Awesome 6 Free"; font-weight: 900; content: "\f054"; font-size: 0.6rem; color: #94a3b8; transition: transform 0.3s; }
        .has-submenu[aria-expanded="true"]::after { transform: rotate(90deg); }

        /* ---------- COLLAPSED (icon-only rail) ---------- */
        #sidebar.active .collapse,
        #sidebar.active .has-submenu::after,
        #sidebar.active .link-text,
        #sidebar.active .sub-group-btn,
        #sidebar.active .sidebar-search,
        #sidebar.active .brand-full { display: none !important; }

        #sidebar.active { min-width: 76px; max-width: 76px; }
        #sidebar.active .sidebar-header { justify-content: center; padding: 0; }
        #sidebar.active .brand-short { display: block !important; }
        #sidebar.active #mainMenu > li { padding: 2px 10px; }
        #sidebar.active .nav-btn { justify-content: center; padding: 12px 0; }
        #sidebar.active .nav-btn i.main-icon { margin-right: 0; font-size: 1.15rem; }

        .brand-short { display: none; }

        /* Floating tooltip shown only when sidebar is collapsed (JS-driven) */
        .side-tooltip {
            position: fixed;
            background: #1e293b;
            color: #fff;
            padding: 6px 12px;
            border-radius: 6px;
            font-size: 0.8rem;
            font-family: 'Inter', sans-serif;
            white-space: nowrap;
            z-index: 4000;
            pointer-events: none;
            opacity: 0;
            transform: translateY(-50%) translateX(-4px);
            transition: opacity 0.12s ease, transform 0.12s ease;
        }
        .side-tooltip.show { opacity: 1; transform: translateY(-50%) translateX(0); }

        #content { flex-grow: 1; display: flex; flex-direction: column; min-width: 0; height: 100vh; position: relative; }
        .navbar { height: 65px; background: var(--navbar-bg); border-bottom: 1px solid var(--border-col); padding: 0 20px; display: flex; align-items: center; z-index: 1000; }
        .hamburger-btn { font-size: 1.1rem; color: var(--text-main); cursor: pointer; border: none; background: none; padding: 6px 10px; border-radius: 8px; }
        .hamburger-btn:hover { background: var(--sidebar-hover); }

        .icon-btn {
            width: 38px; height: 38px;
            display: inline-flex; align-items: center; justify-content: center;
            border: none; background: none; color: var(--text-dim);
            border-radius: 50%; font-size: 1.05rem; cursor: pointer; transition: 0.15s;
        }
        .icon-btn:hover { background: var(--sidebar-hover); color: var(--accent-blue-dark); }

        .avatar-circle {
            width: 38px; height: 38px;
            border-radius: 50%;
            background: #1d4ed8;
            color: #fff;
            display: inline-flex; align-items: center; justify-content: center;
            font-weight: 600; font-size: 0.85rem; letter-spacing: 0.5px;
            flex-shrink: 0;
        }
        .avatar-circle-lg { width: 46px; height: 46px; font-size: 1rem; }

        .avatar-btn { border: none; background: none; padding: 0; cursor: pointer; line-height: 0; }
        .avatar-btn::after { display: none; }

        .profile-menu {
            min-width: 260px;
            border: 1px solid var(--border-col);
            border-radius: 12px;
            box-shadow: 0 12px 28px rgba(15, 23, 42, 0.12);
            margin-top: 12px;
            overflow: hidden;
        }
        .profile-menu-header {
            display: flex; align-items: center; gap: 12px;
            padding: 16px;
            border-bottom: 1px solid var(--border-col);
        }
        .profile-menu-name { font-weight: 700; color: #0f172a; font-size: 0.92rem; }
        .profile-menu-email { font-size: 0.8rem; color: var(--accent-blue); word-break: break-all; }

        .profile-menu-item {
            display: flex; align-items: center; justify-content: space-between;
            padding: 10px 16px;
            font-size: 0.87rem;
            color: #334155;
            text-decoration: none;
            background: none;
            border: none;
            width: 100%;
            text-align: left;
            cursor: pointer;
        }
        .profile-menu-item:hover { background: var(--sidebar-hover); color: var(--accent-blue-dark); }
        .profile-menu-item i { font-size: 0.8rem; color: var(--text-dim); }
        .report-wrapper { flex-grow: 1; padding: 15px; display: flex; flex-direction: column; overflow: hidden; }
        .report-container { border-radius: 12px; box-shadow: 0 4px 6px rgba(0,0,0,0.05); border: 1px solid var(--border-col); flex-grow: 1; display: flex; overflow: hidden; position: relative; background: #fff; }
        .welcome-screen { position: relative; display: flex; flex-direction: column; align-items: center; justify-content: center; width: 100%; height: 100%; text-align: center; }
        #bg-canvas { position: absolute; top: 0; left: 0; width: 100%; height: 100%; z-index: 1; }
        iframe { width: 100%; height: 100%; border: none; position: relative; z-index: 5; }
        @media (max-width: 768px) { #sidebar { margin-left: -280px; position: fixed; height: 100%; } #sidebar.active { margin-left: 0; } }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper">
            <nav id="sidebar">
                <div class="sidebar-header">
                    <h3 class="fw-bold mb-0 brand-full" style="font-size: 1.1rem;">BK SOFTWARES</h3>
                    <h3 class="fw-bold mb-0 brand-short" style="font-size: 1.4rem;"><img src="~/favicon.png" runat="server" alt="Logo" style="width:36px; height:36px;" /></h3>
                </div>
                <div class="sidebar-search">
                    <div class="input-group">
                        <span class="input-group-text bg-transparent border-0"><i class="fas fa-search fa-xs text-muted"></i></span>
                        <input type="text" id="menuSearch" class="form-control" placeholder="Search menu..." />
                    </div>
                </div>
                <div class="flex-grow-1 mt-2">
                    <ul class="list-unstyled" id="mainMenu">
                        <li>
                            <asp:LinkButton ID="btnMenuDash" runat="server" CssClass="nav-btn" OnClick="btnMenuDash_Click">
                                <i class="fas fa-th-large main-icon"></i> <span class="link-text">Dashboard</span>
                            </asp:LinkButton>
                        </li>

                        <!-- 1. TRANSACTIONS -->
                        <li>
                            <a href="#transSub" data-bs-toggle="collapse" class="nav-btn has-submenu"><i class="fas fa-folder main-icon"></i><span class="link-text">Transactions</span></a>
                            <div class="collapse" id="transSub">
                                <ul class="list-unstyled">
                                    <li>
                                        <a href="#purEntrySub" data-bs-toggle="collapse" class="sub-group-btn has-submenu">Purchase Transactions</a>
                                        <div class="collapse" id="purEntrySub">
                                            <ul class="submenu-inner">
                                                <li id="liPurchase" runat="server"><asp:LinkButton runat="server" OnClick="btnPurchase_Click" CssClass="nav-btn">Order Creation</asp:LinkButton></li>
                                                <li id="liPurOrder" runat="server"><asp:LinkButton runat="server" OnClick="btnPurchaseOrder_Click" CssClass="nav-btn">Order Approval</asp:LinkButton></li>
                                                <li id="liPurReturn" runat="server"><asp:LinkButton runat="server" OnClick="btnPurchaseReturn_Click" CssClass="nav-btn">GRN Creation</asp:LinkButton></li>
                                                <li id="li10" runat="server"><asp:LinkButton runat="server" OnClick="btnPurchaseReturn_Click" CssClass="nav-btn">Invoice Creation</asp:LinkButton></li>
                                                <li id="li9" runat="server"><asp:LinkButton runat="server" OnClick="btnPurchaseReturn_Click" CssClass="nav-btn">Return Creation</asp:LinkButton></li>
                                            </ul>
                                        </div>
                                    </li>
                                   <li>
                                        <a href="#ProductionEntrySub" data-bs-toggle="collapse" class="sub-group-btn has-submenu">Production Transactions</a>
                                        <div class="collapse" id="ProductionEntrySub">
                                            <ul class="submenu-inner">
                                                <li id="li11" runat="server"><asp:LinkButton runat="server" OnClick="btnSales_Click" CssClass="nav-btn">Production Order Creation</asp:LinkButton></li>
                                                <li id="li12" runat="server"><asp:LinkButton runat="server" OnClick="btnSalesOrder_Click" CssClass="nav-btn">Job Work Issue</asp:LinkButton></li>
                                                <li id="li13" runat="server"><asp:LinkButton runat="server" OnClick="btnSalesReturn_Click" CssClass="nav-btn">Job Work Receipt</asp:LinkButton></li>
                                                <li id="li14" runat="server"><asp:LinkButton runat="server" OnClick="btnQuotation_Click" CssClass="nav-btn">Production Entry</asp:LinkButton></li>
                                                <li id="li15" runat="server"><asp:LinkButton runat="server" OnClick="btnQuotation_Click" CssClass="nav-btn">Quality Check & Grading Entry</asp:LinkButton></li>
                                                <li id="li16" runat="server"><asp:LinkButton runat="server" OnClick="btnQuotation_Click" CssClass="nav-btn">Wastage Entry</asp:LinkButton></li>
                                            </ul>
                                        </div>
                                    </li>
                                    <li>
                                        <a href="#BoutiqueEntrySub" data-bs-toggle="collapse" class="sub-group-btn has-submenu">Boutique - Customization</a>
                                        <div class="collapse" id="BoutiqueEntrySub">
                                            <ul class="submenu-inner">
                                                <li id="li26" runat="server"><asp:LinkButton runat="server" OnClick="btnSales_Click" CssClass="nav-btn">Measurement Chart Entry</asp:LinkButton></li>
                                                <li id="li27" runat="server"><asp:LinkButton runat="server" OnClick="btnSalesOrder_Click" CssClass="nav-btn">Order Slip / Job Card Creation</asp:LinkButton></li>
                                                <li id="li28" runat="server"><asp:LinkButton runat="server" OnClick="btnSalesReturn_Click" CssClass="nav-btn">Stitching Status Tracker</asp:LinkButton></li>
                                                <li id="li29" runat="server"><asp:LinkButton runat="server" OnClick="btnQuotation_Click" CssClass="nav-btn">Trial/Fitting Appointment</asp:LinkButton></li>
                                                <li id="li30" runat="server"><asp:LinkButton runat="server" OnClick="btnQuotation_Click" CssClass="nav-btn">Delivery & Final Payment Entry</asp:LinkButton></li>
                                            </ul>
                                        </div>
                                    </li>
                                    <li>
                                        <a href="#salesEntrySub" data-bs-toggle="collapse" class="sub-group-btn has-submenu">Sales Transactions</a>
                                        <div class="collapse" id="salesEntrySub">
                                            <ul class="submenu-inner">
                                                <li id="liSalesOrder" runat="server"><asp:LinkButton runat="server" OnClick="btnSalesOrder_Click" CssClass="nav-btn">Order Creation</asp:LinkButton></li>
                                                <li id="liSales" runat="server"><asp:LinkButton runat="server" OnClick="btnSales_Click" CssClass="nav-btn">Invoice Creation</asp:LinkButton></li>
                                                <li id="liSalesReturn" runat="server"><asp:LinkButton runat="server" OnClick="btnSalesReturn_Click" CssClass="nav-btn">Return Creation</asp:LinkButton></li>
                                                <li id="liQuotation" runat="server"><asp:LinkButton runat="server" OnClick="btnQuotation_Click" CssClass="nav-btn">Quotation Creation</asp:LinkButton></li>
                                                <li id="li17" runat="server"><asp:LinkButton runat="server" OnClick="btnQuotation_Click" CssClass="nav-btn">Delivery Note</asp:LinkButton></li>
                                            </ul>
                                        </div>
                                    </li>
                                    <li>
                                        <a href="#finEntrySub" data-bs-toggle="collapse" class="sub-group-btn has-submenu">Financials Entry</a>
                                        <div class="collapse" id="finEntrySub">
                                            <ul class="submenu-inner">
                                                <li id="liVoucher" runat="server"><asp:LinkButton runat="server" OnClick="btnMenuVoucher_Click" CssClass="nav-btn">Voucher Entry</asp:LinkButton></li>
                                                <li id="li25" runat="server"><asp:LinkButton runat="server" OnClick="btnMenuVoucher_Click" CssClass="nav-btn">Bank Reconciliation</asp:LinkButton></li>
                                            </ul>
                                        </div>
                                    </li>
                                    <li>
                                        <a href="#TranEntrySub" data-bs-toggle="collapse" class="sub-group-btn has-submenu">Stock Management</a>
                                        <div class="collapse" id="TranEntrySub">
                                            <ul class="submenu-inner">
                                                <li id="liAdjustment" runat="server"><asp:LinkButton runat="server" OnClick="btnAdjustment_Click" CssClass="nav-btn">Stock Adjustment</asp:LinkButton></li>
                                                <li id="liBTransfer" runat="server"><asp:LinkButton runat="server" OnClick="btnBTransfer_Click" CssClass="nav-btn">Godown Transfer</asp:LinkButton></li>
                                            </ul>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </li>

                        <!-- 2. MASTER SETUP -->
                        <li>
                            <a href="#masterSub" data-bs-toggle="collapse" class="nav-btn has-submenu"><i class="fas fa-folder main-icon"></i><span class="link-text">Account Master</span></a>
                            <div class="collapse" id="masterSub">
                                <ul class="submenu-inner">
                                    <li id="liLedger" runat="server"><asp:LinkButton runat="server" OnClick="btnLedgerCreation_Click" CssClass="nav-btn">Ledger Creation</asp:LinkButton></li>
                                    <li id="liCustomers" runat="server"><asp:LinkButton runat="server" OnClick="btnCustomerCreation_Click" CssClass="nav-btn">Customers Creation</asp:LinkButton></li>
                                    <li id="liSupplier" runat="server"><asp:LinkButton runat="server" OnClick="btnSupplierCreation_Click" CssClass="nav-btn">Supplier Creation</asp:LinkButton></li>
                                    <li id="liJobWork" runat="server"><asp:LinkButton runat="server" OnClick="btnJobWorkCreation_Click" CssClass="nav-btn">Job Worker Creation</asp:LinkButton></li>
                                    <li id="liGroupMaster" runat="server"><asp:LinkButton runat="server" OnClick="btnGroupMaster_Click" CssClass="nav-btn">Ledger Groups</asp:LinkButton></li>
                                    <li id="liAreaMaster" runat="server"><asp:LinkButton runat="server" OnClick="btnAreaMaster_Click" CssClass="nav-btn">Area Creation</asp:LinkButton></li>
                                    <li id="liVoucherType" runat="server"><asp:LinkButton runat="server" OnClick="btnVoucher_Click" CssClass="nav-btn">Voucher Type</asp:LinkButton></li>
                                </ul>
                            </div>
                        </li>
                        <li>
                            <a href="#invMasterSub" data-bs-toggle="collapse" class="nav-btn has-submenu"><i class="fas fa-folder main-icon"></i><span class="link-text">Inventory Master</span></a>
                            <div class="collapse" id="invMasterSub">
                                <ul class="submenu-inner">
                                    <li id="liItem" runat="server"><asp:LinkButton runat="server" OnClick="btnItemCreation_Click" CssClass="nav-btn">Item Creation</asp:LinkButton></li>
                                    <li id="liCategory" runat="server"><asp:LinkButton runat="server" OnClick="btnCategoryMaster_Click" CssClass="nav-btn">Category Creation</asp:LinkButton></li>
                                    <li id="liSubCategory" runat="server"><asp:LinkButton runat="server" OnClick="btnSubCategoryMaster_Click" CssClass="nav-btn">Sub-Category Creation</asp:LinkButton></li>
                                    <li id="liColor" runat="server"><asp:LinkButton runat="server" OnClick="btnColorMaster_Click" CssClass="nav-btn">Color Creation</asp:LinkButton></li>
                                    <li id="liUOM" runat="server"><asp:LinkButton runat="server" OnClick="btnUnitMaster_Click" CssClass="nav-btn">Units Creation</asp:LinkButton></li>
                                    <li id="liUnitConv" runat="server"><asp:LinkButton runat="server" OnClick="btnUnitConvMaster_Click" CssClass="nav-btn">Units Conversion</asp:LinkButton></li>
                                    <li id="liWeave" runat="server"><asp:LinkButton runat="server" OnClick="btnWeaveType_Click" CssClass="nav-btn">Weave Type</asp:LinkButton></li>
                                    <li id="liGSTCreation" runat="server"><asp:LinkButton runat="server" OnClick="btnGSTCreation_Click" CssClass="nav-btn">GST Creation</asp:LinkButton></li>
                                    <li id="liBarcode" runat="server"><asp:LinkButton runat="server" OnClick="btnBarcode_Click" CssClass="nav-btn">Barcode</asp:LinkButton></li>
                                    <li id="liGodown" runat="server"><asp:LinkButton runat="server" OnClick="btnGodownMaster_Click" CssClass="nav-btn">Godowns Master</asp:LinkButton></li>
                                </ul>
                            </div>
                        </li>
                        <li>
                            <a href="#genMasterSub" data-bs-toggle="collapse" class="nav-btn has-submenu"><i class="fas fa-folder main-icon"></i><span class="link-text">General Master</span></a>
                            <div class="collapse" id="genMasterSub">
                                <ul class="submenu-inner">
                                    <li id="liCompany" runat="server"><asp:LinkButton runat="server" OnClick="btnCmpyCreation_Click" CssClass="nav-btn">Company Profile</asp:LinkButton></li>
                                    <li id="liUser" runat="server"><asp:LinkButton runat="server" OnClick="btnUserMaster_Click" CssClass="nav-btn">User Management</asp:LinkButton></li>
                                    <li id="liFinancialYearMaster" runat="server"><asp:LinkButton runat="server" OnClick="btnFinancialYearMasterMaster_Click" CssClass="nav-btn">Financial Year Setup</asp:LinkButton></li>
                                    <li id="liBackupForm" runat="server"><asp:LinkButton runat="server" OnClick="btnBackupForm_Click" CssClass="nav-btn">Backup Maintance</asp:LinkButton></li>
                                </ul>
                            </div>
                        </li>

                        <!-- 3. ACCOUNTS -->
                        <li>
                            <a href="#accRepSub" data-bs-toggle="collapse" class="nav-btn has-submenu"><i class="fas fa-folder main-icon"></i><span class="link-text">Accounts Report</span></a>
                            <div class="collapse" id="accRepSub">
                                <ul class="submenu-inner">
                                    <li>
                                        <a href="#accBookSub" data-bs-toggle="collapse" class="sub-group-btn has-submenu">Account Book</a>
                                        <div class="collapse" id="accBookSub">
                                            <ul class="submenu-inner">
                                                <li id="liDayBook" runat="server"><asp:LinkButton runat="server" OnClick="btnDayBook_Click" CssClass="nav-btn">DayBook Report</asp:LinkButton></li>
                                                <li id="li22" runat="server"><asp:LinkButton runat="server" OnClick="btnDayBook_Click" CssClass="nav-btn">CashBook Report</asp:LinkButton></li>
                                                <li id="li23" runat="server"><asp:LinkButton runat="server" OnClick="btnDayBook_Click" CssClass="nav-btn">BankBook Report</asp:LinkButton></li>
                                            </ul>
                                        </div>
                                    </li>
                                    <li id="liLedgerRep" runat="server"><asp:LinkButton runat="server" OnClick="btnLedger_Click" CssClass="nav-btn">Ledger Statement</asp:LinkButton></li>
                                    <li id="liCashBank" runat="server"><asp:LinkButton runat="server" OnClick="btnCashBank_Click" CssClass="nav-btn">Cash/Bank Book</asp:LinkButton></li>
                                    <li id="liTrial" runat="server"><asp:LinkButton runat="server" OnClick="btnTrialBalance_Click" CssClass="nav-btn">Trial Balance</asp:LinkButton></li>
                                    <li id="liBS" runat="server"><asp:LinkButton runat="server" OnClick="btnBalanceSheet_Click" CssClass="nav-btn">Balance Sheet</asp:LinkButton></li>
                                    <li id="liPL" runat="server"><asp:LinkButton runat="server" OnClick="btnProfitLoss_Click" CssClass="nav-btn">Profit & Loss</asp:LinkButton></li>
                                    <li>
                                        <a href="#accOutstandingSub" data-bs-toggle="collapse" class="sub-group-btn has-submenu">Outstanding Report</a>
                                        <div class="collapse" id="accOutstandingSub">
                                            <ul class="submenu-inner">
                                                <li id="li31" runat="server"><asp:LinkButton runat="server" OnClick="btnGSTR1_Click" CssClass="nav-btn">Receivables</asp:LinkButton></li>
                                                <li id="li32" runat="server"><asp:LinkButton runat="server" OnClick="btnGSTR2_Click" CssClass="nav-btn">Payables</asp:LinkButton></li>
                                            </ul>
                                        </div>
                                    </li>
                                    <li>
                                        <a href="#accGSTSub" data-bs-toggle="collapse" class="sub-group-btn has-submenu">GST Report</a>
                                        <div class="collapse" id="accGSTSub">
                                            <ul class="submenu-inner">
                                                <li id="liGSTR1" runat="server"><asp:LinkButton runat="server" OnClick="btnGSTR1_Click" CssClass="nav-btn">GSTR-1 (Sales) Report</asp:LinkButton></li>
                                                <li id="liGSTR2" runat="server"><asp:LinkButton runat="server" OnClick="btnGSTR2_Click" CssClass="nav-btn">GSTR-2 (Purchase) Report</asp:LinkButton></li>
                                                <li id="liGSTR3B" runat="server"><asp:LinkButton runat="server" OnClick="btnGSTR3B_Click" CssClass="nav-btn">GSTR-3B Summary</asp:LinkButton></li>
                                                <li id="liHSN" runat="server"><asp:LinkButton runat="server" OnClick="btnHSNSummary_Click" CssClass="nav-btn">HSN Summary Report</asp:LinkButton></li>
                                            </ul>
                                        </div>
                                    </li>
                                </ul>
                            </div>
                        </li>

                        <!-- 4. REPORTS -->
                        <li>
                            <a href="#gstRepSub" data-bs-toggle="collapse" class="nav-btn has-submenu"><i class="fas fa-folder main-icon"></i><span class="link-text">Inventory Reports</span></a>
                            <div class="collapse" id="gstRepSub">
                                <ul class="submenu-inner">
                                    <li id="liStockSum" runat="server"><asp:LinkButton runat="server" OnClick="btnStockSummary_Click" CssClass="nav-btn">Stock Summary</asp:LinkButton></li>
                                    <li id="liStockDet" runat="server"><asp:LinkButton runat="server" OnClick="btnStockDetail_Click" CssClass="nav-btn">Sales Register</asp:LinkButton></li>
                                    <li id="li21" runat="server"><asp:LinkButton runat="server" OnClick="btnStockDetail_Click" CssClass="nav-btn">Purchase Register</asp:LinkButton></li>
                                    <li id="li20" runat="server"><asp:LinkButton runat="server" OnClick="btnStockDetail_Click" CssClass="nav-btn">Job Work Status Report</asp:LinkButton></li>
                                </ul>
                            </div>
                        </li>

                        <!-- SETTINGS -->
                        <li>
                            <a href="#setSub" data-bs-toggle="collapse" class="nav-btn has-submenu"><i class="fas fa-folder main-icon"></i><span class="link-text">Settings</span></a>
                            <div class="collapse" id="setSub">
                                <ul class="submenu-inner">
                                    <li id="liCompSett" runat="server"><asp:LinkButton runat="server" OnClick="btnCompSett_Click" CssClass="nav-btn">Company Setting</asp:LinkButton></li>
                                    <li id="li1" runat="server"><asp:LinkButton runat="server" OnClick="btnCompSett_Click" CssClass="nav-btn">General Setting</asp:LinkButton></li>
                                    <li id="liItemSett" runat="server"><asp:LinkButton runat="server" OnClick="btnitemSett_Click" CssClass="nav-btn">Item Setting</asp:LinkButton></li>
                                    <li id="liFormSet" runat="server"><asp:LinkButton runat="server" OnClick="btnFormSet_Click" CssClass="nav-btn">Form Setup</asp:LinkButton></li>
                                </ul>
                            </div>
                        </li>
                    </ul>
                </div>
            </nav>

            <div id="content">
                <nav class="navbar shadow-sm">
                    <button type="button" class="hamburger-btn me-3" onclick="toggleSidebar()"><i class="fas fa-bars"></i></button>
                    <div class="flex-grow-1"><h5 class="m-0 fw-bold text-dark"><asp:Literal ID="litCompName" runat="server" /></h5></div>
                    <div class="d-flex align-items-center gap-1">
                        <asp:LinkButton ID="LinkActivity" runat="server" OnClick="btnActivity_Click" CssClass="icon-btn"><i class="bi bi-bell"></i></asp:LinkButton>

                        <div class="dropdown ms-1">
                            <button type="button" class="avatar-btn" data-bs-toggle="dropdown" aria-expanded="false">
                                <div class="avatar-circle" id="spnUserInitials">
                                    <asp:Literal ID="litUserInitials" runat="server" />
                                    <asp:PlaceHolder ID="phHeaderIcon" runat="server">
                                        <i class="fas fa-user"></i>
                                    </asp:PlaceHolder>
                                </div>
                            </button>
                            <div class="dropdown-menu dropdown-menu-end profile-menu p-0">
                                <div class="profile-menu-header">
                                    <div class="avatar-circle avatar-circle-lg">
                                        <asp:Literal ID="litUserInitialsLg" runat="server" />
                                        <asp:PlaceHolder ID="phMenuIcon" runat="server">
                                            <i class="fas fa-user"></i>
                                        </asp:PlaceHolder>
                                    </div>
                                        <div class="profile-menu-name"><asp:Literal ID="litUsername" runat="server" /></div>
                                        <div class="profile-menu-email"><asp:Literal ID="litUserEmail" runat="server" /></div>
                                </div>
                                <ul class="list-unstyled mb-0 py-2">
                                    <li>
                                        <asp:LinkButton ID="lnkMyProfile" runat="server" OnClick="btnMyProfile_Click" CssClass="profile-menu-item">
                                            <span><i class="bi bi-person-badge me-2"></i> My Profile</span>
                                            <i class="bi bi-chevron-right small text-muted"></i>
                                        </asp:LinkButton>
                                    </li>
                                    <li class="dropdown-divider"></li>
                                    <li>
                                        <asp:LinkButton ID="lnkLogout" runat="server" OnClick="btnLogout_Click" CssClass="profile-menu-item text-danger">
                                            <span><i class="bi bi-box-arrow-right me-2"></i> Log Out</span>
                                        </asp:LinkButton>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </nav>
                <div class="report-wrapper">
                    <asp:Panel ID="pnlReportArea" runat="server" CssClass="report-container">
                        <asp:Panel ID="pnlWelcome" runat="server" CssClass="welcome-screen">
                            <canvas id="bg-canvas"></canvas>
                            <div style="position:relative; z-index:2;">
                                <h1 class="fw-bold text-dark">Welcome, <asp:Literal ID="litWelcomeUser" runat="server" />!</h1>
                                <p class="text-muted">Smart Billing & Multi-Tenant Inventory Solution</p>
                            </div>
                        </asp:Panel>
                        <iframe id="ifrReport" runat="server" style="display:none;" class="h-100 w-100 border-0"></iframe>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function toggleSidebar() {
            $('#sidebar').toggleClass('active');
            $('#wrapper').toggleClass('mobile-active');
            $('.brand-full').toggleClass('d-none');
            $('.brand-short').toggleClass('d-none');
            $('.side-tooltip').remove();
        }

        $(document).ready(function () {
            $('#spnUserInitialsLg').text($('#spnUserInitials').text().trim());

            $('.nav-btn').on('click', function () {
                $('.nav-btn').removeClass('active-link');
                $(this).addClass('active-link');
                if ($(window).width() <= 768) toggleSidebar();
            });

            $('#menuSearch').on('keyup', function () {
                var val = $(this).val().toLowerCase();
                if (val === "") { $("#mainMenu li").show(); $(".collapse").collapse('hide'); return; }
                $("#mainMenu li").hide();
                $("#mainMenu li a, #mainMenu li button").each(function () {
                    if ($(this).text().toLowerCase().indexOf(val) > -1) {
                        $(this).parents("li").show();
                        $(this).parents(".collapse").collapse('show');
                    }
                });
            });

            var $tooltip = null;
            $('#sidebar').on('mouseenter', '.nav-btn', function () {
                if (!$('#sidebar').hasClass('active')) return;
                var label = $(this).find('span.link-text').first().text().trim();
                if (!label) return;
                if ($tooltip) $tooltip.remove();
                $tooltip = $('<div class="side-tooltip"></div>').text(label);
                $('body').append($tooltip);
                var rect = this.getBoundingClientRect();
                $tooltip.css({
                    top: (rect.top + rect.height / 2) + 'px',
                    left: (rect.right + 12) + 'px'
                });
                requestAnimationFrame(function () { $tooltip && $tooltip.addClass('show'); });
            }).on('mouseleave', '.nav-btn', function () {
                if ($tooltip) { $tooltip.remove(); $tooltip = null; }
            });

            const canvas = document.getElementById('bg-canvas');
            if (canvas) {
                const ctx = canvas.getContext('2d');
                let particles = [];
                function resize() { canvas.width = canvas.parentElement.offsetWidth; canvas.height = canvas.parentElement.offsetHeight; }
                window.addEventListener('resize', resize); resize();
                class P {
                    constructor() { this.x = Math.random() * canvas.width; this.y = Math.random() * canvas.height; this.s = Math.random() * 2 + 1; this.vx = Math.random() * 0.4 - 0.2; this.vy = Math.random() * 0.4 - 0.2; }
                    u() { this.x += this.vx; this.y += this.vy; if (this.x > canvas.width) this.x = 0; if (this.y > canvas.height) this.y = 0; }
                    d() { ctx.fillStyle = 'rgba(37,99,235,0.12)'; ctx.beginPath(); ctx.arc(this.x, this.y, this.s, 0, Math.PI * 2); ctx.fill(); }
                }
                for (let i = 0; i < 40; i++) particles.push(new P());
                function anim() { ctx.clearRect(0, 0, canvas.width, canvas.height); particles.forEach(p => { p.u(); p.d(); }); requestAnimationFrame(anim); }
                anim();
            }

        });
    </script>
</body>
</html>

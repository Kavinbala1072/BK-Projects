<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MainForm.aspx.cs" Inherits="BKSoftwares.MainForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dashboard | BK Softwares</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
<style>
    :root {
        --sidebar-bg: #0f172a; --sidebar-hover: #1e293b; --primary-accent: #6366f1;
        --text-muted: #94a3b8; --navbar-bg: #ffffff; --body-bg: #f8fafc; --transition-speed: 0.3s;
    }
    body, html { height: 100%; margin: 0; background-color: var(--body-bg); font-family: 'Inter', sans-serif; overflow: hidden; }
    #wrapper { display: flex; width: 100%; height: 100vh; align-items: stretch; position: relative; }
    
    /* --- SIDEBAR --- */
    #sidebar { 
        min-width: 260px; max-width: 260px; 
        background: var(--sidebar-bg); color: #fff; 
        transition: all var(--transition-speed) ease-in-out; 
        z-index: 2000; display: flex; flex-direction: column; 
        box-shadow: 4px 0 10px rgba(0,0,0,0.1); 
    }

    @media (min-width: 769px) {
        #sidebar.active { min-width: 80px; max-width: 80px; }
        #sidebar.active .sidebar-header h3, #sidebar.active .nav-btn span, #sidebar.active .menu-header { display: none !important; }
        #sidebar.active .sidebar-header { justify-content: center; padding: 0; }
        #sidebar.active .sidebar-header::after { content: 'BK'; font-weight: 800; color: var(--primary-accent); font-size: 1.2rem; }
        #sidebar.active .nav-btn { justify-content: center; padding: 15px 0; }
        #sidebar.active .nav-btn i { margin-right: 0; font-size: 1.3rem; }
    }

    @media (max-width: 768px) {
        #sidebar { position: fixed; left: -260px; height: 100%; }
        #sidebar.active { left: 0; }
        #wrapper.mobile-active::after {
            content: ""; position: fixed; top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(0,0,0,0.5); z-index: 1500;
        }
    }

    .sidebar-header { height: 65px; display: flex; align-items: center; padding: 0 20px; background: #020617; border-bottom: 1px solid #1e293b; overflow: hidden; flex-shrink: 0; }
    .sidebar-header h3 { font-size: 1.1rem; font-weight: 700; color: #fff; margin: 0; white-space: nowrap; }
    
    .nav-btn { padding: 12px 20px; display: flex; align-items: center; color: var(--text-muted); text-decoration: none; background: none; width: 100%; text-align: left; border: none; transition: 0.2s; white-space: nowrap; border-left: 4px solid transparent; }
    .nav-btn:hover { color: #fff; background: var(--sidebar-hover); border-left: 4px solid var(--primary-accent); }
    .nav-btn i { width: 25px; font-size: 1.1rem; margin-right: 12px; text-align: center; }
    .menu-header { font-size: 0.65rem; text-transform: uppercase; letter-spacing: 1.2px; font-weight: 700; color: #475569; padding: 20px 20px 8px 20px; display: block; }
    
    /* --- CONTENT --- */
    #content { flex-grow: 1; display: flex; flex-direction: column; min-width: 0; height: 100vh; position: relative; }
    .navbar { height: 65px; background: var(--navbar-bg); border-bottom: 1px solid #e2e8f0; padding: 0 15px; display: flex; align-items: center; z-index: 1000; }
    .report-wrapper { flex-grow: 1; padding: 15px; display: flex; flex-direction: column; overflow: hidden; }
    .report-container { background-color: #ffffff; border-radius: 12px; box-shadow: 0 4px 6px rgba(0,0,0,0.05); border: 1px solid #e2e8f0; flex-grow: 1; display: flex; overflow: hidden; position: relative; }
    .report-frame { width: 100%; height: 100%; border: none; }
    
    .welcome-screen { position: relative; display: flex; flex-direction: column; align-items: center; justify-content: center; width: 100%; height: 100%; text-align: center; background: #fff; }
    #bg-canvas { position: absolute; top: 0; left: 0; width: 100%; height: 100%; z-index: 1; }
    .welcome-content { position: relative; z-index: 2; pointer-events: none; }
    .welcome-icon { font-size: 4rem; color: var(--primary-accent); margin-bottom: 20px; }
</style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper">
            <nav id="sidebar">
                <div class="sidebar-header">
                    <h3 class="fw-bold text-white">BK Softwares</h3>
                </div>
                <div class="flex-grow-1 mt-2 overflow-auto">
                    <ul class="list-unstyled">
                        <li><asp:LinkButton ID="btnMenuDash" runat="server" CssClass="nav-btn menu-link" OnClick="btnMenuDash_Click"><i class="fas fa-tachometer-alt"></i> <span>Dashboard</span></asp:LinkButton></li>
                        <li class="menu-header">Master</li>
                        <li><asp:LinkButton ID="btnMenuCust" runat="server" CssClass="nav-btn menu-link" OnClick="btnMenuCust_Click"><i class="fas fa-user-plus"></i> <span>Customer Creation</span></asp:LinkButton></li>
                        <li><asp:LinkButton ID="btnReporting" runat="server" CssClass="nav-btn menu-link" OnClick="btnRLogin_Click"><i class="fas fa-user-plus"></i> <span>Reporting Users</span></asp:LinkButton></li>
                        <li><asp:LinkButton ID="btnKey" runat="server" CssClass="nav-btn menu-link" OnClick="btnKey_Click"><i class="fas fa-key"></i> <span>Key Activation</span></asp:LinkButton></li>
                        <li><asp:LinkButton ID="btnMenuVoucher" runat="server" CssClass="nav-btn menu-link" OnClick="btnMenuVoucher_Click"><i class="fas fa-receipt"></i> <span>Voucher Entry</span></asp:LinkButton></li>
                        <li class="menu-header">Reports</li>
                        <li><asp:LinkButton ID="btnRepCust" runat="server" CssClass="nav-btn menu-link" OnClick="btnRepCust_Click"><i class="fas fa-chart-line"></i> <span>Usage Analysis</span></asp:LinkButton></li>
                        <li><asp:LinkButton ID="btnoutstand" runat="server" CssClass="nav-btn menu-link" OnClick="btnOutstanding_Click"><i class="fas fa-hand-holding-usd"></i> <span>Outstanding</span></asp:LinkButton></li>
                    </ul>
                </div>
            </nav>

            <div id="content">
                <nav class="navbar shadow-sm">
                    <button type="button" id="sidebarCollapse" class="btn btn-light border me-3"><i class="fas fa-bars"></i></button>
                    <div class="flex-grow-1"><h5 class="m-0 fw-bold text-dark d-none d-sm-block"><asp:Literal ID="litCompName" runat="server" /></h5></div>
                    <div class="d-flex align-items-center">
                        <div class="me-3 d-none d-md-block text-end">
                            <small class="text-muted d-block" style="font-size: 0.65rem;">System Users</small>
                            <strong class="small"><asp:Literal ID="litUsername" runat="server" /></strong>
                        </div>
                        <asp:LinkButton ID="lnkLogout" runat="server" OnClick="btnLogout_Click" OnClientClick="logoutUser();" CssClass="btn btn-sm btn-danger rounded-pill px-3">
                            <i class="fas fa-power-off me-1"></i> Logout
                        </asp:LinkButton>
                    </div>
                </nav>

                <div class="report-wrapper">
                    <asp:Panel ID="pnlReportArea" runat="server" CssClass="report-container">
                        <asp:Panel ID="pnlWelcome" runat="server" CssClass="welcome-screen">
                            <canvas id="bg-canvas"></canvas>
                            <div class="welcome-content">
                                <i class="fas fa-rocket welcome-icon"></i>
                                <h1 class="fw-bold">Welcome, <asp:Literal ID="litWelcomeUser" runat="server" />!</h1>
                                <%--<p class="text-muted"></p>--%>
                            </div>
                        </asp:Panel>
                        <iframe id="ifrReport" runat="server" class="report-frame"></iframe>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };

        function logoutUser() {
            localStorage.removeItem("BK_AuthToken");
            localStorage.removeItem("BK_TokenExpiry");
            localStorage.removeItem("BK_UserName");
        }

        function checkAuth() {
            var token = localStorage.getItem("BK_AuthToken");
            var expiryStr = localStorage.getItem("BK_TokenExpiry");

            if (!token || !expiryStr) {
                window.location.replace("AppLogin.aspx");
                return;
            }

            var expiryDate = new Date(expiryStr);
            if (new Date() > expiryDate) {
                alert("Session expired. Please login again.");
                logoutUser();
                window.location.replace("AppLogin.aspx");
            }
        }

        $(document).ready(function () {
            checkAuth();

            if (window.history && window.history.pushState) {
                window.history.pushState('forward', null, './MainForm.aspx');
                $(window).on('popstate', function () {
                    window.location.href = 'MainForm.aspx';
                });
            }

            $('#sidebarCollapse').on('click', function () {
                $('#sidebar').toggleClass('active');
                if ($(window).width() <= 768) { $('#wrapper').toggleClass('mobile-active'); }
            });

            $('.menu-link').on('click', function () {
                if ($(window).width() <= 768) {
                    $('#sidebar').removeClass('active');
                    $('#wrapper').removeClass('mobile-active');
                }
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
                    d() { ctx.fillStyle = 'rgba(99,102,241,0.3)'; ctx.beginPath(); ctx.arc(this.x, this.y, this.s, 0, Math.PI * 2); ctx.fill(); }
                }
                for (let i = 0; i < 50; i++) particles.push(new P());
                function anim() { ctx.clearRect(0, 0, canvas.width, canvas.height); particles.forEach(p => { p.u(); p.d(); }); requestAnimationFrame(anim); }
                anim();
            }
        });
    </script>
</body>
</html>
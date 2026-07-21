<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MainForm.aspx.vb" Inherits="WebApplication1.MainForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporting - Enterprise Analytics</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    <style>
        :root {
            --sidebar-bg: #0f172a;
            --sidebar-hover: #1e293b;
            --primary-accent: #6366f1;
            --text-muted: #94a3b8;
            --navbar-bg: #ffffff;
            --body-bg: #f8fafc;
            --transition-speed: 0.3s;
        }

        body, html { 
            height: 100%; margin: 0; 
            background-color: var(--body-bg); 
            font-family: 'Inter', sans-serif; 
            overflow: hidden; 
        }

        #wrapper { display: flex; width: 100%; height: 100vh; align-items: stretch; position: relative; }

        /* --- SIDEBAR STYLE --- */
        #sidebar { 
            min-width: 260px; max-width: 260px; 
            background: var(--sidebar-bg); color: #fff; 
            transition: all var(--transition-speed) cubic-bezier(0.4, 0, 0.2, 1); 
            z-index: 2000; display: flex; flex-direction: column;
            box-shadow: 4px 0 10px rgba(0,0,0,0.1);
        }

        /* Large Screen Collapsed State */
        @media (min-width: 769px) {
            #sidebar.active { min-width: 70px; max-width: 70px; }
            #sidebar.active .sidebar-header h3, 
            #sidebar.active .nav-btn span { display: none; }
            #sidebar.active .sidebar-header { justify-content: center; padding: 0; }
            #sidebar.active .sidebar-header::after { content: 'BK'; font-weight: 800; color: var(--primary-accent); }
            #sidebar.active .nav-btn { justify-content: center; padding: 18px 0; }
            #sidebar.active .nav-btn i { margin: 0; }
        }

        .sidebar-header { 
            height: 65px; display: flex; align-items: center; padding: 0 20px; 
            background: #020617; border-bottom: 1px solid #1e293b; 
            overflow: hidden; flex-shrink: 0;
        }
        
        .sidebar-header h3 { font-size: 1.1rem; font-weight: 700; color: #fff; margin: 0; white-space: nowrap; }
        .mobile-close { display: none; background: none; border: none; color: white; font-size: 1.2rem; }

        .nav-btn { 
            padding: 14px 20px; display: flex; align-items: center; 
            color: var(--text-muted); text-decoration: none; 
            background: none; width: 100%; text-align: left; border: none;
            transition: all 0.2s; white-space: nowrap; border-left: 3px solid transparent;
        }
        .nav-btn:hover { color: #fff; background: var(--sidebar-hover); border-left: 3px solid var(--primary-accent); }
        .nav-btn i { width: 30px; font-size: 1.1rem; flex-shrink: 0; }
        .nav-btn span { font-weight: 500; font-size: 0.95rem; }

        /* --- CONTENT AREA --- */
        #content { flex-grow: 1; display: flex; flex-direction: column; min-width: 0; height: 100vh; }
        
        .navbar { 
            height: 65px; background: var(--navbar-bg); border-bottom: 1px solid #e2e8f0; 
            padding: 0 15px; display: flex; align-items: center; z-index: 1000;
        }

        .comp-name-display { 
            font-size: 0.95rem; font-weight: 700; color: #0f172a;
            white-space: nowrap; overflow: hidden; text-overflow: ellipsis;
            margin-right: 10px;
        }

        .report-wrapper { flex-grow: 1; padding: 15px; display: flex; flex-direction: column; overflow: hidden; }
        .report-container { background-color: #ffffff; border-radius: 12px; box-shadow: 0 4px 6px rgba(0,0,0,0.05); border: 1px solid #e2e8f0; flex-grow: 1; display: flex; overflow: hidden; position: relative; }
        .report-frame { width: 100%; height: 100%; border: none; display: block; }

        /* --- WELCOME SCREEN --- */
        .welcome-screen { position: relative; display: flex; flex-direction: column; align-items: center; justify-content: center; width: 100%; height: 100%; text-align: center; padding: 40px; background: #ffffff; overflow: hidden; }
        #bg-canvas { position: absolute; top: 0; left: 0; width: 100%; height: 100%; z-index: 1; }
        .welcome-content { position: relative; z-index: 2; pointer-events: none; }
        .welcome-icon { font-size: 4rem; color: var(--primary-accent); margin-bottom: 20px; opacity: 0.8; }
        .welcome-screen h1 { font-weight: 800; color: #0f172a; margin-bottom: 10px; font-size: 1.5rem; }
        .welcome-screen p { color: #64748b; max-width: 500px; font-size: 1rem; }

        .btn-toggle { background: #f1f5f9; color: #475569; border: 1px solid #e2e8f0; border-radius: 8px; padding: 8px 12px; margin-right: 12px; flex-shrink: 0;}

        /* --- SMALL SCREEN CORRECTIONS --- */
        @media (max-width: 768px) {
            #sidebar { 
                position: fixed; 
                left: -260px; /* Hidden off-screen */
                height: 100vh; 
                box-shadow: none;
            }
            #sidebar.active { 
                left: 0; 
                box-shadow: 10px 0 30px rgba(0,0,0,0.3);
            }
            .mobile-close { display: block; }
            
            /* Background Overlay */
            #wrapper.mobile-active::after { 
                content: ""; position: fixed; top: 0; left: 0; right: 0; bottom: 0; 
                background: rgba(15, 23, 42, 0.5); z-index: 1040; backdrop-filter: blur(2px); 
            }

            .comp-name-display { font-size: 0.8rem; max-width: 200px; }
            .report-wrapper { padding: 10px; }
            .navbar { padding: 0 10px; }
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="wrapper">
            <!-- Sidebar -->
            <nav id="sidebar">
                <div class="sidebar-header">
                    <h3 class="fw-bold text-white"><i class="fas fa-chart-pie me-2"></i>BK Reporting</h3>
                    <button type="button" class="mobile-close" id="btnCloseSidebar"><i class="fas fa-times"></i></button>
                </div>
                <div class="flex-grow-1 mt-2 overflow-auto">
                    <ul class="list-unstyled">
                        <li><asp:LinkButton ID="btnMenuDash" runat="server" CssClass="nav-btn menu-link" OnClick="btnMenuDash_Click"><i class="fas fa-th-large"></i> <span>Dashboard</span></asp:LinkButton></li>
                        <li><asp:LinkButton ID="btnMenuPayable" runat="server" CssClass="nav-btn menu-link" OnClick="btnMenuOutstandingPayable_Click"><i class="fas fa-file-invoice-dollar"></i> <span>Outstanding</span></asp:LinkButton></li>
                        <li><asp:LinkButton ID="btnMenuLedger" runat="server" CssClass="nav-btn menu-link" OnClick="btnMenuLedger_Click"><i class="fas fa-book"></i> <span>Ledger Reports</span></asp:LinkButton></li>
                        <li><asp:LinkButton ID="btnMenuStock" runat="server" CssClass="nav-btn menu-link" OnClick="btnMenuStock_Click"><i class="fas fa-boxes"></i> <span>Stock Summary</span></asp:LinkButton></li>
                        <li><asp:LinkButton ID="btnMenuSales" runat="server" CssClass="nav-btn menu-link" OnClick="btnMenuSales_Click"><i class="fas fa-shopping-cart"></i> <span>Sales Report</span></asp:LinkButton></li>
                        <li><asp:LinkButton ID="btnMenuPurchase" runat="server" CssClass="nav-btn menu-link" OnClick="btnMenuPurchase_Click"><i class="fas fa-truck-loading"></i> <span>Purchase Report</span></asp:LinkButton></li>
                        <li><asp:LinkButton ID="btnLedger" runat="server" CssClass="nav-btn menu-link" OnClick="btnLedger_Click"><i class="fas fa-address-card"></i> <span>Ledger Details</span></asp:LinkButton></li>
                        <li><asp:LinkButton ID="btnItem" runat="server" CssClass="nav-btn menu-link" OnClick="btnItem_Click"><i class="fas fa-barcode"></i> <span>Item Details</span></asp:LinkButton></li>                        
                    </ul>
                </div>
            </nav>

            <!-- Main Content Area -->
            <div id="content">
                <nav class="navbar">
                    <div class="container-fluid p-0 d-flex align-items-center">
                        <button type="button" id="sidebarCollapse" class="btn btn-toggle"><i class="fas fa-bars"></i></button>
                        
                        <div class="comp-name-display flex-grow-1">
                            <asp:Literal ID="litCompName" runat="server"></asp:Literal>
                        </div>

                        <div class="d-flex align-items-center">
                            <div class="welcome-msg me-2 d-none d-lg-block">
                                <span class="text-muted small">User:</span> <strong class="small text-dark"><asp:Literal ID="litUsername" runat="server"></asp:Literal></strong>
                            </div>
                            <asp:LinkButton ID="lnkCompany" runat="server" OnClick="lnkCompany_Click" CssClass="btn btn-sm btn-outline-primary border-0 me-1 fw-semibold">
                                 <i class="fas fa-sync-alt"></i><span class="d-none d-md-inline"> Switch</span>
                            </asp:LinkButton>
                            <asp:LinkButton ID="lnkLogout" runat="server" OnClick="btnLogout_Click" CssClass="btn btn-sm btn-danger rounded-circle" style="width:32px; height:32px; padding:0; display:flex; align-items:center; justify-content:center;">
                                <i class="fas fa-power-off" style="font-size: 0.8rem;"></i>
                            </asp:LinkButton>
                        </div>
                    </div>
                </nav>

                <div class="report-wrapper">
                    <asp:Panel ID="pnlReportArea" runat="server" CssClass="report-container">
                        <asp:Panel ID="pnlWelcome" runat="server" CssClass="welcome-screen">
                            <canvas id="bg-canvas"></canvas>
                            <div class="welcome-content">
                                <i class="fas fa-chart-line welcome-icon"></i>
                                <h1>Welcome, <asp:Literal ID="litWelcomeUser" runat="server" />!</h1>
                                <p>Select a module from the menu to start viewing your reports.</p>
                            </div>
                        </asp:Panel>
                        <iframe id="ifrReport" runat="server" class="report-frame"></iframe>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function () {
            function toggleSidebar() {
                $('#sidebar').toggleClass('active');
                if ($(window).width() <= 768) {
                    $('#wrapper').toggleClass('mobile-active');
                }
            }

            $('#sidebarCollapse, #btnCloseSidebar').on('click', function () {
                toggleSidebar();
            });

            $('.menu-link').on('click', function () {
                if ($(window).width() <= 768) {
                    $('#sidebar').removeClass('active');
                    $('#wrapper').removeClass('mobile-active');
                }
            });

            $(document).on('click', '#wrapper.mobile-active', function (e) {
                if (e.target.id === 'wrapper') {
                    $('#sidebar').removeClass('active');
                    $('#wrapper').removeClass('mobile-active');
                }
            });

            const canvas = document.getElementById('bg-canvas');
            if (canvas) {
                const ctx = canvas.getContext('2d');
                let particles = [];
                function resizeCanvas() {
                    canvas.width = canvas.parentElement.offsetWidth;
                    canvas.height = canvas.parentElement.offsetHeight;
                }
                window.addEventListener('resize', resizeCanvas);
                resizeCanvas();

                class Particle {
                    constructor() {
                        this.x = Math.random() * canvas.width;
                        this.y = Math.random() * canvas.height;
                        this.size = Math.random() * 2 + 1;
                        this.speedX = Math.random() * 0.4 - 0.2;
                        this.speedY = Math.random() * 0.4 - 0.2;
                        this.opacity = Math.random() * 0.3 + 0.1;
                    }
                    update() {
                        this.x += this.speedX; this.y += this.speedY;
                        if (this.x > canvas.width) this.x = 0; if (this.x < 0) this.x = canvas.width;
                        if (this.y > canvas.height) this.y = 0; if (this.y < 0) this.y = canvas.height;
                    }
                    draw() {
                        ctx.fillStyle = `rgba(99, 102, 241, ${this.opacity})`;
                        ctx.beginPath(); ctx.arc(this.x, this.y, this.size, 0, Math.PI * 2); ctx.fill();
                    }
                }
                for (let i = 0; i < 50; i++) particles.push(new Particle());
                function animate() {
                    ctx.clearRect(0, 0, canvas.width, canvas.height);
                    particles.forEach(p => { p.update(); p.draw(); });
                    requestAnimationFrame(animate);
                }
                animate();
            }
        });
    </script>
</body>
</html>
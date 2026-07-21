<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="WebApplication1._Default" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Reporting - Secure Access</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800&display=swap" rel="stylesheet" />
    <style>
        :root {
            --primary-color: #6366f1;
            --primary-hover: #4f46e5;
            --dark-bg: #0f172a;
            --card-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1), 0 10px 10px -5px rgba(0, 0, 0, 0.04);
            --border-color: #e2e8f0;
        }

        body, html { 
            height: 100%; margin: 0; 
            background-color: #f8fafc; 
            font-family: 'Inter', -apple-system, sans-serif; 
            -webkit-font-smoothing: antialiased;
            overflow-x: hidden;
        }

        #bg-canvas {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            z-index: 1;
            pointer-events: none;
        }

        .main-wrapper { 
            display: flex; align-items: center; justify-content: center; 
            min-height: 100vh; padding: 1.5rem; 
            background-image: radial-gradient(circle at top right, #eef2ff 0%, #f8fafc 50%);
            position: relative;
        }

        .config-card { 
            position: relative;
            z-index: 10;
            border: 1px solid var(--border-color); 
            border-radius: 1.5rem; 
            box-shadow: var(--card-shadow); 
            overflow: hidden; width: 100%; 
            max-width: 450px; background: white; 
            transition: transform 0.3s ease;
        }

        .card-header { 
            background: var(--dark-bg); 
            color: white; padding: 2.5rem 2rem; 
            text-align: center; border: none;
        }

        .setup-label { 
            font-size: 0.65rem; font-weight: 700; 
            color: #64748b; text-transform: uppercase; 
            letter-spacing: 0.05em; display: block; 
            margin-bottom: 0.5rem; 
        }

        .btn-action { 
            background: var(--primary-color); 
            color: white; border: none; 
            padding: 12px; font-weight: 600; 
            border-radius: 0.75rem; transition: all 0.2s; 
            width: 100%; text-transform: none;
            font-size: 0.95rem;
        }
        .btn-action:hover { 
            background: var(--primary-hover); 
            transform: translateY(-1px); 
            box-shadow: 0 4px 12px rgba(99, 102, 241, 0.3);
            color: #fff; 
        }

        .otp-display { 
            background: #fffbeb; border: 2px dashed #f59e0b; 
            border-radius: 1rem; padding: 1.25rem; 
            text-align: center; margin-bottom: 1.5rem; 
        }
        .otp-code { 
            font-size: 2.5rem; font-weight: 800; 
            letter-spacing: 12px; color: #92400e; 
            display: block; font-family: monospace;
            margin-right: -12px;
        }

        .input-group-text { 
            background-color: #f1f5f9; 
            border: 1px solid var(--border-color);
            border-right: none; color: #94a3b8; 
            border-radius: 0.75rem 0 0 0.75rem;
            width: 45px; justify-content: center;
        }

        .form-control, .form-select { 
            border: 1px solid var(--border-color);
            padding: 10px 12px;
            font-size: 0.95rem;
            border-radius: 0.75rem;
        }
        
        .input-group .form-control {
            border-left: none;
            border-radius: 0 0.75rem 0.75rem 0;
        }

        .form-control:focus, .form-select:focus {
            border-color: var(--primary-color);
            box-shadow: 0 0 0 3px rgba(99, 102, 241, 0.1);
        }

        /* Responsive Tweaks */
        @media (max-width: 576px) {
            .main-wrapper { padding: 1rem; }
            .card-header { padding: 2rem 1.5rem; }
            .config-card { border-radius: 1rem; }
            .otp-code { font-size: 2rem; letter-spacing: 8px; }
        }

        /* Error Message Styling */
        #lblMsg { font-size: 0.85rem; font-weight: 500; }
        .text-danger { color: #ef4444 !important; }
        .text-success { color: #10b981 !important; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="main-wrapper">
            <!-- Background Animation Canvas -->
            <canvas id="bg-canvas"></canvas>

            <div class="config-card">
                <div class="card-header">
                    <div class="mb-3">
                        <span class="p-3 rounded-circle bg-white bg-opacity-10 d-inline-block">
                            <i class="fas fa-chart-pie fa-2x text-white"></i>
                        </span>
                    </div>
                    <h4 class="mb-1 fw-bold">Reporting Login</h4>
                    <p class="text-white text-opacity-50 small mb-0">Secure Web Access</p>
                </div>
                
                <div class="card-body p-4 p-sm-5">
                    <asp:Label ID="lblMsg" runat="server" CssClass="d-block mb-3"></asp:Label>

                    <!-- STEP 1: LOGIN -->
                    <asp:Panel ID="pnlLogin" runat="server">
                        <div class="mb-3">
                            <label class="setup-label">Identity Code</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="fas fa-user-circle"></i></span>
                                <asp:TextBox ID="txtUserCode" runat="server" CssClass="form-control" placeholder="Enter User ID"></asp:TextBox>
                            </div>
                        </div>
                        <div class="mb-4">
                            <label class="setup-label">Security Key</label>
                            <div class="input-group">
                                <span class="input-group-text"><i class="fas fa-key"></i></span>
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter Password"></asp:TextBox>
                            </div>
                        </div>
                        <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" CssClass="btn-action" />
                    </asp:Panel>

                    <!-- STEP 2: OTP (SYSTEM BUSY) -->
                    <asp:Panel ID="pnlOTP" runat="server" Visible="false">
                        <div class="otp-display">
                            <span class="text-warning fw-bold small d-block mb-2">VERIFICATION REQUIRED</span>
                            <asp:Label ID="lblOTP" runat="server" CssClass="otp-code">0000</asp:Label>
                        </div>
                        <div class="mb-4">
                            <label class="setup-label text-center">Enter Verification Code</label>
                            <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control text-center fw-bold fs-3" placeholder="· · · ·" MaxLength="4" autoComplete="off"></asp:TextBox>
                        </div>
                        <asp:Button ID="btnVerifyOTP" runat="server" Text="Reset Session" OnClick="btnVerifyOTP_Click" CssClass="btn-action" />
                    </asp:Panel>

                    <!-- STEP 3: SQL CONFIGURATION -->
                    <asp:Panel ID="pnlSqlConfig" runat="server" Visible="false">
                        <label class="setup-label">Server Address</label>
                        <div class="input-group mb-3">
                            <span class="input-group-text"><i class="fas fa-server"></i></span>
                            <asp:TextBox ID="txtServer" runat="server" CssClass="form-control" placeholder="localhost\SQLEXPRESS"></asp:TextBox>
                        </div>
                        <label class="setup-label">Database Name</label>
                        <div class="input-group mb-3">
                            <span class="input-group-text"><i class="fas fa-database"></i></span>
                            <asp:TextBox ID="txtDB" runat="server" CssClass="form-control" placeholder="Reporting_DB"></asp:TextBox>
                        </div>
                        <div class="row g-3">
                            <div class="col-6 mb-3">
                                <label class="setup-label">SQL UID</label>
                                <asp:TextBox ID="txtSqlUser" runat="server" CssClass="form-control" placeholder="sa"></asp:TextBox>
                            </div>
                            <div class="col-6 mb-3">
                                <label class="setup-label">SQL Pass</label>
                                <asp:TextBox ID="txtSqlPass" runat="server" CssClass="form-control" TextMode="Password" placeholder="••••••"></asp:TextBox>
                            </div>
                        </div>
                        <asp:Button ID="btnSaveSql" runat="server" Text="Connect & Continue" OnClick="btnSaveSql_Click" CssClass="btn-action" />
                    </asp:Panel>

                    <!-- STEP 4: COMPANY SELECTION -->
                    <asp:Panel ID="pnlSelection" runat="server" Visible="false">
                        <div class="mb-3">
                            <label class="setup-label">Organization</label>
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCompany_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="mb-4">
                            <label class="setup-label">Fiscal Period</label>
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-select"></asp:DropDownList>
                        </div>
                        <asp:Button ID="btnEnterMain" runat="server" Text="Launch" OnClick="btnEnterMain_Click" CssClass="btn-action" />
                    </asp:Panel>

                </div>
                <div class="card-footer bg-light border-0 py-3 text-center">
                    <span class="text-muted extra-small" style="font-size: 0.7rem;">&copy; <%: DateTime.Now.Year %> BK Reporting</span>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function () {
            const canvas = document.getElementById('bg-canvas');
            if (canvas) {
                const ctx = canvas.getContext('2d');
                let particles = [];
                function resizeCanvas() {
                    canvas.width = window.innerWidth;
                    canvas.height = window.innerHeight;
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
                for (let i = 0; i < 70; i++) particles.push(new Particle());
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
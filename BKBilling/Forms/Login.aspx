<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BKBilling.Forms.Login" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Enterprise Login | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700&display=swap" rel="stylesheet">
    
    <style>
        :root {
            --primary: #2563eb;
            --primary-dark: #1d4ed8;
            --bg-body: #f8fafc;
            --text-main: #1e293b;
            --text-muted: #64748b;
        }

        body, html { height: 100%; margin: 0; font-family: 'Inter', sans-serif; overflow: hidden; background: #fff; }
        .login-wrapper { display: flex; height: 100vh; width: 100vw; }

        /* LEFT PANEL: BRANDING */
        .brand-section {
            flex: 1.5;
            background: linear-gradient(135deg, #2563eb 0%, #7c3aed 100%);
            position: relative;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            overflow: hidden;
        }

        .blob { position: absolute; background: rgba(255,255,255,0.1); border-radius: 50%; z-index: 1; }
        .blob-1 { width: 400px; height: 400px; top: -100px; right: -100px; }
        .blob-2 { width: 300px; height: 300px; bottom: -50px; left: -50px; border-radius: 30% 70% 70% 30% / 30% 30% 70% 70%; }

        .brand-content { position: relative; z-index: 10; text-align: center; padding: 40px; }
        .brand-logo-img { width: 100px; margin-bottom: 25px; filter: drop-shadow(0 10px 15px rgba(0,0,0,0.2)); }
        .brand-content h1 { font-weight: 800; font-size: 3rem; letter-spacing: -1.5px; margin-bottom: 10px; }
        .brand-content p { opacity: 0.9; font-size: 1.2rem; max-width: 400px; margin: 0 auto; font-weight: 300; }

        /* RIGHT PANEL: FORM */
        .form-section {
            flex: 1;
            background: white;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 40px;
            position: relative;
        }

        .login-card { width: 100%; max-width: 420px; animation: fadeInUp 0.5s ease-out; }
        .login-card h2 { font-weight: 700; color: var(--text-main); margin-bottom: 8px; font-size: 1.75rem; }
        .login-card p.subtitle { color: var(--text-muted); font-size: 0.95rem; margin-bottom: 35px; }

        /* INPUT GROUPS */
        .input-group-pro {
            background: #fff;
            border: 1px solid #e2e8f0;
            border-left: 4px solid var(--primary);
            border-radius: 10px;
            display: flex;
            align-items: center;
            margin-bottom: 20px;
            padding: 5px 15px;
            transition: all 0.3s ease;
        }
        .input-group-pro:focus-within {
            border-color: var(--primary);
            box-shadow: 0 4px 12px rgba(37,99,235,0.08);
            transform: translateX(4px);
        }
        .input-group-pro i { color: #94a3b8; font-size: 1.1rem; width: 25px; text-align: center; }
        .input-group-pro select, .input-group-pro input {
            border: none !important;
            box-shadow: none !important;
            outline: none !important;
            padding: 12px;
            width: 100%;
            font-size: 0.95rem;
            color: var(--text-main);
            background: transparent;
        }

        .btn-login {
            background: var(--primary);
            color: white;
            border: none;
            width: 100%;
            padding: 15px;
            border-radius: 10px;
            font-weight: 600;
            font-size: 1rem;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 10px;
            transition: 0.3s;
            box-shadow: 0 4px 14px 0 rgba(37,99,235,0.39);
            margin-top: 10px;
        }
        .btn-login:hover { background: var(--primary-dark); transform: translateY(-2px); box-shadow: 0 6px 20px rgba(37,99,235,0.23); }

        .extra-links {
            display: flex;
            justify-content: space-between;
            margin-top: 25px;
            font-size: 0.85rem;
        }
        .link-text { color: var(--primary); font-weight: 700; cursor: pointer; text-decoration: none; transition: 0.2s; }
        .link-text:hover { color: var(--primary-dark); text-decoration: underline; }
        
        #msg { 
            margin-top: 25px; 
            padding: 14px; 
            border-radius: 10px; 
            font-size: 0.88rem; 
            font-weight: 600; 
            display: none;
            text-align: center;
        }

        .setup-btn { position: absolute; bottom: 20px; right: 20px; display: none; opacity: 0.5; }
        .setup-btn:hover { opacity: 1; }

        @keyframes fadeInUp {
            from { opacity: 0; transform: translateY(20px); }
            to { opacity: 1; transform: translateY(0); }
        }

        @media (max-width: 992px) {
            .brand-section { display: none; }
            .form-section { background: var(--bg-body); }
            .login-card { background: white; padding: 40px; border-radius: 20px; box-shadow: 0 10px 25px rgba(0,0,0,0.05); }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-wrapper">
            <!-- Left Side -->
            <div class="brand-section">
                <div class="blob blob-1"></div>
                <div class="blob blob-2"></div>
                <div class="brand-content">
                    <img src="~/favicon.png" runat="server" class="brand-logo-img" alt="Logo" />
                    <h1>BK SOFTWARES</h1>
                    <p>Next-generation inventory and multi-tenant billing workspace.</p>
                </div>
            </div>

            <!-- Right Side -->
            <div class="form-section">
                <div class="login-card">
                    
                    <!-- View: Login -->
                    <div id="viewLogin" class="view-pane active-pane">
                        <h2>Welcome back</h2>
                        <p class="subtitle">Please enter your details to sign in</p>

                        <div class="input-group-pro">
                            <i class="fas fa-university"></i>
                            <select id="ddlCompany"></select>
                        </div>

                        <div class="input-group-pro">
                            <i class="fas fa-user"></i>
                            <input type="text" id="txtUser" placeholder="Username or Email" autocomplete="off" />
                        </div>

                        <div class="input-group-pro">
                            <i class="fas fa-key"></i>
                            <input type="password" id="txtPass" placeholder="Password" />
                        </div>

                        <div class="extra-links">
                            <label class="text-muted" style="cursor:pointer">
                                <input type="checkbox" class="form-check-input me-1"> Remember me
                            </label>
                            <span class="link-text" onclick="toggleView('Forgot')">Forgot Password?</span>
                        </div>

                        <button type="button" class="btn-login" id="btnLogin" onclick="doLogin()">
                            <i class="fas fa-sign-in-alt"></i> SIGN IN
                        </button>
                    </div>

                    <!-- View: Forgot Password -->
                    <div id="viewForgot" class="view-pane" style="display:none;">
                        <h2>Account Recovery</h2>
                        <p class="subtitle">Verify identity to reset your password</p>
                        
                        <div class="input-group-pro"><i class="fas fa-university"></i><select id="ddlForgotCompany"></select></div>
                        <div class="input-group-pro"><i class="fas fa-user-tag"></i><input type="text" id="forUser" placeholder="Registered Username" /></div>
                        <div class="input-group-pro"><i class="fas fa-shield-alt"></i><input type="password" id="forSecret" placeholder="Admin Secret Key" /></div>
                        <div class="input-group-pro"><i class="fas fa-lock"></i><input type="password" id="forNewPass" placeholder="Enter New Password" /></div>

                        <button type="button" class="btn-login" onclick="doReset()">
                            <i class="fas fa-sync"></i> RESET ACCESS
                        </button>
                        <div class="text-center mt-4">
                            <span class="link-text" onclick="toggleView('Login')">Back to Login</span>
                        </div>
                    </div>

                    <!-- Message Area -->
                    <div id="msg"></div>
                </div>

                <button type="button" id="btnSetup" class="btn btn-sm btn-outline-secondary setup-btn" onclick="initDB()">
                    <i class="fas fa-database"></i> Setup DB
                </button>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        // Frame Buster
        if (window.top !== window.self) { window.top.location.href = window.location.href; }

        function toggleView(v) {
            $('.view-pane').hide();
            $('#view' + v).fadeIn();
            $('#msg').hide();
        }

        function showMsg(m, isErr) {
            $('#msg').stop().hide().text(m).css({
                'display': 'block',
                'color': isErr ? '#991b1b' : '#166534',
                'background': isErr ? '#fef2f2' : '#f0fdf4',
                'border': isErr ? '1px solid #fee2e2' : '1px solid #dcfce7'
            }).fadeIn();
        }

        $(document).ready(function () {
            loadCompanies();
            // Shift + F2 for setup
            $(window).keydown(function (e) { if (e.shiftKey && e.which == 113) $('#btnSetup').fadeToggle(); });
            $('#txtPass').keypress(function (e) { if (e.which == 13) doLogin(); });
        });

        function loadCompanies() {
            $.ajax({
                type: "POST",
                url: "Login.aspx/GetCompanies",
                contentType: "application/json",
                success: function (r) {
                    var ddl = $('#ddlCompany, #ddlForgotCompany').empty();
                    ddl.append('<option value="0">-- Select Organization --</option>');
                    $.each(r.d, function (i, item) { ddl.append($('<option>', { value: item.ID, text: item.Name })); });
                }
            });
        }

        function doLogin() {
            var userData = { user: $('#txtUser').val(), pass: $('#txtPass').val(), companyId: $('#ddlCompany').val() };
            if (!userData.user || !userData.pass || userData.companyId == "0") {
                showMsg("Organization and Credentials are required", true);
                return;
            }
            showMsg("Authenticating...", false);
            $.ajax({
                type: "POST",
                url: "Login.aspx/ProcessLogin",
                data: JSON.stringify(userData),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var res = response.d;
                    if (res.success) {
                        localStorage.setItem("BK_AuthToken", res.token);
                        showMsg("Verification successful. Redirecting...", false);
                        window.location.replace(res.redirect);
                    } else {
                        showMsg(res.message, true);
                    }
                },
                error: function () { showMsg("Connection failed.", true); }
            });
        }

        function doReset() {
            var d = { user: $('#forUser').val(), secret: $('#forSecret').val(), newPass: $('#forNewPass').val(), companyId: $('#ddlForgotCompany').val() };
            if (!d.user || !d.newPass || d.companyId === "0") { showMsg("Please fill all fields.", true); return; }
            showMsg("Processing...", false);
            $.ajax({
                type: "POST",
                url: "Login.aspx/ResetPassword",
                data: JSON.stringify(d),
                contentType: "application/json",
                success: function (r) {
                    if (r.d.success) {
                        showMsg(r.d.message, false);
                        setTimeout(function () { toggleView('Login'); }, 2000);
                    } else {
                        showMsg(r.d.message, true);
                    }
                }
            });
        }

        function initDB() {
            if (confirm("Execute system schema update?")) {
                $.ajax({
                    type: "POST", url: "Login.aspx/UpdateDB", contentType: "application/json",
                    success: function (r) { alert(r.d); loadCompanies(); }
                });
            }
        }
    </script>
</body>
</html>
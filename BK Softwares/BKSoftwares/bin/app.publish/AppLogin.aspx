<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppLogin.aspx.cs" Inherits="BKSoftwares.AppLogin" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Welcome to BK Softwares</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.1.1/animate.min.css" />

    <style>
        :root {
            --primary-gradient: linear-gradient(135deg, #6366f1 0%, #4338ca 100%);
            --glass-bg: rgba(255, 255, 255, 0.95);
        }

        body {
            font-family: 'Poppins', sans-serif;
            background: #f3f4f6;
            height: 100vh;
            margin: 0;
            display: flex;
            align-items: center;
            justify-content: center;
            overflow: hidden;
        }

        .login-wrapper {
            width: 100%;
            max-width: 1000px;
            min-height: 600px;
            background: #fff;
            height: auto;
            display: flex;
            border-radius: 24px;
            box-shadow: 0 25px 50px -12px rgba(0, 0, 0, 0.15);
            overflow: hidden;
            margin: 15px;
            align-self: center;
        }

        /* Left Side: Image Section */
        .image-side {
            flex: 1.2;
            background: url('https://images.unsplash.com/photo-1550751827-4bd374c3f58b?q=80&w=2070&auto=format&fit=crop') center center;
            background-size: cover;
            position: relative;
            display: flex;
            flex-direction: column;
            justify-content: flex-end;
            padding: 40px;
            color: white;
            min-height: 100%;
        }

        .image-side::before {
            content: "";
            position: absolute;
            top: 0; left: 0; right: 0; bottom: 0;
            background: linear-gradient(to bottom, rgba(0,0,0,0) 0%, rgba(15, 23, 42, 0.8) 100%);
        }

        .image-content {
            position: relative;
            z-index: 10;
        }

        /* Right Side: Form Section */
        .form-side {
            flex: 1;
            padding: 40px;
            display: flex;
            flex-direction: column;
            justify-content: center;
            background: var(--glass-bg);
        }

        #viewRegister .mb-2 {
            margin-bottom: 0.8rem !important;
        }

        .brand-logo {
            font-size: 1.5rem;
            font-weight: 700;
            color: #1e1b4b;
            margin-bottom: 30px;
            display: flex;
            align-items: center;
            gap: 10px;
        }

        .form-label {
            font-size: 0.85rem;
            font-weight: 600;
            color: #64748b;
            margin-bottom: 8px;
        }

        .form-control {
            border-radius: 12px;
            padding: 12px 15px;
            border: 1px solid #e2e8f0;
            background: #f8fafc;
            transition: 0.3s;
        }

        .form-control:focus {
            box-shadow: 0 0 0 4px rgba(99, 102, 241, 0.1);
            border-color: #6366f1;
            background: #fff;
        }

        .btn-action {
            background: var(--primary-gradient);
            color: white;
            border: none;
            border-radius: 12px;
            padding: 14px;
            font-weight: 600;
            transition: 0.3s;
            margin-top: 10px;
        }

        .btn-action:hover {
            transform: translateY(-2px);
            box-shadow: 0 10px 15px -3px rgba(99, 102, 241, 0.3);
            color: white;
        }

        .link-toggle {
            color: #6366f1;
            text-decoration: none;
            font-size: 0.85rem;
            font-weight: 600;
            cursor: pointer;
        }

        .link-toggle:hover {
            text-decoration: underline;
        }

        .view-pane {
            display: none;
        }

        .active-pane {
            display: block;
        }

        /* Mobile adjustments */
        @media (max-width: 850px) {
            .image-side { display: none; }
            .login-wrapper { max-width: 450px; height: auto; }
            .form-side { padding: 40px 30px; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-wrapper animate__animated animate__zoomIn">
            
            <!-- Branding Image Area -->
            <div class="image-side">
                <div class="image-content">
                    <h2 class="fw-bold">Next-Gen IT Solutions</h2>
                    <p class="opacity-75">Secure, scalable, and professional management software for your business growth.</p>
                </div>
            </div>

            <!-- Functional Form Area -->
            <div class="form-side">
                <div class="brand-logo">
                    <div class="bg-primary text-white rounded-3 px-2 py-1">
                        <i class="fas fa-cube"></i>
                    </div>
                    <span>BK SOFTWARES</span>
                </div>

                <!-- LOGIN VIEW -->
                <div id="viewLogin" class="view-pane active-pane animate__animated">
                    <h4 class="fw-bold text-dark mb-1">Welcome Back!</h4>
                    <p class="text-muted small mb-4">Please enter your credentials to log in.</p>
                    
                    <div class="mb-3">
                        <label class="form-label">USERNAME</label>
                        <div class="input-group">
                            <span class="input-group-text bg-white border-end-0 text-muted"><i class="far fa-user"></i></span>
                            <input type="text" id="txtUsername" class="form-control border-start-0" placeholder="Enter username" />
                        </div>
                    </div>
                    
                    <div class="mb-4">
                        <label class="form-label">PASSWORD</label>
                        <div class="input-group">
                            <span class="input-group-text bg-white border-end-0 text-muted"><i class="fas fa-lock"></i></span>
                            <input type="password" id="txtPassword" class="form-control border-start-0" placeholder="••••••••" />
                        </div>
                    </div>

                    <button type="button" class="btn btn-action w-100 mb-4" onclick="performLogin()">Sign In</button>
                    
                    <div class="d-flex justify-content-between">
                        <span class="link-toggle" onclick="showView('Register')">Create Account</span>
                        <span class="link-toggle" onclick="showView('Forgot')">Forgot Password?</span>
                    </div>
                </div>

                <!-- REGISTER VIEW -->
                <div id="viewRegister" class="view-pane animate__animated">
                    <h4 class="fw-bold text-dark mb-1">Join Us</h4>
                    <p class="text-muted small mb-4">Set up a new system administrator account.</p>
                    
                    <div class="mb-2">
                        <label class="form-label">FULL NAME</label>
                        <input type="text" id="regName" class="form-control" placeholder="John Doe" />
                    </div>
                    <div class="mb-2">
                        <label class="form-label">USERNAME</label>
                        <input type="text" id="regUser" class="form-control" placeholder="johndoe123" />
                    </div>
                    <div class="mb-2">
                        <label class="form-label">PASSWORD</label>
                        <input type="password" id="regPass" class="form-control" placeholder="••••••••" />
                    </div>
                    <div class="mb-4">
                        <label class="form-label text-danger">SECRET ADMIN KEY</label>
                        <input type="password" id="regSecret" class="form-control border-danger-subtle" placeholder="Required for registration" />
                    </div>

                    <button type="button" class="btn btn-action w-100 mb-3" onclick="performRegister()">Register Now</button>
                    <div class="text-center">
                        <span class="link-toggle" onclick="showView('Login')">Back to Login</span>
                    </div>
                </div>

                <!-- FORGOT PASSWORD VIEW -->
                <div id="viewForgot" class="view-pane animate__animated">
                    <h4 class="fw-bold text-dark mb-1">Reset Access</h4>
                    <p class="text-muted small mb-4">Validate your identity to recover your password.</p>
                    
                    <div class="mb-3">
                        <label class="form-label">USERNAME</label>
                        <input type="text" id="forUser" class="form-control" placeholder="Enter your username" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">NEW PASSWORD</label>
                        <input type="password" id="forPass" class="form-control" placeholder="••••••••" />
                    </div>
                    <div class="mb-4">
                        <label class="form-label text-danger">SECRET ADMIN KEY</label>
                        <input type="password" id="forSecret" class="form-control border-danger-subtle" placeholder="Validate reset" />
                    </div>

                    <button type="button" class="btn btn-action w-100 mb-3" onclick="performReset()">Update Password</button>
                    <div class="text-center">
                        <span class="link-toggle" onclick="showView('Login')">Back to Login</span>
                    </div>
                </div>

                <div id="loader" class="text-center mt-3" style="display:none;">
                    <div class="spinner-border spinner-border-sm text-primary" role="status"></div>
                </div>

            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        function showView(viewName) {
            $('.view-pane').removeClass('active-pane animate__fadeInRight animate__fadeInLeft');
            $('.view-pane').hide();

            $('#view' + viewName).show().addClass('active-pane animate__fadeInRight');
        }

        function performLogin() {
            var data = { username: $('#txtUsername').val(), password: $('#txtPassword').val() };
            if (!data.username || !data.password) { alert("Please fill all fields"); return; }

            $('#loader').show();
            $.ajax({
                type: "POST", url: "AppLogin.aspx/ProcessLogin", data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (r) {
                    $('#loader').hide();
                    if (r.d.success) {
                        localStorage.setItem("BK_AuthToken", r.d.token);
                        localStorage.setItem("BK_TokenExpiry", r.d.expiry);
                        localStorage.setItem("BK_UserName", r.d.user);
                        window.location.href = "MainForm.aspx";
                    } else { alert(r.d.message); }
                }
            });
        }

        function performRegister() {
            var data = { name: $('#regName').val(), user: $('#regUser').val(), pass: $('#regPass').val(), secret: $('#regSecret').val() };
            if (!data.name || !data.user || !data.pass || !data.secret) { alert("All fields are required"); return; }

            $('#loader').show();
            $.ajax({
                type: "POST", url: "AppLogin.aspx/CreateUser", data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (r) {
                    $('#loader').hide();
                    alert(r.d.message);
                    if (r.d.success) showView('Login');
                }
            });
        }

        function performReset() {
            var data = { user: $('#forUser').val(), newPass: $('#forPass').val(), secret: $('#forSecret').val() };
            if (!data.user || !data.newPass || !data.secret) { alert("All fields are required"); return; }

            $('#loader').show();
            $.ajax({
                type: "POST", url: "AppLogin.aspx/ResetPassword", data: JSON.stringify(data),
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (r) {
                    $('#loader').hide();
                    alert(r.d.message);
                    if (r.d.success) showView('Login');
                }
            });
        }
    </script>
</body>
</html>
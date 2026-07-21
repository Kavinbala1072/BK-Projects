<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BKBilling.Forms.Login" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <title>BK Softwares | Portal</title>
    <%--<link href="~/Image/favicon.png" rel="shortcut icon" type="image" />--%>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <style>
        :root { --primary: #4f46e5; --primary-dark: #3730a3; }
        body, html { height: 100%; margin: 0; font-family: 'Inter', system-ui, -apple-system, sans-serif; overflow: hidden; }
        .main-container { display: flex; height: 100vh; }
        .side-panel { flex: 1; background: linear-gradient(rgba(200, 200, 229, 0.85), rgba(200, 200, 229, 0.95)), url('https://images.unsplash.com/photo-1554224155-6726b3ff858f?q=80&w=1500'); 
                      background-size: cover; display: flex; flex-direction: column; justify-content: center; align-items: center; color: white; padding: 40px; text-align: center; }
        /*.side-panel { flex: 1; background: linear-gradient(rgba(79, 70, 229, 0.85), rgba(55, 48, 163, 0.95));} */
        .form-panel { width: 500px; background: #f9fafb; display: flex; align-items: center; justify-content: center; position: relative; }
        .login-card { background: white; width: 100%; max-width: 400px; padding: 40px; border-radius: 24px; box-shadow: 0 10px 30px rgba(0,0,0,0.05); }
        .form-control, .form-select { border-radius: 10px; padding: 12px; border: 1px solid #e5e7eb; }
        .btn-primary { background: var(--primary); border: none; border-radius: 12px; padding: 14px; font-weight: 600; width: 100%; transition: 0.3s; }
        .btn-primary:hover { background: var(--primary-dark); transform: translateY(-1px); }
        .view-pane { display: none; } .active-pane { display: block; animation: fadeInUp 0.4s; }
        @keyframes fadeInUp { from { opacity: 0; transform: translateY(20px); } to { opacity: 1; transform: translateY(0); } }
        .setup-btn { position: absolute; bottom: 20px; right: 20px; display: none; }
        .link-text { color: var(--primary); cursor: pointer; text-decoration: none; font-weight: 600; font-size: 0.85rem; }
    </style>
</head>
<body>
    <div class="main-container">
        <div class="side-panel">
            <img src="~/favicon.png" runat="server" alt="Logo" style="width:100px;" />
            <%--<i class="fas fa-file-invoice-dollar mb-4" style="font-size: 5rem;"></i>--%>
            <h1 class="fw-bold" style="color: white;">BK SOFTWARES</h1>
            <p class="opacity-75" style="color: white;">Smart Billing & Multi-Tenant Inventory Solution</p>
        </div>
        <div class="form-panel">
            <div class="login-card">
                <div id="viewLogin" class="view-pane active-pane">
                    <div class="text-center mb-4"><h3 class="fw-bold">Sign In</h3></div>
                    <label class="form-label small fw-bold">Select Company</label>
                    <select id="ddlCompany" class="form-select mb-3"></select>
                    <div class="mb-3">
                        <input type="text" id="txtUser" class="form-control" placeholder="Username" autocomplete="off" />
                    </div>
                    <div class="mb-3">
                        <input type="password" id="txtPass" class="form-control" placeholder="Password" />
                    </div>
                    <button type="button" class="btn btn-primary" id="btnLogin" onclick="doLogin()">Login</button>
                    <div class="mt-3 text-center">
                        <span class="link-text" onclick="toggleView('Forgot')">Forgot Password?</span>
                    </div>
                </div>

                <div id="viewForgot" class="view-pane">
                    <h5 class="fw-bold mb-3">Reset Password</h5>
                    <select id="ddlForgotCompany" class="form-select mb-3"></select>
                    <input type="text" id="forUser" class="form-control mb-2" placeholder="Username" />
                    <input type="password" id="forSecret" class="form-control mb-2" placeholder="Admin Secret Key" />
                    <input type="password" id="forNewPass" class="form-control mb-3" placeholder="New Password" />
                    <button type="button" class="btn btn-primary" onclick="doReset()">Reset Access</button>
                    <div class="mt-3 text-center"><span class="link-text" onclick="toggleView('Login')">Back to Login</span></div>
                </div>

                <div id="msg" class="mt-3 text-center small fw-bold" style="min-height: 20px;"></div>
            </div>
            <button type="button" id="btnSetup" class="btn btn-sm btn-dark setup-btn" onclick="initDB()">Setup System</button>
        </div>
    </div>

<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script>
    function toggleView(v) {
        $('.view-pane').removeClass('active-pane').hide();
        $('#view' + v).addClass('active-pane').show();
        $('#msg').text('');
    }

    function showMsg(m, isErr) {
        $('#msg').text(m).css('color', isErr ? '#ef4444' : '#10b981');
    }

    $(document).ready(function () {
        loadCompanies();
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
                /*ddl.append('<option value="1">-- Select Company --</option>');*/
                $.each(r.d, function (i, item) { ddl.append($('<option>', { value: item.ID, text: item.Name })); });
            }
        });
    }

    function doLogin() {
        var userData = {
            user: $('#txtUser').val(),
            pass: $('#txtPass').val(),
            companyId: $('#ddlCompany').val()
        };

        if (!userData.user || !userData.pass) {
            showMsg("Enter username and password", true);
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
                    localStorage.setItem("BK_TokenExpiry", res.expiry);

                    showMsg("Redirecting...", false);
                    window.location.replace(res.redirect);
                } else {
                    showMsg(res.message, true);
                }
            },
            error: function (xhr) {
                showMsg("Server Error: " + xhr.statusText, true);
            }
        });
    }

    function doReset() {
        var d = { user: $('#forUser').val(), secret: $('#forSecret').val(), newPass: $('#forNewPass').val(), companyId: $('#ddlForgotCompany').val() };
        if (!d.user || !d.newPass || d.companyId === "0") { showMsg("Fill all fields and select company", true); return; }

        $.ajax({
            type: "POST", url: "Login.aspx/ResetPassword", data: JSON.stringify(d), contentType: "application/json",
            success: function (r) {
                alert(r.d.message);
                if (r.d.success) toggleView('Login');
            }
        });
    }

    function initDB() {
        if (confirm("Setup/Update Database Schema?")) {
            $.ajax({
                type: "POST", url: "Login.aspx/UpdateDB", contentType: "application/json",
                success: function (r) { alert(r.d); loadCompanies(); }
            });
        }
    }
</script>
</body>
</html>
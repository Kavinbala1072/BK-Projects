<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs" Inherits="BKSoftwares.CreateUser" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Application Users | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <style>
        body { background-color: #f8fafc; font-family: 'Inter', sans-serif; padding: 30px; }
        .setup-card { background: white; border-radius: 16px; box-shadow: 0 10px 25px rgba(0,0,0,0.05); max-width: 500px; margin: auto; padding: 40px; border: 1px solid #e2e8f0; }
        .btn-create { background: #6366f1; color: white; border: none; padding: 12px; font-weight: 600; border-radius: 10px; width: 100%; transition: 0.3s; }
        .btn-create:hover { background: #4338ca; transform: translateY(-2px); }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="setup-card">
            <div class="text-center mb-4">
                <div class="bg-primary text-white d-inline-flex rounded-circle p-3 mb-3 shadow-sm">
                    <i class="fas fa-user-plus fs-3"></i>
                </div>
                <h4 class="fw-bold">Register App User</h4>
                <p class="text-muted small">Add a new user code to the GitHub Cloud Database</p>
            </div>

            <div class="mb-3">
                <label class="form-label small fw-bold">USER CODE (e.g. BK0003)</label>
                <asp:TextBox ID="txtNewUserCode" runat="server" CssClass="form-control" placeholder="BK0001"></asp:TextBox>
            </div>

            <div class="mb-3">
                <label class="form-label small fw-bold">SECURITY PASSWORD</label>
                <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" placeholder="••••••••"></asp:TextBox>
            </div>

            <div class="mb-4">
                <label class="form-label small fw-bold text-danger">ADMIN SECRET KEY</label>
                <asp:TextBox ID="txtAdminSecret" runat="server" CssClass="form-control border-danger" TextMode="Password"></asp:TextBox>
            </div>

            <asp:Button ID="btnCreateUser" runat="server" Text="Push to GitHub Cloud" CssClass="btn-create shadow-sm" OnClick="btnCreateUser_Click" />

            <div class="mt-3 text-center">
                <asp:Label ID="lblStatus" runat="server" CssClass="small fw-bold"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
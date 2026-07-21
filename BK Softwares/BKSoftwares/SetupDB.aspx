<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetupDB.aspx.cs" Inherits="BKSoftwares.SetupDB" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Database Setup</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <div class="container mt-5">
        <div class="card shadow">
            <div class="card-body text-center">
                <h2>Initial Database Setup</h2>
                <p>Click the button below to create tables and default admin user.</p>
                <form runat="server">
                    <asp:Button ID="btnSetup" runat="server" Text="Initialize Database" CssClass="btn btn-danger btn-lg" OnClick="btnSetup_Click" />
                </form>
                <br />
                <asp:Label ID="lblStatus" runat="server" CssClass="fw-bold"></asp:Label>
            </div>
        </div>
    </div>
</body>
</html>
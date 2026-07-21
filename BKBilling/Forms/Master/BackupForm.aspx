<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BackupForm.aspx.cs" Inherits="BKBilling.Forms.Master.BackupForm" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Local Drive Backup | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <style>
        body { background: #f8fafc; background-image: radial-gradient(#cbd5e1 0.7px, transparent 0.7px); background-size: 24px 24px; min-height: 100vh; padding: 40px 20px; font-family: 'Inter', sans-serif; }
        .card-custom { background: #ffffff; border-radius: 16px; border: 1px solid #e2e8f0; box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1); max-width: 700px; margin: 0 auto; overflow: hidden; }
        .form-header { background: #ffffff; padding: 25px 40px; border-bottom: 1px solid #f1f5f9; }
        .section-title { font-size: 0.75rem; font-weight: 800; color: #6366f1; text-transform: uppercase; letter-spacing: 0.1em; margin-bottom: 20px; display: flex; align-items: center; }
        .section-title::after { content: ""; height: 1px; flex-grow: 1; background: #f1f5f9; margin-left: 15px; }
        .path-box { background: #f1f5f9; padding: 15px; border-radius: 10px; font-family: monospace; color: #1e293b; border: 1px solid #cbd5e1; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <div class="container">
            <div class="card-custom">
                <div class="form-header text-center">
                    <h3 class="fw-bold m-0 text-dark"><i class="fas fa-hdd me-2 text-primary"></i>Local Drive Backup</h3>
                    <p class="text-muted small">Save a backup file directly to the server's hard drive.</p>
                </div>

                <div class="p-5">
                    <p class="section-title">Target Location</p>
                    
                    <div class="mb-4">
                        <label class="form-label fw-bold small text-muted">Drive Path on Server:</label>
                        <asp:TextBox ID="txtBackupPath" runat="server" CssClass="form-control" placeholder="e.g., D:\SQLBackups\"></asp:TextBox>
                        <small class="text-info"><i class="fas fa-info-circle me-1"></i>Ensure the folder ends with a backslash (\)</small>
                    </div>

                    <div class="alert alert-secondary border-0 mb-4 p-3 small">
                        <i class="fas fa-shield-halved me-2"></i>
                        <strong>Permission Requirement:</strong> The SQL Server Service account (MSSQLSERVER) must have <strong>Write Permissions</strong> on this folder.
                    </div>

                    <asp:LinkButton ID="btnLocalBackup" runat="server" CssClass="btn btn-success w-100 py-2 fw-bold shadow-sm" OnClick="btnLocalBackup_Click">
                        <i class="fas fa-play-circle me-2"></i>START LOCAL BACKUP NOW
                    </asp:LinkButton>

                    <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="mt-4">
                        <div class="path-box">
                            <i class="fas fa-check-circle text-success me-2"></i>
                            Backup Saved To:<br />
                            <asp:Label ID="lblFinalPath" runat="server" CssClass="fw-bold"></asp:Label>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
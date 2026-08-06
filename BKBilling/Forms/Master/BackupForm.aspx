<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BackupForm.aspx.cs" Inherits="BKBilling.Forms.Master.BackupForm" EnableEventValidation="false" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Backup Maintenance | Pro ERP</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800&display=swap" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />

    <style>
        :root { --erp-blue: #2563eb; --erp-bg: #f8fafc; --erp-border: #e2e8f0; --erp-text: #1e293b; --erp-text-light: #64748b; }
        html, body { height: 100%; margin: 0; padding: 0; background-color: #fff; font-family: 'Inter', sans-serif; color: var(--erp-text); overflow: hidden; }
        form { height: 100%; }
        .erp-wrapper { display: flex; flex-direction: column; height: 100vh; }
        .erp-header { padding: 15px 30px; border-bottom: 1px solid var(--erp-border); background: #fff; flex-shrink: 0; display: flex; justify-content: space-between; align-items: center; }
        .erp-body { flex-grow: 1; overflow-y: auto; background: #fff; padding: 40px; display: flex; justify-content: center; }
        .erp-footer { padding: 10px 30px; border-top: 1px solid var(--erp-border); background: #fff; display: flex; justify-content: space-between; align-items: center; flex-shrink: 0; }

        .page-title { font-size: 18px; font-weight: 800; margin: 0; }
        .section-title { font-size: 11px; font-weight: 800; color: var(--erp-blue); text-transform: uppercase; letter-spacing: 1px; border-bottom: 1px solid #f1f5f9; padding-bottom: 5px; margin-bottom: 15px; display: flex; align-items: center; gap: 8px; }
        .form-label { font-weight: 600; font-size: 12px; color: var(--erp-text-light); margin-bottom: 3px; }

        .backup-card { width: 100%; max-width: 600px; }
        .path-box { background: #f1f5f9; padding: 15px; border-radius: 10px; font-family: 'Courier New', monospace; color: #1e293b; border: 1px solid #cbd5e1; font-size: 13px; }
        .info-alert { background: #f0f7ff; border: 1px solid #cfe2ff; border-radius: 10px; padding: 15px; display: flex; gap: 12px; }

        .btn-primary-erp { background: var(--erp-blue); color: #fff !important; font-weight: 600; border-radius: 8px; padding: 10px 20px; font-size: 14px; border: none; text-decoration: none; width: 100%; text-align: center; display: block; box-shadow: 0 4px 6px rgba(37,99,235,0.2); }
        .btn-tool { width: 34px; height: 34px; border-radius: 6px; border: 1px solid var(--erp-border); background: #fff; display: inline-flex; align-items: center; justify-content: center; color: var(--erp-text-light); text-decoration: none; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>
                <div class="erp-wrapper">
                    
                    <!-- HEADER SECTION -->
                    <div class="erp-header">
                        <h4 class="page-title"><i class="fas fa-database text-primary me-2"></i>Backup Maintenance</h4>
                    </div>

                    <!-- BODY SECTION -->
                    <div class="erp-body">
                        <div class="backup-card">
                            <p class="section-title"><i class="fas fa-hdd"></i> 1. Server Configuration</p>
                            
                            <div class="mb-4">
                                <label class="form-label">Target Drive Path on Server</label>
                                <asp:TextBox ID="txtBackupPath" runat="server" CssClass="form-control" placeholder="e.g., D:\SQLBackups\"></asp:TextBox>
                                <div class="mt-1 small text-muted"><i class="fas fa-info-circle me-1"></i>Path must exist on the machine where SQL Server is installed.</div>
                            </div>

                            <div class="info-alert mb-4">
                                <i class="fas fa-shield-halved text-primary mt-1"></i>
                                <div class="small">
                                    <strong class="d-block mb-1">System Requirement</strong>
                                    The SQL Server Service account (MSSQLSERVER) must have <strong>Full Write Permissions</strong> on the target folder, or the backup will fail.
                                </div>
                            </div>

                            <asp:LinkButton ID="btnLocalBackup" runat="server" CssClass="btn-primary-erp bg-success" OnClick="btnLocalBackup_Click">
                                <i class="fas fa-play-circle me-2"></i>GENERATE SERVER-SIDE BACKUP
                            </asp:LinkButton>

                            <asp:Panel ID="pnlSuccess" runat="server" Visible="false" CssClass="mt-4">
                                <div class="path-box animate__animated animate__fadeIn">
                                    <div class="text-success fw-bold mb-1"><i class="fas fa-check-circle me-2"></i>Backup Completed Successfully</div>
                                    <div class="text-muted small mb-1">File Location:</div>
                                    <asp:Label ID="lblFinalPath" runat="server" CssClass="fw-bold break-all"></asp:Label>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>

                    <!-- FOOTER SECTION -->
                    <div class="erp-footer">
                        <div class="small text-muted">Local drive backups are non-recoverable if the server hardware fails.</div>
                        <div class="small text-muted text-uppercase fw-bold">DB Utility v1.2</div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Toast Notification -->
        <div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3">
            <div id="msgToast" class="toast align-items-center text-white border-0 shadow-lg" role="alert">
                <div class="d-flex">
                    <div class="toast-body"><i id="msgIcon"></i> <span id="msgText"></span></div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function showNotification(message, type) {
            var toastEl = document.getElementById('msgToast');
            var msgText = document.getElementById('msgText');
            var msgIcon = document.getElementById('msgIcon');
            if (!toastEl) return;

            msgText.innerText = message;
            toastEl.classList.remove('bg-danger', 'bg-success', 'bg-primary');

            if (type === 'error') {
                toastEl.classList.add('bg-danger');
                msgIcon.className = "fas fa-exclamation-triangle me-2";
            } else if (type === 'success') {
                toastEl.classList.add('bg-success');
                msgIcon.className = "fas fa-check-circle me-2";
            } else {
                toastEl.classList.add('bg-primary');
                msgIcon.className = "fas fa-info-circle me-2";
            }
            var toast = new bootstrap.Toast(toastEl, { delay: 4000 });
            toast.show();
        }
    </script>
</body>
</html>
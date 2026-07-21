<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemSetting.aspx.cs" Inherits="BKBilling.Forms.Settings.ItemSetting" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Item Settings | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body { background: #f8fafc; background-image: radial-gradient(#cbd5e1 0.7px, transparent 0.7px); background-size: 24px 24px; min-height: 100vh; padding: 40px 20px; font-family: 'Inter', sans-serif; }
        .card-custom { background: #ffffff; border-radius: 16px; border: 1px solid #e2e8f0; box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1); max-width: 900px; margin: 0 auto; overflow: hidden; }
        .form-header { background: #ffffff; padding: 25px 40px; border-bottom: 1px solid #f1f5f9; }
        .section-title { font-size: 0.75rem; font-weight: 800; color: #6366f1; text-transform: uppercase; letter-spacing: 0.1em; margin-bottom: 20px; display: flex; align-items: center; }
        .section-title::after { content: ""; height: 1px; flex-grow: 1; background: #f1f5f9; margin-left: 15px; }
        .step-box { background: #f8fafc; border: 1px solid #e2e8f0; border-radius: 12px; padding: 25px; height: 100%; transition: 0.3s; }
        .step-box:hover { border-color: #6366f1; box-shadow: 0 10px 15px -3px rgba(99, 102, 241, 0.1); }
        .step-num { width: 32px; height: 32px; background: #6366f1; color: white; border-radius: 50%; display: flex; align-items: center; justify-content: center; font-weight: bold; margin-bottom: 15px; }
    </style>
    <script>
        function showNotification(message, type) {
            var toastEl = document.getElementById('msgToast');
            document.getElementById('msgText').innerText = message;
            toastEl.classList.remove('bg-danger', 'bg-success');
            toastEl.classList.add(type === 'error' ? 'bg-danger' : 'bg-success');
            new bootstrap.Toast(toastEl).show();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <div class="container-fluid">
            <div class="card-custom">
                <div class="form-header">
                    <h3 class="fw-bold m-0 text-dark"><i class="fas fa-file-excel me-2 text-success"></i>Bulk Item Manager</h3>
                    <p class="text-muted small m-0">Export your product list or import new items via Excel.</p>
                </div>

                <div class="p-5">
                    <div class="row g-4">
                        <!-- Step 1: Export -->
                        <div class="col-md-6">
                            <div class="step-box">
                                <div class="step-num">1</div>
                                <h5 class="fw-bold">Export Template</h5>
                                <p class="text-muted small">Download a clean Excel file with the correct headers to ensure your data matches the system requirements.</p>
                                <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-outline-primary w-100 fw-bold py-2 mt-3" OnClick="btnExport_Click">
                                    <i class="fas fa-download me-2"></i> DOWNLOAD TEMPLATE
                                </asp:LinkButton>
                            </div>
                        </div>

                        <!-- Step 2: Import -->
                        <div class="col-md-6">
                            <div class="step-box">
                                <div class="step-num">2</div>
                                <h5 class="fw-bold">Import Data</h5>
                                <p class="text-muted small">Select your filled Excel file to upload items in bulk. Duplicates will be automatically skipped.</p>
                                <div class="mt-3">
                                    <asp:FileUpload ID="fuImport" runat="server" CssClass="form-control form-control-sm mb-2" />
                                    <asp:LinkButton ID="btnImport" runat="server" CssClass="btn btn-success w-100 fw-bold py-2" OnClick="btnImport_Click">
                                        <i class="fas fa-upload me-2"></i> START IMPORT
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="mt-5 pt-4 border-top">
                        <p class="section-title">Important Instructions</p>
                        <ul class="small text-muted">
                            <li>Keep the <strong>Item_Code</strong> unique; the system uses this to identify products.</li>
                            <li>For <strong>Item_Type</strong>, use only "Item" or "Material".</li>
                            <li>Ensure that Category, Color, and Unit IDs match the values in your Master lists.</li>
                            <li>Do not change the header names in the Excel file.</li>
                        </ul>
                    </div>
                </div>
            </div>

            <!-- Toast -->
            <div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3">
                <div id="msgToast" class="toast align-items-center text-white border-0 shadow-lg" role="alert">
                    <div class="d-flex"><div class="toast-body"><span id="msgText"></span></div><button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button></div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
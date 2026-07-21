<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompSetting.aspx.cs" Inherits="BKBilling.Forms.Settings.CompSetting" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Company Settings | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body { background: #f8fafc; background-image: radial-gradient(#cbd5e1 0.7px, transparent 0.7px); background-size: 24px 24px; min-height: 100vh; padding: 20px; font-family: 'Inter', sans-serif; }
        .card-custom { background: #ffffff; border-radius: 16px; border: 1px solid #e2e8f0; box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1); margin: 0 auto; overflow: hidden; }
        .nav-tabs { background: #f8fafc; padding: 10px 20px 0 20px; border-bottom: 1px solid #e2e8f0; }
        .nav-link { border: none !important; color: #64748b; font-weight: 600; padding: 12px 20px; cursor: pointer; }
        .nav-link.active { color: #6366f1 !important; background: #fff !important; border-bottom: 3px solid #6366f1 !important; }
        .form-body { padding: 30px; min-height: 500px; }
        .section-title { font-size: 0.75rem; font-weight: 800; color: #6366f1; text-transform: uppercase; letter-spacing: 0.1em; margin-bottom: 20px; display: flex; align-items: center; }
        .section-title::after { content: ""; height: 1px; flex-grow: 1; background: #f1f5f9; margin-left: 15px; }
        .form-label { font-weight: 600; font-size: 0.8rem; color: #475569; }
        .form-control-sm, .form-select-sm { border-radius: 6px; border: 1px solid #cbd5e1; padding: 8px 12px; }
        .statutory-box { padding: 20px; border-radius: 12px; height: 100%; border: 1px solid transparent; }
        .bg-tcs { background: #f0f7ff; border-color: #cfe2ff !important; }
        .bg-tds { background: #fff7ed; border-color: #ffedd5 !important; }
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
                <div class="p-4 border-bottom d-flex justify-content-between align-items-center">
                    <h3 class="fw-bold m-0 text-dark"><i class="fas fa-sliders me-2 text-primary"></i>Company Configuration</h3>
                    <asp:LinkButton ID="btnSaveAll" runat="server" CssClass="btn btn-primary px-4 fw-bold shadow-sm" OnClick="btnSaveAll_Click">
                        <i class="fas fa-save me-2"></i>SAVE ALL SETTINGS
                    </asp:LinkButton>
                </div>

                <ul class="nav nav-tabs">
                    <li class="nav-item"><asp:LinkButton ID="tab1" runat="server" CssClass="nav-link active" OnClick="SwitchTab">General</asp:LinkButton></li>
                    <li class="nav-item"><asp:LinkButton ID="tab2" runat="server" CssClass="nav-link" OnClick="SwitchTab">Statutory (GST/TDS)</asp:LinkButton></li>
                    <li class="nav-item"><asp:LinkButton ID="tab3" runat="server" CssClass="nav-link" OnClick="SwitchTab">Print & Footer</asp:LinkButton></li>
                </ul>

                <div class="form-body">
                    <asp:MultiView ID="mvSettings" runat="server" ActiveViewIndex="0">
                        
                        <!-- TAB 1: GENERAL -->
                        <asp:View ID="vwGeneral" runat="server">
                            <p class="section-title">Email & Regional</p>
                            <div class="row g-3 mb-4">
                                <div class="col-md-6">
                                    <label class="form-label">Email ID</label>
                                    <asp:TextBox ID="setEmail" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Email Password</label>
                                    <asp:TextBox ID="setEmailPass" runat="server" CssClass="form-control form-control-sm" TextMode="Password"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Currency Format</label>
                                    <asp:TextBox ID="setCurrencyFmt" runat="server" CssClass="form-control form-control-sm" placeholder="e.g. ₹ #,##,##0.00"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Default Language</label>
                                    <asp:TextBox ID="setLanguage" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <p class="section-title">Company Bank Details</p>
                            <div class="row g-3">
                                <div class="col-md-6">
                                    <label class="form-label">Bank Name</label>
                                    <asp:TextBox ID="setBankName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Account No</label>
                                    <asp:TextBox ID="setBankAcNo" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Bank IFSC</label>
                                    <asp:TextBox ID="setBankIfsc" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Bank Branch</label>
                                    <asp:TextBox ID="setBankBranch" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                        </asp:View>

                        <!-- TAB 2: STATUTORY -->
                        <asp:View ID="vwStatutory" runat="server">
                            <div class="row g-4">
                                <div class="col-md-6 border-end">
                                    <p class="section-title">GST & API Config</p>
                                    <div class="row g-3 mb-3">
                                        <div class="col-6">
                                            <label class="form-label">GST TIN/UIN</label>
                                            <asp:TextBox ID="gstTin" runat="server" CssClass="form-control form-control-sm" MaxLength="15"></asp:TextBox>
                                        </div>
                                        <div class="col-6">
                                            <label class="form-label">Dealer Type</label>
                                            <asp:DropDownList ID="gstDealerType" runat="server" CssClass="form-select form-select-sm">
                                                <asp:ListItem>Regular</asp:ListItem>
                                                <asp:ListItem>Composition</asp:ListItem>
                                                <asp:ListItem>Unregistered</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-6">
                                            <label class="form-label">GST Onwards</label>
                                            <asp:TextBox ID="gstOnwards" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="col-6">
                                            <label class="form-label">State</label>
                                            <asp:TextBox ID="gstState" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-6">
                                            <label class="form-label">Reg. Mobile No</label>
                                            <asp:TextBox ID="gstMobile" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-6">
                                            <label class="form-label">Pincode</label>
                                            <asp:TextBox ID="gstPincode" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                        </div>
                                        <div class="col-12">
                                            <label class="form-label">Cash Transaction Limit</label>
                                            <asp:TextBox ID="gstCashLimit" runat="server" CssClass="form-control form-control-sm" Text="0"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="p-3 bg-light rounded border">
                                        <label class="form-label fw-bold small text-primary"><i class="fas fa-key me-1"></i> E-Invoice API</label>
                                        <div class="row g-2">
                                            <div class="col-6"><asp:TextBox ID="gstUser" runat="server" CssClass="form-control form-control-sm" placeholder="API Username"></asp:TextBox></div>
                                            <div class="col-6"><asp:TextBox ID="gstPass" runat="server" CssClass="form-control form-control-sm" TextMode="Password" placeholder="API Password"></asp:TextBox></div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <div class="statutory-box bg-tcs mb-3">
                                        <h6 class="fw-bold text-primary mb-3">Tax Collection at Source (TCS)</h6>
                                        <div class="row g-2 mb-2">
                                            <div class="col-4">
                                                <label class="form-label">Enable</label>
                                                <asp:DropDownList ID="tcsEnabled" runat="server" CssClass="form-select form-select-sm">
                                                    <asp:ListItem Value="0">No</asp:ListItem><asp:ListItem Value="1">Yes</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-8">
                                                <label class="form-label">TCS Ledger</label>
                                                <asp:DropDownList ID="ddlTCSLedger" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="row g-2">
                                            <div class="col-6"><label class="form-label">Limit</label><asp:TextBox ID="txtTcsLimit" runat="server" CssClass="form-control form-control-sm"></asp:TextBox></div>
                                            <div class="col-3"><label class="form-label">PAN%</label><asp:TextBox ID="txtTcsPan" runat="server" CssClass="form-control form-control-sm" placeholder="0.1"></asp:TextBox></div>
                                            <div class="col-3"><label class="form-label">NoPAN%</label><asp:TextBox ID="txtTcsNoPan" runat="server" CssClass="form-control form-control-sm" placeholder="1"></asp:TextBox></div>
                                        </div>
                                    </div>

                                    <div class="statutory-box bg-tds">
                                        <h6 class="fw-bold text-warning mb-3">Tax Deducted at Source (TDS)</h6>
                                        <div class="row g-2 mb-2">
                                            <div class="col-4">
                                                <label class="form-label">Enable</label>
                                                <asp:DropDownList ID="tdsEnabled" runat="server" CssClass="form-select form-select-sm">
                                                    <asp:ListItem Value="0">No</asp:ListItem><asp:ListItem Value="1">Yes</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-8">
                                                <label class="form-label">TAN Number</label>
                                                <asp:TextBox ID="tdsTan" runat="server" CssClass="form-control form-control-sm" placeholder="TAN NO"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row g-2">
                                            <div class="col-12"><label class="form-label">TDS Ledger</label><asp:DropDownList ID="ddlTDSLedger" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:View>

                        <!-- TAB 3: PRINT -->
                        <asp:View ID="vwPrint" runat="server">
                            <p class="section-title">Invoice Document Design</p>
                            <label class="form-label">Terms & Conditions</label>
                            <asp:TextBox ID="setTerms" runat="server" CssClass="form-control form-control-sm mb-3" TextMode="MultiLine" Rows="4"></asp:TextBox>
                            <label class="form-label">Footer Note</label>
                            <asp:TextBox ID="setFooter" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </asp:View>
                    </asp:MultiView>
                </div>
            </div>

            <!-- Toast Notification -->
            <div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3">
                <div id="msgToast" class="toast align-items-center text-white border-0 shadow-lg" role="alert">
                    <div class="d-flex"><div class="toast-body"><span id="msgText"></span></div><button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button></div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
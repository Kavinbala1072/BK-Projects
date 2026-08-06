<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompSetting.aspx.cs" Inherits="BKBilling.Forms.Settings.CompSetting" EnableEventValidation="false" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Company Settings | Pro ERP</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800&display=swap" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />

    <style>
        :root { --erp-blue: #2563eb; --erp-bg: #f8fafc; --erp-border: #e2e8f0; --erp-text: #1e293b; --erp-text-light: #64748b; }
        html, body { height: 100%; margin: 0; padding: 0; background-color: #fff; font-family: 'Inter', sans-serif; color: var(--erp-text); overflow: hidden; }
        form { height: 100%; }
        .erp-wrapper { display: flex; flex-direction: column; height: 100vh; }
        .erp-header { padding: 15px 30px; border-bottom: 1px solid var(--erp-border); background: #fff; flex-shrink: 0; }
        .erp-body { flex-grow: 1; overflow-y: auto; background: #fff; padding: 20px 30px; }
        .erp-footer { padding: 10px 30px; border-top: 1px solid var(--erp-border); background: #fff; display: flex; justify-content: space-between; align-items: center; flex-shrink: 0; }

        .page-title { font-size: 18px; font-weight: 800; margin: 0; }
        
        /* Custom Tabs Styling */
        .erp-tabs { display: flex; gap: 5px; margin-top: 15px; border-bottom: 1px solid var(--erp-border); }
        .erp-tab-link { padding: 8px 20px; font-size: 13px; font-weight: 600; color: var(--erp-text-light); text-decoration: none; border-bottom: 3px solid transparent; transition: 0.2s; }
        .erp-tab-link:hover { color: var(--erp-blue); background: #f1f5f9; }
        .erp-tab-link.active { color: var(--erp-blue); border-bottom-color: var(--erp-blue); }

        .section-title { font-size: 11px; font-weight: 800; color: var(--erp-blue); text-transform: uppercase; letter-spacing: 1px; border-bottom: 1px solid #f1f5f9; padding-bottom: 5px; margin-bottom: 15px; display: flex; align-items: center; gap: 8px; }
        .form-label { font-weight: 600; font-size: 12px; color: var(--erp-text-light); margin-bottom: 3px; }
        .btn-primary-erp { background: var(--erp-blue); color: #fff !important; font-weight: 600; border-radius: 8px; padding: 8px 25px; font-size: 14px; border: none; text-decoration: none; box-shadow: 0 4px 6px rgba(37,99,235,0.2); }
        
        .statutory-card { background: #f8fafc; border: 1px solid var(--erp-border); border-radius: 12px; padding: 15px; height: 100%; }
        .bg-tcs { border-left: 4px solid #3b82f6; }
        .bg-tds { border-left: 4px solid #f59e0b; }
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
                        <div class="d-flex justify-content-between align-items-center">
                            <div>
                                <h4 class="page-title"><i class="fas fa-cog text-primary me-2"></i>Company Configuration</h4>
                                <div class="erp-tabs">
                                    <asp:LinkButton ID="tab1" runat="server" CssClass="erp-tab-link active" OnClick="SwitchTab">General Settings</asp:LinkButton>
                                    <asp:LinkButton ID="tab2" runat="server" CssClass="erp-tab-link" OnClick="SwitchTab">Statutory (GST/TDS)</asp:LinkButton>
                                    <asp:LinkButton ID="tab3" runat="server" CssClass="erp-tab-link" OnClick="SwitchTab">Printing & Invoice</asp:LinkButton>
                                </div>
                            </div>
                            <div class="d-flex gap-2">
                                <button type="button" class="btn btn-outline-secondary btn-sm fw-bold border-0 px-3" onclick="window.parent.resetToWelcome()"><i class="fas fa-times me-1"></i> CLOSE</button>
                                <asp:LinkButton ID="btnSaveAll" runat="server" CssClass="btn-primary-erp bg-success" OnClick="btnSaveAll_Click">
                                    <i class="fas fa-save me-2"></i>SAVE ALL SETTINGS
                                </asp:LinkButton>
                            </div>
                        </div>
                    </div>

                    <!-- BODY SECTION -->
                    <div class="erp-body">
                        <asp:MultiView ID="mvSettings" runat="server" ActiveViewIndex="0">
                            
                            <!-- VIEW 1: GENERAL -->
                            <asp:View ID="vwGeneral" runat="server">
                                <div class="row g-4">
                                    <div class="col-md-6 border-end">
                                        <p class="section-title"><i class="fas fa-envelope"></i> Email & Regional</p>
                                        <div class="mb-3">
                                            <label class="form-label">System Email ID</label>
                                            <asp:TextBox ID="setEmail" runat="server" CssClass="form-control" placeholder="notifications@domain.com"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label">App Password</label>
                                            <asp:TextBox ID="setEmailPass" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                        </div>
                                        <div class="row g-2">
                                            <div class="col-6">
                                                <label class="form-label">Currency Format</label>
                                                <asp:TextBox ID="setCurrencyFmt" runat="server" CssClass="form-control" placeholder="₹ #,##,##0.00"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="form-label">System Language</label>
                                                <asp:TextBox ID="setLanguage" runat="server" CssClass="form-control" placeholder="English"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <p class="section-title"><i class="fas fa-university"></i> Default Bank Details</p>
                                        <div class="mb-3">
                                            <label class="form-label">Bank Name</label>
                                            <asp:TextBox ID="setBankName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label">Account Number</label>
                                            <asp:TextBox ID="setBankAcNo" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="row g-2">
                                            <div class="col-6">
                                                <label class="form-label">IFSC Code</label>
                                                <asp:TextBox ID="setBankIfsc" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="form-label">Branch Name</label>
                                                <asp:TextBox ID="setBankBranch" runat="server" CssClass="form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:View>

                            <!-- VIEW 2: STATUTORY -->
                            <asp:View ID="vwStatutory" runat="server">
                                <div class="row g-4">
                                    <div class="col-md-4 border-end">
                                        <p class="section-title"><i class="fas fa-file-invoice"></i> GST Configuration</p>
                                        <div class="row g-2 mb-3">
                                            <div class="col-7"><label class="form-label">GST TIN/UIN</label><asp:TextBox ID="gstTin" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox></div>
                                            <div class="col-5"><label class="form-label">Dealer Type</label><asp:DropDownList ID="gstDealerType" runat="server" CssClass="form-select"><asp:ListItem>Regular</asp:ListItem><asp:ListItem>Composition</asp:ListItem><asp:ListItem>Unregistered</asp:ListItem></asp:DropDownList></div>
                                        </div>
                                        <div class="row g-2 mb-3">
                                            <div class="col-6"><label class="form-label">GST Onwards</label><asp:TextBox ID="gstOnwards" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox></div>
                                            <div class="col-6"><label class="form-label">State</label><asp:TextBox ID="gstState" runat="server" CssClass="form-control"></asp:TextBox></div>
                                        </div>
                                        <div class="row g-2 mb-3">
                                            <div class="col-6"><label class="form-label">Reg. Mobile</label><asp:TextBox ID="gstMobile" runat="server" CssClass="form-control"></asp:TextBox></div>
                                            <div class="col-6"><label class="form-label">Pincode</label><asp:TextBox ID="gstPincode" runat="server" CssClass="form-control"></asp:TextBox></div>
                                        </div>
                                        <div class="mb-3"><label class="form-label">Cash Txn Limit</label><asp:TextBox ID="gstCashLimit" runat="server" CssClass="form-control" Text="0"></asp:TextBox></div>
                                        
                                        <div class="p-3 bg-light rounded border">
                                            <p class="form-label text-primary fw-bold mb-2"><i class="fas fa-key me-1"></i> E-Invoice API</p>
                                            <asp:TextBox ID="gstUser" runat="server" CssClass="form-control mb-2" placeholder="API User"></asp:TextBox>
                                            <asp:TextBox ID="gstPass" runat="server" CssClass="form-control" TextMode="Password" placeholder="API Password"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="col-md-4 border-end">
                                        <div class="statutory-card bg-tcs">
                                            <p class="section-title text-primary"><i class="fas fa-hand-holding-usd"></i> TCS Settings</p>
                                            <div class="row g-2 mb-3">
                                                <div class="col-4"><label class="form-label">Enabled</label><asp:DropDownList ID="tcsEnabled" runat="server" CssClass="form-select"><asp:ListItem Value="0">No</asp:ListItem><asp:ListItem Value="1">Yes</asp:ListItem></asp:DropDownList></div>
                                                <div class="col-8"><label class="form-label">TCS Ledger</label><asp:DropDownList ID="ddlTCSLedger" runat="server" CssClass="form-select"></asp:DropDownList></div>
                                            </div>
                                            <div class="mb-3"><label class="form-label">Exemption Limit</label><asp:TextBox ID="txtTcsLimit" runat="server" CssClass="form-control"></asp:TextBox></div>
                                            <div class="row g-2">
                                                <div class="col-6"><label class="form-label">PAN %</label><asp:TextBox ID="txtTcsPan" runat="server" CssClass="form-control" placeholder="0.1"></asp:TextBox></div>
                                                <div class="col-6"><label class="form-label">No PAN %</label><asp:TextBox ID="txtTcsNoPan" runat="server" CssClass="form-control" placeholder="1"></asp:TextBox></div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="statutory-card bg-tds">
                                            <p class="section-title text-warning"><i class="fas fa-cut"></i> TDS Settings</p>
                                            <div class="row g-2 mb-3">
                                                <div class="col-4"><label class="form-label">Enabled</label><asp:DropDownList ID="tdsEnabled" runat="server" CssClass="form-select"><asp:ListItem Value="0">No</asp:ListItem><asp:ListItem Value="1">Yes</asp:ListItem></asp:DropDownList></div>
                                                <div class="col-8"><label class="form-label">TAN Number</label><asp:TextBox ID="tdsTan" runat="server" CssClass="form-control" placeholder="ABCD12345E"></asp:TextBox></div>
                                            </div>
                                            <div class="mb-3"><label class="form-label">Default TDS Ledger</label><asp:DropDownList ID="ddlTDSLedger" runat="server" CssClass="form-select"></asp:DropDownList></div>
                                        </div>
                                    </div>
                                </div>
                            </asp:View>

                            <!-- VIEW 3: PRINT -->
                            <asp:View ID="vwPrint" runat="server">
                                <div class="row">
                                    <div class="col-md-8">
                                        <p class="section-title"><i class="fas fa-print"></i> Invoice Document Design</p>
                                        <div class="mb-4">
                                            <label class="form-label">Default Terms & Conditions</label>
                                            <asp:TextBox ID="setTerms" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="8"></asp:TextBox>
                                        </div>
                                        <div>
                                            <label class="form-label">Invoice Footer Note</label>
                                            <asp:TextBox ID="setFooter" runat="server" CssClass="form-control" placeholder="Thank you for your business!"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </asp:View>
                        </asp:MultiView>
                    </div>

                    <!-- FOOTER SECTION -->
                    <div class="erp-footer">
                        <div class="small text-muted">System Version: <b>v2.4.0</b></div>
                        <div class="small text-muted text-uppercase fw-bold">Company Sno: <asp:Literal ID="litCid" runat="server" /></div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3">
            <div id="msgToast" class="toast align-items-center text-white border-0 shadow-lg" role="alert">
                <div class="d-flex"><div class="toast-body"><span id="msgText"></span></div><button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button></div>
            </div>
        </div>
    </form>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function showNotification(message, type) {
            var toastEl = document.getElementById('msgToast');
            document.getElementById('msgText').innerText = message;
            toastEl.classList.remove('bg-danger', 'bg-success');
            toastEl.classList.add(type === 'error' ? 'bg-danger' : 'bg-success');
            new bootstrap.Toast(toastEl, { delay: 4000 }).show();
        }
    </script>
</body>
</html>
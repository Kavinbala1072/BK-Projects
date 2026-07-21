<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityForm.aspx.cs" Inherits="BKBilling.Forms.ActivityForm" %>
<%@ Register Src="~/Components/StandardGrid.ascx" TagName="StandardGrid" TagPrefix="uc1" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Ledger Master | BK Softwares</title>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <style>
        body { background: #f8fafc; background-image: radial-gradient(#cbd5e1 0.7px, transparent 0.7px); background-size: 24px 24px; min-height: 100vh; padding: 20px 10px; font-family: 'Inter', sans-serif; }
        .container-fluid { padding-right: 0px; padding-left: 0px; }
        .card-custom { background: white; border-radius: 16px; border: 1px solid #e2e8f0; box-shadow: 0 10px 15px -3px rgba(0,0,0,0.1); margin: 0 auto; }
        .section-title { font-size: 0.75rem; font-weight: 800; color: #6366f1; text-transform: uppercase; letter-spacing: 1px; border-bottom: 1px solid #f1f5f9; padding-bottom: 8px; margin-bottom: 15px; display: flex; align-items: center; }
        .section-title i { margin-right: 8px; }
        .form-label { font-weight: 600; font-size: 0.78rem; color: #475569; margin-bottom: 3px; }
        .form-control-sm, .form-select-sm { border-radius: 8px; border: 1px solid #cbd5e1; padding: 8px 12px; }
        .modern-switch {
            display: flex; align-items: center; background: #f8fafc;
            padding: 10px 15px 10px 45px; border-radius: 12px; border: 1px solid #e2e8f0;
            position: relative; min-height: 45px; transition: all 0.2s;
        }
        .modern-switch span { position: absolute; left: 10px; display: flex; align-items: center; }
        .modern-switch .form-check-input {
            width: 2.5em !important; height: 1.25em !important; margin-left: 0 !important;
            cursor: pointer; appearance: none; -webkit-appearance: none; background-color: #cbd5e1; border: none !important;
        }
        .modern-switch .form-check-input:checked { background-color: #10b981 !important; }
        .modern-switch label { font-weight: 600; color: #334155; cursor: pointer; font-size: 0.85rem; margin: 0 0 0 5px; }

        /* --- CUSTOM LEDGER TABLE (replaces StandardGrid usercontrol) --- */
        .erp-grid-main {
            height: 480px; border: 1px solid #d1dbe5; border-radius: 8px; display: flex;
            flex-direction: column; flex-grow: 1; background: #fff; overflow: hidden;
            box-shadow: 0 4px 15px rgba(0,0,0,0.05);
        }
        .erp-top-toolbar {
            display: flex; justify-content: space-between; align-items: center;
            padding: 14px 20px; border-bottom: 1px solid #e2e8f0; background: #fff; flex-shrink: 0;
        }
        .std-search-group { max-width: 350px; width: 100%; position: relative; }
        .std-search-group i { position: absolute; left: 12px; top: 10px; color: #94a3b8; z-index: 5; }
        .std-search-group .form-control { padding-left: 35px; border-radius: 6px; border: 1px solid #cbd5e1; font-size: 13px; }
        .btn-action-round {
            width: 32px; height: 32px; border-radius: 50%; border: 1px solid #e2e8f0;
            background: #fff; color: #64748b; display: inline-flex; align-items: center;
            justify-content: center; cursor: pointer; text-decoration: none; transition: 0.2s;
        }
        .btn-action-round:hover { background: #f1f5f9; color: #6366f1; }
        .table-scroll-area { flex-grow: 1; overflow: auto; position: relative; }
        .table-scroll-area::-webkit-scrollbar { width: 8px; height: 8px; }
        .table-scroll-area::-webkit-scrollbar-thumb { background: #cbd5e1; border-radius: 10px; }
        .gv-ledgers { width: 100%; border-collapse: collapse; font-size: 12.8px; }
        .gv-ledgers th {
            background: #e9eff8 !important; color: #475569; padding: 10px 15px;
            border: 1px solid #d1dbe5; text-transform: uppercase; font-size: 11px;
            font-weight: 700; position: sticky; top: 0; z-index: 20; text-align: left;
        }
        .gv-ledgers th a { color: #475569; text-decoration: none; }
        .gv-ledgers td { padding: 9px 15px; border: 1px solid #eef2f6; vertical-align: middle; }
        .gv-ledgers tr:hover td { background: #f8fafc; }
        .erp-grid-footer {
            padding: 10px 20px; background: #f8fafc; border-top: 1px solid #d1dbe5;
            display: flex; justify-content: space-between; align-items: center;
            font-size: 12px; color: #64748b; flex-shrink: 0;
        }
    </style>
    <script>
        function showNotification(message, type) {
            setTimeout(function () {
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
            }, 100);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <asp:HiddenField ID="hfLedgerID" runat="server" />

        <div class="container-fluid">
            <!-- VIEW 1: DIRECTORY LIST -->
            <asp:Panel ID="pnlList" runat="server">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h4 class="fw-bold m-0 text-dark"><i class="fas fa-address-book me-2 text-primary"></i>Account Directory</h4>
                        <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn btn-primary px-4 fw-bold shadow-sm" OnClick="btnOpenCreate_Click">
                            <i class="fas fa-plus me-1"></i> New Ledger
                        </asp:LinkButton>
                    </div>
                    <uc1:StandardGrid ID="gridLedgers" runat="server" />
                </div>
            </asp:Panel>

            <!-- VIEW 2: SETUP FORM (unchanged) -->
            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4 border-bottom pb-3">
                        <div>
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="text-decoration-none small text-muted fw-bold" OnClick="btnBack_Click">
                                <i class="fas fa-arrow-left me-1"></i> BACK TO LIST
                            </asp:LinkButton>
                            <h4 class="fw-bold m-0 mt-1">Ledger Setup</h4>
                        </div>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success px-5 fw-bold shadow-sm" OnClick="btnSave_Click">
                            <i class="fas fa-save me-2"></i>SAVE LEDGER
                        </asp:LinkButton>
                    </div>

                    <div class="row g-4">
                        <!-- COLUMN 1: IDENTITY -->
                        <div class="col-md-4 border-end">
                            <p class="section-title"><i class="fas fa-id-card"></i> 1. Identity & Limits</p>
                            <div class="mb-3">
                                <label class="form-label">Ledger Name *</label>
                                <asp:TextBox ID="txtLedgerName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-6">
                                    <label class="form-label">Ledger Code</label>
                                    <asp:TextBox ID="txtLedgerCode" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <label class="form-label">Status</label>
                                    <asp:DropDownList ID="ddlActive" runat="server" CssClass="form-select form-select-sm">
                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                        <asp:ListItem Value="0">Inactive</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-6">
                                    <label class="form-label">Account Group</label>
                                    <asp:DropDownList ID="ddlGroup" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                                <div class="col-6">
                                    <label class="form-label">Area / Route</label>
                                    <asp:DropDownList ID="ddlArea" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-6">
                                    <label class="form-label">Credit Limit (Amt)</label>
                                    <asp:TextBox ID="txtCreditLimit" runat="server" CssClass="form-control form-control-sm" Text="0"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <label class="form-label">Credit Limit (Days)</label>
                                    <asp:TextBox ID="txtCreditDays" runat="server" CssClass="form-control form-control-sm" Text="0"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modern-switch form-check form-switch mb-3">
                                <asp:CheckBox ID="chkTDS" runat="server" CssClass="form-check-input" />
                                <label for="<%= chkTDS.ClientID %>">TDS/TCS Applicable?</label>
                            </div>
                            <div>
                                <label class="form-label">PAN No</label>
                                <asp:TextBox ID="txtPAN" runat="server" CssClass="form-control form-control-sm" MaxLength="10" placeholder="ABCDE1234F"></asp:TextBox>
                            </div>
                        </div>

                        <!-- COLUMN 2: GST & BANKING -->
                        <div class="col-md-4 border-end">
                            <p class="section-title"><i class="fas fa-file-invoice-dollar"></i> 2. GST Compliance</p>
                            <div class="modern-switch form-check form-switch mb-3">
                                <asp:CheckBox ID="chkUseGST" runat="server" CssClass="form-check-input" Checked="true" />
                                <label for="<%= chkUseGST.ClientID %>">Enable GST for this Ledger</label>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">GSTIN / Tax No</label>
                                <asp:TextBox ID="txtGSTIN" runat="server" CssClass="form-control form-control-sm" MaxLength="15"></asp:TextBox>
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-6">
                                    <label class="form-label">Dealer Type</label>
                                    <asp:DropDownList ID="ddlDealerType" runat="server" CssClass="form-select form-select-sm">
                                        <asp:ListItem>Regular</asp:ListItem>
                                        <asp:ListItem>Composition</asp:ListItem>
                                        <asp:ListItem>Unregistered</asp:ListItem>
                                        <asp:ListItem>Consumer</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-6">
                                    <label class="form-label">GST State</label>
                                    <asp:DropDownList ID="ddlGSTState" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                            </div>

                            <p class="section-title mt-4"><i class="fas fa-university"></i> 3. Banking</p>
                            <div class="mb-2">
                                <label class="form-label">Bank Name</label>
                                <asp:TextBox ID="txtBank" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Bank Name"></asp:TextBox>
                                <asp:TextBox ID="txtBranch" runat="server" CssClass="form-control form-control-sm" placeholder="Branch"></asp:TextBox>
                            </div>
                            <div class="mb-2">
                                <label class="form-label">A/c No & IFSC</label>
                                <asp:TextBox ID="txtAcNo" runat="server" CssClass="form-control form-control-sm mb-1" placeholder="Account No"></asp:TextBox>
                                <asp:TextBox ID="txtIfsc" runat="server" CssClass="form-control form-control-sm" placeholder="IFSC Code"></asp:TextBox>
                            </div>
                        </div>

                        <!-- COLUMN 3: ADDRESS & FINANCIALS -->
                        <div class="col-md-4">
                            <p class="section-title"><i class="fas fa-map-marked-alt"></i> 4. Address & Contact</p>
                            <asp:TextBox ID="txtAdd1" runat="server" CssClass="form-control form-control-sm mb-2" placeholder="Building/Plot No"></asp:TextBox>
                            <asp:TextBox ID="txtAdd2" runat="server" CssClass="form-control form-control-sm mb-2" placeholder="Street/Locality"></asp:TextBox>
                            <asp:TextBox ID="txtAdd3" runat="server" CssClass="form-control form-control-sm mb-3" placeholder="City/Pincode"></asp:TextBox>

                            <div class="mb-3">
                                <label class="form-label">Contact Person</label>
                                <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control form-control-sm mb-2" placeholder="Name"></asp:TextBox>
                                <div class="row g-2">
                                    <div class="col-6"><asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-sm" placeholder="Email"></asp:TextBox></div>
                                    <div class="col-6"><asp:TextBox ID="txtPhone" runat="server" CssClass="form-control form-control-sm" placeholder="Phone"></asp:TextBox></div>
                                </div>
                            </div>

                            <p class="section-title mt-4"><i class="fas fa-coins"></i> 5. Financials</p>
                            <div class="row g-2 mb-3">
                                <div class="col-7">
                                    <label class="form-label">Opening Balance</label>
                                    <asp:TextBox ID="txtOpening" runat="server" CssClass="form-control form-control-sm" Text="0.00"></asp:TextBox>
                                </div>
                                <div class="col-5">
                                    <label class="form-label">Type</label>
                                    <asp:DropDownList ID="ddlBalType" runat="server" CssClass="form-select form-select-sm">
                                        <asp:ListItem>Debit</asp:ListItem>
                                        <asp:ListItem>Credit</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Remarks</label>
                                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Notification Toast -->
            <div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3">
                <div id="msgToast" class="toast align-items-center text-white border-0 shadow-lg" role="alert">
                    <div class="d-flex">
                        <div class="toast-body"><i id="msgIcon"></i> <span id="msgText"></span></div>
                        <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>

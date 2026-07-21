<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SupplierMaster.aspx.cs" Inherits="BKBilling.Forms.Master.SupplierMaster" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <title>Customer Master | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body { background: #f8fafc; background-image: radial-gradient(#cbd5e1 0.7px, transparent 0.7px); background-size: 24px 24px; min-height: 100vh; padding: 20px 10px; font-family: 'Inter', sans-serif; }
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
        
        .gv-style th { background: #f8fafc; color: #64748b; font-size: 0.75rem; text-transform: uppercase; padding: 12px; border-bottom: 1px solid #e2e8f0; }
        .gv-style td { vertical-align: middle; padding: 12px; border-bottom: 1px solid #f1f5f9; }
        .badge-status { font-weight: 700; font-size: 0.75rem; padding: 4px 10px; border-radius: 6px; text-transform: uppercase; }
        .badge-active { background-color: #ecfdf5; color: #10b981; }
        .badge-inactive { background-color: #fef2f2; color: #ef4444; }
    </style>
    <script>
        function showNotification(message, type) {
            var toastEl = document.getElementById('msgToast');
            var msgText = document.getElementById('msgText');
            var msgIcon = document.getElementById('msgIcon');
            msgText.innerText = message;
            toastEl.classList.remove('bg-danger', 'bg-success', 'bg-primary');
            if (type === 'error') { toastEl.classList.add('bg-danger'); msgIcon.className = "fas fa-exclamation-triangle me-2"; }
            else if (type === 'success') { toastEl.classList.add('bg-success'); msgIcon.className = "fas fa-check-circle me-2"; }
            else { toastEl.classList.add('bg-primary'); msgIcon.className = "fas fa-info-circle me-2"; }
            var toast = new bootstrap.Toast(toastEl, { delay: 4000 });
            toast.show();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <asp:HiddenField ID="hfCustomerID" runat="server" />

        <div class="container-fluid">
            <!-- VIEW 1: LIST -->
            <asp:Panel ID="pnlList" runat="server">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <div>
                            <h4 class="fw-bold m-0 text-dark"><i class="fas fa-users me-2 text-primary"></i>Supplier Directory</h4>
                            <p class="text-muted small m-0">Group: Sundry Creditors (1000000033)</p>
                        </div>
                        <div class="d-flex gap-3">
                            <div class="input-group input-group-sm" style="width: 300px;">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search name or mobile..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" />
                                <span class="input-group-text bg-light border-start-0"><i class="fas fa-search text-muted"></i></span>
                            </div>
                            <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn btn-primary px-4 fw-bold shadow-sm" OnClick="btnOpenCreate_Click">
                                <i class="fas fa-plus-circle me-2"></i>New Supplier
                            </asp:LinkButton>
                        </div>
                    </div>
                    <asp:GridView ID="gvCustomers" runat="server" AutoGenerateColumns="false" CssClass="table gv-style" 
                        GridLines="None" DataKeyNames="Ledger_Sno" OnRowCommand="gvCustomers_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="ledger_code" HeaderText="Code" ItemStyle-Width="100px" />
                            <asp:BoundField DataField="ledger_name" HeaderText="Customer Name" ItemStyle-CssClass="fw-bold text-dark" />
                            <asp:BoundField DataField="Ledger_Phone" HeaderText="Mobile" />
                            <asp:BoundField DataField="Area_Name" HeaderText="Area" />
                            <asp:BoundField DataField="Ledger_open" HeaderText="Opening Bal" DataFormatString="{0:N2}" />
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("ledger_Active")) ? "badge-status badge-active" : "badge-status badge-inactive" %>'>
                                        <%# Convert.ToBoolean(Eval("ledger_Active")) ? "ACTIVE" : "INACTIVE" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-end pe-3">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-outline-primary fw-bold" 
                                        CommandName="EditRecord" CommandArgument='<%# Eval("Ledger_Sno") %>'><i class="fas fa-pen-to-square me-1"></i>Edit</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate><div class="p-5 text-center text-muted">No Supplier found.</div></EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </asp:Panel>

            <!-- VIEW 2: FORM -->
            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4 border-bottom pb-3">
                        <div>
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="text-decoration-none small text-muted fw-bold" OnClick="btnBack_Click">
                                <i class="fas fa-arrow-left me-1"></i> BACK TO DIRECTORY
                            </asp:LinkButton>
                            <h3 class="fw-bold m-0 mt-1 text-dark">Supplier Setup</h3>
                        </div>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success px-5 fw-bold shadow-sm" OnClick="btnSave_Click">
                            <i class="fas fa-check-circle me-2"></i>Save Supplier
                        </asp:LinkButton>
                    </div>

                    <div class="row g-4">
                        <!-- Identity -->
                        <div class="col-md-4 border-end">
                            <p class="section-title"><i class="fas fa-id-card"></i> 1. Basic Identity</p>
                            <div class="mb-3">
                                <label class="form-label">Supplier Name *</label>
                                <asp:TextBox ID="txtCustName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-6">
                                    <label class="form-label">Supplier Code *</label>
                                    <asp:TextBox ID="txtCustCode" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
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
                                <asp:CheckBox ID="chkActive" runat="server" CssClass="form-check-input" Checked="true" />
                                <label for="<%= chkActive.ClientID %>">Account Active?</label>
                            </div>
                        </div>

                        <!-- Contact -->
                        <div class="col-md-4 border-end">
                            <p class="section-title"><i class="fas fa-map-location-dot"></i> 2. Contact & Address</p>
                            <div class="mb-3">
                                <label class="form-label">Contact Person</label>
                                <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-6">
                                    <label class="form-label">Mobile Number *</label>
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control form-control-sm" MaxLength="15"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <label class="form-label">Email Address</label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <asp:TextBox ID="txtAdd1" runat="server" CssClass="form-control form-control-sm mb-2" placeholder="Address Line 1"></asp:TextBox>
                            <asp:TextBox ID="txtAdd2" runat="server" CssClass="form-control form-control-sm mb-2" placeholder="Address Line 2"></asp:TextBox>
                            <asp:TextBox ID="txtAdd3" runat="server" CssClass="form-control form-control-sm" placeholder="City / Pincode"></asp:TextBox>
                        </div>

                        <!-- Statutory -->
                        <div class="col-md-4">
                            <p class="section-title"><i class="fas fa-file-invoice-dollar"></i> 3. Tax & Opening</p>
                            <div class="mb-3">
                                <label class="form-label">GSTIN Number</label>
                                <asp:TextBox ID="txtGST" runat="server" CssClass="form-control form-control-sm" MaxLength="15"></asp:TextBox>
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-7">
                                    <label class="form-label">GST State</label>
                                    <asp:DropDownList ID="ddlState" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                                <div class="col-5">
                                    <label class="form-label">Dealer Type</label>
                                    <asp:DropDownList ID="ddlDealer" runat="server" CssClass="form-select form-select-sm">
                                        <asp:ListItem>Unregistered</asp:ListItem>
                                        <asp:ListItem>Regular</asp:ListItem>
                                        <asp:ListItem>Consumer</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-7">
                                    <label class="form-label">Opening Balance</label>
                                    <asp:TextBox ID="txtOpening" runat="server" CssClass="form-control form-control-sm" Text="0.00"></asp:TextBox>
                                </div>
                                <div class="col-5">
                                    <label class="form-label">Balance Type</label>
                                    <asp:DropDownList ID="ddlBalType" runat="server" CssClass="form-select form-select-sm">
                                        <asp:ListItem>Debit</asp:ListItem>
                                        <asp:ListItem>Credit</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="2" placeholder="Remarks..."></asp:TextBox>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- TOAST -->
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

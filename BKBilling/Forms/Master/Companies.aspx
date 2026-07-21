<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Companies.aspx.cs" Inherits="BKBilling.Forms.Master.Companies" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Company & User Management | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function showNotification(message, type) {
            var toastEl = document.getElementById('msgToast');
            var msgText = document.getElementById('msgText');
            var msgIcon = document.getElementById('msgIcon');
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

        function confirmWorkspaceInit() {
            if (confirm("Do you want to initialize or update the workspace (Default Ledgers, Groups, and Tables) for this company?")) {
                document.getElementById('<%= hfInitWorkspace.ClientID %>').value = "true";
            } else {
                document.getElementById('<%= hfInitWorkspace.ClientID %>').value = "false";
            }
            return true;
        }
    </script>
    <style>
        body { background-color: #f1f5f9; padding: 20px; font-family: 'Inter', sans-serif; }
        .card-custom { background: white; border-radius: 12px; border: none; box-shadow: 0 10px 15px -3px rgba(0,0,0,0.1); }
        .section-title { font-size: 0.85rem; font-weight: 700; color: #4f46e5; text-transform: uppercase; letter-spacing: 1px; border-bottom: 2px solid #f1f5f9; padding-bottom: 8px; margin-bottom: 15px; }
        .form-label { font-weight: 600; font-size: 0.78rem; color: #475569; margin-bottom: 3px; }
        .form-control-sm, .form-select-sm { border-radius: 6px; border: 1px solid #cbd5e1; }
        .admin-section { background-color: #f0f9ff; border: 1px solid #bae6fd; border-radius: 12px; padding: 20px; height: 100%; }
        .admin-header { color: #0369a1 !important; border-bottom-color: #bae6fd !important; }
        .gv-style { border-radius: 10px; overflow: hidden; border: none !important; }
        .gv-style th { background: #f8fafc; color: #64748b; font-size: 0.75rem; text-transform: uppercase; padding: 12px; }
        .gv-style td { padding: 12px; font-size: 0.9rem; border-bottom: 1px solid #f1f5f9; }
        .toast-container { z-index: 9999; }
        .toast { min-width: 320px; border-radius: 12px; border: none; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <asp:HiddenField ID="hfCompanySno" runat="server" />
        <asp:HiddenField ID="hfInitWorkspace" runat="server" Value="false" />

        <div class="container-fluid">
            <asp:Panel ID="pnlList" runat="server">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h4 class="fw-bold m-0 text-dark"><i class="fas fa-university me-2 text-primary"></i>Company Directory</h4>
                        <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn btn-primary px-4 fw-bold shadow-sm" OnClick="btnOpenCreate_Click">
                            <i class="fas fa-plus-circle me-2"></i>New Registration
                        </asp:LinkButton>
                    </div>
                    <asp:GridView ID="gvCompanies" runat="server" AutoGenerateColumns="false" CssClass="table gv-style" 
                        GridLines="None" DataKeyNames="Company_Sno" OnRowCommand="gvCompanies_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Company_Sno" HeaderText="ID" />
                            <asp:BoundField DataField="Company_Name" HeaderText="Company Name" />
                            <asp:BoundField DataField="GSTIN" HeaderText="GSTIN" />
                            <asp:BoundField DataField="Phone" HeaderText="Phone" />
                            <asp:BoundField DataField="Created_Date" HeaderText="Reg. Date" DataFormatString="{0:dd-MMM-yyyy}" />
                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-outline-primary" 
                                        CommandName="EditRecord" CommandArgument='<%# Eval("Company_Sno") %>'>
                                        <i class="fas fa-edit me-1"></i>Edit
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4 border-bottom pb-3">
                        <div>
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="text-decoration-none small text-muted fw-bold" OnClick="btnBack_Click">
                                <i class="fas fa-arrow-left me-1"></i> Back
                            </asp:LinkButton>
                            <h4 class="fw-bold m-0 mt-1">Company & Admin Setup</h4>
                        </div>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success px-5 fw-bold shadow-sm" 
                            OnClick="btnSave_Click" OnClientClick="return confirmWorkspaceInit();">
                            <i class="fas fa-save me-2"></i>SAVE ALL DETAILS
                        </asp:LinkButton>
                    </div>

                    <div class="row g-4">
                        <!-- Left Column -->
                        <div class="col-md-4 border-end">
                            <p class="section-title">1. Company Identity</p>
                            <div class="mb-3">
                                <label class="form-label">Company Name *</label>
                                <asp:TextBox ID="C_Name" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-6">
                                    <label class="form-label">GSTIN</label>
                                    <asp:TextBox ID="C_GST" runat="server" CssClass="form-control form-control-sm" MaxLength="15"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <label class="form-label">PAN</label>
                                    <asp:TextBox ID="C_PAN" runat="server" CssClass="form-control form-control-sm" MaxLength="10"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row g-2 mb-3">
                                <div class="col-6">
                                    <label class="form-label">FY Start</label>
                                    <asp:TextBox ID="C_FY" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <label class="form-label">Currency Symbol</label>
                                    <asp:TextBox ID="C_CurSym" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
                            <p class="section-title mt-4">2. Contact & Regional</p>
                            <div class="mb-2">
                                <label class="form-label">Company Phone</label>
                                <asp:TextBox ID="C_Phone" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="mb-2">
                                <label class="form-label">Company Email</label>
                                <asp:TextBox ID="C_Email" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="mb-2">
                                <label class="form-label">Currency Format</label>
                                <asp:DropDownList ID="C_CurFmt" runat="server" CssClass="form-select form-select-sm">
                                    <asp:ListItem Value="INR">Indian (Lakhs)</asp:ListItem>
                                    <asp:ListItem Value="USD">International (Millions)</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>

                        <!-- Middle Column -->
                        <div class="col-md-3 border-end">
                            <p class="section-title">3. Registered Address</p>
                            <div class="mb-3">
                                <label class="form-label">Address Line 1</label>
                                <asp:TextBox ID="C_Add1" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Address Line 2</label>
                                <asp:TextBox ID="C_Add2" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">State</label>
                                <asp:TextBox ID="C_State" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Country</label>
                                <asp:TextBox ID="C_Country" runat="server" CssClass="form-control form-control-sm" Text="India"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Right Column (Admin) -->
                        <div class="col-md-5">
                            <div class="admin-section">
                                <p class="section-title admin-header">4. Admin Profile</p>
                                <div class="row g-2 mb-2">
                                    <div class="col-7">
                                        <label class="form-label">Full Name *</label>
                                        <asp:TextBox ID="U_FullName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-5">
                                        <label class="form-label">Join Date</label>
                                        <asp:TextBox ID="U_JoinDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row g-2 mb-2">
                                    <div class="col-6">
                                        <label class="form-label">Username *</label>
                                        <asp:TextBox ID="U_Username" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-6">
                                        <label class="form-label">User Phone</label>
                                        <asp:TextBox ID="U_Phone" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="mb-2">
                                    <label class="form-label">User Email</label>
                                    <asp:TextBox ID="U_Email" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                                <div class="row g-2 mb-2">
                                    <div class="col-6">
                                        <label class="form-label">User Address 1</label>
                                        <asp:TextBox ID="U_Add1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-6">
                                        <label class="form-label">User Address 2</label>
                                        <asp:TextBox ID="U_Add2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                    </div>
                                </div>

                                <p class="section-title admin-header mt-4">5. Security</p>
                                <div class="row g-2">
                                    <div class="col-6">
                                        <label class="form-label">Password</label>
                                        <asp:TextBox ID="U_Pass" runat="server" CssClass="form-control form-control-sm" TextMode="Password" placeholder="Required for new"></asp:TextBox>
                                    </div>
                                    <div class="col-6">
                                        <label class="form-label">Confirm Password</label>
                                        <asp:TextBox ID="U_Confirm" runat="server" CssClass="form-control form-control-sm" TextMode="Password"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

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
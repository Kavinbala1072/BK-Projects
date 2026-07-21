<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeaveTypeMaster.aspx.cs" Inherits="BKBilling.Forms.Inventory.WeaveTypeMaster" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Weave Type Master | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body { background: #f8fafc; background-image: radial-gradient(#cbd5e1 0.7px, transparent 0.7px); background-size: 24px 24px; min-height: 100vh; padding: 40px 20px; font-family: 'Inter', sans-serif; }
        .card-custom { background: #ffffff; border-radius: 16px; border: 1px solid #e2e8f0; box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1); margin: 0 auto; overflow: hidden; }
        .form-header { background: #ffffff; padding: 25px 40px; border-bottom: 1px solid #f1f5f9; }
        .btn-save { background-color: #10b981; border: none; color: white; padding: 10px 24px; border-radius: 8px; font-weight: 700; transition: 0.2s; text-decoration: none; }
        .btn-save:hover { background-color: #059669; transform: translateY(-1px); color: white; }
        .section-title { font-size: 0.75rem; font-weight: 800; color: #6366f1; text-transform: uppercase; letter-spacing: 0.1em; margin-bottom: 25px; display: flex; align-items: center; }
        .section-title::after { content: ""; height: 1px; flex-grow: 1; background: #f1f5f9; margin-left: 15px; }
        .form-body { padding: 40px; }
        .form-label { font-weight: 600; font-size: 0.82rem; color: #334155; margin-bottom: 6px; }
        .form-control-sm { padding: 10px 12px; border: 1px solid #e2e8f0; border-radius: 8px; }
        .gv-style th { background: #f8fafc; color: #64748b; font-size: 0.75rem; text-transform: uppercase; padding: 15px; border-bottom: 1px solid #e2e8f0; }
        .gv-style td { padding: 15px; font-size: 0.9rem; color: #334155; border-bottom: 1px solid #f1f5f9; vertical-align: middle; }
        .badge-status { font-weight: 700; font-size: 0.75rem; padding: 4px 10px; border-radius: 6px; text-transform: uppercase; }
        .badge-active { background-color: #ecfdf5; color: #10b981; }
        .badge-inactive { background-color: #fef2f2; color: #ef4444; }
        .modern-switch { display: flex; align-items: center; background: #f8fafc; padding: 10px 15px 10px 45px; border-radius: 12px; border: 1px solid #e2e8f0; position: relative; min-height: 45px; width: fit-content; min-width: 280px; }
        .modern-switch span { position: absolute; left: 10px; display: flex; align-items: center; }
        .modern-switch .form-check-input { width: 2.5em !important; height: 1.25em !important; margin-left: 0 !important; cursor: pointer; appearance: none; -webkit-appearance: none; background-color: #cbd5e1; border: none !important; }
        .modern-switch .form-check-input:checked { background-color: #10b981 !important; }
        .modern-switch label { font-weight: 600; color: #334155; cursor: pointer; font-size: 0.88rem; margin: 0 0 0 5px; }
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
        <asp:HiddenField ID="hfWeaveSno" runat="server" />

        <div class="container-fluid">
            <!-- VIEW 1: DIRECTORY LIST -->
            <asp:Panel ID="pnlList" runat="server">
                <div class="card-custom">
                    <div class="form-header d-flex justify-content-between align-items-center">
                        <div>
                            <h3 class="fw-bold m-0 text-dark">Weave Type Directory</h3>
                            <p class="text-muted small m-0">Classification for fabric construction (Handloom, Power loom, etc.)</p>
                        </div>
                        <div class="d-flex gap-3">
                            <div class="input-group input-group-sm" style="width: 280px;">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" />
                                <span class="input-group-text bg-light border-start-0"><i class="fas fa-search text-muted"></i></span>
                            </div>
                            <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn btn-primary px-4 fw-bold shadow-sm" OnClick="btnOpenCreate_Click">
                                <i class="fas fa-plus me-2"></i>New Weave Type
                            </asp:LinkButton>
                        </div>
                    </div>
                    
                    <asp:GridView ID="gvWeave" runat="server" AutoGenerateColumns="false" CssClass="table gv-style" 
                        GridLines="None" DataKeyNames="Weave_Sno" OnRowCommand="gvWeave_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Weave_Sno" HeaderText="ID" ItemStyle-Width="120px" ItemStyle-CssClass="text-muted font-monospace small" />
                            <asp:BoundField DataField="Weave_Name" HeaderText="Weave Type Name" ItemStyle-CssClass="fw-bold text-dark" />
                            
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge-status badge-active" : "badge-status badge-inactive" %>'>
                                        <%# Convert.ToBoolean(Eval("IsActive")) ? "ACTIVE" : "INACTIVE" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-end pe-4">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-outline-primary fw-bold" 
                                        CommandName="EditRecord" CommandArgument='<%# Eval("Weave_Sno") %>'>
                                        <i class="fas fa-pen-to-square me-1"></i>Edit
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>

            <!-- VIEW 2: SETUP FORM -->
            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <div class="card-custom">
                    <div class="form-header d-flex justify-content-between align-items-center">
                        <div>
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="text-decoration-none small text-muted fw-bold d-block mb-1" OnClick="btnBack_Click">
                                <i class="fas fa-arrow-left me-1"></i> BACK TO DIRECTORY
                            </asp:LinkButton>
                            <h3 class="fw-bold m-0 text-dark">Weave Setup</h3>
                        </div>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn-save shadow-sm" OnClick="btnSave_Click" ValidationGroup="save">
                            <i class="fas fa-check-circle me-2"></i>SAVE WEAVE TYPE
                        </asp:LinkButton>
                    </div>

                    <div class="form-body">
                        <div class="row g-5">
                            <div class="col-md-7">
                                <p class="section-title">Identity</p>
                                <div class="mb-4">
                                    <label class="form-label">Weave Type Name <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtWeaveName" runat="server" CssClass="form-control form-control-sm" placeholder="e.g. HANDLOOM"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtWeaveName" ErrorMessage="Name is required" CssClass="text-danger small mt-1 d-block" Display="Dynamic" ValidationGroup="save" />
                                </div>
                            </div>

                            <div class="col-md-5 border-start ps-md-5">
                                <p class="section-title">Configuration</p>
                                <div class="modern-switch form-check form-switch mb-4">
                                    <asp:CheckBox ID="chkActive" runat="server" CssClass="form-check-input" Checked="true" />
                                    <label for="<%= chkActive.ClientID %>">Is this weave type active?</label>
                                </div>
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
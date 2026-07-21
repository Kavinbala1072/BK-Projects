<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AreaMaster.aspx.cs" Inherits="BKBilling.Forms.Master.AreaMaster" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Area Master | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body { background-color: #f1f5f9; padding: 20px 10px; font-family: 'Inter', sans-serif; }
        .card-custom { background: white; border-radius: 12px; border: none; box-shadow: 0 10px 15px -3px rgba(0,0,0,0.1); }
        .section-title { font-size: 0.85rem; font-weight: 700; color: #4f46e5; text-transform: uppercase; letter-spacing: 1px; border-bottom: 2px solid #f1f5f9; padding-bottom: 8px; margin-bottom: 15px; }
        .form-label { font-weight: 600; font-size: 0.78rem; color: #475569; margin-bottom: 3px; }
        .form-control-sm, .form-select-sm { border-radius: 6px; border: 1px solid #cbd5e1; }
        
        /* GridView Styling */
        .gv-style { border-radius: 10px; overflow: hidden; border: none !important; }
        .gv-style th { background: #f8fafc; color: #64748b; font-size: 0.75rem; text-transform: uppercase; padding: 12px; }
        .gv-style td { padding: 12px; font-size: 0.9rem; border-bottom: 1px solid #f1f5f9; vertical-align: middle; }

.modern-switch {
    display: inline-flex;
    align-items: center;
    gap: 12px;
    padding: 12px 18px;
    background: #fff;
    border: 1px solid #dbe3eb;
    border-radius: 12px;
    box-shadow: 0 2px 8px rgba(0,0,0,.05);
    transition: .25s;
}

.modern-switch:hover {
    border-color: #2563eb;
    box-shadow: 0 5px 15px rgba(37,99,235,.12);
}

/* ASP.NET wrapper */
.modern-switch span {
    display: flex;
    align-items: center;
}

/* Bootstrap Switch */
.modern-switch .form-check-input {
    width: 3rem;
    height: 1.5rem;
    cursor: pointer;
    margin: 0;
    background-color: #cbd5e1;
    border: none;
}

.modern-switch .form-check-input:checked {
    background-color: #16a34a;
}

.modern-switch .form-check-label {
    font-size: 15px;
    font-weight: 600;
    color: #334155;
    margin: 0;
    cursor: pointer;
    user-select: none;
}
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
        <asp:HiddenField ID="hfAreaId" runat="server" />

        <div class="container-fluid">
            <!-- VIEW 1: LIST VIEW -->
            <asp:Panel ID="pnlList" runat="server">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h4 class="fw-bold m-0 text-dark"><i class="fas fa-map-location-dot me-2 text-primary"></i>Area Directory</h4>
                        <div class="d-flex gap-2">
                             <div class="input-group input-group-sm" style="width: 250px;">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search areas..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" />
                                <span class="input-group-text bg-white"><i class="fas fa-search text-muted"></i></span>
                            </div>
                            <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn btn-primary px-4 fw-bold shadow-sm" OnClick="btnOpenCreate_Click">
                                <i class="fas fa-plus-circle me-2"></i>New Area
                            </asp:LinkButton>
                        </div>
                    </div>
                    
                    <asp:GridView ID="gvArea" runat="server" AutoGenerateColumns="false" CssClass="table gv-style" 
                        GridLines="None" DataKeyNames="Area_Sno" OnRowCommand="gvArea_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Area_Sno" HeaderText="ID" ItemStyle-Width="120px" />
                            <asp:BoundField DataField="Area_Name" HeaderText="Area Name" ItemStyle-CssClass="fw-bold text-dark" />
                            <asp:BoundField DataField="ParentAreaName" HeaderText="Under Area" />
                            
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <%-- Removed Toggle LinkButton, showing simple status label --%>
                                    <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge-status badge-active" : "badge-status badge-inactive" %>'>
                                        <i class='<%# Convert.ToBoolean(Eval("IsActive")) ? "fas fa-check me-1" : "fas fa-times me-1" %>'></i>
                                        <%# Convert.ToBoolean(Eval("IsActive")) ? "ACTIVE" : "INACTIVE" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-end pe-4">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-outline-primary" 
                                        CommandName="EditArea" CommandArgument='<%# Eval("Area_Sno") %>'>
                                        <i class="fas fa-edit me-1"></i>Edit
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>

            <!-- VIEW 2: FORM VIEW -->
            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <div class="card-custom p-4" style="max-width: 600px; margin: 0 auto;">
                    <div class="d-flex justify-content-between align-items-center mb-4 border-bottom pb-3">
                        <div>
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="text-decoration-none small text-muted fw-bold" OnClick="btnBack_Click">
                                <i class="fas fa-arrow-left me-1"></i> Back
                            </asp:LinkButton>
                            <h4 class="fw-bold m-0 mt-1">Area Setup</h4>
                        </div>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success px-5 fw-bold shadow-sm" OnClick="btnSave_Click" ValidationGroup="save">
                            <i class="fas fa-save me-2"></i>SAVE ALL DETAILS
                        </asp:LinkButton>
                    </div>

                    <div class="row g-3">
                        <div class="col-12">
                            <p class="section-title">Identity</p>
                            <label class="form-label">Area / Route Name *</label>
                            <asp:TextBox ID="txtAreaName" runat="server" CssClass="form-control form-control-sm" placeholder="Enter name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="txtAreaName" ErrorMessage="Area name is required" CssClass="text-danger small" Display="Dynamic" ValidationGroup="save" />
                        </div>
                        <div class="col-12">
                            <label class="form-label">Under Area (Parent Group)</label>
                            <asp:DropDownList ID="ddlAreaUnder" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                        </div>

                        <div class="col-12 mt-4 pt-3 border-top">
                            <p class="section-title">Configuration</p>
                                <div class="form-check form-switch modern-switch">
                                    <asp:CheckBox ID="chkIsActive"
                                        runat="server"
                                        CssClass="form-check-input" />

                                    <label class="form-check-label ms-2"
                                        for="<%= chkIsActive.ClientID %>">
                                        Is this Area currently active?
                                    </label>
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
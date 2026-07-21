<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserMaster.aspx.cs" Inherits="BKBilling.Forms.Master.UserMaster" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>User Management | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body { background-color: #f1f5f9; padding: 20px; font-family: 'Inter', sans-serif; }
        .card-custom { background: white; border-radius: 12px; border: none; box-shadow: 0 10px 15px -3px rgba(0,0,0,0.1); }
        .section-title { font-size: 0.85rem; font-weight: 700; color: #4f46e5; text-transform: uppercase; letter-spacing: 1px; border-bottom: 2px solid #f1f5f9; padding-bottom: 8px; margin-bottom: 15px; }
        .form-label { font-weight: 600; font-size: 0.78rem; color: #475569; margin-bottom: 3px; }
        .gv-style th { background: #f8fafc; color: #64748b; font-size: 0.75rem; text-transform: uppercase; padding: 12px; }
        .gv-style td { vertical-align: middle; }
    </style>
    <script>
        function showNotification(message, type) {
            var toastEl = document.getElementById('msgToast');
            document.getElementById('msgText').innerText = message;
            var msgIcon = document.getElementById('msgIcon');

            toastEl.classList.remove('bg-danger', 'bg-success', 'bg-primary');
            if (type === 'error') { toastEl.classList.add('bg-danger'); msgIcon.className = "fas fa-exclamation-triangle me-2"; }
            else if (type === 'success') { toastEl.classList.add('bg-success'); msgIcon.className = "fas fa-check-circle me-2"; }
            else { toastEl.classList.add('bg-primary'); msgIcon.className = "fas fa-info-circle me-2"; }

            new bootstrap.Toast(toastEl, { delay: 3000 }).show();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <asp:HiddenField ID="hfUserSno" runat="server" />

        <div class="container-fluid">
            <!-- LIST VIEW -->
            <asp:Panel ID="pnlList" runat="server">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h4 class="fw-bold m-0 text-dark"><i class="fas fa-users-cog me-2 text-primary"></i>User Master</h4>
                        <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn btn-primary px-4 shadow-sm" OnClick="btnOpenCreate_Click">
                            <i class="fas fa-user-plus me-2"></i>Create New User
                        </asp:LinkButton>
                    </div>
                    
                    <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" CssClass="table gv-style" 
                        GridLines="None" DataKeyNames="User_Sno" OnRowCommand="gvUsers_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Username" HeaderText="Login ID" />
                            <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                            <asp:BoundField DataField="Role" HeaderText="Role" />
                            <asp:BoundField DataField="Join_Date" HeaderText="Join Date" DataFormatString="{0:dd-MMM-yyyy}" />
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge bg-success" : "badge bg-danger" %>'>
                                        <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actions" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CssClass="btn btn-sm btn-outline-primary" CommandName="EditRecord" CommandArgument='<%# Eval("User_Sno") %>'>
                                        <i class="fas fa-edit me-1"></i>Edit
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>

            <!-- FORM VIEW -->
            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4 border-bottom pb-3">
                        <div>
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="text-decoration-none small text-muted fw-bold" OnClick="btnBack_Click">
                                <i class="fas fa-arrow-left"></i> Back to List
                            </asp:LinkButton>
                            <h4 class="fw-bold m-0 mt-1">User Registration</h4>
                        </div>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success px-5 fw-bold shadow-sm" OnClick="btnSave_Click">
                            <i class="fas fa-save me-2"></i>SAVE USER
                        </asp:LinkButton>
                    </div>

                    <div class="row g-4">
                        <div class="col-md-6 border-end">
                            <p class="section-title">1. Account & Security</p>
                            <div class="row g-3">
                                <div class="col-md-6">
                                    <label class="form-label">Username (Login ID) *</label>
                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">User Role</label>
                                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select form-select-sm">
                                        <asp:ListItem>Staff</asp:ListItem>
                                        <asp:ListItem>Manager</asp:ListItem>
                                        <asp:ListItem>Operator</asp:ListItem>
                                        <asp:ListItem>Salesman</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Join Date</label>
                                    <asp:TextBox ID="txtJoinDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Account Status</label>
                                    <div class="mt-1">
                                        <asp:CheckBox ID="chkIsActive" runat="server" Text=" &nbsp; Is Active" Checked="true" CssClass="form-check-input ms-1" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Password</label>
                                    <asp:TextBox ID="txtPass" runat="server" CssClass="form-control form-control-sm" TextMode="Password"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Confirm Password</label>
                                    <asp:TextBox ID="txtConfirm" runat="server" CssClass="form-control form-control-sm" TextMode="Password"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <p class="section-title">2. Profile Information</p>
                            <div class="row g-3">
                                <div class="col-md-12">
                                    <label class="form-label">Full Name *</label>
                                    <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Phone Number</label>
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">Email Address</label>
                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    <label class="form-label">Address Line 1</label>
                                    <asp:TextBox ID="txtAdd1" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    <label class="form-label">Address Line 2</label>
                                    <asp:TextBox ID="txtAdd2" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                                </div>
                            </div>
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
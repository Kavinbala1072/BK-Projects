<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ColorMaster.aspx.cs" Inherits="BKBilling.Forms.Master.ColorMaster" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Color Master | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        /* Professional Workspace Style */
        body { 
            background: #f8fafc; 
            background-image: radial-gradient(#cbd5e1 0.7px, transparent 0.7px);
            background-size: 24px 24px;
            min-height: 100vh;
            padding: 40px 20px;
            font-family: 'Inter', sans-serif;
        }

        .card-custom { 
            background: #ffffff; 
            border-radius: 16px; 
            border: 1px solid #e2e8f0; 
            box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1);
            margin: 0 auto;
            overflow: hidden;
        }

        /* Header & Actions */
        .form-header { background: #ffffff; padding: 25px 40px; border-bottom: 1px solid #f1f5f9; }
        .btn-save { background-color: #10b981; border: none; color: white; padding: 10px 24px; border-radius: 8px; font-weight: 700; transition: 0.2s; text-decoration: none; }
        .btn-save:hover { background-color: #059669; transform: translateY(-1px); color: white; }

        /* Typography & Decor */
        .section-title { 
            font-size: 0.75rem; font-weight: 800; color: #6366f1; 
            text-transform: uppercase; letter-spacing: 0.1em; 
            margin-bottom: 25px; display: flex; align-items: center;
        }
        .section-title::after { content: ""; height: 1px; flex-grow: 1; background: #f1f5f9; margin-left: 15px; }

        .form-body { padding: 40px; }
        .form-label { font-weight: 600; font-size: 0.82rem; color: #334155; margin-bottom: 6px; }
        .form-control-sm { border-radius: 8px; border: 1px solid #e2e8f0; padding: 10px 12px; }

        /* GridView Styling */
        .gv-style { border: none !important; margin-bottom: 0; }
        .gv-style th { background: #f8fafc; color: #64748b; font-size: 0.75rem; text-transform: uppercase; padding: 15px; border-bottom: 1px solid #e2e8f0; }
        .gv-style td { padding: 15px; font-size: 0.9rem; color: #334155; border-bottom: 1px solid #f1f5f9; vertical-align: middle; }
        
        .color-preview-circle { width: 32px; height: 32px; border-radius: 50%; border: 2px solid white; box-shadow: 0 0 0 1px #e2e8f0; display: inline-block; }

        /* Modern Toggle Logic for Form View */
        .modern-switch {
            display: flex; align-items: center; background: #f8fafc;
            padding: 10px 15px 10px 45px; border-radius: 12px; border: 1px solid #e2e8f0;
            position: relative; min-height: 45px; width: fit-content; min-width: 280px;
        }
        .modern-switch span { position: absolute; left: 10px; display: flex; align-items: center; }
        .modern-switch .form-check-input {
            width: 2.5em !important; height: 1.25em !important; cursor: pointer; margin: 0 !important;
            appearance: none; -webkit-appearance: none; background-color: #cbd5e1; border: none !important;
        }
        .modern-switch .form-check-input:checked { background-color: #10b981 !important; }
        .modern-switch label { font-weight: 600; color: #334155; cursor: pointer; font-size: 0.88rem; margin: 0 0 0 5px; }

        /* Notification Toast */
        .toast-container { z-index: 9999; }
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

        function updateColorName() {
            var hex = document.getElementById('<%= txtHex.ClientID %>').value;
            document.getElementById('hexDisplay').innerText = hex.toUpperCase();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <asp:HiddenField ID="hfColorSno" runat="server" />

        <div class="container-fluid">
            <!-- VIEW 1: DIRECTORY LIST -->
            <asp:Panel ID="pnlList" runat="server">
                <div class="card-custom">
                    <div class="form-header d-flex justify-content-between align-items-center">
                        <div>
                            <h3 class="fw-bold m-0 text-dark">Color Directory</h3>
                            <p class="text-muted small m-0">Standardized color pallet for garment inventory.</p>
                        </div>
                        <div class="d-flex gap-3">
                            <div class="input-group input-group-sm" style="width: 280px;">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by name..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" />
                                <span class="input-group-text bg-light border-start-0"><i class="fas fa-search text-muted"></i></span>
                            </div>
                            <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn btn-primary px-4 fw-bold shadow-sm" OnClick="btnOpenCreate_Click">
                                <i class="fas fa-plus-circle me-2"></i>New Color
                            </asp:LinkButton>
                        </div>
                    </div>
                    
                    <asp:GridView ID="gvColors" runat="server" AutoGenerateColumns="false" CssClass="table gv-style" 
                        GridLines="None" DataKeyNames="Color_Sno" OnRowCommand="gvColors_RowCommand">
                        <Columns>
                            <asp:TemplateField HeaderText="Swatch" ItemStyle-Width="80px">
                                <ItemTemplate>
                                    <div class="color-preview-circle" style='<%# "background-color:" + Eval("Color_HexCode") %>'></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Color_Name" HeaderText="Color Name" ItemStyle-CssClass="fw-bold text-dark" />
                            <asp:BoundField DataField="Color_HexCode" HeaderText="Hex Code" ItemStyle-CssClass="text-muted font-monospace small" />
                            
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge bg-success" : "badge bg-danger" %>'>
                                        <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-end pe-4">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-sm btn-outline-primary fw-bold" 
                                        CommandName="EditRecord" CommandArgument='<%# Eval("Color_Sno") %>'>
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
                            <h3 class="fw-bold m-0 text-dark">Color Setup</h3>
                        </div>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn-save shadow-sm" OnClick="btnSave_Click">
                            <i class="fas fa-check-circle me-2"></i>SAVE COLOR
                        </asp:LinkButton>
                    </div>

                    <div class="form-body">
                        <div class="row g-5">
                            <!-- Left: Basic Info -->
                            <div class="col-md-7">
                                <p class="section-title">Identity & Shade</p>
                                
                                <div class="mb-4">
                                    <label class="form-label">Color Name <span class="text-danger">*</span></label>
                                    <asp:TextBox ID="txtColorName" runat="server" CssClass="form-control form-control-sm" placeholder="e.g. DARK CRIMSON"></asp:TextBox>
                                </div>

                                <div class="mb-4">
                                    <label class="form-label">Color Picker / Hex Code</label>
                                    <div class="d-flex align-items-center gap-3 p-3 bg-light rounded-3 border">
                                        <asp:TextBox ID="txtHex" runat="server" CssClass="form-control" TextMode="Color" oninput="updateColorName()" style="width:60px; height:45px; cursor:pointer; border:none; padding:2px; background:transparent;"></asp:TextBox>
                                        <div>
                                            <div class="fw-bold text-dark small" id="hexDisplay">#000000</div>
                                            <div class="text-muted x-small">Select the shade for reports.</div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Right: Configuration -->
                            <div class="col-md-5 border-start ps-md-5">
                                <p class="section-title">Configuration</p>
                                
                                <div class="modern-switch form-check form-switch mb-4">
                                    <asp:CheckBox ID="chkActive" runat="server" CssClass="form-check-input" Checked="true" />
                                    <label for="<%= chkActive.ClientID %>">Is this color active?</label>
                                </div>
                                
                                <div class="p-3 bg-light rounded border-start border-4 border-primary small text-muted">
                                    <i class="fas fa-info-circle me-1"></i> <strong>Note:</strong> Deactivating a color will hide it from new Item creations.
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Toast UI -->
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
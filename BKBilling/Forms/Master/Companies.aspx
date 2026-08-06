<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Companies.aspx.cs" Inherits="BKBilling.Forms.Master.Companies" EnableEventValidation="false" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Company Directory | Pro ERP</title>
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
        .erp-body { flex-grow: 1; overflow-y: auto; background: #fff; position: relative; padding:5px;}
        .erp-footer { padding: 10px 30px; border-top: 1px solid var(--erp-border); background: #fff; display: flex; justify-content: space-between; align-items: center; flex-shrink: 0; }

        .page-title { font-size: 18px; font-weight: 800; margin: 0; }
        .search-pill { position: relative; width: 280px; }
        .search-pill i { position: absolute; left: 12px; top: 11px; color: var(--erp-text-light); font-size: 13px; }
        .search-pill .form-control { padding-left: 35px; border-radius: 8px; border: 1px solid var(--erp-border); height: 38px; background: #f1f5f9; font-size: 13px; }

        /* Grid Design */
        .gv-container { width: 100%; overflow: auto; height: 100%; padding-bottom: 80px; }
        .gv-pro { width: 100%; border-collapse: separate; border-spacing: 0; min-width: 1100px; }
        .gv-pro th { 
            background: #f8fafc !important; color: var(--erp-text-light); font-size: 11px; font-weight: 700;
            padding: 12px 15px; border-bottom: 2px solid var(--erp-border); border-right: 1px solid #f1f5f9;
            text-transform: uppercase; position: sticky; top: 0; z-index: 100; overflow: visible !important;
        }
        .gv-pro td { padding: 10px 15px; border-bottom: 1px solid #f1f5f9; font-size: 13px; vertical-align: middle; white-space: nowrap; }
        .hdr-wrap { display: flex; align-items: center; justify-content: space-between; position: relative; }
        .filt-icon { cursor: pointer; color: #cbd5e1; font-size: 12px; padding: 2px; }
        .flyout-panel {
            display: none; position: absolute; top: 100%; right: 0; width: 220px;
            background: #fff; border: 1px solid var(--erp-border); border-radius: 8px;
            box-shadow: 0 10px 25px rgba(0,0,0,0.1); padding: 12px; z-index: 500; text-transform: none; font-weight: normal; margin-top:5px;
        }

        /* Form Design */
        .section-title { font-size: 11px; font-weight: 800; color: var(--erp-blue); text-transform: uppercase; letter-spacing: 1px; border-bottom: 1px solid #f1f5f9; padding-bottom: 5px; margin-bottom: 15px; display: flex; align-items: center; gap: 8px; }
        .form-label { font-weight: 600; font-size: 12px; color: var(--erp-text-light); margin-bottom: 3px; }
        .admin-box { background: #f0f7ff; border: 1px solid #cfe2ff; border-radius: 12px; padding: 15px; }

        /* Action Buttons */
        .btn-primary-erp { background: var(--erp-blue); color: #fff !important; font-weight: 600; border-radius: 8px; padding: 8px 20px; font-size: 14px; border: none; text-decoration: none; }
        .btn-tool { width: 34px; height: 34px; border-radius: 6px; border: 1px solid var(--erp-border); background: #fff; display: inline-flex; align-items: center; justify-content: center; color: var(--erp-text-light); text-decoration: none; }
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
                            <div class="d-flex align-items-center gap-3">
                                <h4 class="page-title"><i class="fas fa-university text-primary me-2"></i><asp:Literal ID="litTitle" runat="server" /></h4>
                                
                                <asp:PlaceHolder ID="phSearchControls" runat="server">
                                    <div class="search-pill">
                                        <i class="fas fa-search"></i>
                                        <asp:TextBox ID="txtSearchAll" runat="server" CssClass="form-control" placeholder="Search Companies..." AutoPostBack="true" OnTextChanged="GridFilter_Changed"></asp:TextBox>
                                    </div>
                                </asp:PlaceHolder>
                            </div>

                            <div class="d-flex gap-2">
                                <asp:PlaceHolder ID="phSearchButtons" runat="server">
                                    <asp:LinkButton ID="btnSync" runat="server" CssClass="btn-tool" OnClick="GridFilter_Changed"><i class="fas fa-sync-alt"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn-primary-erp" OnClick="btnOpenCreate_Click"><i class="fas fa-plus me-2"></i>Register Company</asp:LinkButton>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="phAddButtons" runat="server" Visible="false">
                                    <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-outline-secondary btn-sm d-inline-flex align-items-center justify-content-center me-2"  Style="width:34px; height:34px;" OnClick="btnBack_Click"> <i class="fas fa-search"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn-primary-erp bg-success" OnClick="btnSave_Click" OnClientClick="return confirmWorkspaceInit();"><i class="fas fa-save me-2"></i>SAVE REGISTRATION</asp:LinkButton>
                                </asp:PlaceHolder>
                            </div>
                        </div>
                    </div>

                    <!-- BODY SECTION -->
                    <div class="erp-body">
                        
                        <!-- VIEW 1: SEARCH SCREEN -->
                        <asp:Panel ID="pnlList" runat="server">
                            <div class="gv-container">
                                <asp:GridView ID="gvCompanies" runat="server" AutoGenerateColumns="false" CssClass="gv-pro"
                                    GridLines="None" ShowHeaderWhenEmpty="true" AllowSorting="true" OnSorting="gvCompanies_Sorting" OnRowCommand="gvCompanies_RowCommand"
                                    AllowPaging="true" PagerSettings-Visible="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="ID" HeaderStyle-Width="60px" SortExpression="Company_Sno">
                                            <ItemTemplate><%# Eval("Company_Sno") %></ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField SortExpression="Company_Name">
                                            <HeaderTemplate>
                                                <div class="hdr-wrap">
                                                    <span>COMPANY NAME</span>
                                                    <i class="fas fa-filter filt-icon" onclick="toggleFlyout(event, 'f_name')"></i>
                                                    <div id="f_name" class="flyout-panel" onclick="event.stopPropagation()">
                                                        <label class="form-label">Filter Name</label>
                                                        <asp:TextBox ID="flt_name" runat="server" CssClass="form-control mb-2"></asp:TextBox>
                                                        <asp:Button runat="server" Text="Apply" CssClass="btn btn-primary btn-sm w-100" OnClick="GridFilter_Changed" />
                                                    </div>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate><b><%# Eval("Company_Name") %></b></ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField SortExpression="GSTIN">
                                            <HeaderTemplate>
                                                <div class="hdr-wrap">
                                                    <span>GSTIN</span>
                                                    <i class="fas fa-filter filt-icon" onclick="toggleFlyout(event, 'f_gst')"></i>
                                                    <div id="f_gst" class="flyout-panel" onclick="event.stopPropagation()">
                                                        <label class="form-label">Filter GST</label>
                                                        <asp:TextBox ID="flt_gst" runat="server" CssClass="form-control mb-2"></asp:TextBox>
                                                        <asp:Button runat="server" Text="Apply" CssClass="btn btn-primary btn-sm w-100" OnClick="GridFilter_Changed" />
                                                    </div>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate><%# Eval("GSTIN") %></ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="Phone" HeaderText="PHONE" />
                                        <asp:BoundField DataField="Created_Date" HeaderText="REG. DATE" DataFormatString="{0:dd-MMM-yyyy}" />

                                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="text-center" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnView" runat="server" CommandName="ViewRecord" CommandArgument='<%# Eval("Company_Sno") %>' CssClass="btn-tool border-0 text-info">
                                                    <i class="far fa-eye"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditRecord" CommandArgument='<%# Eval("Company_Sno") %>' CssClass="btn-tool border-0 text-primary">
                                                    <i class="far fa-edit"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>

                        <!-- VIEW 2: ADD/EDIT SCREEN -->
                        <asp:Panel ID="pnlForm" runat="server" Visible="false">
                            <div class="container-fluid p-4">
                                <div class="row g-4">
                                    
                                    <!-- COL 1: IDENTITY & REGIONAL -->
                                    <div class="col-md-4 border-end">
                                        <p class="section-title"><i class="fas fa-id-card"></i> 1. Company Identity</p>
                                        <div class="mb-3"><label class="form-label">Company Name *</label><asp:TextBox ID="C_Name" runat="server" CssClass="form-control"></asp:TextBox></div>
                                        <div class="row g-2 mb-3">
                                            <div class="col-6"><label class="form-label">GSTIN</label><asp:TextBox ID="C_GST" runat="server" CssClass="form-control" MaxLength="15"></asp:TextBox></div>
                                            <div class="col-6"><label class="form-label">PAN</label><asp:TextBox ID="C_PAN" runat="server" CssClass="form-control" MaxLength="10"></asp:TextBox></div>
                                        </div>
                                        <div class="row g-2 mb-3">
                                            <div class="col-6"><label class="form-label">FY Start</label><asp:TextBox ID="C_FY" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox></div>
                                            <div class="col-6"><label class="form-label">Currency Sym</label><asp:TextBox ID="C_CurSym" runat="server" CssClass="form-control"></asp:TextBox></div>
                                        </div>
                                        <div class="mb-3"><label class="form-label">Currency Format</label>
                                            <asp:DropDownList ID="C_CurFmt" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="INR">Indian (Lakhs)</asp:ListItem>
                                                <asp:ListItem Value="USD">International (Millions)</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <p class="section-title mt-4"><i class="fas fa-phone-alt"></i> 2. Contact Info</p>
                                        <div class="mb-3"><label class="form-label">Phone No</label><asp:TextBox ID="C_Phone" runat="server" CssClass="form-control"></asp:TextBox></div>
                                        <div><label class="form-label">Email Address</label><asp:TextBox ID="C_Email" runat="server" CssClass="form-control"></asp:TextBox></div>
                                    </div>

                                    <!-- COL 2: REGISTERED ADDRESS -->
                                    <div class="col-md-3 border-end">
                                        <p class="section-title"><i class="fas fa-map-marked-alt"></i> 3. Registered Address</p>
                                        <div class="mb-3"><label class="form-label">Address Line 1</label><asp:TextBox ID="C_Add1" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox></div>
                                        <div class="mb-3"><label class="form-label">Address Line 2</label><asp:TextBox ID="C_Add2" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox></div>
                                        <div class="mb-3"><label class="form-label">State</label><asp:TextBox ID="C_State" runat="server" CssClass="form-control"></asp:TextBox></div>
                                        <div><label class="form-label">Country</label><asp:TextBox ID="C_Country" runat="server" CssClass="form-control"></asp:TextBox></div>
                                    </div>

                                    <!-- COL 3: ADMIN USER PROFILE -->
                                    <div class="col-md-5">
                                        <div class="admin-box">
                                            <p class="section-title text-primary"><i class="fas fa-user-shield"></i> 4. Primary Admin Profile</p>
                                            <div class="row g-2 mb-3">
                                                <div class="col-7"><label class="form-label">Full Name *</label><asp:TextBox ID="U_FullName" runat="server" CssClass="form-control"></asp:TextBox></div>
                                                <div class="col-5"><label class="form-label">Join Date</label><asp:TextBox ID="U_JoinDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox></div>
                                            </div>
                                            <div class="row g-2 mb-3">
                                                <div class="col-6"><label class="form-label">Username *</label><asp:TextBox ID="U_Username" runat="server" CssClass="form-control"></asp:TextBox></div>
                                                <div class="col-6"><label class="form-label">Mobile</label><asp:TextBox ID="U_Phone" runat="server" CssClass="form-control"></asp:TextBox></div>
                                            </div>
                                            <div class="mb-3"><label class="form-label">Admin Email</label><asp:TextBox ID="U_Email" runat="server" CssClass="form-control"></asp:TextBox></div>
                                            <div class="row g-2 mb-4">
                                                <div class="col-6"><label class="form-label">Password</label><asp:TextBox ID="U_Pass" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox></div>
                                                <div class="col-6"><label class="form-label">Confirm</label><asp:TextBox ID="U_Confirm" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox></div>
                                            </div>
                                            <p class="section-title text-primary"><i class="fas fa-map-marker-alt"></i> 5. Admin Address</p>
                                            <div class="row g-2">
                                                <div class="col-6"><asp:TextBox ID="U_Add1" runat="server" CssClass="form-control" placeholder="Address 1"></asp:TextBox></div>
                                                <div class="col-6"><asp:TextBox ID="U_Add2" runat="server" CssClass="form-control" placeholder="Address 2"></asp:TextBox></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>

                    <!-- FOOTER SECTION -->
                    <asp:Panel ID="pnlFooter" runat="server" CssClass="erp-footer">
                        <div class="small text-muted">Showing <b><asp:Literal ID="litVisibleCount" runat="server" Text="0" /></b> records</div>
                        <div class="d-flex align-items-center gap-2">
                            <asp:LinkButton ID="btnPrev" runat="server" CssClass="btn-tool" OnClick="Pager_Click" CommandArgument="Prev"><i class="fas fa-chevron-left"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnNext" runat="server" CssClass="btn-tool" OnClick="Pager_Click" CommandArgument="Next"><i class="fas fa-chevron-right"></i></asp:LinkButton>
                            <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-select form-select-sm ms-2" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="GridFilter_Changed">
                                <asp:ListItem Text="10 Rows" Value="10" />
                                <asp:ListItem Text="25 Rows" Value="25" />
                                <asp:ListItem Text="50 Rows" Value="50" />
                            </asp:DropDownList>
                        </div>
                    </asp:Panel>

                    <asp:HiddenField ID="hfCompanySno" runat="server" />
                    <asp:HiddenField ID="hfViewMode" runat="server" Value="0" />
                    <asp:HiddenField ID="hfInitWorkspace" runat="server" Value="false" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3">
            <div id="msgToast" class="toast align-items-center text-white border-0 shadow-lg" role="alert">
                <div class="d-flex">
                    <div class="toast-body"><i id="msgIcon"></i> <span id="msgText"></span></div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>
        </div>
    </form>
    <script>
        function toggleFlyout(e, id) {
            e.stopPropagation();
            const panel = document.getElementById(id);
            const isVisible = panel.style.display === 'block';
            document.querySelectorAll('.flyout-panel').forEach(p => p.style.display = 'none');
            if (!isVisible) panel.style.display = 'block';
        }
        document.addEventListener('click', () => document.querySelectorAll('.flyout-panel').forEach(p => p.style.display = 'none'));

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

        function confirmWorkspaceInit() {
            if (document.getElementById('<%= hfViewMode.ClientID %>').value === "1") return false;
            if (confirm("Do you want to initialize or update the workspace (Default Ledgers, Groups, and Tables) for this company?")) {
                document.getElementById('<%= hfInitWorkspace.ClientID %>').value = "true";
            } else {
                document.getElementById('<%= hfInitWorkspace.ClientID %>').value = "false";
            }
            return true;
        }
    </script>
</body>
</html>
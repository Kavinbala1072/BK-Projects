<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserMaster.aspx.cs" Inherits="BKBilling.Forms.Master.UserMaster" EnableEventValidation="false" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>User Management | Pro ERP</title>
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

        /* Toolbars */
        .page-title { font-size: 18px; font-weight: 800; margin: 0; }
        .search-pill { position: relative; width: 280px; }
        .search-pill i { position: absolute; left: 12px; top: 11px; color: var(--erp-text-light); font-size: 13px; }
        .search-pill .form-control { padding-left: 35px; border-radius: 8px; border: 1px solid var(--erp-border); height: 38px; background: #f1f5f9; font-size: 13px; }

        /* Grid Scroll & Flyout Filters */
        .gv-container { width: 100%; overflow: auto; height: 100%; padding-bottom: 100px; }
        .gv-pro { width: 100%; border-collapse: separate; border-spacing: 0; min-width: 1000px; }
        .gv-pro th { 
            background: #f8fafc !important; color: var(--erp-text-light); font-size: 11px; font-weight: 700;
            padding: 12px 15px; border-bottom: 2px solid var(--erp-border); border-right: 1px solid #f1f5f9;
            text-transform: uppercase; position: sticky; top: 0; z-index: 10;
            position: sticky; 
            top: 0; 
            z-index: 100; /* Increased Z-index for headers */
            overflow: visible !important; 
        }
        .gv-pro td { padding: 10px 15px; border-bottom: 1px solid #f1f5f9; font-size: 13px; vertical-align: middle; white-space: nowrap; }
        .hdr-wrap { 
            display: flex; 
            align-items: center; 
            justify-content: space-between; 
            position: relative; 
        }
        .filt-icon { cursor: pointer; color: #cbd5e1; font-size: 12px; padding: 2px; }
        /*.flyout-panel {
            display: none; position: absolute; top: 35px; left: 0; width: 220px;
            background: #fff; border: 1px solid var(--erp-border); border-radius: 8px;
            box-shadow: 0 10px 25px rgba(0,0,0,0.1); padding: 12px; z-index: 500; text-transform: none; font-weight: normal;
        }*/

        .flyout-panel {
            display: none; 
            position: absolute; 
            top: 100%; /* Positions it exactly at the bottom of the header text */
            right: 0;   /* Aligns it to the right edge (under the icon) */
            left: auto; /* Overrides previous left:0 */
            width: 240px;
            background: #fff; 
            border: 1px solid var(--erp-border); 
            border-radius: 8px;
            box-shadow: 0 10px 25px rgba(0,0,0,0.15); 
            padding: 15px; 
            z-index: 1000; /* Higher than sticky headers */
            text-transform: none; 
            font-weight: normal;
            margin-top: 5px; /* Adds a small gap */
        }

        /* Form Design */
        .section-title { font-size: 11px; font-weight: 800; color: var(--erp-blue); text-transform: uppercase; letter-spacing: 1px; border-bottom: 1px solid #f1f5f9; padding-bottom: 5px; margin-bottom: 15px; display: flex; align-items: center; gap: 8px; }
        .form-label { font-weight: 600; font-size: 12px; color: var(--erp-text-light); margin-bottom: 3px; }
        .modern-switch { display: flex; align-items: center; background: #f8fafc; padding: 10px 15px; border-radius: 8px; border: 1px solid var(--erp-border); margin-bottom: 10px; cursor: pointer; }
        .modern-switch .form-check-input { width: 32px; height: 16px; margin-right: 10px; cursor: pointer; }

        /* Action Buttons */
        .btn-primary-erp { background: var(--erp-blue); color: #fff !important; font-weight: 600; border-radius: 8px; padding: 8px 20px; font-size: 14px; border: none; text-decoration: none; }
        .btn-tool { width: 34px; height: 34px; border-radius: 6px; border: 1px solid var(--erp-border); background: #fff; display: inline-flex; align-items: center; justify-content: center; color: var(--erp-text-light); text-decoration: none; }
        
        .pro-badge { display: inline-block; padding: 3px 10px; border-radius: 6px; font-size: 11px; font-weight: 700; }
        .pro-badge-active { background: #dcfce7; color: #15803d; }
        .pro-badge-inactive { background: #fee2e2; color: #b91c1c; }
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
                                <h4 class="page-title"><i class="fas fa-users-cog text-primary me-2"></i><asp:Literal ID="litTitle" runat="server" /></h4>
                                
                                <asp:PlaceHolder ID="phSearchControls" runat="server">
                                    <div class="search-pill">
                                        <i class="fas fa-search"></i>
                                        <asp:TextBox ID="txtSearchAll" runat="server" CssClass="form-control" placeholder="Global User Search..." AutoPostBack="true" OnTextChanged="GridFilter_Changed"></asp:TextBox>
                                    </div>
                                </asp:PlaceHolder>
                            </div>

                            <div class="d-flex gap-2">
                                <asp:PlaceHolder ID="phSearchButtons" runat="server">
                                    <asp:LinkButton ID="btnSync" runat="server" CssClass="btn-tool" OnClick="GridFilter_Changed"><i class="fas fa-sync-alt"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn-primary-erp" OnClick="btnOpenCreate_Click"><i class="fas fa-user-plus me-2"></i>New User</asp:LinkButton>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="phAddButtons" runat="server" Visible="false">
                                    <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-outline-secondary btn-sm d-inline-flex align-items-center justify-content-center me-2"  Style="width:34px; height:34px;" OnClick="btnBack_Click"> <i class="fas fa-search"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn-primary-erp bg-success" OnClick="btnSave_Click"><i class="fas fa-save me-2"></i>SAVE USER</asp:LinkButton>
                                </asp:PlaceHolder>
                            </div>
                        </div>
                    </div>

                    <!-- BODY SECTION -->
                    <div class="erp-body">
                        
                        <!-- VIEW 1: USER LIST -->
                        <asp:Panel ID="pnlList" runat="server">
                            <div class="gv-container">
                                <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" CssClass="gv-pro"
                                    GridLines="None" ShowHeaderWhenEmpty="true" AllowSorting="true" OnSorting="gvUsers_Sorting" OnRowCommand="gvUsers_RowCommand"
                                    AllowPaging="true" PagerSettings-Visible="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SNO" HeaderStyle-Width="60px">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                        </asp:TemplateField>

                                        <%-- 1. LOGIN ID --%>
                                        <asp:TemplateField SortExpression="Username">
                                            <HeaderTemplate>
                                                <div class="hdr-wrap">
                                                    <span>LOGIN ID</span>
                                                    <i class="fas fa-filter filt-icon" onclick="toggleFlyout(event, 'f_user')"></i>
                                                    <div id="f_user" class="flyout-panel" onclick="event.stopPropagation()">
                                                        <label class="form-label">Filter Username</label>
                                                        <asp:TextBox ID="flt_user" runat="server" CssClass="form-control mb-2"></asp:TextBox>
                                                        <asp:Button runat="server" Text="Apply" CssClass="btn btn-primary btn-sm w-100" OnClick="GridFilter_Changed" />
                                                    </div>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate><%# Eval("Username") %></ItemTemplate>
                                        </asp:TemplateField>

                                        <%-- 2. FULL NAME --%>
                                        <asp:TemplateField SortExpression="FullName">
                                            <HeaderTemplate>
                                                <div class="hdr-wrap">
                                                    <span>FULL NAME</span>
                                                    <i class="fas fa-filter filt-icon" onclick="toggleFlyout(event, 'f_name')"></i>
                                                    <div id="f_name" class="flyout-panel" onclick="event.stopPropagation()">
                                                        <label class="form-label">Search Name</label>
                                                        <asp:TextBox ID="flt_name" runat="server" CssClass="form-control mb-2"></asp:TextBox>
                                                        <asp:Button runat="server" Text="Apply" CssClass="btn btn-primary btn-sm w-100" OnClick="GridFilter_Changed" />
                                                    </div>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate><b><%# Eval("FullName") %></b></ItemTemplate>
                                        </asp:TemplateField>

                                        <%-- 3. ROLE --%>
                                        <asp:TemplateField SortExpression="Role">
                                            <HeaderTemplate>
                                                <div class="hdr-wrap">
                                                    <span>ROLE</span>
                                                    <i class="fas fa-filter filt-icon" onclick="toggleFlyout(event, 'f_role')"></i>
                                                    <div id="f_role" class="flyout-panel" onclick="event.stopPropagation()">
                                                        <label class="form-label">Filter Role</label>
                                                        <asp:DropDownList ID="flt_role" runat="server" CssClass="form-select mb-2">
                                                            <asp:ListItem Value="">All Roles</asp:ListItem>
                                                            <asp:ListItem>Staff</asp:ListItem>
                                                            <asp:ListItem>Manager</asp:ListItem>
                                                            <asp:ListItem>Operator</asp:ListItem>
                                                            <asp:ListItem>Salesman</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Button runat="server" Text="Apply" CssClass="btn btn-primary btn-sm w-100" OnClick="GridFilter_Changed" />
                                                    </div>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate><%# Eval("Role") %></ItemTemplate>
                                        </asp:TemplateField>

                                        <%-- 4. JOIN DATE --%>
                                        <asp:TemplateField SortExpression="Join_Date">
                                            <HeaderTemplate>
                                                <div class="hdr-wrap">
                                                    <span>JOIN DATE</span>
                                                    <i class="fas fa-filter filt-icon" onclick="toggleFlyout(event, 'f_date')"></i>
                                                    <div id="f_date" class="flyout-panel" onclick="event.stopPropagation()">
                                                        <label class="form-label">Search Date</label>
                                                        <asp:TextBox ID="flt_date" runat="server" CssClass="form-control mb-2" placeholder="YYYY-MM-DD"></asp:TextBox>
                                                        <asp:Button runat="server" Text="Apply" CssClass="btn btn-primary btn-sm w-100" OnClick="GridFilter_Changed" />
                                                    </div>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate><%# Eval("Join_Date", "{0:dd-MMM-yyyy}") %></ItemTemplate>
                                        </asp:TemplateField>
        
                                        <%-- 5. STATUS --%>
                                        <asp:TemplateField SortExpression="IsActive">
                                            <HeaderTemplate>
                                                <div class="hdr-wrap">
                                                    <span>STATUS</span>
                                                    <i class="fas fa-filter filt-icon" onclick="toggleFlyout(event, 'f_status')"></i>
                                                    <div id="f_status" class="flyout-panel" onclick="event.stopPropagation()">
                                                        <label class="form-label">Account Status</label>
                                                        <asp:DropDownList ID="flt_status" runat="server" CssClass="form-select mb-2">
                                                            <asp:ListItem Value="">All</asp:ListItem>
                                                            <asp:ListItem Value="1">Active</asp:ListItem>
                                                            <asp:ListItem Value="0">Inactive</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <asp:Button runat="server" Text="Apply" CssClass="btn btn-primary btn-sm w-100" OnClick="GridFilter_Changed" />
                                                    </div>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "pro-badge pro-badge-active" : "pro-badge pro-badge-inactive" %>'>
                                                    <%# Convert.ToBoolean(Eval("IsActive")) ? "ACTIVE" : "INACTIVE" %>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="text-center" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <!-- View Button -->
                                                <asp:LinkButton ID="btnView" runat="server" CommandName="ViewRecord" CommandArgument='<%# Eval("User_Sno") %>' CssClass="btn-tool border-0 text-info">
                                                    <i class="far fa-eye"></i>
                                                </asp:LinkButton>
        
                                                <!-- Edit Button -->
                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditRecord" CommandArgument='<%# Eval("User_Sno") %>' CssClass="btn-tool border-0 text-primary">
                                                    <i class="far fa-edit"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </asp:Panel>

                        <!-- VIEW 2: ADD/EDIT SCREEN (12 FIELDS) -->
                        <asp:Panel ID="pnlForm" runat="server" Visible="false">
                            <div class="container-fluid p-4">
                                <div class="row g-4">
                                    
                                    <!-- COL 1: Account & Security -->
                                    <div class="col-md-4 border-end">
                                        <p class="section-title"><i class="fas fa-shield-alt"></i> 1. Account & Security</p>
                                        <div class="mb-3">
                                            <label class="form-label">Username (Login ID) *</label>
                                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label">User Role</label>
                                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-select">
                                                <asp:ListItem>Staff</asp:ListItem>
                                                <asp:ListItem>Manager</asp:ListItem>
                                                <asp:ListItem>Operator</asp:ListItem>
                                                <asp:ListItem>Salesman</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="row g-2 mb-3">
                                            <div class="col-6">
                                                <label class="form-label">Password</label>
                                                <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="form-label">Confirm</label>
                                                <asp:TextBox ID="txtConfirm" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="modern-switch form-check form-switch mb-3">
                                            <asp:CheckBox ID="chkIsActive" runat="server" CssClass="form-check-input" Checked="true" />
                                            <label class="form-label mb-0" for="<%= chkIsActive.ClientID %>">Account is Active</label>
                                        </div>
                                    </div>

                                    <!-- COL 2: Profile Details -->
                                    <div class="col-md-4 border-end">
                                        <p class="section-title"><i class="fas fa-user-circle"></i> 2. Profile Details</p>
                                        <div class="mb-3">
                                            <label class="form-label">Full Name *</label>
                                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label">Phone Number</label>
                                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="Mobile / Contact"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label">Email Address</label>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="user@domain.com"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label">Joining Date</label>
                                            <asp:TextBox ID="txtJoinDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                        </div>
                                    </div>

                                    <!-- COL 3: Contact Address -->
                                    <div class="col-md-4">
                                        <p class="section-title"><i class="fas fa-map-marked-alt"></i> 3. Contact Address</p>
                                        <div class="mb-3">
                                            <label class="form-label">Address Line 1</label>
                                            <asp:TextBox ID="txtAdd1" runat="server" CssClass="form-control" placeholder="Street / Building"></asp:TextBox>
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label">Address Line 2</label>
                                            <asp:TextBox ID="txtAdd2" runat="server" CssClass="form-control" placeholder="City / Area"></asp:TextBox>
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

                    <asp:HiddenField ID="hfUserSno" runat="server" />
                    <asp:HiddenField ID="hfProfileMode" runat="server" Value="0" />
                    <asp:HiddenField ID="hfViewMode" runat="server" Value="0" />
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

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        // Filter Flyout Toggle
        function toggleFlyout(e, id) {
            e.stopPropagation();
            const panel = document.getElementById(id);
            const isVisible = panel.style.display === 'block';
            document.querySelectorAll('.flyout-panel').forEach(p => p.style.display = 'none');
            if (!isVisible) panel.style.display = 'block';
        }
        document.querySelectorAll('.flyout-panel').forEach(panel => {
            panel.addEventListener('click', function (e) {
                e.stopPropagation();
            });
        });
        document.addEventListener('click', () => document.querySelectorAll('.flyout-panel').forEach(p => p.style.display = 'none'));

        function closePanel() {
            if (window.parent && typeof window.parent.resetToWelcome === 'function') {
                window.parent.resetToWelcome();
            }
        }

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
</body>
</html>
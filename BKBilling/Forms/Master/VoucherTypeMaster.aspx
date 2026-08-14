<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VoucherTypeMaster.aspx.cs" Inherits="BKBilling.Forms.Master.VoucherTypeMaster" EnableEventValidation="false" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Voucher Configuration | Pro ERP</title>
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
        .search-pill { position: relative; width: 250px; }
        .search-pill i { position: absolute; left: 12px; top: 11px; color: var(--erp-text-light); font-size: 13px; }
        .search-pill .form-control { padding-left: 35px; border-radius: 8px; border: 1px solid var(--erp-border); height: 38px; background: #f1f5f9; font-size: 13px; }

        /* Grid Design */
        .gv-container { width: 100%; overflow: auto; height: 100%; padding-bottom: 80px; }
        .gv-pro { width: 100%; border-collapse: separate; border-spacing: 0; min-width: 1000px; }
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
        .modern-switch { display: flex; align-items: center; background: #f8fafc; padding: 10px 15px; border-radius: 8px; border: 1px solid var(--erp-border); width: fit-content; cursor: pointer; }
        .modern-switch .form-check-input { width: 2.8rem; height: 1.5rem; cursor: pointer; background-color: #cbd5e1; border: none; background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='-4 -4 8 8'%3e%3ccircle r='3' fill='rgba(255, 255, 255, 1)'/%3e%3c/svg%3e"); }
        .modern-switch .form-check-input:checked { background-color: #16a34a; background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='-4 -4 8 8'%3e%3ccircle r='3' fill='%23fff'/%3e%3c/svg%3e"); }

        .preview-box { background: #f0fdf4; border: 2px dashed #16a34a; padding: 15px; border-radius: 12px; text-align: center; }
        .badge-status { font-weight: 700; font-size: 11px; padding: 4px 10px; border-radius: 6px; }
        .badge-active { background-color: #ecfdf5; color: #10b981; }
        .badge-inactive { background-color: #fef2f2; color: #ef4444; }

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
                                <h4 class="page-title"><i class="fas fa-file-invoice text-primary me-2"></i>Voucher Configuration</h4>
                                
                                <asp:PlaceHolder ID="phSearchControls" runat="server">
                                    <div class="search-pill">
                                        <i class="fas fa-search"></i>
                                        <asp:TextBox ID="txtSearchAll" runat="server" CssClass="form-control" placeholder="Search voucher types..." AutoPostBack="true" OnTextChanged="GridFilter_Changed"></asp:TextBox>
                                    </div>
                                </asp:PlaceHolder>
                            </div>

                            <div class="d-flex gap-2">
                                <asp:PlaceHolder ID="phSearchButtons" runat="server">
                                    <asp:LinkButton ID="btnSync" runat="server" CssClass="btn-tool" OnClick="GridFilter_Changed"><i class="fas fa-sync-alt"></i></asp:LinkButton>
                                </asp:PlaceHolder>
                                <asp:PlaceHolder ID="phAddButtons" runat="server" Visible="false">
                                    <asp:LinkButton ID="btnBack" runat="server" CssClass="btn btn-outline-secondary btn-sm d-inline-flex align-items-center justify-content-center me-2"  Style="width:34px; height:34px;" OnClick="btnBack_Click"> <i class="fas fa-search"></i></asp:LinkButton>
                                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn-primary-erp bg-success" OnClick="btnSave_Click"><i class="fas fa-save me-2"></i>SAVE SETTINGS</asp:LinkButton>
                                </asp:PlaceHolder>
                            </div>
                        </div>
                    </div>

                    <!-- BODY SECTION -->
                    <div class="erp-body">
                        
                        <!-- VIEW 1: SEARCH SCREEN -->
                        <asp:Panel ID="pnlList" runat="server">
                            <div class="gv-container">
                                <asp:GridView ID="gvVTypes" runat="server" AutoGenerateColumns="false" CssClass="gv-pro"
                                    GridLines="None" ShowHeaderWhenEmpty="true" AllowSorting="true" OnSorting="gvVTypes_Sorting" OnRowCommand="gvVTypes_RowCommand"
                                    AllowPaging="true" PagerSettings-Visible="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SNO" HeaderStyle-Width="60px">
                                            <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField SortExpression="Voucher_Name">
                                            <HeaderTemplate>
                                                <div class="hdr-wrap">
                                                    <span>VOUCHER TYPE</span>
                                                    <i class="fas fa-filter filt-icon" onclick="toggleFlyout(event, 'f_name')"></i>
                                                    <div id="f_name" class="flyout-panel" onclick="event.stopPropagation()">
                                                        <label class="form-label">Filter Type</label>
                                                        <asp:TextBox ID="flt_name" runat="server" CssClass="form-control mb-2"></asp:TextBox>
                                                        <asp:Button runat="server" Text="Apply" CssClass="btn btn-primary btn-sm w-100" OnClick="GridFilter_Changed" />
                                                    </div>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate><b><%# Eval("Voucher_Name") %></b></ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="Prefix" HeaderText="PREFIX" />
                                        <asp:BoundField DataField="Suffix" HeaderText="SUFFIX" />
                                        
                                        <asp:TemplateField HeaderText="STATUS" HeaderStyle-Width="120px">
                                            <ItemTemplate>
                                                <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge-status badge-active" : "badge-status badge-inactive" %>'>
                                                    <%# Convert.ToBoolean(Eval("IsActive")) ? "ACTIVE" : "INACTIVE" %>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Action" ItemStyle-CssClass="text-center" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnView" runat="server" CommandName="ViewRecord" CommandArgument='<%# Eval("VoucherType_Sno") %>' CssClass="btn-tool border-0 text-info">
                                                    <i class="far fa-eye"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditRecord" CommandArgument='<%# Eval("VoucherType_Sno") %>' CssClass="btn-tool border-0 text-primary">
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
                                    
                                    <!-- COL 1: NUMBERING -->
                                    <div class="col-md-6 border-end">
                                        <p class="section-title"><i class="fas fa-hashtag"></i> 1. Numbering & Printing: <asp:Literal ID="litVName" runat="server" /></p>
                                        <div class="mb-3">
                                            <label class="form-label">Print Title (Header on Document)</label>
                                            <asp:TextBox ID="txtPrintTitle" runat="server" CssClass="form-control" placeholder="e.g. TAX INVOICE"></asp:TextBox>
                                        </div>
                                        <div class="row g-2 mb-3">
                                            <div class="col-6">
                                                <label class="form-label">Prefix</label>
                                                <asp:TextBox ID="txtPrefix" runat="server" CssClass="form-control" placeholder="e.g. INV/"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="form-label">Suffix</label>
                                                <asp:TextBox ID="txtSuffix" runat="server" CssClass="form-control" placeholder="e.g. /2024"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="row g-2 mb-4">
                                            <div class="col-6">
                                                <label class="form-label">Zero Padding Width</label>
                                                <asp:DropDownList ID="ddlWidth" runat="server" CssClass="form-select">
                                                    <asp:ListItem Value="4">4 Digits (0001)</asp:ListItem>
                                                    <asp:ListItem Value="5">5 Digits (00001)</asp:ListItem>
                                                    <asp:ListItem Value="6">6 Digits (000001)</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-6">
                                                <label class="form-label">Status</label>
                                                <div class="modern-switch form-check form-switch">
                                                    <input type="checkbox" id="chkActive" runat="server" class="form-check-input" checked="checked" />
                                                    <label class="form-label mb-0 ms-2" for="<%= chkActive.ClientID %>">Active</label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="preview-box">
                                            <small class="text-muted d-block mb-1 text-uppercase fw-bold" style="font-size:10px;">Generated Format Preview</small>
                                            <h3 class="fw-bold text-success mb-0" style="letter-spacing:1px;">INV/00001/26</h3>
                                        </div>
                                    </div>

                                    <!-- COL 2: ACCOUNTING -->
                                    <div class="col-md-6">
                                        <p class="section-title"><i class="fas fa-book"></i> 2. Default Account Posting</p>
                                        <div class="mb-3">
                                            <label class="form-label text-primary">Main Transaction Ledger</label>
                                            <asp:DropDownList ID="ddlMainLedger" runat="server" CssClass="form-select"></asp:DropDownList>
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label">Default Discount Ledger</label>
                                            <asp:DropDownList ID="ddlDiscountLedger" runat="server" CssClass="form-select"></asp:DropDownList>
                                        </div>
                                        <div class="mb-3">
                                            <label class="form-label">Round-Off Adjustment Ledger</label>
                                            <asp:DropDownList ID="ddlRoundOffLedger" runat="server" CssClass="form-select"></asp:DropDownList>
                                        </div>
                                        <div class="mt-4 pt-3 border-top">
                                            <div class="modern-switch form-check form-switch">
                                                <input type="checkbox" id="chkTaxInclusive" runat="server" class="form-check-input" />
                                                <label class="form-label mb-0 ms-2" for="<%= chkTaxInclusive.ClientID %>">Default to Tax Inclusive Pricing</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </div>

                    <!-- FOOTER SECTION -->
                    <asp:Panel ID="pnlFooter" runat="server" CssClass="erp-footer">
                        <div class="small text-muted">Showing <b><asp:Literal ID="litVisibleCount" runat="server" Text="0" /></b> recorded voucher types</div>
                        <div class="d-flex align-items-center gap-2">
                            <asp:LinkButton ID="btnPrev" runat="server" CssClass="btn-tool" OnClick="Pager_Click" CommandArgument="Prev"><i class="fas fa-chevron-left"></i></asp:LinkButton>
                            <asp:LinkButton ID="btnNext" runat="server" CssClass="btn-tool" OnClick="Pager_Click" CommandArgument="Next"><i class="fas fa-chevron-right"></i></asp:LinkButton>
                            <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-select form-select-sm ms-2" Width="100px" AutoPostBack="true" OnSelectedIndexChanged="GridFilter_Changed">
                                <asp:ListItem Text="25 Rows" Value="25" />
                                <asp:ListItem Text="50 Rows" Value="50" />
                            </asp:DropDownList>
                        </div>
                    </asp:Panel>

                    <asp:HiddenField ID="hfVTypeID" runat="server" />
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
        function toggleFlyout(e, id) {
            e.stopPropagation();
            const panel = document.getElementById(id);
            const isVisible = panel.style.display === 'block';
            document.querySelectorAll('.flyout-panel').forEach(p => p.style.display = 'none');
            if (!isVisible) panel.style.display = 'block';
        }
        document.querySelectorAll('.flyout-panel').forEach(p => p.addEventListener('click', e => e.stopPropagation()));
        document.addEventListener('click', () => document.querySelectorAll('.flyout-panel').forEach(p => p.style.display = 'none'));

        function showNotification(message, type) {
            setTimeout(function () {
                var toastEl = document.getElementById('msgToast');
                var msgText = document.getElementById('msgText');
                var msgIcon = document.getElementById('msgIcon');
                if (!toastEl) return;
                msgText.innerText = message;
                toastEl.classList.remove('bg-danger', 'bg-success', 'bg-primary');
                if (type === 'error') { toastEl.classList.add('bg-danger'); msgIcon.className = "fas fa-exclamation-triangle me-2"; }
                else if (type === 'success') { toastEl.classList.add('bg-success'); msgIcon.className = "fas fa-check-circle me-2"; }
                else { toastEl.classList.add('bg-primary'); msgIcon.className = "fas fa-info-circle me-2"; }
                new bootstrap.Toast(toastEl, { delay: 4000 }).show();
            }, 100);
        }
    </script>
</body>
</html>
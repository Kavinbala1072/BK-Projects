<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinancialYearMaster.aspx.cs" Inherits="BKBilling.Forms.Master.FinancialYearMaster" EnableEventValidation="false" %>

<!DOCTYPE html>
<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <title>Financial Year Setup | Pro ERP</title>
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
        .erp-body { flex-grow: 1; overflow-y: auto; background: #fff; position: relative; padding: 20px 30px; }
        .erp-footer { padding: 10px 30px; border-top: 1px solid var(--erp-border); background: #fff; display: flex; justify-content: space-between; align-items: center; flex-shrink: 0; }

        .page-title { font-size: 18px; font-weight: 800; margin: 0; }
        .section-title { font-size: 11px; font-weight: 800; color: var(--erp-blue); text-transform: uppercase; letter-spacing: 1px; border-bottom: 1px solid #f1f5f9; padding-bottom: 5px; margin-bottom: 15px; display: flex; align-items: center; gap: 8px; }
        .form-label { font-weight: 600; font-size: 12px; color: var(--erp-text-light); margin-bottom: 3px; }
        
        /* Grid Styling */
        .gv-pro { width: 100%; border-collapse: separate; border-spacing: 0; }
        .gv-pro th { background: #f8fafc !important; color: var(--erp-text-light); font-size: 11px; font-weight: 700; padding: 12px 15px; border-bottom: 2px solid var(--erp-border); text-transform: uppercase; }
        .gv-pro td { padding: 12px 15px; border-bottom: 1px solid #f1f5f9; font-size: 13px; vertical-align: middle; }

        .setup-card { background: #f8fafc; border: 1px solid var(--erp-border); border-radius: 12px; padding: 20px; height: 100%; }
        .active-year-box { background: #ecfdf5; border: 1px solid #a7f3d0; border-radius: 10px; padding: 15px; }
        
        .btn-primary-erp { background: var(--erp-blue); color: #fff !important; font-weight: 600; border-radius: 8px; padding: 8px 20px; font-size: 14px; border: none; text-decoration: none; }
        .btn-tool { width: 34px; height: 34px; border-radius: 6px; border: 1px solid var(--erp-border); background: #fff; display: inline-flex; align-items: center; justify-content: center; color: var(--erp-text-light); text-decoration: none; }
        .pro-badge-active { background: #dcfce7; color: #15803d; padding: 4px 10px; border-radius: 6px; font-size: 10px; font-weight: 800; }
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
                            <div>
                                <h4 class="page-title"><i class="fas fa-calendar-check text-primary me-2"></i>Financial Year Master</h4>
                            </div>
                            <div class="d-flex gap-2">
                                <asp:LinkButton ID="btnSync" runat="server" CssClass="btn-tool" OnClick="btnSync_Click"><i class="fas fa-sync-alt"></i></asp:LinkButton>
                                <%--<button type="button" class="btn btn-outline-secondary btn-sm fw-bold px-3 border-0" onclick="window.parent.resetToWelcome()"><i class="fas fa-times"></i></button>--%>
                            </div>
                        </div>
                    </div>

                    <!-- BODY SECTION -->
                    <div class="erp-body">
                        <div class="row g-4">
                            
                            <!-- LEFT: CONFIGURATION -->
                            <div class="col-md-4">
                                <div class="setup-card">
                                    <p class="section-title"><i class="fas fa-plus-circle"></i> 1. Create New Period</p>
                                    <div class="mb-3">
                                        <label class="form-label">FY Label / Name *</label>
                                        <asp:TextBox ID="txtFYName" runat="server" CssClass="form-control" placeholder="e.g. 2024-2025"></asp:TextBox>
                                    </div>
                                    <div class="row g-2 mb-3">
                                        <div class="col-6">
                                            <label class="form-label">Start Date</label>
                                            <asp:TextBox ID="txtStart" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                        </div>
                                        <div class="col-6">
                                            <label class="form-label">End Date</label>
                                            <asp:TextBox ID="txtEnd" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:LinkButton ID="btnSaveYear" runat="server" CssClass="btn-primary-erp w-100 text-center mb-5" OnClick="btnSaveYear_Click">
                                        <i class="fas fa-save me-2"></i>ADD TO REPOSITORY
                                    </asp:LinkButton>

                                    <p class="section-title mt-4"><i class="fas fa-toggle-on"></i> 2. Activate Workspace</p>
                                    <div class="active-year-box">
                                        <label class="form-label text-success fw-bold">Select Active Accounting Year</label>
                                        <asp:DropDownList ID="ddlActiveFY" runat="server" CssClass="form-select mb-3"></asp:DropDownList>
                                        <asp:LinkButton ID="btnSetActive" runat="server" CssClass="btn btn-success w-100 fw-bold shadow-sm" OnClick="btnSetActive_Click">
                                            <i class="fas fa-check-double me-2"></i>SET AS ACTIVE
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>

                            <!-- RIGHT: HISTORY GRID -->
                            <div class="col-md-8">
                                <p class="section-title"><i class="fas fa-history"></i> Period History & Status</p>
                                <div class="border rounded-3 overflow-hidden">
                                    <asp:GridView ID="gvYears" runat="server" AutoGenerateColumns="false" CssClass="gv-pro" GridLines="None" ShowHeaderWhenEmpty="true">
                                        <Columns>
                                            <asp:TemplateField HeaderText="SNO" HeaderStyle-Width="50px">
                                                <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FY_Name" HeaderText="Year Description" ItemStyle-CssClass="fw-bold" />
                                            <asp:BoundField DataField="StartDate" HeaderText="From Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                            <asp:BoundField DataField="EndDate" HeaderText="To Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                            <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# Eval("IsActiveYear").ToString() == "1" ? "<span class='pro-badge-active'><i class='fas fa-circle fa-xs me-1'></i> ACTIVE NOW</span>" : "<span class='text-muted small'>Inactive</span>" %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- FOOTER SECTION -->
                    <div class="erp-footer">
                        <div class="small text-muted">Showing <b><asp:Literal ID="litVisibleCount" runat="server" Text="0" /></b> recorded financial periods</div>
                        <div class="small text-muted">Company ID: <b><asp:Literal ID="litCid" runat="server" /></b></div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!-- Toast -->
        <div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3">
            <div id="msgToast" class="toast align-items-center text-white border-0 shadow-lg" role="alert">
                <div class="d-flex"><div class="toast-body"><span id="msgText"></span></div><button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button></div>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function showNotification(message, type) {
            var toastEl = document.getElementById('msgToast');
            document.getElementById('msgText').innerText = message;
            toastEl.classList.remove('bg-danger', 'bg-success', 'bg-primary');
            toastEl.classList.add(type === 'error' ? 'bg-danger' : 'bg-success');
            var toast = new bootstrap.Toast(toastEl, { delay: 4000 });
            toast.show();
        }
    </script>
</body>
</html>
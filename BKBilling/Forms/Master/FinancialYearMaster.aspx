<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinancialYearMaster.aspx.cs" Inherits="BKBilling.Forms.Master.FinancialYearMaster" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Financial Year Setup | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <style>
        body { background: #f8fafc; background-image: radial-gradient(#cbd5e1 0.7px, transparent 0.7px); background-size: 24px 24px; min-height: 100vh; padding: 40px 20px; font-family: 'Inter', sans-serif; }
        .card-custom { background: #ffffff; border-radius: 16px; border: 1px solid #e2e8f0; box-shadow: 0 20px 25px -5px rgba(0, 0, 0, 0.1); margin: 0 auto; overflow: hidden; }
        .form-header { background: #ffffff; padding: 25px 40px; border-bottom: 1px solid #f1f5f9; }
        .section-title { font-size: 0.75rem; font-weight: 800; color: #6366f1; text-transform: uppercase; letter-spacing: 0.1em; margin-bottom: 20px; display: flex; align-items: center; }
        .section-title::after { content: ""; height: 1px; flex-grow: 1; background: #f1f5f9; margin-left: 15px; }
        .form-label { font-weight: 600; font-size: 0.82rem; color: #334155; margin-bottom: 6px; }
        .active-selection-box { background: #f0fdf4; border: 1px solid #bbf7d0; border-radius: 12px; padding: 20px; }
        .gv-style th { background: #f8fafc; color: #64748b; font-size: 0.75rem; text-transform: uppercase; padding: 15px; border-bottom: 1px solid #e2e8f0; }
        .gv-style td { padding: 15px; font-size: 0.9rem; border-bottom: 1px solid #f1f5f9; vertical-align: middle; }
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
        <div class="container-fluid">
            <div class="card-custom">
                <div class="form-header">
                    <h3 class="fw-bold m-0 text-dark"><i class="fas fa-calendar-days me-2 text-primary"></i>Financial Year Master</h3>
                    <p class="text-muted small m-0">Setup and switch between accounting periods.</p>
                </div>

                <div class="p-4">
                    <div class="row g-5">
                        <!-- LEFT: ACTIONS -->
                        <div class="col-md-5 border-end">
                            <p class="section-title">1. Create New Period</p>
                            <div class="mb-3">
                                <label class="form-label">FY Name / Label</label>
                                <asp:TextBox ID="txtFYName" runat="server" CssClass="form-control form-control-sm" placeholder="e.g. 2024-2025"></asp:TextBox>
                            </div>
                            <div class="row g-2 mb-4">
                                <div class="col-6">
                                    <label class="form-label">Start Date</label>
                                    <asp:TextBox ID="txtStart" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <label class="form-label">End Date</label>
                                    <asp:TextBox ID="txtEnd" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                            <asp:LinkButton ID="btnSaveYear" runat="server" CssClass="btn btn-outline-primary w-100 fw-bold py-2" OnClick="btnSaveYear_Click">
                                <i class="fas fa-plus-circle me-1"></i> Add to List
                            </asp:LinkButton>

                            <div class="mt-5">
                                <p class="section-title">2. Select Active Year</p>
                                <div class="active-selection-box">
                                    <label class="form-label text-success fw-bold">Currently Selected Workspace</label>
                                    <asp:DropDownList ID="ddlActiveFY" runat="server" CssClass="form-select form-select-sm mb-3"></asp:DropDownList>
                                    <asp:LinkButton ID="btnSetActive" runat="server" CssClass="btn btn-success w-100 fw-bold py-2 shadow-sm" OnClick="btnSetActive_Click">
                                        <i class="fas fa-check-circle me-1"></i> SET AS ACTIVE YEAR
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <!-- RIGHT: GRID -->
                        <div class="col-md-7">
                            <p class="section-title">Historical & Current Years</p>
                            <asp:GridView ID="gvYears" runat="server" AutoGenerateColumns="false" CssClass="table gv-style" GridLines="None">
                                <Columns>
                                    <asp:BoundField DataField="FY_Name" HeaderText="Year Name" ItemStyle-CssClass="fw-bold text-dark" />
                                    <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                    <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:dd-MMM-yyyy}" />
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <%# Eval("IsActiveYear").ToString() == "1" ? "<span class='badge bg-success'>ACTIVE NOW</span>" : "<span class='text-muted small'>-</span>" %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Toast -->
            <div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3">
                <div id="msgToast" class="toast align-items-center text-white border-0 shadow-lg" role="alert">
                    <div class="d-flex"><div class="toast-body"><span id="msgText"></span></div><button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button></div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VoucherTypeMaster.aspx.cs" Inherits="BKBilling.Forms.Master.VoucherTypeMaster" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Voucher Settings | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <style>
        body { background: #f8fafc; background-image: radial-gradient(#cbd5e1 0.7px, transparent 0.7px); background-size: 24px 24px; min-height: 100vh; padding: 20px 10px; font-family: 'Inter', sans-serif; }
        .card-custom { background: white; border-radius: 16px; border: 1px solid #e2e8f0; box-shadow: 0 10px 15px -3px rgba(0,0,0,0.1); margin: 0 auto;}
        .form-header { padding: 25px 40px; border-bottom: 1px solid #f1f5f9; }
        .section-title { font-size: 0.75rem; font-weight: 800; color: #6366f1; text-transform: uppercase; letter-spacing: 1px; border-bottom: 1px solid #f1f5f9; padding-bottom: 8px; margin-bottom: 15px; display: flex; align-items: center; }
        .section-title i { margin-right: 8px; }
        .form-label { font-weight: 600; font-size: 0.78rem; color: #475569; }
        .gv-style th { background: #f8fafc; color: #64748b; font-size: 0.75rem; text-transform: uppercase; padding: 12px; }
        .preview-box { background: #f0fdf4; border: 2px dashed #16a34a; padding: 15px; border-radius: 12px; text-align: center; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <asp:HiddenField ID="hfVTypeID" runat="server" />

        <div class="container-fluid">
            <!-- VIEW 1: LIST -->
            <asp:Panel ID="pnlList" runat="server">
                <div class="card-custom">
                    <div class="form-header">
                        <h3 class="fw-bold m-0 text-dark"><i class="fas fa-file-invoice me-2 text-primary"></i>Voucher Configuration</h3>
                        <p class="text-muted small m-0">Set up numbering formats and default ledger postings.</p>
                    </div>
                    <div class="p-4">
                        <asp:GridView ID="gvVTypes" runat="server" AutoGenerateColumns="false" CssClass="table gv-style" GridLines="None" OnRowCommand="gvVTypes_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="Voucher_Name" HeaderText="Voucher Type" ItemStyle-CssClass="fw-bold" />
                                <asp:BoundField DataField="Prefix" HeaderText="Prefix" />
                                <asp:BoundField DataField="Suffix" HeaderText="Suffix" />
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <span class='<%# Convert.ToBoolean(Eval("IsActive")) ? "badge bg-success" : "badge bg-danger" %>'>
                                            <%# Convert.ToBoolean(Eval("IsActive")) ? "Active" : "Inactive" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-end">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditRecord" CommandArgument='<%# Eval("VoucherType_Sno") %>' CssClass="btn btn-sm btn-outline-primary px-3">Configure</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </asp:Panel>

            <!-- VIEW 2: FORM -->
            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <div class="card-custom">
                    <div class="form-header d-flex justify-content-between align-items-center">
                        <div>
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="text-decoration-none small text-muted fw-bold" OnClick="btnBack_Click"><i class="fas fa-arrow-left me-1"></i> BACK</asp:LinkButton>
                            <h4 class="fw-bold m-0 mt-1">Setup: <asp:Literal ID="litVName" runat="server" /></h4>
                        </div>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success px-5 fw-bold shadow-sm" OnClick="btnSave_Click">SAVE SETTINGS</asp:LinkButton>
                    </div>
                    <div class="p-4">
                        <div class="row g-4">
                            <!-- COLUMN 1: NUMBERING -->
                            <div class="col-md-6 border-end">
                                <p class="section-title"><i class="fas fa-hashtag"></i> 1. Numbering & Printing</p>
                                <div class="mb-3">
                                    <label class="form-label">Print Title (Header on Invoice)</label>
                                    <asp:TextBox ID="txtPrintTitle" runat="server" CssClass="form-control form-control-sm" placeholder="e.g. TAX INVOICE"></asp:TextBox>
                                </div>
                                <div class="row g-2 mb-3">
                                    <div class="col-6">
                                        <label class="form-label">Prefix</label>
                                        <asp:TextBox ID="txtPrefix" runat="server" CssClass="form-control form-control-sm" placeholder="e.g. INV/"></asp:TextBox>
                                    </div>
                                    <div class="col-6">
                                        <label class="form-label">Suffix</label>
                                        <asp:TextBox ID="txtSuffix" runat="server" CssClass="form-control form-control-sm" placeholder="e.g. /2024"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="row g-2 mb-3">
                                    <div class="col-6">
                                        <label class="form-label">Zero Padding</label>
                                        <asp:DropDownList ID="ddlWidth" runat="server" CssClass="form-select form-select-sm">
                                            <asp:ListItem Value="4">4 Digits (0001)</asp:ListItem>
                                            <asp:ListItem Value="5" Selected="True">5 Digits (00001)</asp:ListItem>
                                            <asp:ListItem Value="6">6 Digits (000001)</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-6">
                                        <label class="form-label">Voucher Status</label>
                                        <div class="form-check form-switch mt-1">
                                            <asp:CheckBox ID="chkActive" runat="server" CssClass="form-check-input" Checked="true" />
                                            <label class="form-check-label small fw-bold">Active</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="preview-box mt-4">
                                    <small class="text-muted d-block mb-1">Generated Example:</small>
                                    <h3 class="fw-bold text-success mb-0" id="previewText">INV/00001/26</h3>
                                </div>
                            </div>

                            <!-- COLUMN 2: ACCOUNTING -->
                            <div class="col-md-6">
                                <p class="section-title"><i class="fas fa-book"></i> 2. Account Posting Ledger</p>
                                <div class="mb-3">
                                    <label class="form-label text-primary">Default Main Ledger (Sales/Purchase)</label>
                                    <asp:DropDownList ID="ddlMainLedger" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Discount Ledger</label>
                                    <asp:DropDownList ID="ddlDiscountLedger" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label">Round Off Ledger</label>
                                    <asp:DropDownList ID="ddlRoundOffLedger" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList>
                                </div>
                                <div class="mt-4 pt-3 border-top">
                                    <div class="form-check form-switch">
                                        <asp:CheckBox ID="chkTaxInclusive" runat="server" CssClass="form-check-input" />
                                        <label class="form-check-label fw-bold small text-muted">Is Tax Inclusive by default?</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
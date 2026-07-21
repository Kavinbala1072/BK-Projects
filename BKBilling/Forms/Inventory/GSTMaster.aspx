<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GSTMaster.aspx.cs" Inherits="BKBilling.Forms.Master.GSTMaster" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>GST Master | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <style>
        body { background: #f8fafc; background-image: radial-gradient(#cbd5e1 0.7px, transparent 0.7px); background-size: 24px 24px; min-height: 100vh; padding: 20px; font-family: 'Inter', sans-serif; }
        .card-custom { background: white; border-radius: 16px; border: 1px solid #e2e8f0; box-shadow: 0 10px 15px -3px rgba(0,0,0,0.1); margin: 0 auto; }
        .form-header { padding: 25px 40px; border-bottom: 1px solid #f1f5f9; }
        .section-title { font-size: 0.75rem; font-weight: 800; color: #6366f1; text-transform: uppercase; letter-spacing: 1px; margin-bottom: 15px; display: flex; align-items: center; }
        .form-control-sm, .form-select-sm { border-radius: 6px; border: 1px solid #cbd5e1; padding: 6px 10px; font-size: 0.85rem; }
        
        /* ERP Table Layout */
        .erp-table { width: 100%; border-collapse: collapse; background: #fff; border: 1px solid #e2e8f0; }
        .erp-table th { background: #f8fafc; color: #64748b; font-size: 0.75rem; text-transform: uppercase; padding: 12px; border: 1px solid #e2e8f0; text-align: center; }
        .erp-table td { padding: 6px 10px; border: 1px solid #f1f5f9; vertical-align: middle; }
        .type-cell { background: #fcfdfe; font-weight: bold; width: 80px; text-align: center; color: #4f46e5; border-right: 2px solid #e2e8f0 !important; }
        .sub-label { color: #64748b; font-size: 0.78rem; width: 140px; font-weight: 600; }
        .rate-box { width: 70px; text-align: center; font-weight: bold; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <asp:HiddenField ID="hfGSTSno" runat="server" />

        <div class="container-fluid">
            <!-- VIEW 1: DIRECTORY -->
            <asp:Panel ID="pnlList" runat="server">
                <div class="card-custom p-4">
                    <div class="d-flex justify-content-between align-items-center mb-4">
                        <h4 class="fw-bold m-0"><i class="fas fa-percent me-2 text-primary"></i>GST Directory</h4>
                        <asp:LinkButton ID="btnOpenCreate" runat="server" CssClass="btn btn-primary px-4 fw-bold shadow-sm" OnClick="btnOpenCreate_Click">+ New Tax</asp:LinkButton>
                    </div>
                    <asp:GridView ID="gvGST" runat="server" AutoGenerateColumns="false" CssClass="table" GridLines="None" OnRowCommand="gvGST_RowCommand">
                        <Columns>
                            <asp:BoundField DataField="Tax_Name" HeaderText="Tax Name" ItemStyle-CssClass="fw-bold" />
                            <asp:BoundField DataField="IGST_Rate" HeaderText="IGST Rate" />
                            <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-end">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="EditRecord" CommandArgument='<%# Eval("GST_Sno") %>' CssClass="btn btn-sm btn-outline-primary">Configure</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>

            <!-- VIEW 2: FORM -->
            <asp:Panel ID="pnlForm" runat="server" Visible="false">
                <div class="card-custom">
                    <div class="form-header d-flex justify-content-between align-items-center">
                        <div>
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="text-decoration-none small text-muted fw-bold" OnClick="btnBack_Click"><i class="fas fa-arrow-left"></i> BACK</asp:LinkButton>
                            <h3 class="fw-bold m-0 mt-1">Tax Configuration</h3>
                        </div>
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-success px-5 fw-bold shadow-sm" OnClick="btnSave_Click">SAVE GST</asp:LinkButton>
                    </div>
                    <div class="p-4">
                        <div class="row mb-4">
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Tax Name *</label>
                                <asp:TextBox ID="txtTaxName" runat="server" CssClass="form-control form-control-sm text-primary fw-bold" placeholder="e.g. 18% OIL"></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-bold">Print Name</label>
                                <asp:TextBox ID="txtPrintName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                        </div>

                        <table class="erp-table">
                            <thead>
                                <tr>
                                    <th>Type</th><th>Rate %</th><th>Mapping Category</th><th>Account Posting Ledger</th>
                                </tr>
                            </thead>
                            <tbody>
                                <!-- SGST -->
                                <tr>
                                    <td rowspan="4" class="type-cell">SGST</td>
                                    <td rowspan="4" class="text-center"><asp:TextBox ID="txtSGST" runat="server" CssClass="form-control form-control-sm rate-box" Text="0.00"></asp:TextBox></td>
                                    <td class="sub-label">Local Sales</td>
                                    <td><asp:DropDownList ID="ddlSGST_LocSales" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                                </tr>
                                <tr><td class="sub-label">Sales Tax</td><td><asp:DropDownList ID="ddlSGST_SalesTax" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td></tr>
                                <tr><td class="sub-label">Local Purchase</td><td><asp:DropDownList ID="ddlSGST_LocPur" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td></tr>
                                <tr><td class="sub-label">Purchase Tax</td><td><asp:DropDownList ID="ddlSGST_PurTax" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td></tr>

                                <!-- CGST -->
                                <tr>
                                    <td rowspan="2" class="type-cell">CGST</td>
                                    <td rowspan="2" class="text-center"><asp:TextBox ID="txtCGST" runat="server" CssClass="form-control form-control-sm rate-box" Text="0.00"></asp:TextBox></td>
                                    <td class="sub-label">Sales Tax</td>
                                    <td><asp:DropDownList ID="ddlCGST_SalesTax" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                                </tr>
                                <tr><td class="sub-label">Purchase Tax</td><td><asp:DropDownList ID="ddlCGST_PurTax" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td></tr>

                                <!-- IGST -->
                                <tr>
                                    <td rowspan="4" class="type-cell">IGST</td>
                                    <td rowspan="4" class="text-center"><asp:TextBox ID="txtIGST" runat="server" CssClass="form-control form-control-sm rate-box" Text="0.00"></asp:TextBox></td>
                                    <td class="sub-label">Interstate Sales</td>
                                    <td><asp:DropDownList ID="ddlIGST_IntSales" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td>
                                </tr>
                                <tr><td class="sub-label">Sales Tax</td><td><asp:DropDownList ID="ddlIGST_SalesTax" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td></tr>
                                <tr><td class="sub-label">Interstate Purchase</td><td><asp:DropDownList ID="ddlIGST_IntPur" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td></tr>
                                <tr><td class="sub-label">Purchase Tax</td><td><asp:DropDownList ID="ddlIGST_PurTax" runat="server" CssClass="form-select form-select-sm"></asp:DropDownList></td></tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
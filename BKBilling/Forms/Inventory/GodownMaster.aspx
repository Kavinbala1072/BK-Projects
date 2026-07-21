<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GodownMaster.aspx.cs" Inherits="BKBilling.Forms.Master.GodownMaster" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Godown Master | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    <style>
        body { font-family: 'Inter', sans-serif; background-color: #f4f7fe; padding: 20px; }
        .card { border: none; box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075); border-radius: 12px; }
        .card-header { background: #fff; border-bottom: 1px solid #edf2f7; font-weight: 700; color: #334155; padding: 15px 20px; }
        .form-label { font-weight: 600; font-size: 0.82rem; color: #475569; margin-bottom: 4px; }
        .form-control-sm, .form-select-sm { border-radius: 6px; border: 1px solid #cbd5e1; }
        .table thead { background-color: #f8fafc; color: #64748b; font-size: 0.8rem; text-transform: uppercase; }
        .btn-primary { background-color: #003366; border-color: #003366; }
        .godown-icon { width: 40px; height: 40px; background: #eff6ff; color: #2563eb; border-radius: 10px; display: flex; align-items: center; justify-content: center; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <div class="container-fluid">
            
            <div class="row mb-4">
                <div class="col-12">
                    <h4 class="fw-bold mb-0 text-dark"><i class="fas fa-warehouse text-primary me-2"></i>Godown / Warehouse Master</h4>
                    <p class="text-muted small">Manage storage locations, branch addresses, and local GST details.</p>
                </div>
            </div>

            <div class="row">
                <!-- Entry Form -->
                <div class="col-lg-4">
                    <div class="card shadow-sm mb-4">
                        <div class="card-header">Godown Details</div>
                        <div class="card-body">
                            <asp:HiddenField ID="hfGodownId" runat="server" />
                            
                            <div class="mb-3">
                                <label class="form-label">Godown Name</label>
                                <asp:TextBox ID="txtGodownName" runat="server" CssClass="form-control form-control-sm" placeholder="e.g. Main Warehouse" />
                            </div>

                            <div class="row">
                                <div class="col-md-7 mb-3">
                                    <label class="form-label">GSTIN (Optional)</label>
                                    <asp:TextBox ID="txtGSTIN" runat="server" CssClass="form-control form-control-sm" placeholder="22AAAAA0000A1Z5" MaxLength="15" />
                                </div>
                                <div class="col-md-5 mb-3">
                                    <label class="form-label">Phone No.</label>
                                    <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control form-control-sm" placeholder="Contact No" />
                                </div>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Address</label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" Rows="2" placeholder="Street, Area details..." />
                            </div>

                            <div class="row mb-4">
                                <div class="col-md-6">
                                    <label class="form-label">City</label>
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control form-control-sm" placeholder="City" />
                                </div>
                                <div class="col-md-6">
                                    <label class="form-label">State</label>
                                    <asp:TextBox ID="txtState" runat="server" CssClass="form-control form-control-sm" placeholder="State" />
                                </div>
                            </div>

                            <div class="d-grid gap-2">
                                <asp:Button ID="btnSave" runat="server" Text="Save Godown" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Reset" CssClass="btn btn-outline-secondary" OnClick="btnClear_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- List View -->
                <div class="col-lg-8">
                    <div class="card shadow-sm">
                        <div class="card-header d-flex justify-content-between align-items-center">
                            <span>Available Locations</span>
                            <div class="input-group input-group-sm" style="width: 280px;">
                                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search by name or city..." AutoPostBack="true" OnTextChanged="txtSearch_TextChanged" />
                                <span class="input-group-text bg-white"><i class="fas fa-search text-muted"></i></span>
                            </div>
                        </div>
                        <div class="card-body p-0">
                            <div class="table-responsive">
                                <asp:GridView ID="gvGodown" runat="server" CssClass="table table-hover align-middle mb-0" 
                                    AutoGenerateColumns="false" GridLines="None" OnRowCommand="gvGodown_RowCommand">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-CssClass="ps-4" ItemStyle-Width="60px">
                                            <ItemTemplate>
                                                <div class="godown-icon"><i class="fas fa-map-marked-alt fa-xs"></i></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Godown Info">
                                            <ItemTemplate>
                                                <div class="fw-bold text-dark"><%# Eval("Godown_Name") %></div>
                                                <div class="text-muted small"><i class="fas fa-phone me-1" style="font-size:0.7rem;"></i><%# Eval("Phone_No") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="GSTIN" HeaderText="GSTIN" ItemStyle-CssClass="text-primary fw-medium" />
                                        <asp:TemplateField HeaderText="Address">
                                            <ItemTemplate>
                                                <div class="small text-truncate" style="max-width:200px;"><%# Eval("Address") %></div>
                                                <div class="small text-muted"><%# Eval("City") %>, <%# Eval("State") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Actions" ItemStyle-CssClass="text-end pe-4">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" CommandName="EditGodown" CommandArgument='<%# Eval("Godown_Sno") %>' CssClass="btn btn-sm btn-light text-primary me-2"><i class="fas fa-edit"></i></asp:LinkButton>
                                                <asp:LinkButton runat="server" CommandName="DeleteGodown" CommandArgument='<%# Eval("Godown_Sno") %>' CssClass="btn btn-sm btn-light text-danger" OnClientClick="return confirm('Delete this godown? Ensure no stock is currently linked.')"><i class="fas fa-trash"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="p-5 text-center text-muted">No godowns defined.</div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
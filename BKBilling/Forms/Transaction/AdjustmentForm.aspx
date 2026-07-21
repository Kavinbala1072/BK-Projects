<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdjustmentForm.aspx.cs" Inherits="BKBilling.Forms.Transaction.AdjustmentForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Adjustment</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    <style>
        body { font-family: 'Inter', sans-serif; background-color: #f8fafc; padding: 20px; }
        .card { border: none; box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075); border-radius: 10px; }
        .card-header { background-color: #fff; border-bottom: 1px solid #e2e8f0; font-weight: 700; color: #1e293b; }
        .form-label { font-weight: 600; font-size: 0.85rem; color: #64748b; }
        .table thead { background-color: #f1f5f9; }
        .btn-primary { background-color: #003366; border-color: #003366; }
        .btn-primary:hover { background-color: #00264d; border-color: #00264d; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        
        <div class="container-fluid">
            <!-- Header -->
            <div class="d-flex justify-content-between align-items-center mb-4">
                <h4 class="mb-0 fw-bold"><i class="fas fa-boxes text-primary me-2"></i>Stock Adjustment</h4>
                <div>
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary" OnClick="btnSave_Click">
                        <i class="fas fa-save me-1"></i> Save Adjustment
                    </asp:LinkButton>
                    <asp:LinkButton ID="btnClear" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnClear_Click">
                        <i class="fas fa-redo me-1"></i> Reset
                    </asp:LinkButton>
                </div>
            </div>

            <div class="row">
                <!-- Master Info -->
                <div class="col-md-12 mb-4">
                    <div class="card">
                        <div class="card-body">
                            <div class="row g-3">
                                <div class="col-md-3">
                                    <label class="form-label">Reference No.</label>
                                    <asp:TextBox ID="txtRefNo" runat="server" CssClass="form-control" ReadOnly="true" />
                                </div>
                                <div class="col-md-3">
                                    <label class="form-label">Adjustment Date</label>
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date" />
                                </div>
                                <div class="col-md-3">
                                    <label class="form-label">Godown / Location</label>
                                    <asp:DropDownList ID="ddlGodown" runat="server" CssClass="form-select"></asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <label class="form-label">Adjusted By</label>
                                    <asp:TextBox ID="txtUser" runat="server" CssClass="form-control" ReadOnly="true" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Input Entry -->
                <div class="col-md-12 mb-4">
                    <div class="card">
                        <div class="card-header">Add Items</div>
                        <div class="card-body">
                            <asp:UpdatePanel ID="updEntry" runat="server">
                                <ContentTemplate>
                                    <div class="row g-2 align-items-end">
                                        <div class="col-md-4">
                                            <label class="form-label">Select Product</label>
                                            <asp:DropDownList ID="ddlItem" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlItem_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <label class="form-label">Current Stock</label>
                                            <asp:TextBox ID="txtCurrStock" runat="server" CssClass="form-control bg-light" ReadOnly="true" Text="0" />
                                        </div>
                                        <div class="col-md-2">
                                            <label class="form-label">Type</label>
                                            <asp:DropDownList ID="ddlType" runat="server" CssClass="form-select">
                                                <asp:ListItem Value="ADD">Surplus (+)</asp:ListItem>
                                                <asp:ListItem Value="DEDUCT">Shortage / Damage (-)</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <label class="form-label">Adjustment Qty</label>
                                            <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" TextMode="Number" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Button ID="btnAdd" runat="server" Text="Add to List" CssClass="btn btn-success w-100" OnClick="btnAdd_Click" />
                                        </div>
                                    </div>
                                    <div class="row mt-2">
                                        <div class="col-12">
                                            <label class="form-label">Reason / Remarks</label>
                                            <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" placeholder="Enter reason for adjustment..." />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>

                <!-- List Grid -->
                <div class="col-md-12">
                    <div class="card">
                        <div class="card-body p-0">
                            <asp:GridView ID="gvAdjustment" runat="server" CssClass="table table-hover mb-0" AutoGenerateColumns="False" OnRowDeleting="gvAdjustment_RowDeleting" GridLines="None">
                                <Columns>
                                    <asp:TemplateField HeaderText="#">
                                        <ItemTemplate><%# Container.DataItemIndex + 1 %></ItemTemplate>
                                        <ItemStyle Width="50px" CssClass="ps-3" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ItemName" HeaderText="Product Name" />
                                    <asp:BoundField DataField="Type" HeaderText="Action" />
                                    <asp:BoundField DataField="Qty" HeaderText="Quantity" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Reason" />
                                    <asp:TemplateField HeaderText="Remove">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" CssClass="text-danger"><i class="fas fa-trash"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="p-4 text-center text-muted">No items added yet.</div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
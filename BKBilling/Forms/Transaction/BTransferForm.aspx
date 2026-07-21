<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BTransferForm.aspx.cs" Inherits="BKBilling.Forms.Transaction.BTransferForm" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Branch Transfer (GST)</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    <style>
        body { font-family: 'Inter', sans-serif; background-color: #f4f7fe; padding: 20px; }
        .card { border: none; box-shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075); border-radius: 10px; margin-bottom: 20px; }
        .card-header { background: #fff; border-bottom: 1px solid #edf2f7; font-weight: 700; color: #334155; }
        .form-label { font-weight: 600; font-size: 0.82rem; color: #475569; }
        .gst-badge { font-size: 0.75rem; padding: 3px 8px; border-radius: 4px; background: #e0e7ff; color: #4338ca; }
        .table thead { background-color: #f8fafc; font-size: 0.85rem; }
        .total-box { background: #1e293b; color: white; border-radius: 8px; padding: 15px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <div class="container-fluid">
            <!-- Header Actions -->
            <div class="d-flex justify-content-between align-items-center mb-4">
                <h4 class="fw-bold mb-0 text-dark"><i class="fas fa-exchange-alt text-primary me-2"></i>Branch Stock Transfer</h4>
                <div>
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="btn btn-primary px-4" OnClick="btnSave_Click">
                        <i class="fas fa-check-circle me-1"></i> Post Transfer
                    </asp:LinkButton>
                </div>
            </div>

            <div class="row">
                <!-- Master Details -->
                <div class="col-lg-8">
                    <div class="card">
                        <div class="card-header">Transfer Details</div>
                        <div class="card-body">
                            <div class="row g-3">
                                <div class="col-md-4">
                                    <label class="form-label">Transfer Type</label>
                                    <asp:DropDownList ID="ddlTransferType" runat="server" CssClass="form-select form-select-sm">
                                        <asp:ListItem Value="Intra">Intra-State (Same State)</asp:ListItem>
                                        <asp:ListItem Value="Inter">Inter-State (IGST Applicable)</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label class="form-label">Transfer / DC No.</label>
                                    <asp:TextBox ID="txtDocNo" runat="server" CssClass="form-control form-control-sm" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label class="form-label">Date</label>
                                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control form-control-sm" TextMode="Date"></asp:TextBox>
                                </div>

                                <!-- From Branch -->
                                <div class="col-md-6">
                                    <div class="p-3 border rounded bg-light">
                                        <label class="form-label text-primary">FROM BRANCH (Source)</label>
                                        <asp:DropDownList ID="ddlSourceBranch" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="Branch_Changed"></asp:DropDownList>
                                        <div class="mt-2"><span class="gst-badge">GSTIN: <asp:Literal ID="litSourceGST" runat="server" Text="Not Available" /></span></div>
                                    </div>
                                </div>

                                <!-- To Branch -->
                                <div class="col-md-6">
                                    <div class="p-3 border rounded bg-light">
                                        <label class="form-label text-success">TO BRANCH (Destination)</label>
                                        <asp:DropDownList ID="ddlDestBranch" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="Branch_Changed"></asp:DropDownList>
                                        <div class="mt-2"><span class="gst-badge">GSTIN: <asp:Literal ID="litDestGST" runat="server" Text="Not Available" /></span></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Item Input -->
                    <div class="card">
                        <div class="card-body">
                            <asp:UpdatePanel ID="upItems" runat="server">
                                <ContentTemplate>
                                    <div class="row g-2 align-items-end">
                                        <div class="col-md-5">
                                            <label class="form-label">Product Name</label>
                                            <asp:DropDownList ID="ddlProduct" runat="server" CssClass="form-select"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <label class="form-label">Qty</label>
                                            <asp:TextBox ID="txtQty" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label class="form-label">Transfer Rate</label>
                                            <asp:TextBox ID="txtRate" runat="server" CssClass="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <label class="form-label">GST %</label>
                                            <asp:DropDownList ID="ddlGST" runat="server" CssClass="form-select">
                                                <asp:ListItem>0</asp:ListItem>
                                                <asp:ListItem>5</asp:ListItem>
                                                <asp:ListItem>12</asp:ListItem>
                                                <asp:ListItem Selected="True">18</asp:ListItem>
                                                <asp:ListItem>28</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:LinkButton ID="btnAdd" runat="server" CssClass="btn btn-dark w-100" OnClick="btnAdd_Click"><i class="fas fa-plus"></i></asp:LinkButton>
                                        </div>
                                    </div>

                                    <div class="table-responsive mt-4">
                                        <asp:GridView ID="gvItems" runat="server" CssClass="table table-sm border" AutoGenerateColumns="false" OnRowDeleting="gvItems_RowDeleting">
                                            <Columns>
                                                <asp:BoundField DataField="ItemName" HeaderText="Product" />
                                                <asp:BoundField DataField="Qty" HeaderText="Qty" />
                                                <asp:BoundField DataField="Rate" HeaderText="Rate" />
                                                <asp:BoundField DataField="Taxable" HeaderText="Taxable" />
                                                <asp:BoundField DataField="GST" HeaderText="GST%" />
                                                <asp:BoundField DataField="Total" HeaderText="Total Amount" />
                                                <asp:CommandField ShowDeleteButton="true" ControlStyle-CssClass="text-danger" DeleteText="<i class='fas fa-trash'></i>" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>

                <!-- Calculation Summary -->
                <div class="col-lg-4">
                    <div class="card bg-white h-100">
                        <div class="card-header">Tax Summary</div>
                        <div class="card-body">
                            <div class="d-flex justify-content-between mb-2"><span>Sub Total:</span><asp:Label ID="lblSubTotal" runat="server" Text="0.00" Font-Bold="true" /></div>
                            <div class="d-flex justify-content-between mb-2"><span>CGST:</span><asp:Label ID="lblCGST" runat="server" Text="0.00" /></div>
                            <div class="d-flex justify-content-between mb-2"><span>SGST:</span><asp:Label ID="lblSGST" runat="server" Text="0.00" /></div>
                            <div class="d-flex justify-content-between mb-2"><span>IGST:</span><asp:Label ID="lblIGST" runat="server" Text="0.00" /></div>
                            <hr />
                            <div class="total-box text-center mt-4">
                                <small>NET TRANSFER VALUE</small>
                                <h2 class="mb-0 fw-bold">₹ <asp:Literal ID="litGrandTotal" runat="server" Text="0.00" /></h2>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
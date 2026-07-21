<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintVoucher.aspx.cs" Inherits="BKSoftwares.PrintVoucher" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Print Voucher</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        body { padding: 30px; font-family: sans-serif; }
        .voucher-box { border: 2px solid #333; padding: 20px; border-radius: 10px; position: relative; }
        .watermark { position: absolute; top: 30%; left: 30%; font-size: 5rem; opacity: 0.1; transform: rotate(-45deg); font-weight: bold; }
        @media print { .no-print { display: none; } }
    </style>
</head>
<body onload="window.print()">
    <div class="container">
        <div class="voucher-box">
            <div class="row">
                <div class="col-6">
                    <h2 class="fw-bold">BK SOFTWARES</h2>
                    <p class="text-muted">Software Solutions & IT Services</p>
                </div>
                <div class="col-6 text-end">
                    <h4 class="text-uppercase fw-bold"><asp:Literal ID="litVchType" runat="server" /> SLIP</h4>
                    <p>No: <strong class="text-danger"><asp:Literal ID="litVchNo" runat="server" /></strong></p>
                    <p>Date: <asp:Literal ID="litVchDate" runat="server" /></p>
                </div>
            </div>
            <hr />
            <div class="mt-4">
                <p>Received with thanks from / Paid to:</p>
                <h4 class="border-bottom pb-2"><asp:Literal ID="litCustName" runat="server" /></h4>
            </div>
            <div class="row mt-4">
                <div class="col-8">
                    <p>On Account of: <em><asp:Literal ID="litNarration" runat="server" /></em></p>
                    <p>Payment Mode: <strong><asp:Literal ID="litMode" runat="server" /></strong></p>
                </div>
                <div class="col-4 text-center">
                    <div class="border p-3 bg-light">
                        <small>Amount</small>
                        <h3 class="fw-bold mb-0">₹ <asp:Literal ID="litAmount" runat="server" /></h3>
                    </div>
                </div>
            </div>
            <div class="row mt-5">
                <div class="col-6 text-center"><br /><hr class="w-50 mx-auto" /><small>Receiver's Signature</small></div>
                <div class="col-6 text-center"><br /><hr class="w-50 mx-auto" /><small>Authorized Signatory</small></div>
            </div>
        </div>
        <div class="mt-3 text-center no-print">
            <button class="btn btn-dark" onclick="window.print()">Print Again</button>
            <button class="btn btn-secondary" onclick="window.close()">Close Window</button>
        </div>
    </div>
</body>
</html>
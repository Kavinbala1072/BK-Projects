<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Vouchers.aspx.cs" Inherits="BKSoftwares.Vouchers" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Voucher Management | BK Softwares</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    <style>
        :root {
            --primary: #6366f1; --receipt: #10b981; --payment: #f43f5e;
            --bg: #f8fafc; --dark: #0f172a; --text-light: #64748b;
        }
        body { background-color: var(--bg); font-family: 'Inter', sans-serif; color: var(--dark); padding: 20px; }

        /* Header UI */
        .page-header { margin-bottom: 2rem; }
        .btn-add { background: var(--primary); color: white; border-radius: 10px; padding: 10px 24px; font-weight: 600; border: none; transition: 0.3s; }
        .btn-add:hover { background: #4f46e5; transform: translateY(-2px); color: white; }

        /* Card Container */
        .main-container { background: white; border-radius: 16px; box-shadow: 0 10px 15px -3px rgba(0,0,0,0.05); border: 1px solid #e2e8f0; overflow: hidden; }
        
        /* Table Styling (Desktop) */
        .table { margin-bottom: 0; }
        .table thead { background: #f1f5f9; }
        .table thead th { font-size: 0.75rem; text-transform: uppercase; letter-spacing: 0.05em; color: var(--text-light); padding: 16px; border: none; }
        .table tbody td { padding: 16px; border-bottom: 1px solid #f1f5f9; vertical-align: middle; font-size: 0.95rem; }
        .table tbody tr:hover { background-color: #f8fafc; }

        /* Badges */
        .v-badge { padding: 6px 12px; border-radius: 8px; font-size: 0.75rem; font-weight: 700; text-transform: uppercase; }
        .v-receipt { background: #dcfce7; color: var(--receipt); }
        .v-payment { background: #fee2e2; color: var(--payment); }
        .mode-tag { font-size: 0.8rem; color: var(--text-light); background: #f1f5f9; padding: 2px 8px; border-radius: 4px; }

        /* --- MOBILE CARD VIEW --- */
        @media (max-width: 768px) {
            body { padding: 10px; }
            .main-container { background: transparent; border: none; box-shadow: none; }
            .resp-table thead { display: none; }
            .resp-table, .resp-table tbody, .resp-table tr, .resp-table td { display: block; width: 100%; }
            
            .resp-table tr { 
                background: white; border-radius: 16px; margin-bottom: 16px; 
                padding: 20px; box-shadow: 0 4px 6px -1px rgba(0,0,0,0.1);
                border: 1px solid #e2e8f0; position: relative;
            }

            .resp-table td { border: none; padding: 5px 0; display: flex; justify-content: space-between; align-items: center; text-align: right; }
            
            /* Logic for Mobile Card Labels */
            .resp-table td::before { content: attr(data-label); font-weight: 600; color: var(--text-light); font-size: 0.8rem; text-align: left; }
            
            /* Specialized Row Styling for Mobile */
            .resp-table td[data-label="Voucher No"] { border-bottom: 1px solid #f1f5f9; padding-bottom: 10px; margin-bottom: 10px; }
            .resp-table td[data-label="Amount"] { font-size: 1.25rem; font-weight: 800; color: var(--dark); padding-top: 10px; }
            .resp-table td[data-label="Action"] { border-top: 1px solid #f1f5f9; margin-top: 10px; padding-top: 15px; justify-content: center; gap: 20px; }
            
            .btn-action-mobile { flex: 1; padding: 10px; }
        }

        /* Modal Styling */
        .modal-content { border-radius: 20px; border: none; }
        .modal-header { border-bottom: 1px solid #f1f5f9; padding: 24px; }
        .form-control, .form-select { border-radius: 10px; padding: 12px; border: 1px solid #e2e8f0; transition: 0.3s; }
        .form-control:focus { box-shadow: 0 0 0 4px rgba(99, 102, 241, 0.1); border-color: var(--primary); }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <!-- Professional Header -->
            <div class="page-header d-flex justify-content-between align-items-center flex-wrap gap-3">
                <div>
                    <h2 class="fw-bold m-0 text-dark">Voucher Entry</h2>
                    <%--<p class="text-muted m-0">Create and manage your financial transactions</p>--%>
                </div>
                <button type="button" class="btn-add shadow-sm" onclick="openVoucherModal(0)">
                    <i class="fas fa-plus-circle me-2"></i>New Voucher
                </button>
            </div>

            <!-- List Section -->
            <div class="main-container">
                <div class="table-responsive">
                    <table class="table table-hover align-middle resp-table">
                        <thead>
                            <tr>
                                <th class="ps-4">Voucher Info</th>
                                <th>Type</th>
                                <th>Customer Name</th>
                                <th>Method</th>
                                <th class="text-end">Amount</th>
                                <th class="text-center">Action</th>
                            </tr>
                        </thead>
                        <tbody id="voucherTableBody"></tbody>
                    </table>
                </div>
            </div>
        </div>

        <!-- Modern Modal -->
        <div class="modal fade" id="vchModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-lg modal-dialog-centered">
                <div class="modal-content shadow-lg">
                    <div class="modal-header">
                        <h5 class="fw-bold m-0" id="vchTitle">New Voucher</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body p-4">
                        <input type="hidden" id="hfVchID" value="0" />
                        <div class="row g-4">
                            <div class="col-md-6">
                                <label class="form-label fw-semibold small text-uppercase">Voucher Type</label>
                                <select id="ddlType" class="form-select fw-bold">
                                    <option value="Receipt">RECEIPT (Money In)</option>
                                    <option value="Payment">PAYMENT (Money Out)</option>
                                </select>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-semibold small text-uppercase">Transaction Date</label>
                                <input type="date" id="txtDate" class="form-control" />
                            </div>
                            <div class="col-md-12">
                                <label class="form-label fw-semibold small text-uppercase">Client / Customer</label>
                                <select id="ddlCustomer" class="form-select"></select>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-semibold small text-uppercase">Total Amount (₹)</label>
                                <input type="number" id="txtAmount" class="form-control form-control-lg fw-bold text-primary" placeholder="0.00" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-semibold small text-uppercase">Payment Mode</label>
                                <select id="ddlMode" class="form-select">
                                    <option>Cash</option>
                                    <option>Bank Transfer</option>
                                    <option>UPI / GPay</option>
                                    <option>Cheque</option>
                                </select>
                            </div>
                            <div class="col-md-12">
                                <label class="form-label fw-semibold small text-uppercase">Narration / Remarks</label>
                                <textarea id="txtNarration" class="form-control" rows="2" placeholder="Describe the transaction..."></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer bg-light p-3 border-0">
                        <button type="button" class="btn btn-light px-4" data-bs-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary px-5 fw-bold" onclick="saveVoucher()">Confirm & Save</button>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>

    <script>
        $(document).ready(function () {
            loadCustomers();
            loadVouchers();
        });

        function loadVouchers() {
            $.ajax({
                type: "POST", url: "Vouchers.aspx/GetRecentVouchers", data: "{}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (r) {
                    var html = "";
                    var data = JSON.parse(r.d);
                    $.each(data, function (k, v) {
                        var typeClass = v.VoucherType == "Receipt" ? "v-receipt" : "v-payment";
                        var date = v.VoucherDate.split('T')[0];
                        html += `<tr>
                            <td data-label="Voucher No" class="ps-4">
                                <div class="fw-bold text-primary">${v.VoucherNo}</div>
                                <div class="text-muted small">${date}</div>
                            </td>
                            <td data-label="Type"><span class="v-badge ${typeClass}">${v.VoucherType}</span></td>
                            <td data-label="Customer" class="fw-semibold">${v.CustomerName}</td>
                            <td data-label="Method"><span class="mode-tag">${v.PaymentMode}</span></td>
                            <td data-label="Amount" class="text-md-end fw-bold">₹ ${v.Amount.toLocaleString()}</td>
                            <td data-label="Action" class="text-center">
                                <button type="button" class="btn btn-sm btn-outline-primary border-0 me-1" onclick="editVoucher(${v.VoucherID})"><i class="fa fa-edit"></i></button>
                                <button type="button" class="btn btn-sm btn-outline-success border-0" onclick="printVoucher(${v.VoucherID})"><i class="fa fa-print"></i></button>
                            </td>
                        </tr>`;
                    });
                    $('#voucherTableBody').html(html);
                }
            });
        }

        // Logic functions remain identical to your working version
        function openVoucherModal(id) {
            if (id == 0) {
                $('#hfVchID').val('0'); $('#vchTitle').text('New Voucher');
                $('#txtAmount').val(''); $('#txtNarration').val(''); $('#ddlCustomer').val('0');
                document.getElementById('txtDate').valueAsDate = new Date();
                $('#vchModal').modal('show');
            }
        }

        function loadCustomers() {
            $.ajax({
                type: "POST", url: "Vouchers.aspx/GetCustomerDropdown", data: "{}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (r) {
                    var data = JSON.parse(r.d);
                    var ddl = $('#ddlCustomer');
                    ddl.append('<option value="0">Select Customer</option>');
                    $.each(data, function (k, v) {
                        ddl.append($("<option></option>").val(v.CustomerID).html(v.CustomerName));
                    });
                }
            });
        }

        function editVoucher(id) {
            $.ajax({
                type: "POST", url: "Vouchers.aspx/GetVoucherByID", data: JSON.stringify({ id: id }),
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (r) {
                    var v = JSON.parse(r.d)[0];
                    $('#hfVchID').val(v.VoucherID); $('#ddlType').val(v.VoucherType);
                    $('#txtDate').val(v.VoucherDate.split('T')[0]); $('#ddlCustomer').val(v.CustomerID);
                    $('#txtAmount').val(v.Amount); $('#ddlMode').val(v.PaymentMode);
                    $('#txtNarration').val(v.Narration); $('#vchTitle').text('Edit: ' + v.VoucherNo);
                    $('#vchModal').modal('show');
                }
            });
        }

        function saveVoucher() {
            var obj = {
                VchID: $('#hfVchID').val(), VchType: $('#ddlType').val(), VchDate: $('#txtDate').val(),
                CustID: $('#ddlCustomer').val(), Amount: $('#txtAmount').val(), Mode: $('#ddlMode').val(),
                Narration: $('#txtNarration').val()
            };
            if (obj.CustID == "0" || obj.Amount <= 0) { alert("Please complete form details."); return; }
            $.ajax({
                type: "POST", url: "Vouchers.aspx/SaveVoucher", data: JSON.stringify({ vch: obj }),
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (r) { $('#vchModal').modal('hide'); loadVouchers(); }
            });
        }

        function printVoucher(id) { window.open("PrintVoucher.aspx?ID=" + id, "_blank"); }
    </script>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Customers.aspx.cs" Inherits="BKSoftwares.Customers" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Customer Master | BK Softwares</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    <style>
        :root {
            --primary: #6366f1; --accent: #4f46e5;
            --bg: #f8fafc; --dark: #0f172a; --text-light: #64748b;
        }
        body { background-color: var(--bg); font-family: 'Inter', sans-serif; color: var(--dark); padding: 20px; }

        /* Header UI */
        .page-header { margin-bottom: 2rem; }
        .btn-add { background: var(--primary); color: white; border-radius: 10px; padding: 10px 24px; font-weight: 600; border: none; transition: 0.3s; }
        .btn-add:hover { background: var(--accent); transform: translateY(-2px); color: white; }

        /* Search Box */
        .search-container { max-width: 400px; margin-bottom: 1.5rem; }
        .search-box { border-radius: 12px; padding: 12px 15px; border: 1px solid #e2e8f0; background: white; box-shadow: 0 2px 4px rgba(0,0,0,0.02); }

        /* Main Table Container */
        .main-container { background: white; border-radius: 16px; box-shadow: 0 10px 15px -3px rgba(0,0,0,0.05); border: 1px solid #e2e8f0; overflow: hidden; }
        
        /* Table Styling (Desktop) */
        .table { margin-bottom: 0; }
        .table thead { background: #f1f5f9; }
        .table thead th { font-size: 0.75rem; text-transform: uppercase; letter-spacing: 0.05em; color: var(--text-light); padding: 16px; border: none; }
        .table tbody td { padding: 16px; border-bottom: 1px solid #f1f5f9; vertical-align: middle; font-size: 0.95rem; }
        .table tbody tr:hover { background-color: #f8fafc; }

        /* Badges */
        .cust-badge { background: #e0e7ff; color: #4338ca; font-weight: 700; padding: 5px 10px; border-radius: 6px; font-size: 0.8rem; }
        .app-badge { background: #f1f5f9; color: #475569; padding: 4px 8px; border-radius: 6px; font-size: 0.75rem; font-weight: 600; border: 1px solid #e2e8f0; }

        /* --- MOBILE CARD VIEW --- */
        @media (max-width: 768px) {
            body { padding: 10px; }
            .search-container { max-width: 100%; }
            .main-container { background: transparent; border: none; box-shadow: none; }
            .resp-table thead { display: none; }
            .resp-table, .resp-table tbody, .resp-table tr, .resp-table td { display: block; width: 100%; }
            
            .resp-table tr { 
                background: white; border-radius: 16px; margin-bottom: 16px; 
                padding: 20px; box-shadow: 0 4px 6px -1px rgba(0,0,0,0.1);
                border: 1px solid #e2e8f0; position: relative;
            }

            .resp-table td { border: none; padding: 8px 0; display: flex; justify-content: space-between; align-items: center; text-align: right; }
            .resp-table td::before { content: attr(data-label); font-weight: 600; color: var(--text-light); font-size: 0.8rem; text-align: left; text-transform: uppercase; }
            
            /* UI Tweaks for Mobile Card */
            .resp-table td[data-label="Code"] { border-bottom: 1px solid #f1f5f9; padding-bottom: 12px; margin-bottom: 10px; }
            .resp-table td[data-label="Action"] { border-top: 1px solid #f1f5f9; margin-top: 12px; padding-top: 15px; justify-content: center; }
            .btn-edit-mobile { width: 100%; padding: 10px; border-radius: 10px; }
        }

        /* Modal Styling */
        .modal-content { border-radius: 20px; border: none; }
        .form-control, .form-select { border-radius: 10px; padding: 12px; border: 1px solid #e2e8f0; }
        .form-control:focus { box-shadow: 0 0 0 4px rgba(99, 102, 241, 0.1); border-color: var(--primary); }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <!-- Header -->
            <div class="page-header d-flex justify-content-between align-items-center flex-wrap gap-3">
                <div>
                    <h2 class="fw-bold m-0 text-dark">Customer Master</h2>
                    <%--<p class="text-muted m-0">Manage your client registry and system configurations</p>--%>
                </div>
                <button type="button" class="btn-add shadow-sm" onclick="openNewModal()">
                    <i class="fas fa-plus-circle me-2"></i>New Customer
                </button>
            </div>

            <!-- Search Area -->
            <div class="search-container">
                <div class="input-group search-box">
                    <span class="input-group-text bg-white border-0"><i class="fa fa-search text-muted"></i></span>
                    <input type="text" id="txtSearch" class="form-control border-0" placeholder="Search clients..." onkeyup="filterTable()" />
                </div>
            </div>

            <!-- List Section -->
            <div class="main-container">
                <div class="table-responsive">
                    <table class="table table-hover align-middle resp-table">
                        <thead>
                            <tr>
                                <th class="ps-4">Code</th>
                                <th>Customer Name</th>
                                <th>Company</th>
                                <th>Application</th>
                                <th class="text-center">Nodes</th>
                                <th>City</th>
                                <th class="text-center">Action</th>
                            </tr>
                        </thead>
                        <tbody id="custTableBody"></tbody>
                    </table>
                </div>
            </div>
        </div>

        <!-- Modal -->
        <div class="modal fade" id="custModal" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog modal-lg modal-dialog-centered">
                <div class="modal-content shadow-lg">
                    <div class="modal-header border-0 p-4 pb-0">
                        <h5 class="fw-bold m-0" id="modalTitle">Customer Details</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                    </div>
                    <div class="modal-body p-4">
                        <input type="hidden" id="hfCustID" value="0" />
                        <div class="row g-4">
                            <div class="col-md-6">
                                <label class="form-label fw-semibold small text-uppercase">Full Name</label>
                                <input type="text" id="txtName" class="form-control" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-semibold small text-uppercase">Company Name</label>
                                <input type="text" id="txtCompanyName" class="form-control" />
                            </div>
                            <div class="col-md-4">
                                <label class="form-label fw-semibold small text-uppercase">Application</label>
                                <input type="text" id="txtApp" class="form-control" placeholder="e.g. GStock" />
                            </div>
                            <div class="col-md-4">
                                <label class="form-label fw-semibold small text-uppercase">Nodes (Systems)</label>
                                <input type="number" id="txtSysCount" class="form-control" value="0" />
                            </div>
                            <div class="col-md-4">
                                <label class="form-label fw-semibold small text-uppercase">Phone</label>
                                <input type="text" id="txtPhone" class="form-control" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-semibold small text-uppercase">Email</label>
                                <input type="email" id="txtEmail" class="form-control" />
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-semibold small text-uppercase">City</label>
                                <input type="text" id="txtCity" class="form-control" />
                            </div>
                            <div class="col-md-12">
                                <label class="form-label fw-semibold small text-uppercase">Address</label>
                                <textarea id="txtAddress" class="form-control" rows="2"></textarea>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label fw-semibold small text-uppercase text-primary">Opening Balance (₹)</label>
                                <input type="number" id="txtBalance" class="form-control fw-bold border-primary" value="0" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer bg-light p-3 border-0">
                        <button type="button" class="btn btn-light px-4" data-bs-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary px-5 fw-bold" onclick="saveCustomer()">Save Customer</button>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        $(document).ready(function () { loadCustomers(); });

        function loadCustomers() {
            $.ajax({
                type: "POST", url: "Customers.aspx/GetCustomers", data: "{}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (r) {
                    var html = ""; var data = JSON.parse(r.d);
                    $.each(data, function (k, v) {
                        html += `<tr>
                            <td data-label="Code" class="ps-4"><span class="cust-badge">${v.CustCode}</span></td>
                            <td data-label="Customer" class="fw-bold">${v.CustomerName}</td>
                            <td data-label="Company" class="text-muted">${v.CompanyName || '---'}</td>
                            <td data-label="App"><span class='app-badge'>${v.Application || '---'}</span></td>
                            <td data-label="Nodes" class='text-center fw-bold text-primary'>${v.SystemCount}</td>
                            <td data-label="City">${v.City || '---'}</td>
                            <td data-label="Action" class='text-center'>
                                <button type='button' class='btn btn-sm btn-outline-primary border-0 btn-edit-mobile' onclick='editCustomer(${v.CustomerID})'>
                                    <i class='fa fa-edit me-1'></i> Edit
                                </button>
                            </td>
                        </tr>`;
                    });
                    $('#custTableBody').html(html);
                }
            });
        }

        function openNewModal() {
            $('#hfCustID').val('0'); $('#form1')[0].reset();
            $('#modalTitle').text('New Customer Registration'); $('#custModal').modal('show');
        }

        function editCustomer(id) {
            $.ajax({
                type: "POST", url: "Customers.aspx/GetCustomerByID", data: JSON.stringify({ id: id }),
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (r) {
                    var c = JSON.parse(r.d)[0];
                    $('#hfCustID').val(c.CustomerID); $('#txtName').val(c.CustomerName);
                    $('#txtCompanyName').val(c.CompanyName); $('#txtApp').val(c.Application);
                    $('#txtSysCount').val(c.SystemCount); $('#txtPhone').val(c.Phone);
                    $('#txtEmail').val(c.Email); $('#txtCity').val(c.City);
                    $('#txtAddress').val(c.Address); $('#txtBalance').val(c.OpeningBalance);
                    $('#modalTitle').text('Edit Customer Profile'); $('#custModal').modal('show');
                }
            });
        }

        function saveCustomer() {
            var obj = {
                ID: $('#hfCustID').val(), Name: $('#txtName').val(), Company: $('#txtCompanyName').val(),
                App: $('#txtApp').val(), SysCount: $('#txtSysCount').val(), Phone: $('#txtPhone').val(),
                Email: $('#txtEmail').val(), City: $('#txtCity').val(), Address: $('#txtAddress').val(), Bal: $('#txtBalance').val()
            };
            $.ajax({
                type: "POST", url: "Customers.aspx/SaveCustomer", data: JSON.stringify({ obj: obj }),
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (r) { $('#custModal').modal('hide'); loadCustomers(); }
            });
        }

        function filterTable() {
            var val = $('#txtSearch').val().toLowerCase();
            $("#custTableBody tr").filter(function () { $(this).toggle($(this).text().toLowerCase().indexOf(val) > -1); });
        }
    </script>
</body>
</html>
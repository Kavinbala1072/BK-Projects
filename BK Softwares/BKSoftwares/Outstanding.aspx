<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Outstanding.aspx.cs" Inherits="BKSoftwares.Outstanding" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Outstanding Report | BK Softwares</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />

    <style>
        :root {
            --primary: #6366f1; --success: #10b981; --danger: #f43f5e;
            --bg: #f8fafc; --dark: #0f172a; --text-light: #64748b;
        }
        body { background-color: var(--bg); font-family: 'Inter', sans-serif; color: var(--dark); padding: 20px; }

        /* Header UI */
        .page-header { margin-bottom: 1.5rem; }
        .btn-print { background: var(--dark); color: white; border-radius: 10px; padding: 10px 20px; font-weight: 600; border: none; transition: 0.3s; }
        .btn-print:hover { background: #1e293b; transform: translateY(-2px); color: white; }

        /* Summary Widgets */
        .stat-card { 
            background: white; border-radius: 16px; padding: 20px; border: 1px solid #e2e8f0; 
            box-shadow: 0 4px 6px -1px rgba(0,0,0,0.05); transition: 0.3s;
            border-left: 5px solid var(--primary);
        }
        .stat-card.success { border-left-color: var(--success); }
        .stat-card.danger { border-left-color: var(--danger); }
        .stat-card.dark { border-left-color: var(--dark); }
        
        .stat-label { font-size: 0.75rem; font-weight: 700; text-transform: uppercase; color: var(--text-light); letter-spacing: 0.05em; }

        /* Main Container */
        .main-container { background: white; border-radius: 16px; box-shadow: 0 10px 15px -3px rgba(0,0,0,0.05); border: 1px solid #e2e8f0; overflow: hidden; margin-top: 2rem; }
        
        /* Table Styling (Desktop) */
        .table { margin-bottom: 0; }
        .table thead { background: #f1f5f9; }
        .table thead th { font-size: 0.75rem; text-transform: uppercase; letter-spacing: 0.05em; color: var(--text-light); padding: 16px; border: none; }
        .table tbody td { padding: 16px; border-bottom: 1px solid #f1f5f9; vertical-align: middle; font-size: 0.9rem; }
        
        /* Typography & Colors */
        .cust-id { background: #e0e7ff; color: #4338ca; font-weight: 700; padding: 4px 8px; border-radius: 6px; font-size: 0.75rem; }
        .amt-pos { color: var(--success); font-weight: 700; }
        .amt-neg { color: var(--danger); font-weight: 700; }

        /* Search Filter */
        .search-box { border-radius: 10px; border: 1px solid #e2e8f0; padding: 10px 15px; max-width: 300px; }

        /* --- MOBILE CARD VIEW --- */
        @media (max-width: 992px) {
            body { padding: 10px; }
            .main-container { background: transparent; border: none; box-shadow: none; }
            .resp-table thead { display: none; }
            .resp-table, .resp-table tbody, .resp-table tr, .resp-table td { display: block; width: 100%; }
            
            .resp-table tr { 
                background: white; border-radius: 16px; margin-bottom: 16px; 
                padding: 15px 20px; box-shadow: 0 4px 6px -1px rgba(0,0,0,0.1);
                border: 1px solid #e2e8f0; position: relative;
            }

            .resp-table td { border: none; padding: 8px 0; display: flex; justify-content: space-between; align-items: center; text-align: right; border-bottom: 1px solid #f8fafc; }
            .resp-table td:last-child { border-bottom: none; background: #f8fafc; margin-top: 10px; border-radius: 12px; padding: 12px; font-size: 1.1rem; }
            
            .resp-table td::before { content: attr(data-label); font-weight: 700; color: var(--text-light); font-size: 0.7rem; text-align: left; text-transform: uppercase; }
        }
        
        @media print { .no-print { display: none !important; } body { padding: 0; background: white; } .main-container { border: none; box-shadow: none; } }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <!-- Premium Header -->
            <div class="page-header d-flex justify-content-between align-items-center flex-wrap gap-3 no-print">
                <div>
                    <h2 class="fw-bold m-0 text-dark">Outstanding Report</h2>
                    <%--<p class="text-muted m-0 small">Real-time financial balance overview for all customers</p>--%>
                </div>
                <div class="d-flex gap-2">
                    <input type="text" id="txtSearch" class="search-box form-control" placeholder="Filter by customer..." onkeyup="filterTable()" />
                    <button type="button" class="btn-print shadow-sm" onclick="window.print()">
                        <i class="fas fa-print me-2"></i>Print
                    </button>
                </div>
            </div>

            <!-- Stats Overview -->
            <div class="row g-3">
                <div class="col-6 col-md-3">
                    <div class="stat-card">
                        <div class="stat-label">Total Opening</div>
                        <h3 class="fw-bold m-0 mt-1" id="sumOpening">₹ 0.00</h3>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="stat-card success">
                        <div class="stat-label">Receipts (Cr)</div>
                        <h3 class="fw-bold m-0 mt-1 text-success" id="sumReceipts">₹ 0.00</h3>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="stat-card danger">
                        <div class="stat-label">Payments (Dr)</div>
                        <h3 class="fw-bold m-0 mt-1 text-danger" id="sumPayments">₹ 0.00</h3>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="stat-card dark">
                        <div class="stat-label">Balance Outstanding</div>
                        <h3 class="fw-bold m-0 mt-1" id="sumNet">₹ 0.00</h3>
                    </div>
                </div>
            </div>

            <!-- Main Report Table -->
            <div class="main-container">
                <div id="loader" class="text-center p-5 no-print">
                    <div class="spinner-border text-primary" role="status"></div>
                    <p class="text-muted mt-2 small fw-bold">Calculating Balances...</p>
                </div>
                <div class="table-responsive">
                    <table class="table table-hover align-middle resp-table" id="outstandingTable">
                        <thead>
                            <tr>
                                <th class="ps-4">Code</th>
                                <th>Customer Name</th>
                                <th class="text-end">Opening</th>
                                <th class="text-end">Receipts (-)</th>
                                <th class="text-end">Payments (+)</th>
                                <th class="pe-4 text-end">Net Balance</th>
                            </tr>
                        </thead>
                        <tbody id="outstandingTableBody"></tbody>
                    </table>
                </div>
            </div>
            <%--<p class="text-center text-muted small mt-4 no-print">Generated on: <%= DateTime.Now.ToString("f") %></p>--%>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function () { loadOutstanding(); });

        function loadOutstanding() {
            $.ajax({
                type: "POST", url: "Outstanding.aspx/GetOutstandingReport", data: "{}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (r) {
                    $('#loader').hide();
                    var data = JSON.parse(r.d);
                    var html = "";
                    var tOpen = 0, tRec = 0, tPay = 0, tNet = 0;

                    $.each(data, function (k, v) {
                        tOpen += v.OpeningBalance;
                        tRec += v.TotalReceipts;
                        tPay += v.TotalPayments;
                        tNet += v.Balance;

                        var balanceStyle = v.Balance > 0 ? "amt-neg" : "amt-pos";

                        html += `<tr>
                            <td data-label="Code" class="ps-4"><span class="cust-id">${v.CustCode}</span></td>
                            <td data-label="Customer"><div class="fw-bold text-dark">${v.CustomerName}</div></td>
                            <td data-label="Opening" class="text-md-end text-muted">₹ ${v.OpeningBalance.toLocaleString('en-IN', { minimumFractionDigits: 2 })}</td>
                            <td data-label="Receipts" class="text-md-end amt-pos">₹ ${v.TotalReceipts.toLocaleString('en-IN', { minimumFractionDigits: 2 })}</td>
                            <td data-label="Payments" class="text-md-end amt-neg">₹ ${v.TotalPayments.toLocaleString('en-IN', { minimumFractionDigits: 2 })}</td>
                            <td data-label="Net Balance" class="pe-4 text-md-end fw-bold ${balanceStyle}">
                                ₹ ${v.Balance.toLocaleString('en-IN', { minimumFractionDigits: 2 })}
                            </td>
                        </tr>`;
                    });

                    $('#outstandingTableBody').html(html).fadeIn();

                    $('#sumOpening').text('₹ ' + tOpen.toLocaleString('en-IN', { minimumFractionDigits: 2 }));
                    $('#sumReceipts').text('₹ ' + tRec.toLocaleString('en-IN', { minimumFractionDigits: 2 }));
                    $('#sumPayments').text('₹ ' + tPay.toLocaleString('en-IN', { minimumFractionDigits: 2 }));
                    $('#sumNet').text('₹ ' + tNet.toLocaleString('en-IN', { minimumFractionDigits: 2 }));
                }
            });
        }

        function filterTable() {
            var val = $('#txtSearch').val().toLowerCase();
            $("#outstandingTableBody tr").filter(function () { $(this).toggle($(this).text().toLowerCase().indexOf(val) > -1); });
        }
    </script>
</body>
</html>
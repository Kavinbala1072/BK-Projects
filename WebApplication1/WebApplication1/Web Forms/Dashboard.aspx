<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Dashboard.aspx.vb" Inherits="WebApplication1.Dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Enterprise Analytics Dashboard</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@300;400;500;600;700;800&display=swap" rel="stylesheet" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>

    <style>
        :root {
            --bg-body: #f1f5f9;
            --card-bg: #ffffff;
            --primary: #6366f1;
            --success: #10b981;
            --danger: #ef4444;
            --warning: #f59e0b;
            --info: #0ea5e9;
            --text-main: #1e293b;
            --text-muted: #64748b;
            --border: #e2e8f0;
        }

        * { box-sizing: border-box; }

        body { 
            background-color: var(--bg-body); 
            font-family: 'Inter', sans-serif; 
            color: var(--text-main); 
            -webkit-font-smoothing: antialiased;
            margin: 0; padding: 0;
            height: auto;
            overflow-y: auto; 
        }

        .dashboard-wrapper {
            display: grid;
            grid-template-columns: 1fr;
            gap: 1rem;
            padding: 0.5rem;
            width: 100%;
        }

        @media (min-width: 1200px) {
            body {
                height: 100vh;
                overflow: hidden;
            }
            .dashboard-wrapper {
                grid-template-columns: 320px 1fr 320px;
                height: 100vh;
            }
            .scroll-content {
                overflow-y: auto;
            }
            .list-scroll-box {
                overflow-y: auto;
            }
        }
        .glass-card { 
            background: var(--card-bg); 
            border-radius: 1rem; 
            border: 1px solid var(--border); 
            box-shadow: 0 1px 3px 0 rgb(0 0 0 / 0.1);
            display: flex; 
            flex-direction: column;
            height: auto;
            min-height: 0;
            overflow: hidden; 
            flex-grow: 1;
            padding: 0 1rem 1rem 1rem;
        }

        .card-header-custom {
            padding: 1rem 1rem 1rem 1rem;
            display: flex; align-items: center; justify-content: space-between;
            flex-shrink: 0;
        }

        .section-title { 
            font-size: 0.75rem; font-weight: 800; color: var(--text-muted); 
            text-transform: uppercase; letter-spacing: 0.05em; 
        }

        .date-input{
            height:30px;
            width: 120px;
            font-size: 12px;
        }
        .form-btn{
            height:30px;
            width: 30px;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: 6px;
            background: #f1f5f9;
            color: #64748b;
            text-decoration: none;
            transition: all 0.2s;
        }
        .form-btn:hover { background: #e2e8f0; color: var(--primary); }

        .stat-item { 
            display: flex; align-items: center; gap: 0.85rem; padding: 0.75rem; 
            border-radius: 0.75rem; background: #fff; border: 1px solid var(--border); margin-bottom: 0.6rem; 
            flex-shrink: 0;
        }

        .icon-box { 
            width: 36px; height: 36px; border-radius: 0.65rem; 
            display: flex; align-items: center; justify-content: center; font-size: 0.9rem; flex-shrink: 0; 
        }
        
        .val-large { font-size: 1rem; font-weight: 700; color: var(--text-main); display: block; line-height: 1.2; }
        .lbl-tiny { font-size: 0.6rem; font-weight: 700; color: var(--text-muted); text-transform: uppercase; display: block; margin-bottom: 2px; }

        .side-value-card { 
            padding: 8px 12px; border-radius: 10px; border-left: 4px solid var(--primary); 
            background: #f8fafc; margin-bottom: 6px;
        }
        .card-val-text { font-weight: 700; font-size: 0.85rem; color: var(--text-main); }

        .list-scroll-box {
            flex-grow: 1;
            min-height: 50px;
        }

        .list-item-row { 
            display: flex; justify-content: space-between; align-items: center; 
            padding: 6px 0; border-bottom: 1px solid #f1f5f9; 
        }
        .list-item-name { font-size: 0.75rem; font-weight: 600; color: var(--text-main); }
        .list-item-val { font-size: 0.8rem; font-weight: 700; }

        .chart-box { position: relative; height: 130px; width: 100%; max-width: 130px; margin: 0 auto; }
        .trend-box { position: relative; flex-grow: 1; width: 100%; min-height: 300px; }

        .main-col { display: flex; flex-direction: column; gap: 1rem; height: auto; }
        
        @media (min-width: 1200px) {
            .main-col { height: 100%; overflow: hidden; }
        }

        .badge-lite { background: #f1f5f9; color: #64748b; font-size: 0.7rem; font-weight: 600; border: 1px solid var(--border); }

        @media (max-width: 991px) {
            .border-end { border-end: none !important; border-bottom: 1px solid var(--border); padding-bottom: 1rem; margin-bottom: 1rem; }
        }

        #loader-wrapper {
            position: fixed; top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(255, 255, 255, 0.9); z-index: 9999;
            display: flex; flex-direction: column; justify-content: center; align-items: center;
        }
        .loader-spinner {
            width: 45px; height: 45px;
            border: 4px solid #f3f3f3; border-top: 4px solid var(--accent);
            border-radius: 50%; animation: spin 0.8s linear infinite;
        }
        .loader-text { margin-top: 15px; font-weight: 600; color: var(--primary-dark1); font-size: 0.85rem; letter-spacing: 0.5px; }
        @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }

    </style>

<%--        <script type="text/javascript">
            function showLoader() {
                document.getElementById('loader-wrapper').style.display = 'flex';
            }
            window.onload = function () {
                document.getElementById('loader-wrapper').style.display = 'none';
            };
        </script>--%>
</head>
<body>
    <form id="form1" runat="server">
<%--        <div id="loader-wrapper">
            <div class="loader-spinner"></div>
            <div class="loader-text">FETCHING DATA...</div>
        </div>--%>

        <asp:HiddenField ID="hfSToday" runat="server" Value="0" />
        <asp:HiddenField ID="hfPToday" runat="server" Value="0" />
        <asp:HiddenField ID="hfSMonth" runat="server" Value="0" />
        <asp:HiddenField ID="hfPMonth" runat="server" Value="0" />
        <asp:HiddenField ID="hfOutLabels" runat="server" Value="[]" />
        <asp:HiddenField ID="hfOutDrData" runat="server" Value="[]" />
        <asp:HiddenField ID="hfOutCrData" runat="server" Value="[]" />

        <div class="dashboard-wrapper">            
            <div class="glass-card">
                <div class="card-header-custom">
                    <span class="section-title">Today Overview</span>
                    <div class="d-flex gap-2 align-items-center">
                        <asp:TextBox ID="txtToDate" runat="server" TextMode="Date" CssClass="form-control date-input" AutoPostBack="true" OnTextChanged="txtToDate_TextChanged"></asp:TextBox>
                        <asp:LinkButton ID="btnRefresh" runat="server" CssClass="form-btn" OnClick="btnRefresh_Click"><i class="fas fa-sync-alt"></i> </asp:LinkButton>
                    </div>
                </div>
                
                    <div class="stat-item" style="background: #eff6ff;">
                        <div class="icon-box" style="background: #3b82f6; color: #fff;"><i class="fas fa-wallet"></i></div>
                        <div><span class="lbl-tiny" style="color:#1d4ed8">Cash In Hand</span><span class="val-large">₹<asp:Literal ID="litCashInHand" runat="server" /></span></div>
                    </div>

                    <div class="stat-item">
                        <div class="icon-box" style="background: #eef2ff; color: #6366f1;"><i class="fas fa-shopping-cart"></i></div>
                        <div><span class="lbl-tiny">Sales Today</span><span class="val-large">₹<asp:Literal ID="litSalesToday" runat="server" /></span></div>
                    </div>

                    <div class="stat-item">
                        <div class="icon-box" style="background: #ecfdf5; color: #10b981;"><i class="fas fa-cash-register"></i></div>
                        <div><span class="lbl-tiny">Cash Sales</span><span class="val-large text-success">₹<asp:Literal ID="litCashSales" runat="server" /></span></div>
                    </div>

                    <div class="stat-item">
                        <div class="icon-box" style="background: #fff1f2; color: #f43f5e;"><i class="fas fa-truck-loading"></i></div>
                        <div><span class="lbl-tiny">Purchase Today</span><span class="val-large text-danger">₹<asp:Literal ID="litPurToday" runat="server" /></span></div>
                    </div>

                    <div class="stat-item">
                        <div class="icon-box" style="background: #fffbeb; color: #f59e0b;"><i class="fas fa-undo"></i></div>
                        <div><span class="lbl-tiny">Sales Returns</span><span class="val-large text-warning">₹<asp:Literal ID="litReturns" runat="server" /></span></div>
                    </div>

                    <div class="stat-item">
                        <div class="icon-box" style="background: #f0f9ff; color: #0ea5e9;"><i class="fas fa-file-invoice-dollar"></i></div>
                        <div><span class="lbl-tiny">Total Receipts</span><span class="val-large text-info">₹<asp:Literal ID="litReceipts" runat="server" /></span></div>
                    </div>

                <div class="scroll-content">
                    <div class="mt-2 d-flex flex-column flex-grow-1">
                        <h6 class="lbl-tiny text-info border-bottom pb-2 mb-2">Bank Balances</h6>
                        <div class="list-scroll-box">
                            <asp:Repeater ID="rptBankBalances" runat="server">
                                <ItemTemplate>
                                    <div class="list-item-row">
                                        <span class="list-item-name"><%# Eval("ledger_name") %></span>
                                        <span class="list-item-val text-info">₹<%# Eval("ClosingBalance", "{0:N2}") %></span>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>

            <div class="main-col">
                <div class="glass-card p-3">
                    <div class="row g-3">
                        <div class="col-lg-6 border-end">
                            <span class="section-title mb-2 d-block">Daily Split</span>
                            <div class="d-flex align-items-center justify-content-between">
                                <div class="chart-box"><canvas id="chartDaily"></canvas></div>
                                <div class="flex-grow-1 ms-3">
                                    <div class="side-value-card" style="border-left-color: var(--primary);"><span class="lbl-tiny">Sale</span><span class="card-val-text" id="lblDS">₹0</span></div>
                                    <div class="side-value-card" style="border-left-color: var(--success);"><span class="lbl-tiny">Purchase</span><span class="card-val-text" id="lblDP">₹0</span></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 ps-lg-4">
                            <span class="section-title mb-2 d-block">Monthly Split</span>
                            <div class="d-flex align-items-center justify-content-between">
                                <div class="chart-box"><canvas id="chartMonth"></canvas></div>
                                <div class="flex-grow-1 ms-3">
                                    <div class="side-value-card" style="border-left-color: var(--primary);"><span class="lbl-tiny">MTD Sale</span><span class="card-val-text" id="lblMS">₹0</span></div>
                                    <div class="side-value-card" style="border-left-color: var(--warning);"><span class="lbl-tiny">MTD Pur.</span><span class="card-val-text" id="lblMP">₹0</span></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="glass-card p-4 flex-grow-1">
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <span class="section-title">Receivable vs Payable Trend</span>
                        <span class="badge badge-lite p-2">Analytics View</span>
                    </div>
                    <div class="trend-box">
                        <canvas id="outBarChart"></canvas>
                    </div>
                </div>
            </div>

            <div class="glass-card">
                <div class="card-header-custom">
                    <span class="section-title">Outstanding Ledgers</span>
                    <i class="fas fa-users text-muted"></i>
                </div>
                <div class="scroll-content">
                    <h6 class="lbl-tiny text-primary border-bottom pb-2 mb-2">Top Debtors</h6>
                    <div class="list-scroll-box">
                        <asp:Repeater ID="rptTopCustomers" runat="server">
                            <ItemTemplate>
                                <div class="list-item-row">
                                    <span class="list-item-name text-truncate" style="max-width: 160px;"><%# Eval("ledger_Name") %></span>
                                    <span class="list-item-val text-primary">₹<%# Eval("TotalBalance", "{0:N0}") %></span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <div class="mt-3">
                        <h6 class="lbl-tiny text-danger border-bottom pb-2 mb-2">Top Creditors</h6>
                        <div class="list-scroll-box">
                            <asp:Repeater ID="rptTopSuppliers" runat="server">
                                <ItemTemplate>
                                    <div class="list-item-row">
                                        <span class="list-item-name text-truncate" style="max-width: 160px;"><%# Eval("ledger_Name") %></span>
                                        <span class="list-item-val text-danger">₹<%# Eval("TotalBalance", "{0:N0}") %></span>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <script>
            window.onload = function () {
                const sToday = parseFloat(document.getElementById('<%= hfSToday.ClientID %>').value) || 0;
                const pToday = parseFloat(document.getElementById('<%= hfPToday.ClientID %>').value) || 0;
                const sMonth = parseFloat(document.getElementById('<%= hfSMonth.ClientID %>').value) || 0;
                const pMonth = parseFloat(document.getElementById('<%= hfPMonth.ClientID %>').value) || 0;

                const fmt = v => '₹' + v.toLocaleString('en-IN');
                document.getElementById('lblDS').innerText = fmt(sToday);
                document.getElementById('lblDP').innerText = fmt(pToday);
                document.getElementById('lblMS').innerText = fmt(sMonth);
                document.getElementById('lblMP').innerText = fmt(pMonth);

                const donutOpt = (val1, val2, color1, color2) => ({
                    type: 'doughnut',
                    data: {
                        datasets: [{
                            data: (val1 === 0 && val2 === 0) ? [1] : [val1, val2],
                            backgroundColor: (val1 === 0 && val2 === 0) ? ['#f1f5f9'] : [color1, color2],
                            borderWidth: 0
                        }]
                    },
                    options: { responsive: true, maintainAspectRatio: false, cutout: '78%', plugins: { legend: { display: false } } }
                });

                new Chart(document.getElementById('chartDaily'), donutOpt(sToday, pToday, '#6366f1', '#10b981'));
                new Chart(document.getElementById('chartMonth'), donutOpt(sMonth, pMonth, '#6366f1', '#f59e0b'));

                try {
                    const ctx = document.getElementById('outBarChart').getContext('2d');
                    const outLabels = JSON.parse(document.getElementById('<%= hfOutLabels.ClientID %>').value);
                    const outDrData = JSON.parse(document.getElementById('<%= hfOutDrData.ClientID %>').value);
                    const outCrData = JSON.parse(document.getElementById('<%= hfOutCrData.ClientID %>').value);

                    new Chart(ctx, {
                        type: 'bar',
                        data: {
                            labels: outLabels,
                            datasets: [
                                {
                                    label: 'Receivable',
                                    data: outDrData,
                                    backgroundColor: '#6366f1',
                                    hoverBackgroundColor: '#4f46e5',
                                    borderRadius: 10,
                                    borderSkipped: false,
                                    barPercentage: 0.6,
                                    categoryPercentage: 0.5
                                },
                                {
                                    label: 'Payable',
                                    data: outCrData,
                                    backgroundColor: '#f43f5e',
                                    hoverBackgroundColor: '#e11d48',
                                    borderRadius: 10,
                                    borderSkipped: false,
                                    barPercentage: 0.6,
                                    categoryPercentage: 0.5
                                }
                            ]
                        },
                        options: {
                            responsive: true,
                            maintainAspectRatio: false,
                            interaction: { intersect: false, mode: 'index' },
                            plugins: {
                                legend: {
                                    position: 'top',
                                    align: 'end',
                                    labels: {
                                        boxWidth: 8,
                                        usePointStyle: true,
                                        pointStyle: 'circle',
                                        font: { size: 11, weight: '600' }
                                    }
                                },
                                tooltip: {
                                    backgroundColor: '#1e293b',
                                    padding: 12,
                                    bodySpacing: 4,
                                    usePointStyle: true
                                }
                            },
                            scales: {
                                y: {
                                    grid: { color: '#f1f5f9', drawBorder: false },
                                    ticks: {
                                        font: { size: 10, weight: '500' },
                                        color: '#64748b',
                                        callback: function (value) {
                                            const absVal = Math.abs(value);
                                            if (absVal >= 10000000) return (value / 10000000).toFixed(1) + 'Cr';
                                            if (absVal >= 100000) return (value / 100000).toFixed(1) + 'L';
                                            if (absVal >= 1000) return (value / 1000).toFixed(0) + 'k';
                                            return value;
                                        }
                                    }
                                },
                                x: {
                                    grid: { display: false },
                                    ticks: { font: { size: 11, weight: '600' }, color: '#1e293b' }
                                }
                            }
                        }
                    });
                } catch (e) { console.error(e); }
            };
        </script>
    </form>
</body>
</html>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerList.aspx.cs" Inherits="BKSoftwares.CustomerList" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Usage Analysis | BK Softwares</title>
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
        .btn-refresh { background: var(--dark); color: white; border-radius: 10px; padding: 10px 20px; font-weight: 600; border: none; transition: 0.3s; }
        .btn-refresh:hover { background: #1e293b; transform: translateY(-2px); color: white; }

        /* Summary Widgets */
        .stat-card { 
            background: white; border-radius: 16px; padding: 20px; border: 1px solid #e2e8f0; 
            box-shadow: 0 4px 6px -1px rgba(0,0,0,0.05); transition: 0.3s;
        }
        .stat-card:hover { transform: translateY(-3px); box-shadow: 0 10px 15px -3px rgba(0,0,0,0.1); }
        .stat-label { font-size: 0.75rem; font-weight: 700; text-transform: uppercase; color: var(--text-light); letter-spacing: 0.05em; }

        /* Main Container */
        .main-container { background: white; border-radius: 16px; box-shadow: 0 10px 15px -3px rgba(0,0,0,0.05); border: 1px solid #e2e8f0; overflow: hidden; margin-top: 2rem; }
        
        /* Table Styling (Desktop) */
        .table { margin-bottom: 0; }
        .table thead { background: #f1f5f9; }
        .table thead th { font-size: 0.75rem; text-transform: uppercase; letter-spacing: 0.05em; color: var(--text-light); padding: 16px; border: none; }
        .table tbody td { padding: 16px; border-bottom: 1px solid #f1f5f9; vertical-align: middle; font-size: 0.9rem; }
        
        /* Badges */
        .id-badge { background: #e0e7ff; color: #4338ca; font-weight: 700; padding: 4px 8px; border-radius: 6px; font-size: 0.75rem; }
        .app-badge { background: #f1f5f9; color: var(--text-light); padding: 4px 8px; border-radius: 6px; font-size: 0.75rem; font-weight: 600; border: 1px solid #e2e8f0; }
        .version-tag { font-size: 0.7rem; font-weight: 700; color: #b45309; background: #fef3c7; padding: 2px 6px; border-radius: 4px; margin-left: 5px; }
        .status-indicator { width: 9px; height: 9px; border-radius: 50%; display: inline-block; margin-right: 6px; }
        .online { background-color: var(--success); box-shadow: 0 0 8px var(--success); }
        .offline { background-color: var(--danger); }

        .version-container { display: flex; flex-direction: column; gap: 2px; }
        .v-cur { font-size: 0.7rem; color: var(--text-light); font-weight: 600; }
        .v-new { font-size: 0.7rem; color: var(--primary); font-weight: 700; }
        .badge-update { background: #fee2e2; color: #ef4444; font-size: 0.65rem; padding: 1px 4px; border-radius: 3px; font-weight: 800; border: 1px solid #fecaca; margin-top: 2px; display: inline-block; }

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
            .resp-table td:last-child { border-bottom: none; background: #f8fafc; margin-top: 10px; border-radius: 12px; padding: 12px; justify-content: center; flex-direction: column; text-align: center; }
            
            .resp-table td::before { content: attr(data-label); font-weight: 700; color: var(--text-light); font-size: 0.7rem; text-align: left; text-transform: uppercase; }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid">
            <!-- Premium Header -->
            <div class="page-header d-flex justify-content-between align-items-center flex-wrap gap-3">
                <div>
                    <h2 class="fw-bold m-0 text-dark">Usage Analysis</h2>
                    <%--<p class="text-muted m-0 small">Monitoring real-time client activity and system matching</p>--%>
                </div>
                <button type="button" class="btn-refresh shadow-sm" onclick="fetchData()">
                    <i class="fas fa-sync-alt me-2"></i>Refresh Report
                </button>
            </div>

            <!-- Stats Overview -->
            <div class="row g-3">
                <div class="col-6 col-md-3">
                    <div class="stat-card">
                        <div class="stat-label">Live Logs</div>
                        <h2 class="fw-bold m-0" id="statTotal">0</h2>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="stat-card">
                        <div class="stat-label">Verified</div>
                        <h2 class="fw-bold m-0 text-success" id="statMatched">0</h2>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="stat-card">
                        <div class="stat-label">Total Nodes</div>
                        <h2 class="fw-bold m-0 text-primary" id="statSystems">0</h2>
                    </div>
                </div>
                <div class="col-6 col-md-3">
                    <div class="stat-card">
                        <div class="stat-label">Expired/Trial</div>
                        <h2 class="fw-bold m-0 text-danger" id="statExpired">0</h2>
                    </div>
                </div>
            </div>

            <!-- Main Data Table -->
            <div class="main-container">
                <div id="loader" class="text-center p-5">
                    <div class="spinner-border text-primary" role="status"></div>
                    <p class="text-muted mt-2 small fw-bold">Synchronizing Data...</p>
                </div>
                <div class="table-responsive">
                    <table class="table table-hover align-middle resp-table">
                        <thead>
                            <tr>
                                <th class="ps-4">ID</th>
                                <th>Client Name</th>
                                <th>Application</th>
                                <th class="text-center">Nodes</th>
                                <th>AMC Status</th>
                                <th>DB Match</th>
                                <th class="pe-4 text-end">Last Activity</th>
                            </tr>
                        </thead>
                        <tbody id="reportTableBody"></tbody>
                    </table>
                </div>
            </div>
        </div>
    </form>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
        $(document).ready(function () { fetchData(); });

        function fetchData() {
            $('#reportTableBody').hide(); $('#loader').show();
            $.ajax({
                type: "POST", url: "CustomerList.aspx/GetUsageReport", data: "{}",
                contentType: "application/json; charset=utf-8", dataType: "json",
                success: function (r) {
                    $('#loader').hide();
                    var data = JSON.parse(r.d);
                    var html = ""; var matched = 0; var sys = 0; var expired = 0;

                    $.each(data, function (k, v) {
                        var isMatch = v.MatchStatus === "Verified";
                        if (isMatch) matched++;
                        sys += parseInt(v.SystemCount) || 0;

                        var amc = v["AMC expiry"] || "Trial/None";
                        if (amc === "Trial/None") expired++;

                        var serverClass = (v.server === "Server" || v.server === "True") ? "online" : "offline";

                        // Version Matching Logic
                        var curV = v.Version || '0.0';
                        var newV = v.NewVersion || '0.0';
                        var needsUpdate = curV !== newV;

                        html += `<tr>
                        <td data-label="Client ID" class="ps-4"><span class="id-badge">${v.DisplayID}</span></td>
                        <td data-label="Client Name"><div class="fw-bold text-dark">${v["Company name"] || 'Unknown'}</div></td>
                        <td data-label="Software">
                            <span class="app-badge">${v.Application}</span>
                            <div class="version-container mt-1">
                                <span class="v-cur">Current: v${curV}</span>
                                <span class="v-new">Latest: v${newV}</span>
                                ${needsUpdate ? '<span class="badge-update">UPDATE REQ.</span>' : ''}
                            </div>
                        </td>
                        <td data-label="Nodes" class="text-md-center fw-bold fs-5 text-primary">${v.SystemCount || 0}</td>
                        <td data-label="AMC Status"><span class="${amc === 'Trial/None' ? 'text-danger' : 'text-success'} fw-bold">${amc}</span></td>
                        <td data-label="Match">
                            <div class="small text-muted mb-1 d-md-block">Key: ${v.secretPassword || '---'}</div>
                            ${isMatch ? '<span class="text-success small fw-bold"><i class="fas fa-check-circle me-1"></i>Verified</span>' : '<span class="text-muted small italic">Unregistered</span>'}
                        </td>
                        <td data-label="Last Activity" class="text-md-end pe-4">
                            <div class="small fw-bold">
                                <span class="status-indicator ${serverClass}"></span>${v.server.toUpperCase()}
                            </div>
                            <div class="text-muted" style="font-size: 0.75rem;">${v.lastlogin}</div>
                        </td>
                    </tr>`;
                    });

                    $('#reportTableBody').html(html).fadeIn();
                    $('#statTotal').text(data.length); $('#statMatched').text(matched);
                    $('#statSystems').text(sys); $('#statExpired').text(expired);
                }
            });
        }
    </script>
</body>
</html>
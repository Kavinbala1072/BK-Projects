<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FormSetting.aspx.cs" Inherits="BKBilling.Forms.Settings.FormSetting" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Menu Configuration | BK Softwares</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap" rel="stylesheet" />
    
    <style>
        /* Force no page scroll */
        html, body { 
            height: 100vh; 
            width: 100vw; 
            overflow: hidden; 
            background-color: #f1f5f9; 
            font-family: 'Inter', sans-serif;
            margin: 0; padding: 0;
        }

        /* Fixed Layout Container */
        #form1 { height: 100vh; display: flex; flex-direction: column; }

        .header-bar {
            background: #003366;
            color: white;
            padding: 12px 25px;
            display: flex;
            justify-content: space-between;
            align-items: center;
            box-shadow: 0 2px 10px rgba(0,0,0,0.1);
            z-index: 100;
        }

        .search-area {
            background: #fff;
            padding: 10px 25px;
            border-bottom: 1px solid #e2e8f0;
        }

        /* Scrollable Grid Area (Internal Scroll only) */
        .content-scroll {
            flex: 1;
            overflow-y: auto;
            padding: 20px;
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(200px, 1fr)); /* 3-4 columns */
            gap: 12px;
            align-content: start;
        }

        /* Modern Item Card */
        .setting-item {
            background: white;
            border-radius: 8px;
            padding: 12px 15px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            border: 1px solid #e2e8f0;
            transition: all 0.2s;
        }
        .setting-item:hover { border-color: #003366; box-shadow: 0 4px 6px rgba(0,0,0,0.05); }

        .item-info { display: flex; flex-direction: column; }
        .item-name { font-size: 0.85rem; font-weight: 600; color: #1e293b; }
        .item-id { font-size: 0.7rem; color: #94a3b8; font-family: monospace; }

        /* Modern Toggle Switch */
        .switch { position: relative; display: inline-block; width: 42px; height: 22px; margin-bottom: 0; }
        .switch input { opacity: 0; width: 0; height: 0; }
        .slider { position: absolute; cursor: pointer; top: 0; left: 0; right: 0; bottom: 0; background-color: #cbd5e1; transition: .3s; border-radius: 34px; }
        .slider:before { position: absolute; content: ""; height: 16px; width: 16px; left: 3px; bottom: 3px; background-color: white; transition: .3s; border-radius: 50%; }
        input:checked + .slider { background-color: #10b981; }
        input:checked + .slider:before { transform: translateX(20px); }

        .btn-update { 
            background: #fff; color: #003366; border: none; font-weight: 700; 
            padding: 6px 20px; border-radius: 6px; font-size: 0.85rem;
        }
        .btn-update:hover { background: #e2e8f0; }

        /* Custom Scrollbar for the grid */
        .content-scroll::-webkit-scrollbar { width: 6px; }
        .content-scroll::-webkit-scrollbar-thumb { background: #cbd5e1; border-radius: 10px; }

        .toast-container { z-index: 9999; }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function showNotification(message, type) {
            var toastEl = document.getElementById('msgToast');
            document.getElementById('msgText').innerText = message;
            toastEl.className = "toast align-items-center text-white border-0 shadow-lg bg-" + (type === 'success' ? 'success' : 'danger');
            new bootstrap.Toast(toastEl, { delay: 3000 }).show();
            if (type === 'success') {
                setTimeout(function () { window.parent.location.reload(); }, 2000);
            }
        }

        // Live filtering to find settings quickly
        $(document).ready(function () {
            $("#txtFilter").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $(".setting-item").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />

        <!-- Fixed Header -->
        <div class="header-bar">
            <div>
                <h5 class="m-0 fw-bold"><i class="fas fa-cog me-2"></i>Form Setup</h5>
            </div>
            <asp:LinkButton ID="btnSave" runat="server" CssClass="btn-update" OnClick="btnSave_Click">
                <i class="fas fa-save me-2"></i>Apply Changes
            </asp:LinkButton>
        </div>

        <!-- Fixed Search Area -->
        <div class="search-area">
            <div class="input-group input-group-sm" style="max-width: 400px;">
                <span class="input-group-text bg-transparent border-end-0"><i class="fas fa-search text-muted"></i></span>
                <input type="text" id="txtFilter" class="form-control border-start-0" placeholder="Find a module..." />
            </div>
        </div>

        <!-- Multi-Column Scrollable Area -->
        <div class="content-scroll">
            <asp:Repeater ID="rptSettings" runat="server">
                <ItemTemplate>
                    <div class="setting-item">
                        <div class="item-info">
                            <span class="item-name"><%# Eval("Form_Name") %></span>
                            <span class="item-id"><%# Eval("Control_ID") %></span>
                            <asp:HiddenField ID="hfSno" runat="server" Value='<%# Eval("Setting_Sno") %>' />
                        </div>
                        <label class="switch">
                            <asp:CheckBox ID="chk" runat="server" Checked='<%# Eval("Is_Enabled") %>' />
                            <span class="slider"></span>
                        </label>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- Toast -->
        <div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3">
            <div id="msgToast" class="toast align-items-center text-white border-0" role="alert">
                <div class="d-flex">
                    <div class="toast-body"><span id="msgText"></span></div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
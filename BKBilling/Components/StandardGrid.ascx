<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StandardGrid.ascx.cs" Inherits="BKBilling.Components.StandardGrid" %>

<style>
    /* PRO ERP DESIGN SYSTEM */
    .pro-grid-box { border: 1px solid #dee2e6; border-radius: 4px; background: #fff; font-family: 'Segoe UI', system-ui, sans-serif; box-shadow: 0 1px 2px rgba(0,0,0,0.05); }
    
    /* TOOLBAR */
    .pro-toolbar { display: flex; justify-content: space-between; align-items: center; padding: 10px 15px; border-bottom: 1px solid #dee2e6; background: #fff; }
    .pro-search-group { position: relative; width: 280px; }
    .pro-search-group i { position: absolute; left: 12px; top: 10px; color: #adb5bd; font-size: 13px; }
    .pro-search-group .form-control { padding-left: 35px; height: 34px; border-radius: 17px; font-size: 13px; border: 1px solid #ced4da; }

    .pro-date-group { display: flex; align-items: center; gap: 10px; background: #f8f9fa; padding: 4px 15px; border-radius: 20px; border: 1px solid #e2e8f0; }
    .pro-date-group label { font-size: 10px; font-weight: 700; color: #6c757d; text-transform: uppercase; margin: 0; }
    .date-input { border: none; font-size: 12px; color: #333; outline: none; background: transparent; }

    /* GRID CORE */
    .pro-table-container { overflow: auto; min-height: 400px; position: relative; }
    .gv-pro-style { width: 100%; border-collapse: collapse; table-layout: fixed; border: none; }
    .gv-pro-style th { 
        background: #f8f9fa !important; color: #495057; font-size: 12px; font-weight: 600;
        padding: 12px; border: 1px solid #dee2e6; position: sticky; top: 0; z-index: 10;
    }
    .gv-pro-style td { padding: 10px 12px; font-size: 13px; border: 1px solid #eee; color: #212529; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
    .gv-pro-style tr:hover td { background: #f1f7ff; }

    /* FLYOUT FILTER UI */
    .header-flex { display: flex; justify-content: space-between; align-items: center; width: 100%; }
    .menu-dots { cursor: pointer; color: #adb5bd; padding: 0 4px; }
    .menu-dots:hover { color: #0d6efd; }

    .pro-flyout-menu, .pro-filter-pop {
        display: none; position: absolute; background: #fff; border: 1px solid #ccc;
        box-shadow: 0 4px 12px rgba(0,0,0,0.15); z-index: 1000; min-width: 180px;
        border-radius: 4px; padding: 5px 0;
    }
    .pro-filter-pop { width: 240px; padding: 15px; z-index: 1100; border: 1px solid #0d6efd; }
    .menu-item { padding: 8px 15px; font-size: 13px; color: #333; display: flex; align-items: center; gap: 10px; cursor: pointer; text-decoration: none !important; }
    .menu-item:hover { background: #0d6efd; color: #fff; }
    
    .filter-pop-label { font-size: 11px; font-weight: 600; color: #6c757d; margin-bottom: 6px; display: block; }
    .filter-pop-input { font-size: 12px; margin-bottom: 10px; height: 30px; border-radius: 4px; }

    /* BUTTONS */
    .btn-icon-circle { width: 32px; height: 32px; border-radius: 50%; border: 1px solid #dee2e6; background: #fff; display: inline-flex; align-items: center; justify-content: center; color: #6c757d; transition: 0.2s; cursor: pointer; }
    .btn-icon-circle:hover { background: #f8f9fa; color: #0d6efd; border-color: #0d6efd; }

    /* FOOTER */
    .pro-footer { padding: 10px 15px; background: #f8f9fa; border-top: 1px solid #dee2e6; display: flex; justify-content: space-between; align-items: center; font-size: 12px; }
</style>

<script>
    function toggleProMenu(e, id) {
        e.stopPropagation();
        const el = document.getElementById(id);
        const isOpen = el.style.display === 'block';
        document.querySelectorAll('.pro-flyout-menu, .pro-filter-pop').forEach(m => m.style.display = 'none');
        if (!isOpen) el.style.display = 'block';
    }
    function stopPopClose(e) { e.stopPropagation(); }
    document.addEventListener('click', () => document.querySelectorAll('.pro-flyout-menu, .pro-filter-pop').forEach(m => m.style.display = 'none'));
</script>

<div class="pro-grid-box">
    <!-- Toolbar -->
    <div class="pro-toolbar">
        <div class="d-flex gap-3 align-items-center">
            <div class="pro-search-group">
                <i class="fas fa-search"></i>
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search values..." AutoPostBack="true" OnTextChanged="Refresh_Click"></asp:TextBox>
            </div>
            <div class="pro-date-group">
                <label>From:</label>
                <asp:TextBox ID="txtFrom" runat="server" TextMode="Date" CssClass="date-input" AutoPostBack="true" OnTextChanged="Refresh_Click"></asp:TextBox>
                <label class="ms-2">To:</label>
                <asp:TextBox ID="txtTo" runat="server" TextMode="Date" CssClass="date-input" AutoPostBack="true" OnTextChanged="Refresh_Click"></asp:TextBox>
            </div>
        </div>
        <div class="d-flex gap-2">
            <asp:LinkButton ID="btnSync" runat="server" CssClass="btn-icon-circle" OnClick="Refresh_Click" ToolTip="Refresh Data"><i class="fas fa-sync-alt"></i></asp:LinkButton>
            <asp:LinkButton ID="btnXls" runat="server" CssClass="btn-icon-circle" OnClick="Export_Click" ToolTip="Export to Excel"><i class="fas fa-file-excel text-success"></i></asp:LinkButton>
        </div>
    </div>

    <!-- Table -->
    <div class="pro-table-container">
        <asp:GridView ID="gvInternal" runat="server" AutoGenerateColumns="false" CssClass="gv-pro-style" 
            ShowHeaderWhenEmpty="true" OnRowCommand="gvInternal_RowCommand" OnSorting="gvInternal_Sorting" AllowSorting="true">
        </asp:GridView>
    </div>

    <!-- Footer -->
    <div class="pro-footer">
        <div class="text-muted">Total: <asp:Literal ID="litTotal" runat="server" /> | Visible: <asp:Literal ID="litVisible" runat="server" /></div>
        <div class="d-flex gap-2 align-items-center">
            <asp:LinkButton ID="btnPrev" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="Prev_Click"><i class="fas fa-chevron-left"></i></asp:LinkButton>
            <asp:LinkButton ID="btnNext" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="Next_Click"><i class="fas fa-chevron-right"></i></asp:LinkButton>
            <asp:DropDownList ID="ddlSize" runat="server" CssClass="form-select form-select-sm ms-2" Width="90px" AutoPostBack="true" OnSelectedIndexChanged="Refresh_Click">
                <asp:ListItem Text="25" Value="25" Selected="True" />
                <asp:ListItem Text="50" Value="50" />
                <asp:ListItem Text="100" Value="100" />
            </asp:DropDownList>
        </div>
    </div>
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StandardGrid.ascx.cs" Inherits="BKBilling.Components.StandardGrid" %>

<style>
    /* COMPONENT CONTAINER */
    .erp-grid-main {
        border: 1px solid #d1dbe5; border-radius: 8px; display: flex;
        flex-direction: column; flex-grow: 1; background: #fff; overflow: hidden;
        box-shadow: 0 4px 15px rgba(0,0,0,0.05);
    }

    /* 1. TOOLBAR DESIGN */
    .erp-top-toolbar {
        display: flex; justify-content: space-between; align-items: center;
        padding: 14px 20px; border-bottom: 1px solid #e2e8f0; background: #fff; flex-shrink: 0;
    }
    .std-search-group { max-width: 350px; width: 100%; position: relative; }
    .std-search-group i { position: absolute; left: 12px; top: 10px; color: #94a3b8; z-index: 5; }
    .std-search-group .form-control { padding-left: 35px; border-radius: 6px; border: 1px solid #cbd5e1; font-size: 13px; }

    .btn-action-round {
        width: 32px; height: 32px; border-radius: 50%; border: 1px solid #e2e8f0;
        background: #fff; color: #64748b; display: inline-flex; align-items: center;
        justify-content: center; cursor: pointer; text-decoration: none; transition: 0.2s;
    }
    .btn-action-round:hover { background: #f1f5f9; color: #6366f1; }
    .btn-new-erp { background: #2563eb; color: #fff !important; padding: 7px 18px; border-radius: 6px; text-decoration: none !important; font-weight: 600; font-size: 13px; border: none; }

    /* 2. TABLE & SCROLLBAR */
    .table-scroll-area { flex-grow: 1; overflow: auto; position: relative; }
    .table-scroll-area::-webkit-scrollbar { width: 8px; height: 8px; }
    .table-scroll-area::-webkit-scrollbar-thumb { background: #cbd5e1; border-radius: 10px; }

    .gv-std { width: 100%; border-collapse: collapse; font-size: 12.8px; }
    
    /* STICKY HEADER */
    .gv-std th {
        background: #e9eff8 !important; color: #475569; padding: 10px 15px;
        border: 1px solid #d1dbe5; text-transform: uppercase; font-size: 11px;
        font-weight: 700; position: sticky; top: 0; z-index: 20;
    }

    /* HEADER CONTEXT MENU UI */
    .header-wrapper { display: flex; align-items: center; justify-content: space-between; position: relative; }
    .btn-header-menu { cursor: pointer; padding: 2px 6px; border-radius: 4px; color: #94a3b8; }
    .btn-header-menu:hover { background: #d1dbe5; color: #334155; }

    .column-context-menu {
        display: none; position: absolute; top: 100%; right: 0; z-index: 1000;
        min-width: 180px; background: #fff; border: 1px solid #d1dbe5;
        border-radius: 8px; box-shadow: 0 10px 25px rgba(0,0,0,0.1); padding: 5px 0;
    }
    .menu-open .column-context-menu { display: block; }
    .menu-item {
        padding: 8px 15px; font-size: 12px; color: #334155; display: flex;
        align-items: center; gap: 10px; cursor: pointer; text-decoration: none !important;
    }
    .menu-item:hover { background-color: #f1f5f9; color: #2563eb; }
    .menu-item i { width: 14px; font-size: 13px; color: #64748b; }
    .menu-divider { height: 1px; background: #f1f5f9; margin: 5px 0; }

    /* FILTERS */
    .header-filter {
        display: block; width: 100%; margin-top: 5px; padding: 4px 8px;
        font-size: 11px; border: 1px solid #d1dbe5; border-radius: 4px;
    }
    .hide-col-filters .header-filter { display: none; }

    /* 3. FOOTER */
    .erp-grid-footer {
        padding: 10px 20px; background: #f8fafc; border-top: 1px solid #d1dbe5;
        display: flex; justify-content: space-between; align-items: center;
        font-size: 12px; color: #64748b; flex-shrink: 0;
    }
    filter-row { display: flex; gap: 2px; margin-top: 5px; }
    .ddl-operator { width: 40px !important; font-size: 10px !important; padding: 0 !important; background: #f1f5f9; border: 1px solid #cbd5e1; border-radius: 4px 0 0 4px; }
    .header-filter { flex-grow: 1; border-radius: 0 4px 4px 0 !important; }

    /* Column Picker Styles */
    .column-picker-panel {
        display: none; position: absolute; right: 20px; top: 60px; z-index: 1001;
        background: #fff; border: 1px solid #d1dbe5; border-radius: 8px; 
        box-shadow: 0 10px 25px rgba(0,0,0,0.2); padding: 10px; min-width: 200px;
    }
    .column-picker-item { display: flex; align-items: center; gap: 8px; padding: 4px 0; font-size: 12px; }
</style>

<script>
    function toggleHeaderMenu(e, btn) {
        e.stopPropagation();
        const wrapper = btn.closest('.header-wrapper');
        document.querySelectorAll('.header-wrapper').forEach(el => {
            if (el !== wrapper) el.classList.remove('menu-open');
        });
        wrapper.classList.toggle('menu-open');
    }
    document.addEventListener('click', () => document.querySelectorAll('.header-wrapper').forEach(el => el.classList.remove('menu-open')));
</script>
<div class="erp-grid-main">
    <div class="erp-top-toolbar">
        <div class="std-search-group">
            <i class="fas fa-search"></i>
            <asp:TextBox ID="txtInternalSearch" runat="server" placeholder="Global search..." AutoPostBack="true" OnTextChanged="btnAction_Click" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="d-flex gap-2">
            <asp:LinkButton ID="btnRefresh" runat="server" CssClass="btn-action-round" OnClick="btnAction_Click" ToolTip="Refresh"><i class="fas fa-sync-alt"></i></asp:LinkButton>
            <asp:LinkButton ID="btnExport" runat="server" CssClass="btn-action-round text-success" OnClick="btnExport_Click" ToolTip="Export Excel"><i class="fas fa-file-excel"></i></asp:LinkButton>
            
            <!-- Column Visibility Toggle -->
            <span class="btn-action-round text-secondary" title="Columns" onclick="$('.column-picker-panel').toggle();"><i class="fas fa-columns"></i></span>
            
            <span class="btn-action-round text-primary" title="Toggle Filters" onclick="$('.erp-grid-main').toggleClass('hide-col-filters');"><i class="fas fa-filter"></i></span>
            
            <asp:LinkButton ID="btnAddNew" runat="server" CssClass="btn-new-erp ms-2" OnClick="Add_Triggered" CausesValidation="false">
                <i class="fas fa-plus me-2"></i><asp:Literal ID="litBtnText" runat="server" Text="New" />
            </asp:LinkButton>
        </div>
    </div>

    <!-- Column Picker Dropdown -->
    <div class="column-picker-panel">
        <h6 class="fw-bold border-bottom pb-2 mb-2" style="font-size: 11px;">Show/Hide Columns</h6>
        <asp:CheckBoxList ID="cblColumns" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cblColumns_SelectedIndexChanged" CssClass="column-picker-list"></asp:CheckBoxList>
    </div>

    <div class="table-scroll-area">
        <asp:GridView ID="gvInternal" runat="server" AutoGenerateColumns="false" CssClass="gv-std" GridLines="None"
            ShowHeaderWhenEmpty="true" AllowPaging="true" OnRowCommand="gvInternal_RowCommand" 
            AllowSorting="true" OnSorting="gvInternal_Sorting">
            <PagerSettings Visible="false" />
        </asp:GridView>
    </div>

    <div class="erp-grid-footer">
        <div>Total: <asp:Literal ID="litCount" runat="server" Text="0" /> Records</div>
        <div class="d-flex align-items-center gap-3">
            <div class="d-flex gap-1">
                <asp:LinkButton ID="btnPrev" runat="server" CssClass="btn btn-sm btn-light border" OnClick="btnPrev_Click" CausesValidation="false"><i class="fas fa-chevron-left"></i></asp:LinkButton>
                <asp:LinkButton ID="btnNext" runat="server" CssClass="btn btn-sm btn-light border" OnClick="btnNext_Click" CausesValidation="false"><i class="fas fa-chevron-right"></i></asp:LinkButton>
            </div>
            <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="form-select form-select-sm" style="width:110px;" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                <asp:ListItem Text="10 Rows" Value="10" />
                <asp:ListItem Text="50 Rows" Value="50" Selected="True" />
                <asp:ListItem Text="100 Rows" Value="100" />
            </asp:DropDownList>
        </div>
    </div>
</div>
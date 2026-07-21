using System;
using System.Collections.Generic;

namespace BKBilling.Components
{
    // MUST be Serializable to stay in ViewState
    [Serializable]
    public class GridColumnDef
    {
        public string FieldKey { get; set; }
        public string HeaderText { get; set; }
        public string Format { get; set; }
        public string Width { get; set; }
        public bool Bold { get; set; }
        public string CssClassFromField { get; set; }
        public string CssClassPrefix { get; set; } = "";
        public bool Visible { get; set; } = true;
    }

    // Already marked, but keep it here
    [Serializable]
    public class ColumnFilterState
    {
        public string Value { get; set; }
        public string Operator { get; set; } = "LIKE";
    }

    [Serializable]
    public class GridActionDef
    {
        public string Key { get; set; }
        public string Icon { get; set; }
        public string Label { get; set; }
        public string CssClass { get; set; }
        public string Tooltip { get; set; }
    }

    public class GridRowActionEventArgs : EventArgs
    {
        public string ActionKey { get; set; }
        public string RowKey { get; set; }
    }
}
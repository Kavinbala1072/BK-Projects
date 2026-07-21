using System;
using System.Collections.Generic;

namespace BKBilling.Components
{
    // MUST be serializable for ViewState
    [Serializable]
    public class GridColumnDef
    {
        public string FieldKey { get; set; }
        public string HeaderText { get; set; }
        public string Format { get; set; }
        public string Width { get; set; }
        public bool Bold { get; set; }
        public bool Visible { get; set; } = true;
        public string Align { get; set; } // NEW optional: "Left"|"Right"|"Center"
    }

    [Serializable]
    public class FilterCriteria
    {
        public string Operator { get; set; } = "Contains";
        public string Value { get; set; }
    }

    [Serializable]
    public class GridActionDef
    {
        public string Key { get; set; }
        public string Icon { get; set; }
        public string Tooltip { get; set; }
    }

    public class GridRowActionEventArgs : EventArgs
    {
        public string ActionKey { get; set; }
        public string RowKey { get; set; }
    }
}

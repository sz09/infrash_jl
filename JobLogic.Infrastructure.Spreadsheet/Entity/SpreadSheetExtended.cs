using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JobLogic.Infrastructure.Spreadsheet
{
    public class SpreadSheetExtended
    {
        public bool Delete { get; set; }
        public int Index { get { return RowIndex; } }

        internal int RowIndex { get; set; }
        internal string StartAddress { get; set; }
        internal string EndAddress { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MergeSorter
{
    class SortConfig
    {
        public char SplitChar { get; set; } = '|';
        public List<SortConfigField> Fields {  get; set; } = new List<SortConfigField>();
    }

    class SortConfigField
    {
        public bool IsAscSort { get; set; } = false;
        public int Index { get; set; }
        public ColumnType Type { get; set; }
    }
}

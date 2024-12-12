using System;
using System.Linq;

namespace MergeSorter
{
    class Row : IComparable<Row>
    {
        private readonly string _rowData;
        private readonly SortConfig _config;

        private readonly IComparable[] _sortKeys;

        public Row(string rowData, SortConfig config)
        {
            _rowData = rowData;
            _config = config;
            _sortKeys = new IComparable[_config.Fields.Count];

            fillSortKeys();
        }

        public string RowData => _rowData;

        private void fillSortKeys()
        {
            var parts = _rowData.Split(_config.SplitChar).Select(x=>x.Trim('"')).ToArray();
            for (int i = 0; i < _config.Fields.Count; i++)
            {
                var field = _config.Fields[i];
                switch (field.Type)
                {
                    case ColumnType.Default:
                    case ColumnType.String:
                        _sortKeys[i] = parts[field.Index];
                        break;
                    case ColumnType.Int:
                        _sortKeys[i] = Int32.Parse(parts[field.Index]);
                        break;
                    case ColumnType.Long:
                        _sortKeys[i] = Int64.Parse(parts[field.Index]);
                        break;
                    case ColumnType.Float:
                        _sortKeys[i] = Single.Parse(parts[field.Index]);
                        break;
                    case ColumnType.Double:
                        _sortKeys[i] = Double.Parse(parts[field.Index]);
                        break;
                    case ColumnType.Decimal:
                        _sortKeys[i] = Decimal.Parse(parts[field.Index]);
                        break;
                }
            }
        }

        public int CompareTo(Row other)
        {
            if (other == null) return 1;
            if (other == this) return 0;

            for (var i=0; i < _sortKeys.Length; i++)
            {
                var key = _sortKeys[i];
                var otherKey = other._sortKeys[i];
                var compare = key.CompareTo(otherKey);
                if (compare != 0)
                {
                    if (!_config.Fields[i].IsAscSort)
                    {
                        // изменение направления сравнения
                        return (compare > 0 ? -1 : 1);
                    }
                    else
                    {
                        return compare;
                    }
                }
            }

            return 0;
        }
    }
}

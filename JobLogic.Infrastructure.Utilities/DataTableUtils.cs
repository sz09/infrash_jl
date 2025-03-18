using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace JobLogic.Infrastructure.Utilities
{
    public static class DataTableUtils
    {
        public static IEnumerable<DataColumn> GetColumns<T>(this DataTable dataTable)
        {
            var dateColumns = dataTable.Columns.OfType<DataColumn>().ToList().Where(m => m.DataType == typeof(T));
            return dateColumns;
        }

        public static void Convert<T>(this DataColumn column, Func<object, T> conversion)
        {
            foreach (DataRow row in column.Table.Rows)
            {
                if (!(row[column.ColumnName] == DBNull.Value))
                    row[column] = conversion(row[column]);
            }
        }

    }
}

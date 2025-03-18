using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace JobLogic.Infrastructure.Spreadsheet
{
    public static class Extensions
    {
        static Func<double, DateTime> convertDateTime = new Func<double, DateTime>(excelDate =>
        {
            if (excelDate < 1)
                throw new ArgumentException("Excel dates cannot be smaller than 0.");

            var dateOfReference = new DateTime(1900, 1, 1);

            if (excelDate > 60d)
                excelDate = excelDate - 2;
            else
                excelDate = excelDate - 1;
            return dateOfReference.AddDays(excelDate);
        });

        public static IList<T> ToList<T>(this ExcelWorksheet worksheet, out Dictionary<string, int> columnsDictionary, Dictionary<string, string> map = null, ExcelCellAddress startAddress = null, ExcelCellAddress endAddress = null) where T : SpreadSheetExtended, new()
        {
            return ToWorksheetList<T>(worksheet, out columnsDictionary, map, startAddress, endAddress);
        }
        public static IList<T> ToList<T>(this ExcelWorksheet worksheet, Dictionary<string, string> map = null, ExcelCellAddress startAddress = null, ExcelCellAddress endAddress = null) where T : SpreadSheetExtended, new()
        {
            var columnsDictionary = new Dictionary<string, int>();
            return ToWorksheetList<T>(worksheet, out columnsDictionary, map, startAddress, endAddress);
        }

        static IList<T> ToWorksheetList<T>(ExcelWorksheet worksheet, out Dictionary<string, int> columnsDictionary, Dictionary<string, string> map, ExcelCellAddress startAddress, ExcelCellAddress endAddress) where T : SpreadSheetExtended, new()
        {
            var props = typeof(T).GetProperties()
                .Select(prop =>
                {
                    var displayAttribute = (DisplayAttribute)prop.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
                    return new
                    {
                        Name = prop.Name,
                        ColumnName = displayAttribute == null ? prop.Name : displayAttribute.Name,
                        Order = displayAttribute == null || !displayAttribute.GetOrder().HasValue ? 999 : displayAttribute.Order,
                        PropertyInfo = prop,
                        PropertyType = prop.PropertyType,
                        HasDisplayName = displayAttribute != null
                    };
                })
            .Where(prop => !string.IsNullOrWhiteSpace(prop.ColumnName))
            .ToList();

            var retList = new List<T>();
            var columns = new List<SpreadSheetMap>();
            columnsDictionary = new Dictionary<string, int>();

            int startCol, startRow, endCol, endRow;
            ExtractDimension(worksheet, startAddress, endAddress, out startCol, out startRow, out endCol, out endRow);

            // Assume first row has column names
            for (int col = startCol; col <= endCol; col++)
            {
                var cellValue = (worksheet.Cells[startRow, col].Value ?? string.Empty).ToString().Trim();
                if (!string.IsNullOrWhiteSpace(cellValue))
                {
                    columns.Add(new SpreadSheetMap()
                    {
                        Name = cellValue,
                        MappedTo = map == null || map.Count == 0 ?
                            ColumnIndexToColumnLetter(col) :
                            map.ContainsKey(cellValue) ? map[cellValue] : string.Empty,
                        Index = col
                    });
                }
            }

            // Now iterate over all the rows
            for (int rowIndex = startRow + 1; rowIndex <= endRow; rowIndex++)
            {
                var item = new T();
                columns.ForEach(column =>
                {
                    var value = worksheet.Cells[rowIndex, column.Index].Value;
                    var prop = string.IsNullOrWhiteSpace(column.MappedTo) ?
                        null :
                        props.FirstOrDefault(p => p.ColumnName.Contains(column.MappedTo));

                    // Excel stores all numbers as doubles, but we're relying on the object's property types
                    if (prop != null)
                    {
                        var propertyType = prop.PropertyType;
                        var parsedValue = ExtractParsedValue(value, propertyType);
                        {
                            prop.PropertyInfo.SetValue(item, parsedValue);
                        }
                    }
                });

                item.RowIndex = rowIndex;
                item.StartAddress = startAddress.Address;
                item.EndAddress = endAddress.Address;
                retList.Add(item);
            }

            return retList;
        }

        public static void ModifyWorksheet<T>(this ExcelWorksheet worksheet, IList<T> items, Dictionary<string, int> columnsDictionary) where T : SpreadSheetExtended
        {
            foreach (var item in items)
            {           
                foreach (PropertyInfo propertyInfo in item.GetType().GetProperties())
                {
                    if (columnsDictionary.ContainsKey(propertyInfo.Name))
                    {
                        if (propertyInfo.CanRead && propertyInfo.CanWrite)
                        {
                            var columnIndex = columnsDictionary[propertyInfo.Name];
                            var rowIndex = item.RowIndex;
                            worksheet.Cells[rowIndex, columnIndex].Value = propertyInfo.GetValue(item, null);
                        }
                    }
                }
            }            
        }

        static void ExtractDimension(ExcelWorksheet worksheet, ExcelCellAddress startAddress, ExcelCellAddress endAddress, out int startCol, out int startRow, out int endCol, out int endRow)
        {
            var start = startAddress ?? worksheet.Dimension.Start;
            var end = endAddress ?? worksheet.Dimension.End;
            startCol = start.Column;
            startRow = start.Row;
            endCol = end.Column;
            endRow = worksheet.Dimension.End.Row;
        }
        public static object ExtractParsedValue(object value, Type propertyType)
        {
            object parsedValue = null;
            var valueStr = value == null ? string.Empty : value.ToString().Trim();

            if (propertyType == typeof(int?) || propertyType == typeof(int))
            {
                int val;
                if (!int.TryParse(valueStr, out val))
                {
                    val = default(int);
                }

                parsedValue = val;
            }
            else if (propertyType == typeof(short?) || propertyType == typeof(short))
            {
                short val;
                if (!short.TryParse(valueStr, out val))
                    val = default(short);
                parsedValue = val;
            }
            else if (propertyType == typeof(long?) || propertyType == typeof(long))
            {
                long val;
                if (!long.TryParse(valueStr, out val))
                    val = default(long);
                parsedValue = val;
            }
            else if (propertyType == typeof(decimal?) || propertyType == typeof(decimal))
            {
                decimal val;
                if (!decimal.TryParse(valueStr, out val))
                    val = default(decimal);
                parsedValue = val;
            }
            else if (propertyType == typeof(double?) || propertyType == typeof(double))
            {
                double val;
                if (!double.TryParse(valueStr, out val))
                    val = default(double);
                parsedValue = val;
            }
            else if (propertyType == typeof(DateTime?) || propertyType == typeof(DateTime))
            {
                parsedValue = convertDateTime((double)value);
            }
            else if (propertyType.IsEnum)
            {
                try
                {
                    parsedValue = Enum.ToObject(propertyType, int.Parse(valueStr));
                }
                catch (Exception ex)
                {
                    parsedValue = Enum.ToObject(propertyType, 0);
                }
            }
            else if (propertyType == typeof(string))
            {
                parsedValue = valueStr;
            }
            else
            {
                try
                {
                    parsedValue = Convert.ChangeType(value, propertyType);
                }
                catch (Exception ex)
                {
                    parsedValue = valueStr;
                }
            }

            return parsedValue;
        }
        static string ColumnIndexToColumnLetter(int colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            int mod = 0;

            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = ((div - mod) / 26);
            }
            return colLetter;
        }
    }
}

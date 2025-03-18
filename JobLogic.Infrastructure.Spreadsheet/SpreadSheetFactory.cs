using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace JobLogic.Infrastructure.Spreadsheet
{
    public class SpreadSheetFactory : ISpreadSheetFactory
    {
        public SpreadSheetAction<T> ProcessSpreadSheetFile<T>(Stream spreadsheetFile, string password = null, int workSheetPage = 1, string startAddress = "A1", string endAddress = "Z1") where T : SpreadSheetExtended, new()
        {
            if (spreadsheetFile == null) throw new Exception("File Not Found");

            return new SpreadSheetAction<T>(spreadsheetFile, password, workSheetPage, startAddress, endAddress);
        }
    }

    public class SpreadSheetAction<T>: IDisposable where T : SpreadSheetExtended, new()
    {
        Stream _originalExcelFile;
        ExcelPackage _package;
        ExcelWorksheet _worksheet;
        readonly Dictionary<string, int> _columnKeyPair;
        readonly string _startAddress;
        readonly string _endAddress;

        internal SpreadSheetAction(Stream originalFile, string password, int workSheetPage, string startAddress, string endAddress)
        {
            _originalExcelFile = originalFile;
            _startAddress = startAddress;
            _endAddress = endAddress;

            _package = string.IsNullOrWhiteSpace(password) ? new ExcelPackage(originalFile) : new ExcelPackage(originalFile, password);
            _worksheet = _package.Workbook.Worksheets[workSheetPage];
            CurrentSheet = _worksheet.ToList<T>(out _columnKeyPair, startAddress: new ExcelCellAddress(startAddress), endAddress: new ExcelCellAddress(endAddress));
        }

        public IList<T> CurrentSheet { get; set; }

        public void AddCellValue(object value, int row, int coloumn) 
        {
            if (_package == null || _originalExcelFile == null) throw new Exception();
            _worksheet.Cells[row, coloumn].Value = value;
        }

        public void AddHeader(string coloumnAddress, string headerName, out int row, out int column, Color fontColour = default(Color), Color backgroundColour = default(Color), bool border = false)
        {
            if (_package == null || _originalExcelFile == null) throw new Exception();
            _worksheet.Cells[coloumnAddress].Value = headerName;
            if (border)
            {
                _worksheet.Cells[coloumnAddress].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            if (!fontColour.IsEmpty)
            {
                _worksheet.Cells[coloumnAddress].Style.Font.Bold = true;
                _worksheet.Cells[coloumnAddress].Style.Fill.PatternType = ExcelFillStyle.Solid;
                _worksheet.Cells[coloumnAddress].Style.Font.Color.SetColor(fontColour);
            }
            if (!backgroundColour.IsEmpty)
            {
                _worksheet.Cells[coloumnAddress].Style.Fill.PatternType = ExcelFillStyle.Solid;
                _worksheet.Cells[coloumnAddress].Style.Fill.BackgroundColor.SetColor(backgroundColour);
            }
            row = _worksheet.Cells[coloumnAddress].Start.Row;
            column = _worksheet.Cells[coloumnAddress].Start.Column;
        }

        public MemoryStream DownloadModifiedFile()
        {
            var data = _package.GetAsByteArray();
            return new MemoryStream(data);
        }

        public Stream DownloadOriginalFile() => _originalExcelFile;

        public void ModifyExisitingFile<Ts>(bool modifyAllCell = false, bool deleteFlaggedRow = false) where Ts : SpreadSheetExtended, new()
        {
            if (!modifyAllCell && !deleteFlaggedRow) return;
            
            if (_package == null || _originalExcelFile == null) throw new Exception();

            if (modifyAllCell || deleteFlaggedRow)
            {
                if (modifyAllCell)
                {
                    _worksheet.ModifyWorksheet(CurrentSheet, _columnKeyPair);
                }
                if (deleteFlaggedRow)
                {
                    var countDeleted = 0;
                    foreach (var item in CurrentSheet.Where(m => m.Delete))
                    {
                        _worksheet.DeleteRow(item.RowIndex - countDeleted, 1, true);
                        countDeleted++;
                    }
                    //Re - map due to shifted items in row index
                    if (CurrentSheet.Any(m => m.Delete))
                    {
                        CurrentSheet = (IList<T>)_worksheet.ToList<Ts>(startAddress: new ExcelCellAddress(_startAddress), endAddress: new ExcelCellAddress(_endAddress), map: null);
                    }
                }
            }
        }

        public void Dispose()
        {
            _package.Dispose();
            _originalExcelFile.Dispose();
            _worksheet.Dispose();
        }
    }
}

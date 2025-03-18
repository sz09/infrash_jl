using System.IO;

namespace JobLogic.Infrastructure.Spreadsheet
{
    public interface ISpreadSheetFactory
    {
         SpreadSheetAction<T> ProcessSpreadSheetFile<T>(Stream spreadsheetFile, string password = null, int workSheetPage = 1, string startAddress = "A1", string endAddress = "Z1") where T : SpreadSheetExtended, new();
    }
}
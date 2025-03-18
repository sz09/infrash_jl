using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobLogic.Infrastructure.Utilities
{
    public enum ExportType
    {
        Customer,
        Site,
        Job,
        Quote,
        Invoice,
        Part,
        Staff,
        PortalUser,
        Library,
        PPMContract,
        PPMInvoice,
        RefcomLogbook,
        RefcomLeakCheck,
        Asset,
        AssetLogbook,
        PurchaseOrder,
        Priority,
        Report,
        NonProductiveTime, 
        Equipment,
        Stock,
        PortalUserRole,
        PPMQuote,
        StockValuation,
        StockAdjustment,
        CFTSCertificatesIssuedReport,
        PPMPartsRequired, 
        GoodsReceivedNotes,
        NonJobExpenses
    }

    public interface ICSVExportLine
    {
        string ExportString { get; }
    }

    public class ExportReponse
    {
        public byte[] FileContents { get; set; }
        public string ContentType { get; set; }
        public string FileDownloadName { get; set; }
    }

    public static class ExportHelper
    {
        public static ExportReponse ExportData(ExportType exportType, string csv)
        {
            var filename = "JL" + exportType.ToString() + "Export_" + DateTime.UtcNow.ToString("ddMMyy-HHmm") + ".csv";
            var data = Encoding.ASCII.GetBytes(csv);
            return new ExportReponse() { FileContents = data, FileDownloadName = filename, ContentType = "application/octet-stream" };
        }

        public static string HeadersAsCSV(string headers, IEnumerable<ICSVExportLine> data)
        {
            var csv = headers + Environment.NewLine + string.Join(Environment.NewLine, data.Select(x => x.ExportString).ToArray());
            return csv;
        }

        public static string HeadersAsCSV(string headers, IEnumerable<IEnumerable<string>> rows)
        {
            var csv = new StringBuilder(headers + Environment.NewLine);
            foreach (var row in rows)
            {
                csv.Append(StringUtils.AsDelimiteredStringForCSV(row.ToArray()) + Environment.NewLine);
            }
            return csv.ToString();
        }
    }
}
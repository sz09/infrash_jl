using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using JobLogic.Infrastructure.Spreadsheet;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Unit.Joblogic.Spreadsheet
{
    [TestCategory("Unit")]
    [TestClass]
    public class SpreadSheetFactoryTest
    {
        [TestMethod]
        public void ProcessSpreadSheet()
        {
            using (var sreamReader = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}\\Remittance.xlsx"))
            {
                using (var destination = new MemoryStream())
                {
                    sreamReader.BaseStream.CopyTo(destination);
                    ISpreadSheetFactory spreadSheetFactory = new SpreadSheetFactory();
                    var remittance = spreadSheetFactory.ProcessSpreadSheetFile<RemittanceTest>(destination, "123456", 1, "A4", "V4");
                }

                //test.AddCellValue("asdasd", 1, 3);

                //using (FileStream fs = new FileStream("outpssssut.xlsx", FileMode.OpenOrCreate))
                //{
                //    var hhh = test.DownloadModifiedFile();
                //    hhh.Position = 0;
                //    hhh.CopyTo(fs);
                //    fs.Flush();
                //}
            }
        }

        [TestMethod]
        public void AddCellValue()
        {
            SpreadSheetAction<RemittanceTest> remittance = null;
            using (var sreamReader = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}\\Remittance.xlsx"))
            {
                using (var destination = new MemoryStream())
                {
                    sreamReader.BaseStream.CopyTo(destination);
                    ISpreadSheetFactory spreadSheetFactory = new SpreadSheetFactory();
                    remittance = spreadSheetFactory.ProcessSpreadSheetFile<RemittanceTest>(destination, "123456", 1, "A4", "V4");
                }
            }

            remittance.AddCellValue("Testing", 1, 3); //string
            remittance.AddCellValue(1, 1, 4); //int
            remittance.AddCellValue(2.3, 1, 5); //double
            remittance.AddCellValue('A', 1, 6); //char
            remittance.AddCellValue(true, 1, 7); //bool
            remittance.AddCellValue(DateTime.UtcNow, 1, 8); //date
        }

        [TestMethod]
        public void AddHeaderValue()
        {
            using (var sreamReader = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}\\Remittance.xlsx"))
            {
                using (var destination = new MemoryStream())
                {
                    sreamReader.BaseStream.CopyTo(destination);
                    ISpreadSheetFactory spreadSheetFactory = new SpreadSheetFactory();
                    var remittance = spreadSheetFactory.ProcessSpreadSheetFile<RemittanceTest>(destination, "123456", 1, "A4", "V4");
                    int row, column;
                    remittance.AddHeader("V4", "New Header sdfsdfdsf", out row, out column, System.Drawing.Color.Blue, System.Drawing.Color.Yellow, true);
                    remittance.AddCellValue("dd", 5, column);
                    remittance.AddCellValue("rizhwan ", 6, column);

                    using (var data = remittance.DownloadModifiedFile())
                    {
                        using (var fileStream = new FileStream("test1ss.xlsx", FileMode.Create, FileAccess.Write))
                        {
                            data.CopyTo(fileStream);
                        }
                    }

                }
            }
        }
    }

    class RemittanceTest : SpreadSheetExtended
    {
        [Display(Name = "R")]
        public string Account { get; set; }

        [Display(Name = "C")]
        public string Agreement { get; set; }

        [Display(Name = "E")]
        public string AgreementDate { get; set; }

        [Display(Name = "P")]
        public string BrokerRef { get; set; }

        [Display(Name = "I")]
        public int Cash { get; set; }

        [Display(Name = "B")]
        public string Client { get; set; }

        [Display(Name = "O")]
        public double ClientCode { get; set; }

        [Display(Name = "F")]
        public string Code { get; set; }

        [Display(Name = "Q")]
        public string CustomerAddress { get; set; }

        [Display(Name = "D")]
        public string CustomerName { get; set; }

        [Display(Name = "G")]
        public string TotalInstalments { get; set; }

        [Display(Name = "L")]
        public decimal Net { get; set; }

        [Display(Name = "M")]
        public decimal Net2 { get; set; }

        [Display(Name = "N")]
        public decimal Net3 { get; set; }

        [Display(Name = "H")]
        public int OfferMonths { get; set; }

        [Display(Name = "U")]
        public string PayType { get; set; }

        [Display(Name = "T")]
        public int PpsAmt { get; set; }

        [Display(Name = "S")]
        public int PpsInd { get; set; }

        [Display(Name = "J")]
        public int CommChargeRate { get; set; }

        [Display(Name = "A")]
        public int ShopCode { get; set; }

        [Display(Name = "K")]
        public int CommCharge { get; set; }
    }
}

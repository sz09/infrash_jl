using JobLogic.Infrastructure.UnitTest;
using JobLogic.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System; 

namespace Tests.Unit.Joblogic.Infrastructure.Utilites
{
    [TestClass]
    public class UKHolidayCalculatorTest: BaseUnitTest
    {
        [TestMethod]
        [DataRow("Jan 1 2015")]
        [DataRow("Jan 1 2016")]
        [DataRow("Jan 2 2017")]
        [DataRow("Jan 1 2018")]
        [DataRow("Jan 1 2019")]
        [DataRow("Jan 1 2020")]
        public void should_be_january_bank_holiday(string date)
        {
            // Arrange
            DateTime dateTime = DateTime.Parse(date); 

            // Act
            bool isBankHoliday = UKHolidayCalculator.IsJanuaryBankHolidayMonday(dateTime);

            // Assert
            Assert.IsTrue(isBankHoliday);
        }

        [TestMethod]
        public void should_not_be_january_bank_holiday()
        {
            // Arrange
            DateTime dateTime = DateTime.Parse("Jan 1 2017");
            
            // Act
            bool isBankHoliday = UKHolidayCalculator.IsMayBankHolidayMonday(dateTime);

            // Assert
            Assert.IsFalse(isBankHoliday);
        }

        [TestMethod]
        [DataRow("May 4 2015")]
        [DataRow("May 25 2015")]
        [DataRow("May 2 2016")]
        [DataRow("May 30 2016")]
        [DataRow("May 1 2017")]
        [DataRow("May 29 2017")]
        public void should_be_may_bank_holiday(string date)
        {
            // Arrange
            DateTime dateTime = DateTime.Parse(date);
            

            // Act
            bool isBankHoliday = UKHolidayCalculator.IsMayBankHolidayMonday(dateTime);

            // Assert
            Assert.IsTrue(isBankHoliday);
        }

        [TestMethod]
        public void should_not_be_may_bank_holiday()
        {
            // Arrange
            DateTime dateTime = DateTime.Parse("May 9 2017");
         
            // Act
            bool isBankHoliday = UKHolidayCalculator.IsMayBankHolidayMonday(dateTime);

            // Assert
            Assert.IsFalse(isBankHoliday);
        }

        [TestMethod]
        [DataRow("August 31 2015")]
        [DataRow("August 29 2016")]
        [DataRow("August 28 2017")]
        [DataRow("August 27 2018")]
        public void should_be_august_bank_holiday(string date)
        {
            // Arrange
            DateTime dateTime = DateTime.Parse(date);
            // Act
            bool isBankHoliday = UKHolidayCalculator.IsAugustBankHolidayMonday(dateTime);

            // Assert
            Assert.IsTrue(isBankHoliday);
        }

        [TestMethod]
        public void should_not_be_august_bank_holiday()
        {
            // Arrange
            DateTime dateTime = DateTime.Parse("August 14 2017");
           
            // Act
            bool isBankHoliday = UKHolidayCalculator.IsMayBankHolidayMonday(dateTime);

            // Assert
            Assert.IsFalse(isBankHoliday);
        }

        [TestMethod]
        [DataRow("6 April 2015")]
        [DataRow("28 March 2016")]
        [DataRow("17 April 2017")]
        [DataRow("2 April 2018")]
        [DataRow("22 April 2019")]
        [DataRow("13 April 2020")]
        public void should_be_easter_bank_holiday(string date)
        {
            // Arrange
            DateTime dateTime = DateTime.Parse(date);
               // Act
            bool isBankHoliday = UKHolidayCalculator.IsEasterBankHolidayMonday(dateTime);

            // Assert
            Assert.IsTrue(isBankHoliday);
        }

        [TestMethod]
        public void should_not_be_easter_bank_holiday()
        {
            // Arrange
            DateTime dateTime = DateTime.Parse("April 13 2015");
           
            // Act
            bool isBankHoliday = UKHolidayCalculator.IsMayBankHolidayMonday(dateTime);

            // Assert
            Assert.IsFalse(isBankHoliday);
        }

        [TestMethod]
        [DataRow("26 December 2014")]
        [DataRow("28 December 2015")]
        [DataRow("27 December 2016")]
        [DataRow("26 December 2017")]
        [DataRow("26 December 2018")]
        [DataRow("26 December 2019")]
        [DataRow("26 December 2019")]
        [DataRow("28 December 2020")]
        public void should_be_december_bank_holiday(string date)
        {
            // Arrange
            DateTime dateTime = DateTime.Parse(date);
            
            // Act
            bool isBankHoliday = UKHolidayCalculator.IsDecemberBankHoliday(dateTime);

            // Assert
            Assert.IsTrue(isBankHoliday);
        }

        [TestMethod]
        public void should_not_be_december_bank_holiday()
        {
            // Arrange
            DateTime dateTime = DateTime.Parse("29 December 2014");
           
            // Act
            bool isBankHoliday = UKHolidayCalculator.IsMayBankHolidayMonday(dateTime);

            // Assert
            Assert.IsFalse(isBankHoliday);
        }

        [TestMethod]
        [DataRow(2017)]
        [DataRow(2018)]
        [DataRow(2019)]
        [DataRow(2020)]
        [DataRow(2021)]
        public void should_be_8_BankHoliday_PerYear(int year)
        {
            // Arrange

            // Act
            var allbankHoliday = UKHolidayCalculator.GetAllBankHolidayInYear(year);

            // Assert
            Assert.AreEqual(8,allbankHoliday.Count);
        }
    }
}

using JobLogic.Infrastructure.Calculator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests.Unit.JobLogic.Infrastructure.Calculator
{
    [TestCategory("Unit")]
    [TestClass]
    public class LoanCalculatorTest
    {
        private ILoanCalculator _LoanCalculator;
        private const string outofRangeMessge = "Specified argument was out of the range of valid values. Parameter name: ";
        private const string paramsOrderAmountName = "orderAmount";
        private const string paramsAprName = "apr";
        private const string paramsNumberOfYearsName = "numberOfYears";
        private const string paramsDepositName = "deposit";

        public LoanCalculatorTest()
        {
            _LoanCalculator = new LoanCalculator();
        }

        #region fail case
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), outofRangeMessge + paramsOrderAmountName)]
        public void LoanCalculator_Is_OrderAmount_Negative_Failed() => _LoanCalculator.CalculateRate(-1, 1, 1);

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), outofRangeMessge + paramsAprName)]
        public void LoanCalculator_Is_Apr_Negative_Failed() => _LoanCalculator.CalculateRate(1, -1, 1);

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), outofRangeMessge + paramsAprName)]
        public void LoanCalculator_Is_Apr_Zero_Failed() => _LoanCalculator.CalculateRate(1, 0, 1);

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), outofRangeMessge + paramsAprName)]
        public void LoanCalculator_Is_Apr_Over_100_Percent_Failed() => _LoanCalculator.CalculateRate(1, 101, 1);

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), outofRangeMessge + paramsNumberOfYearsName)]
        public void LoanCalculator_Is_numberOfmonthlyInstalment_Is_0_Failed() => _LoanCalculator.CalculateRate(1, 1, 0);

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), outofRangeMessge + paramsNumberOfYearsName)]
        public void LoanCalculator_Is_numberOfmonthlyInstalment_Is_Negative_Failed() => _LoanCalculator.CalculateRate(1, 1, -1);

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), outofRangeMessge + paramsDepositName)]
        public void LoanCalculator_Is_deposit_Is_Negative_Failed() => _LoanCalculator.CalculateRate(1, 1, -1);

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException), "deposit cannot be greater than orderAmount: Parameter name: deposit")]
        public void LoanCalculator_Is_deposit_greater_than_orderAmount_Failed() => _LoanCalculator.CalculateRate(1500, 1, 1, 1501);
        #endregion

        #region pass case
        [TestMethod]
        public void LoanCalculator_3_Year_Instalment_At_8_point_9_Apr_Passed()
        {
            var response = _LoanCalculator.CalculateRate(5000, 8.9, 3); // orderAmount, apr, numberOfyears,

            Assert.AreEqual(157.96, response.MonthlyPayments);
            Assert.AreEqual(5686.56, response.TotalAmountPayable);
            Assert.AreEqual(686.56, response.InterestCharged);

            Assert.AreEqual(36, response.TermsInMonth);
        }

        [TestMethod]
        public void LoanCalculator_5_Year_Instalment_At_8_point_9_Apr_Passed()
        {
            var response = _LoanCalculator.CalculateRate(5000, 8.9, 5);

            Assert.AreEqual(102.71, response.MonthlyPayments);
            Assert.AreEqual(6162.60, response.TotalAmountPayable);
            Assert.AreEqual(1162.60, response.InterestCharged);

            Assert.AreEqual(60, response.TermsInMonth);
        }

        [TestMethod]
        public void LoanCalculator_7_Year_Instalment_At_8_point_9_Apr_Passed()
        {
            var response = _LoanCalculator.CalculateRate(5000, 8.9, 7);

            Assert.AreEqual(79.32, response.MonthlyPayments);
            Assert.AreEqual(6662.88, response.TotalAmountPayable);
            Assert.AreEqual(1662.88, response.InterestCharged);

            Assert.AreEqual(84, response.TermsInMonth);
        }

        [TestMethod]
        public void LoanCalculator_10_Year_Instalment_At_8_point_9_Apr_Passed()
        {
            var response = _LoanCalculator.CalculateRate(5000, 8.9, 10);

            Assert.AreEqual(62.14, response.MonthlyPayments);
            Assert.AreEqual(7456.80, response.TotalAmountPayable);
            Assert.AreEqual(2456.80, response.InterestCharged);

            Assert.AreEqual(120, response.TermsInMonth);
        }

        [TestMethod]
        public void LoanCalculator_3_Year_Instalment_At_10_Apr_Passed()
        {
            var response = _LoanCalculator.CalculateRate(5000, 10, 3);

            Assert.AreEqual(160.32, response.MonthlyPayments);
            Assert.AreEqual(5771.52, response.TotalAmountPayable);
            Assert.AreEqual(771.52, response.InterestCharged);

            Assert.AreEqual(36, response.TermsInMonth);
        }

        [TestMethod]
        public void LoanCalculator_5_Year_Instalment_At_10_Apr_Passed()
        {
            var response = _LoanCalculator.CalculateRate(5000, 10, 5);

            Assert.AreEqual(105.17, response.MonthlyPayments);
            Assert.AreEqual(6310.20, response.TotalAmountPayable);
            Assert.AreEqual(1310.20, response.InterestCharged);

            Assert.AreEqual(60, response.TermsInMonth);
        }

        [TestMethod]
        public void LoanCalculator_7_Year_Instalment_At_10_Apr_Passed()
        {
            var response = _LoanCalculator.CalculateRate(5000, 10, 7);

            Assert.AreEqual(81.89, response.MonthlyPayments);
            Assert.AreEqual(6878.76, response.TotalAmountPayable);
            Assert.AreEqual(1878.76, response.InterestCharged);

            Assert.AreEqual(84, response.TermsInMonth);
        }

        [TestMethod]
        public void LoanCalculator_10_Year_Instalment_At_10_Apr_Passed()
        {
            var response = _LoanCalculator.CalculateRate(5000, 10, 10);

            Assert.AreEqual(64.88, response.MonthlyPayments);
            Assert.AreEqual(7785.60, response.TotalAmountPayable);
            Assert.AreEqual(2785.60, response.InterestCharged);

            Assert.AreEqual(120, response.TermsInMonth);
        }
        #endregion
    }
}

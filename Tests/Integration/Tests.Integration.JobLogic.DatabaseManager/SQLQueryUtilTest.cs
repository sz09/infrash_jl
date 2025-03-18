using JobLogic.DatabaseManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tests.Integration.JobLogic.DatabaseManager
{
    [TestClass]
    public class SQLQueryUtilTest : BaseTest
    {
        [TestMethod]
        public void SQLFormatSplitFunction_WithString_ReturnCorrectResult()
        {
            //Arrange 
            var input = "SQLPara1";

            // Action
            var output = SQLQueryUtils.FormatSplitFunction(input);

            // Assert

            Assert.AreEqual(" Select TRIM(VALUE) from STRING_SPLIT(@SQLPara1,',')", output);

        }

        [TestMethod]
        public void SQLFormatSplitFunction_With_IEnumarable_Int_ReturnCorrectResult()
        {
            //Arrange 
            var input = new List<int> { 1, 2, 3, 4, 5 };

            // Action
            var output = input.FormatSplitFunction();

            // Assert

            Assert.AreEqual("'1','2','3','4','5'", output);

        }

        [TestMethod]
        public void SQLSplitByComma_ReturnCorrectResult()
        {
            //Arrange 
            var input = "1, 2, 3, 4, 5 ";

            // Action
            var output = input.SplitByComma(typeof(int));

            // Assert

            Assert.AreEqual("'1','2','3','4','5'", output);

        }

        [TestMethod]
        public void SQLSplitByComma_EmptyList_ReturnCorrectResult()
        {
            //Arrange 
            var input = " ";

            // Action
            var output = input.SplitByComma(typeof(int));

            // Assert

            Assert.AreEqual("NULL", output);

        }
    }
}

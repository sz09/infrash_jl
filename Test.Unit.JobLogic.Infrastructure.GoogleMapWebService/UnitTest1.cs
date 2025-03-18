using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using JobLogic.Infrastructure.GoogleMapWebService;

namespace Test.Unit.JobLogic.Infrastructure.GoogleMapWebService
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_GetLongLatByAddressAsync_NullPostalCodeWithValidAddress_ReturnGoodValue()
        {
            var googleService = new GoogleMapWebService(string.Empty);

        }
    }
}

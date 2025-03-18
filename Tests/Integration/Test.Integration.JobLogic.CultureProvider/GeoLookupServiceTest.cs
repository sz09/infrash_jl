using Microsoft.VisualStudio.TestTools.UnitTesting;
using JobLogic.Infrastructure.CultureProvider;
using System.Configuration;

namespace Test.Integration.JobLogic.CultureProvider
{
    [TestCategory("Integration")]
    [TestClass]
    public class GeoLookupServiceTest
    {
        private IGeoLookupService _geoLookupService;

        [TestInitialize]
        public void Initialize()
        {
            _geoLookupService = new GeoLookupService(ConfigurationManager.AppSettings["ipstackApi_Key"]);
        }


        [TestMethod]
        public void GetGeoData_NoIpAddress_ShouldReturnError()
        {
            var response = _geoLookupService.GetGeoData(string.Empty);
            //Assert.IsFalse(response.Success);
            Assert.IsNull(response);
        }

        [TestMethod]
        public void GetGeoData_DebugIpAddress_ShouldReturnError()
        {
            var response = _geoLookupService.GetGeoData("::1");
            //Assert.IsFalse(response.Success);
            Assert.IsNull(response);
        }

        [TestMethod]
        public void GetGeoData_LocalIpAddress_ShouldReturnError()
        {
            var response = _geoLookupService.GetGeoData("192.168.141.1");
            //Assert.IsTrue(response.Success);
            Assert.IsNotNull(response);

            var geoData = response;
            Assert.IsNotNull(geoData.JsonResponse);
            Assert.AreEqual(null, geoData.country_name);
            Assert.AreEqual(null, geoData.time_zone);
            Assert.AreEqual(null, geoData.calling_code);
            Assert.AreEqual(null, geoData.currency_code);
            Assert.AreEqual(null, geoData.country_code);
        }

        [TestMethod]
        public void GetGeoData_AustralianIpAddress_ShouldReturnAustralianDetails()
        {
            var response = _geoLookupService.GetGeoData("110.174.165.78");
            // Assert.IsTrue(response.Success);
            Assert.IsNotNull(response);

            var geoData = response;
            Assert.IsNotNull(geoData.JsonResponse);
            Assert.AreEqual("Australia", geoData.country_name);
            Assert.IsTrue(geoData.time_zone.StartsWith("Australia/"));
            Assert.AreEqual("61", geoData.calling_code);
            Assert.AreEqual("AUD", geoData.currency_code);
            Assert.AreEqual("AU", geoData.country_code);
        }

        [TestMethod]
        public void GetGeoData_UKIpAddress_ShouldReturnUKDetails()
        {
            var response = _geoLookupService.GetGeoData("81.133.60.32");
            //Assert.IsTrue(response.Success);
            Assert.IsNotNull(response);

            var geoData = response;
            Assert.IsNotNull(geoData.JsonResponse);
            Assert.AreEqual("United Kingdom", geoData.country_name);
            Assert.AreEqual("Europe/London", geoData.time_zone);
            Assert.AreEqual("44", geoData.calling_code);
            Assert.AreEqual("GBP", geoData.currency_code);
            Assert.AreEqual("GB", geoData.country_code);
        }

        [TestMethod]
        public void GetGeoData_AmericanIpAddress_ShouldReturnAmericanDetails()
        {
            var response = _geoLookupService.GetGeoData("104.243.47.157");
            // Assert.IsTrue(response.Success);
            Assert.IsNotNull(response);

            var geoData = response;
            Assert.IsNotNull(geoData.JsonResponse);
            Assert.AreEqual("United States", geoData.country_name);
            Assert.IsTrue(geoData.time_zone.StartsWith("America/"));
            Assert.AreEqual("1", geoData.calling_code);
            Assert.AreEqual("USD", geoData.currency_code);
            Assert.AreEqual("US", geoData.country_code);
        }

        [TestMethod]
        public void GetGeoData_PakistanIpAddress_ShouldReturnPakistanDetails()
        {
            var response = _geoLookupService.GetGeoData("203.223.174.148");
            //Assert.IsTrue(response.Success);
            Assert.IsNotNull(response);

            var geoData = response;
            Assert.IsNotNull(geoData.JsonResponse);
            Assert.AreEqual("Pakistan", geoData.country_name);
            Assert.IsTrue(geoData.time_zone.StartsWith("Asia/"));
            Assert.AreEqual("92", geoData.calling_code);
            Assert.AreEqual("PKR", geoData.currency_code);
            Assert.AreEqual("PK", geoData.country_code);
        }

        [TestMethod]
        public void GetGeoData_CandianIpAddress_ShouldReturnCandianDetails()
        {
            var response = _geoLookupService.GetGeoData("142.113.35.112");
            // Assert.IsTrue(response.Success);
            Assert.IsNotNull(response);

            var geoData = response;
            Assert.IsNotNull(geoData.JsonResponse);
            Assert.AreEqual("Canada", geoData.country_name);
            Assert.IsTrue(geoData.time_zone.StartsWith("America/"));
            Assert.AreEqual("1", geoData.calling_code);
            Assert.AreEqual("CAD", geoData.currency_code);
            Assert.AreEqual("CA", geoData.country_code);
        }
    }
}

using JobLogic.Infrastructure.Log;
using JobLogic.Infrastructure.Track;
using JobLogic.Infrastructure.UnitTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Integration.JobLogic.Infrastructure.Track
{
    [TestClass]
    public class ApplicationInsightsTrackServiceTests
    {
        const string FAKE_KEY = "FAKE_KEY";

        [TestMethod]
        public void TestTrackEvent()
        {
            var kk = new ApplicationInsightsTrackService(FAKE_KEY);
            var AppName = ValueGenerator.String();
            kk.TrackEvent(ValueGenerator.String(), new { AppLevel = ValueGenerator.String(), AppName});
        }

        [TestMethod]
        public void TestTraceCritical()
        {
            var kk = new ApplicationInsightsTrackService(FAKE_KEY);
            var AppName = ValueGenerator.String();
            kk.TraceCritical(ValueGenerator.String(), new { AppLevel = ValueGenerator.Int(), AppName });
        }

        [TestMethod]
        public void TestTraceError()
        {
            var kk = new ApplicationInsightsTrackService(FAKE_KEY);
            kk.TraceError(ValueGenerator.String(), ValueGenerator.Int());
        }

        [TestMethod]
        public void TestTraceInformation()
        {
            var kk = new ApplicationInsightsTrackService(FAKE_KEY);
            kk.TraceInformation(ValueGenerator.String(), ValueGenerator.CreateObject<ApplicationLog>());
        }

        [TestMethod]
        public void TestTraceWarning()
        {
            var kk = new ApplicationInsightsTrackService(FAKE_KEY);
            kk.TraceWarning(null, ValueGenerator.CreateObject<ApplicationLog>());
        }
    }
}

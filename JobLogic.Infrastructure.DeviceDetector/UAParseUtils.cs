using DeviceDetectorNET.Parser;
using System;

namespace JobLogic.Infrastructure.DeviceDetector
{
    public static class UAParseUtils
    {
        public static class UAParseUtils_Response
        {
            public const string error_parsing = "PARSING ERROR!";
        }

        public class UAParseResult
        {
            public string DeviceName { get; set; }
            public string OS { get; set; }
            public string OSVersion { get; set; }
            public string Browser { get; set; }
            public string BrowserVersion { get; set; }
            public bool IsMobile { get; set; }

            public UAParseResult()
            {
                Browser = UAParseUtils_Response.error_parsing;
                DeviceName = UAParseUtils_Response.error_parsing;
                OS = UAParseUtils_Response.error_parsing;
                OSVersion = UAParseUtils_Response.error_parsing;
                BrowserVersion = UAParseUtils_Response.error_parsing;
            }
        }

        static UAParseUtils()
        {
            DeviceDetectorNET.DeviceDetector.SetVersionTruncation(VersionTruncation.VERSION_TRUNCATION_NONE);
        }

        public static UAParseResult Parse(string userAgent)
        {
            try
            {
                var detector = new DeviceDetectorNET.DeviceDetector(userAgent);
                detector.Parse();
                var branchName = detector.GetBrandName();
                var model = detector.GetModel();
                var hasBranchNameOrModel = !string.IsNullOrWhiteSpace(branchName + model);
                var device = hasBranchNameOrModel ? $"{branchName} {model}" : detector.GetDeviceName();
                var osMatch = detector.GetOs().Match;
                var browserClient = detector.GetBrowserClient().Match;
                return new UAParseResult
                {
                    Browser = browserClient?.Name,
                    DeviceName = device,
                    OS = osMatch?.Name,
                    OSVersion = osMatch?.Version,
                    BrowserVersion = browserClient?.Version,
                    IsMobile = detector.IsMobile()
                };
            }
            //Since this is 3rd party library, caution is necessary
            catch (Exception ex)
            {
                //Gently fail with an error parsing result
                return new UAParseResult();
            }
        }
    }
}

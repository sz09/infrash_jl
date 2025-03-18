using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.Unit.JobLogic.Infrastructure.Microservice.Client
{
    [TestClass]
    public class SetupInitializer
    {
        public const string PREFERRED_ROUTE_NAME = "MyTestPreferredRoute";
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            Environment.SetEnvironmentVariable("JL_PREFERRED_ROUTE", "MyTestPreferredRoute");
        }
    }
}

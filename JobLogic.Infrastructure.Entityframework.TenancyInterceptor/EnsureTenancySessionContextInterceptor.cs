using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;

namespace JobLogic.Infrastructure.Entityframework.TenancyInterceptor
{
    public class EnsureTenancySessionContextInterceptor
    {
        static EnsureTenancySessionContextInterceptor()
        {
            const string INTERCEPT_NAME = "JL_Session_Context_Interceptor";
            DbInterception.Remove(new TenancySessionContextInterceptor(INTERCEPT_NAME));
            DbInterception.Add(new TenancySessionContextInterceptor(INTERCEPT_NAME));
        }
        /// <summary>
        /// Do Nothing, just a dummy method to ensure static constructor is called, and remind developer ToMakeSureDbContextIsFromITenancyDbContext
        /// </summary>
        public static void RememberToMakeSureDbContextIsFromITenancyDbContext() { }
    }
}

using System;

namespace JobLogic.Infrastructure.Microservice.Client
{
    public static class OperationInfoRuntimeDataSetter
    {
        /// <summary>
        ///  Set RuntimeData for Operation Info
        /// </summary>
        /// <param name="operationInfo"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="doOverwriteIfExist"></param>
        /// <returns>Return true if Set successfully, otherwise false.</returns>
        /// <exception cref="InvalidOperationException">Invalid Operation Type</exception>
        public static bool TrySet(ITenantlessOperationInfo operationInfo, string key, string value, bool doOverwriteIfExist)
        {
            var opInfo = operationInfo as OperationInfo ?? throw new InvalidOperationException("Invalid Operation Type");
            return opInfo.TrySetRuntimeData(key, value, doOverwriteIfExist);
        }
    }
}

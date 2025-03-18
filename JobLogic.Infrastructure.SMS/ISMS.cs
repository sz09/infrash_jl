using JobLogic.Infrastructure.Contract.Extensions;
using JobLogic.Infrastructure.OneOf;
using JobLogic.Infrastructure.ServiceResponders;
using System;
using System.Threading;
using System.Threading.Tasks;
using Twilio.Rest.Lookups.V1;

namespace JobLogic.Infrastructure.SMS
{
    public interface ISMSNotification
    {
        [Obsolete("Should use Async version instead")]
        Response<SMSResponse> SendSMS(string toPhoneNumber, string body, string fromPhoneNumber = null, string countryCode = null);

        Task<Response<SMSResponse>> SendSMSAsync(string toPhoneNumber, string body, string fromPhoneNumber = null, string countryCode = null, string statusCallbackUrl = null, CancellationToken cancellationToken = default);

        [Obsolete("Should use Async version instead")]
        Response<SMSTwoWayResponse> SendTwoWaySMS(Guid tenantId, string flowId, string jobNumber, string to, string from);

        Task<Response<SMSTwoWayResponse>> SendTwoWaySMSAsync(Guid tenantId, string flowId, string jobNumber, string to, string from, CancellationToken cancellationToken = default);

        [Obsolete("Should use Async version instead")]
        /// <summary>
        /// Tries to validate the phone number using twilio api. If the number doesn't contain a +, then try sending country code as well.
        /// </summary>
        Response<SMSPhoneNumberResource> ValidatePhoneNumber(string toPhoneNumber, string countryCode = null);

        /// <summary>
        /// Tries to validate the phone number using twilio api. If the number doesn't contain a +, then try sending country code as well.
        /// </summary>
        Task<Response<SMSPhoneNumberResource>> ValidatePhoneNumberAsync(string toPhoneNumber, string countryCode = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Validate phone number bypass with error code 20404
        /// </summary>
        /// <param name="toPhoneNumber"></param>
        /// <param name="countryCode"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<OneOf<bool, Error>> ValidatePhoneNumberFilterWithApiCodeAsync(string toPhoneNumber, string countryCode = null, CancellationToken cancellationToken = default);
    }
}

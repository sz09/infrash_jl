using JobLogic.Infrastructure.Contract.Extensions;
using JobLogic.Infrastructure.OneOf;
using JobLogic.Infrastructure.ServiceResponders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Twilio;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Rest.Lookups.V1;
using Twilio.Rest.Studio.V1.Flow;
using Twilio.Types;

namespace JobLogic.Infrastructure.SMS
{
    public class TwilioSMSNotification : ISMSNotification
    {
        private readonly string _accountSid;
        private readonly string _keySid;
        private readonly string _keySecret;
        private readonly string _smsFromAlphaNumerics;
        private const string _unableToSendSMS = "Failed To Send SMS";
        private const string _unableToValidateNumber = "Failed To Validate Number";

        private const string _ukISOCountryCode = "GB";
        private const int _invalidPhoneNumberExceptionCode = 20404;

        public TwilioSMSNotification(string accountSid, string authToken, string smsFromAlphaNumerics)
        {
            _accountSid = accountSid;
            _keySid = accountSid;
            _keySecret = authToken;
            _smsFromAlphaNumerics = smsFromAlphaNumerics;
        }

        public TwilioSMSNotification(string accountSid, string keySid, string keySecret, string smsFromAlphaNumerics)
        {
            _accountSid = accountSid;
            _keySid = keySid;
            _keySecret = keySecret;
            _smsFromAlphaNumerics = smsFromAlphaNumerics;
        }

        [Obsolete("Should use Async version instead")]
        public Response<SMSResponse> SendSMS(string toPhoneNumber, string body, string smsFromAlphaNumerics = null, string countryCode = null)
        {
            try
            {
                TwilioClient.Init(_keySid, _keySecret, _accountSid);

                var isoCountryCode = GetISOCountryCode(countryCode);

                var phoneNumber = !string.IsNullOrWhiteSpace(isoCountryCode) ?
                    GetVerifiedToPhoneNumber(toPhoneNumber, isoCountryCode) :
                    new PhoneNumber(toPhoneNumber);

               
                var smsResponse = MessageResource.Create(to: phoneNumber,
                                                         from: new PhoneNumber(GetFromPhoneNumberBasedOnAlphaNumericSupported(smsFromAlphaNumerics, countryCode)),
                                                         body: body);

                if (smsResponse == null)
                    return ResponseFactory.ReturnWithError<SMSResponse>(_unableToSendSMS);

                var respone = new SMSResponse
                {
                    Sid = smsResponse.Sid,
                    To = smsResponse.To,
                    From = smsResponse.From.ToString(),
                    Body = smsResponse.Body,
                    Cost = smsResponse.Price,
                    Status = smsResponse.Status.ToString()
                };

                return ResponseFactory.Return(respone);
            }
            catch (ApiException ex)
            {
                return ResponseFactory.ReturnWithError<SMSResponse>($"Error: {ex.Code} - Status: {ex.Status} - Message: {ex.Message}");
            }
        }

        public async Task<Response<SMSResponse>> SendSMSAsync(string toPhoneNumber, string body, string smsFromAlphaNumerics = null, string countryCode = null, string statusCallbackUrl = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                TwilioClient.Init(_keySid, _keySecret, _accountSid);

                var isoCountryCode = GetISOCountryCode(countryCode);

                var phoneNumber = !string.IsNullOrWhiteSpace(isoCountryCode) ?
                    GetVerifiedToPhoneNumber(toPhoneNumber, isoCountryCode) :
                    new PhoneNumber(toPhoneNumber);

                var isValidUrl = !string.IsNullOrEmpty(statusCallbackUrl) && Uri.TryCreate(statusCallbackUrl, UriKind.Absolute, out Uri result);
                var smsResponse = await MessageResource.CreateAsync(
                    to: phoneNumber,
                    from: new PhoneNumber(GetFromPhoneNumberBasedOnAlphaNumericSupported(smsFromAlphaNumerics, countryCode)), 
                    body: body,
                    statusCallback: isValidUrl ? new Uri(statusCallbackUrl) : null);

                if (smsResponse == null)
                    return ResponseFactory.ReturnWithError<SMSResponse>(_unableToSendSMS);

                var respone = new SMSResponse
                {
                    Sid = smsResponse.Sid,
                    To = smsResponse.To,
                    From = smsResponse.From.ToString(),
                    Body = smsResponse.Body,
                    Cost = smsResponse.Price,
                    Status = smsResponse.Status.ToString()
                };

                return ResponseFactory.Return(respone);
            }
            catch (ApiException ex)
            {
                return ResponseFactory.ReturnWithError<SMSResponse>($"Error: {ex.Code} - Status: {ex.Status} - Message: {ex.Message}");
            }
        }

        [Obsolete("Should use Async version instead")]
        public Response<SMSTwoWayResponse> SendTwoWaySMS(Guid tenantId, string flowId, string jobNumber, string to, string from)
        {
            try
            {
                TwilioClient.Init(_keySid, _keySecret, _accountSid);

                var phoneNumer = GetVerifiedToPhoneNumber(to, _ukISOCountryCode);
                var smsEngagementOption = new CreateEngagementOptions(flowId, to: phoneNumer, from: new PhoneNumber(from))
                {
                    Parameters = new { jobNumber, tenantId }
                };

                var smsResponse = EngagementResource.Create(smsEngagementOption);
                if (smsResponse == null)
                    return ResponseFactory.ReturnWithError<SMSTwoWayResponse>(_unableToSendSMS);                

                var respone = new SMSTwoWayResponse
                {
                    FlowSid = smsResponse.Sid,
                    ContactChannelAddress = smsResponse.ContactChannelAddress,
                    Sid = smsResponse.Sid,
                    AccountSid = smsResponse.AccountSid,
                    ContactSid = smsResponse.ContactSid,
                    DateCreated = smsResponse.DateCreated
                };

                return ResponseFactory.Return(respone);
            }
            catch (ApiException ex)
            {
                return ResponseFactory.ReturnWithError<SMSTwoWayResponse>($"Error: {ex.Code} - Status: {ex.Status} - Message: {ex.Message}");
            }
        }

        public async Task<Response<SMSTwoWayResponse>> SendTwoWaySMSAsync(Guid tenantId, string flowId, string jobNumber, string to, string from, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                TwilioClient.Init(_keySid, _keySecret, _accountSid);

                var phoneNumer = GetVerifiedToPhoneNumber(to, _ukISOCountryCode);

                var smsEngagementOption = new CreateEngagementOptions(flowId, to: phoneNumer, from: new PhoneNumber(from))
                {
                    Parameters = new { jobNumber, tenantId }
                };
                var smsResponse = await EngagementResource.CreateAsync(smsEngagementOption);

                if (smsResponse == null)
                    return ResponseFactory.ReturnWithError<SMSTwoWayResponse>(_unableToSendSMS);

                var respone = new SMSTwoWayResponse
                {
                    FlowSid = smsResponse.Sid,
                    ContactChannelAddress = smsResponse.ContactChannelAddress,
                    Sid = smsResponse.Sid,
                    AccountSid = smsResponse.AccountSid,
                    ContactSid = smsResponse.ContactSid,
                    DateCreated = smsResponse.DateCreated
                };

                return ResponseFactory.Return(respone);
            }
            catch (ApiException ex)
            {
                return ResponseFactory.ReturnWithError<SMSTwoWayResponse>($"Error: {ex.Code} - Status: {ex.Status} - Message: {ex.Message}");
            }
        }

        [Obsolete("Should use Async version instead")]
        public Response<SMSPhoneNumberResource> ValidatePhoneNumber(string fullPhoneNumber, string countryCode = null)
        {
            try
            {
                TwilioClient.Init(_keySid, _keySecret, _accountSid);

                if (countryCode != null)
                {
                    var resourceWithCountry = PhoneNumberResource.Fetch(countryCode: countryCode, pathPhoneNumber: new PhoneNumber(fullPhoneNumber));

                    return ValidatePhoneNumber(resourceWithCountry);
                }
                else
                {
                    var resource = PhoneNumberResource.Fetch(pathPhoneNumber: new PhoneNumber(fullPhoneNumber));

                    return ValidatePhoneNumber(resource);
                }
            }
            catch (ApiException ex)
            {
                return ResponseFactory.ReturnWithError<SMSPhoneNumberResource>($"Error: {ex.Code} - Status: {ex.Status} - Message: {ex.Message}");
            }
        }

        public async Task<Response<SMSPhoneNumberResource>> ValidatePhoneNumberAsync(string fullPhoneNumber, string countryCode = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                TwilioClient.Init(_keySid, _keySecret, _accountSid);

                if (countryCode != null)
                {
                    var resourceWithCountry = await PhoneNumberResource.FetchAsync(countryCode: countryCode, pathPhoneNumber: new PhoneNumber(fullPhoneNumber));

                    return ValidatePhoneNumber(resourceWithCountry);
                }
                else
                {
                    var resource = await PhoneNumberResource.FetchAsync(pathPhoneNumber: new PhoneNumber(fullPhoneNumber));

                    return ValidatePhoneNumber(resource);
                }
            }
            catch (ApiException ex)
            {
                return ResponseFactory.ReturnWithError<SMSPhoneNumberResource>($"Error: {ex.Code} - Status: {ex.Status} - Message: {ex.Message}");
            }
        }

        private static Response<SMSPhoneNumberResource> ValidatePhoneNumber(PhoneNumberResource resource)
        {
            if (resource == null || resource.PhoneNumber == null)
                return ResponseFactory.ReturnWithError<SMSPhoneNumberResource>(_unableToValidateNumber);

            var response = new SMSPhoneNumberResource
            {
                CallerName = resource.CallerName,
                CountryCode = resource.CountryCode,
                PhoneNumber = resource.PhoneNumber.ToString(),
                NationalFormat = resource.NationalFormat,
                Carrier = resource.Carrier,
                AddOns = resource.AddOns,
                Url = resource.Url
            };

            return ResponseFactory.Return(response);
        }

        private PhoneNumber GetVerifiedToPhoneNumber(string to, string countryCode)
        {
            // This Code is used to validate the phone numbers before sending 
            var phoneNumberResource = PhoneNumberResource.Fetch(
                            countryCode: countryCode,
                            pathPhoneNumber: new Twilio.Types.PhoneNumber(to)
                            );

            return phoneNumberResource.PhoneNumber;
        }

        private string GetISOCountryCode(string countryCode)
        {
            if (string.IsNullOrWhiteSpace(countryCode)) return null;

            // Converty The culture code or conutry name to ISO country codes
           return countryCode.Split(new char[] { '-' }, 2, StringSplitOptions.RemoveEmptyEntries).Last();
        }

        private string GetFromPhoneNumberBasedOnAlphaNumericSupported(string smsFromAlphaNumerics, string countryCode)
        {

            var fromAlphaNumerics = !string.IsNullOrWhiteSpace(smsFromAlphaNumerics) ? smsFromAlphaNumerics : _smsFromAlphaNumerics;

            if (string.IsNullOrWhiteSpace(countryCode)) return fromAlphaNumerics;

            // At the moment We allow UK customers to use.
            var allowedCountryCodes = new List<string>
            {
                "en-GB",
                "GB"
            };

            if (allowedCountryCodes.Contains(countryCode))
                return fromAlphaNumerics;
            else
                return _smsFromAlphaNumerics;
        }

        public async Task<OneOf<bool, Error>> ValidatePhoneNumberFilterWithApiCodeAsync(string phoneNumber, string countryCode = null, CancellationToken cancellationToken = default)
        {
            try
            {
                TwilioClient.Init(_keySid, _keySecret, _accountSid);

                var phone = await PhoneNumberResource.FetchAsync(
                    type: null,
                    countryCode: countryCode?.ToUpper(),
                    pathPhoneNumber: new Twilio.Types.PhoneNumber(phoneNumber));

                return true;
            }
            catch (Exception e)
            {
                if (e is ApiException apiException && apiException.Code == _invalidPhoneNumberExceptionCode)
                    return false;

                return new Error(e.Message);
            }
        }
    }
}

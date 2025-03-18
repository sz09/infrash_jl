using JobLogic.Infrastructure.ServiceResponders;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobLogic.Infrastructure.CAPTCHA
{
    public static class BaseCAPTCHAMessage
    {
        public const string InvalidCAPTCHA = "Invalid CAPTCHA";
        public const string CAPTCHAError = "You have not confirmed that you are not a robot";
    }

    public class BaseCAPTCHARequest
    {
    }

    public interface IBaseCAPTCHAValidator<T>
        where T : BaseCAPTCHARequest
    {
        Task<Response> ValidateAsync(T captchaRequest, CancellationToken cancellationToken = default);
    }

    public abstract class BaseCAPTCHAValidator<T> : IBaseCAPTCHAValidator<T>
        where T : BaseCAPTCHARequest
    {
        public Task<Response> ValidateAsync(T captchaRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                if (captchaRequest == null)
                {
                    return Task.FromResult(ResponseFactory.ReturnWithError(BaseCAPTCHAMessage.InvalidCAPTCHA));
                }

                return DoValidateAsync(captchaRequest, cancellationToken);
            }
            catch (Exception ex)
            {
                return Task.FromResult(ResponseFactory.ReturnWithError(ex.Message));
            }
        }

        protected abstract Task<Response> DoValidateAsync(T captchaRequest, CancellationToken cancellationToken = default);
    }
}

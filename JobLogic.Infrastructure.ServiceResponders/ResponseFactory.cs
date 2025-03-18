using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JobLogic.Infrastructure.ServiceResponders
{
    public static class ResponseFactory
    {
        public static Response Return()
        {
            return new Response { Status = ResponseStatus.SUCCESS };
        }

        public static Response Return(object data)
        {
            return new Response(data) { Status = ResponseStatus.SUCCESS };
        }

        public static Response ReturnWithSuccessMessage(string message)
        {
            return new Response() { Status = ResponseStatus.SUCCESS, Message = message };
        }

        public static Response<T> ReturnWithSuccessMessage<T>(string message)
        {
            return new Response<T>() { Status = ResponseStatus.SUCCESS, Message = message };
        }

        public static Response<T> Return<T>(T data)
        {
            return new Response<T>(data) { Status = ResponseStatus.SUCCESS };
        }

        public static Response<T> ReturnWithValidationError<T>(IEnumerable<ValidationResult> validationMessages)
        {
            var v = new Response<T>();
            v.AddValidation(validationMessages);
            return v;
        }

        public static Response ReturnWithValidationError(IEnumerable<ValidationResult> validationMessages)
        {
            var v = new Response();
            v.AddValidation(validationMessages);
            return v;
        }

        public static Response ReturnWithValidationError(string validationMessage)
        {
            return ReturnWithValidationError(new List<ValidationResult>() {new ValidationResult(validationMessage)});
        }

        public static Response<T> ReturnWithError<T>(string errorMessage)
        {
            return ReturnWithError<T>(new[] { errorMessage });
        }

        public static Response<T> ReturnWithNotExistError<T>(string errorMessage)
        {
            var v = ReturnWithError<T>(errorMessage);
            v.NotExist = true;
            return v;
        }

        public static Response<T> ReturnWithError<T>(params string[] errorMessages)
        {
            var v = new Response<T>();
            v.AddError(errorMessages);
            return v;
        }

        public static Response<T> ReturnWithError<T>(IEnumerable<string> errorMessages)
        {
            return ReturnWithError<T>(errorMessages.ToArray());
        }

        public static Response ReturnWithError(string errorMessage)
        {
            return ReturnWithError(new[] { errorMessage });
        }

        public static Response ReturnWithNotExistError(string errorMessage)
        {
            var v = ReturnWithError(errorMessage);
            v.NotExist = true;
            return v;
        }

        public static Response ReturnWithError(params string[] errorMessages)
        {
            var v = new Response();
            v.AddError(errorMessages);
            return v;
        }

        public static Response ReturnWithError(IEnumerable<string> errorMessages)
        {
            return ReturnWithError(errorMessages.ToArray());
        }


        public static Response ReturnWithException(Exception ex)
        {
            var v = new Response();
            v.AddException(ex);
            return v;
        }

        public static Response<T> ReturnWithException<T>(Exception ex)
        {
            var v = new Response<T>();
            v.AddException(ex);
            return v;
        }
    }
}

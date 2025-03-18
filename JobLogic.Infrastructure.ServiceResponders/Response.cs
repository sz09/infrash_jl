using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace JobLogic.Infrastructure.ServiceResponders
{
    public class Response<T>
    {
        public List<ValidationResult> ValidationMessages { get; set; }
        public List<string> ErrorMessages { get; set; }
        public Exception Exception { get; set; }
        public ResponseStatus Status { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }


        public Response()
        {
            Status = ResponseStatus.FAILURE;
            ValidationMessages = new List<ValidationResult>();
            ErrorMessages = new List<string>();
        }

        public Response(T data) : this()
        {
            Data = data;
        }

        public void AddValidation(IEnumerable<ValidationResult> validationMessages)
        {
            Status = ResponseStatus.VALIDATION;
            ValidationMessages.AddRange(validationMessages);
        }

        public void AddValidation(IEnumerable<string> validationMessages)
        {
            AddValidation(validationMessages.Select(v => new ValidationResult(v)));
        }

        public void AddValidation(string validationMessage)
        {
            AddValidation(new List<ValidationResult>() { new ValidationResult(validationMessage) });
        }

        public void AddError(params string[] errorMessages)
        {
            Status = ResponseStatus.FAILURE;
            ErrorMessages.AddRange(errorMessages);
        }

        public string ErrorMessagesToString(string separator = "\r\n")
        {
            return string.Join(separator, ErrorMessages);
        }

        public string ValidationMessagesToString(string separator = "\r\n")
        {
            return string.Join(separator, ValidationMessages);
        }

        public void AddException(Exception ex)
        {
            Status = ResponseStatus.EXCEPTION;
            Exception = ex;
        }

        public void AddMessage(string message)
        {
            Message = message;
        }

        public bool Success
        {
            get { return Status == ResponseStatus.SUCCESS; }
        }

        public bool NotExist { get; set; }

        public string GetAllErrors()
        {
            return string.Concat(this.ValidationMessagesToString(), this.ErrorMessagesToString());
        }
    }

    public class Response : Response<object>
    {
        public Response()
            : base()
        {

        }

        public Response(object data)
            : base(data)
        {

        }
    }
}

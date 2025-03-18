using System.Collections.Generic;
using System.Web.Mvc;

namespace JobLogic.Infrastructure.JsonResponders
{
    public class JsonResultData
    {
        public JsonResultData()
        {
            errors = new List<string>();
        }

        public bool success { get; set; }
        public IEnumerable<string> errors { get; set; }
        public string Message { get; set; }
        public string redirectUrl { get; set; }
        public List<JsonValidationData> ValidationData { get; set; }
    }

    public class JsonValidationData
    {
        public string Key { get; set; }
        public List<string> Message { get; set; }
    }

    public class JsonResultData<T> : JsonResultData
    {
        public T AdditionalData { get; set; }
    }

    public class JobLogicJsonResult : JsonResult
    {
        public JsonResultData JsonReturnData { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            Data = JsonReturnData;
            JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            MaxJsonLength = int.MaxValue;
            base.ExecuteResult(context);
        }
    }

    public static class JsonResultFactory
    {
        public static JobLogicJsonResult Success(string message = null)
        {
            return new JobLogicJsonResult()
            {
                JsonReturnData = new JsonResultData() { success = true, Message = message }
            };
        }

        public static JobLogicJsonResult Fail(params string[] messages)
        {
            return new JobLogicJsonResult()
            {
                JsonReturnData = new JsonResultData()
                {
                    success = false,
                    errors = messages
                }
            };
        }

        public static JobLogicJsonResult Fail(List<JsonValidationData> validation, params string[] messages)
        {
            return new JobLogicJsonResult()
            {
                JsonReturnData = new JsonResultData()
                {
                    success = false,
                    errors = messages,
                    ValidationData = validation
                }
            };
        }

        public static JobLogicJsonResult Success<T>(T t, string message = null)
        {
            return new JobLogicJsonResult()
            {
                JsonReturnData = new JsonResultData<T>() { success = true, AdditionalData = t, Message = message }
            };
        }

        public static JobLogicJsonResult Success(dynamic t, string message = null)
        {
            return new JobLogicJsonResult()
            {
                JsonReturnData = new JsonResultData<dynamic>() { success = true, AdditionalData = t, Message = message }
            };
        }

        //public static JsonResultItem FailWithMessage(string message, params string[] errors)
        //{
        //    return new JsonResultItem()
        //    {
        //        JsonReturnData = new JsonResulData() { Success = false, Errors = errors.ToList(), Message = message }
        //    };
        //}

        public static JobLogicJsonResult Redirect(string redirectUrl)
        {
            return new JobLogicJsonResult()
            {
                JsonReturnData = new JsonResultData() { success = true, redirectUrl = redirectUrl }
            };
        }
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Common.Logs;
using NTS.Common.Helpers;
using Newtonsoft.Json;

namespace NTSFile.Api.Attributes
{
    public class ApiHandleExceptionSystemAttribute : ExceptionFilterAttribute
    {

        public override void OnException(HttpActionExecutedContext context)
        {
            base.OnException(context);

            string message = ResourceUtil.GetResourcesNoLag(ErrorResourceKey.ERR0001);

            if (context.Exception is NTSException)
            {
                var customError = context.Exception as NTSException;
                message = customError.Message ?? customError.ErrorCode;
            }
            else
            {
                NtsLog.LogError(context.Exception);

                if (context.Exception is NTSLogException)
                {
                    NtsLog.LogError(JsonConvert.SerializeObject((context.Exception as NTSLogException).ExceptionInfo));
                }
                else
                {
                    NtsLog.LogError(JsonConvert.SerializeObject(context.Exception.GetExceptionInfo()));
                }
            }

            context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, message);
        }

    }
}

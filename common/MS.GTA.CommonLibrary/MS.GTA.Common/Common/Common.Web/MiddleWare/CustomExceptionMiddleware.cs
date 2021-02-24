//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Web.MiddleWare
{
    using System.Net;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using MS.GTA.Common.Base;
    using MS.GTA.ServicePlatform.Exceptions;
    using Newtonsoft.Json;

    /// <summary>
    /// Handler for custom exceptions in the Application.
    /// </summary>
    public class CustomExceptionMiddleware : IExceptionFilter
    {
        /// <summary>
        /// Interface method to handle the exception in Filter
        /// </summary>
        /// <param name="context">Context of the exception</param>
        public void OnException(ExceptionContext context)
        {
            HttpStatusCode statusCode = (context.Exception as WebException != null &&
                        ((HttpWebResponse)(context.Exception as WebException).Response) != null) ?
                         ((HttpWebResponse)(context.Exception as WebException).Response).StatusCode
                         : (HttpStatusCode)context.Exception.GetHttpStatusCode();

            string code = "" ;
            
            var mex = context.Exception as MonitoredException;
            if (mex != null)
            {
                code = mex.ServiceErrorCode;
            }

            string message = context.Exception.Message;
            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)statusCode;
            response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(
                new
                {   error = new {
                        message,
                        code,
                    },
                });
            response.ContentLength = result.Length;
            response.WriteAsync(result);
        }
    }
}

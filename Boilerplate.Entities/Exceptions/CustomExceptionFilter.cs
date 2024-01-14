using Microsoft.AspNetCore.WebUtilities;
using Microsoft.SqlServer.Management.Smo.Wmi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace Boilerplate.Entities.Exceptions
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string exceptionMessage = string.Empty;

            exceptionMessage = actionExecutedContext.Exception.InnerException == null
                ? actionExecutedContext.Exception.Message
                : actionExecutedContext.Exception.InnerException.Message;
           
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An unhandled exception was thrown by service."), 
                              ReasonPhrase= "Internal Server Error.Please Contact your Administrator.",
                              StatusCode = HttpStatusCode.InternalServerError
            };
            actionExecutedContext.Response = response;
        }
    }
}

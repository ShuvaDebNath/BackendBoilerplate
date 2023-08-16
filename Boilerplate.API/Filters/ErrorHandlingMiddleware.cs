using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
//using Boilerplate.Entities.ExceptionHandlers;
namespace Boilerplate.API.Filters
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //string? message = ((Exception)exception.ExceptionHandler()).Message.ToString();
            var result = JsonConvert.SerializeObject(new
            {
                status = false,
                message = exception.InnerException != null ? exception.InnerException.Message : exception.Message,
                messageType = "Exception",
                data = (string)null
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }
    }
}

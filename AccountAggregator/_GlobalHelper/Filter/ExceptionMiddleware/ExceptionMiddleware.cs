using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AccountAggregator._GlobalHelper.Filter.ExceptionMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private static ILogger logger = LogManager.GetCurrentClassLogger();
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong: {ex}", "Stopped program because of exception");

                await HandleException(httpContext, ex);
            }
        }
        private static Task HandleException(HttpContext context, Exception exception)
        {

            HttpStatusCode code = HttpStatusCode.InternalServerError; // 500 if unexpected
            
            string result = JsonConvert.SerializeObject(new {Code = (int)HttpStatusCode.InternalServerError,  error = exception.Message });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(result);
            
        }
    }
}

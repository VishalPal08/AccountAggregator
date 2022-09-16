using AccountAggregator.ServiceLayer.Global;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace AccountAggregator._GlobalHelper
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        
        public void OnException(ExceptionContext context)
        {
            _ClsException ObjEx = new _ClsException();

            HttpStatusCode status = HttpStatusCode.InternalServerError;


            string Controller = context.RouteData.Values["controller"].ToString();
            string Action = context.RouteData.Values["action"].ToString();
            string Exception = context.Exception.Message;
            int StatusCode = (int)HttpStatusCode.InternalServerError;

            ObjEx.InsertException(Controller, Action, StatusCode, Exception);

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";
            response.WriteAsync(Exception);

        }

        
    }
}

using AccountAggregator.InterfaceLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace AccountAggregator._GlobalHelper
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CustomAuthorizeCheckSumAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IAuthentication _AuthSvc;
        public CustomAuthorizeCheckSumAttribute(IAuthentication AuthSvc)
        {
            this._AuthSvc = AuthSvc;
        }
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext != null)

            {
                Microsoft.Extensions.Primitives.StringValues _OrignalChecksum;
                filterContext.HttpContext.Request.Headers.TryGetValue("Checksum", out _OrignalChecksum);

                var RequestBodyJsonString = "";
                var request = filterContext.HttpContext.Request;
                HttpRequestRewindExtensions.EnableBuffering(request);
                using (StreamReader reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
                {
                    RequestBodyJsonString = reader.ReadToEnd();
                    request.Body.Position = 0;
                }

                JObject jo = JObject.Parse(RequestBodyJsonString);

                String SourceName = jo.Property("SourceName").Value.ToString();

                string _CalculateCheckSum = _AuthSvc.GenerateChecksum(RequestBodyJsonString, SourceName);

                if (_OrignalChecksum.Equals(_CalculateCheckSum))
                {
                    filterContext.HttpContext.Response.Headers.Add("Checksum", _OrignalChecksum);
                    filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");

                    filterContext.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");

                    return;
                }
                else
                {
                    filterContext.HttpContext.Response.Headers.Add("Checksum", _OrignalChecksum);
                    filterContext.HttpContext.Response.Headers.Add("AuthStatus", "NotAuthorized");

                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
                    filterContext.Result = new JsonResult("NotAuthorized")
                    {
                        Value = new
                        {
                            Status = "Error",
                            Message = "Invalid checksum value"
                        },
                    };
                }
            }

        }
    }
}


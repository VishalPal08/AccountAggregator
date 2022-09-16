using AccountAggregator._GlobalHelper;
using AccountAggregator.InterfaceLayer;
using AccountAggregator.ModelLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;

namespace AccountAggregator.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthenticationController : ControllerBase
    {
        private readonly IAuthentication _AuthSvc;
        private readonly IConfiguration configuration;
        private readonly IBOLRequestNResponse _BolSvc;
        public UserAuthenticationController(IAuthentication AuthSvc, IConfiguration configuration, IBOLRequestNResponse BolSvc)
        {
            this._AuthSvc = AuthSvc;
            this.configuration = configuration;
            this._BolSvc = BolSvc;
        }

       
        //[HttpGet]
        //public IActionResult Get()
        //{
        //    return Ok("Success");
        //}

        [HttpPost]
        [Route("GenerateCheckSum")]
        public IActionResult GenerateCheckSum(BOLPRequest ObjBOLPRequest)
        {
            try
            {
                var reader = new StreamReader(Request.Body);
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                var JsonString = reader.ReadToEnd();
                

                string CheckSum = _AuthSvc.GenerateChecksum(JsonString, ObjBOLPRequest.SourceName);

                return Ok(new { CheckSumValue = CheckSum });
            }
            catch(Exception ex)
            {
                throw new MyAppException(ex.Message, ex.InnerException);
            }
        }

        [TypeFilter(typeof(CustomAuthorizeCheckSumAttribute))]
        [HttpPost]
        [Route("RedirectUrl")]
        public IActionResult GetRedirectUrl(BOLPRequest ObjBOLPRequest)
        {
            try
            {
                DateTime _ReqBolDt = DateTime.Now;
                string TokenGenerateAPI = configuration.GetValue<string>("GenerateTokenAPI");
                string GenerateRedirectAPI = configuration.GetValue<string>("GenerateRedirectAPI");

                string _TxnGuid = _BolSvc.InsertBasicNBankDetails(ObjBOLPRequest);

               // string JsonRequest = ServiceLayer.Global._JsonConvert.SerializeObject<BOLPRequest>(ObjBOLPRequest);

                #region Token Generation 

                    RequestAuthenication ObjRequestAuthBody = new RequestAuthenication()
                    {
                        fiuID = "Aero",
                        redirection_key = "KtTVEqCeuhBTim",
                        userId = "finduittest25@gmail.com"
                    };

                    DateTime _ReqAuthDt = DateTime.Now;
                    ResponseAuth ObjAuthResponse = _AuthSvc.GetToken(TokenGenerateAPI, ObjRequestAuthBody);
                    DateTime _ResAuthDt = DateTime.Now;

                    _BolSvc.InsertRequestAndResponse<RequestAuthenication, ResponseAuth>(ObjRequestAuthBody, ObjAuthResponse, _ReqAuthDt, _ResAuthDt, 2, _TxnGuid);

                #endregion

                #region  Generate Redirect Url

                    RequestRedirectUrl ObjRequestRedirectUrl = new RequestRedirectUrl()
                    {
                        clienttrnxid = _TxnGuid,
                        fiuID = "Aero",
                        userId = "finduittest25@gmail.com",
                        aaCustomerHandleId = ObjBOLPRequest.ObjBasicDetail.ProposerMobileNo + "@CAMSAA",
                        aaCustomerMobile = ObjBOLPRequest.ObjBasicDetail.ProposerMobileNo,
                        sessionId = ObjAuthResponse.sessionId,
                        useCaseId = "1",
                        fipid = "FIPCAMS-UAT",
                        addfip = "FALSE"
                    };
                    DateTime _ReqRedirectUrlDt = DateTime.Now;
                    ResponseRedirectUrl ObjRedirectUrlResponse = _AuthSvc.GetRedirectionUrl(GenerateRedirectAPI, ObjRequestRedirectUrl, ObjAuthResponse.token);
                    DateTime _ResRedirectUrlDt = DateTime.Now;

                    _BolSvc.InsertRequestAndResponse<RequestRedirectUrl, ResponseRedirectUrl>(ObjRequestRedirectUrl, ObjRedirectUrlResponse, _ReqRedirectUrlDt, _ResRedirectUrlDt, 3, _TxnGuid);

                #endregion

                DateTime _ResBolDt = DateTime.Now;

                ResponseBOLP ObjBolRes = new ResponseBOLP()
                {
                    StatusCode = (int)HttpStatusCode.OK,
                    Message = "Successfully generate redirection url",
                    TransactionId = _TxnGuid,
                    RedirectUrl = ObjRedirectUrlResponse.redirectionurl
                };

                _BolSvc.InsertRequestAndResponse<BOLPRequest, ResponseBOLP>(ObjBOLPRequest, ObjBolRes, _ReqBolDt, _ResBolDt, 1, _TxnGuid);

                return Ok(new { ObjBolRes });
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _BolSvc.Dispose();
                _AuthSvc.Dispose();
            }
        }
        
    }
}
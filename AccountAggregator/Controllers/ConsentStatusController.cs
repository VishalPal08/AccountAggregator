using System;
using AccountAggregator.InterfaceLayer;
using AccountAggregator.ModelLayer;
using AccountAggregator.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace AccountAggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsentStatusController : ControllerBase
    {
        private readonly IConsent _ConsentSvc;
        public ConsentStatusController(IConsent ConsentSvc)
        {
            this._ConsentSvc = ConsentSvc;
        }

        [HttpPost]
        [Route("ConsentStatusNotification")]
        public IActionResult ConsentStatusNotification([FromBody] RequestForConsent ObjConsent)
        {
            try
            {
                ResponseConsent ObjResConsent;

                DateTime _RequestConsent = new DateTime();
                DateTime _ResponseConsent = new DateTime();
                _RequestConsent = DateTime.Now;
                

                #region Checking Consent Id

                if(string.IsNullOrEmpty(ObjConsent.ObjConsentStatusNotification.ConsentStatusNotification.consentId))
                {
                    ObjResConsent = GenerateResponseObject(ObjConsent.ObjConsentStatusNotification.clienttxnid, ObjConsent.ObjConsentStatusNotification.timestamp,
                       false, "Consent Id null or empty in current consent status notification request body");

                    this._ConsentSvc.InsertRequestAndResponse<RequestConsentStatusNotification, ResponseConsent>
                        (ObjConsent.ObjConsentStatusNotification, ObjResConsent, _RequestConsent, _ResponseConsent, 4, ObjConsent.ObjConsentStatusNotification.clienttxnid);

                    return Ok(new { ObjResConsent });
                }
                else if (string.IsNullOrEmpty(ObjConsent.ObjDownloadStatement.pdfDetail.consentid))
                {
                    ObjResConsent = GenerateResponseObject(ObjConsent.ObjConsentStatusNotification.clienttxnid, ObjConsent.ObjConsentStatusNotification.timestamp,
                       false, "Consent Id null or empty in current downloadStatement request body");

                    this._ConsentSvc.InsertRequestAndResponse<RequestDownloadStatement, ResponseConsent>
                        (ObjConsent.ObjDownloadStatement, ObjResConsent, _RequestConsent, _ResponseConsent, 4, ObjConsent.ObjConsentStatusNotification.clienttxnid);

                    return Ok(new { ObjResConsent });
                }
                else if (ObjConsent.ObjConsentStatusNotification.ConsentStatusNotification.consentId != ObjConsent.ObjDownloadStatement.pdfDetail.consentid)
                {

                    _ResponseConsent = DateTime.Now;

                    ObjResConsent = GenerateResponseObject(ObjConsent.ObjConsentStatusNotification.clienttxnid, ObjConsent.ObjConsentStatusNotification.timestamp,
                        false, "Consent notification and download statement does not has simlar consentId");
                    
                    this._ConsentSvc.InsertRequestAndResponse<RequestConsentStatusNotification, ResponseConsent>
                        (ObjConsent.ObjConsentStatusNotification, ObjResConsent, _RequestConsent, _ResponseConsent, 4, ObjConsent.ObjConsentStatusNotification.clienttxnid);

                    return NotFound(new { ObjResConsent });
                }

                #endregion

                #region Checking Client Transaction Id

                if (string.IsNullOrEmpty(ObjConsent.ObjConsentStatusNotification.clienttxnid))
                {
                    ObjResConsent = GenerateResponseObject(ObjConsent.ObjConsentStatusNotification.clienttxnid, ObjConsent.ObjConsentStatusNotification.timestamp,
                       false, "Clienttxnid null or empty in current consent status notification request body");

                    this._ConsentSvc.InsertRequestAndResponse<RequestConsentStatusNotification, ResponseConsent>
                        (ObjConsent.ObjConsentStatusNotification, ObjResConsent, _RequestConsent, _ResponseConsent, 4, ObjConsent.ObjConsentStatusNotification.clienttxnid);

                    return Ok(new { ObjResConsent });
                }
                else if (string.IsNullOrEmpty(ObjConsent.ObjDownloadStatement.clienttxnid))
                {
                    ObjResConsent = GenerateResponseObject(ObjConsent.ObjConsentStatusNotification.clienttxnid, ObjConsent.ObjConsentStatusNotification.timestamp,
                       false, "Clienttxnid null or empty in downloadStatement request body");

                    this._ConsentSvc.InsertRequestAndResponse<RequestDownloadStatement, ResponseConsent>
                        (ObjConsent.ObjDownloadStatement, ObjResConsent, _RequestConsent, _ResponseConsent, 4, ObjConsent.ObjConsentStatusNotification.clienttxnid);

                    return Ok(new { ObjResConsent });
                }
                else if (ObjConsent.ObjConsentStatusNotification.clienttxnid != ObjConsent.ObjDownloadStatement.clienttxnid)
                {
                    _ResponseConsent = DateTime.Now;

                    ObjResConsent = GenerateResponseObject(ObjConsent.ObjConsentStatusNotification.clienttxnid, ObjConsent.ObjConsentStatusNotification.timestamp, false, "Consent notification and download statement does not have simlar clienttxnid Id");
                    
                    this._ConsentSvc.InsertRequestAndResponse<RequestConsentStatusNotification, ResponseConsent>
                        (ObjConsent.ObjConsentStatusNotification, ObjResConsent, _RequestConsent, _ResponseConsent, 4, ObjConsent.ObjConsentStatusNotification.clienttxnid);

                    return NotFound(new { ObjResConsent });
                }

                #endregion

                #region Check client transaction id exits or not

                if (!this._ConsentSvc.CheckClientTransactionIdExitOrNot(ObjConsent.ObjConsentStatusNotification.clienttxnid))
                {
                    _ResponseConsent = DateTime.Now;

                    ObjResConsent = GenerateResponseObject(ObjConsent.ObjConsentStatusNotification.clienttxnid, ObjConsent.ObjConsentStatusNotification.timestamp, false, "Client tranasction id mentioned in the request payload has not available in DB ");

                    this._ConsentSvc.InsertRequestAndResponse<RequestConsentStatusNotification, ResponseConsent>
                       (ObjConsent.ObjConsentStatusNotification, ObjResConsent, _RequestConsent, _ResponseConsent, 4, ObjConsent.ObjConsentStatusNotification.clienttxnid);

                    return NotFound(new { ObjResConsent });

                }

                #endregion

                #region Consent Status Notification

                if (ObjConsent.ObjConsentStatusNotification.purpose == "ConsentStatusNotification")
                {
                    _ResponseConsent = DateTime.Now;

                    ObjResConsent = GenerateResponseObject(ObjConsent.ObjConsentStatusNotification.clienttxnid, ObjConsent.ObjConsentStatusNotification.timestamp, true, "success”");

                    this._ConsentSvc.InsertRequestAndResponse<RequestConsentStatusNotification, ResponseConsent>(ObjConsent.ObjConsentStatusNotification, ObjResConsent, _RequestConsent, _ResponseConsent, 4, ObjConsent.ObjConsentStatusNotification.clienttxnid);

                   // return Ok(new { ObjResConsent });
                }

                #endregion


                #region Downloadstatement

                if (ObjConsent.ObjDownloadStatement.purpose == "Push_DATA")
                {


                    if(!string.IsNullOrEmpty(ObjConsent.ObjDownloadStatement.pdfDetail.pdfbase64))
                    {
                        ObjConsent.ObjDownloadStatement.pdfDetail.pdfbase64 = ObjConsent.ObjDownloadStatement.pdfDetail.pdfbase64.Replace("\"", "");

                        string DecryptionPdfBase64Data = Cryptography.DecryptString(ObjConsent.ObjDownloadStatement.pdfDetail.pdfbase64).Replace("\"", "");



                        _ConsentSvc.ConvertBase64ToPdf(DecryptionPdfBase64Data, ObjConsent.ObjDownloadStatement.clienttxnid);

                        string message = _ConsentSvc.SaveTransactionDetails(ObjConsent.ObjDownloadStatement, string.Empty, ObjConsent.ObjDownloadStatement.pdfDetail.pdfbase64, string.Empty, string.Empty, (int)TypeOfPdf.PdfBase64);
                    }

                    //if (!string.IsNullOrEmpty(ObjConsent.ObjDownloadStatement.pdfDetail.pdfbinary))
                    //{
                    //    string message = _ConsentSvc.SaveTransactionDetails(ObjConsent.ObjDownloadStatement, ObjConsent.ObjDownloadStatement.pdfDetail.pdfbinary, string.Empty, string.Empty, string.Empty, (int)TypeOfPdf.PdfBinary);
                    //}
                    
                    //if (!string.IsNullOrEmpty(ObjConsent.ObjDownloadStatement.pdfDetail.xmlData))
                    //{
                    //    string DecryptionXmlAccountData = Cryptography.DecryptString(ObjConsent.ObjDownloadStatement.pdfDetail.xmlData);

                    //    string message = _ConsentSvc.SaveTransactionDetails(ObjConsent.ObjDownloadStatement, DecryptionXmlAccountData, string.Empty, string.Empty, string.Empty, (int)TypeOfPdf.Xml);
                    //}

                    if (!string.IsNullOrEmpty(ObjConsent.ObjDownloadStatement.pdfDetail.jsonData))
                    {
                        string DecryptionJsonAccountData = Cryptography.DecryptString(ObjConsent.ObjDownloadStatement.pdfDetail.jsonData);

                        string message = _ConsentSvc.SaveTransactionDetails(ObjConsent.ObjDownloadStatement, DecryptionJsonAccountData, string.Empty, string.Empty, string.Empty, (int)TypeOfPdf.Json);

                        _ResponseConsent = DateTime.Now;

                        ObjResConsent = GenerateResponseObject(ObjConsent.ObjDownloadStatement.clienttxnid, ObjConsent.ObjDownloadStatement.timestamp, true, "success");
                        ObjResConsent.Message = message;

                        this._ConsentSvc.InsertRequestAndResponse<RequestDownloadStatement, ResponseConsent>(ObjConsent.ObjDownloadStatement, ObjResConsent, _RequestConsent, _RequestConsent, 5, ObjConsent.ObjDownloadStatement.clienttxnid);

                        return Ok(new { ObjResConsent });
                    }
                    
                    

                    return NoContent();
                }

                #endregion



                return Ok();
                
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                _ConsentSvc.Dispose();
            }
        }


        private ResponseConsent GenerateResponseObject(string clienttxnid,string timestamp,bool result,string Message)
        {
            try
            {
                return new ResponseConsent(clienttxnid, timestamp, result, Message);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        
    }
}
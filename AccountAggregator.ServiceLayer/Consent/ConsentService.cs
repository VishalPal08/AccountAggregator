using AccountAggregator.DataBaseLayer;
using AccountAggregator.InterfaceLayer;
using AccountAggregator.ModelLayer;
using AccountAggregator.ServiceLayer.Global;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace AccountAggregator.ServiceLayer
{
    public class ConsentService : IConsent
    {
        DbConnection ConnectionManager;
        public IConfiguration configuration = null;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        bool disposed = false;

        public ConsentService()
        {
            ConnectionManager = DbConnection.SingleInstance;
        }
        ~ConsentService()
        {
            Dispose(false);
        }


        public bool CheckClientTransactionIdExitOrNot(string ClientTxnId)
        {
            try
            {
                string SelectQuery = "Select count(1) from [dbo].[txn_ProposerDetails] Where TxtGuid = @TxtGuid";
                
                bool exists = false;
                using (IDbConnection cn = ConnectionManager.connection)
                {
                    cn.Open();

                    exists = cn.ExecuteScalar<bool>(SelectQuery, new { TxtGuid = ClientTxnId }); 
                }

                return exists;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private string GetApplicationNumberBaseOnClienttxnId(string ClientTxnId)
        {
            try
            {
                string SelectQuery = "Select policynumber,ApplicationNumber from [dbo].[txn_ProposerDetails] Where TxtGuid = @TxtGuid";

                string ApplicationNumber = string.Empty;
                using (IDbConnection cn = ConnectionManager.connection)
                {
                    cn.Open();

                    IEnumerable<IDictionary<string, object>> ProposerDetails = cn.Query(SelectQuery, new { TxtGuid = ClientTxnId }) as IEnumerable<IDictionary<string, object>>;

                    var PolicyNo = ProposerDetails.ElementAt(0)["policynumber"];
                    var ApplicationNo = ProposerDetails.ElementAt(0)["ApplicationNumber"];
                    ApplicationNumber = ApplicationNo + "_" + PolicyNo;
                }

                return ApplicationNumber;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public void InsertRequestAndResponse<T, T1>(T RequestBody, T1 ResponseBody, DateTime RequestTime, DateTime ResponseTime, int TypeId, string TxnGuidId)
        {
            try
            {
                string JsonRequest = _JsonConvert.SerializeObject<T>(RequestBody);
                string JsonResponse = _JsonConvert.SerializeObject<T1>(ResponseBody);

                string Insertquery = "Insert into [dbo].[txn_RequestNResponse] " +
                                    " (TypeId,TxtGuid,RequestBody,ResponseBody,RequestTimeStamp,ResponseTimeStamp) " +
                                    "values(@TypeId,@TxtGuid,@RequestBody,@ResponseBody,@ReqTimeStamp,@ResTimeStamp)";

                var parameters = new Dictionary<string, object>()
                {
                    ["TypeId"] = TypeId,
                    ["TxtGuid"] = TxnGuidId,
                    ["RequestBody"] = JsonRequest,
                    ["ResponseBody"] = JsonResponse,
                    ["ReqTimeStamp"] = RequestTime,
                    ["ResTimeStamp"] = ResponseTime
                };

                using (IDbConnection cn = ConnectionManager.connection)
                {
                    cn.Open();
                    cn.Execute(Insertquery, parameters);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }



        public string SaveTransactionDetails(RequestDownloadStatement ObjStatement,string JsonAccount,string PdfBase64, string PdfBinary,string XMLData,int StatementFlg)
        {
            try
            {

                DynamicParameters _params = new DynamicParameters(new
                {
                    purpose = ObjStatement.purpose,
                    ver = ObjStatement.ver,
                    timestamp = ObjStatement.timestamp,
                    txnid = ObjStatement.txnid,
                    clienttxnid = ObjStatement.clienttxnid,
                    customerId = ObjStatement.customerId,
                    fipid= ObjStatement.fipid,
                    fipname = ObjStatement.fipname,
                    pan = ObjStatement.pan,
                    JsonData = JsonAccount,
                    PdfBase64 = PdfBase64,
                    PdfBinary = PdfBinary,
                    XMLData = XMLData,
                    consentid = ObjStatement.pdfDetail.consentid,
                    maskedAccountNumber = ObjStatement.pdfDetail.maskedAccountNumber,
                    PdfDetailsFlg = StatementFlg
                });

                _params.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output,size : 1000);


                using (IDbConnection cn = ConnectionManager.connection)
                {
                    cn.Open();
                    cn.Execute("SP_StoreTransactionDetails", _params, commandType: CommandType.StoredProcedure);
                }

                string _Message = _params.Get<string>("@Message");

                return _Message;
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        public void ConvertBase64ToPdf(string Base64Data, string ClientTxnId)
        {
            try
            {
                string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string projectPath = appDirectory.Substring(0, appDirectory.IndexOf("\\bin"));

                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                configuration = new ConfigurationBuilder().SetBasePath(projectPath)
                                       .AddJsonFile(string.Format("appsettings.{0}.json", environment), optional: false, reloadOnChange: true)
                                       .Build();

                string ConvertBase64ToPdfPath = configuration["Base64ToPdf"];

                if (!Directory.Exists(ConvertBase64ToPdfPath))
                {
                    Directory.CreateDirectory(ConvertBase64ToPdfPath);
                }

                string FileName = GetApplicationNumberBaseOnClienttxnId(ClientTxnId);




                ConvertBase64ToPdfPath = ConvertBase64ToPdfPath + FileName + ".pdf";

                using (System.IO.FileStream stream = System.IO.File.Create(ConvertBase64ToPdfPath))
                {
                    System.Byte[] byteArray = System.Convert.FromBase64String(Base64Data);
                    stream.Write(byteArray, 0, byteArray.Length);
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                // Console.WriteLine("This is the first call to Dispose. Necessary clean-up will be done!");

                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    // Console.WriteLine("Explicit call: Dispose is called by the user.");
                }
                else
                {
                    // Console.WriteLine("Implicit call: Dispose is called through finalization.");
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // Console.WriteLine("Unmanaged resources are cleaned up here.");

                // TODO: set large fields to null.

                disposedValue = true;
            }
            else
            {
                // Console.WriteLine("Dispose is called more than one time. No need to clean up!");
            }
        }



        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            GC.SuppressFinalize(this);
        }


        #endregion



    }
}

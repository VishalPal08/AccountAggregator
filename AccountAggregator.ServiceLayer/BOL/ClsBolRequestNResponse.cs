using AccountAggregator.DataBaseLayer;
using AccountAggregator.InterfaceLayer;
using AccountAggregator.ModelLayer;
using AccountAggregator.ServiceLayer.Global;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AccountAggregator.ServiceLayer
{
    public class ClsBolRequestNResponse : IBOLRequestNResponse
    {
        DbConnection ConnectionManager;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        bool disposed = false;

        public ClsBolRequestNResponse()
        {
            ConnectionManager = DbConnection.SingleInstance;
        }
        ~ClsBolRequestNResponse()
        {
            Dispose(false);
        }

        public string InsertBasicNBankDetails(BOLPRequest ObjBolRequest)
        {
            try
            {

                Dictionary<string, string> _dcProposer = ReturnProposerDictionary(ObjBolRequest);

                DataTable _dtProposer = CreateDataTable(_dcProposer);

                DataTable _dtBankDeatils = new DataTable();

                foreach (BankAccountDetails ObjBA in ObjBolRequest.ObjBankAccountDetails)
                {
                    Dictionary<string, string> _dcBankDetails = ReturnBankDetailsDictionary(ObjBA);

                    _dtBankDeatils = CreateDataTable(_dcBankDetails);
                }

                DynamicParameters _params = new DynamicParameters(new
                {
                    ProposerDetails = _dtProposer.AsTableValuedParameter("dbo.UT_ProposerDetails"),
                    BankDetails = _dtBankDeatils.AsTableValuedParameter("dbo.UT_BankDetails"),
                });

                _params.Add("@BankTransactionId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                _params.Add("@ProposerId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                _params.Add("@GuidForProposer", dbType: DbType.String, direction: ParameterDirection.Output,size:500);


                using (IDbConnection cn = ConnectionManager.connection)
                {
                    cn.Open();
                    cn.Execute("SP_ProposerNBankDeatils", _params, commandType: CommandType.StoredProcedure);
                }

                string _TxtGuid = _params.Get<string>("@GuidForProposer");

                return _TxtGuid;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void InsertRequestAndResponse<T, T1>(T RequestBody, T1 ResponseBody, DateTime RequestTime, DateTime ResponseTime,int TypeId,string TxnGuidId)
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
            catch(Exception ex)
            {
                throw;
            }
        }

        private Dictionary<string, string> ReturnProposerDictionary(BOLPRequest ObjBolRequest)
        {
            try
            {
                string DateOfBirth = ObjBolRequest.ObjBasicDetail.ProposerDateOfBirth == null ? "" : ObjBolRequest.ObjBasicDetail.ProposerDateOfBirth.Value.ToString("yyyy/MM/dd");

                Dictionary<string, string> dictProposer = new Dictionary<string, string> {
                        { "ApplicationNumber", ObjBolRequest.ApplicationNumber },
                        { "SourceName", ObjBolRequest.SourceName },
                        { "policynumber", ObjBolRequest.PolicyNumber },
                        { "ProductName", ObjBolRequest.ProductName },
                        { "FirstName", ObjBolRequest.ObjBasicDetail.ProposerFirstName },
                        { "LastName", ObjBolRequest.ObjBasicDetail.ProposerLastName },
                        { "DateOfBirth", DateOfBirth },
                        { "Gender", ObjBolRequest.ObjBasicDetail.ProposerGender },
                        { "EmailId", ObjBolRequest.ObjBasicDetail.ProposerEmailId },
                        { "MobileNo", ObjBolRequest.ObjBasicDetail.ProposerMobileNo },
                        { "MaritalStatus", ObjBolRequest.ObjBasicDetail.ProposerMaritalStatus },
                        { "PANCardNo", ObjBolRequest.ObjBasicDetail.ProposerPANCardNo }
                };

                return dictProposer;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private Dictionary<string, string> ReturnBankDetailsDictionary(BankAccountDetails ObjBankDetails)
        {
            try
            {
                Dictionary<string, string> dictProposer = new Dictionary<string, string> {
                        { "AccountNumber", ObjBankDetails.AccountNumber},
                        { "AccountHolderName", ObjBankDetails.AccountHolderName },
                        { "IFSCCode", ObjBankDetails.IFSCCode},
                        { "AccountType", ObjBankDetails.AccountType },
                        { "BankName", ObjBankDetails.BankName },
                        { "BranchName", ObjBankDetails.BranchName },
                };

                return dictProposer;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private DataTable CreateDataTable(Dictionary<string, string> DynamicDicstionary)
        {
            try
            {
                DataTable _dt = new DataTable();

              //  Type dicType = DynamicDicstionary.GetType();

                foreach(var Key in DynamicDicstionary.Select(x=>x.Key))
                {
                    _dt.Columns.Add(new DataColumn(Key, typeof(string)));
                }

                DataRow _dr = _dt.NewRow();
                

                foreach (DataColumn col in _dt.Columns)
                {
                    _dr[col.ColumnName] = DynamicDicstionary.Where(x => x.Key == col.ColumnName).Select(x => x.Value).FirstOrDefault();
                }

                _dt.Rows.Add(_dr);

                return _dt;
            }
            catch (Exception ex)
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

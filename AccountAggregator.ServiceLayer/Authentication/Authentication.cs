using AccountAggregator.DataBaseLayer;
using AccountAggregator.InterfaceLayer;
using AccountAggregator.ModelLayer;
using AccountAggregator.ServiceLayer.Global;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace AccountAggregator.ServiceLayer
{
    public class Authentication : IAuthentication
    {
        DbConnection ConnectionManager;
        private static TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        bool disposed = false;

        public Authentication()
        {
            ConnectionManager = DbConnection.SingleInstance;
        }
        ~Authentication()
        {
            Dispose(false);
        }


        public ResponseAuth GetToken(string URL, RequestAuthenication ObjAuth)
        {
            try
            {
                string Response  = _RestApiCall.CallingPOSTApi<RequestAuthenication>(URL, ObjAuth,string.Empty);

                return _JsonConvert.DeSerializeObject<ResponseAuth>(Response);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public ResponseRedirectUrl GetRedirectionUrl(string URL, RequestRedirectUrl ObjRedirectUrl,string Token)
        {
            try
            {
                string Response = _RestApiCall.CallingPOSTApi<RequestRedirectUrl>(URL, ObjRedirectUrl, Token);

                return _JsonConvert.DeSerializeObject<ResponseRedirectUrl>(Response);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public string GenerateChecksum(string EncryptionObj,string SourceName)
        {
            try
            {
                string SelectQuery = "SELECT Top 1 HashKey FROM MstCheckSumKey WHERE SourceName = @SouceName Order by CheckSumId desc";

                var parameters = new Dictionary<string, object>()
                {
                    ["SouceName"] = SourceName
                };
                string HashKey = string.Empty;
                using (IDbConnection cn = ConnectionManager.connection)
                {
                    cn.Open();

                    HashKey = cn.Query<string>(SelectQuery, parameters).FirstOrDefault();
                }

                if(!string.IsNullOrEmpty(HashKey))
                {
                    byte[] key = Encoding.UTF8.GetBytes(HashKey.TrimStart());
                    byte[] bytes = Encoding.UTF8.GetBytes(EncryptionObj);
                    HMACSHA256 hashstring = new HMACSHA256(key);
                    byte[] hash = hashstring.ComputeHash(bytes);
                    return Convert.ToBase64String(hash);
                }

                return "";
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

using AccountAggregator.DataBaseLayer;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;

namespace AccountAggregator.ServiceLayer.Global
{
    public class _ClsException
    {
        DbConnection ConnectionManager;
        public _ClsException()
        {
            ConnectionManager = DbConnection.SingleInstance;
        }
        ~_ClsException()
        {
            Dispose(false);
        }
        public void InsertException(string ControllerName,string ActionName,int ErrorCode,string Exception)
        {

            string Insertquery = "Insert into [dbo].[txn_Exception](ErrorCode,ErrorMessage,Controller,Action,CurrentTimeStamp) " +
                        "values(@ErrorCode,@ErrorMessage,@Controller,@Action,@CurrentTimeStamp)";

            var parameters = new Dictionary<string, object>()
            {
                ["ErrorCode"] = ErrorCode,
                ["ErrorMessage"] = Exception,
                ["Controller"] = ControllerName,
                ["Action"] = ActionName,
                ["CurrentTimeStamp"] = DateTime.Now
            };

            using (IDbConnection cn = ConnectionManager.connection)
            {
                cn.Open();
                cn.Execute(Insertquery, parameters);
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

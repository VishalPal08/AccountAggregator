using AccountAggregator.ModelLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountAggregator.InterfaceLayer
{
    public interface IConsent
    {
        bool CheckClientTransactionIdExitOrNot(string ClientTxnId);
        void InsertRequestAndResponse<T, T1>(T RequestBody, T1 ResponseBody, DateTime RequestTime, DateTime ResponseTime, int TypeId, string TxnGuidId);
        string SaveTransactionDetails(RequestDownloadStatement ObjStatement, string JsonAccount, string PdfBase64, string PdfBinary, string XMLData, int StatementFlg);
        void ConvertBase64ToPdf(string Base64Data, string ClientTxnId);
        void Dispose();
    }
}

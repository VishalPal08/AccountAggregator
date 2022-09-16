using AccountAggregator.ModelLayer;
using System;

namespace AccountAggregator.InterfaceLayer
{
    public interface IBOLRequestNResponse
    {
        string InsertBasicNBankDetails(BOLPRequest ObjBolRequest);
        void InsertRequestAndResponse<T, T1>(T RequestBody, T1 ResponseBody, DateTime RequestTime, DateTime ResponseTime, int TypeId, string TxnGuidId);
        void Dispose();
    }
}

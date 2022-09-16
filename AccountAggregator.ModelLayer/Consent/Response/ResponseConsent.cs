using System;
using System.Collections.Generic;
using System.Text;

namespace AccountAggregator.ModelLayer
{
    public class ResponseConsent
    {
        public ResponseConsent(string clienttxnid, string timestamp, bool result, string Message)
        {
            this.clienttxnid = clienttxnid;
            this.timestamp = timestamp;
            this.result = result;
            this.Message = Message;
        }
        public string clienttxnid { get; set; }
        public string timestamp { get; set; }
        public bool result { get; set; }
        public string Message { get; set; }
    }
}

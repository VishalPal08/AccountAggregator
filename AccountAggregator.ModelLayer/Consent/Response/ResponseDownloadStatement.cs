using System;
using System.Collections.Generic;
using System.Text;

namespace AccountAggregator.ModelLayer
{
    public class ResponseDownloadStatement
    {
        public string ClientTransactionId { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AccountAggregator.ModelLayer
{
    public class ResponseBOLP
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }
        public string RedirectUrl { get; set; }
    }
}

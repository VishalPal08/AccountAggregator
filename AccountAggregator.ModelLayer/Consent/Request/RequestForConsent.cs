using System;
using System.Collections.Generic;
using System.Text;

namespace AccountAggregator.ModelLayer
{
    public class RequestForConsent
    {
        public RequestConsentStatusNotification ObjConsentStatusNotification { get; set; }
        public RequestDownloadStatement ObjDownloadStatement { get; set; }
    }
}

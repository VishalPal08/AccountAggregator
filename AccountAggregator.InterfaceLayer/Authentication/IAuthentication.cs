using AccountAggregator.ModelLayer;
using System;

namespace AccountAggregator.InterfaceLayer
{
    public interface IAuthentication
    {
        ResponseAuth GetToken(string URL, RequestAuthenication ObjAuth);
        ResponseRedirectUrl GetRedirectionUrl(string URL, RequestRedirectUrl ObjRedirectUrl, string Token);
        string GenerateChecksum(string EncryptionObj, string SourceName);
        void Dispose();
    }
}

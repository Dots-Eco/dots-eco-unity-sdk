using UnityEngine.Networking;


namespace DotsEcoCertificateSDK
{
    public class ImpactSummaryRequestBuilder : IRequestBuilder
    {
        private string appToken;
        private string companyId;
        private string remoteUserId;
        private string authToken;

        public ImpactSummaryRequestBuilder(string authToken, string companyId, string appToken, string remoteUserId)
        {
            this.authToken = authToken;
            this.companyId = appToken;
            this.companyId = companyId;
            this.remoteUserId = remoteUserId;
        }

        public UnityWebRequest BuildRequest()
        {
            UnityWebRequest request = UnityWebRequest.Get($"{Constants.ImpactSummaryPath}?company={companyId}&app_token={appToken}&user={remoteUserId}");
            request.SetRequestHeader("Content-Type", Constants.ContentType);
            request.SetRequestHeader("auth-token", authToken);

            return request;
        }
    }
}

using UnityEngine;
using UnityEngine.Networking;


namespace DotsEcoCertificateSDK
{
    public class ImpactSummaryTotalsRequestBuilder : IRequestBuilder
    {
        private string appToken;
        private string remoteUserId;
        private string authToken;

        public ImpactSummaryTotalsRequestBuilder(string authToken, string appToken, string remoteUserId)
        {
            this.authToken = authToken;
            this.appToken = appToken;
            this.remoteUserId = remoteUserId;
        }

        public UnityWebRequest BuildRequest()
        {
            UnityWebRequest request = UnityWebRequest.Get($"{Constants.ImpactSummaryTotalsPath}?app_token={appToken}&user={remoteUserId}");
            Debug.Log(request.uri);
            // UnityWebRequest request = UnityWebRequest.Get($"{Constants.ImpactSummaryPath}?company={companyId}&app_token={appToken}&user={remoteUserId}");
            request.SetRequestHeader("Content-Type", Constants.ContentType);
            request.SetRequestHeader("auth-token", authToken);

            return request;
        }
    }

    public class ImpactSummaryCompanyRequestBuilder : IRequestBuilder
    {
        private string appToken;
        private string remoteUserId;
        private string authToken;

        public ImpactSummaryCompanyRequestBuilder(string authToken, string appToken, string remoteUserId)
        {
            this.authToken = authToken;
            this.appToken = appToken;
            this.remoteUserId = remoteUserId;
        }

        public UnityWebRequest BuildRequest()
        {
            UnityWebRequest request = string.IsNullOrEmpty(remoteUserId)
                ? UnityWebRequest.Get($"{Constants.ImpactSummaryProjectPath}?app_token={appToken}")
                : UnityWebRequest.Get($"{Constants.ImpactSummaryProjectPath}?app_token={appToken}&user={remoteUserId}");
            Debug.Log(request.uri);
            // UnityWebRequest request = UnityWebRequest.Get($"{Constants.ImpactSummaryPath}?company={companyId}&app_token={appToken}&user={remoteUserId}");
            request.SetRequestHeader("Content-Type", Constants.ContentType);
            request.SetRequestHeader("auth-token", authToken);

            return request;
        }
    }
}
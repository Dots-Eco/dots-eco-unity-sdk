using System;
using UnityEngine.Networking;

namespace DotsEcoCertificateSDK
{
    public class GetCertificatesListRequestBuilder : IRequestBuilder
    {
        private string appToken;
        private string userId;

        public GetCertificatesListRequestBuilder(string appToken, string userId)
        {
            if (string.IsNullOrEmpty(appToken))
            {
                throw new ArgumentException("authToken is required!", nameof(appToken));
            }
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("userId is required!", nameof(userId));
            }

            this.appToken = appToken;
            this.userId = userId;
        }

        public UnityWebRequest BuildRequest()
        {
            string fullUrl = Constants.GetListUrlPath + appToken + "/" + userId  + "/"+ "?format=sdk";

            UnityWebRequest request = UnityWebRequest.Get(fullUrl);

            request.SetRequestHeader("Content-Type", Constants.ContentType);
            request.SetRequestHeader("app_token", appToken);

            return request;
        }
    }
}
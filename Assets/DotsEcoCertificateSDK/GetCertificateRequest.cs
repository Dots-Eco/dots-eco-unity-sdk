using System;
using UnityEngine.Networking;

namespace DotsEcoCertificateSDK
{
    public class GetCertificateRequestBuilder : IRequestBuilder
    {
        private readonly string certificateId;
        private string authToken;

        public GetCertificateRequestBuilder(string authToken, string certificateId)
        {
            if (string.IsNullOrEmpty(authToken))
            {
                throw new ArgumentException("authToken is required!", nameof(authToken));
            }
            if (string.IsNullOrEmpty(certificateId))
            {
                throw new ArgumentException("certificateId is required!", nameof(certificateId));
            }

            this.certificateId = certificateId;
            this.authToken = authToken;
        }

        public UnityWebRequest BuildRequest()
        {
            string fullUrl = string.Format(Constants.GetUrlPath, certificateId);

            UnityWebRequest request = UnityWebRequest.Get(fullUrl);

            request.SetRequestHeader("Content-Type", Constants.ContentType);
            request.SetRequestHeader("auth-token", authToken);

            return request;
        }
    }
}
using UnityEngine.Networking;
using System;

namespace DotsEcoCertificateSDK
{
    public class CertificateService
    {
        private string authToken;

        public CertificateService(string authToken)
        {
            if (string.IsNullOrEmpty(authToken))
            {
                throw new ArgumentException("authToken is required!", nameof(authToken));
            }
            this.authToken = authToken;
        }

        public UnityWebRequest SubscribeToEmailNotificationRequest(string authToken, string certificateId, string email)
        {
            var builder = new NotificationSubscriptionRequestBuilder(authToken, certificateId, email);
            return PrepareRequest(builder);
        }

        public UnityWebRequest GetCertificateRequest(string appToken, string certificateId)
        {
            GetCertificateRequestBuilder builder = new GetCertificateRequestBuilder(appToken, certificateId);
            return PrepareRequest(builder);
        }

        public UnityWebRequest GetCertificateRequest(GetCertificateRequestBuilder builder)
        {
            return PrepareRequest(builder);
        }
        
        public UnityWebRequest GetCertificatesListRequest(string appToken, string userID)
        {
            GetCertificatesListRequestBuilder builder = new GetCertificatesListRequestBuilder(appToken, userID);
            return PrepareRequest(builder);
        }
        
        public UnityWebRequest GetCertificatesListRequest(GetCertificatesListRequestBuilder builder)
        {
            return PrepareRequest(builder);
        }

        public UnityWebRequest CreateCertificateRequest(string appToken, int allocationId, int impactQty, string remoteUserId)
        {
            CreateCertificateRequestBuilder builder = new CreateCertificateRequestBuilder(appToken, allocationId, impactQty, remoteUserId);
            return PrepareRequest(builder);
        }

        public UnityWebRequest CreateCertificateRequest(CreateCertificateRequestBuilder builder)
        {
            return PrepareRequest(builder);
        }

        public UnityWebRequest CreateImpactSummaryTotalsRequest(string appToken, string remoteUserId)
        {
            ImpactSummaryTotalsRequestBuilder builder = new ImpactSummaryTotalsRequestBuilder(authToken, appToken, remoteUserId);
            return PrepareRequest(builder);
        }

        public UnityWebRequest CreateImpactSummaryProjectRequest(string appToken, string remoteUserId)
        {
            var builder = new ImpactSummaryCompanyRequestBuilder(authToken, appToken, remoteUserId);
            return PrepareRequest(builder);
        }

        private UnityWebRequest PrepareRequest(IRequestBuilder builder)
        {
            UnityWebRequest request = builder.BuildRequest();
            request.SetRequestHeader("auth-token", authToken);
            return request;
        }
    }
}
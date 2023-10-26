using System;
using UnityEngine;
using UnityEngine.Networking;

namespace DotsEcoCertificateSDK
{
    [System.Serializable]
    public class CreateCertificateRequest
    {
        public string app_token;
        public int impact_qty;
        public int allocation_id;
        public string remote_user_id;
        public string name_on_certificate;
        public string remote_user_email;
        public string certificate_design;
        public string send_certificate_by_email;
        public string certificate_info;
        public string lang_code;
        public string currency;
    }

    public class CreateCertificateRequestBuilder : IRequestBuilder
    {
        private CreateCertificateRequest requestData = new CreateCertificateRequest();

        public CreateCertificateRequestBuilder(string appToken, int allocationId, int impactQty, string remoteUserId)
        {
            if (string.IsNullOrEmpty(appToken))
            {
                throw new ArgumentException("appToken is required!", nameof(appToken));
            }
            if (allocationId < 1)
            {
                throw new ArgumentException("allocationId has to be greater than 0!", nameof(impactQty));
            }
            if (impactQty < 1)
            {
                throw new ArgumentException("impactQty has to be greater than 0!", nameof(impactQty));
            }
            if (string.IsNullOrEmpty(remoteUserId))
            {
                throw new ArgumentException("remoteUserId is required!", nameof(remoteUserId));
            }

            requestData.app_token = appToken;
            requestData.allocation_id = allocationId;
            requestData.impact_qty = impactQty;
            requestData.remote_user_id = remoteUserId;
        }

        public CreateCertificateRequestBuilder WithNameOnCertificate(string nameOnCertificate)
        {
            requestData.name_on_certificate = nameOnCertificate;
            return this;
        }

        public CreateCertificateRequestBuilder WithRemoteUserEmail(string remoteUserEmail)
        {
            requestData.remote_user_email = remoteUserEmail;
            return this;
        }

        public CreateCertificateRequestBuilder WithCertificateDesign(string certificateDesign)
        {
            requestData.certificate_design = certificateDesign;
            return this;
        }

        public CreateCertificateRequestBuilder WithSendCertificateByEmail(string sendCertificateByEmail)
        {
            requestData.send_certificate_by_email = sendCertificateByEmail;
            return this;
        }

        public CreateCertificateRequestBuilder WithCertificateInfo(string certificateInfo)
        {
            requestData.certificate_info = certificateInfo;
            return this;
        }

        public CreateCertificateRequestBuilder WithLangCode(string langCode)
        {
            requestData.lang_code = langCode;
            return this;
        }

        public CreateCertificateRequestBuilder WithCurrency(string currency)
        {
            requestData.currency = currency;
            return this;
        }

        public UnityWebRequest BuildRequest()
        {
            UnityWebRequest request = new UnityWebRequest(Constants.CreateUrlPath, UnityWebRequest.kHttpVerbPOST);
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(requestData));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", Constants.ContentType);
            return request;
        }
    }
}
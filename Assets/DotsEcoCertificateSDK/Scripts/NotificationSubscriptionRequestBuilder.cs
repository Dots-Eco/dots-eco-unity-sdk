using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace DotsEcoCertificateSDK
{
    [Serializable]
    public class EmailSubscriptionRequestBody
    {
        public string certificate_id;
        public string email;
    }
    
    public class NotificationSubscriptionRequestBuilder : IRequestBuilder
    {
        private string authToken;

        private EmailSubscriptionRequestBody emailSubscriptionRequestBody = new EmailSubscriptionRequestBody();

        public NotificationSubscriptionRequestBuilder(string authToken, string certificateId, string email)
        {
            this.authToken = authToken;
            emailSubscriptionRequestBody.certificate_id = certificateId;
            emailSubscriptionRequestBody.email = email;
        }

        public UnityWebRequest BuildRequest()
        {
            // var fullUrl = Constants.EmailNotificationPath + 

            var request = new UnityWebRequest(Constants.EmailNotificationPath, UnityWebRequest.kHttpVerbPOST);
            var bodyRaw = Encoding.UTF8.GetBytes(JsonUtility.ToJson(emailSubscriptionRequestBody));
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer(); 

            // var request = UnityWebRequest.PostWwwForm(Constants.EmailNotificationPath, JsonUtility.ToJson(emailSubscriptionRequestBody));
            request.SetRequestHeader("Content-Type", Constants.ContentType);

            return request;
        }
    }
}
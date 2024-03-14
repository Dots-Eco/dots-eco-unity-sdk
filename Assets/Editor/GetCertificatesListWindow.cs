using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace DotsEcoCertificateSDK
{
    public class GetCertificatesListWindow : EditorWindow
    {
        private string certificateId = "";
        private string logText = "";
        private Texture2D certificateImage;
        private string certificateUrl = "";
        private bool isFetchingCertificate = false;
        private bool isFetchingImage = false;
        private CertificateService _certificateService;
        
        public static void ShowWindow()
        {
            GetWindow<GetCertificatesListWindow>("Certificates list");
        }
        
        private void Awake()
        {
            string authToken = EditorPrefs.GetString("DotsEco_AuthToken");
            _certificateService = new CertificateService(authToken);
        }
        
        private void OnGUI()
        {
            EditorGUILayout.LabelField("Certificates list", EditorStyles.boldLabel);

            certificateId = EditorGUILayout.TextField("Certificate ID:", certificateId);

            EditorGUI.BeginDisabledGroup(isFetchingCertificate || isFetchingImage);
            if (GUILayout.Button(isFetchingCertificate ? "Sending..." : "Send"))
            {
                FetchCertificate();
            }
            EditorGUI.EndDisabledGroup();
        }
        
        private void FetchCertificate()
        {
            logText = "";
            isFetchingCertificate = true;
            certificateId = certificateId.Trim();

            string appToken = EditorPrefs.GetString("DotsEco_AuthToken"); // Assuming this is the correct token for the service

            if (string.IsNullOrEmpty(appToken))
            {
                logText += "Error: Authentication token is missing. Please provide it in the configuration window.\n";
                return;
            }

            if (string.IsNullOrEmpty(certificateId))
            {
                logText += "Error: Certificate ID is required.\n";
                return;
            }

            logText += $"Execcuting request using sandbox app token {appToken} and certificate id {certificateId}...\n";
            UnityWebRequest request = _certificateService.GetCertificateRequest(appToken, certificateId);
            var operation = request.SendWebRequest();

            EditorApplication.update += CheckRequestCompletion;

            void CheckRequestCompletion()
            {
                if (!operation.isDone) return;

                EditorApplication.update -= CheckRequestCompletion;

                isFetchingCertificate = false;

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    logText += "Error: " + request.error + "\nMessage" + request.downloadHandler.text;
                }
                else
                {
                    logText += $"Certificate response [{request.downloadHandler.text}]\n";

                    CertificateResponse certificate = JsonUtility.FromJson<CertificateResponse>(request.downloadHandler.text);
                    certificateUrl = certificate.certificate_url; 
                    if (!string.IsNullOrEmpty(certificate.certificate_image_url))
                    {
                        //FetchImage(Regex.Unescape(certificate.certificate_image_url));
                    }
                }
            }
        }
    }
}

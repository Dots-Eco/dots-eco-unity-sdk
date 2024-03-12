using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace DotsEcoCertificateSDK
{
    public class CreateSanboxCertificateWindow : EditorWindow
    {
        private bool isSendingRequest = false;
        private CertificateService certificateService;
        private Texture2D certificateImage;
        private string certificateUrl = "";
        private string logText = "";

        private string appToken = "";
        
        private string impactQty = "1";
        private string allocationId = "5";
        private string remoteUserId = "ecohero";
        private string nameOnCertificate = "Eco Hero";
        private string remoteUserEmail = "eco@dots.eco";
        private string certificateDesign = "silver";
        private string sendCertificateByEmail = "yes";
        private string certificateInfo = "save the planet";
        private string langCode = "EN";
        private string currency = "USD";

        private Rendering rendering;

        private Geolocation geolocation;

        private string logoUrl = "https://nginx.staging.dots-eco-drupal.de3.amazee.io/sites/default/files/logo/Group%203308_3.png";
        
        private string certificateHeader = "&lt;p&gt;For planting, 10, tree&lt;/p&gt;";
        private string greeting = "&lt;p&gt;For greeting Dots.eco, 10, Dots.eco&lt;/p&gt;";

        private string iconUrl = "https://nginx.staging.dots-eco-drupal.de3.amazee.io/sites/default/files/2024-02/tree%201.png";

        public string categoryThemeName = "Climate";
        public string categoryThemePrimary = "006B54ff";
        public string categoryThemeSecondary = "#38C961ff";
        public string categoryThemeTertiary = "#B3F281ff";
        public string categoryThemeBackground = "#F4FDEDff";
        public string categoryThemeScratchImageUrl = "https://nginx.staging.dots-eco-drupal.de3.amazee.io/sites/default/files/2024-01/scratch.svg";
        
        public static void ShowWindow()
        {
            GetWindow<CreateSanboxCertificateWindow>("Create Certificate in Sandbox");
        }

        private void Awake()
        {
            string authToken = EditorPrefs.GetString("DotsEco_AuthToken");
            certificateService = new CertificateService(authToken);
            
            appToken = EditorPrefs.GetString("DotsEco_SandboxAppToken");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Create Certificate in Sandbox", EditorStyles.boldLabel);

            EditorGUILayout.Space();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("App Token:", appToken);
            EditorGUI.EndDisabledGroup();
            
            impactQty = EditorGUILayout.TextField("Impact Quantity (required):", impactQty);
            allocationId = EditorGUILayout.TextField("Allocation ID (required):", allocationId);
            remoteUserId = EditorGUILayout.TextField("Remote User ID (required, Developers need to provide their own remote user id (For example unique user account id)):", remoteUserId);
            
            EditorGUILayout.Space();
            
            nameOnCertificate = EditorGUILayout.TextField("Name on Certificate:", nameOnCertificate);
            remoteUserEmail = EditorGUILayout.TextField("Remote User Email:", remoteUserEmail);
            certificateDesign = EditorGUILayout.TextField("Certificate Design:", certificateDesign);
            sendCertificateByEmail = EditorGUILayout.TextField("Send Certificate by Email:", sendCertificateByEmail);
            certificateInfo = EditorGUILayout.TextField("Certificate Info:", certificateInfo);
            langCode = EditorGUILayout.TextField("Language Code:", langCode);
            currency = EditorGUILayout.TextField("Currency:", currency);

            EditorGUILayout.Space();
            
            // Rendering
            // rendering -> app -> logo_url
            logoUrl = EditorGUILayout.TextField("Logo URL:", logoUrl);

            // rendering -> certificate_header
            certificateHeader = EditorGUILayout.TextField("Certificate header:", certificateHeader);
            
            // rendering -> greeting
            greeting = EditorGUILayout.TextField("Greeting:", greeting);
            
            // rendering -> theme -> impact_type_category -> icon_url
            EditorGUILayout.LabelField("Impact type category");
            iconUrl = EditorGUILayout.TextField("Icon URL:", iconUrl);
            
            // rendering -> theme -> category_theme
            EditorGUILayout.LabelField("Category theme");
            categoryThemeName = EditorGUILayout.TextField("Category theme name:", categoryThemeName);
            categoryThemePrimary = EditorGUILayout.TextField("Primary color:", categoryThemePrimary);
            categoryThemeSecondary = EditorGUILayout.TextField("Secondary color:", categoryThemeSecondary);
            categoryThemeTertiary = EditorGUILayout.TextField("Tertiary color:", categoryThemeTertiary);
            categoryThemeBackground = EditorGUILayout.TextField("Background color:", categoryThemeBackground);
            categoryThemeScratchImageUrl = EditorGUILayout.TextField("Scratch image URL:", categoryThemeScratchImageUrl);

            bool canSend = !string.IsNullOrEmpty(appToken) && !string.IsNullOrEmpty(impactQty) &&
                           !string.IsNullOrEmpty(allocationId) && !string.IsNullOrEmpty(remoteUserId);

            EditorGUI.BeginDisabledGroup(isSendingRequest);
            if (GUILayout.Button(isSendingRequest ? "Sending..." : "Send"))
            {
                SendCreateCertificateRequest();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Log:", EditorStyles.boldLabel);
            EditorGUILayout.TextArea(logText, GUILayout.Height(100));

            if (certificateImage)
            {
                GUILayout.Label(certificateImage, GUILayout.Height(400), GUILayout.ExpandWidth(true));
            }
            if (!string.IsNullOrEmpty(certificateUrl))
            {
                if (GUILayout.Button("Open Certificate in Browser"))
                {
                    Application.OpenURL(certificateUrl);
                }
            }
        }
        private void SendCreateCertificateRequest()
        {
            isSendingRequest = true;

            string appToken = EditorPrefs.GetString("DotsEco_SandboxAppToken");

            if (string.IsNullOrEmpty(appToken))
            {
                logText += "Error: Authentication token is missing. Please provide it in the configuration window.";
                return;
            }

            rendering = new Rendering(new App(logoUrl), certificateHeader, greeting,
                new Theme(new ImpactTypeCategory(iconUrl), 
                    new CategoryTheme(categoryThemeName, categoryThemePrimary, categoryThemeSecondary, 
                        categoryThemeTertiary, categoryThemeBackground, categoryThemeScratchImageUrl)));

            CreateCertificateRequestBuilder builder =
                new CreateCertificateRequestBuilder(appToken, int.Parse(allocationId), int.Parse(impactQty),
                        remoteUserId)
                    .WithNameOnCertificate(nameOnCertificate)
                    .WithRemoteUserEmail(remoteUserEmail)
                    .WithCertificateDesign(certificateDesign)
                    .WithSendCertificateByEmail(sendCertificateByEmail)
                    .WithCertificateInfo(certificateInfo)
                    .WithLangCode(langCode)
                    .WithCurrency(currency)
                    .WithGeolocation(geolocation)
                    .WithRendering(rendering);

            UnityWebRequest request = certificateService.CreateCertificateRequest(builder);
            var operation = request.SendWebRequest();

            EditorApplication.update += CheckRequestCompletion;

            void CheckRequestCompletion()
            {
                if (!operation.isDone) return;

                EditorApplication.update -= CheckRequestCompletion;

                isSendingRequest = false;

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    logText += $"Error: {request.error}, Message: [{request.downloadHandler.text}]\n";
                }
                else
                {
                    logText = request.downloadHandler.text;
                    CertificateResponse certificate = JsonUtility.FromJson<CertificateResponse>(request.downloadHandler.text);
                    certificateUrl = certificate.certificate_url;
                    PlayerPrefs.SetString(Constants.CertificateIDName, certificate.certificate_id);
                    Debug.Log(PlayerPrefs.GetString(Constants.CertificateIDName));
                    if (!string.IsNullOrEmpty(certificate.certificate_image_url))
                    {
                        FetchImage(Regex.Unescape(certificate.certificate_image_url));
                    }
                }
                Repaint();
            }
        }
        private void FetchImage(string imageUrl)
        {
            UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageUrl);
            var imageOperation = imageRequest.SendWebRequest();

            EditorApplication.update += CheckImageRequestCompletion;

            void CheckImageRequestCompletion()
            {
                if (!imageOperation.isDone) return;

                EditorApplication.update -= CheckImageRequestCompletion;

                if (imageRequest.result == UnityWebRequest.Result.ConnectionError || imageRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error loading image: " + imageRequest.error);
                }
                else
                {
                    certificateImage = DownloadHandlerTexture.GetContent(imageRequest);
                }

                Repaint();
            }
        }
    }
}
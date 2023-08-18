using System.Text.RegularExpressions;
using DotsEcoCertificateSDK;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class CreateSanboxCertificateWindow : EditorWindow
{
    private bool isSendingRequest = false;
    private CertificateService certificateService;
    private Texture2D certificateImage;
    private string jsonResponse = "";


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

    public static void ShowWindow()
    {
        GetWindow<CreateSanboxCertificateWindow>("Create Certificate in Sandbox");
    }

    private void Awake()
    {
        string authToken = EditorPrefs.GetString("DotsEco_AuthToken");
        appToken = EditorPrefs.GetString("DotsEco_SandboxAppToken");
        certificateService = new CertificateService(authToken);
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
        remoteUserId = EditorGUILayout.TextField("Remote User ID (required):", remoteUserId);
        EditorGUILayout.Space();
        nameOnCertificate = EditorGUILayout.TextField("Name on Certificate:", nameOnCertificate);
        remoteUserEmail = EditorGUILayout.TextField("Remote User Email:", remoteUserEmail);
        certificateDesign = EditorGUILayout.TextField("Certificate Design:", certificateDesign);
        sendCertificateByEmail = EditorGUILayout.TextField("Send Certificate by Email:", sendCertificateByEmail);
        certificateInfo = EditorGUILayout.TextField("Certificate Info:", certificateInfo);
        langCode = EditorGUILayout.TextField("Language Code:", langCode);
        currency = EditorGUILayout.TextField("Currency:", currency);

        bool canSend = !string.IsNullOrEmpty(appToken) && !string.IsNullOrEmpty(impactQty) &&
                       !string.IsNullOrEmpty(allocationId) && !string.IsNullOrEmpty(remoteUserId);

        EditorGUI.BeginDisabledGroup(isSendingRequest);
        if (GUILayout.Button(isSendingRequest ? "Sending..." : "Send"))
        {
            SendCreateCertificateRequest();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Response:", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(jsonResponse, GUILayout.Height(100));

        if (certificateImage)
        {
            GUILayout.Label(certificateImage, GUILayout.Height(400), GUILayout.ExpandWidth(true));
        }
    }
    private void SendCreateCertificateRequest()
    {
        isSendingRequest = true;

        string appToken = EditorPrefs.GetString("DotsEco_SandboxAppToken");

        if (string.IsNullOrEmpty(appToken))
        {
            jsonResponse = "Error: Authentication token is missing. Please provide it in the configuration window.";
            return;
        }

        CreateCertificateRequestBuilder builder = new CreateCertificateRequestBuilder(appToken, int.Parse(impactQty), int.Parse(allocationId), remoteUserId)
            .WithNameOnCertificate(nameOnCertificate)
            .WithRemoteUserEmail(remoteUserEmail)
            .WithCertificateDesign(certificateDesign)
            .WithSendCertificateByEmail(sendCertificateByEmail)
            .WithCertificateInfo(certificateInfo)
            .WithLangCode(langCode)
            .WithCurrency(currency);

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
                jsonResponse = "Error: " + request.error;
            }
            else
            {
                jsonResponse = request.downloadHandler.text;
                 CertificateResponse certificate = JsonUtility.FromJson<CertificateResponse>(request.downloadHandler.text);
                
            }

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                jsonResponse = "Error: " + request.error;
            }
            else
            {
                jsonResponse = request.downloadHandler.text;
                CertificateResponse certificate = JsonUtility.FromJson<CertificateResponse>(request.downloadHandler.text);

                if (!string.IsNullOrEmpty(certificate.certificate_image_url))
                {
                    FetchImage(Regex.Unescape(certificate.certificate_image_url));
                }
            }
            Repaint();
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

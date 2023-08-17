using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using DotsEcoCertificateSDK;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class GetCertificateWindow : EditorWindow
{
    private string certificateId = "";
    private string jsonResponse = "";
    private Texture2D certificateImage;
    private string certificateUrl = "";
    private bool isFetchingCertificate = false;
    private bool isFetchingImage = false;
    private CertificateService certificateService;

    public static void ShowWindow()
    {
        GetWindow<GetCertificateWindow>("Test Get Certificate");
    }

    private void Awake()
    {
        string authToken = EditorPrefs.GetString("DotsEco_AuthToken");
        certificateService = new CertificateService(authToken);
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Test Get Certificate", EditorStyles.boldLabel);

        certificateId = EditorGUILayout.TextField("Certificate ID:", certificateId);

        EditorGUI.BeginDisabledGroup(isFetchingCertificate || isFetchingImage);
        if (GUILayout.Button(isFetchingCertificate ? "Sending..." : "Send"))
        {
            FetchCertificate();
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Response:", EditorStyles.boldLabel);
        EditorStyles.textField.wordWrap = true;
        EditorGUILayout.TextArea(jsonResponse, GUILayout.Height(120));

        if (!string.IsNullOrEmpty(certificateUrl))
        {
            if (GUILayout.Button("Open Certificate in Browser"))
            {
                Application.OpenURL(certificateUrl);
            }
        }
        if (certificateImage)
        {
            GUILayout.Label(certificateImage, GUILayout.Height(400), GUILayout.ExpandWidth(true));
        }
    }

    private void FetchCertificate()
    {
        isFetchingCertificate = true;

        string appToken = EditorPrefs.GetString("DotsEco_AuthToken"); // Assuming this is the correct token for the service

        if (string.IsNullOrEmpty(appToken))
        {
            jsonResponse = "Error: Authentication token is missing. Please provide it in the configuration window.";
            return;
        }

        if (string.IsNullOrEmpty(certificateId))
        {
            jsonResponse = "Error: Certificate ID is required.";
            return;
        }

        UnityWebRequest request = certificateService.GetCertificateRequest(appToken, certificateId);
        var operation = request.SendWebRequest();

        EditorApplication.update += CheckRequestCompletion;

        void CheckRequestCompletion()
        {
            if (!operation.isDone) return;

            EditorApplication.update -= CheckRequestCompletion;

            isFetchingCertificate = false;

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                jsonResponse = "Error: " + request.error;
            }
            else
            {
                jsonResponse = request.downloadHandler.text;

                CertificateResponse certificate = JsonUtility.FromJson<CertificateResponse>(request.downloadHandler.text);
                certificateUrl = certificate.certificate_url; // Store the certificate URL
                if (!string.IsNullOrEmpty(certificate.certificate_image_url))
                {
                    FetchImage(Regex.Unescape(certificate.certificate_image_url));
                }
            }
        }
    }


    private void FetchImage(string imageUrl)
    {
        
        isFetchingImage = true;

        UnityWebRequest imageRequest = UnityWebRequestTexture.GetTexture(imageUrl);
        var imageOperation = imageRequest.SendWebRequest();

        EditorApplication.update += CheckImageRequestCompletion;

        void CheckImageRequestCompletion()
        {
            if (!imageOperation.isDone) return;

            EditorApplication.update -= CheckImageRequestCompletion;

            isFetchingImage = false;

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

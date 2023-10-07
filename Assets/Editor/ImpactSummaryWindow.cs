using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace DotsEcoCertificateSDK
{
    public class ImpactSummaryWindow : EditorWindow
    {
        private string appToken = "";
        private string remoteUserId = "";
        private string companyId = "";
        private string logText = "";
        private Dictionary<string, ImpactSummary> impactSummaries = new Dictionary<string, ImpactSummary>();
        private Dictionary<string, List<Texture2D>> impactImages = new Dictionary<string, List<Texture2D>>();

        private bool isFetchingCertificates = false;
        private CertificateService certificateService;

        public static void ShowWindow()
        {
            GetWindow<ImpactSummaryWindow>("Impact Summary for Application User");
        }

        private void Awake()
        {
            appToken = EditorPrefs.GetString("DotsEco_SandboxAppToken");
            if (string.IsNullOrEmpty(appToken))
            {
                throw new ArgumentException("appToken is required!", nameof(appToken));
            }
            string authToken = EditorPrefs.GetString("DotsEco_AuthToken");
            if (string.IsNullOrEmpty(authToken))
            {
                throw new ArgumentException("authToken is required!", nameof(authToken));
            }
            certificateService = new CertificateService(authToken);
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("List Certificates by User", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("App Token:", appToken);
            EditorGUI.EndDisabledGroup();
            companyId = EditorGUILayout.TextField("Company ID (required):", companyId);
            remoteUserId = EditorGUILayout.TextField("Remote User ID (required):", remoteUserId);

            EditorGUI.BeginDisabledGroup(isFetchingCertificates);
            if (GUILayout.Button(isFetchingCertificates ? "Fetching..." : "Fetch"))
            {
                FetchImpactSummary();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Log:", EditorStyles.boldLabel);
            EditorStyles.textField.wordWrap = true;
            if (!String.IsNullOrEmpty(logText))
            {
                EditorGUILayout.TextArea(logText, GUILayout.Height(120));
            }
            foreach (KeyValuePair<string, ImpactSummary> kv in impactSummaries)
            {
                ImpactSummary impactSummary = kv.Value;
                EditorGUILayout.LabelField("impact category id:", kv.Key);
                EditorGUILayout.LabelField("impact category:", impactSummary.impact_type_category_name);
                EditorGUILayout.LabelField("impact unit:", impactSummary.unit);
                EditorGUILayout.LabelField("total:", impactSummary.total);
                Texture2D turtleTexture = Resources.Load<Texture2D>("Images/nondots-turtle-16");
                if (turtleTexture != null)
                {
                    GUILayout.Label(turtleTexture, GUILayout.Height(64), GUILayout.Width(64 ), GUILayout.ExpandWidth(false));
                }
                else
                {
                    GUILayout.Label("Texture not found!");
                }

                EditorGUILayout.Space();
                
                //for (int i = 0; i < Int32.Parse(kv.Value.total); i++)
                //{
                //    if (i % 5 == 0) { Rect rowRect = EditorGUILayout.BeginHorizontal(GUILayout.Height(EditorGUIUtility.singleLineHeight)); };
                //    foreach (Texture image in impactImages[kv.Key])
                //    {
                //        GUILayout.Box(image, GUILayout.MaxHeight(16), GUILayout.MaxWidth(16));
                //    }
                //    if (i - 1 % 5 == 0) { EditorGUILayout.EndHorizontal(); };
                //}
            }
        }

        private void FetchImpactSummary()
        {
            logText = "";
            isFetchingCertificates = true;
            logText += $"Fetching impact summary for company: {companyId}, appToken: {appToken}, userId: {remoteUserId}\n";
            UnityWebRequest request = certificateService.ImpactSummaryByUserId(companyId, appToken, remoteUserId);
            var operation = request.SendWebRequest();

            EditorApplication.update += CheckRequestCompletion;

            void CheckRequestCompletion()
            {
                if (!operation.isDone) return;

                EditorApplication.update -= CheckRequestCompletion;

                isFetchingCertificates = false;

                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    logText += $"Error: {request.error}, Message: {request.downloadHandler.text}\n";
                }
                else
                {
                    logText += $"Received impact summary response: [{request.downloadHandler.text}]\n";
                    impactSummaries = JsonConvert.DeserializeObject<Dictionary<string, ImpactSummary>>(request.downloadHandler.text);
                    //foreach(KeyValuePair<string, ImpactSummary> kv in impactSummaries)
                    //{
                    //    List<Texture2D> images = new List<Texture2D>();
                    //    for(int i=0; i< Int32.Parse(kv.Value.total); i++)
                    //    {
                    //        images.Add(Resources.Load<Texture2D>("Assets/Resources/Images/nondots-turtle-16.jpeg"));
                    //    }
                    //    impactImages.TryAdd(kv.Key, images);
                    //}
                }

                Repaint();
            }
        }
    }
}

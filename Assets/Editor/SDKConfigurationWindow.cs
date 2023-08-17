using System;
using UnityEditor;
using UnityEngine;

public class SDKConfigurationWindow : EditorWindow
{
    private string authToken = "";
    private string sandboxAppToken ="";
    private string productionAppToken = "";

    private const string authTokenKey = "DotsEco_AuthToken";
    private const string sandboxAppTokenKey = "DotsEco_SandboxAppToken";
    private const string productionAppTokenKey = "DotsEco_ProductionAppToken";

    private void OnEnable()
    {
        authToken = EditorPrefs.GetString(authTokenKey, "");
        sandboxAppToken = EditorPrefs.GetString(sandboxAppTokenKey, "");
        productionAppToken = EditorPrefs.GetString(productionAppTokenKey, "");
    }

    [MenuItem("Window/Dots.eco Certificate SDK Configuration")]
    public static void ShowWindow()
    {
        GetWindow<SDKConfigurationWindow>("Dots.eco Certificate SDK Configuration");
    }

    private void OnGUI()
    {
        authToken = EditorGUILayout.PasswordField("Authentication Token:", authToken);
        EditorPrefs.SetString(authTokenKey, authToken);

        sandboxAppToken = EditorGUILayout.TextField("Sandbox App Token:", sandboxAppToken);
        EditorPrefs.SetString(sandboxAppTokenKey, sandboxAppToken);

        productionAppToken = EditorGUILayout.TextField("Production App Token:", productionAppToken);
        EditorPrefs.SetString(productionAppTokenKey, productionAppToken);


        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Testing", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        if (!String.IsNullOrEmpty(sandboxAppToken) && GUILayout.Button("Create Certificate in Sandbox"))
        {
            CreateSanboxCertificateWindow.ShowWindow();
        }

        if (GUILayout.Button("Test Get Certificate"))
        {
            GetCertificateWindow.ShowWindow();
        }


    }

    [MenuItem("Tools/DotsEco Certificate SDK Configuration Editor")]
    public static void ShowMyEditor()
    {
        EditorWindow editorWindow = GetWindow<SDKConfigurationWindow>();
        editorWindow.titleContent = new GUIContent("Dots.eco Certificate SDK Configuration Editor");
    }
}

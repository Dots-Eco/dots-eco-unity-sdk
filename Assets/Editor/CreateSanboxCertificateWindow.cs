using UnityEditor;
using UnityEngine;

public class CreateSanboxCertificateWindow : EditorWindow
{
    private string appToken = "";
    private string impactQty = "";
    private string allocationId = "";
    private string remoteUserId = "";
    private string nameOnCertificate = "";
    private string remoteUserEmail = "";
    private string certificateDesign = "";
    private string sendCertificateByEmail = "";
    private string certificateInfo = "";
    private string langCode = "";
    private string currency = "";
    private string jsonResponse = "";

    public static void ShowWindow()
    {
        GetWindow<CreateSanboxCertificateWindow>("Create Certificate in Sandbox");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Create Certificate in Sandbox", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        appToken = EditorGUILayout.TextField("App Token (required):", appToken);
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

        EditorGUI.BeginDisabledGroup(!canSend);
        if (GUILayout.Button("Send"))
        {
            // TODO: Implement the SDK call for creating certificate here.
            // For now, this is just a mockup:
            jsonResponse = "{ \"response\": \"This is a mockup JSON response.\" }";

            // When you implement the SDK call, use the resulting JSON to populate the jsonResponse variable.
        }
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Response:", EditorStyles.boldLabel);
        EditorGUILayout.TextArea(jsonResponse, GUILayout.Height(100));
    }
}

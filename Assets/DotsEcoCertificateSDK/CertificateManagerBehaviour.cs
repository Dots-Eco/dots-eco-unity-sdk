using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class CertificateManagerBehaviour : MonoBehaviour
{
    private CertificateService certificateService;

    private void Awake()
    {
        string authToken = Environment.GetEnvironmentVariable("DOTS_AUTH_TOKEN");
        if (string.IsNullOrEmpty(authToken))
        {
            throw new ArgumentException("authToken is required!", nameof(authToken));
        }
        certificateService = new CertificateService(authToken);
    }

    public void GetCertificate(string appToken, string certificateId, System.Action<CertificateResponse> onSuccess, System.Action<ErrorResponse> onError)
    {
        var request = certificateService.GetCertificateRequest(appToken, certificateId);
        StartCoroutine(Send(request, onSuccess, onError));
    }

    public void GetCertificate(GetCertificateRequestBuilder builder, System.Action<CertificateResponse> onSuccess, System.Action<ErrorResponse> onError)
    {
        var request = certificateService.GetCertificateRequest(builder);
        StartCoroutine(Send(request, onSuccess, onError));
    }

    public void CreateCertificate(string appToken, int impactQty, int allocationId, string remoteUserId, System.Action<CertificateResponse> onSuccess, System.Action<ErrorResponse> onError)
    {
        var request = certificateService.CreateCertificateRequest(appToken, impactQty, allocationId, remoteUserId);
        StartCoroutine(Send(request, onSuccess, onError));
    }

    public void CreateCertificate(CreateCertificateRequestBuilder builder, System.Action<CertificateResponse> onSuccess, System.Action<ErrorResponse> onError)
    {
        var request = certificateService.CreateCertificateRequest(builder);
        StartCoroutine(Send(request, onSuccess, onError));
    }

    private IEnumerator Send(UnityWebRequest request, System.Action<CertificateResponse> onSuccess, System.Action<ErrorResponse> onError)
    {
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
            onError?.Invoke(errorResponse);
        }
        else
        {
            CertificateResponse response = JsonUtility.FromJson<CertificateResponse>(request.downloadHandler.text);
            onSuccess?.Invoke(response);
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

namespace DotsEcoCertificateSDK
{
    public class CertificateManagerBehaviour : MonoBehaviour
    {
        public event Action<CertificateResponse[]> OnGetCertificatesListSuccess;
        public event Action OnGetCertificatesListError;

        [SerializeField] private string authToken = "";
        [SerializeField] private string appToken = "";
        [SerializeField] private string userId = "";
        
        [SerializeField] private bool showLogs = false;
        
        private CertificateService certificateService;
        private string certificateId = "";
        
        public CertificateResponse[] CertificatesArray { get; private set; }

        private void Awake()
        {
            if (string.IsNullOrEmpty(authToken))
            {
                throw new ArgumentException("authToken is required!", nameof(authToken));
            }

            certificateService = new CertificateService(authToken);
        }
        
        private void Start()
        {
            //certificateId = PlayerPrefs.GetString(Constants.CertificateIDName, "756369-430-178");

            //GetCertificatesList(appToken, userId, GetCertificatesListSuccess, GetCertificatesListError);
        }
        
        public void GetPredefinedCertificatesList()
        {
            var request = certificateService.GetCertificatesListRequest(appToken, userId);
            
            StartCoroutine(SendCertificatesListRequest(request, GetCertificatesListSuccess, 
                GetCertificatesListError));
        }
        
        private void GetCertificatesListSuccess(CertificateResponse[] certificates)
        {
            if (showLogs)
            {
                DebugCertificatesList(certificates);
            }
            
            CertificatesArray = certificates;
            OnGetCertificatesListSuccess?.Invoke(certificates);
        }
        
        private void GetCertificatesListError(ErrorResponse errorResponse)
        {
            if (showLogs)
            {
                Debug.Log("Failed to load certificates list: " + errorResponse.message);
            }
            
            OnGetCertificatesListError?.Invoke();
        }
        
        private void DebugCertificatesList(CertificateResponse[] certificates)
        {
            foreach (var certificate in certificates)
            {
                CertificateDebugger.DebugCertificateResponseAsJSONString(certificate);
            }
        }

        public void GetCertificate(string appToken, string certificateId, System.Action<CertificateResponse> onSuccess,
            System.Action<ErrorResponse> onError)
        {
            var request = certificateService.GetCertificateRequest(appToken, certificateId);
            StartCoroutine(SendSingleCertificateRequest(request, onSuccess, onError));
        }

        public void GetCertificate(GetCertificateRequestBuilder builder, System.Action<CertificateResponse> onSuccess,
            System.Action<ErrorResponse> onError)
        {
            var request = certificateService.GetCertificateRequest(builder);
            StartCoroutine(SendSingleCertificateRequest(request, onSuccess, onError));
        }

        public void GetCertificatesList(string appToken, string userID, System.Action<CertificateResponse[]> onSuccess,
            System.Action<ErrorResponse> onError)
        {
            var request = certificateService.GetCertificatesListRequest(appToken, userID);
            StartCoroutine(SendCertificatesListRequest(request, onSuccess, onError));
        }

        // TODO: Temporary for presentation only
        public void CreatePredefinedCertificate()
        {
            var request = certificateService.CreateCertificateRequest(appToken, 1, 44, userId);
            StartCoroutine(SendSingleCertificateRequest(request, null, null));
        }

        public void CreateCertificate(string appToken, int allocationId, int impactQty, string remoteUserId,
            System.Action<CertificateResponse> onSuccess, System.Action<ErrorResponse> onError)
        {
            var request = certificateService.CreateCertificateRequest(appToken, allocationId, impactQty, remoteUserId);
            StartCoroutine(SendSingleCertificateRequest(request, onSuccess, onError));
        }

        public void CreateCertificate(CreateCertificateRequestBuilder builder,
            System.Action<CertificateResponse> onSuccess, System.Action<ErrorResponse> onError)
        {
            var request = certificateService.CreateCertificateRequest(builder);
            StartCoroutine(SendSingleCertificateRequest(request, onSuccess, onError));
        }

        private IEnumerator SendSingleCertificateRequest(UnityWebRequest request, System.Action<CertificateResponse> onSuccess,
            System.Action<ErrorResponse> onError)
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                onError?.Invoke(errorResponse);
            }
            else
            {
                CertificateResponse response = JsonUtility.FromJson<CertificateResponse>(request.downloadHandler.text);

                if (showLogs) Debug.Log(response);
                
                onSuccess?.Invoke(response);
            }
        }

        private IEnumerator SendCertificatesListRequest(UnityWebRequest request, System.Action<CertificateResponse[]> onSuccess, System.Action<ErrorResponse> onError)
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                ErrorResponse errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                onError?.Invoke(errorResponse);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                CertificatesArray = JsonHelper.FromJson<CertificateResponse>("{ \"Items\": " + jsonResponse + "}");
                
                onSuccess?.Invoke(CertificatesArray);
            }
        }
    }
}
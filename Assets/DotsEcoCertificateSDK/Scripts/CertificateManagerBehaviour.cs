using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using DotsEcoCertificateSDK.Impact;
using DotsEcoCertificateSDKUtility;
using Newtonsoft.Json;
using UnityEditor.PackageManager;
using UnityEngine.UI;

namespace DotsEcoCertificateSDK 
{
    public class CertificateManagerBehaviour : MonoBehaviour
    {
        #region Singleton
        
        private static CertificateManagerBehaviour _instance;
        public static CertificateManagerBehaviour Instance => _instance ? _instance : _instance = FindInstance();
        private static CertificateManagerBehaviour FindInstance() => GameObject.FindObjectOfType<CertificateManagerBehaviour>();
        
        #endregion

        public event Action OnGetCertificatesStart;
        public event Action<CertificateResponse[]> OnGetCertificatesListSuccess;
        public event Action OnGetCertificatesListError;

        [SerializeField] private EmailContext _emailRoot;
        [SerializeField] private bool showLogs = false;
        
        private CertificateService certificateService;
        private string certificateId = "";
        private string userId = "";
        
        public CertificateResponse[] CertificatesArray { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void StartService(string userID)
        {
            if (string.IsNullOrEmpty(Configurations.Instance.TokenConfig.AuthToken))
            {
                throw new ArgumentException("authToken is required!", nameof(Configurations.Instance.TokenConfig.AuthToken));
            }

            userId = userID;
            certificateService = new CertificateService(Configurations.Instance.TokenConfig.AuthToken);
        }

        public void OpenWallet() => GetPredefinedCertificatesList();

        public void ShowEmail(string certificateId)
        {
            _emailRoot.CertificateId = certificateId;
            _emailRoot.gameObject.SetActive(true);
        }
        
        public void GetPredefinedCertificatesList()
        {
            OnGetCertificatesStart?.Invoke();
            var request = certificateService.GetCertificatesListRequest(Configurations.Instance.TokenConfig.AppToken, userId);
            
            StartCoroutine(SendCertificatesListRequest(request, GetCertificatesListSuccess, 
                GetCertificatesListError));
        }

        public void SendImpactUserRequest(Action<bool, ImpactSummaryTotalResponse> onComplete)
        {
            var request = certificateService.CreateImpactSummaryTotalsRequest(Configurations.Instance.TokenConfig.AppToken, userId);
            StartCoroutine(SendImpactTotalCoroutine(request, ImpactTotalError, onComplete));
        }

        public void SendImpactProjectRequest(Action<bool, ImpactSummaryTotalResponse> onComplete)
        {
            var request = certificateService.CreateImpactSummaryProjectRequest(Configurations.Instance.TokenConfig.AuthToken, null);
            StartCoroutine(SendImpactTotalCoroutine(request, ImpactProjectError, onComplete));
        }

        internal void SubscribeToEmailNotification(string certificateId, string email, Action<bool> onComplete)
        {
            var request = certificateService.SubscribeToEmailNotificationRequest(Configurations.Instance.TokenConfig.AuthToken, certificateId, email);
            StartCoroutine(SubscribeToEmailNotificationCoroutine(request, onComplete, SubscribeToEmailNotificationsError));
        }

        internal void PingTotals(string userId)
        {
            var request = certificateService.CreateImpactSummaryTotalsRequest(Configurations.Instance.TokenConfig.AppToken, userId);
            StartCoroutine(PingTotalsCoroutine(request));
        }

        internal void PingProject(string userId)
        {
            var request = certificateService.CreateImpactSummaryTotalsRequest(Configurations.Instance.TokenConfig.AppToken, userId);
            StartCoroutine(PingProjectCoroutine(request));
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
            GenericErrorMessage("Failed to load certificates list", errorResponse);
            OnGetCertificatesListError?.Invoke();
        }

        private void SubscribeToEmailNotificationsError(ErrorResponse errorResponse) => GenericErrorMessage("Failed to subscribe to notifications", errorResponse);

        private void ImpactTotalError(ErrorResponse errorResponse) => GenericErrorMessage("Failed to get total impact", errorResponse);

        private void ImpactProjectError(ErrorResponse errorResponse) => GenericErrorMessage("Failed to get project impact", errorResponse);

        private bool GenericErrorMessage(string header, ErrorResponse errorResponse)
        {
            if (showLogs == false)
                return false;

            Debug.Log(header + ": " + errorResponse.message);
            return true;
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
        public void CreatePredefinedCertificate(int allocationId)
        {
            var request = certificateService.CreateCertificateRequest(Configurations.Instance.TokenConfig.AppToken, allocationId, 44, userId);
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

        private IEnumerator SubscribeToEmailNotificationCoroutine(UnityWebRequest request, Action<bool> onComplete, Action<ErrorResponse> onError)
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                onError?.Invoke(errorResponse);
                onComplete?.Invoke(false);
            }
            else
            {
                string jsonResponse = request.downloadHandler.text;
                Debug.Log(jsonResponse);
                onComplete?.Invoke(true);
                // CertificatesArray = JsonHelper.FromJson<>()
            }
        }

        private IEnumerator SendImpactTotalCoroutine(UnityWebRequest request, Action<ErrorResponse> onError, Action<bool, ImpactSummaryTotalResponse> onComplete)
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                onError?.Invoke(errorResponse);
                onComplete?.Invoke(false, null);
            }
            else
            {
                var jsonResponse = "{ \"Items\": " + request.downloadHandler.text + "}";
                var response = JsonConvert.DeserializeObject<ImpactSummaryTotalResponse>(jsonResponse);
                onComplete?.Invoke(true, response);
            }
        }

        private IEnumerator SendImpactProjectCoroutine(UnityWebRequest request, Action<ErrorResponse> onError, Action<bool, ImpactSummaryProjectResponse> onComplete)
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                var errorResponse = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                onError?.Invoke(errorResponse);
                onComplete?.Invoke(false, null);
            }
            else
            {
                var jsonResponse = "{ \"Items\": " + request.downloadHandler.text + "}";
                var response = JsonConvert.DeserializeObject<ImpactSummaryProjectResponse>(jsonResponse);
                onComplete?.Invoke(true, response);
            }
        }

        private IEnumerator PingTotalsCoroutine(UnityWebRequest request)
        {
            yield return request.SendWebRequest();
            Debug.Log(request.downloadHandler.text);
            var response = JsonConvert.DeserializeObject<ImpactSummaryTotalResponse>("{ \"Items\": " + request.downloadHandler.text + "}");
        }

        private IEnumerator PingProjectCoroutine(UnityWebRequest request)
        {
            yield return request.SendWebRequest();
            Debug.Log(request.downloadHandler.text);
            var response = JsonConvert.DeserializeObject<ImpactSummaryProjectResponse>("{ \"Items\": " + request.downloadHandler.text + "}");
        }
    }
}
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Networking;

namespace DotsEcoCertificateSDK
{
    public class CertificateHandler : MonoBehaviour
    {
        public event Action<CertificateResponse> OnCertificateLoaded;
        public event Action<Texture2D> OnCertificateTextureLoaded;
        public event Action<Texture2D> OnScratchMeTextureLoaded;
        public event Action<Texture2D> OnAppLogoTextureLoaded;
        public event Action<Texture2D> OnImpactLogoTextureLoaded;
        
        [SerializeField] private CertificateManagerBehaviour certificateManagerBehaviour;

        [Header("Config")]
        [SerializeField] private string appToken = "";
        [SerializeField] private string certificateId = "";
        [SerializeField] private HyperlinksConfig hyperlinksConfig;
        
        [SerializeField] private bool showLogs = false;

        public CertificateResponse CertificateResponse { get; private set; }
        
        public Texture2D CertificateTexture { get; private set; }
        public Texture2D ScratchMeTexture { get; private set; }
        public Texture2D AppLogoTexture { get; private set; }
        public Texture2D ImpactLogoTexture { get; private set; }
        
        public HyperlinksConfig HyperlinksConfig { get => hyperlinksConfig; private set => hyperlinksConfig = value; }

        private void Start()
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
            {
                Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            }
            
            certificateManagerBehaviour.GetCertificate(appToken, certificateId, OnGetCertificateSuccess, OnGetCertificateError);
        }

        private void OnGetCertificateSuccess(CertificateResponse certificateResponse)
        {
            Debug.Log($"Certificate {certificateId} has been loaded successfully.");

            CertificateResponse = certificateResponse;
            
            OnCertificateLoaded?.Invoke(certificateResponse);
            
            StartCoroutine(LoadWebTexture(certificateResponse.certificate_image_url, CertificateTextureLoaded));
            //StartCoroutine(LoadWebTexture(certificateResponse.rendering.theme.category_theme.scratch_image_url, ScratchMeTextureLoaded)); // TODO: This is loading SVG, we can't use it right now
            StartCoroutine(LoadWebTexture(certificateResponse.certificate_image_url, ScratchMeTextureLoaded));
            StartCoroutine(LoadWebTexture(certificateResponse.rendering.app.logo_url, AppLogoTextureLoaded));
            StartCoroutine(LoadWebTexture(certificateResponse.rendering.theme.impact_type_category.icon_url, ImpactLogoTextureLoaded));

            if (showLogs)
            {
                DebugCertificate(certificateResponse);
            }
        }

        private void CertificateTextureLoaded(Texture2D texture)
        {
            CertificateTexture = texture;
            OnCertificateTextureLoaded?.Invoke(texture);
        }

        private void ScratchMeTextureLoaded(Texture2D texture)
        {
            ScratchMeTexture = texture;
            OnScratchMeTextureLoaded?.Invoke(texture);
        }
        
        private void AppLogoTextureLoaded(Texture2D texture)
        {
            AppLogoTexture = texture;
            OnAppLogoTextureLoaded?.Invoke(texture);
        }
        
        private void ImpactLogoTextureLoaded(Texture2D texture)
        {
            ImpactLogoTexture = texture;
            OnImpactLogoTextureLoaded?.Invoke(texture);
        }

        private void OnGetCertificateError(ErrorResponse errorResponse)
        {
            if (showLogs) Debug.Log("Failed to load certificate: " + errorResponse.message);
        }
        
        private IEnumerator LoadWebTexture(string url, Action<Texture2D> onTextureLoaded, Action onError = null)
        {
            url = Regex.Unescape(url);
    
            using UnityWebRequest textureRequest = UnityWebRequestTexture.GetTexture(url);
    
            yield return textureRequest.SendWebRequest();

            if (textureRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D loadedTexture = DownloadHandlerTexture.GetContent(textureRequest);
                
                onTextureLoaded?.Invoke(loadedTexture);
        
                if (showLogs) Debug.Log("Texture from web loaded");
            }
            else
            {
                onError?.Invoke();
                
                if (showLogs) Debug.LogError("Failed to load web texture: " + textureRequest.error);
            }
        }

        private static void DebugCertificate(CertificateResponse certificateResponse)
        {
            Debug.Log("certificate_id: " + certificateResponse.certificate_id);
            Debug.Log("certificate_url: " + certificateResponse.certificate_url);
            Debug.Log("certificate_image_url: " + certificateResponse.certificate_image_url);
            Debug.Log("app_id: " + certificateResponse.app_id);
            Debug.Log("app_name: " + certificateResponse.app_name);
            Debug.Log("remote_user_id: " + certificateResponse.remote_user_id);
            Debug.Log("name_on_certificate: " + certificateResponse.name_on_certificate);
            Debug.Log("certificate_design: " + certificateResponse.certificate_design);
            Debug.Log("certificate_info: " + certificateResponse.certificate_info);
            Debug.Log("impact_qty: " + certificateResponse.impact_qty);
            Debug.Log("impact_type_id: " + certificateResponse.impact_type_id);
            Debug.Log("impact_type_name: " + certificateResponse.impact_type_name);
            Debug.Log("impact_status: " + certificateResponse.impact_status);
            Debug.Log("created_timestamp: " + certificateResponse.created_timestamp);
            Debug.Log("allocation_id: " + certificateResponse.allocation_id);
            Debug.Log("country: " + certificateResponse.country);
            
            Debug.Log("geolocation: " + certificateResponse.geolocation[0].lat);
            Debug.Log("geolocation: " + certificateResponse.geolocation[0].lng);
            
            Debug.Log("App logo url: " + certificateResponse.rendering.app.logo_url);
            
            Debug.Log("impact title: " + certificateResponse.rendering.impact_title);
            
            Debug.Log("impact type category: " + certificateResponse.rendering.theme.impact_type_category.icon_url);
            
            Debug.Log("theme name: " + certificateResponse.rendering.theme.category_theme.name);
            Debug.Log("primary color: " + certificateResponse.rendering.theme.category_theme.primary);
            Debug.Log("secondary color: " + certificateResponse.rendering.theme.category_theme.secondary);
            Debug.Log("tertiary color: " + certificateResponse.rendering.theme.category_theme.tertiary);
            Debug.Log("background color: " + certificateResponse.rendering.theme.category_theme.background);
            Debug.Log("scratch image url: " + certificateResponse.rendering.theme.category_theme.scratch_image_url);

            for (int i = 0; i < certificateResponse.geolocation.Count; i++)
            {
                Debug.Log($"Geolocation point {i}: lat: {certificateResponse.geolocation[i].lat}, " +
                    $"lng: {certificateResponse.geolocation[i].lng}");
            }
        }
    }
}
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using ScratchCardAsset;

namespace DotsEcoCertificateSDK
{
    public class CertificateHandler : MonoBehaviour
    {
        public event Action<CertificateResponse> OnCertificateLoaded;
        public event Action<Texture2D> OnCertificateTextureLoaded;
        public event Action<Texture2D> OnLocationTextureLoaded;
        public event Action<Texture2D> OnScratchMeTextureLoaded;
        public event Action<Texture2D> OnAppLogoTextureLoaded;
        public event Action<Texture2D> OnImpactLogoTextureLoaded;
        
        [SerializeField] private CertificateManagerBehaviour certificateManagerBehaviour;
        
        [SerializeField] private HyperlinksConfig hyperlinksConfig;
        
        [SerializeField] private ScratchCardManager scratchCardManager;

        [SerializeField] private int certificateIndex = 0;
        
        [SerializeField] private bool showLogs = false;

        public CertificateResponse CurrentCertificateResponse { get; private set; }
        public Texture2D CertificateTexture { get; private set; }
        public Texture2D LocationTexture { get; private set; }
        public Texture2D ScratchMeTexture { get; private set; }
        public Texture2D AppLogoTexture { get; private set; }
        public Texture2D ImpactLogoTexture { get; private set; }
        
        public HyperlinksConfig HyperlinksConfig { get => hyperlinksConfig; private set => hyperlinksConfig = value; }
        
        private void Awake()
        {
            certificateManagerBehaviour.OnGetCertificatesListSuccess += SetupCertificateWithIndex;
        }

        public void SetupCertificate(CertificateResponse certificate)
        {
            CurrentCertificateResponse = certificate;
            LoadCertificateImages(CurrentCertificateResponse);
            
            scratchCardManager.ClearScratchCard();
            
            OnCertificateLoaded?.Invoke(CurrentCertificateResponse);
        }

        private void SetupCertificateWithIndex(CertificateResponse[] certificates)
        {
            CurrentCertificateResponse = certificates[certificateIndex];
            LoadCertificateImages(CurrentCertificateResponse);
            
            scratchCardManager.ClearScratchCard();
            
            OnCertificateLoaded?.Invoke(CurrentCertificateResponse);
        }

        private void LoadCertificateImages(CertificateResponse certificateResponse)
        {
            if (CurrentCertificateResponse == null) return;
            
            string certificateImageUrl = CurrentCertificateResponse.certificate_image_url;
            if (certificateImageUrl != "")
            {                
                StartCoroutine(LoadWebTexture(CurrentCertificateResponse.certificate_image_url, CertificateTextureLoaded));
            }
            
            // TODO: This is loading SVG, we can't use it right now
            //StartCoroutine(LoadWebTexture(CurrentCertificateResponse.rendering.theme.category_theme.scratch_image_url, ScratchMeTextureLoaded));
            
            string appLogoUrl = CurrentCertificateResponse.rendering.app.logo_url;
            if (appLogoUrl != "")
            {
                StartCoroutine(LoadWebTexture(appLogoUrl, AppLogoTextureLoaded));
            }

            string locationIconUrl = CurrentCertificateResponse.rendering.project.location_image_url;
            if (locationIconUrl != "")
            {
                StartCoroutine(LoadWebTexture(locationIconUrl, LocationTextureLoaded));
            }

            string impactIconUrl = CurrentCertificateResponse.rendering.theme.impact_type_category.icon_url;
            if (impactIconUrl != "")
            {
                StartCoroutine(LoadWebTexture(impactIconUrl, ImpactLogoTextureLoaded));
            }
        }

        private void CertificateTextureLoaded(Texture2D texture)
        {            
            if (showLogs) Debug.Log("CertificateTextureLoaded");
            CertificateTexture = texture;
            OnCertificateTextureLoaded?.Invoke(texture);
        }
        
        private void LocationTextureLoaded(Texture2D texture)
        {
            if (showLogs) Debug.Log("LocationTextureLoaded");
            LocationTexture = texture;
            OnLocationTextureLoaded?.Invoke(texture);
        }

        private void ScratchMeTextureLoaded(Texture2D texture)
        {
            if (showLogs) Debug.Log("ScratchMeTextureLoaded");
            ScratchMeTexture = texture;
            OnScratchMeTextureLoaded?.Invoke(texture);
        }
        
        private void AppLogoTextureLoaded(Texture2D texture)
        {
            if (showLogs) Debug.Log("AppLogoTextureLoaded");
            AppLogoTexture = texture;
            OnAppLogoTextureLoaded?.Invoke(texture);
        }
        
        private void ImpactLogoTextureLoaded(Texture2D texture)
        {
            if (showLogs) Debug.Log("ImpactLogoTextureLoaded");
            ImpactLogoTexture = texture;
            OnImpactLogoTextureLoaded?.Invoke(texture);
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
    }
}
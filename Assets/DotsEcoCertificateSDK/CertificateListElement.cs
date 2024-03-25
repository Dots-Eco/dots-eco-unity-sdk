using System;
using System.Collections;
using System.Net;
using System.Text.RegularExpressions;
using DotsEcoCertificateSDKUtility;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace DotsEcoCertificateSDK
{
    public class CertificateListElement : MonoBehaviour
    {
        private const string DOTS_ECO_CERTIFICATE_ID_TEXT = "Certificate #";
        private const string COUNTRY_PREFIX_TEXT = "in ";

        [SerializeField] private Image certificateImage;
        
        [SerializeField] private TextMeshProUGUI certificateIdText;
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI countryText;
        
        [SerializeField] private Button viewButton;
        [SerializeField] private ShareButton shareButton;
        
        [SerializeField] private Image appIconImage;
        
        [SerializeField] private bool showLogs = false;
        
        private CertificateHandler _certificateHandler;
        private CanvasGroup _listCanvasGroup;
        private CanvasGroup _mainViewCanvasGroup;
        private CertificateResponse _certificateResponse;

        public void Setup(CertificateResponse certificateResponse, CertificateHandler certificateHandler, CanvasGroup listCanvasGroup, CanvasGroup mainViewCanvasGroup)
        {
            _certificateResponse = certificateResponse;
            
            _certificateHandler = certificateHandler;
            
            _listCanvasGroup = listCanvasGroup;
            _mainViewCanvasGroup = mainViewCanvasGroup;
            
            certificateIdText.text = DOTS_ECO_CERTIFICATE_ID_TEXT + _certificateResponse.certificate_id;
            
            string certificateHeader = certificateResponse.rendering.certificate_header;
            string cleanedCertificateHeader = WebUtilityDotsEco.ParseHTMLString(certificateHeader);
            titleText.text = cleanedCertificateHeader;
            
            countryText.text = COUNTRY_PREFIX_TEXT + _certificateResponse.country;
            shareButton.Link = _certificateResponse.certificate_url;
            
            StartCoroutine(LoadWebTexture(certificateResponse.rendering.app.logo_url, AppLogoTextureLoaded));
            
            viewButton.onClick.AddListener(SetupViewButton);
        }

        private void SetupViewButton()
        {
            _certificateHandler.SetupCertificate(_certificateResponse);
            _listCanvasGroup.Hide();
            _mainViewCanvasGroup.Show();
        }
        
        private void AppLogoTextureLoaded(Texture2D texture)
        {
            appIconImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
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

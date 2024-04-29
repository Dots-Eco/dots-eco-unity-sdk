using System.Net;
using DotsEcoCertificateSDKUtility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace DotsEcoCertificateSDK
{
    public class CertificateView : MonoBehaviour
    {
        private const string DOTS_ECO_CERTIFICATE_ID_TEXT = "Dots.eco certificate: ";
        private const string THANK_YOU_TEXT = "Thank You ";

        [SerializeField] private CertificateHandler certificateHandler;
        
        [SerializeField] private CanvasGroup certificateViewCanvasGroup;
        [SerializeField] private CanvasGroup certificatesListCanvasGroup;
        
        [Header("Components")]
        [SerializeField] private Image mainContentBackgroundImage;
        
        [Header("Top")] 
        [SerializeField] private TextMeshProUGUI certificateIDText;
        [SerializeField] private Button certificateIDLinkButton;
        [SerializeField] private Button downloadCertificateButton;
        [SerializeField] private TextMeshProUGUI certificateTitleText;

        [Header("Middle")] 
        [SerializeField] private Image middleBackgroundImage;
        [SerializeField] private Image impactIconBackgroundImage;
        [SerializeField] private Image impactIconImage;
        [SerializeField] private TextMeshProUGUI thankYouText;

        [Header("ScratchArea")] 
        [SerializeField] private Image scratchMeImage;

        [SerializeField] private Image locationImage;
        [SerializeField] private TextMeshProUGUI impactLocationText;

        [Header("Bottom")] 
        [SerializeField] private ShareButton shareButton;
        [SerializeField] private Image shareButtonImage;
        [SerializeField] private Image companyLogoImage;

        [Header("Footer/Links")] 
        [SerializeField] private Button impactInformationLinkButton;
        [SerializeField] private Button termsOfUseLinkButton;
        [SerializeField] private Button privacyPolicyLinkButton;


        private void Awake()
        {
            certificateHandler.OnCertificateLoaded += OnCertificateLoaded;
            certificateHandler.OnCertificateTextureLoaded += OnCertificateTextureLoaded;
            certificateHandler.OnLocationTextureLoaded += OnLocationTextureLoaded;
            certificateHandler.OnScratchMeTextureLoaded += OnScratchMeTextureLoaded;
            certificateHandler.OnAppLogoTextureLoaded += OnAppLogoTextureLoaded;
            certificateHandler.OnImpactLogoTextureLoaded += OnImpactLogoTextureLoaded;
        }

        private void Start()
        {
            SetupButtons();            
        }

        private void SetupButtons()
        {
            impactInformationLinkButton.onClick.AddListener(() => Application.OpenURL(certificateHandler.HyperlinksConfig.ImpactInformation));
            termsOfUseLinkButton.onClick.AddListener(() => Application.OpenURL(certificateHandler.HyperlinksConfig.TermsOfUse));
            privacyPolicyLinkButton.onClick.AddListener(() => Application.OpenURL(certificateHandler.HyperlinksConfig.PrivacyPolice));
        }
        
        public void Hide() 
        {
            CertificateManagerBehaviour.Instance.GetPredefinedCertificatesList();
            certificateViewCanvasGroup.Hide();
        }

        private void OnCertificateLoaded(CertificateResponse certificateResponse)
        {
            if (certificateResponse.rendering.theme != null)
            {
                ColorUtility.TryParseHtmlString(certificateResponse.rendering.theme.category_theme.primary, out Color primaryColor);
                ColorUtility.TryParseHtmlString(certificateResponse.rendering.theme.category_theme.secondary, out Color secondaryColor);
                ColorUtility.TryParseHtmlString(certificateResponse.rendering.theme.category_theme.background, out Color backgroundColor);   
                
                mainContentBackgroundImage.color = backgroundColor;
                middleBackgroundImage.color = secondaryColor;
                
                thankYouText.color = primaryColor;
                
                shareButtonImage.color = primaryColor;

                certificateTitleText.color = primaryColor;
            }
            else
            {
                Debug.LogWarning("Warning: Certificate has no rendering theme");
            }

            certificateIDText.text = $"{DOTS_ECO_CERTIFICATE_ID_TEXT}<u>{certificateResponse.certificate_id}</u>";
            
            string certificateHeader = certificateResponse.rendering.certificate_header;
            string cleanedCertificateHeader = WebUtilityDotsEco.ParseHTMLString(certificateHeader);
            certificateTitleText.text = cleanedCertificateHeader;
            
            thankYouText.text = THANK_YOU_TEXT + certificateResponse.name_on_certificate + "!";
            
            impactLocationText.text = certificateResponse.country;

            // TODO: Rework to open subscribe window when ready
            //certificateIDLinkButton.onClick.AddListener(() => Application.OpenURL(certificateResponse.certificate_url));
            shareButton.Link = certificateResponse.certificate_url;
        }

        private void OnCertificateTextureLoaded(Texture2D texture)
        {
            //Debug.Log("Certificate texture loaded");
            downloadCertificateButton.onClick.AddListener(() =>
            {
                DeviceUtility.SaveCertificateImageToDevice(texture,
                    certificateHandler.CurrentCertificateResponse.certificate_id);
                //Debug.Log("Certificate image saved");
            });
        }
        
        private void OnLocationTextureLoaded(Texture2D texture)
        {
            locationImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }

        private void OnScratchMeTextureLoaded(Texture2D texture)
        {
            // TODO: Implement when ready (we need scratchme image from certificate response)
            //scratchMeImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }

        private void OnAppLogoTextureLoaded(Texture2D texture)
        {
            companyLogoImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }

        private void OnImpactLogoTextureLoaded(Texture2D texture)
        {
            impactIconImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }
}

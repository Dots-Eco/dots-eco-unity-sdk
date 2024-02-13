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

        [Header("Top")] 
        [SerializeField] private TextMeshProUGUI certificateIDText;
        [SerializeField] private Button certificateIDLinkButton;
        [SerializeField] private Button downloadCertificateButton;
        [SerializeField] private TextMeshProUGUI certificateTitleText;
        [SerializeField] private Button closeButton;

        [SerializeField] private Image mainContentBackgroundImage;

        [Header("Middle")] 
        [SerializeField] private Image impactIconBackgroundImage;
        [SerializeField] private Image impactIconImage;
        [SerializeField] private TextMeshProUGUI thankYouText;

        [SerializeField] private Image middleBackgroundImage;

        [Header("ScratchArea")] 
        [SerializeField] private Image scratchMeImage;

        [SerializeField] private Image locationImage;
        [SerializeField] private TextMeshProUGUI impactLocationText;

        [Header("Bottom")] [SerializeField] 
        private ShareButton shareButton;
        [SerializeField] private Image companyLogoImage;

        [Header("Footer/Links")] 
        [SerializeField] private Button impactInformationLinkButton;
        [SerializeField] private Button termsOfUseLinkButton;
        [SerializeField] private Button privacyPolicyLinkButton;

        private void Awake()
        {
            certificateHandler.OnCertificateLoaded += OnCertificateLoaded;
            certificateHandler.OnCertificateTextureLoaded += OnCertificateTextureLoaded;
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

        private void OnCertificateLoaded(CertificateResponse certificateResponse)
        {
            certificateIDText.text = $"{DOTS_ECO_CERTIFICATE_ID_TEXT}<u>{certificateResponse.certificate_id}</u>";
            thankYouText.text = THANK_YOU_TEXT + certificateResponse.name_on_certificate;
            
            string titleText = certificateResponse.rendering.impact_title;
            string replacedString = titleText.Replace("with", "\nwith");
            certificateTitleText.text = replacedString;
            
            impactLocationText.text = certificateResponse.country;

            certificateIDLinkButton.onClick.AddListener(() => Application.OpenURL(certificateResponse.certificate_url));
            shareButton.Link = certificateResponse.certificate_url;

            ColorUtility.TryParseHtmlString(certificateResponse.rendering.theme.category_theme.primary, out Color mainContentColor);
            mainContentBackgroundImage.color = mainContentColor;
            
            ColorUtility.TryParseHtmlString(certificateResponse.rendering.theme.category_theme.primary, out Color middleBackgroundColor);
            middleBackgroundImage.color = middleBackgroundColor;
        }

        private void OnCertificateTextureLoaded(Texture2D texture)
        {
            downloadCertificateButton.onClick.AddListener(() => DeviceUtility.SaveCertificateImageToDevice(texture,
                    certificateHandler.CertificateResponse.certificate_id));
        }

        private void OnScratchMeTextureLoaded(Texture2D texture)
        {
            // TODO: Implement when ready
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

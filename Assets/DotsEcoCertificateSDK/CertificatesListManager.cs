using DotsEcoCertificateSDKUtility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DotsEcoCertificateSDK
{
    public class CertificatesListManager : MonoBehaviour
    {
        private const string CERTIFICATES_COUNT_TEXT = " certificates";
        
        [SerializeField] private CertificateListElement certificateListElementPrefab;
        [SerializeField] private CertificateManagerBehaviour certificateManagerBehaviour;
        [SerializeField] private CertificateHandler certificateHandler;
        
        [Header("Components")]
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI certificatesCountText;
        [SerializeField] private RectTransform listContainer;
        
        [Header("Canvas groups")]
        [SerializeField] private CanvasGroup listCanvasGroup;
        [SerializeField] private CanvasGroup mainViewCanvasGroup;

        private void Awake()
        {
            certificateManagerBehaviour.OnGetCertificatesListSuccess += SetupCertificatesList;
            
            closeButton.onClick.AddListener(Close);
        }
        
        public void Open()
        {
            listCanvasGroup.Show();
        }

        public void Close()
        {
            listCanvasGroup.Hide();
        }

        private void SetupCertificatesList(CertificateResponse[] certificates)
        {
            certificatesCountText.text = certificates.Length + CERTIFICATES_COUNT_TEXT;
            
            foreach (CertificateResponse certificate in certificates)
            {
                Instantiate(certificateListElementPrefab, listContainer)
                    .Setup(certificate, certificateHandler, listCanvasGroup, mainViewCanvasGroup);
            }
        }
    }
}

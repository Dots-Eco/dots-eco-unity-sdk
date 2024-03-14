using TMPro;
using UnityEngine;

namespace DotsEcoCertificateSDK
{
    public class CertificatesListManager : MonoBehaviour
    {
        private const string CERTIFICATES_COUNT_TEXT = " certificates";
        
        [SerializeField] private CertificateManagerBehaviour certificateManagerBehaviour;
        
        [SerializeField] private CertificateListElement certificateListElementPrefab;
        
        [SerializeField] private TextMeshProUGUI certificatesCountText;
        [SerializeField] private RectTransform listContainer;
        
        [SerializeField] private CanvasGroup listCanvasGroup;
        [SerializeField] private CanvasGroup mainViewCanvasGroup;

        private void Awake()
        {
            certificateManagerBehaviour.OnGetCertificatesListSuccess += SetupCertificatesList;
        }

        private void SetupCertificatesList(CertificateResponse[] certificates)
        {
            certificatesCountText.text = certificates.Length + CERTIFICATES_COUNT_TEXT;
            
            foreach (CertificateResponse certificate in certificates)
            {
                Instantiate(certificateListElementPrefab, listContainer).Setup(certificate, listCanvasGroup, mainViewCanvasGroup);
            }
        }
    }
}

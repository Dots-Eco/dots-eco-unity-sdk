using System.Collections.Generic;
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
            certificateManagerBehaviour.OnGetCertificatesListError += OnGetCertificatesListError;
            
            closeButton.onClick.AddListener(Close);
        }
        
        public void Open()
        {
            certificateManagerBehaviour.GetPredefinedCertificatesList();
            listCanvasGroup.Show();
        }

        public void Close()
        {
            listCanvasGroup.Hide();
        }

        private void SetupCertificatesList(CertificateResponse[] certificates)
        {
            foreach (Transform child in listContainer)
            {
                Destroy(child.gameObject);
            }
            
            certificatesCountText.text = certificates.Length + CERTIFICATES_COUNT_TEXT;
            
            foreach (CertificateResponse certificate in certificates)
            {
                var certificateElement = Instantiate(certificateListElementPrefab, listContainer);
                certificateElement.Setup(certificate, certificateHandler, 
                    listCanvasGroup, mainViewCanvasGroup);
            }
        }

        private void OnGetCertificatesListError()
        {
            certificatesCountText.text = "0" + CERTIFICATES_COUNT_TEXT;
        }
    }
}

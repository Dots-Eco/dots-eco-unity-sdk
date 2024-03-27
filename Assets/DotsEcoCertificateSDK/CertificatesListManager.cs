using System;
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
        
        public event Action<CertificateResponse[]> OnListRetrieved;
        
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
        
        public CertificateResponse[] CertificateResponses { get; private set; }

        private void Awake()
        {
            certificateManagerBehaviour.OnGetCertificatesListSuccess += OnGetCertificatesListSuccess;
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

        private void OnGetCertificatesListSuccess(CertificateResponse[] certificates)
        {
            CertificateResponses = certificates;
            
            OnListRetrieved?.Invoke(certificates);
            
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

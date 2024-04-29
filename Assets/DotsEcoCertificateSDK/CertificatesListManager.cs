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
            certificateManagerBehaviour.OnGetCertificatesStart += OnGetCertificatesStart;
            
            // closeButton.onClick.AddListener(Close);
        }
        
        public void Open()
        {
            // certificateManagerBehaviour.GetPredefinedCertificatesList();
            listCanvasGroup.Show();
        }

        public void Close()
        {
            // certificateManagerBehaviour.Hide();
            listCanvasGroup.Hide();
        }

        private void OnGetCertificatesStart()
        {
            // for (int i = 0; i < listContainer.transform.childCount; i++)
            // {
            //     Destroy(listContainer.transform.GetChild(i).gameObject);
            // }
            listCanvasGroup.Show();
        }

        private void OnGetCertificatesListSuccess(CertificateResponse[] certificates)
        {
            CertificateResponses = certificates;
            
            OnListRetrieved?.Invoke(certificates);
            
            // foreach (Transform child in listContainer)
            // {
            //     Destroy(child.gameObject);
            // }
            
            for (int i = 0; i < listContainer.transform.childCount; i++)
            {
                Destroy(listContainer.transform.GetChild(i).gameObject);
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

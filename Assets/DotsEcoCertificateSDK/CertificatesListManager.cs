using System;
using System.Collections.Generic;
using DotsEcoCertificateSDK.Impact;
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
        
        [Header("Impacts")] 
        [SerializeField] private GameObject overlayHeader;
        [SerializeField] private GameObject overlayHeader2;
        [SerializeField] private ImpactRow userImpactRow;
        [SerializeField] private ImpactRow userImpactRow2;
        [SerializeField] private ImpactRow projectImpactRow;
        [SerializeField] private ImpactRow projectImpactRow2;
        
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
            
            certificateManagerBehaviour.SendImpactUserRequest(OnUserRequestSuccess);
            certificateManagerBehaviour.SendImpactUserRequest(OnCompanyRequestSuccess);
        }

        private void OnUserRequestSuccess(bool isSuccess, ImpactSummaryTotalResponse response)
        {
            userImpactRow.Setup(response);
            userImpactRow2.Setup(response);
        }

        private void OnCompanyRequestSuccess(bool isSuccess, ImpactSummaryTotalResponse response)
        {
            projectImpactRow.Setup(response);
            projectImpactRow2.Setup(response);
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
            
            if (overlayHeader)
                overlayHeader.SetActive(certificates.Length > 0);
            if (overlayHeader2)
                overlayHeader2.SetActive(certificates.Length > 0);
        }

        private void OnGetCertificatesListError()
        {
            certificatesCountText.text = "0" + CERTIFICATES_COUNT_TEXT;
        }
    }
}

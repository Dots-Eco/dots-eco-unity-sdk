using DotsEcoCertificateSDK;
using UnityEngine;

namespace ScratchCardAsset
{
    public class ScratchCardAutofill : MonoBehaviour
    {
        [SerializeField] private ScratchCardManager scratchCardManager;
        
        [SerializeField, Range(0f, 1f)] private float autofillThreshold = 0.65f;
        
        [SerializeField] private CertificateHandler certificateHandler;

        private void OnEnable()
        {
            scratchCardManager.Progress.OnProgress += OnScratchProgress;
            certificateHandler.OnCertificateLoaded += OnCertificateLoaded;
        }

        private void OnDisable()
        {
            scratchCardManager.Progress.OnProgress -= OnScratchProgress;
            certificateHandler.OnCertificateLoaded -= OnCertificateLoaded;
        }

        private void OnScratchProgress(float progress)
        {
            if (progress >= autofillThreshold)
            {
                scratchCardManager.Card.Fill();
            }
        }
        
        private void OnCertificateLoaded(CertificateResponse certificateResponse)
        {
            
        }
    }
}

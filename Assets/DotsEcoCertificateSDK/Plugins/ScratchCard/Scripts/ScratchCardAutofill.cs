using DotsEcoCertificateSDK;
using DotsEcoCertificateSDK.Scripts.Utility;
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
                DotsEcoPlayerPrefsHelper.AddScratchedCertificate(certificateHandler.CurrentCertificateResponse.certificate_id);
            }
        }
        
        private void OnCertificateLoaded(CertificateResponse certificateResponse)
        {
            var isScratched = DotsEcoPlayerPrefsHelper.IsScratched(certificateResponse.certificate_id);
            if (isScratched)
            {
                scratchCardManager.Card.Fill();
            }
        }
    }
}
using System;
using UnityEngine;

namespace DotsEcoCertificateSDK
{
    [CreateAssetMenu(fileName = "PlayerPrefs Config (Sample)", menuName = "DotsEco/PlayerPrefsConfig")]
    public class PlayerPrefsConfig : ScriptableObject
    {
        private const string DEFAULT_VALUE_SCRATCHED_CERTIFICATES_KEY = "dots-eco.scratched-certificates";
        
        [SerializeField] private string _scratchedCertificatesKey = DEFAULT_VALUE_SCRATCHED_CERTIFICATES_KEY;

        public string ScratchedCertificatesKey => _scratchedCertificatesKey;

        #if UNITY_EDITOR
        private void Reset()
        {
            _scratchedCertificatesKey = DEFAULT_VALUE_SCRATCHED_CERTIFICATES_KEY;
        }
        #endif
    }
}
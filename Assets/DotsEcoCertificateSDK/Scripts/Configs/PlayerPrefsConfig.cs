using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace DotsEcoCertificateSDK
{
    [CreateAssetMenu(fileName = "PlayerPrefs Config (Sample)", menuName = "DotsEco/PlayerPrefsConfig")]
    public class PlayerPrefsConfig : ScriptableObject
    {
        private const string DEFAULT_VALUE_SCRATCHED_CERTIFICATES_KEY = "dots-eco.scratched-certificates";
        [Header("Change PlayerPrefs keys if they conflict with keys in your project")][Space]
        [SerializeField] private string _playerPrefsKeyScratched = DEFAULT_VALUE_SCRATCHED_CERTIFICATES_KEY;

        public string PlayerPrefsKeyScratched => _playerPrefsKeyScratched;

        #if UNITY_EDITOR
        private void Reset()
        {
            _playerPrefsKeyScratched = DEFAULT_VALUE_SCRATCHED_CERTIFICATES_KEY;
        }
        #endif
    }
}
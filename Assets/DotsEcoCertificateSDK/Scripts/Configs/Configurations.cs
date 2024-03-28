using UnityEngine;

namespace DotsEcoCertificateSDK
{
    internal class Configurations : MonoBehaviour
    {
        #region Singleton

        private static Configurations _instance;
        public static Configurations Instance => _instance ? _instance : _instance = FindConfigurationInstance();
        private static Configurations FindConfigurationInstance() => FindObjectOfType<Configurations>();

        #endregion

        [SerializeField] private HyperlinksConfig _hyperlinksConfig;
        [SerializeField] private PlayerPrefsConfig _playerPrefsConfig;

        public HyperlinksConfig HyperlinksConfig => _hyperlinksConfig;
        public PlayerPrefsConfig PlayerPrefsConfig => _playerPrefsConfig;
    }
}
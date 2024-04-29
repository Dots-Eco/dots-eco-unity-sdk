using UnityEngine;

namespace DotsEcoCertificateSDK
{
    [CreateAssetMenu(fileName = "Token Config (Sample)", menuName = "DotsEco/TokenConfig")]
    public class TokenConfig : ScriptableObject
    {
        [SerializeField] private string _appToken;
        [SerializeField] private string _authToken;

        public string AppToken => _appToken;
        public string AuthToken => _authToken;
    }
}
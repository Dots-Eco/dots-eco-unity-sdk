using UnityEngine;

namespace DotsEcoCertificateSDK
{
    [CreateAssetMenu(fileName = "DotsEcoHyperlinksConfig", menuName = "DotsEco/HyperlinksConfig", order = 0)]
    public class HyperlinksConfig : ScriptableObject
    {
        public string ImpactInformation = "https://dots.eco/impact-information/";
        public string TermsOfUse = "https://dots.eco/terms-and-conditions/";
        public string PrivacyPolice = "https://dots.eco/privacy-policy-2/";
    }
}
using UnityEngine;

namespace DotsEcoCertificateSDK
{
    public class EmailContext : MonoBehaviour
    {
        private string _certificateId;

        public string CertificateId
        {
            get => _certificateId;
            set => _certificateId = value;
        }
    }
}
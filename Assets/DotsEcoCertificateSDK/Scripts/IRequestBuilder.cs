using UnityEngine.Networking;

namespace DotsEcoCertificateSDK
{
    public interface IRequestBuilder
    {
        UnityWebRequest BuildRequest();
    }
}
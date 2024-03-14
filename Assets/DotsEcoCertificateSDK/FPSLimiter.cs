using UnityEngine;

namespace DotsEcoCertificateSDK
{
    public class FPSSetter : MonoBehaviour
    {
    
        [SerializeField] private int _targetFrameRate = 60;
        private void Start()
        {
            Application.targetFrameRate = _targetFrameRate;
        }
    }
}

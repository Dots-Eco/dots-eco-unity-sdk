using System;
using UnityEngine;

namespace DotsEcoCertificateSDK
{
    public class TestStarter : MonoBehaviour
    {
        [SerializeField] private string _testUserId;

        private void Awake()
        {
            CertificateManagerBehaviour.Instance.StartService(_testUserId); 
        }
        
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.O))
            {
                CertificateManagerBehaviour.Instance.OpenWallet();
            }
        }
    }
}
using System;
using UnityEngine;

namespace DotsEcoCertificateSDK.Scripts.Utility
{
    public class AutomaticDisabler : MonoBehaviour
    {
        private void OnDisable()
        {
            gameObject.SetActive(false);
        }
    }
}
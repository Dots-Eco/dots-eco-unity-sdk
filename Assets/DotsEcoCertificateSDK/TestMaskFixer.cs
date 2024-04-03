using System;
using Coffee.UISoftMask;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace DotsEcoCertificateSDK
{
    public class TestMaskFixer : MonoBehaviour
    {
        private void OnEnable()
        {
            var old = GetComponent<SoftMaskable>();
            if (old)
                Destroy(old);
            gameObject.AddComponent<SoftMaskable>();
            // var oldp = transform.parent.GetComponent<SoftMask>();
            // if (oldp)
            //     Destroy(oldp);
            // transform.parent.AddComponent<SoftMask>();
            //
            // var mask = transform.parent.GetComponent<Mask>();
            // if (mask)
            //     mask.showMaskGraphic = false;
        }
    }
}
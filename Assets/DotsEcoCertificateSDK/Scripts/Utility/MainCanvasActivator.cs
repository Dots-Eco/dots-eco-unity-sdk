using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DotsEcoCertificateSDK.Scripts.Utility
{
    public class MainCanvasActivator : MonoBehaviour
    {
        [SerializeField] private GraphicRaycaster _main;
        [SerializeField] private CanvasGroup[] _children;

        private void FixedUpdate()
        {
            var anyActive = _children.Any(x => x && x.alpha > 0.1f);
            _main.enabled = anyActive;
        }
    }
}
using System;
using UnityEngine;
using Utility;

namespace DotsEcoCertificateSDK
{
    internal class OrientationSwitcher : MonoBehaviour
    {
        [SerializeField] private CanvasHelper _canvasHelper;
        [SerializeField] private GameObject _vertical;
        [SerializeField] private GameObject _horizontal;

        private void OnOrientationChanged(ScreenOrientation screenOrientation)
        {
            switch (screenOrientation)
            {
                case ScreenOrientation.Portrait:
                case ScreenOrientation.PortraitUpsideDown:
                    _vertical.SetActive(true);
                    _horizontal.SetActive(false);
                    break;
                case ScreenOrientation.LandscapeLeft: 
                case ScreenOrientation.LandscapeRight:
                    _vertical.SetActive(false);
                    _horizontal.SetActive(true);
                    break;
                default:
                    _vertical.SetActive(true);
                    _horizontal.SetActive(false);
                    break;
            }
        }

        private void OnEnable()
        {
            OnOrientationChanged(_canvasHelper.LastOrientation);
        }
    }
}
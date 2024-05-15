using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace DotsEcoCertificateSDK.Scripts.Utility
{
    public class OrientationSwitcherCustom : MonoBehaviour
    {
        [SerializeField] private CanvasHelper _canvasHelper;
        [SerializeField] private UnityEvent Portrait;
        [SerializeField] private UnityEvent Landscape;
        
        public void OnOrientationChanged(ScreenOrientation screenOrientation)
        {
            switch (screenOrientation)
            {
                case ScreenOrientation.Portrait:
                case ScreenOrientation.PortraitUpsideDown:
                    Portrait?.Invoke();
                    break;
                case ScreenOrientation.LandscapeLeft: 
                case ScreenOrientation.LandscapeRight:
                    Landscape?.Invoke();
                    break;
                default:
                    Portrait?.Invoke();
                    break;
            }
        }

        private void OnEnable()
        {
            OnOrientationChanged(_canvasHelper.LastOrientation);
        }
    }
}
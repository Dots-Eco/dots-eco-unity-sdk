using System;
using UnityEngine;

namespace Utility
{
    [ExecuteAlways]
    [RequireComponent(typeof(Canvas))]
    public class CanvasHelper : MonoBehaviour
    {
        public event Action<DeviceScreenOrientation> OnOrientationChanged;
        public event Action OnResolutionChanged;
 
        [SerializeField] private RectTransform safeAreaTransform;
        [SerializeField] private bool showLogs = false;

        private DeviceScreenOrientation _lastOrientation = DeviceScreenOrientation.Horizontal;
        private Vector2 _lastResolution = Vector2.zero;
        private Rect _lastSafeArea = Rect.zero;
        
        private Canvas _canvas;
        
        public DeviceScreenOrientation LastOrientation
        {
            get => _lastOrientation;
            private set => _lastOrientation = value;
        }
 
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            
            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;
            _lastSafeArea = Screen.safeArea;
 
            _lastOrientation = (Screen.width / Screen.height > 1) ? DeviceScreenOrientation.Horizontal : DeviceScreenOrientation.Vertical;
            
            ApplySafeArea();
        }
 
        private void Update()
        {
            if (Screen.width != (int)_lastResolution.x || Screen.height != (int)_lastResolution.y) ResolutionChanged();
            if (Screen.safeArea != _lastSafeArea) SafeAreaChanged();
            
#if UNITY_EDITOR
            ApplySafeArea();
#endif
        }

        private void ApplySafeArea()
        {
            if(safeAreaTransform == null) return;
 
            Rect safeArea = Screen.safeArea;
 
            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;
            Rect pixelRect = _canvas.pixelRect;
            anchorMin.x /= pixelRect.width;
            anchorMin.y /= pixelRect.height;
            anchorMax.x /= pixelRect.width;
            anchorMax.y /= pixelRect.height;
 
            safeAreaTransform.anchorMin = anchorMin;
            safeAreaTransform.anchorMax = anchorMax;
        }
 
        private void ResolutionChanged()
        {
            if (showLogs) Debug.Log("Resolution changed from " + _lastResolution + " to (" + Screen.width + ", " + Screen.height + ")");

            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;
            
            DeviceScreenOrientation newOrientation = (Screen.width / Screen.height > 1) ? DeviceScreenOrientation.Horizontal : DeviceScreenOrientation.Vertical;
 
            if (newOrientation != _lastOrientation)
            {
                _lastOrientation = newOrientation;
                OnOrientationChanged?.Invoke(newOrientation);
                
                Debug.Log("Orientation changed to " + newOrientation);
            }
            
            OnResolutionChanged?.Invoke();
        }
 
        private void SafeAreaChanged()
        {
            if (showLogs) Debug.Log("Safe Area changed from " + _lastSafeArea + " to " + Screen.safeArea.size);

            _lastSafeArea = Screen.safeArea;
        }
    }

    public enum DeviceScreenOrientation
    {
        Vertical = 0,
        Horizontal = 1
    }
}
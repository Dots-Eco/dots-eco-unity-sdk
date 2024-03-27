using System;
using UnityEngine;

namespace Utility
{
    [ExecuteAlways]
    [RequireComponent(typeof(Canvas))]
    public class CanvasHelper : MonoBehaviour
    {
        public static event Action<ScreenOrientation> OnScreenOrientationChanged;
        public event Action OnResolutionChanged;
 
        [SerializeField] private RectTransform safeAreaTransform;
        [SerializeField] private bool showLogs = false;

        private Vector2 _lastResolution = Vector2.zero;
        private Rect _lastSafeArea = Rect.zero;
        
        private Canvas _canvas;
        
        public ScreenOrientation LastOrientation { get; private set; } = ScreenOrientation.Portrait;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            
            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;
            _lastSafeArea = Screen.safeArea;
            
            LastOrientation = Screen.orientation;
            
            ApplySafeArea();
        }
 
        private void Update()
        {
            if (Screen.width != (int)_lastResolution.x || Screen.height != (int)_lastResolution.y) ResolutionChanged();
            if (Screen.safeArea != _lastSafeArea) SafeAreaChanged();
            if (Screen.orientation != LastOrientation) OrientationChanged();
            
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

        private void OrientationChanged()
        {
            if (showLogs) Debug.Log("Orientation changed from: " + LastOrientation + "to: " + Screen.orientation);
            
            LastOrientation = Screen.orientation;
            
            OnScreenOrientationChanged?.Invoke(Screen.orientation);
        }
 
        private void ResolutionChanged()
        {
            if (showLogs) Debug.Log("Resolution changed from " + _lastResolution + " to (" + Screen.width + ", " + Screen.height + ")");

            _lastResolution.x = Screen.width;
            _lastResolution.y = Screen.height;
            
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
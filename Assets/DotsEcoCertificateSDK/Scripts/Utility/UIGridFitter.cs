using System;
using UnityEngine;
using UnityEngine.UI;

namespace DotsEcoCertificateSDKUtility
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class UIGridElementFitter : MonoBehaviour
    {
        [SerializeField] private RectTransform targetRect;
        
        [SerializeField] private bool stretchHorizontal = false;
        [SerializeField] private bool stretchVertical = false;
        
        private float defaultHorizontalSize;
        private float defaultVerticalSize;
        
        private GridLayoutGroup gridLayout;
        
        private void Awake()
        {
            gridLayout = GetComponent<GridLayoutGroup>();
        }
        
        private void Start()
        {
            if (gridLayout == null)
            {
                gridLayout = GetComponent<GridLayoutGroup>();
                if (gridLayout == null)
                {
                    Debug.LogError("No grid layout found on " + gameObject.name);
                    return;
                }
            }
            
            defaultHorizontalSize = gridLayout.cellSize.x;
            defaultVerticalSize = gridLayout.cellSize.y;
            
            UpdateSize();
        }

        private void UpdateSize()
        {
            Vector2 newCellSize = new Vector2(
                stretchHorizontal ? targetRect.rect.width : defaultHorizontalSize,
                stretchVertical ? targetRect.rect.height : defaultVerticalSize
            );
            
            gridLayout.cellSize= newCellSize;
        }
    }   
}
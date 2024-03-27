using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Utility;

[RequireComponent(typeof(GridLayoutGroupPlus))]
public class GridPlusScreenOrientationController : MonoBehaviour
{
    [SerializeField] private CanvasHelper canvasHelper;
    [SerializeField] private ScrollRect scrollRect;
    
    [Header("Parameters")]
    [SerializeField] private GridParameters verticalGridParameters;
    [SerializeField] private GridParameters horizontalGridParameters;
    
    private GridLayoutGroupPlus gridLayoutGroupPlus;
    private RectTransform gridRectTransform;
    
    private void Awake()
    {
        gridLayoutGroupPlus = GetComponent<GridLayoutGroupPlus>();
        gridRectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        SetGridParameters(
            canvasHelper.LastOrientation is ScreenOrientation.Portrait or ScreenOrientation.PortraitUpsideDown
                ? verticalGridParameters
                : horizontalGridParameters);
    }

    private void OnEnable()
    {
        CanvasHelper.OnScreenOrientationChanged += OnScreenOrientationChanged;
    }

    private void OnDisable()
    {
        CanvasHelper.OnScreenOrientationChanged -= OnScreenOrientationChanged;
    }

    private void OnScreenOrientationChanged(ScreenOrientation orientation)
    {
        SetGridParameters(orientation is ScreenOrientation.Portrait or ScreenOrientation.PortraitUpsideDown
            ? verticalGridParameters
            : horizontalGridParameters);
    }

    private void SetGridParameters(GridParameters gridParameters)
    {
        gridRectTransform.anchoredPosition = gridParameters.anchoredPosition;
        gridRectTransform.sizeDelta = gridParameters.sizeDelta;
        
        gridRectTransform.anchorMin = gridParameters.anchorMin;
        gridRectTransform.anchorMax = gridParameters.anchorMax;
        
        gridRectTransform.pivot = gridParameters.pivot;
            
        gridLayoutGroupPlus.HorizontalCellSizeHandle = gridParameters.horizontalCellSizeHandle;
        gridLayoutGroupPlus.VerticalCellSizeHandle = gridParameters.verticalCellSizeHandle;
            
        gridLayoutGroupPlus.constraint = gridParameters.constraint;
        gridLayoutGroupPlus.constraintCount = gridParameters.constraintCount;
            
        gridLayoutGroupPlus.ExpandParentHorizontally = gridParameters.expandHorizontally;
        gridLayoutGroupPlus.ExpandParentVertically = gridParameters.expandVertically;
        
        gridLayoutGroupPlus.padding = gridParameters.scrollRectPadding;
        
        scrollRect.vertical = gridParameters.scrollRectVerticalScrollEnabled;
        scrollRect.horizontal = gridParameters.scrollRectHorizontalScrollEnabled;
    }

    [Serializable]
    private struct GridParameters
    {
        public Vector2 anchoredPosition;
        public Vector2 sizeDelta;
        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public Vector2 pivot;

        public GridLayoutGroupPlus.CellSizeHandle horizontalCellSizeHandle;
        public GridLayoutGroupPlus.CellSizeHandle verticalCellSizeHandle;
        public GridLayoutGroupPlus.Constraint constraint;

        public RectOffset scrollRectPadding;
        
        public int constraintCount;
        public bool expandHorizontally;
        public bool expandVertically;

        public bool scrollRectVerticalScrollEnabled;
        public bool scrollRectHorizontalScrollEnabled;
    }
}

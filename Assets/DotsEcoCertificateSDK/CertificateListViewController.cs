using System;
using DotsEcoCertificateSDK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utility;

[RequireComponent(typeof(GridLayoutGroupPlus))]
public class CertificateListViewController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CertificatesListManager certificatesListManager;
    [SerializeField] private CanvasHelper canvasHelper;
    [SerializeField] private ScrollRect scrollRect;

    [Header("Header components")]
    [SerializeField] private RectTransform headerContainer;
    [SerializeField] private TextMeshProUGUI walletText;
    
    [Header("Parameters")]
    [SerializeField] private GridParameters verticalParameters;
    [SerializeField] private GridParameters horizontalParameters;

    private GridLayoutGroupPlus gridLayoutGroupPlus;
    private RectTransform gridRectTransform;

    private GridParameters currentGridParameters;
    
    private void Awake()
    {
        gridLayoutGroupPlus = GetComponent<GridLayoutGroupPlus>();
        gridRectTransform = GetComponent<RectTransform>();
        
        certificatesListManager.OnListRetrieved += OnListRetrieved;
    }

    private void Start()
    {
        currentGridParameters = canvasHelper.LastOrientation is ScreenOrientation.Portrait or ScreenOrientation.PortraitUpsideDown
                ? verticalParameters
                : horizontalParameters;
        
        SetGridParameters(currentGridParameters);
    }

    private void OnEnable()
    {
        CanvasHelper.OnScreenOrientationChanged += OnScreenOrientationChanged;
    }

    private void OnDisable()
    {
        CanvasHelper.OnScreenOrientationChanged -= OnScreenOrientationChanged;
    }
    
    private void OnListRetrieved(CertificateResponse[] certificates)
    {
        int certificatesCount = certificates.Length;
        walletText.text = string.Format(currentGridParameters.walletString, certificatesCount);
    }

    private void OnScreenOrientationChanged(ScreenOrientation orientation)
    {
        currentGridParameters = orientation is ScreenOrientation.Portrait or ScreenOrientation.PortraitUpsideDown
            ? verticalParameters
            : horizontalParameters;
        
        SetGridParameters(currentGridParameters);
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
        
        headerContainer.anchorMin = gridParameters.headerContainerAnchorMin;
        headerContainer.anchorMax = gridParameters.headerContainerAnchorMax;
        
        int certificatesCount = certificatesListManager.CertificateResponses?.Length ?? 0;
        
        walletText.fontSize = gridParameters.fontSize;
        walletText.text = string.Format(gridParameters.walletString, certificatesCount);
    }

    [Serializable]
    private struct GridParameters
    {
        [Header("Grid parameters")]
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
        
        [Header("Header components parameters")]
        public Vector2 headerContainerAnchorMin;
        public Vector2 headerContainerAnchorMax;
        
        [Header("Header text")]
        public string walletString;
        public float fontSize;
    }
}

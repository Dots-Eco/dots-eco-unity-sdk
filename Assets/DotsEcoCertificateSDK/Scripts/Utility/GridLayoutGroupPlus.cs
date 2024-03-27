using UnityEngine;
using System.Collections.Generic;
 using UnityEngine.Serialization;

 namespace UnityEngine.UI
 {
     [AddComponentMenu("Layout/Grid Layout Group Plus", 153)]
     /// <summary>
     /// Layout class to arrange child elements in a grid format.
     /// </summary>
     /// <remarks>
     /// The GridLayoutGroup component is used to layout child layout elements in a uniform grid where all cells have the same size. The size and the spacing between cells is controlled by the GridLayoutGroup itself. The children have no influence on their sizes.
     /// </remarks>
     public class GridLayoutGroupPlus : LayoutGroup
     {
         /// <summary>
         /// Which corner is the starting corner for the grid.
         /// </summary>
         public enum Corner
         {
             /// <summary>
             /// Upper Left corner.
             /// </summary>
             UpperLeft = 0,

             /// <summary>
             /// Upper Right corner.
             /// </summary>
             UpperRight = 1,

             /// <summary>
             /// Lower Left corner.
             /// </summary>
             LowerLeft = 2,

             /// <summary>
             /// Lower Right corner.
             /// </summary>
             LowerRight = 3
         }

         /// <summary>
         /// The grid axis we are looking at.
         /// </summary>
         /// <remarks>
         /// As the storage is a [][] we make access easier by passing a axis.
         /// </remarks>
         public enum Axis
         {
             /// <summary>
             /// Horizontal axis
             /// </summary>
             Horizontal = 0,

             /// <summary>
             /// Vertical axis.
             /// </summary>
             Vertical = 1
         }

         /// <summary>
         /// Constraint type on either the number of columns or rows.
         /// </summary>
         public enum Constraint
         {
             /// <summary>
             /// Don't constrain the number of rows or columns.
             /// </summary>
             Flexible = 0,

             /// <summary>
             /// Constrain the number of columns to a specified number.
             /// </summary>
             FixedColumnCount = 1,

             /// <summary>
             /// Constraint the number of rows to a specified number.
             /// </summary>
             FixedRowCount = 2
         }

         /// <summary>
         /// How to calculate padding
         /// </summary>
         public enum PaddingMode
         {
             /// <summary>
             /// Padding in pixels
             /// </summary>
             Fixed = 0,

             /// <summary>
             /// Padding relative to TargetRect size
             /// </summary>
             RelativeToTargetRect = 1,

             /// <summary>
             /// Padding relative to cell size
             /// </summary>
             RelativeToCell = 2
         }

         /// <summary>
         /// How to calculate cell size
         /// </summary>
         public enum CellSizeHandle
         {
             /// <summary>
             /// Cell size int pixels
             /// </summary>
             FixedPixelSize = 0,

             /// <summary>
             /// Stretch cells to fill empty space
             /// </summary>
             Stretch = 1,

             /// <summary>
             /// Cell size in percent relative to target RectTransform
             /// </summary>
             RelativeToTargetRectTransform = 3
         }

         [SerializeField] protected PaddingMode m_PaddingMode = PaddingMode.Fixed;

         public PaddingMode paddingMode
         {
             get { return m_PaddingMode; }
             set { SetProperty(ref m_PaddingMode, value); }
         }

         public Vector4 relativePadding;

         [SerializeField] protected Corner m_StartCorner = Corner.UpperLeft;

         /// <summary>
         /// Which corner should the first cell be placed in?
         /// </summary>
         public Corner startCorner
         {
             get { return m_StartCorner; }
             set { SetProperty(ref m_StartCorner, value); }
         }

         [SerializeField] protected Axis m_StartAxis = Axis.Horizontal;

         /// <summary>
         /// Which axis should cells be placed along first
         /// </summary>
         /// <remarks>
         /// When startAxis is set to horizontal, an entire row will be filled out before proceeding to the next row. When set to vertical, an entire column will be filled out before proceeding to the next column.
         /// </remarks>
         public Axis startAxis
         {
             get { return m_StartAxis; }
             set { SetProperty(ref m_StartAxis, value); }
         }

         [SerializeField] protected Vector2 m_Spacing = Vector2.zero;

         /// <summary>
         /// The spacing to use between layout elements in the grid on both axises.
         /// </summary>
         public Vector2 spacing
         {
             get { return m_Spacing; }
             set { SetProperty(ref m_Spacing, value); }
         }

         [SerializeField] protected Constraint m_Constraint = Constraint.Flexible;

         /// <summary>
         /// Which constraint to use for the GridLayoutGroup.
         /// </summary>
         /// <remarks>
         /// Specifying a constraint can make the GridLayoutGroup work better in conjunction with a [[ContentSizeFitter]] component. When GridLayoutGroup is used on a RectTransform with a manually specified size, there's no need to specify a constraint.
         /// </remarks>
         public Constraint constraint
         {
             get { return m_Constraint; }
             set { SetProperty(ref m_Constraint, value); }
         }

         [SerializeField] protected int m_ConstraintCount = 2;

         /// <summary>
         /// How many cells there should be along the constrained axis.
         /// </summary>
         public int constraintCount
         {
             get { return m_ConstraintCount; }
             set { SetProperty(ref m_ConstraintCount, Mathf.Max(1, value)); }
         }

         [SerializeField] private CellSizeHandle m_HorizontalCellSizeHandle = CellSizeHandle.FixedPixelSize;

         /// <summary>
         /// How to calculate cell width
         /// </summary>
         public CellSizeHandle HorizontalCellSizeHandle
         {
             get { return m_HorizontalCellSizeHandle; }
             set { SetProperty(ref m_HorizontalCellSizeHandle, value); }
         }

         [SerializeField] private CellSizeHandle m_VerticalCellSizeHandle = CellSizeHandle.FixedPixelSize;

         /// <summary>
         /// How to calculate cell height
         /// </summary>
         public CellSizeHandle VerticalCellSizeHandle
         {
             get { return m_VerticalCellSizeHandle; }
             set { SetProperty(ref m_VerticalCellSizeHandle, value); }
         }

         [SerializeField] protected Vector2 m_CellSize = new Vector2(100, 100);

         /// <summary>
         /// The size to use for each cell in the grid.
         /// </summary>
         public Vector2 cellSize
         {
             get { return m_CellSize; }
             set { SetProperty(ref m_CellSize, value); }
         }
         
         [SerializeField] protected bool expandParentHorizontally = false;
         [SerializeField] protected bool expandParentVertically = false;
         
         public bool ExpandParentHorizontally
         {
             get { return expandParentHorizontally; }
             set { SetProperty(ref expandParentHorizontally, value); }
         }
         
         public bool ExpandParentVertically
         {
             get { return expandParentVertically; }
             set { SetProperty(ref expandParentVertically, value); }
         }
         
         protected RectTransform m_ParentRectTransform;

         public RectTransform m_TargetRectTransform = default;

         /// <summary>
         /// RectTransform for relative cell size calculation
         /// </summary>
         [SerializeField]
         protected RectTransform targetRectTransform
         {
             get { return m_TargetRectTransform; }
             set { SetProperty(ref m_TargetRectTransform, value); }
         }

         protected GridLayoutGroupPlus()
         {
         }

#if UNITY_EDITOR
         protected override void OnValidate()
         {
             base.OnValidate();

             if (m_ParentRectTransform == null)
             {
                 GetComponent<RectTransform>();
             }
             
             constraintCount = constraintCount;
         }

#endif

         /// <summary>
         /// Called by the layout system to calculate the horizontal layout size.
         /// Also see ILayoutElement
         /// </summary>
         public override void CalculateLayoutInputHorizontal()
         {
             base.CalculateLayoutInputHorizontal();

             int minColumns = 0;
             int preferredColumns = 0;

             if (paddingMode == PaddingMode.RelativeToTargetRect)
             {
                 padding = new RectOffset(
                     (int)(relativePadding.x * targetRectTransform.rect.width),
                     (int)(relativePadding.y * targetRectTransform.rect.width),
                     (int)(relativePadding.z * targetRectTransform.rect.height),
                     (int)(relativePadding.w * targetRectTransform.rect.height));
             }
             else if (paddingMode == PaddingMode.RelativeToCell)
             {
                 padding = new RectOffset(
                     (int)(relativePadding.x * rectChildren[0].rect.width),
                     (int)(relativePadding.y * rectChildren[0].rect.width),
                     (int)(relativePadding.z * rectChildren[0].rect.height),
                     (int)(relativePadding.w * rectChildren[0].rect.height));
             }

             if (m_Constraint == Constraint.FixedColumnCount)
             {
                 minColumns = preferredColumns = m_ConstraintCount;
             }
             else if (m_Constraint == Constraint.FixedRowCount)
             {
                 minColumns = preferredColumns =
                     Mathf.CeilToInt(rectChildren.Count / (float)m_ConstraintCount - 0.001f);
             }
             else
             {
                 minColumns = 1;
                 preferredColumns = Mathf.CeilToInt(Mathf.Sqrt(rectChildren.Count));
             }

             SetLayoutInputForAxis(
                 padding.horizontal + (cellSize.x + spacing.x) * minColumns - spacing.x,
                 padding.horizontal + (cellSize.x + spacing.x) * preferredColumns - spacing.x,
                 -1, 0);
         }

         /// <summary>
         /// Called by the layout system to calculate the vertical layout size.
         /// Also see ILayoutElement
         /// </summary>
         public override void CalculateLayoutInputVertical()
         {
             int minRows = 0;
             if (m_Constraint == Constraint.FixedColumnCount)
             {
                 minRows = Mathf.CeilToInt(rectChildren.Count / (float)m_ConstraintCount - 0.001f);
             }
             else if (m_Constraint == Constraint.FixedRowCount)
             {
                 minRows = m_ConstraintCount;
             }
             else
             {
                 float width = rectTransform.rect.width;
                 int cellCountX = Mathf.Max(1,
                     Mathf.FloorToInt((width - padding.horizontal + spacing.x + 0.001f) / (cellSize.x + spacing.x)));
                 minRows = Mathf.CeilToInt(rectChildren.Count / (float)cellCountX);
             }

             float minSpace = padding.vertical + (cellSize.y + spacing.y) * minRows - spacing.y;
             SetLayoutInputForAxis(minSpace, minSpace, -1, 1);
         }

         /// <summary>
         /// Called by the layout system
         /// Also see ILayoutElement
         /// </summary>
         public override void SetLayoutHorizontal()
         {
             SetCellsAlongAxis(0);
         }

         /// <summary>
         /// Called by the layout system
         /// Also see ILayoutElement
         /// </summary>
         public override void SetLayoutVertical()
         {
             SetCellsAlongAxis(1);
         }

         private void SetCellsAlongAxis(int axis)
         {
             // Normally a Layout Controller should only set horizontal values when invoked for the horizontal axis
             // and only vertical values when invoked for the vertical axis.
             // However, in this case we set both the horizontal and vertical position when invoked for the vertical axis.
             // Since we only set the horizontal position and not the size, it shouldn't affect children's layout,
             // and thus shouldn't break the rule that all horizontal layout must be calculated before all vertical layout.

             if (axis == 0)
             {
                 // Only set the sizes when invoked for horizontal axis, not the positions.
                 for (int i = 0; i < rectChildren.Count; i++)
                 {
                     RectTransform rect = rectChildren[i];

                     m_Tracker.Add(this, rect,
                         DrivenTransformProperties.Anchors |
                         DrivenTransformProperties.AnchoredPosition |
                         DrivenTransformProperties.SizeDelta);

                     rect.anchorMin = Vector2.up;
                     rect.anchorMax = Vector2.up;
                     rect.sizeDelta = cellSize;
                 }

                 return;
             }

             float width = rectTransform.rect.size.x;
             float height = rectTransform.rect.size.y;

             int cellCountX = 1;
             int cellCountY = 1;
             if (m_Constraint == Constraint.FixedColumnCount)
             {
                 cellCountX = m_ConstraintCount;

                 if (rectChildren.Count > cellCountX)
                     cellCountY = rectChildren.Count / cellCountX + (rectChildren.Count % cellCountX > 0 ? 1 : 0);
             }
             else if (m_Constraint == Constraint.FixedRowCount)
             {
                 cellCountY = m_ConstraintCount;

                 if (rectChildren.Count > cellCountY)
                     cellCountX = rectChildren.Count / cellCountY + (rectChildren.Count % cellCountY > 0 ? 1 : 0);
             }
             else
             {
                 if (cellSize.x + spacing.x <= 0)
                     cellCountX = int.MaxValue;
                 else
                     cellCountX = Mathf.Max(1,
                         Mathf.FloorToInt((width - padding.horizontal + spacing.x + 0.001f) /
                             (cellSize.x + spacing.x)));

                 if (cellSize.y + spacing.y <= 0)
                     cellCountY = int.MaxValue;
                 else
                     cellCountY = Mathf.Max(1,
                         Mathf.FloorToInt((height - padding.vertical + spacing.y + 0.001f) / (cellSize.y + spacing.y)));
             }

             int cornerX = (int)startCorner % 2;
             int cornerY = (int)startCorner / 2;

             int cellsPerMainAxis, actualCellCountX, actualCellCountY;
             if (startAxis == Axis.Horizontal)
             {
                 cellsPerMainAxis = cellCountX;
                 actualCellCountX = Mathf.Clamp(cellCountX, 1, rectChildren.Count);
                 actualCellCountY = Mathf.Clamp(cellCountY, 1,
                     Mathf.CeilToInt(rectChildren.Count / (float)cellsPerMainAxis));
             }
             else
             {
                 cellsPerMainAxis = cellCountY;
                 actualCellCountY = Mathf.Clamp(cellCountY, 1, rectChildren.Count);
                 actualCellCountX = Mathf.Clamp(cellCountX, 1,
                     Mathf.CeilToInt(rectChildren.Count / (float)cellsPerMainAxis));
             }

             float requiredSpaceX = actualCellCountX * cellSize.x + (actualCellCountX - 1) * spacing.x;

             if (HorizontalCellSizeHandle == CellSizeHandle.RelativeToTargetRectTransform) requiredSpaceX = 0;

             Vector2 requiredSpace = new Vector2(requiredSpaceX,
                 actualCellCountY * cellSize.y + (actualCellCountY - 1) * spacing.y);

             Vector2 startOffset = new Vector2(
                 GetStartOffset(0, requiredSpace.x),
                 GetStartOffset(1, requiredSpace.y)
             );

             for (int i = 0; i < rectChildren.Count; i++)
             {
                 int positionX;
                 int positionY;
                 if (startAxis == Axis.Horizontal)
                 {
                     positionX = i % cellsPerMainAxis;
                     positionY = i / cellsPerMainAxis;
                 }
                 else
                 {
                     positionX = i / cellsPerMainAxis;
                     positionY = i % cellsPerMainAxis;
                 }

                 if (cornerX == 1)
                     positionX = actualCellCountX - 1 - positionX;
                 if (cornerY == 1)
                     positionY = actualCellCountY - 1 - positionY;

                 if (HorizontalCellSizeHandle == CellSizeHandle.Stretch)
                 {
                     float stretchedSize = ((width - (spacing[0] * (actualCellCountX - 1))) / actualCellCountX);
                     SetChildAlongAxis(rectChildren[i], 0, startOffset.x + (stretchedSize + spacing[0]) * positionX,
                         stretchedSize);
                 }
                 else if (HorizontalCellSizeHandle == CellSizeHandle.RelativeToTargetRectTransform &&
                          targetRectTransform != null)
                 {
                     float cellSizeX = cellSize.x * targetRectTransform.rect.size.x;
                     SetChildAlongAxis(rectChildren[i], 0, startOffset.x + (cellSizeX + spacing[0]) * positionX,
                         cellSizeX);
                 }
                 else
                 {
                     SetChildAlongAxis(rectChildren[i], 0, startOffset.x + (cellSize[0] + spacing[0]) * positionX,
                         cellSize[0]);
                 }

                 if (VerticalCellSizeHandle == CellSizeHandle.Stretch)
                 {
                     float stretchedSize = ((height - (spacing[1] * (actualCellCountY - 1))) / actualCellCountY);
                     SetChildAlongAxis(rectChildren[i], 1, startOffset.y + (stretchedSize + spacing[1]) * positionY,
                         stretchedSize);
                 }
                 else if (VerticalCellSizeHandle == CellSizeHandle.RelativeToTargetRectTransform &&
                          targetRectTransform != null)
                 {
                     float cellSizeY = cellSize.y * targetRectTransform.rect.size.y;
                     SetChildAlongAxis(rectChildren[i], 1, startOffset.y + (cellSizeY + spacing[1]) * positionY,
                         cellSizeY);
                 }
                 else
                 {
                     SetChildAlongAxis(rectChildren[i], 1, startOffset.y + (cellSize[1] + spacing[1]) * positionY,
                         cellSize[1]);
                 }
             }
             
             if (m_ParentRectTransform == null)
             {
                 m_ParentRectTransform = GetComponent<RectTransform>();
             }

             if (rectChildren.Count == 0)
             {
                 return;
             }

             Vector2 parentSizeDelta = m_ParentRectTransform.sizeDelta;
             
             float parentSizeHorizontal = expandParentHorizontally ? 0 : parentSizeDelta.x;
             float parentSizeVertical = expandParentVertically ? 0 : parentSizeDelta.y;
             
             foreach (RectTransform child in rectChildren)
             {
                 if (HorizontalCellSizeHandle != CellSizeHandle.Stretch) parentSizeHorizontal += child.sizeDelta.x;
                 if (VerticalCellSizeHandle != CellSizeHandle.Stretch) parentSizeVertical += child.sizeDelta.y;
             }

             if (HorizontalCellSizeHandle != CellSizeHandle.Stretch) parentSizeHorizontal += (rectChildren.Count - 1) * spacing.x;
             if (VerticalCellSizeHandle != CellSizeHandle.Stretch) parentSizeVertical += (rectChildren.Count - 1) * spacing.y;
            
             m_ParentRectTransform.sizeDelta = new Vector2(parentSizeHorizontal, parentSizeVertical);
         }
     }
 }
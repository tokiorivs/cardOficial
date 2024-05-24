using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scales grid layout items based upon the
/// - gridlayout transform/grid items parent
/// - rowCount, columnCount 
/// - spacing
/// </summary>
public class GridScaler : MonoBehaviour
{
    [SerializeField] int m_DefaultSpacing = 10;
    [SerializeField] GridLayoutGroup m_GridLayoutGroup;
    
    public void Setup(int rows, int columns, int? spacing = null)
    {
        // Constraining how many grid items per row
        m_GridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        m_GridLayoutGroup.constraintCount = columns;

        // Used to scale the grids
        RectTransform rectTransform = m_GridLayoutGroup.GetComponent<RectTransform>();

        if(rectTransform == null)
        {
            Debug.LogError("GridScaler m_GridLayoutGroup Gameobject RectTransform == null");
            return;
        }


        spacing = (spacing != null) ? (int)spacing : m_DefaultSpacing;

        float rectWidth = rectTransform.rect.width;
        float rectHeight = rectTransform.rect.height;

        // Account for spacing
        rectWidth -= ((int)spacing * columns);
        rectHeight -= ((int)spacing * rows);

        // Division
        rectWidth = rectWidth / columns;
        rectHeight = rectHeight / rows;

        float cellSize = (rectWidth < rectHeight) ? rectWidth : rectHeight;
        m_GridLayoutGroup.cellSize = new Vector2(cellSize, cellSize);
        m_GridLayoutGroup.spacing = new Vector2((int)spacing, (int)spacing);
    }
}

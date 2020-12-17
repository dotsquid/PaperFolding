using UnityEngine;

using Borders = System.Collections.Generic.List<FoldDragPointBorder>;

public class FoldDragPointController : MonoBehaviour
{
    private delegate float Comp(float a, float b);

    [SerializeField]
    private FoldDragPoint[] _dragPoints;
    [SerializeField]
    private HeroTapController _heroTapController;

    private Borders _topBorders;
    private Borders _leftBorders;
    private Borders _rightBorders;
    private Borders _bottomBorders;

    public void Init()
    {
        var estimateCount = _dragPoints.Length / 4 + 1;
        _topBorders = new Borders(estimateCount);
        _leftBorders = new Borders(estimateCount);
        _rightBorders = new Borders(estimateCount);
        _bottomBorders = new Borders(estimateCount);

        foreach (var dragPoint in _dragPoints)
        {
            var xBorder = new FoldDragPointBorder();
            var yBorder = new FoldDragPointBorder();
            dragPoint.Init(_heroTapController, this, xBorder, yBorder);
            var dir = dragPoint.allowedDir;
            if (dir.x < 0.0f)
                _leftBorders.Add(xBorder);
            else if (dir.x > 0.0f)
                _rightBorders.Add(xBorder);
            if (dir.y < 0.0f)
                _bottomBorders.Add(yBorder);
            else if (dir.y > 0.0f)
                _topBorders.Add(yBorder);
        }
    }

    public Vector2 RespectBorders(Vector2 position, Vector2 dir)
    {
        if (dir.x < 0.0f)
        {
            position.x = RespectBorder(position.x, Mathf.Min, _rightBorders);
        }
        else if (dir.x > 0.0f)
        {
            position.x = RespectBorder(position.x, Mathf.Max, _leftBorders);
        }

        if (dir.y < 0.0f)
        {
            position.y = RespectBorder(position.y, Mathf.Min, _topBorders);
        }
        else if (dir.y > 0.0f)
        {
            position.y = RespectBorder(position.y, Mathf.Max, _bottomBorders);
        }

        return position;
    }

    private static float RespectBorder(float value, Comp comp, Borders borders)
    {
        foreach (var border in borders)
            value = comp(value, border.value);
        return value;
    }
}

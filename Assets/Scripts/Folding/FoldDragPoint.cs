using UnityEngine;
using UnityEngine.EventSystems;

public class FoldDragPoint : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler
{
    private static readonly Rect kBounds = new Rect(-0.5f, -0.5f, 1.0f, 1.0f);
    private static readonly Vector2 kPaperCornerOffset = new Vector2(0.0f, 0.707f);
    private static readonly Vector2 kPaperEdgeOffset = new Vector2(0.0f, 0.5f);
    private static readonly Vector3 kForward = Vector3.forward;

    public enum Type
    {
        Corner,
        Edge
    }

    [SerializeField]
    private Type _type;
    [SerializeField]
    private FoldDispatcher _foldDispatcher;
    [SerializeField]
    private FoldDragPoint[] _neighbours;

    private Transform _transform;
    private FoldController _acquiredFoldController;
    private Vector2 _origin;
    private Vector2 _allowedDir;
    private Rect _bounds;
    private float _paperAngleTheta;
    private float _distance;
    private int _lockCounter;

    public bool isLocked
    {
        get => _lockCounter > 0;
        set
        {
            if (value || _lockCounter > 0)
                _lockCounter += value ? +1 : -1;
        }
    }

    private void Awake()
    {
        _transform = transform;
        _origin = _transform.position;
        InitBounds();
        InitPaperAngleTheta();
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (_acquiredFoldController == null && !isLocked)
        {
            _acquiredFoldController = _foldDispatcher.Acquire();
            SetNeighboursLockState(true);
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (_acquiredFoldController == null)
            return;

        var camera = eventData.pressEventCamera;
        Vector2 worldPosition = camera.ScreenToWorldPoint(eventData.position);
        Vector2 clampedPosition = worldPosition.Clamp(_bounds);
        var allowedPosition = clampedPosition * _allowedDir;
        if (_type == Type.Corner)
        {
            float xy = Mathf.Max(allowedPosition.x, allowedPosition.y);
            clampedPosition.x = xy * _allowedDir.x;
            clampedPosition.y = xy * _allowedDir.y;
        }
        _transform.position = clampedPosition;

        var dir = _origin - clampedPosition;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _distance = dir.magnitude;

        var data = default(FoldControllerData);
        var paperAngle = 2.0f * angle;
        switch (_type)
        {
            case Type.Corner:
                data.paperPosition = clampedPosition + kPaperCornerOffset.Rotate(paperAngle - _paperAngleTheta);
                break;

            case Type.Edge:
                data.paperPosition = clampedPosition + kPaperEdgeOffset.Rotate(paperAngle - _paperAngleTheta);
                break;
        }
        data.paperRotation = Quaternion.AngleAxis(paperAngle, kForward);

        data.maskOrigin = (_origin + clampedPosition) * 0.5f;
        data.maskRotation = Quaternion.AngleAxis(angle, kForward);

        data.dragDirection = dir;
        data.dragDistance = _distance;
        _acquiredFoldController.Drag(data);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        const float kEpsilon = 0.025f;
        if (_distance < kEpsilon && _acquiredFoldController != null)
        {
            SetNeighboursLockState(false);
            _foldDispatcher.Release(_acquiredFoldController);
            _acquiredFoldController = null;
            _transform.position = _origin;
        }
    }

    void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }

    private void SetNeighboursLockState(bool lockState)
    {
        foreach (var neighbour in _neighbours)
            neighbour.isLocked = lockState;
    }

    private void InitBounds()
    {
        _bounds = kBounds;
        if (_type == Type.Edge)
        {
            var angle = RoundDirectionAngle(_origin, 90.0f);
            if (Mathf.Approximately(angle, 0.0f) ||
                Mathf.Approximately(angle, 180.0f))
                _bounds.yMin = _bounds.yMax = 0.0f;
            if (Mathf.Approximately(angle, 90.0f) ||
                Mathf.Approximately(angle, 270.0f))
                _bounds.xMin = _bounds.xMax = 0.0f;
        }
        _allowedDir.x = Mathf.Approximately(_origin.x, 0.0f) ? 0.0f : Mathf.Sign(_origin.x);
        _allowedDir.y = Mathf.Approximately(_origin.y, 0.0f) ? 0.0f : Mathf.Sign(_origin.y);
    }

    private void InitPaperAngleTheta()
    {
        var dir = _origin;
        //if (_type == Type.Edge)
        //    dir = dir.Rotate(45.0f);
        var angle = RoundDirectionAngle(dir, 22.5f);
        _paperAngleTheta = angle + 90.0f;
    }

    private static float RoundDirectionAngle(Vector2 dir, float baseAnfle)
    {
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle = MathExt.Wrap360(angle);
        return ((int)(angle / baseAnfle)) * baseAnfle;
    }
}

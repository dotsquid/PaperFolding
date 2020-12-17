using UnityEngine;
using UnityEngine.EventSystems;

public class FoldDragPoint : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler, IPointerClickHandler
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
    private GameObject _heroTapControllerObject;
    private FoldDragPointController _controller;
    private FoldController _acquiredFoldController;
    private FoldDragPointBorder _xBorder;
    private FoldDragPointBorder _yBorder;
    private Vector2 _origin;
    private Vector2 _allowedDir;
    private Rect _bounds;
    private float _paperAngleTheta;
    private float _distance;
    private int _lockCounter;
    private bool _isDragging;

    public bool isLocked
    {
        get => _lockCounter > 0;
        set
        {
            if (value || _lockCounter > 0)
                _lockCounter += value ? +1 : -1;
        }
    }

    public Vector2 allowedDir => _allowedDir;
    
    public void Init(HeroTapController heroTapController, FoldDragPointController controller, FoldDragPointBorder xBorder, FoldDragPointBorder yBorder)
    {
        _transform = transform;
        _origin = _transform.position;
        _heroTapControllerObject = heroTapController.gameObject;
        _controller = controller;
        _xBorder = xBorder;
        _yBorder = yBorder;
        InitBounds();
        InitPaperAngleTheta();
        SetFoldBounds(_origin);
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        _isDragging = true;
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
        Vector2 clampedPosition = _controller.RespectBorders(worldPosition.Clamp(_bounds), _allowedDir);
        var allowedPosition = clampedPosition * _allowedDir;
        if (_type == Type.Corner)
        {
            float xy = Mathf.Max(allowedPosition.x, allowedPosition.y);
            clampedPosition.x = xy * _allowedDir.x;
            clampedPosition.y = xy * _allowedDir.y;
        }
        SetPosition(clampedPosition);
        SetFoldBounds(clampedPosition);

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
            SetPosition(_origin);
        }
        _isDragging = false;
    }

    void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!_isDragging)
            ExecuteEvents.Execute(_heroTapControllerObject, eventData, ExecuteEvents.pointerClickHandler);
    }

    private void SetNeighboursLockState(bool lockState)
    {
        foreach (var neighbour in _neighbours)
            neighbour.isLocked = lockState;
    }

    private void SetFoldBounds(Vector2 position)
    {
        _xBorder.value = position.x;
        _yBorder.value = position.y;
    }

    private void SetPosition(Vector2 position)
    {
        // keep Z
        var selfPosition = _transform.position;
        selfPosition.x = position.x;
        selfPosition.y = position.y;
        _transform.position = selfPosition;
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

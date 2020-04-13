using UnityEngine;
using UnityEngine.EventSystems;

public class FoldHotPoint : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IInitializePotentialDragHandler
{
    private static readonly Rect kBounds = new Rect(-0.5f, -0.5f, 1.0f, 1.0f);
    //private static readonly Vector2 kPaperOffset = new Vector2(0.5f, 0.5f);
    private static readonly Vector2 kPaperOffset = new Vector2(0.0f, 0.707f);
    private static readonly Vector3 kForward = Vector3.forward;

    public enum Type
    {
        Corner,
        Edge
    }

    [SerializeField]
    private Type _type;
    [SerializeField]
    private Transform _paperTransform;
    [SerializeField]
    private Transform _maskTransform;
    [SerializeField]
    private SpriteRenderer _shadow;
    [SerializeField]
    private FloatRange _shadowDistanceRange = new FloatRange(0.0f, 0.1f);
    [SerializeField]
    private FloatRange _shadowOpacityRange = new FloatRange(0.0f, 0.32f);

    private Transform _transform;
    private Vector2 _origin;
    private SpriteRenderer _sprite;
    private Color _origColor;

    private void Awake()
    {
        _transform = transform;
        _origin = _transform.position;
        _sprite = GetComponent<SpriteRenderer>();
        _origColor = _sprite.color;
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        _sprite.color = Color.red;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        var camera = eventData.pressEventCamera;
        Vector2 worldPosition = camera.ScreenToWorldPoint(eventData.position);
        Vector2 clampedPosition = worldPosition.Clamp(kBounds);
        _transform.position = clampedPosition;

        var dir = _origin - clampedPosition;
        var dist = dir.magnitude;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        var paperAngle = 2.0f * angle;
        var paperPosition = clampedPosition + kPaperOffset.Rotate(paperAngle - 135.0f);
        var paperRotation = Quaternion.AngleAxis(paperAngle, kForward);
        _paperTransform.SetPositionAndRotation(paperPosition, paperRotation);

        var maskSize = _maskTransform.lossyScale.x * 0.5f;
        var midPoint = (_origin + clampedPosition) * 0.5f;
        var maskOffset = dir.normalized * maskSize;
        var maskPosition = midPoint + maskOffset;
        var maskRotation = Quaternion.AngleAxis(angle, kForward);
        _maskTransform.SetPositionAndRotation(maskPosition, maskRotation);

        var shadowOpacity = MathExt.LerpClamp(dist, _shadowDistanceRange, _shadowOpacityRange);
        _shadow.SetAlpha(shadowOpacity);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        _sprite.color = _origColor;
    }

    void IInitializePotentialDragHandler.OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }
}

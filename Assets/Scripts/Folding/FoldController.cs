using UnityEngine;
using UnityEngine.Rendering;

public struct FoldControllerData
{
    public Vector2 paperPosition;
    public Quaternion paperRotation;
    public Vector2 maskOrigin;
    public Quaternion maskRotation;
    public Vector2 dragDirection;
    public float dragDistance;
}

public class FoldController : MonoBehaviour
{
    [SerializeField]
    private Transform _paperTransform;
    [SerializeField]
    private Transform _maskBackTransform;
    [SerializeField]
    private Transform _maskFrontTransform;
    [SerializeField]
    private SortingGroup _sortingGroup;
    [SerializeField]
    private SpriteRenderer _shadowRenderer;
    [SerializeField]
    private FloatRange _shadowDistanceRange = new FloatRange(0.0f, 0.025f);
    [SerializeField]
    private FloatRange _shadowOpacityRange = new FloatRange(0.0f, 0.16f);

    private GameObject _gameObject;

    public void Init()
    {
        _gameObject = gameObject;
        _gameObject.SetActive(false);
        _maskFrontTransform.localScale = _maskFrontTransform.localScale;
    }

    public void Acquire(int order)
    {
        _gameObject.SetActive(true);
        _sortingGroup.sortingOrder = order;
    }

    public void Release()
    {
        _gameObject.SetActive(false);
    }

    public void Drag(FoldControllerData data)
    {
        const float kEpsilon = 0.005f;

        var maskSize = _maskBackTransform.lossyScale.x * 0.5f;
        var maskOffset = data.dragDirection.normalized * maskSize;
        var maskPosition = data.maskOrigin + maskOffset;

        _paperTransform.SetPositionAndRotation(data.paperPosition, data.paperRotation);
        _maskBackTransform.SetPositionAndRotation(maskPosition, data.maskRotation);
        _maskFrontTransform.SetPositionAndRotation(maskPosition, data.maskRotation);

        var shadowOpacity = MathExt.LerpClamp(data.dragDistance, _shadowDistanceRange, _shadowOpacityRange);
        _shadowRenderer.SetAlpha(shadowOpacity);

        var isActive = data.dragDistance > kEpsilon;
        _gameObject.SetActive(isActive);
    }
}

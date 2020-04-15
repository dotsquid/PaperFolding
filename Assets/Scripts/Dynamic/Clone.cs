using UnityEngine;

public class Clone : MonoBehaviour
{
    [SerializeField]
    private Transform _root;
    [SerializeField]
    private Transform _owner;

    private Transform _transform;

    public void Init(Transform root, Transform owner)
    {
        _root = root;
        _owner = owner;
        _transform = transform;
        InitScale();
        SetLayerRecursively(_transform, LayerMask.NameToLayer("Back"));
    }

    private void Update()
    {
        var localPosition = _root.InverseTransformPoint(_owner.position);
        var localRotation = Quaternion.Inverse(_root.rotation) * _owner.rotation;
        localRotation = Quaternion.Euler(-localRotation.eulerAngles);
        _transform.localPosition = localPosition;
        _transform.localRotation = localRotation;
    }

    private void InitScale()
    {
        var localScale = _transform.localScale;
        localScale.x *= -1.0f;
        _transform.localScale = localScale;
    }

    private static void SetLayerRecursively(Transform transform, int layer)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.layer = layer;
            SetLayerRecursively(child, layer);
        }
    }
}

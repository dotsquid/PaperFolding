using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField, Min(0.0f)]
    private float _speed = 0.1f;

    private Transform _transform;
    private Vector2 _target;
    private Vector2 _velocity;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        Vector2 position = _transform.position;
        float duration = (_target - position).magnitude / _speed;
        _transform.position = Vector2.SmoothDamp(_transform.position, _target, ref _velocity, duration);
    }

    public void SetTarget(Vector2 target)
    {
        _target = target;
    }
}

using System;
using UnityEngine;

[Serializable]
public class MinMaxRange<T> : UnityEngine.Object
{
    public readonly T min;
    public readonly T max;

    public MinMaxRange(T min, T max)
    {
        this.min = min;
        this.max = max;
    }
}

[Serializable]
public struct IntRange
{
    [SerializeField]
    private int _min;
    [SerializeField]
    private int _max;

    public int min
    {
        set { _min = Math.Min(value, _max); }
        get { return _min; }
    }

    public int max
    {
        set { _max = Math.Max(_min, value); }
        get { return _max; }
    }

    public int count
    {
        get { return _max - _min; }
    }

    public IntRange(int min, int max)
    {
        _min = min;
        _max = max;
    }

    public int Repeat(int t)
    {
        int length = _max - _min + 1;
        return ((t - _min) % length + length) % length + _min;
    }

    public static IntRange operator -(IntRange range)
    {
        return new IntRange(-range._max, -range._min);
    }

    public override string ToString()
    {
        return string.Format("[{0}; {1}]", _min, _max);
    }
}

[Serializable]
public struct FloatRange
{
    public static FloatRange zero = new FloatRange(0.0f, 0.0f);

    [SerializeField]
    private float _min;
    [SerializeField]
    private float _max;

    public float min
    {
        set { _min = Math.Min(value, _max); }
        get { return _min; }
    }

    public float max
    {
        set { _max = Math.Max(_min, value); }
        get { return _max; }
    }

    public float distance
    {
        get { return _max - _min; }
    }

    public FloatRange(float min, float max)
    {
        _min = min;
        _max = max;
    }

    public bool Contains(float value)
    {
        return value >= _min && value <= _max;
    }

    public float Lerp(float t)
    {
        return Mathf.Lerp(_min, _max, t);
    }

    public float InverseLerp(float value)
    {
        return Mathf.InverseLerp(_min, _max, value);
    }

    public float InverseLerp(float value, AnimationCurve curve)
    {
        float t = Mathf.InverseLerp(_min, _max, value);
        return curve.Evaluate(t);
    }

    public static FloatRange Lerp(FloatRange left, FloatRange right, float t)
    {
        float min = Mathf.Lerp(left.min, right.min, t);
        float max = Mathf.Lerp(left.max, right.max, t);
        return new FloatRange(min, max);
    }

    public static FloatRange operator -(FloatRange range)
    {
        return new FloatRange(-range._max, -range._min);
    }

    public override string ToString()
    {
        return string.Format("[{0}; {1}]", _min, _max);
    }
}

[Serializable]
public struct Vector2Range
{
    [SerializeField]
    private Vector2 _min;
    [SerializeField]
    private Vector2 _max;

    public Vector2 min
    {
        set { _min = Vector2.Min(value, _max); }
        get { return _min; }
    }

    public Vector2 max
    {
        set { _max = Vector2.Max(_min, value); }
        get { return _max; }
    }

    public Vector2Range(Vector2 min, Vector2 max)
    {
        _min = min;
        _max = max;
    }

    public override string ToString()
    {
        return string.Format("[{0}; {1}]", _min, _max);
    }
}

[Serializable]
public struct Vector3Range
{
    [SerializeField]
    private Vector3 _min;
    [SerializeField]
    private Vector3 _max;

    public Vector3 min
    {
        set { _min = Vector3.Min(value, _max); }
        get { return _min; }
    }

    public Vector3 max
    {
        set { _max = Vector3.Max(_min, value); }
        get { return _max; }
    }

    public Vector3Range(Vector3 min, Vector3 max)
    {
        _min = min;
        _max = max;
    }

    public override string ToString()
    {
        return string.Format("[{0}; {1}]", _min, _max);
    }
}

public static class RangeExt
{
    public static int Random(this IntRange range, bool inclusive = true)
    {
        return UnityEngine.Random.Range(range.min, range.max + (inclusive ? 1 : 0));
    }

    public static float Random(this FloatRange range)
    {
        return UnityEngine.Random.Range(range.min, range.max);
    }

    public static Vector2 Random(this Vector2Range range)
    {
        var result = new Vector2()
        {
            x = UnityEngine.Random.Range(range.min.x, range.max.x),
            y = UnityEngine.Random.Range(range.min.y, range.max.y)
        };
        return result;
    }

    public static Vector3 Random(this Vector3Range range)
    {
        var result = new Vector3()
        {
            x = UnityEngine.Random.Range(range.min.x, range.max.x),
            y = UnityEngine.Random.Range(range.min.y, range.max.y),
            z = UnityEngine.Random.Range(range.min.z, range.max.z)
        };
        return result;
    }

    public static int Clamp(this IntRange range, int value)
    {
        return Mathf.Clamp(value, range.min, range.max);
    }

    public static float Clamp(this FloatRange range, float value)
    {
        return Mathf.Clamp(value, range.min, range.max);
    }

    public static float Lerp(this FloatRange range, float t)
    {
        return Mathf.Lerp(range.min, range.max, t);
    }

    public static float Lerp(this FloatRange range, float t, AnimationCurve curve)
    {
        float p = curve.Evaluate(t);
        return Mathf.Lerp(range.min, range.max, p);
    }

    public static float LerpClamp(this FloatRange range, float t)
    {
        return MathExt.LerpClamp(range.min, range.max, t);
    }

    public static float LerpClamp(this FloatRange range, float t, AnimationCurve curve)
    {
        float p = curve.Evaluate(t);
        return MathExt.LerpClamp(range.min, range.max, p);
    }
}

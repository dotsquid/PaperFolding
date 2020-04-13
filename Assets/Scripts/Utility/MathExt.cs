using UnityEngine;

public static class MathExt
{
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        return Quaternion.Euler(0, 0, degrees) * v;
    }

    public static Vector2 Rotate(this Vector2 v, float degrees, Vector2 pivot)
    {
        Vector2 dir = v - pivot;
        return pivot + (Vector2)(Quaternion.Euler(0, 0, degrees) * dir);
    }

    public static Vector2 Clamp(this Vector2 vector, Rect rect)
    {
        var x = vector.x;
        var y = vector.y;
        x = Mathf.Min(x, rect.xMax);
        y = Mathf.Min(y, rect.yMax);
        x = Mathf.Max(x, rect.xMin);
        y = Mathf.Max(y, rect.yMin);
        vector.x = x;
        vector.y = y;
        return vector;
    }

    public static float InverseLerp(float min_t, float max_t, float val)
    {
        if (min_t != max_t)
            return (val - min_t) / (max_t - min_t);
        else
            return 0.0f;
    }

    public static float InverseLerp(FloatRange range, float val)
    {
        float min_t = range.min;
        float max_t = range.max;
        if (min_t != max_t)
            return (val - min_t) / (max_t - min_t);
        else
            return 0.0f;
    }

    public static float InverseLerp(float min_t, float max_t, float val, AnimationCurve curve)
    {
        float t = 0.0f;
        if (min_t != max_t)
            t = (val - min_t) / (max_t - min_t);
        return curve.Evaluate(t);
    }

    public static float InverseLerp(FloatRange range, float val, AnimationCurve curve)
    {
        float t = 0.0f;
        float min_t = range.min;
        float max_t = range.max;
        if (min_t != max_t)
            t = (val - min_t) / (max_t - min_t);
        return curve.Evaluate(t);
    }

    public static float Lerp(float min_t, float max_t, float ratio)
    {
        return min_t + ratio * (max_t - min_t);
    }

    public static Vector2 Lerp(Vector2 min_t, Vector2 max_t, float ratio)
    {
        return min_t + ratio * (max_t - min_t);
    }

    public static Vector3 Lerp(Vector3 min_t, Vector3 max_t, float ratio)
    {
        return min_t + ratio * (max_t - min_t);
    }

    public static float Lerp(float val_t, float min_t, float max_t, float min_r, float max_r)
    {
        return min_r + (max_r - min_r) * (val_t - min_t) / (max_t - min_t);
    }

    public static float Lerp(float val_t, float min_t, float max_t, float min_r, float max_r, AnimationCurve curve)
    {
        float p = (val_t - min_t) / (max_t - min_t);
        return min_r + (max_r - min_r) * curve.Evaluate(p);
    }

    public static float Lerp(float val_t, FloatRange t, FloatRange r)
    {
        return r.min + (r.max - r.min) * (val_t - t.min) / (t.max - t.min);
    }

    public static float Lerp(float val_t, FloatRange t, FloatRange r, AnimationCurve curve)
    {
        float p = (val_t - t.min) / (t.max - t.min);
        return r.min + (r.max - r.min) * curve.Evaluate(p);
    }

    public static float LerpClamp(float min_t, float max_t, float ratio)
    {
        float r = Mathf.Clamp01(ratio);
        return Lerp(min_t, max_t, r);
    }

    public static Vector2 LerpClamp(Vector2 min_t, Vector2 max_t, float ratio)
    {
        float r = Mathf.Clamp01(ratio);
        return Lerp(min_t, max_t, r);
    }

    public static Vector3 LerpClamp(Vector3 min_t, Vector3 max_t, float ratio)
    {
        float r = Mathf.Clamp01(ratio);
        return Lerp(min_t, max_t, r);
    }

    public static float LerpClamp(float val_t, float min_t, float max_t, float min_r, float max_r)
    {
        float real_min_r = System.Math.Min(min_r, max_r);
        float real_max_r = System.Math.Max(min_r, max_r);
        float val = min_r + (max_r - min_r) * (val_t - min_t) / (max_t - min_t);
        return Mathf.Clamp(val, real_min_r, real_max_r);
    }

    public static float LerpClamp(float val_t, float min_t, float max_t, float min_r, float max_r, AnimationCurve curve)
    {
        float real_min_r = System.Math.Min(min_r, max_r);
        float real_max_r = System.Math.Max(min_r, max_r);
        float p = (val_t - min_t) / (max_t - min_t);
        float val = min_r + (max_r - min_r) * curve.Evaluate(p);
        return Mathf.Clamp(val, real_min_r, real_max_r);
    }

    public static float LerpClamp(float val_t, FloatRange t, FloatRange r)
    {
        float real_min_r = System.Math.Min(r.min, r.max);
        float real_max_r = System.Math.Max(r.min, r.max);
        float val = r.min + (r.max - r.min) * (val_t - t.min) / (t.max - t.min);
        return Mathf.Clamp(val, real_min_r, real_max_r);
    }

    public static float LerpClamp(float val_t, FloatRange t, FloatRange r, AnimationCurve curve)
    {
        float real_min_r = System.Math.Min(r.min, r.max);
        float real_max_r = System.Math.Max(r.min, r.max);
        float p = (val_t - t.min) / (t.max - t.min);
        float val = r.min + (r.max - r.min) * curve.Evaluate(p);
        return Mathf.Clamp(val, real_min_r, real_max_r);
    }
}

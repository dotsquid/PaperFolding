using UnityEngine;

public static class MathExt
{
    public const double PI = System.Math.PI;
    public const double TWO_PI = PI * 2.0;
    public const double HALF_PI = PI * 0.5;

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

    // Floating-point modulo
    // The result (the remainder) has same sign as the divisor.
    // Similar to matlab's mod(); Not similar to fmod() -   Mod(-3,4)= 1   fmod(-3,4)= -3
    public static float Mod(float x, float y)
    {
        if (0.0 == y)
            return x;

        float m = x - y * (float)System.Math.Floor(x / y);

        // handle boundary cases resulted from floating-point cut off:

        if (y > 0)              // modulo range: [0..y)
        {
            if (m >= y)           // Mod(-1e-16             , 360.    ): m= 360.
                return 0;

            if (m < 0)
            {
                if (y + m == y)
                    return 0; // just in case...
                else
                    return y + m; // Mod(106.81415022205296 , m_TWO_PI ): m= -1.421e-14 
            }
        }
        else                    // modulo range: (y..0]
        {
            if (m <= y)           // Mod(1e-16              , -360.   ): m= -360.
                return 0;

            if (m > 0)
            {
                if (y + m == y)
                    return 0; // just in case...
                else
                    return y + m; // Mod(-106.81415022205296, -m_TWO_PI): m= 1.421e-14 
            }
        }

        return m;
    }

    public static double Mod(double x, double y)
    {
        if (0.0 == y)
            return x;

        double m = x - y * System.Math.Floor(x / y);

        // handle boundary cases resulted from floating-point cut off:

        if (y > 0)              // modulo range: [0..y)
        {
            if (m >= y)           // Mod(-1e-16             , 360.    ): m= 360.
                return 0;

            if (m < 0)
            {
                if (y + m == y)
                    return 0; // just in case...
                else
                    return y + m; // Mod(106.81415022205296 , m_TWO_PI ): m= -1.421e-14 
            }
        }
        else                    // modulo range: (y..0]
        {
            if (m <= y)           // Mod(1e-16              , -360.   ): m= -360.
                return 0;

            if (m > 0)
            {
                if (y + m == y)
                    return 0; // just in case...
                else
                    return y + m; // Mod(-106.81415022205296, -m_TWO_PI): m= 1.421e-14 
            }
        }

        return m;
    }

    // wrap [rad] angle to [-PI..PI)
    public static float WrapPosNegPI(float fAng)
    {
        return Mod(fAng + (float)PI, (float)TWO_PI) - (float)PI;
    }

    public static double WrapPosNegPI(double fAng)
    {
        return Mod(fAng + PI, TWO_PI) - PI;
    }

    // wrap [rad] angle to [0..TWO_PI)
    public static float WrapTwoPI(float fAng)
    {
        return Mod(fAng, (float)TWO_PI);
    }

    public static double WrapTwoPI(double fAng)
    {
        return Mod(fAng, TWO_PI);
    }

    // wrap [deg] angle to [-180..180)
    public static float WrapPosNeg180(float fAng)
    {
        return Mod(fAng + 180.0f, 360.0f) - 180.0f;
    }

    public static double WrapPosNeg180(double fAng)
    {
        return Mod(fAng + 180.0, 360.0) - 180.0;
    }

    // wrap [deg] angle to [0..360)
    public static float Wrap360(float fAng)
    {
        return Mod(fAng, 360.0f);
    }

    public static double Wrap360(double fAng)
    {
        return Mod(fAng, 360.0);
    }
}

using UnityEngine;
using UnityEngine.UI;

public static class ColorUtility
{
    public static void CopyFrom(this Gradient self, Gradient other)
    {
        self.alphaKeys = other.alphaKeys;
        self.colorKeys = other.colorKeys;
    }

    public static Color SetAlpha(this Color self, float alpha)
    {
        Color color = self;
        color.a = alpha;
        return color;
    }

    public static Color SetOpaque(this Color self, Color other)
    {
        Color color = other;
        color.a = self.a;
        return color;
    }

    public static void SetAlpha(this SpriteRenderer source, float alpha)
    {
        Color color = source.color;
        color.a = alpha;
        source.color = color;
    }

    public static void SetOpaque(this SpriteRenderer source, Color other)
    {
        Color color = other;
        color.a = source.color.a;
        source.color = color;
    }

    public static void SetAlpha(this Graphic source, float alpha)
    {
        Color color = source.color;
        color.a = alpha;
        source.color = color;
    }

    public static void SetOpaque(this Graphic source, Color other)
    {
        Color color = other;
        color.a = source.color.a;
        source.color = color;
    }
}

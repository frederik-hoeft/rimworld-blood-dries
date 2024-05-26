using UnityEngine;

namespace BloodDries.Extensions;

internal static class ColorExtensions
{
    public static Color Lerp(this Color color, Color other, float percentage, float? alphaPercentage = null)
    {
        float alpha = Mathf.Lerp(color.a, other.a, alphaPercentage ?? percentage);
        float red = Mathf.Lerp(color.r, other.r, percentage);
        float green = Mathf.Lerp(color.g, other.g, percentage);
        float blue = Mathf.Lerp(color.b, other.b, percentage);
        return new Color(red, green, blue, alpha);
    }
}

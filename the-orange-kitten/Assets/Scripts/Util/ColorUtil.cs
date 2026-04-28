using UnityEngine;

namespace Util
{
    public static class ColorUtil
    {
        public static Color WithRGBA(this Color color, float r, float g, float b, float a)
        {
            color.r = r;
            color.g = g;
            color.b = b;
            color.a = a;
            return color;
        }

        public static Color WithRGB(this Color color, float r, float g, float b, float a) =>
            color.WithRGBA(r, g, b, color.a);

        public static Color WithAlpha(this Color color, float alpha) =>
            color.WithRGBA(color.r, color.g, color.b, alpha);
    }
}
using UnityEngine.UI;

namespace Hypercasual
{
    public static class Extensions
    {
        public static T ChangeAlpha<T>(this T g, float newAlpha)
            where T : Graphic
        {
            var color = g.color;
            color.a = newAlpha;
            g.color = color;
            return g;
        }
    }
}
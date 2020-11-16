using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Helpers
{
    public static class ColorExtensions
    {
        public static Color GetRandomColor(float alpha = 1.0f)
        {
            return new Color(Random.value, Random.value, Random.value, alpha);
        }

        public static Color GetRandomGray(float min = 0.4f, float max = 0.8f, float alpha = 1.0f)
        {
            var shade = Random.Range(min, max);
            return new Color(shade, shade, shade, alpha);
        }

        public static Color ToColor(this float[] arr)
        {
            return new Color(arr[0], arr[1], arr[2], arr[3]);
        }

        internal static Color GetColorFromHex(this string hexString)
        {
            Color col;
            hexString = "#" + hexString.Trim('#');
            if (ColorUtility.TryParseHtmlString(hexString, out col))
            {
                return col;
            }

            throw new Exception("Unable to parse color");
        }

        internal static string ToColorHexString(this Color color)
        {
            return "#" + ColorUtility.ToHtmlStringRGBA(color).Trim('#');
        }

    }
}
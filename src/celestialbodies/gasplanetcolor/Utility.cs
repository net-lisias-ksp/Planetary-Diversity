using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PlanetaryDiversity.CelestialBodies.GasPlanetColor
{
    /// <summary>
    /// Contains generic functions for color and texture manipulating
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Modifies a color so that it becomes a multiplier color
        /// </summary>
        public static Color ReColor(Color c, Color average)
        {
            // Do some maths..
            return new Color(c.r / average.r, c.g / average.g, c.b / average.b, 1);
        }

        /// <summary>
        /// An array of all XKCD colors
        /// </summary>
        private static Color[] _colors;

        public static Color[] colors
        {
            get
            {
                if (_colors == null)
                    _colors = typeof(XKCDColors).GetProperties(BindingFlags.Public | BindingFlags.Static).Where(p => p.PropertyType == typeof(Color)).Select(p => (Color)p.GetValue(null, null)).ToArray();
                return _colors;
            }
        }

        // Converts an unreadable texture into a readable one
        public static Texture2D CreateReadable(Texture2D original)
        {
            // Checks
            if (original == null) return null;
            if (original.width == 0 || original.height == 0) return null;

            // Create the new texture
            Texture2D finalTexture = new Texture2D(original.width, original.height);

            // isn't read or writeable ... we'll have to get tricksy
            RenderTexture rt = RenderTexture.GetTemporary(original.width, original.height, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB, 1);
            Graphics.Blit(original, rt);
            RenderTexture.active = rt;

            // Load new texture
            finalTexture.ReadPixels(new Rect(0, 0, finalTexture.width, finalTexture.height), 0, 0);

            // Kill the old one
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);

            // Return
            return finalTexture;
        }

        /// <summary>
        /// Extracts the average color from a texture file
        /// </summary>
        public static Color GetAverageColor(Texture2D texture)
        {
            Byte avgB;
            Byte avgG;
            Byte avgR;
            Int64[] totals = { 0, 0, 0 };

            Int64 width = texture.width;
            Int64 height = texture.height;

            for (Int32 y = 0; y < height; y++)
            {
                for (Int32 x = 0; x < width; x++)
                {
                    Color32 c = texture.GetPixel(x, y);
                    totals[0] += c.b;
                    totals[1] += c.g;
                    totals[2] += c.r;
                }
            }

            avgB = (Byte)(totals[0] / (width * height));
            avgG = (Byte)(totals[1] / (width * height));
            avgR = (Byte)(totals[2] / (width * height));
            return new Color32(avgR, avgG, avgB, 255);
        }
    }
}

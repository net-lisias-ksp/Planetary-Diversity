using PlanetaryDiversity.API;
using System;
using UnityEngine;

namespace PlanetaryDiversity.CelestialBodies.GasPlanetColor
{
    /// <summary>
    /// Changes the colors of Gas Planets
    /// </summary>
    public class ColorTweak : CelestialBodyTweaker
    {
        /// <summary>
        /// Returns the name of the config node that stores the configuration
        /// </summary>
        public override String GetConfig() => "PD_CELESTIAL";

        /// <summary>
        /// Returns the name of the config option that can be used to disable the tweak
        /// </summary>
        public override String GetSetting() => "GasPlanetColor";

        /// <summary>
        /// Changes the parameters of the body
        /// </summary>
        public override Boolean Tweak(CelestialBody body)
        {
            // Is the body a GasPlanet?
            if (body.hasSolidSurface || body.scaledBody.GetComponentsInChildren<SunShaderController>().Length != 0)
                return false;

            // Tweak the color (this only has a chance of 50% to happen
            if (GetRandom(HighLogic.CurrentGame.Seed, 0, 100) < 50)
            {

                // Get the material of the body
                Material material = body.scaledBody.GetComponent<Renderer>().sharedMaterial;

                // Get the average color of the current texture
                Texture2D diffuseMap = Utility.CreateReadable((Texture2D)material.GetTexture("_MainTex"));
                Color average = Utility.GetAverageColor(diffuseMap);

                // Select a new color and apply it
                Color newColor = GetRandomElement(HighLogic.CurrentGame.Seed, Utility.colors);
                material.color = Utility.ReColor(newColor, average);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Makes a color a bit different
        /// </summary>
        private Color AlterColor(Color c)
        {
            return new Color((Single)Math.Min(1, c.r * GetRandomDouble(HighLogic.CurrentGame.Seed, 0.92, 1.05)),
                             (Single)Math.Min(1, c.g * GetRandomDouble(HighLogic.CurrentGame.Seed, 0.92, 1.05)),
                             (Single)Math.Min(1, c.b * GetRandomDouble(HighLogic.CurrentGame.Seed, 0.92, 1.05)), c.a);
        }
    }
}

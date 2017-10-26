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
                Color newColor = Dark(GetRandomElement(HighLogic.CurrentGame.Seed, Utility.colors));
                material.color = Utility.ReColor(newColor, average);

                // Does this planet have an atmosphere?
                if (!body.atmosphere)
                    return true;

                // Recolor the Atmosphere from Ground
                Color afgColor = new Color(1 - newColor.r, 1 - newColor.g, 1 - newColor.b, 1 - newColor.a);
                if (body.afg != null)
                {
                    body.afg.waveLength = afgColor;
                    EventData<AtmosphereFromGround> afgEvent = GameEvents.FindEvent<EventData<AtmosphereFromGround>>("Kopernicus.RuntimeUtility.PatchAFG");
                    afgEvent?.Add((afg) => afg.waveLength = afgColor);
                }
                body.atmosphericAmbientColor = newColor;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Makes a color darker
        /// </summary>
        private static Color Dark(Color c)
        {
            if ((c.r > 0.5) || (c.g > 0.5) || (c.b > 0.5))
            {
                c = c * new Color(0.5f, 0.5f, 0.5f);
            }
            return c;
        }
    }
}

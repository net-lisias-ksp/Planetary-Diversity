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

                // Does this planet have an atmosphere?
                if (!body.atmosphere)
                    return true;

                // Recolor the Atmosphere from Ground
                Color afgColor = new Color(1 - newColor.r, 1 - newColor.g, 1 - newColor.b, 1 - newColor.a);
                if (body.afg != null)
                {
                    body.afg.waveLength = afgColor;
                    EventData<AtmosphereFromGround> afgEvent = GameEvents.FindEvent<EventData<AtmosphereFromGround>>("Kopernicus.RuntimeUtility.PatchAFG");
                    if (afgEvent != null)
                        afgEvent.Add((afg) => afg.waveLength = afgColor);
                }
                body.atmosphericAmbientColor = newColor;

                // Recolor the atmosphere gradient
                Gradient gradient = new Gradient();
                gradient.Add(0.0f, newColor);
                gradient.Add(0.6f, new Color(0.0549f, 0.0784f, 0.141f, 1f));
                gradient.Add(1.0f, new Color(0.0196f, 0.0196f, 0.0196f, 1f));

                // Generate a ramp from the gradient 
                Texture2D ramp = new Texture2D(512, 1);
                Color[] colors = ramp.GetPixels(0);
                for (Int32 i = 0; i < colors.Length; i++)
                {
                    // Compute the position in the gradient 
                    Single k = (Single)i / colors.Length;
                    colors[i] = gradient.ColorAt(k);
                }
                ramp.SetPixels(colors, 0);
                ramp.Apply(true, false);

                // Set the color ramp 
                Renderer renderer = body.scaledBody.GetComponent<Renderer>();
                renderer.sharedMaterial.SetTexture("_rimColorRamp", ramp);
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

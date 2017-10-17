using PlanetaryDiversity.API;
using System;
using System.Linq;
using UnityEngine;

namespace PlanetaryDiversity.CelestialBodies.Atmosphere
{
    /// <summary>
    /// Changes the pressure and temperature curves of the planets
    /// </summary>
    public class AtmosphereTweak : CelestialBodyTweaker
    {
        /// <summary>
        /// Returns the name of the config node that stores the configuration
        /// </summary>
        public override String GetConfig() => "PD_CELESTIAL";

        /// <summary>
        /// Returns the name of the config option that can be used to disable the tweak
        /// </summary>
        public override String GetSetting() => "Atmosphere";

        /// <summary>
        /// Changes the parameters of the body
        /// </summary>
        public override Boolean Tweak(CelestialBody body)
        {
            // Does this body have an atmosphere?
            if (!body.atmosphere)
                return false;

            // Prefab
            PSystemBody pSystemBody = Resources.FindObjectsOfTypeAll<PSystemBody>().FirstOrDefault(b => b.celestialBody.bodyName == body.transform.name);

            // Get a multiplier
            Single mult = (Single)GetRandomDouble(HighLogic.CurrentGame.Seed, 0.9, 1.1);

            // Apply it to both curves
            if (body.atmosphereUsePressureCurve) 
            {
                body.atmospherePressureCurve = new FloatCurve(pSystemBody.celestialBody.atmospherePressureCurve.Curve.keys.Select(k => new Keyframe(k.time, k.value * mult, k.inTangent, k.outTangent)).ToArray());
                body.atmospherePressureSeaLevel *= mult;
            }
            if (body.atmosphereUseTemperatureCurve)
            {
                body.atmosphereTemperatureCurve = new FloatCurve(pSystemBody.celestialBody.atmosphereTemperatureCurve.Curve.keys.Select(k => new Keyframe(k.time, k.value * mult, k.inTangent, k.outTangent)).ToArray());
                body.atmosphereTemperatureSeaLevel *= mult;
            }

            // Did we tweak something?
            return body.atmosphereUsePressureCurve || body.atmosphereUseTemperatureCurve;
        }
    }
}

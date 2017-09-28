using PlanetaryDiversity.API;
using System;
using System.Linq;
using UnityEngine;

namespace PlanetaryDiversity.CelestialBodies.Orbit
{
    /// <summary>
    /// Changes the Orbits of the Celestial Bodies
    /// </summary>
    public class OrbitTweak : CelestialBodyTweaker
    {
        /// <summary>
        /// Returns the name of the config node that stores the configuration
        /// </summary>
        public override String GetConfig() => "PD_CELESTIAL";

        /// <summary>
        /// Returns the name of the config option that can be used to disable the tweak
        /// </summary>
        public override String GetSetting() => "Orbit";

        /// <summary>
        /// Changes the parameters of the body
        /// </summary>
        public override Boolean Tweak(CelestialBody body)
        {
            // Does the body have an orbit?
            if (body.orbit == null)
                return false;

            // Prefab
            PSystemBody pSystemBody = Resources.FindObjectsOfTypeAll<PSystemBody>().FirstOrDefault(b => b.celestialBody.bodyName == body.transform.name);

            // Inclination
            if (GetRandom(HighLogic.CurrentGame.Seed, 0, 100) < 20)
            {
                body.orbitDriver.orbit.inclination = GetRandomDouble(HighLogic.CurrentGame.Seed, -3, 3);
            }

            // SMA
            body.orbitDriver.orbit.semiMajorAxis = pSystemBody.orbitDriver.orbit.semiMajorAxis * GetRandomDouble(HighLogic.CurrentGame.Seed, 0.8, 1.2);

            // LAN
            body.orbitDriver.orbit.LAN = GetRandom(HighLogic.CurrentGame.Seed, 0, 361);

            // MeanAnomalyAtEpoch
            body.orbitDriver.orbit.meanAnomalyAtEpoch = GetRandom(HighLogic.CurrentGame.Seed, 0, 361) * Math.PI / 180d;

            // argumentOfPeriapsis
            body.orbitDriver.orbit.argumentOfPeriapsis = GetRandom(HighLogic.CurrentGame.Seed, 0, 361);

            // Eccentricy
            body.orbitDriver.orbit.eccentricity = pSystemBody.orbitDriver.orbit.eccentricity * GetRandomDouble(HighLogic.CurrentGame.Seed, 0.8, 1.2);

            // Update
            body.orbitDriver.UpdateOrbit();
            body.CBUpdate();

            // Done
            return true;
        }
    }
}

using PlanetaryDiversity.API;
using System;

namespace PlanetaryDiversity.PQSMods.Seed
{
    /// <summary>
    /// Tweaks the seed of the LandControl PQSMod
    /// </summary>
    public class LandControlTweak : PQSModTweaker<PQSLandControl>
    {
        /// <summary>
        /// Returns the name of the config node that stores the configuration
        /// </summary>
        public override String GetConfig() => "PD_PQSMODS_SEED";

        /// <summary>
        /// Returns the name of the config option that can be used to disable the tweak
        /// </summary>
        public override String GetSetting() => "LandControl";

        /// <summary>
        /// Changes the parameters of the PQSMod
        /// </summary>
        public override Boolean Tweak(CelestialBody body, PQSLandControl mod)
        {
            // Get the game seed and apply it
            mod.altitudeSeed = GetRandom(HighLogic.CurrentGame.Seed);
            mod.latitudeSeed = GetRandom(HighLogic.CurrentGame.Seed);
            mod.longitudeSeed = GetRandom(HighLogic.CurrentGame.Seed);
            if (mod.altitudeSimplex != null)
                mod.altitudeSimplex.seed = GetRandom(HighLogic.CurrentGame.Seed);
            if (mod.latitudeSimplex != null)
                mod.latitudeSimplex.seed = GetRandom(HighLogic.CurrentGame.Seed);
            if (mod.longitudeSimplex != null)
                mod.longitudeSimplex.seed = GetRandom(HighLogic.CurrentGame.Seed);

            // Apply it to land classes
            if (mod.landClasses != null)
            {
                for (Int32 i = 0; i < mod.landClasses.Length; i++)
                {
                    PQSLandControl.LandClass landClass = mod.landClasses[i];
                    landClass.coverageSeed = GetRandom(HighLogic.CurrentGame.Seed);
                    landClass.noiseSeed = GetRandom(HighLogic.CurrentGame.Seed);
                    if (landClass.coverageSimplex != null)
                        landClass.coverageSimplex.seed = GetRandom(HighLogic.CurrentGame.Seed);
                    if (landClass.noiseSimplex != null)
                        landClass.noiseSimplex.seed = GetRandom(HighLogic.CurrentGame.Seed);
                }
            }

            // Apply it to scatters
            if (mod.scatters != null)
            {
                for (Int32 i = 0; i < mod.scatters.Length; i++)
                {
                    mod.scatters[i].seed = GetRandom(HighLogic.CurrentGame.Seed);
                }
            }

            // We changed something
            return true;
        }
    }
}
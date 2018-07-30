using PlanetaryDiversity.API;
using System;

namespace PlanetaryDiversity.PQSMods.Seed
{
    /// <summary>
    /// Tweaks the seed of the VoronoiCraters PQSMod
    /// </summary>
    public class VoronoiCratersTweak : PQSModTweaker<PQSMod_VoronoiCraters>
    {
        /// <summary>
        /// Returns the name of the config node that stores the configuration
        /// </summary>
        public override String GetConfig() => "PD_PQSMODS_SEED";

        /// <summary>
        /// Returns the name of the config option that can be used to disable the tweak
        /// </summary>
        public override String GetSetting() => "VoronoiCraters";

        /// <summary>
        /// Changes the parameters of the PQSMod
        /// </summary>
        public override Boolean Tweak(CelestialBody body, PQSMod_VoronoiCraters mod)
        {
            // Get the game seed and apply it
            mod.simplexSeed = GetRandom(HighLogic.CurrentGame.Seed);
            mod.voronoiSeed = GetRandom(HighLogic.CurrentGame.Seed);

            // We changed something
            return true;
        }
    }
}

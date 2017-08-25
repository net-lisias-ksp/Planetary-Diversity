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
            // Get the game seed
            Int32 seed = HighLogic.CurrentGame.Seed;
            Random random = new Random(seed);

            // Apply it
            mod.simplexSeed = random.Next();
            mod.voronoiSeed = random.Next();

            // We changed something
            return true;
        }
    }
}

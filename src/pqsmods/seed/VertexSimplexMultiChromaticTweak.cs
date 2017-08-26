using PlanetaryDiversity.API;
using System;

namespace PlanetaryDiversity.PQSMods.Seed
{
    /// <summary>
    /// Tweaks the seed of the VertexSimplexMultiChromatic PQSMod
    /// </summary>
    public class VertexSimplexMultiChromaticTweak : PQSModTweaker<PQSMod_VertexSimplexMultiChromatic>
    {
        /// <summary>
        /// Returns the name of the config node that stores the configuration
        /// </summary>
        public override String GetConfig() => "PD_PQSMODS_SEED";

        /// <summary>
        /// Returns the name of the config option that can be used to disable the tweak
        /// </summary>
        public override String GetSetting() => "VertexSimplexMultiChromatic";

        /// <summary>
        /// Changes the parameters of the PQSMod
        /// </summary>
        public override Boolean Tweak(CelestialBody body, PQSMod_VertexSimplexMultiChromatic mod)
        {
            // Get the game seed and apply it
            mod.alphaSeed = GetRandom(HighLogic.CurrentGame.Seed);
            mod.redSeed = GetRandom(HighLogic.CurrentGame.Seed);
            mod.greenSeed = GetRandom(HighLogic.CurrentGame.Seed);
            mod.blueSeed = GetRandom(HighLogic.CurrentGame.Seed);

            // We changed something
            return true;
        }
    }
}
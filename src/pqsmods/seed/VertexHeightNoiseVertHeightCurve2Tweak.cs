using PlanetaryDiversity.API;
using System;

namespace PlanetaryDiversity.PQSMods.Seed
{
    /// <summary>
    /// Tweaks the seed of the VertexHeightNoiseVertHeightCurve2 PQSMod
    /// </summary>
    public class VertexHeightNoiseVertHeightCurve2Tweak : PQSModTweaker<PQSMod_VertexHeightNoiseVertHeightCurve2>
    {
        /// <summary>
        /// Returns the name of the config node that stores the configuration
        /// </summary>
        public override String GetConfig() => "PD_PQSMODS_SEED";

        /// <summary>
        /// Returns the name of the config option that can be used to disable the tweak
        /// </summary>
        public override String GetSetting() => "VertexHeightNoiseVertHeightCurve2";

        /// <summary>
        /// Changes the parameters of the PQSMod
        /// </summary>
        public override Boolean Tweak(CelestialBody body, PQSMod_VertexHeightNoiseVertHeightCurve2 mod)
        {
            // Get the game seed and apply it
            mod.ridgedAddSeed = GetRandom(HighLogic.CurrentGame.Seed);
            mod.ridgedSubSeed = GetRandom(HighLogic.CurrentGame.Seed);
            mod.simplexSeed = GetRandom(HighLogic.CurrentGame.Seed);

            // We changed something
            return true;
        }
    }
}
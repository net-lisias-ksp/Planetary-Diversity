using PlanetaryDiversity.API;
using System;

namespace PlanetaryDiversity.PQSMods.Seed
{
    /// <summary>
    /// Tweaks the seed of the VertexHeightNoiseVertHeightCurve PQSMod
    /// </summary>
    public class VertexHeightNoiseVertHeightCurveTweak : PQSModTweaker<PQSMod_VertexHeightNoiseVertHeightCurve>
    {
        /// <summary>
        /// Returns the name of the config node that stores the configuration
        /// </summary>
        public override String GetConfig() => "PD_PQSMODS_SEED";

        /// <summary>
        /// Returns the name of the config option that can be used to disable the tweak
        /// </summary>
        public override String GetSetting() => "VertexHeightNoiseVertHeightCurve";

        /// <summary>
        /// Changes the parameters of the PQSMod
        /// </summary>
        public override Boolean Tweak(CelestialBody body, PQSMod_VertexHeightNoiseVertHeightCurve mod)
        {
            // Get the game seed and apply it
            mod.seed = GetRandom(HighLogic.CurrentGame.Seed);

            // We changed something
            return true;
        }
    }
}
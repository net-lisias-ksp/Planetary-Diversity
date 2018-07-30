using PlanetaryDiversity.API;
using System;

namespace PlanetaryDiversity.PQSMods.Seed
{
    /// <summary>
    /// Tweaks the seed of the VertexRidgedAltitudeCurve PQSMod
    /// </summary>
    public class VertexRidgedAltitudeCurveTweak : PQSModTweaker<PQSMod_VertexRidgedAltitudeCurve>
    {
        /// <summary>
        /// Returns the name of the config node that stores the configuration
        /// </summary>
        public override String GetConfig() => "PD_PQSMODS_SEED";

        /// <summary>
        /// Returns the name of the config option that can be used to disable the tweak
        /// </summary>
        public override String GetSetting() => "VertexRidgedAltitudeCurve";

        /// <summary>
        /// Changes the parameters of the PQSMod
        /// </summary>
        public override Boolean Tweak(CelestialBody body, PQSMod_VertexRidgedAltitudeCurve mod)
        {
            // Get the game seed and apply it
            mod.ridgedAddSeed = GetRandom(HighLogic.CurrentGame.Seed);
            mod.simplexSeed = GetRandom(HighLogic.CurrentGame.Seed);

            // We changed something
            return true;
        }
    }
}
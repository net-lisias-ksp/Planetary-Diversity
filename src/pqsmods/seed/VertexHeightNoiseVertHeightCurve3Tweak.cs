using PlanetaryDiversity.API;
using System;

namespace PlanetaryDiversity.PQSMods.Seed
{
    /// <summary>
    /// Tweaks the seed of the VertexHeightNoiseVertHeightCurve3 PQSMod
    /// </summary>
    public class VertexHeightNoiseVertHeightCurve3Tweak : PQSModTweaker<PQSMod_VertexHeightNoiseVertHeightCurve3>
    {
        /// <summary>
        /// Returns the name of the config node that stores the configuration
        /// </summary>
        public override String GetConfig() => "PD_PQSMODS_SEED";

        /// <summary>
        /// Returns the name of the config option that can be used to disable the tweak
        /// </summary>
        public override String GetSetting() => "VertexHeightNoiseVertHeightCurve3";

        /// <summary>
        /// Changes the parameters of the PQSMod
        /// </summary>
        public override Boolean Tweak(CelestialBody body, PQSMod_VertexHeightNoiseVertHeightCurve3 mod)
        {
            // Get the game seed and apply it
            if (mod.curveMultiplier != null)
                mod.curveMultiplier.seed = GetRandom(HighLogic.CurrentGame.Seed);
            if (mod.deformity != null)
                mod.deformity.seed = GetRandom(HighLogic.CurrentGame.Seed);
            if (mod.ridgedAdd != null)
                mod.ridgedAdd.seed = GetRandom(HighLogic.CurrentGame.Seed);
            if (mod.ridgedSub != null) 
                mod.ridgedSub.seed = GetRandom(HighLogic.CurrentGame.Seed);

            // We changed something
            return true;
        }
    }
}

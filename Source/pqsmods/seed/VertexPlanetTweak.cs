using PlanetaryDiversity.API;
using System;

namespace PlanetaryDiversity.PQSMods.Seed
{
    /// <summary>
    /// Tweaks the seed of the VertexPlanet PQSMod
    /// </summary>
    public class VertexPlanetTweak : PQSModTweaker<PQSMod_VertexPlanet>
    {
        /// <summary>
        /// Returns the name of the config node that stores the configuration
        /// </summary>
        public override String GetConfig() => "PD_PQSMODS_SEED";

        /// <summary>
        /// Returns the name of the config option that can be used to disable the tweak
        /// </summary>
        public override String GetSetting() => "VertexPlanet";

        /// <summary>
        /// Changes the parameters of the PQSMod
        /// </summary>
        public override Boolean Tweak(CelestialBody body, PQSMod_VertexPlanet mod)
        {
            // Get the game seed and apply it
            mod.seed = GetRandom(HighLogic.CurrentGame.Seed);
            mod.continental?.Setup(GetRandom(HighLogic.CurrentGame.Seed));
            mod.continentalRuggedness?.Setup(GetRandom(HighLogic.CurrentGame.Seed));
            if (mod.continentalSharpness != null)
                mod.continentalSharpness.seed = GetRandom(HighLogic.CurrentGame.Seed);
            mod.continentalSharpnessMap?.Setup(GetRandom(HighLogic.CurrentGame.Seed));
            mod.terrainType?.Setup(GetRandom(HighLogic.CurrentGame.Seed));

            // Apply it to the land classes
            if (mod.landClasses != null)
            {
                for (Int32 i = 0; i < mod.landClasses.Length; i++)
                {
                    mod.landClasses[i].colorNoiseMap?.Setup(GetRandom(HighLogic.CurrentGame.Seed));
                }
            }

            // We changed something
            return true;
        }
    }
}
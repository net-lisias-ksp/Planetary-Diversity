using System;
using KSP.Localization;

namespace PlanetaryDiversity
{
    /// <summary>
    /// Gives the user an opportunity to set the seed of their savegame
    /// </summary>
    public class SeedParams : GameParameters.CustomParameterNode
    {
        [GameParameters.CustomIntParameterUI("#LOC_PlanetaryDiversity_SeedParams_Seed", newGameOnly = true, minValue = 0, maxValue = Int32.MaxValue, stepSize = 1)]
        public Int32 Seed { get; set; }

        public override String Title => Localizer.Format("#LOC_PlanetaryDiversity_SeedParams_Title");

        public override String Section => "Advanced";

        public override Int32 SectionOrder => 0;

        public override Boolean HasPresets => true;

        public override GameParameters.GameMode GameMode => GameParameters.GameMode.ANY;

        public override String DisplaySection => "#autoLoc_6002170";

        public override void SetDifficultyPreset(GameParameters.Preset preset)
        {
            Seed = new Random(Environment.TickCount ^ Guid.NewGuid().GetHashCode()).Next();
        }

        public SeedParams() : base()
        {
            GameEvents.onGameStateCreated.Add(OnGameStateCreated);
        }

        void OnGameStateCreated(Game game)
        {
            game.Seed = Seed;
        }
    }
}

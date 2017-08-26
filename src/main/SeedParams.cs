/*using System;
using KSP.Localization;
using UnityEngine;

namespace PlanetaryDiversity
{
    /// <summary>
    /// Gives the user an opportunity to set the seed of their savegame
    /// </summary>
    public class SeedParams : GameParameters.CustomParameterNode
    {
        private PopupDialog dialog;

        [GameParameters.CustomParameterUI("#LOC_PlanetaryDiversity_SeedParams_Seed", newGameOnly = true)]
        public Boolean seedSetter
        {
            get { return false; }
            set
            {
                if (!value || dialog != null)
                    return;
                if (Seed == null)
                    Seed = new System.Random(Environment.TickCount ^ Guid.NewGuid().GetHashCode()).Next().ToString();
                dialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("SeedParams",
                    Localizer.Format("#LOC_PlanetaryDiversity_SeedParams_DialogMsg"),
                    Localizer.Format("#LOC_PlanetaryDiversity_SeedParams_DialogTitle"),
                    UISkinManager.GetSkin("MainMenuSkin"),
                    new DialogGUITextInput(
                        Seed, false, 10000, (s) => { Seed = s; return s; }, 24f
                    ),
                    new DialogGUIButton(
                        Localizer.Format("#LOC_PlanetaryDiversity_SeedParams_DialogOK"), () => { dialog.Dismiss(); }, false
                    )
                    ), true, UISkinManager.GetSkin("MainMenuSkin"));
            }
        }
        
        public String Seed { get; set; }

        public override String Title => Localizer.Format("#LOC_PlanetaryDiversity_SeedParams_Title");

        public override String Section => "Advanced";

        public override Int32 SectionOrder => 3;

        public override Boolean HasPresets => false;

        public override GameParameters.GameMode GameMode => GameParameters.GameMode.ANY;

        public override String DisplaySection => "#autoLoc_6002170";

        public SeedParams() : base()
        {
            GameEvents.onGameStateCreated.Add(OnGameStateCreated);
        }

        void OnGameStateCreated(Game game)
        {
            if (Seed == null)
                game.Seed = new System.Random(Environment.TickCount ^ Guid.NewGuid().GetHashCode()).Next();
            if (!Int32.TryParse(Seed, out game.Seed))
                game.Seed = Seed.GetHashCode();
            Seed = null;
        }
    }
}*/

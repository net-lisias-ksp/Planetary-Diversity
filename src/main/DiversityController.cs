using PlanetaryDiversity.API;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlanetaryDiversity
{
    /// <summary>
    /// This class loads config options, collects modifiers from the AppDomain, and runs them
    /// </summary>
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class DiversityController : MonoBehaviour
    {
        /// <summary>
        /// The Singleton for the DiversityController
        /// </summary>
        public static DiversityController Instance { get; set; }

        /// <summary>
        /// A list of all classes that tweak PQSMods
        /// </summary>
        public List<IPQSModTweaker> PQSModTweakers { get; set; }

        /// <summary>
        /// The configurations for the tweaks
        /// </summary>
        public Dictionary<String, ConfigNode> ConfigCache { get; set; }

        /// <summary>
        /// Called when the Component is created
        /// </summary>
        void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(this);
            PQSModTweakers = new List<IPQSModTweaker>();
            ConfigCache = new Dictionary<String, ConfigNode>();
        }

        /// <summary>
        /// Called when the component is activated
        /// </summary>
        void Start()
        {
            // Get all types who extend a Tweaker Interface and add them to the storage
            AssemblyLoader.loadedAssemblies.TypeOperation(type =>
            {
                if (typeof(IPQSModTweaker).IsAssignableFrom(type) && !type.IsAbstract)
                {
                    IPQSModTweaker tweaker = (IPQSModTweaker)Activator.CreateInstance(type);
                    PQSModTweakers.Add(tweaker);

                    // Get the config
                    String configNodeName = tweaker.GetConfig();
                    if (!ConfigCache.ContainsKey(configNodeName))
                    {
                        ConfigNode config = GameDatabase.Instance.GetConfigs(configNodeName)[0].config;
                        ConfigCache.Add(configNodeName, config);
                    }
                }
            });

            // Register the callback for manipulating the system
            GameEvents.onGameSceneSwitchRequested.Add(OnGameSceneSwitchRequested);
        }

        /// <summary>
        /// Gets called when the users switches from one game scene to another one.
        /// </summary>
        void OnGameSceneSwitchRequested(GameEvents.FromToAction<GameScenes, GameScenes> action)
        {
            // Are we loading a game?
            if (action.from == GameScenes.MAINMENU && action.to == GameScenes.SPACECENTER)
            {
                for (Int32 i = 0; i < PQSModTweakers.Count; i++)
                {
                    // Tweaker
                    IPQSModTweaker tweaker = PQSModTweakers[i];

                    // Check the config
                    ConfigNode config = ConfigCache[tweaker.GetConfig()];

                    // Is the tweak group enabled?
                    if (!config.HasValue("enabled"))
                        continue;
                    if (!Boolean.TryParse(config.GetValue("enabled"), out Boolean isEnabled) || !isEnabled)
                        continue;

                    // Is the tweak itself enabled?
                    String setting = tweaker.GetSetting();
                    if (setting != null)
                    {
                        if (!config.HasValue(setting))
                            continue;
                        if (!Boolean.TryParse(config.GetValue(setting), out isEnabled) || !isEnabled)
                            continue;
                    }

                    // Tweak it!
                    for (Int32 j = 0; j < PSystemManager.Instance.localBodies.Count; j++)
                    {
                        // Get the Body
                        CelestialBody body = PSystemManager.Instance.localBodies[j];

                        // Get the PQSMods
                        PQSMod[] mods = body.GetComponentsInChildren<PQSMod>(true);

                        // Tweak them
                        foreach (PQSMod mod in mods)
                        {
                            tweaker.Tweak(body, mod);
                        }                        
                    }
                }
            }

            // Are we leaving the game?
            if (action.to == GameScenes.MAINMENU)
            {
                // Kill the PQS so it definitly rebuilds when we load a game
                for (Int32 i = 0; i < PSystemManager.Instance.localBodies.Count; i++)
                {
                    CelestialBody body = PSystemManager.Instance.localBodies[i];
                    body.pqsController.ClearCache();
                    body.pqsController.ResetSphere();
                }
            }
        }
    }
}

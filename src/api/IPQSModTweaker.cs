using System;

namespace PlanetaryDiversity.API
{
    /// <summary>
    /// Allows interaction and tweaking of PQSMods
    /// </summary>
    public interface IPQSModTweaker
    {
        /// <summary>
        /// Returns the name of the config node that stores the configuration
        /// </summary>
        String GetConfig();

        /// <summary>
        /// Returns the name of the config option that can be used to disable the tweak
        /// </summary>
        String GetSetting();

        /// <summary>
        /// Changes the parameters of the PQSMod
        /// </summary>
        void Tweak(CelestialBody body, PQSMod mod);
    }

    /// <summary>
    /// Base class for generic PQSMod access
    /// </summary>
    public abstract class PQSModTweaker<T> : IPQSModTweaker where T : PQSMod
    {
        /// <summary>
        /// Returns the name of the config node that stores the configuration
        /// </summary>
        public abstract String GetConfig();

        /// <summary>
        /// Returns the name of the config option that can be used to disable the tweak
        /// </summary>
        public abstract String GetSetting();

        /// <summary>
        /// Changes the parameters of the PQSMod
        /// </summary>
        void IPQSModTweaker.Tweak(CelestialBody body, PQSMod mod)
        {
            if (mod is T)
            {
                Tweak(body, (T)mod);
            }
        }
        
        /// <summary>
        /// Changes the parameters of the PQSMod
        /// </summary>
        public abstract void Tweak(CelestialBody body, T mod);
    }
}

using System;

namespace PlanetaryDiversity.API
{
    /// <summary>
    /// Allows interaction and tweaking of CelestialBodies
    /// </summary>
    public interface ICelestialBodyTweaker
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
        /// Changes the parameters of the CB
        /// </summary>
        Boolean Tweak(CelestialBody body);
    }

    /// <summary>
    /// Base class for generic PQSMod access
    /// </summary>
    public abstract class CelestialBodyTweaker : RandomProvider, ICelestialBodyTweaker
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
        public abstract Boolean Tweak(CelestialBody body);
    }
}

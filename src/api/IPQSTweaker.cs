using System;

namespace PlanetaryDiversity.API
{
    /// <summary>
    /// Allows interaction and tweaking of PQS spheres
    /// </summary>
    public interface IPQSTweaker
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
        /// Changes the parameters of the PQS
        /// </summary>
        Boolean Tweak(CelestialBody body, PQS sphere);
    }

    /// <summary>
    /// Base class for generic PQSMod access
    /// </summary>
    public abstract class PQSTweaker : RandomProvider, IPQSTweaker
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
        public abstract Boolean Tweak(CelestialBody body, PQS sphere);
    }
}

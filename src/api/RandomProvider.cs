using System;

namespace PlanetaryDiversity.API
{
    /// <summary>
    /// Provides access to a static random number generator
    /// </summary>
    public class RandomProvider
    {
        /// <summary>
        /// A random component
        /// </summary>
        private static Random _random;

        /// <summary>
        /// Returns a random number based on a seed.
        /// </summary>
        protected Int32 GetRandom(Int32 Seed)
        {
            // Do we already have a random?
            if (_random == null)
                _random = new Random(Seed);
            return _random.Next();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

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
        
        /// <summary>
        /// Returns a random number based on a seed.
        /// </summary>
        protected Int32 GetRandom(Int32 Seed, Int32 Min, Int32 Max)
        {
            // Do we already have a random?
            if (_random == null)
                _random = new Random(Seed);
            return _random.Next(Min, Max);
        }

        /// <summary>
        /// Returns a random number based on a seed.
        /// </summary>
        protected Double GetRandomDouble(Int32 Seed, Double Min, Double Max)
        {
            // Do we already have a random?
            if (_random == null)
                _random = new Random(Seed);
            return (_random.NextDouble() * (Max - Min)) + Min;
        }

        /// <summary>
        /// Returns a random element from a list
        /// </summary>
        protected T GetRandomElement<T>(Int32 Seed, IEnumerable<T> List)
        {
            // Is the list null?
            if (List == null)
                return default(T);
            return List.ElementAt(GetRandom(Seed, 0, List.Count()));
        }

        /// <summary>
        /// Makes a list where different elements have a different chance
        /// </summary>
        protected IEnumerable<T> MakeChanceList<T>(Dictionary<T, Int32> chances)
        {
            // Did we reach the 100%?
            if (chances.Values.Sum() != 100)
                return null;

            // Make a list
            List<T> list = new List<T>();

            // Add elements
            foreach (KeyValuePair<T, Int32> element in chances)
            {
                for (Int32 i = 0; i < element.Value; i++)
                {
                    list.Add(element.Key);
                }
            }
            return list;
        }

        /// <summary>
        /// Reset the number generator to it's original state.
        /// </summary>
        public static void Reset() 
        {
            _random = null;
        }
    }
}

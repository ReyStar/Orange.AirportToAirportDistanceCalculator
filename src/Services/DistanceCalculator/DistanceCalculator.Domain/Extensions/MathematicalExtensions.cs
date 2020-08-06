using System;

namespace DistanceCalculator.Domain.Extensions
{
    static class MathematicalExtensions
    {
        private static double RadianMultiplier = Math.PI / 180.0;

        /// <summary>
        /// Convert to Radians
        /// </summary>
        public static double ToRadian(this double val)
        {
            return RadianMultiplier * val;
        }
    }
}

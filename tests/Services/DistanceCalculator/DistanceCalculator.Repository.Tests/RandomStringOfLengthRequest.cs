using System;
using System.Text;

namespace DistanceCalculator.Repository.Tests
{
    class RandomStringGenerator
    {
        public RandomStringGenerator() 
            : this("ABCDEFGHIJKLMNOPQRSTUVWXYZ")
        {
        }

        public RandomStringGenerator(string charactersToUse)
        {
            _random = new Random(Guid.NewGuid().GetHashCode());
            _charactersToUse = charactersToUse;
        }

        public string GetRandomString(uint length = 3)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < length; i++)
                sb.Append(GetRandomChar());

            return sb.ToString();
        }

        private readonly Random _random;
        private readonly string _charactersToUse;

        private string GetRandomChar()
        {
            return _charactersToUse[_random.Next(_charactersToUse.Length)].ToString();
        }
    }
}

using NUnit.Framework;
using Orange.AirportToAirportDistanceCalculator.Domain.Models;
using Orange.AirportToAirportDistanceCalculator.Domain.Services;

namespace Orange.AirportToAirportDistanceCalculator.Domain.Tests
{
    public class HaversineGeoCalculatorTests
    {
        private readonly HaversineGeoCalculatorService _service;
        private const double DeltaPercent = 1.00;

        public HaversineGeoCalculatorTests()
        {
            _service = new HaversineGeoCalculatorService();
        }

        [Test]
        [TestCaseSource(typeof(GeoDistanceTestCaseSource), nameof(GeoDistanceTestCaseSource.GetGeoDistanceTestData))]
        public void CalculateDistanceAsync(GeoCoordinate start, GeoCoordinate end, double distance)
        {
            // Arrange

            // Act
            var result = _service.CalculateDistance(start, end);

            // Assert
            Assert.That(distance, Is.EqualTo(result).Within(DeltaPercent).Percent);
        }
    }
}

using NUnit.Framework;
using Orange.AirportToAirportDistanceCalculator.Domain.Models;
using Orange.AirportToAirportDistanceCalculator.Domain.Services;

namespace Orange.AirportToAirportDistanceCalculator.Domain.Tests
{
    public class VincentGeoCalculatorTests
    {
        private readonly VincentGeoCalculatorService _service;
        private const double DeltaPercent = 0.30;

        public VincentGeoCalculatorTests()
        {
            _service = new VincentGeoCalculatorService();
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

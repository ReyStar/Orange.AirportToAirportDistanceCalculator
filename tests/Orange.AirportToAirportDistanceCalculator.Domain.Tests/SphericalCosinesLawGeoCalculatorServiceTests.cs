using NUnit.Framework;
using Orange.AirportToAirportDistanceCalculator.Domain.Models;
using Orange.AirportToAirportDistanceCalculator.Domain.Services;

namespace Orange.AirportToAirportDistanceCalculator.Domain.Tests
{
    public class SphericalCosinesLawGeoCalculatorServiceTests
    {
        private readonly SphericalCosinesLawGeoCalculatorService _service;
        private const double DeltaPercent = 10.00;

        public SphericalCosinesLawGeoCalculatorServiceTests()
        {
            _service = new SphericalCosinesLawGeoCalculatorService();
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

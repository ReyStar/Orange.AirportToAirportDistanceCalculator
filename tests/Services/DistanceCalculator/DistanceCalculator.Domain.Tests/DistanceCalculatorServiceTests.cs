using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using DistanceCalculator.Domain.Exceptions;
using DistanceCalculator.Domain.Interfaces;
using DistanceCalculator.Domain.Models;
using DistanceCalculator.Domain.Services;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace DistanceCalculator.Domain.Tests
{
    [TestFixture]
    internal class DistanceCalculatorServiceTests
    {
        [Test]
        [AutoMoqData]
        public async Task CalculateDistanceAsync_Success_Test([Frozen] Mock<IAirportInformationProviderService> airportInformationProviderService,
                                                              [Frozen] Mock<IGeoCalculatorService> geoCalculatorService,
                                                              DistanceCalculatorService service,
                                                              string departureIATACode, 
                                                              string destinationIATACode,
                                                              AirportInformation departureAirportInformation,
                                                              AirportInformation destinationAirportInformation, 
                                                              double distance)
        {
            // Arrange
            airportInformationProviderService.Setup(x => x.GetInformationAsync(departureIATACode, CancellationToken.None))
                                             .ReturnsAsync(departureAirportInformation);
            airportInformationProviderService.Setup(x => x.GetInformationAsync(destinationIATACode, CancellationToken.None))
                                             .ReturnsAsync(destinationAirportInformation);
            geoCalculatorService.Setup(x =>
                    x.CalculateDistance(departureAirportInformation.Location, destinationAirportInformation.Location))
                                .Returns(distance);

            // Act
            var result =
                await service.CalculateDistanceAsync(departureIATACode, destinationIATACode, CancellationToken.None);
                
            // Assert
            result.Should().NotBeNull();
            result.Miles.Should().Be(distance);

            airportInformationProviderService.VerifyAll();
            geoCalculatorService.VerifyAll();
        }

        [Test]
        [AutoMoqData]
        public async Task CalculateDistanceAsync_DestinationAirportNotFound_Test([Frozen] Mock<IAirportInformationProviderService> airportInformationProviderService,
                                                                                 DistanceCalculatorService service,
                                                                                 string departureIATACode,
                                                                                 string destinationIATACode,
                                                                                 AirportInformation departureAirportInformation)
        {
            // Arrange
            airportInformationProviderService.Setup(x => x.GetInformationAsync(departureIATACode, CancellationToken.None))
                                             .ReturnsAsync(departureAirportInformation);
            airportInformationProviderService.Setup(x => x.GetInformationAsync(destinationIATACode, CancellationToken.None))
                                             .ReturnsAsync((AirportInformation?) null);

            // Act && Assert
            Assert.CatchAsync<AirportNotFoundException>(() => service.CalculateDistanceAsync(departureIATACode, destinationIATACode, CancellationToken.None));

            airportInformationProviderService.VerifyAll();
        }

        [Test]
        [AutoMoqData]
        public async Task CalculateDistanceAsync_DepartureAirportNotFound_Test([Frozen] Mock<IAirportInformationProviderService> airportInformationProviderService,
                                                                               DistanceCalculatorService service,
                                                                               string departureIATACode,
                                                                               string destinationIATACode)
        {
            // Arrange
            airportInformationProviderService.Setup(x => x.GetInformationAsync(departureIATACode, CancellationToken.None))
                                             .ReturnsAsync((AirportInformation?)null);
            airportInformationProviderService.Setup(x => x.GetInformationAsync(destinationIATACode, CancellationToken.None))
                                              .ReturnsAsync((AirportInformation?)null);

            // Act && Assert
            Assert.CatchAsync<AirportNotFoundException>(() => service.CalculateDistanceAsync(departureIATACode, destinationIATACode, CancellationToken.None));
            
            airportInformationProviderService.VerifyAll();
        }

        [Test]
        [AutoMoqData]
        public async Task CalculateDistanceAsync_DepartureEqualDestination_Test(DistanceCalculatorService service,
                                                                                string IATACode
            )
        {
            // Act
            var result = await service.CalculateDistanceAsync(IATACode, IATACode, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Miles.Should().Be(0);
        }
    }
}
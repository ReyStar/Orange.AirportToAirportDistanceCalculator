using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using DistanceCalculator.Application.Interfaces;
using DistanceCalculator.Application.Services;
using DistanceCalculator.Common;
using DistanceCalculator.Domain.Interfaces;
using DistanceCalculator.Domain.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace DistanceCalculator.Application.Tests
{
    [TestFixture]
    internal class DistanceCalculatorCacheServiceTests
    {
        [Test]
        [AutoMoqData]
        public async Task CalculateDistanceAsync_GetDistanceAsync_Success_Test([Frozen] Mock<IDistancesRepository> distancesRepository,
                                                                               DistanceCalculatorDataBaseCacheService service,
                                                                               string departureIATACode,
                                                                               string destinationIATACode,
                                                                               double distance)
        {
            // Arrange
            distancesRepository
                .Setup(x => x.GetDistanceAsync(departureIATACode, destinationIATACode, CancellationToken.None))
                .ReturnsAsync(distance);

            // Act
            var result =
                await service.CalculateDistanceAsync(departureIATACode, destinationIATACode, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Miles.Should().Be(distance);

            distancesRepository.VerifyAll();
        }

        [Test]
        [AutoMoqData]
        public async Task CalculateDistanceAsync_GetDistanceAsync_DepartureEqualDestination_Test(DistanceCalculatorDataBaseCacheService service,
                                                                                                 string IATACode)
        {
            // Arrange
           
            // Act
            var result =
                await service.CalculateDistanceAsync(IATACode, IATACode, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Miles.Should().Be(0.0);
        }

        [Test]
        [AutoMoqData]
        public async Task CalculateDistanceAsync_GetDistanceAsync_ThrowRepositoryException_Test([Frozen] Mock<IDistanceCalculatorService> calculatorService,
                                                                                                [Frozen] Mock<IDistancesRepository> distancesRepository,
                                                                                                [Frozen] Mock<IBackgroundTaskProvider> backgroundTaskProvider,
                                                                                                [Frozen] Mock<ILogger<DistanceCalculatorDataBaseCacheService>> logger,
                                                                                                DistanceCalculatorDataBaseCacheService service,
                                                                                                string departureIATACode,
                                                                                                string destinationIATACode,
                                                                                                GeoDistance distance,
                                                                                                Exception repositoryException)
        {
            // Arrange
            distancesRepository
                .Setup(x => x.GetDistanceAsync(departureIATACode, destinationIATACode, CancellationToken.None))
                .ThrowsAsync(repositoryException);

            logger.Setup(x => x.Log(LogLevel.Error, //It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)));

            calculatorService
                .Setup(x => x.CalculateDistanceAsync(departureIATACode, destinationIATACode, CancellationToken.None))
                .ReturnsAsync(distance);

            backgroundTaskProvider.Setup(x => x.AddBackgroundTask(It.IsAny<Func<CancellationToken, Task>>()));

            // Act
            var result =
                await service.CalculateDistanceAsync(departureIATACode, destinationIATACode, CancellationToken.None);

            // Assert
            result.Should().Be(distance);

            distancesRepository.VerifyAll();
            logger.VerifyAll();
            calculatorService.VerifyAll();
            backgroundTaskProvider.VerifyAll();
        }

        [Test]
        [AutoMoqData]
        public async Task CalculateDistanceAsync_GetDistanceAsync_SavedGeoDistanceNoExist_Test([Frozen] Mock<IDistanceCalculatorService> calculatorService,
                                                                                               [Frozen] Mock<IDistancesRepository> distancesRepository,
                                                                                               [Frozen] Mock<IBackgroundTaskProvider> backgroundTaskProvider,
                                                                                               [Frozen] Mock<ILogger<DistanceCalculatorDataBaseCacheService>> logger,
                                                                                               DistanceCalculatorDataBaseCacheService service,
                                                                                               string departureIATACode,
                                                                                               string destinationIATACode,
                                                                                               GeoDistance distance)
        {
            // Arrange
            distancesRepository
                .Setup(x => x.GetDistanceAsync(departureIATACode, destinationIATACode, CancellationToken.None))
                .ReturnsAsync((double?)null);

            calculatorService
                .Setup(x => x.CalculateDistanceAsync(departureIATACode, destinationIATACode, CancellationToken.None))
                .ReturnsAsync(distance);

            backgroundTaskProvider.Setup(x => x.AddBackgroundTask(It.IsAny<Func<CancellationToken, Task>>()));

            // Act
            var result =
                await service.CalculateDistanceAsync(departureIATACode, destinationIATACode, CancellationToken.None);

            // Assert
            result.Should().Be(distance);
            
            distancesRepository.VerifyAll();
            logger.VerifyAll();
            calculatorService.VerifyAll();
            backgroundTaskProvider.VerifyAll();
        }
    }
}
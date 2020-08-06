using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DistanceCalculator.Application.Interfaces;
using DistanceCalculator.Common;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DistanceCalculator.Repository.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.None)]
    class DistancesRepositoryTests
    {
        private IDistancesRepository _repository;
        private readonly RandomStringGenerator _randomStringGenerator;
        private readonly ServiceProvider _serviceServiceProvider;
        private const string DataBaseName = "Test.DB";

        public DistancesRepositoryTests()
        {
            _randomStringGenerator = new RandomStringGenerator();

            var builder = new ConfigurationBuilder();
            builder.AddInMemoryCollection(new Dictionary<string, string>
            {
                ["ConnectionStrings:DefaultConnection"] = $"Data Source={DataBaseName}",
                ["ConnectionStrings:RequiredVersion"] = "1",
            });
            var configurationRoot = builder.Build();

            _serviceServiceProvider = new ServiceCollection().RegisterRepository(configurationRoot)
                                        .BuildServiceProvider();
        }

        [OneTimeSetUp]
        public void Initialize()
        {
            if (File.Exists(DataBaseName))
            {
                File.Delete(DataBaseName);
            }

            var dataBaseCreator = _serviceServiceProvider.GetService<IDataBaseCreator>();

            dataBaseCreator.Run();

            _repository = _serviceServiceProvider.GetRequiredService<IDistancesRepository>();
        }

        [Test]
        [AutoMoqData]
        public async Task AddDistanceAsync_and_GetDistanceAsync_Success_Test(double distance)
        {
            // Arrange
            string departureIATACode = _randomStringGenerator.GetRandomString();
            string destinationIATACode = _randomStringGenerator.GetRandomString();

            var addedFlag = await _repository.AddDistanceAsync(departureIATACode, destinationIATACode, distance, CancellationToken.None);
            addedFlag.Should().BeTrue();

            // Act
            var savedResult = await _repository.GetDistanceAsync(departureIATACode, destinationIATACode);

            // Assert
            savedResult.Should().HaveValue();
            savedResult.Value.Should().Be(distance);
        }

        [Test]
        [AutoMoqData]
        public async Task GetDistanceAsync_RecordNotFound_Test()
        {
            // Arrange
            string departureIATACode = _randomStringGenerator.GetRandomString();
            string destinationIATACode = _randomStringGenerator.GetRandomString();

            // Act
            var savedResult = await _repository.GetDistanceAsync(departureIATACode, destinationIATACode);

            // Assert
            savedResult.Should().NotHaveValue();
        }


        [Test]
        [AutoMoqData]
        public async Task AddDistanceAsync_TryAddDuplicate_Test()
        {
            // Arrange
            string departureIATACode = _randomStringGenerator.GetRandomString();
            string destinationIATACode = _randomStringGenerator.GetRandomString();
            double distance1 = 1.0;
            double distance2 = 2.0;

            var addedFlagRecord1 = await _repository.AddDistanceAsync(departureIATACode, destinationIATACode, distance1, CancellationToken.None);
            addedFlagRecord1.Should().BeTrue();

            // Act
            var addedFlagRecord2 = await _repository.AddDistanceAsync(departureIATACode, destinationIATACode, distance2, CancellationToken.None);
            
            // Assert
            addedFlagRecord2.Should().BeFalse();
            var savedResult = await _repository.GetDistanceAsync(departureIATACode, destinationIATACode);
            savedResult.Should().HaveValue();
        }
    }
}

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;

namespace DistanceCalculator.Repository.Tests
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(CreateFixture)
        {
        }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            fixture.Customize(new SupportMutableValueTypesCustomization());
            return fixture;
        }
    }
}

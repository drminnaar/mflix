using System;

namespace MFlix.DataTests.DependencyInjection
{
    public sealed class DependencyInjectionFixture
    {
        public DependencyInjectionFixture()
        {
            Services = TestServicesSetup.ConfigureTestServices();
        }

        public IServiceProvider Services { get; init; }
    }
}

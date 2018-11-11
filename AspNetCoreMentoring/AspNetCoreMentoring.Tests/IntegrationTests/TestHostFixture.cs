using AspNetCoreMentoring.API;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;
using Xunit;

namespace AspNetCoreMentoring.Tests.IntegrationTests
{
    public class TestHostFixture : ICollectionFixture<WebApplicationFactory<Startup>>
    {
        public readonly HttpClient Client;

        public TestHostFixture()
        {
            var factory = new WebApplicationFactory<Startup>();
            Client = factory.CreateClient();
        }
    }
}
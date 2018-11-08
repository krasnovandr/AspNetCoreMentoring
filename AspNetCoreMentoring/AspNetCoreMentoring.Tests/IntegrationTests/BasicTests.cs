using AspNetCoreMentoring.API;
using AspNetCoreMentoring.API.Contracts.Dto.Product;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

    [CollectionDefinition("Integration tests collection")]
    public class IntegrationTestsCollection : ICollectionFixture<TestHostFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }


    [Collection("Integration tests collection")]
    public class YourTests
    {
        private readonly TestHostFixture _testHostFixture;

        public YourTests(TestHostFixture testHostFixture)
        {
            _testHostFixture = testHostFixture;
        }

        [Fact]
        public async Task GetProducts_DefaultParams_Returns10Products()
        {
            // When
            var response = await _testHostFixture.Client.GetAsync("api/products");
            List<ProductReadListDto> products =
                JsonConvert.DeserializeObject<List<ProductReadListDto>>(await response.Content.ReadAsStringAsync());

            Assert.NotEmpty(products);
            Assert.Equal(10, products.Count);
        }


        [Fact]
        public async Task GetProduct_DefaultParams_Returns10Products()
        {
            // When
            //int id = 1;
            //var response = await _testHostFixture.Client.GetAsync($"api/products/{id}");

            //ProductWriteItemDto product =
            //    JsonConvert.DeserializeObject<ProductWriteItemDto>(await response.Content.ReadAsStringAsync());

            //Assert.NotNull(product);
            //Assert.NotEmpty(10, product.ProductName);
        }
    }
}
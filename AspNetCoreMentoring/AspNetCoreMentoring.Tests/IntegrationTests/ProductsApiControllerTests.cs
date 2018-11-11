using AspNetCoreMentoring.API.Contracts.Dto.Product;
using AutoFixture;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreMentoring.Tests.IntegrationTests
{
    [Collection("Integration tests collection")]
    public class ProductsApiControllerTests
    {
        private readonly TestHostFixture _testHostFixture;
        private readonly string baseUrl = "api/products";
        Fixture fixture = new Fixture();

        public ProductsApiControllerTests(TestHostFixture testHostFixture)
        {
            _testHostFixture = testHostFixture;
        }

        [Fact]
        public async Task GetProducts_DefaultParams_Returns10Products()
        {
            var response = await _testHostFixture.Client.GetAsync(baseUrl);
            List<ProductReadListDto> products =
                JsonConvert.DeserializeObject<List<ProductReadListDto>>(await response.Content.ReadAsStringAsync());

            Assert.NotEmpty(products);
            Assert.Equal(10, products.Count);
        }

        [Fact]
        public async Task GetProducts_FirstPage3Items_Returns3Products()
        {
            var response = await _testHostFixture.Client.GetAsync($"{baseUrl}?itemsPerPage=3");
            List<ProductReadListDto> products =
                JsonConvert.DeserializeObject<List<ProductReadListDto>>(await response.Content.ReadAsStringAsync());

            Assert.NotEmpty(products);
            Assert.Equal(3, products.Count);
        }


        [Fact]
        public async Task ProductApi_CreateGetUpdateDeleteProduct_SuccesfullyCreatedUpdatedRetrievedDeleted()
        {
            ProductWriteItemDto expectedProduct = GetMockedProduct();

            var createdProduct = await TestProductCreation(expectedProduct);

            var retrievedProduct = await TestGetProductById(createdProduct);

            await TestUpdateProduct(createdProduct, retrievedProduct);

            await TestDeleteProductById(retrievedProduct);

            await TestGetProductByIdNotExists(retrievedProduct);
        }

        private async Task TestGetProductByIdNotExists(ProductReadItemDto retrievedProduct)
        {
            var getProductResponse = await _testHostFixture.Client.GetAsync($"{baseUrl}/{retrievedProduct.ProductId}");

            Assert.False(getProductResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, getProductResponse.StatusCode);
        }

        private async Task TestDeleteProductById(ProductReadItemDto createdProduct)
        {
            var deleteProductResponse = await _testHostFixture.Client.DeleteAsync(
             $"{baseUrl}/{createdProduct.ProductId}");

            Assert.True(deleteProductResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NoContent, deleteProductResponse.StatusCode);
        }

        private async Task TestUpdateProduct(ProductReadItemDto createdProduct, ProductReadItemDto retrievedProduct)
        {
            var updateProductResponse = await _testHostFixture.Client.PutAsJsonAsync(
                $"{baseUrl}/{createdProduct.ProductId}",
                retrievedProduct);

            var updatedProduct =
                JsonConvert.DeserializeObject<ProductReadItemDto>(
                    await updateProductResponse.Content.ReadAsStringAsync());

            Assert.True(updateProductResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, updateProductResponse.StatusCode);
            Assert.Equal(retrievedProduct.ProductId, updatedProduct.ProductId);
            AssertProductsEquality(updatedProduct, retrievedProduct);
        }

        private async Task<ProductReadItemDto> TestGetProductById(ProductReadItemDto createdProduct)
        {
            var getProductResponse = await _testHostFixture.Client.GetAsync($"{baseUrl}/{createdProduct.ProductId}");

            var getProduct =
                JsonConvert.DeserializeObject<ProductReadItemDto>(
                    await getProductResponse.Content.ReadAsStringAsync());

            Assert.True(getProductResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, getProductResponse.StatusCode);
            Assert.Equal(createdProduct.ProductId, getProduct.ProductId);
            AssertProductsEquality(createdProduct, getProduct);

            return getProduct;
        }

        private async Task<ProductReadItemDto> TestProductCreation(ProductWriteItemDto expectedProduct)
        {
            var productCreatedResponse = await _testHostFixture.Client.PostAsJsonAsync(
                baseUrl, expectedProduct);

            var actualProduct =
                JsonConvert.DeserializeObject<ProductReadItemDto>(
                    await productCreatedResponse.Content.ReadAsStringAsync());

            Assert.True(productCreatedResponse.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.Created, productCreatedResponse.StatusCode);

            Assert.NotEqual(default(int), actualProduct.ProductId);
            AssertProductsEquality(expectedProduct, actualProduct);

            return actualProduct;
        }

        private static void AssertProductsEquality(ProductWriteItemDto expectedProduct, ProductReadItemDto actualProduct)
        {
            Assert.Equal(expectedProduct.ProductName, actualProduct.ProductName);
            Assert.Equal(expectedProduct.QuantityPerUnit, actualProduct.QuantityPerUnit);
            Assert.Equal(expectedProduct.ReorderLevel, actualProduct.ReorderLevel);
            Assert.Equal(expectedProduct.SelectedCategoryId, actualProduct.SelectedCategoryId);
            Assert.Equal(expectedProduct.SelectedSupplierId, actualProduct.SelectedSupplierId);
            Assert.Equal(expectedProduct.UnitPrice, actualProduct.UnitPrice);
            Assert.Equal(expectedProduct.UnitsInStock, actualProduct.UnitsInStock);
            Assert.Equal(expectedProduct.UnitsOnOrder, actualProduct.UnitsOnOrder);
        }

        private ProductWriteItemDto GetMockedProduct()
        {
            return new ProductWriteItemDto
            {
                Discontinued = true,
                ProductName = fixture.Create<string>(),
                QuantityPerUnit = "1",
                ReorderLevel = 1,
                SelectedCategoryId = 1,
                SelectedSupplierId = 1,
                UnitPrice = 10,
                UnitsInStock = 10,
                UnitsOnOrder = 10,
            };
        }
    }
}
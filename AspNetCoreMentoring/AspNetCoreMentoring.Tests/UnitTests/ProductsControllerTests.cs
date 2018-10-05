using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using AspNetCoreMentoring.UI.Controllers;
using AspNetCoreMentoring.UI.Mapping;
using AspNetCoreMentoring.UI.ViewModels.Product;
using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AspNetCoreMentoring.Tests
{
    public class ProductsControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IProductsService> _productsService;
        private readonly Mock<ICategoriesService> _categoriesService;
        private readonly Mock<ISupplierService> _supplierService;
        private readonly Mock<IConfigurationRoot> _configuration;
        private readonly Mock<ILogger<ProductsController>> _logger;

        public ProductsControllerTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductsProfile>();
            });

            _mapper = new Mapper(config);

            _productsService = new Mock<IProductsService>();
            _categoriesService = new Mock<ICategoriesService>();
            _supplierService = new Mock<ISupplierService>();
            _logger = new Mock<ILogger<ProductsController>>();
            _configuration = new Mock<IConfigurationRoot>();
        }

        [Fact]
        public async Task Index_ShouldReturnAllProducts_ReturnsAllProducts()
        {
            var expectedProducts = GetTestProducts(10);
            _productsService.Setup(service => service.GetProductsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedProducts);

            _configuration.SetupGet(x => x[It.IsAny<string>()]).Returns("0");

            var controller = new ProductsController(
                _productsService.Object,
                _categoriesService.Object,
                _supplierService.Object,
                _configuration.Object,
                _mapper,
                _logger.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<ProductReadListViewModel>>(
                viewResult.ViewData.Model);

            Assert.Equal(expectedProducts.Count(), model.Count());
            Assert.Equal(expectedProducts.Select(v => v.ProductId), model.Select(v => v.ProductId));
            Assert.Equal(expectedProducts.Select(v => v.Discontinued), model.Select(v => v.Discontinued));
            Assert.Equal(expectedProducts.Select(v => v.ProductName), model.Select(v => v.ProductName));
            Assert.Equal(expectedProducts.Select(v => v.QuantityPerUnit), model.Select(v => v.QuantityPerUnit));
            Assert.Equal(expectedProducts.Select(v => v.ReorderLevel), model.Select(v => v.ReorderLevel));
            Assert.Equal(expectedProducts.Select(v => v.Supplier.CompanyName), model.Select(v => v.SupplierName));
            Assert.Equal(expectedProducts.Select(v => v.Category.CategoryName), model.Select(v => v.CategoryName));
            Assert.Equal(expectedProducts.Select(v => v.UnitPrice), model.Select(v => v.UnitPrice));
            Assert.Equal(expectedProducts.Select(v => v.UnitsInStock), model.Select(v => v.UnitsInStock));
            Assert.Equal(expectedProducts.Select(v => v.UnitsOnOrder), model.Select(v => v.UnitsOnOrder));
        }


        [Fact]
        public async Task CreateProduct_ShouldReturnAllProducts_ReturnsAllProducts()
        {
            var expectedProducts = GetTestProducts(10);
            _productsService.Setup(service => service.GetProductsAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedProducts);

            _configuration.SetupGet(x => x[It.IsAny<string>()]).Returns("0");

            var controller = new ProductsController(
                _productsService.Object,
                _categoriesService.Object,
                _supplierService.Object,
                _configuration.Object,
                _mapper,
                _logger.Object);

            // Act
            var result = await controller.CreateProduct();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<ProductReadListViewModel>>(
                viewResult.ViewData.Model);

            Assert.Equal(expectedProducts.Count(), model.Count());
            Assert.Equal(expectedProducts.Select(v => v.ProductId), model.Select(v => v.ProductId));
            Assert.Equal(expectedProducts.Select(v => v.Discontinued), model.Select(v => v.Discontinued));
            Assert.Equal(expectedProducts.Select(v => v.ProductName), model.Select(v => v.ProductName));
            Assert.Equal(expectedProducts.Select(v => v.QuantityPerUnit), model.Select(v => v.QuantityPerUnit));
            Assert.Equal(expectedProducts.Select(v => v.ReorderLevel), model.Select(v => v.ReorderLevel));
            Assert.Equal(expectedProducts.Select(v => v.Supplier.CompanyName), model.Select(v => v.SupplierName));
            Assert.Equal(expectedProducts.Select(v => v.Category.CategoryName), model.Select(v => v.CategoryName));
            Assert.Equal(expectedProducts.Select(v => v.UnitPrice), model.Select(v => v.UnitPrice));
            Assert.Equal(expectedProducts.Select(v => v.UnitsInStock), model.Select(v => v.UnitsInStock));
            Assert.Equal(expectedProducts.Select(v => v.UnitsOnOrder), model.Select(v => v.UnitsOnOrder));
        }

        private IEnumerable<Products> GetTestProducts(int count)
        {
            Fixture fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var expectedProducts = fixture.CreateMany<Products>(count);

            return expectedProducts;
        }
    }
}

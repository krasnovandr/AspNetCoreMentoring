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

namespace AspNetCoreMentoring.Tests.UnitTests
{
    public class ProductsControllerTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IProductsService> _productsService;
        private readonly Mock<ICategoriesService> _categoriesService;
        private readonly Mock<ISupplierService> _supplierService;
        private readonly Mock<IConfigurationRoot> _configuration;
        private readonly Mock<ILogger<ProductsController>> _logger;
        Fixture _fixture = new Fixture();

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
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Index_ShouldReturnAllProducts_ReturnsAllProducts()
        {
            var expectedProducts = _fixture.CreateMany<Products>(5);
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
        public async Task GetCreateProduct_ShouldReturnCreateView_ReturnsView()
        {
            var expectedCategories = _fixture.CreateMany<Categories>(5);
            _categoriesService.Setup(service => service.GetCategoriesAsync())
                 .ReturnsAsync(expectedCategories).Verifiable();

            var expectedSuppliers = _fixture.CreateMany<Suppliers>(5);
            _supplierService.Setup(service => service.GetSuppliersAsync())
                 .ReturnsAsync(expectedSuppliers).Verifiable();

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

            var actualModel = Assert.IsAssignableFrom<ProductWriteItemViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(expectedSuppliers.Count(), actualModel.Suppliers.Count());
            Assert.Equal(expectedSuppliers.Count(), actualModel.Suppliers.Count());

            _productsService.Verify();
            _categoriesService.Verify();
            _productsService.Verify();
        }

        [Fact]
        public async Task GetEditProduct_CorrectInputId_ReturnsViewResultEditProduct()
        {
            var testId = _fixture.Create<int>();
            var expectedProduct = _fixture.Create<Products>();

            _productsService.Setup(service => service.GetProductAsync(It.Is<int>(v => v == testId)))
                .ReturnsAsync(expectedProduct).Verifiable();

            var expectedCategories = _fixture.CreateMany<Categories>(5);
            _categoriesService.Setup(service => service.GetCategoriesAsync())
                 .ReturnsAsync(expectedCategories).Verifiable();

            var expectedSuppliers = _fixture.CreateMany<Suppliers>(5);
            _supplierService.Setup(service => service.GetSuppliersAsync())
                 .ReturnsAsync(expectedSuppliers).Verifiable();

            var controller = new ProductsController(
               _productsService.Object,
               _categoriesService.Object,
               _supplierService.Object,
               _configuration.Object,
               _mapper,
               _logger.Object);

            var result = await controller.EditProduct(testId);

            var viewResult = Assert.IsType<ViewResult>(result);

            var actualModel = Assert.IsAssignableFrom<ProductWriteItemViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(expectedSuppliers.Count(), actualModel.Suppliers.Count());
            Assert.Equal(expectedSuppliers.Count(), actualModel.Suppliers.Count());

            _productsService.Verify();
            _categoriesService.Verify();
            _supplierService.Verify();
        }


        [Fact]
        public async Task GetEditProduct_IncorrectId_ReturnsRedirectToActionResult()
        {
            var testId = _fixture.Create<int>();

            _productsService.Setup(service => service.GetProductAsync(It.Is<int>(v => v == testId)))
                .ReturnsAsync((Products)null).Verifiable();

            var controller = new ProductsController(
               _productsService.Object,
               _categoriesService.Object,
               _supplierService.Object,
               _configuration.Object,
               _mapper,
               _logger.Object);

            var result = await controller.EditProduct(testId);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("NotFoundView", redirectResult.ActionName);
            Assert.Equal("Error", redirectResult.ControllerName);

            _productsService.Verify();
        }


        [Fact]
        public async Task PostEditProduct_CorrectModel_CallServiceAndRedirectToList()
        {
            var editedProductModel = _fixture.Create<ProductWriteItemViewModel>();

            var controller = new ProductsController(
               _productsService.Object,
               _categoriesService.Object,
               _supplierService.Object,
               _configuration.Object,
               _mapper,
               _logger.Object);

            var result = await controller.EditProduct(editedProductModel);

            var redirectResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);
            _productsService.Verify(mock => mock.UpdateProductAsync(It.IsAny<Products>()), Times.Once());
        }


        [Fact]
        public async Task PostEditCategory_IncorrectModel_ReturnViewWithModel()
        {
            var editedProductModel = _fixture.Create<ProductWriteItemViewModel>();

            var expectedCategories = _fixture.CreateMany<Categories>(5);
            _categoriesService.Setup(service => service.GetCategoriesAsync())
                 .ReturnsAsync(expectedCategories).Verifiable();

            var expectedSuppliers = _fixture.CreateMany<Suppliers>(5);
            _supplierService.Setup(service => service.GetSuppliersAsync())
                 .ReturnsAsync(expectedSuppliers).Verifiable();

            var controller = new ProductsController(
               _productsService.Object,
               _categoriesService.Object,
               _supplierService.Object,
               _configuration.Object,
               _mapper,
               _logger.Object);

            controller.ModelState.AddModelError("error", "some error");

            var result = await controller.EditProduct(editedProductModel);

            var viewResult = Assert.IsType<ViewResult>(result);

            var actualModel = Assert.IsAssignableFrom<ProductWriteItemViewModel>(
                viewResult.ViewData.Model);

            Assert.Equal(expectedSuppliers.Count(), actualModel.Suppliers.Count());
            Assert.Equal(expectedSuppliers.Count(), actualModel.Suppliers.Count());

            _productsService.Verify();
            _categoriesService.Verify();
            _supplierService.Verify();
        }
    }
}

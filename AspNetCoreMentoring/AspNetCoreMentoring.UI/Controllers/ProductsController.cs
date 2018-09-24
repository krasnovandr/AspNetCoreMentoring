using AspNetCoreMentoring.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using AspNetCoreMentoring.UI.ViewModels.Category;
using AspNetCoreMentoring.UI.ViewModels.Product;
using AspNetCoreMentoring.UI.ViewModels.Supplier;
using AutoMapper;
using Serilog;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCoreMentoring.UI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ICategoriesService _categoriesService;
        private readonly ISupplierService _supplierService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(
            IProductsService productsService,
            ICategoriesService categoriesService,
            ISupplierService supplierService,
            IConfiguration configuration,
            IMapper mapper,
            ILogger<ProductsController> logger)
        {
            _productsService = productsService;
            _supplierService = supplierService;
            _categoriesService = categoriesService;
            _configuration = configuration;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var existingProduct = await _productsService.GetProduct(id);
            var model = _mapper.Map<ProductWriteItemViewModel>(existingProduct);

            await FillSelectLists(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductWriteItemViewModel createModel)
        {
            if (ModelState.IsValid)
            {
                Products product = MapViewModelToProduct(createModel);
                await _productsService.UpdateProduct(product);
                return RedirectToAction("Index");
            }

            return View(createModel);
        }


        public async Task<IActionResult> CreateProduct()
        {
            var model = new ProductWriteItemViewModel();
            await FillSelectLists(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductWriteItemViewModel createModel)
        {
            if (ModelState.IsValid)
            {
                Products product = MapViewModelToProduct(createModel);
                await _productsService.CreateProduct(product);
                return RedirectToAction("Index");
            }
            await FillSelectLists(createModel);
            return View("CreateProduct", createModel);
        }

        private Products MapViewModelToProduct(ProductWriteItemViewModel createModel)
        {
            return new Products
            {
                ProductName = createModel.ProductName,
                UnitPrice = createModel.UnitPrice,
                QuantityPerUnit = createModel.QuantityPerUnit,
                Discontinued = createModel.Discontinued,
                UnitsInStock = createModel.UnitsInStock,
                UnitsOnOrder = createModel.UnitsOnOrder,
                ReorderLevel = createModel.UnitsOnOrder,
                SupplierId = createModel.SelectedSupplierId,
                CategoryId = createModel.SelectedCategoryId,
                ProductId = createModel.ProductId

            };
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Max product count from config {0}", _configuration["MaxProductCount"]);
            var itemsPerPage = Convert.ToInt32(_configuration["MaxProductCount"]);

            var products = await _productsService.GetProductsAsync(0, itemsPerPage);

            var result = _mapper.Map<IEnumerable<ProductReadListViewModel>>(products);

            return View(result);
        }

        private async Task FillSelectLists(ProductWriteItemViewModel model)
        {
            var categories = await _categoriesService.GetCategoriesAsync();

            if (categories != null)
            {
                model.Categories = _mapper.Map<IEnumerable<CategoryItemViewModel>>(categories);
            }

            var suppliers = await _supplierService.GetSuppliersAsync();

            if (suppliers != null)
            {
                model.Suppliers = _mapper.Map<IEnumerable<SupplierItemViewModel>>(suppliers);
            }
        }
    }
}

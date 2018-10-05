using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using AspNetCoreMentoring.UI.ViewModels.Category;
using AspNetCoreMentoring.UI.ViewModels.Product;
using AspNetCoreMentoring.UI.ViewModels.Supplier;
using AutoMapper;
using Microsoft.Extensions.Logging;


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

        public async Task<IActionResult> Index(int pageNumber = 0)
        {
            var itemsPerPage = Convert.ToInt32(_configuration["MaxProductCount"]);

            _logger.LogInformation("Max product count from config {0}", itemsPerPage);

            var products = await _productsService.GetProductsAsync(pageNumber, itemsPerPage);

            var result = _mapper.Map<IEnumerable<ProductReadListViewModel>>(products);

            return View(result);
        }

        public async Task<IActionResult> EditProduct(int id)
        {
            var existingProduct = await _productsService.GetProductAsync(id);
            var model = _mapper.Map<ProductWriteItemViewModel>(existingProduct);

            await FillSelectLists(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductWriteItemViewModel createModel)
        {
            if (ModelState.IsValid)
            {
                Products product = _mapper.Map<Products>(createModel);
                await _productsService.UpdateProductAsync(product);
                return RedirectToAction("Index");
            }

            await FillSelectLists(createModel);
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
                Products product = _mapper.Map<Products>(createModel);
                await _productsService.CreateProductAsync(product);
                return RedirectToAction("Index");
            }
            await FillSelectLists(createModel);
            return View("CreateProduct", createModel);
        }

        private async Task FillSelectLists(ProductWriteItemViewModel model)
        {
            var categories = await _categoriesService.GetCategoriesAsync();

            if (categories != null)
            {
                model.Categories = _mapper.Map<IEnumerable<CategoryWriteItemViewModel>>(categories);
            }

            var suppliers = await _supplierService.GetSuppliersAsync();

            if (suppliers != null)
            {
                model.Suppliers = _mapper.Map<IEnumerable<SupplierItemViewModel>>(suppliers);
            }
        }
    }
}

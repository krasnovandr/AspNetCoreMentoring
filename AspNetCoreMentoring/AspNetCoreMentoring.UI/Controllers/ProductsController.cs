using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using AspNetCoreMentoring.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AspNetCoreMentoring.UI.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productsService)
        {
            _productsService = productsService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var products = await _productsService.GetProductsAsync();

            IEnumerable<ProductViewModel> result = MapProductsToViewModel(products);

            return View(result);
        }

        private IEnumerable<ProductViewModel> MapProductsToViewModel(IEnumerable<Products> products)
        {
            return products.Select(product =>
            {
                return new ProductViewModel
                {
                    CategoryName = product.Category.CategoryName,
                    ProductId = product.ProductId,
                    Discontinued = product.Discontinued,
                    ProductName = product.ProductName,
                    QuantityPerUnit = product.QuantityPerUnit,
                    ReorderLevel = product.ReorderLevel,
                    SupplierName = product.Supplier.CompanyName,
                    UnitPrice = product.UnitPrice,
                    UnitsInStock = product.UnitsInStock,
                    UnitsOnOrder = product.UnitsOnOrder
                };
            });

        }
    }
}

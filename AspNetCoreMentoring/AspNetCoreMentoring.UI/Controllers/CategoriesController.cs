using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using AspNetCoreMentoring.UI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.UI.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoriesService.GetCategoriesAsync();

            IEnumerable<CategoryViewModel> result = MapCategoriesToViewModel(categories);

            return View(result);
        }

        private IEnumerable<CategoryViewModel> MapCategoriesToViewModel(IEnumerable<Categories> categories)
        {
            return categories.Select(category =>
            {
                return new CategoryViewModel
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                    Description = category.Description,
                };
            });

        }
    }
}

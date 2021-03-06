﻿using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreMentoring.UI.ViewModels.Category;
using AutoMapper;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Authorization;

namespace AspNetCoreMentoring.UI.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IMapper _mapper;

        public CategoriesController(
            ICategoriesService categoriesService,
            IMapper mapper)
        {
            _categoriesService = categoriesService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoriesService.GetCategoriesAsync();

            var result = _mapper.Map<IEnumerable<CategoryReadListViewModel>>(categories);

            return View(result);
        }

        [HttpGet("images/{id}")]
        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> CategoryImage(int id)
        {
            var category = await _categoriesService.GetCategoryAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return File(category.Picture, Image.Jpeg);
        }

        public async Task<IActionResult> EditCategory(int id)
        {
            var existingCategory = await _categoriesService.GetCategoryAsync(id);

            if (existingCategory == null)
            {
                return RedirectToAction("NotFoundView", "Error");
            }

            var model = _mapper.Map<CategoryWriteItemViewModel>(existingCategory);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(CategoryWriteItemViewModel updateCategoryModel)
        {
            if (ModelState.IsValid)
            {
                var model = _mapper.Map<Categories>(updateCategoryModel);
                await _categoriesService.UpdateCategoryAsync(model);

                return RedirectToAction("Index");
            }

            return View(updateCategoryModel);
        }


        [HttpPost]
        public async Task<IActionResult> UploadCategoryImage(CategoryUpdateImageViewModel updateCategoryModel)
        {
            var imageInMemory = new MemoryStream();

            updateCategoryModel.CategoryImage.CopyTo(imageInMemory);

            await _categoriesService.UpdateCategoryImageAsync
                (updateCategoryModel.CategoryId, imageInMemory.ToArray());

            return RedirectToAction("Index");
        }
    }
}

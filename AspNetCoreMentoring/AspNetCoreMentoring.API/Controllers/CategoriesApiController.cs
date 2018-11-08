using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using AspNetCoreMentoring.API.Contracts.Dto.Category;
using AspNetCoreMentoring.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMentoring.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesApiController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IMapper _mapper;

        public CategoriesApiController(
                ICategoriesService categoriesService,
                IMapper mapper)
        {
            _categoriesService = categoriesService;
            _mapper = mapper;
        }
        [HttpPost]
        [Route("Image")]
        public async Task<IActionResult> UploadCategoryImage([FromForm]CategoryUpdateDto updateCategoryModel)
        {
            var imageInMemory = new MemoryStream();

            updateCategoryModel.CategoryImage.CopyTo(imageInMemory);

            await _categoriesService.UpdateCategoryImageAsync
                (updateCategoryModel.CategoryId, imageInMemory.ToArray());

            return NoContent();
        }

        [HttpGet]
        [Route("Image/{id}")]
        public async Task<IActionResult> GetContent(int id)
        {
            var category = await this._categoriesService.GetCategoryAsync(id);

            if (category == null)
            {
                return this.NotFound();
            }

            return this.File(category.Picture.ToArray(), MediaTypeNames.Image.Jpeg);
        }
    }
}
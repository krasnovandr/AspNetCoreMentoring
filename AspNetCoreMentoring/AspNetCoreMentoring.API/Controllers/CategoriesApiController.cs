using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading.Tasks;
using AspNetCoreMentoring.API.Dto.Category;
using AspNetCoreMentoring.Core.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMentoring.API.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IActionResult> UploadCategoryImage([FromForm]CategoryUpdateDto updateCategoryModel)
        {
            var imageInMemory = new MemoryStream();

            updateCategoryModel.CategoryImage.CopyTo(imageInMemory);

            await _categoriesService.UpdateCategoryImageAsync
                (updateCategoryModel.CategoryId, imageInMemory.ToArray());

            return NoContent();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetContent(int id)
        {
            var category = await this._categoriesService.GetCategoryAsync(id);

            if (category == null)
            {
                return this.NotFound();
            }

            //var result = new HttpResponseMessage(HttpStatusCode.OK)
            //{
            //    Content = new ByteArrayContent(category.Picture.ToArray())
            //};

            //result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            //{
            //    FileName = category.CategoryName
            //};

            //result.Content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Image.Jpeg);

            return this.File(category.Picture.ToArray(), MediaTypeNames.Image.Jpeg);
        }
    }
}
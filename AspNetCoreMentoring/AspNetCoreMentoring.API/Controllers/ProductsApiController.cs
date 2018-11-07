using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMentoring.API.Dto.Product;
using AspNetCoreMentoring.Core.Exceptions;
using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AspNetCoreMentoring.API.Api
{
    [Route("api/products")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsApiController> _logger;
        private readonly IProductsService _productsService;

        public ProductsApiController(
        IProductsService productsService,
        ICategoriesService categoriesService,
        ISupplierService supplierService,
        IMapper mapper,
        ILogger<ProductsApiController> logger)
        {
            _productsService = productsService;
            _mapper = mapper;
            _logger = logger;
        }


        // GET: api/ProductsApi
        [HttpGet]
        public async Task<IActionResult> Get(int pageNumber = 0, int itemsPerPage = 10)
        {
            var products = await _productsService.GetProductsAsync(pageNumber, itemsPerPage);

            var result = _mapper.Map<IEnumerable<ProductReadListDto>>(products);

            return Ok(result);
        }

        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            var existingProduct = await _productsService.GetProductAsync(id);

            if (existingProduct == null)
            {
                return NotFound($"Product with id {id} was not found");
            }
            var model = _mapper.Map<ProductWriteItemDto>(existingProduct);

            return Ok(model);
        }

        // POST: api/ProductsApi
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductWriteItemDto createModel)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            Products product = _mapper.Map<Products>(createModel);
            var cereatedProduct =await _productsService.CreateProductAsync(product);

            return CreatedAtAction("Get", new { id = cereatedProduct.ProductId }, cereatedProduct);
        }

        // PUT: api/ProductsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductWriteItemDto updateModel)
        {
            try
            {
                Products product = _mapper.Map<Products>(updateModel);
                product.ProductId = id;
                var updatedProduct = await _productsService.UpdateProductAsync(product);
            }
            catch (EntityNotFoundException)
            {
                return NotFound($"Product with id {id} was not found");
            }
            return Ok(updateModel);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productsService.DeleteProductAsync(id);
            }
            catch (EntityNotFoundException)
            {

                return NotFound($"Product with id {id} was not found");
            }

            return NoContent();
        }
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure;
using AspNetCoreMentoring.Infrastructure.EfEntities;

namespace AspNetCoreMentoring.Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IGenericRepository<Products> _productsRepository;

        public ProductsService(
            IGenericRepository<Products> productsRepository)
        {
            _productsRepository = productsRepository;
        }

        public async Task CreateProductAsync(Products product)
        {
            await _productsRepository.CreateAsync(product);
        }

        public async Task<Products> GetProductAsync(int productId)
        {
            return await _productsRepository.FindByIdAsync(productId);
        }

        public async Task<IEnumerable<Products>> GetProductsAsync(int page, int itemsPerPage)
        {
            return await _productsRepository.GetWithIncludeAsync(
                null,
                page,
                itemsPerPage,
                pr => pr.Supplier, pr => pr.Category);
        }

        public async Task UpdateProductAsync(Products product)
        {
            await _productsRepository.UpdateAsync(product);
        }
    }
}

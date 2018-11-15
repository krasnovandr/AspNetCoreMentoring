using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMentoring.Core.Exceptions;
using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure;
using AspNetCoreMentoring.Infrastructure.EfEntities;

namespace AspNetCoreMentoring.Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IGenericRepository<Products> _productsRepository;
        private readonly IProductsQuery _productsQuery;

        public ProductsService(
            IGenericRepository<Products> productsRepository,
            IProductsQuery productsQuery)
        {
            _productsQuery = productsQuery;
            _productsRepository = productsRepository;
        }

        public async Task<Products> CreateProductAsync(Products product)
        {
            return await _productsRepository.CreateAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _productsRepository.FindByIdAsync(id);

            if (product == null)
            {
                throw new EntityNotFoundException($"Product with id {id} wasn't found");
            }

            await _productsRepository.DeleteAsync(product);
        }

        public async Task<Products> GetProductAsync(int productId)
        {
            return await _productsRepository.FindByIdAsync(productId);
        }

        public async Task<IEnumerable<ProductQueryResult>> GetProductsAsync(int page, int itemsPerPage)
        {
            var result = await _productsQuery.GetProducts(page,itemsPerPage);

            return result;
        }

        public async Task<Products> UpdateProductAsync(Products productForUpdate)
        {
            Guard.ArgumentNotNull(nameof(productForUpdate), productForUpdate);

            IEnumerable<Products> products = await _productsRepository
                .FindByAsync(v => v.ProductId == productForUpdate.ProductId);

            var product = products.FirstOrDefault();

            if (product == null)
            {
                throw new EntityNotFoundException($"Product with id {productForUpdate.ProductId} wasn't found");
            }

            await _productsRepository.UpdateAsync(productForUpdate);

            return productForUpdate;
        }
    }
}

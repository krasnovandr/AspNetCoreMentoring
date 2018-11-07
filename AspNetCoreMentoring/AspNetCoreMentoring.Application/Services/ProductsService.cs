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

        public ProductsService(
            IGenericRepository<Products> productsRepository)
        {
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

        public async Task<IEnumerable<Products>> GetProductsAsync(int page, int itemsPerPage)
        {
            return await _productsRepository.GetWithIncludeAsync(
                null,
                page,
                itemsPerPage,
                pr => pr.Supplier, pr => pr.Category);
        }

        public async Task<Products> UpdateProductAsync(Products productForUpdate)
        {
            Guard.ArgumentNotNull(nameof(productForUpdate), productForUpdate);

            IEnumerable<Products> products = await _productsRepository
                .FindByAsync(v => v.ProductId == productForUpdate.ProductId);

            var product = products.FirstOrDefault();

            if (product == null )
            {
                throw new EntityNotFoundException($"Product with id {productForUpdate.ProductId} wasn't found");
            }

            await _productsRepository.UpdateAsync(productForUpdate);

            return productForUpdate;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMentoring.Core.Interfaces;
using AspNetCoreMentoring.Infrastructure;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMentoring.Core.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IGenericRepository<Products> _productsRepository;
        private int _maxProductCount;
        public ProductsService(
            IGenericRepository<Products> productsRepository,
            int maxProductCount)
        {
            _maxProductCount = maxProductCount;
            _productsRepository = productsRepository;
        }

        public async Task<IEnumerable<Products>> GetProductsAsync()
        {
            return await _productsRepository.GetWithIncludeAsync(
                null, 
                0, 
                _maxProductCount, 
                pr => pr.Supplier, pr => pr.Category);
        }
    }
}

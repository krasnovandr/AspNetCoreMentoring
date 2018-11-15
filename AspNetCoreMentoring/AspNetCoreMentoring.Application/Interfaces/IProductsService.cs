using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMentoring.Infrastructure;
using AspNetCoreMentoring.Infrastructure.EfEntities;

namespace AspNetCoreMentoring.Core.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<ProductQueryResult>> GetProductsAsync(int page, int itemsPerPage);

        Task<Products> GetProductAsync(int productId);
        Task<Products> CreateProductAsync(Products products);
        Task<Products> UpdateProductAsync(Products productForUpdate);

        Task DeleteProductAsync(int id);
    }
}

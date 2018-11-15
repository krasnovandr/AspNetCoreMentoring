using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.Infrastructure
{
    public interface IProductsQuery
    {
        Task<IEnumerable<ProductQueryResult>> GetProducts(int? page = 0, int? pageSize = 10);
    }
}

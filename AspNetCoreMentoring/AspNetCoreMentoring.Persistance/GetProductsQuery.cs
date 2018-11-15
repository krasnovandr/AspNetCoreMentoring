using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMentoring.Infrastructure.EfEntities;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMentoring.Infrastructure
{
    public class ProductsQuery : IProductsQuery
    {
        private readonly NorthwindContext _context;

        public ProductsQuery(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductQueryResult>> GetProducts(int? page = 0, int? pageSize = 10)
        {
            var query  = _context.Set<Products>().AsNoTracking();
            

            if (page.HasValue && page != 0 && pageSize.HasValue)
            {
                query = query.Skip(((int)page - 1) * (int)pageSize);
            }

            if (pageSize.HasValue && pageSize != 0)
            {
                query = query.Take((int)pageSize);
            }

            var result = query.Select(c => new ProductQueryResult
            {
                ProductId = c.ProductId,
                SupplierName = c.Supplier.CompanyName,
                ProductName = c.ProductName,
                CategoryName = c.Category.CategoryName,
                Discontinued = c.Discontinued,
                QuantityPerUnit = c.QuantityPerUnit,
                ReorderLevel = c.ReorderLevel,
                UnitPrice = c.UnitPrice,
                UnitsInStock = c.UnitsInStock,
                UnitsOnOrder = c.UnitsOnOrder,
            });

            return await result.ToListAsync();
        }



    }
}

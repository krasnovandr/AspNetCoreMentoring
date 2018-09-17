using System.Collections.Generic;
using System.Text;
using AspNetCoreMentoring.Core.Contracts;

namespace AspNetCoreMentoring.Core.Interfaces
{
    interface IProductsService
    {
        IEnumerable<Product> GetProducts();
    }
}

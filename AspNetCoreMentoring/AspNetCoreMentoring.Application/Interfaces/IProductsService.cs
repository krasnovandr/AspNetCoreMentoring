﻿using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetCoreMentoring.Infrastructure.EfEntities;

namespace AspNetCoreMentoring.Core.Interfaces
{
    public interface IProductsService
    {
        Task<IEnumerable<Products>> GetProductsAsync(int page, int itemsPerPage);

        Task<Products> GetProduct(int productId);
        Task CreateProduct(Products products);
        Task UpdateProduct(Products products);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.API.Contracts.Dto.Product
{
    public class ProductCreateItemDto : ProductWriteItemDto
    {
        public int ProductId { get; set; }
    }
}

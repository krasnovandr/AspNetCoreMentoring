using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.API.Dto.Category
{
    public class CategoryUpdateDto
    {
        public int CategoryId { get; set; }

        public IFormFile CategoryImage { get; set; }
    }
}

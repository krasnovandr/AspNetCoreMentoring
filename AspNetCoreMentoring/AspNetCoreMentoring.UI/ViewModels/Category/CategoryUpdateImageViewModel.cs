using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.UI.ViewModels.Category
{
    public class CategoryUpdateImageViewModel
    {
        public int CategoryId { get; set; }

        public IFormFile CategoryImage { get; set; }
    }
}

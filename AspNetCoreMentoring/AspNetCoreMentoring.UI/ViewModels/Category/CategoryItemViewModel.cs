using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMentoring.UI.ViewModels.Category
{
    public class CategoryWriteItemViewModel
    {
        public int CategoryId { get; set; }

        [Required]
        public string CategoryName { get; set; }

        [Required]
        public string Description { get; set; }
        public byte[] Picture { get; set; }
    }
}
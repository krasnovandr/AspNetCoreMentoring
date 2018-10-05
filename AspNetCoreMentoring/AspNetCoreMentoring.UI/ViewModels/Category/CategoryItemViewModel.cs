using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreMentoring.UI.ViewModels.Category
{
    public class CategoryWriteItemViewModel
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
    }
}
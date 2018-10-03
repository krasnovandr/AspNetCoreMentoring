using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AspNetCoreMentoring.UI.Validators;
using AspNetCoreMentoring.UI.ViewModels.Category;
using AspNetCoreMentoring.UI.ViewModels.Supplier;

namespace AspNetCoreMentoring.UI.ViewModels.Product
{
    public class ProductWriteItemViewModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100, MinimumLength = 3)]
        public string ProductName { get; set; }
        public string QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }

        [ProductDiscountValidator(10)]
        public bool Discontinued { get; set; }

        public IEnumerable<SupplierItemViewModel> Suppliers { get; set; }
        public IEnumerable<CategoryItemViewModel> Categories { get; set; }

        [Required]
        public int? SelectedCategoryId { get; set; }
        [Required]
        public int? SelectedSupplierId { get; set; }
    }
}

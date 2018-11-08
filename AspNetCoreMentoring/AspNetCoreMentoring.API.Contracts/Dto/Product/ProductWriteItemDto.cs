using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreMentoring.API.Contracts.Dto.Product
{
    public class ProductWriteItemDto
    {
        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100, MinimumLength = 3)]
        public string ProductName { get; set; }
        public string QuantityPerUnit { get; set; }

        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        [Required]
        public int? SelectedCategoryId { get; set; }
        [Required]
        public int? SelectedSupplierId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreMentoring.Core.Contracts
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

        //public Categories Category { get; set; }
        //public Suppliers Supplier { get; set; }
        //public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}

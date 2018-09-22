using System;
using System.Collections.Generic;
using System.Text;
using AspNetCoreMentoring.Infrastructure.EfEntities;

namespace AspNetCoreMentoring.Core
{
    public class ProductCreateModel
    {
        public IEnumerable<Categories> Categories
        {
            get;
            set;
        }

        public IEnumerable<Suppliers> Suppliers
        {
            get;
            set;
        }
    }
}

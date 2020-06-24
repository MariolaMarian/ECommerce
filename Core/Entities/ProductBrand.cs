using System;
using System.Collections.Generic;

namespace Core.Entities
{

    public class ProductBrand : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
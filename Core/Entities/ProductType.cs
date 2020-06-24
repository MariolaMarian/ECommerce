using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class ProductType : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
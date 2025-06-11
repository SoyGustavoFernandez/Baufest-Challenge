using System;
using System.Collections.Generic;

namespace Mapper___Codility.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public Customer CustomerDetails { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
using System.Collections.Generic;

namespace Mapper___Codility.Entities
{
    public class Product
    {
        public IEnumerable<Category> Categories { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
    }
}
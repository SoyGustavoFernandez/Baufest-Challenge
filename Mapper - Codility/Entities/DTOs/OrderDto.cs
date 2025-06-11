using System.Collections.Generic;

namespace Mapper___Codility.Entities.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string OrderDateFormatted { get; set; }
        public IEnumerable<ProductDto> OrderedProducts { get; set; }
        public TenantDto TenantInformation { get; set; }
    }
}
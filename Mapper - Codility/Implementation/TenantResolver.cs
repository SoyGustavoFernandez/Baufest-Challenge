using Mapper___Codility.Entities;
using Mapper___Codility.Interface;

namespace Mapper___Codility.Implementation
{
    public class TenantResolver : ITenantResolver
    {
        public Tenant Resolve(int id)
        {
            return new Tenant
            {
                Id = id,
                Name = "Sample Tenant",
                Address = "123 Main St",
                Country = "Country"
            };
        }
    }
}
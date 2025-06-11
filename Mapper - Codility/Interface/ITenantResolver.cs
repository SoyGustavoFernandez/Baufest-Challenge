using Mapper___Codility.Entities;

namespace Mapper___Codility.Interface
{
    public interface ITenantResolver
    {
        Tenant Resolve(int id);
    }
}
using Warehouse___Codility.Entities;

namespace Warehouse___Codility.Interface
{
    public interface IWarehouseRepository
    {
        IEnumerable<CapacityRecord> GetCapacityRecords();
        IEnumerable<WarehouseEntry> GetProductRecords();
        void SetCapacityRecord(int productId, int capacity);
        void SetProductRecord(int productId, int v);
    }
}
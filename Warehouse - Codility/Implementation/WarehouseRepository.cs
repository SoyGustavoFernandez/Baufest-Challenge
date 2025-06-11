using Warehouse___Codility.Entities;
using Warehouse___Codility.Interface;

namespace Warehouse___Codility.Implementation
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly List<WarehouseEntry> _products = new List<WarehouseEntry>();
        private readonly List<CapacityRecord> _capacities = new List<CapacityRecord>();

        public IEnumerable<WarehouseEntry> GetProductRecords()
        {
            return _products;
        }

        public IEnumerable<CapacityRecord> GetCapacityRecords()
        {
            return _capacities;
        }

        public void SetProductRecord(int productId, int quantity)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                product.Quantity = quantity;
            }
            else
            {
                _products.Add(new WarehouseEntry { ProductId = productId, Quantity = quantity });
            }
        }

        public void SetCapacityRecord(int productId, int capacity)
        {
            var record = _capacities.FirstOrDefault(c => c.ProductId == productId);
            if (record != null)
            {
                record.Capacity = capacity;
            }
            else
            {
                _capacities.Add(new CapacityRecord { ProductId = productId, Capacity = capacity });
            }
        }
    }
}

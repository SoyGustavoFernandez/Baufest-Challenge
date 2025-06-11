namespace Warehouse___Codility.Entities
{
    public class CapacityRecord
    {
        public int ProductId { get; set; }
        public int Capacity { get; set; }
        public CapacityRecord(int productId, int capacity)
        {
            ProductId = productId;
            Capacity = capacity;
        }
        public CapacityRecord() { }
    }
}
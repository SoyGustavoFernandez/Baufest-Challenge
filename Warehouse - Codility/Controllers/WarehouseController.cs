using Microsoft.AspNetCore.Mvc;
using Warehouse___Codility.Common;
using Warehouse___Codility.Entities;
using Warehouse___Codility.Interface;

using static Warehouse___Codility.Common.Message;

namespace Warehouse___Codility.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseController(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        public IActionResult GetProducts()
        {
            var products = _warehouseRepository.GetProductRecords()
                .Where(p => p.Quantity > 0)
                .Select(p => new WarehouseEntry
                {
                    ProductId = p.ProductId,
                    Quantity = p.Quantity
                });

            return Ok(products);
        }

        public IActionResult SetProductCapacity(int productId, int capacity)
        {
            if (capacity <= 0)
                return BadRequest(new NotPositiveQuantityMessage());

            var product = _warehouseRepository.GetProductRecords()
                .FirstOrDefault(p => p.ProductId == productId);

            if (capacity < (product?.Quantity ?? 0))
                return BadRequest(new Message());

            _warehouseRepository.SetCapacityRecord(productId, capacity);
            return Ok();
        }

        public IActionResult ReceiveProduct(int productId, int qty)
        {
            if (qty <= 0)
                return BadRequest(new NotPositiveQuantityMessage());

            var capacity = _warehouseRepository.GetCapacityRecords()
                .FirstOrDefault(c => c.ProductId == productId)?.Capacity ?? 0;

            var product = _warehouseRepository.GetProductRecords()
                .FirstOrDefault(p => p.ProductId == productId);

            var currentQuantity = product?.Quantity ?? 0;

            if (currentQuantity + qty > capacity)
                return BadRequest(new QuantityTooHighMessage());

            _warehouseRepository.SetProductRecord(productId, currentQuantity + qty);
            return Ok();
        }

        public IActionResult DispatchProduct(int productId, int qty)
        {
            if (qty <= 0)
                return BadRequest(new NotPositiveQuantityMessage());

            var product = _warehouseRepository.GetProductRecords()
                .FirstOrDefault(p => p.ProductId == productId);

            if (product == null || qty > product.Quantity)
                return BadRequest(new QuantityTooHighMessage());

            _warehouseRepository.SetProductRecord(productId, product.Quantity - qty);
            return Ok();
        }
    }
}
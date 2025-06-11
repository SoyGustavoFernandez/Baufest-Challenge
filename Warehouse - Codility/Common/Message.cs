using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Warehouse___Codility.Common
{
    public class Message
    {
        public class NotPositiveQuantityMessage : ModelStateDictionary
        {
            public NotPositiveQuantityMessage()
            {
                AddModelError("Quantity", "La cantidad debe ser positiva.");
            }
        }

        public class QuantityTooHighMessage : ModelStateDictionary
        {
            public QuantityTooHighMessage()
            {
                AddModelError("Quantity", "La cantidad excede la capacidad del producto.");
            }
        }
    }
}
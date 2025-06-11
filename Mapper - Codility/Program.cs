using AutoMapper;
using Mapper___Codility.Entities;
using Mapper___Codility.Entities.DTOs;
using Mapper___Codility.Implementation;
using Mapper___Codility.Interface;
using System;
using System.Globalization;
using System.Linq;

namespace Mapper___Codility
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var tenantResolver = new TenantResolver();
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile(tenantResolver)));
            var mapper = config.CreateMapper();
            var order = new Order
            {
                Id = 1,
                CustomerDetails = new Customer { Name = "John Doe", TenantId = 1 },
                OrderDate = DateTime.Now,
                Products = new[]
                {
                       new Product { Name = "Product A", Price = 10.99m, IsAvailable = true, Categories = new[] { new Category { Name = "Category 1" } } },
                       new Product { Name = "Product B", Price = 5.49m, IsAvailable = false, Categories = new[] { new Category { Name = "Category 2" } } }
                   }
            };

            //Muestro en connsola los datos antes de la conversión
            Console.WriteLine("Converting Order to OrderDto using AutoMapper...");
            Console.WriteLine($"Order ID: {order.Id}");
            Console.WriteLine($"Customer Name: {order.CustomerDetails?.Name}");
            Console.WriteLine($"Order Date: {order.OrderDate.ToString("yyyy-MM-dd")}");
            Console.WriteLine("Ordered Products:");
            foreach (var product in order.Products)
            {
                Console.WriteLine($"- {product.Name} ({(product.IsAvailable ? "Available" : "Not available")}) - ${product.Price.ToString("0.00", CultureInfo.InvariantCulture)} - Categories: {string.Join(", ", product.Categories.Select(c => c.Name))}");
            }

            Console.WriteLine();

            //Muestro en consola el resultado de la conversión
            var orderDto = mapper.Map<OrderDto>(order);
            Console.WriteLine($"Order ID: {orderDto.Id}");
            Console.WriteLine($"Customer Name: {orderDto.CustomerName}");
            Console.WriteLine($"Order Date: {orderDto.OrderDateFormatted}");
            Console.WriteLine("Ordered Products:");
            foreach (var product in orderDto.OrderedProducts)
            {
                Console.WriteLine($"- {product.Name} ({product.Status}) - {product.Price} - Categories: {product.CategoryList}");
            }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile(ITenantResolver tenantResolver)
            {
                CreateMap<Order, OrderDto>()
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerDetails != null ? src.CustomerDetails.Name : null))
                    .ForMember(dest => dest.OrderDateFormatted, opt => opt.MapFrom(src => src.OrderDate.ToString("yyyy-MM-dd")))
                    .ForMember(dest => dest.OrderedProducts, opt => opt.MapFrom(src => src.Products))
                    .ForMember(dest => dest.TenantInformation, opt => opt.MapFrom(src => src.CustomerDetails != null ? tenantResolver.Resolve(src.CustomerDetails.TenantId) : null));

                CreateMap<Product, ProductDto>()
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.IsAvailable ? "Available" : "Not available"))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.IsAvailable ? $"${src.Price.ToString("0.00", CultureInfo.InvariantCulture)}" : null))
                    .ForMember(dest => dest.CategoryList, opt => opt.MapFrom(src => src.Categories != null ? string.Join(", ", src.Categories.Select(c => c.Name)) : null));

                CreateMap<Tenant, TenantDto>();
            }
        }
    }
}
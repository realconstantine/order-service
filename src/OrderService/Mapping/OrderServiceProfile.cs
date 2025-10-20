using AutoMapper;
using OrderService.Data.Entities;
using OrderService.Models.Requests;

namespace OrderService.Mapping
{
    public class OrderServiceProfile : Profile
    {
        public OrderServiceProfile() 
        {
            CreateMap<CreateOrderRequest, Order>()
                .ForMember(dest => dest.ModifyAt, option => option.MapFrom(source => source.CreatedAt))
                .ForMember(dest => dest.ModifyBy, option => option.MapFrom(_ => Environment.MachineName))
                .ForMember(dest => dest.Products, option => option.MapFrom(source => source.Items))
                .AfterMap((source, dest) => dest.Products.ForEach(product => 
                {
                    product.CreatedAt = source.CreatedAt;
                    product.ModifyAt = source.CreatedAt;
                    product.Order = dest;
                    product.OrderId = source.OrderId;
                }));


            CreateMap<OrderedItem, OrderedProduct>()
                .ForMember(dest => dest.OrderedProductKey, option => option.Ignore())
                .ForMember(dest => dest.ModifyBy, option => option.MapFrom(_ => Environment.MachineName));
        }
    }
}

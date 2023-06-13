using BikeStores.Models;
using Order.Host.Models.Dtos;

namespace Order.Host.Mapping
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderEntity, OrderDto>();
        }
    }
}

using AutoMapper;
using Basket.Api.Entities;
using EventBus.Messages.Events;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Basket.Api.Mapper
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}

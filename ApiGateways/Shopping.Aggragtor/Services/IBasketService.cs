using Shopping.Aggragtor.Models;

namespace Shopping.Aggragtor.Services
{
    public interface IBasketService
    {
        Task<BasketModel> GetBasket(string userName);
    }
}

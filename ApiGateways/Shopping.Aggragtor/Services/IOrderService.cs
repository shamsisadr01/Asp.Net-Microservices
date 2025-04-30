using Shopping.Aggragtor.Models;

namespace Shopping.Aggragtor.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseModel>> GetOrderByUserName(string userName);
    }
}

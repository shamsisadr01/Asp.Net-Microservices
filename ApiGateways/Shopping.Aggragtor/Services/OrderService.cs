using Shopping.Aggragtor.Models;

namespace Shopping.Aggragtor.Services
{
    public class OrderService : IOrderService
    {
        public Task<IEnumerable<OrderResponseModel>> GetOrderByUserName(string userName)
        {
            throw new System.NotImplementedException();
        }
    }
}

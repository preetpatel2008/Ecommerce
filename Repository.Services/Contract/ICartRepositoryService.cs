using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Contract
{
    public interface ICartRepositoryService
    {
        long AddUpdateCart(CartModel objcartModel);
        Task<List<CartModel>> Selcart(int userId);
        long RemoveCart(int? CartID, int? UserId);
    }
}

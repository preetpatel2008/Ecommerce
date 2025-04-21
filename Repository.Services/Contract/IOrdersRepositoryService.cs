using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Contract
{
    public interface IOrdersRepositoryService
    {
        long AddUpdateOrders(OrdersModel objordersModel);
        Task<List<OrderDetailsModel>> SelOrders(int userId);
    }
}

using Entity;
using Repository.Services.Context;
using Repository.Services.Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Library
{
    public class OrdersRepositoryService:IOrdersRepositoryService
    {
        private readonly AppDbContext context;

        public OrdersRepositoryService(AppDbContext context)
        {
            this.context = context;
        }

        public long AddUpdateOrders(OrdersModel objordersModel)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@pOrderId", objordersModel.OrderId));
                parms.Add(new SqlParameter("@pUserId", objordersModel.UserId));
                parms.Add(new SqlParameter("@pName", objordersModel.Name));
                parms.Add(new SqlParameter("@pMobileNumber", objordersModel.MobileNumber));
                parms.Add(new SqlParameter("@pPaymentMethod", objordersModel.PaymentMethod));
                parms.Add(new SqlParameter("@pAddress", objordersModel.Address));
                parms.Add(new SqlParameter("@pTotalProducts", objordersModel.TotalProducts));
                parms.Add(new SqlParameter("@pTotalPrice", objordersModel.TotalPrice));
                parms.Add(new SqlParameter("@pPlacedOn", objordersModel.PlacedOn));
                parms.Add(new SqlParameter("@pPaymentStatus", objordersModel.PaymentStatus));
                SqlParameter pOrderIdReturn = new SqlParameter("@pOrderIdReturn", SqlDbType.BigInt) { Value = Convert.ToInt64(0), Direction = ParameterDirection.Output };
                parms.Add(pOrderIdReturn);
                context.Database.ExecuteSqlCommand("EXEC dbo.[ins_upd_Orders] @OrderId=@pOrderId, @UserId = @pUserId, @Name=@pName,@MobileNumber=@pMobileNumber, " +
                    "@PaymentMethod=@pPaymentMethod, @Address=@pAddress,@TotalProducts =@pTotalProducts,@TotalPrice =@pTotalPrice,@PlacedOn =@pPlacedOn,@PaymentStatus =@pPaymentStatus," +
                    "@OrderIdReturn=@pOrderIdReturn OUTPUT", parms.ToArray());
                return pOrderIdReturn.Value != DBNull.Value ? Convert.ToInt64(pOrderIdReturn.Value) : 0;

            }       
            catch (Exception ex)
            {
                throw;
            }

        }

        public async Task<List<OrderDetailsModel>> SelOrders(int userId)
        {
            return await context.Database.SqlQuery<OrderDetailsModel>("EXEC [sel_UserOrders] @UserId = {0}", userId).ToListAsync();
        }
    }
}

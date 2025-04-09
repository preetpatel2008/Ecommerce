using Entity;
using Repository.Services.Context;
using Repository.Services.Contract;
using Repository.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Library
{
    public class CartRepositoryService : ICartRepositoryService
    {
        private readonly AppDbContext context;

        public CartRepositoryService(AppDbContext context)
        {
            this.context = context;
        }
        public long AddUpdateCart(CartModel objcartModel)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@pCartId",objcartModel.CartId));
                parms.Add(new SqlParameter("@pUserId", objcartModel.UserId));
                parms.Add(new SqlParameter("@pProductId", objcartModel.ProductId));
                parms.Add(new SqlParameter("@pProductName", objcartModel.ProductName));
                parms.Add(new SqlParameter("@pPrice", objcartModel.Price));
                parms.Add(new SqlParameter("@pQuantity",objcartModel.Quantity));
                parms.Add(new SqlParameter("@pImageUrl",objcartModel.ImageUrl));
                SqlParameter pCartIdReturn = new SqlParameter("@pCartIdReturn", SqlDbType.BigInt) { Value = Convert.ToInt64(0), Direction = ParameterDirection.Output };
                parms.Add(pCartIdReturn);
                context.Database.ExecuteSqlCommand("EXEC dbo.[ins_upd_Cart] @CartId=@pCartId, @UserId = @pUserId, @ProductId=@pProductId,@ProductName=@pProductName, " +
                    "@Price=@pPrice, @Quantity=@pQuantity,@ImageUrl=@pImageUrl ," +
                    "@CartIdReturn=@pCartIdReturn OUTPUT", parms.ToArray());
                return pCartIdReturn.Value != DBNull.Value ? Convert.ToInt64(pCartIdReturn.Value) : 0;

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<List<CartModel>> Selcart()
        {
            return await context.Database.SqlQuery<CartModel>("EXEC [sel_Cart]").ToListAsync();
        }
    }

}

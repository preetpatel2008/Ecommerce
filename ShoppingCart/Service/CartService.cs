using Entity;
using Repository.Services.Context;
using Repository.Services.Contract;
using Repository.Services.Infrastructure;
using Repository.Services.Library;

namespace ShoppingCart.Service
{
    public class CartService
    {

        private readonly AppDbContext _context;
        private readonly ICartRepositoryService _cartRepositoryService;
        public CartService(AppDbContext context, ICartRepositoryService CartRepositoryService)
        {
            _context = context;
            _cartRepositoryService = CartRepositoryService;
        }

        public async Task<List<CartModel>> GetAllitems(int userId)
        {
            try
            {
                //Get All Products
                return await _cartRepositoryService.Selcart(userId);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<CartModel>();
            }
        }

        public async Task<bool> AddUpdateCart(CartModel objcartModel)
        {
            try
            {
                //User Entry    
                _cartRepositoryService.AddUpdateCartTable(objcartModel);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveCart(int cartId)
        {
            try
            {
                //User Entry    
                _cartRepositoryService.RemoveCart(cartId);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}

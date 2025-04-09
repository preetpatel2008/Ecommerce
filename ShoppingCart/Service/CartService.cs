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

        public async Task<List<CartModel>> GetAllitems()
        {
            try
            {
                //Get All Products
                return await _cartRepositoryService.Selcart();

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
                _cartRepositoryService.AddUpdateCart(objcartModel);
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

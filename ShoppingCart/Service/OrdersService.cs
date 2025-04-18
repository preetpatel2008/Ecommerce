using Entity;
using Microsoft.AspNetCore.Http;
using Repository.Services.Context;
using Repository.Services.Contract;
using Repository.Services.Library;

namespace ShoppingCart.Service
{
    public class OrdersService
    {
        private readonly AppDbContext _context;
        private readonly IOrdersRepositoryService _ordersRepositoryService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrdersService(AppDbContext context, IOrdersRepositoryService OrdersRepositoryService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _ordersRepositoryService = OrdersRepositoryService;
            _httpContextAccessor = httpContextAccessor;
        } 
        public async Task<bool> AddUpdateOrders(OrdersModel objordersModel)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;
                var loginMasterIdClaim = user.FindFirst("LoginMasterId")?.Value;

                if (string.IsNullOrEmpty(loginMasterIdClaim) || !int.TryParse(loginMasterIdClaim, out int userId))
                {
                    return false;
                }

                objordersModel.UserId = userId;
                objordersModel.PlacedOn = DateTime.Now;
                objordersModel.PaymentStatus = "pending";

                _ordersRepositoryService.AddUpdateOrders(objordersModel);
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

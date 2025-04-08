using Entity;
using ShoppingCart.Service;
using Repository.Services.Context;
using Repository.Services.Contract;
using Repository.Services.Infrastructure;
using Repository.Services.Library;

namespace ShoppingCart.Service
{
    public class ProductService
    {
        private readonly AppDbContext _context;
        private readonly IProductRepositoryService _productRepositoryService;
        public ProductService(AppDbContext context, IProductRepositoryService ProductRepositoryService)
        {
            _context = context;
            _productRepositoryService = ProductRepositoryService;

        }
        public async Task<List<ProductModel>> GetAllProductsAsync()
        {
            try
            {
                //Get All Products
                return  await _productRepositoryService.Selproduct();
             
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<ProductModel>();
            }
        }

        public async Task<bool> AddUpdateProduct(ProductModel productModel, IFormFile imageFile)
        {
            try
            {
                //Product Entry    
                await _productRepositoryService.AddUpdateProductTableAsync(productModel, imageFile);
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

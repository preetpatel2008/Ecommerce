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
        public async Task<(List<ProductModel> Data, int TotalCount)> GetAllProducts(long startIndex, long endIndex)
        {
            try
            {
                //Get All Products
                return  await _productRepositoryService.Selproduct(startIndex, endIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return (new List<ProductModel>(), 0);
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

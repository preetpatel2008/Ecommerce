using Entity;
using Microsoft.AspNetCore.Http;

namespace Repository.Services.Contract
{
    public interface IProductRepositoryService
    {
        Task<long> AddUpdateProductTableAsync(ProductModel productModel, IFormFile imageFile);
        Task<List<ProductModel>> Selproduct();
        long RemoveProduct(int? ProductId);
        Task<bool> UpdateProductDetails(ProductModel objproductModel);
    }
}

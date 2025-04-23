using Entity;
using Microsoft.AspNetCore.Http;

namespace Repository.Services.Contract
{
    public interface IProductRepositoryService
    {
        Task<long> AddUpdateProductTableAsync(ProductModel productModel, IFormFile imageFile);
        Task<(List<ProductModel> Data, int TotalCount)> Selproduct(long startIndex, long endIndex);
        long RemoveProduct(int? ProductId);
        Task<bool> UpdateProductDetails(ProductModel objproductModel);
    }
}

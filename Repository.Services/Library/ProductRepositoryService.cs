using Repository.Services.Context;
using Repository.Services.Contract;
using Entity;
using System.Data.SqlClient;
using System.Data;
using Repository.Services.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Repository.Services.Library
{   
    public class ProductRepositoryService : IProductRepositoryService
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public ProductRepositoryService(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        public async Task<long> AddUpdateProductTableAsync(ProductModel productModel, IFormFile imageFile)
        {
            try
            {
                string uniqueFileName = null;
                if (imageFile != null && imageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(env.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                        
                    productModel.ImageUrl = "/images/" + uniqueFileName;
                }

                List<SqlParameter> parms = new List<SqlParameter>();
           
                    parms.Add(new SqlParameter("@pProductId", UtilityFunctions.DBNullToDB(productModel.ProductId)));
                    parms.Add(new SqlParameter("@pProductName", UtilityFunctions.DBNullToDB(productModel.ProductName)));
                    parms.Add (new SqlParameter("@pPrice", productModel.Price));
                    parms.Add (new SqlParameter("@pImageUrl", UtilityFunctions.DBNullToDB(productModel.ImageUrl)));
             

                SqlParameter pProductIdReturn = new SqlParameter("@pProductIdReturn", SqlDbType.Int) { Value = Convert.ToInt64(0), Direction = ParameterDirection.InputOutput };
                parms.Add(pProductIdReturn);

                context.Database.ExecuteSqlCommand("EXEC dbo.[ins_upd_Product] @ProductId=@pProductId, @ProductName=@pProductName, @Price=@pPrice, @ImageUrl=@pImageUrl, @ProductIdReturn=@pProductIdReturn OUTPUT",
                   parms.ToArray());

                return Convert.ToInt64(pProductIdReturn.Value);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public Task<List<ProductModel>> GetAllProductsAsync()
        //{
        //    throw new NotImplementedException();
        //}
        public async Task<List<ProductModel>> Selproduct()
        {
            return await context.Database.SqlQuery<ProductModel>("EXEC [sel_Product]").ToListAsync();
        }
    }
}

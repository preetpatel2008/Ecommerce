using Repository.Services.Context;
using Repository.Services.Contract;
using Entity;
//using System.Data.SqlClient;
using System.Data;
using Repository.Services.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
//using Microsoft.Data.SqlClient;
using System.Data.SqlClient;

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

        public async Task<long> AddUpdateProductTableAsync(ProductModel objProductModel, IFormFile imageFile)
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

                    objProductModel.ImageUrl = "/images/" + uniqueFileName;
                }

                List<SqlParameter> paramObject = new List<SqlParameter>();
                SqlParameter pProductId = new SqlParameter("@pProductId", UtilityFunctions.DBNullToDB(objProductModel.ProductId));
                SqlParameter pProductName = new SqlParameter("@pProductName", UtilityFunctions.DBNullToDB(objProductModel.ProductName));
                SqlParameter pPrice = new SqlParameter("@pPrice", UtilityFunctions.DBNullToDB(objProductModel.Price));
                SqlParameter pImageUrl = new SqlParameter("@pImageUrl", UtilityFunctions.DBNullToDB(objProductModel.ImageUrl));

                SqlParameter pProductIdReturn = new SqlParameter("@pProductIdReturn", SqlDbType.BigInt)
                {
                    Direction = ParameterDirection.Output
                };
                //SqlParameter pProductIdReturn = new SqlParameter("@pProductIdReturn", SqlDbType.Int) { Value = Convert.ToInt64(0), Direction = ParameterDirection.InputOutput };
                //paramObject.Add(pProductIdReturn);

                #region Parms Add to list
                paramObject.Add(pProductId);
                paramObject.Add(pProductName);
                paramObject.Add(pPrice);
                paramObject.Add(pImageUrl);
                paramObject.Add(pProductIdReturn);

                #endregion

                context.Database.ExecuteSqlCommand("EXEC dbo.[ins_upd_Product] @ProductId=@pProductId, @ProductName=@pProductName, @Price=@pPrice, @ImageUrl=@pImageUrl, @ProductIdReturn=@pProductIdReturn OUTPUT",
                   paramObject.ToArray());

                return Convert.ToInt64(pProductIdReturn.Value);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //public async Task<List<ProductModel>> Selproduct()
        //{
        //    return await context.Database.SqlQuery<ProductModel>("EXEC [sel_Product]").ToListAsync();
        //}

        public async Task<(List<ProductModel> Data, int TotalCount)> Selproduct(long startIndex, long endIndex)
        {
            try
            {
                var result = await context.Database
                    .SqlQuery<ProductModel>("EXEC sel_Product @RecordStartIndex, @RecordEndIndex",
                        new SqlParameter("@RecordStartIndex", startIndex),
                        new SqlParameter("@RecordEndIndex", endIndex))
                    .ToListAsync();

                int totalCount = result.Any() ? result.First().TotalRecordCount : 0;

                return (result, totalCount);
            }
            catch (Exception ex) 
            {
                throw;
            }
        }

        public long RemoveProduct(int? ProductId)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@pProductId", UtilityFunctions.DBNullToDB(ProductId)));
                return context.Database.ExecuteSqlCommand("EXEC [del_Product] @ProductId=@pProductId", parms.ToArray());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateProductDetails(ProductModel objproductModel)
        {
            try
            {
                var productIdReturnParam = new SqlParameter("@ProductIdReturn", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                var parameters = new[]
                {
                    new SqlParameter("@ProductId", objproductModel.ProductId),
                    new SqlParameter("@ProductName", objproductModel.ProductName),
                    new SqlParameter("@Price", objproductModel.Price),
                    new SqlParameter("@ImageUrl", objproductModel.ImageUrl),
                    productIdReturnParam
                };

                await context.Database.ExecuteSqlCommandAsync("EXEC [ins_upd_Product] @ProductId, @ProductName, @Price, @ImageUrl,@ProductIdReturn OUTPUT", parameters);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("UpdateUserProfile Error: " + ex.Message);
                return false;
            }
        }
    }
}
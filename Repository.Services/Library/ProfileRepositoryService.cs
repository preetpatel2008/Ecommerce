using Entity;
using Microsoft.AspNetCore.Hosting;
using Repository.Services.Context;
using Repository.Services.Contract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;    

namespace Repository.Services.Library
{
    public class ProfileRepositoryService : IProfileRepositoryService
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public ProfileRepositoryService(AppDbContext context, IWebHostEnvironment env)
        {
            this.context = context;
            this.env = env;
        }

        public async Task<ProfileModel> Selprofile(int loginMasterId)
        {
            var result = await context.Database.SqlQuery<ProfileModel>("EXEC [sel_UserProfile] @LoginMasterId", new SqlParameter("@LoginMasterId", loginMasterId))
                .ToListAsync();

            return result.FirstOrDefault(); // return single profile
        }
        public async Task<bool> UpdateUserProfile(ProfileModel objprofileModel)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@LoginMasterId", objprofileModel.LoginMasterId),
                    new SqlParameter("@FirstName", objprofileModel.FirstName),
                    new SqlParameter("@Email", objprofileModel.Email),
                    new SqlParameter("@MobileNumber", objprofileModel.MobileNumber),
                    new SqlParameter("@Address", objprofileModel.Address)
                };

                await context.Database.ExecuteSqlCommandAsync("EXEC [upd_UserProfile] @LoginMasterId, @FirstName, @Email, @MobileNumber, @Address", parameters);
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

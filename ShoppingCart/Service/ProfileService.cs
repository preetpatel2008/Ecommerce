using Entity;
using Repository.Services.Context;
using Repository.Services.Contract;
using Repository.Services.Library;

namespace ShoppingCart.Service
{
    public class ProfileService
    {
        private readonly AppDbContext _context;
        private readonly IProfileRepositoryService _profileRepositoryService;
        public ProfileService(AppDbContext context, IProfileRepositoryService ProfileRepositoryService)
        {
            _context = context;
            _profileRepositoryService = ProfileRepositoryService;
        }

        public async Task<ProfileModel> GetUserProfile(int loginMasterId)
        {
            try
            {
              
                return await _profileRepositoryService.Selprofile(loginMasterId);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new ProfileModel();
            }
        }

        public async Task<bool> UpdateUserProfile(ProfileModel objprofileModel)
        {
            try
            {
                //User Entry    
                return await _profileRepositoryService.UpdateUserProfile(objprofileModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }


    }
}

using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Services.Contract
{
    public  interface IProfileRepositoryService
    {
        Task<ProfileModel> Selprofile(int loginMasterId);
        Task<bool> UpdateUserProfile(ProfileModel objprofileModel);
    }
}

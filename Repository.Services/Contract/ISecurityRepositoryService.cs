
using Entity;

namespace Repository.Services.Contract
{
    public interface ISecurityRepositoryService
    {
        long CheckIsAuthenticate(string Email);
        Task<LoginModel> GetLoginMasterByID(long LoginMasterID);
        LoginModel GetWebAppLoginMasterByID(long LoginMasterID);
        void UpdateIsProfileComplete(long LoginMasterID);
        Task UpdateDeviceInfoAsync(AuthModel objLogin);
        Task<LoginModel> GetLoginMasterByEmailID(string Email);
        long AddUpdateLoginMaster(LoginModel objLogin);
        long VerifyAccountByEmail(long LognMasterID, string VerificationCode);
        LoginModel ChangePassword(long LoginMasterID, string OldPassword, string NewPassword);
        long UpdateProfile(UserModel usetModel);
        Task<UserProfile> GetProfile(long UserID);
        int CheckIsEmailExistForgotPassword(string Email);
        long UpdateVerificationCode(string Email, string VerificationCode);
        void UpdateOTP(long LoginMasterID, string VerificationCode);
        void UpdateVerificationCodeByRegister(string Email, string VerificationCode);
        void UpdateEmail(long LoginMasterID, string Email);
        void UpdateVerificationCodeAndDateTime(string Email, string VerificationCode, DateTime verificationCodeDateTime);
        bool ResetPassword(long LoginMasterID, string Password);
        public long UpdateSocialLogin(LoginModel objLogin);
        Task<List<ConfigurationModel>> GetConfiguration();
        long DeleteAccount(long? LoginMasterID);
        long LogoutMobile(long? LoginMasterID);
        void UpdateCulture(long LoginMasterID, int CultureID);
        void InsertLoginDetails(long LoginMasterID, bool IsSuccessful);
        List<LoginHistoryModel> GetLoginHistoryDetails(long LoginMasterID);
        List<LoginModel> GetAllInternalUserList(int Type, long EntityTypeID);
    }
}

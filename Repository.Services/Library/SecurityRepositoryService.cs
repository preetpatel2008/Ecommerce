using Repository.Services.Context;
using Repository.Services.Contract;
using Entity;
using System.Data.SqlClient;
using System.Data;
using Repository.Services.Infrastructure;
using System.Data.Entity.Infrastructure;



namespace Repository.Services.Library
{
    public class SecurityRepositoryService : ISecurityRepositoryService
    {
        private readonly AppDbContext context;

        public SecurityRepositoryService(AppDbContext context)
        {
            this.context = context;
        }
        public long AddUpdateLoginMaster(LoginModel objLogin)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                parms.Add(new SqlParameter("@pLoginMasterID", UtilityFunctions.DBNullToDB(objLogin.LoginMasterID)));
                parms.Add(new SqlParameter("@pEntityTypeID", objLogin.EntityTypeID));
                parms.Add(new SqlParameter("@pEntityID", objLogin.EntityID));
                parms.Add(new SqlParameter("@pFirstName", objLogin.FirstName));
                parms.Add(new SqlParameter("@pEmail", objLogin.Email));
                parms.Add(new SqlParameter("@pPassword", UtilityFunctions.DBNullToDB(objLogin.Password)));
                parms.Add(new SqlParameter("@pVerificationCode", UtilityFunctions.DBNullToDB(objLogin.VerificationCode)));
                // parms.Add(new SqlParameter("@pVerificationCodeDateTime", UtilityFunctions.DBNullToDB(objLogin.VerificationCodeDateTime)));
                // parms.Add(new SqlParameter("@pCultureID", objLogin.CultureID));
                parms.Add(new SqlParameter("@pCreatedByID", UtilityFunctions.DBNullToDB(objLogin.CreatedByID)));
                parms.Add(new SqlParameter("@pModifiedByID", UtilityFunctions.DBNullToDB(objLogin.ModifiedByID)));
                SqlParameter pLoginMasterIDreturn = new SqlParameter("@pLoginMasterIDreturn", SqlDbType.BigInt) { Value = Convert.ToInt64(0), Direction = ParameterDirection.InputOutput };
                parms.Add(pLoginMasterIDreturn);
                context.Database.ExecuteSqlCommand("EXEC dbo.[ins_upd_LoginMaster] @LoginMasterID=@pLoginMasterID, @EntityTypeID = @pEntityTypeID, @EntityID=@pEntityID,@FirstName=@pFirstName, " +
                    "@Email=@pEmail, @Password=@pPassword, " +
                    " @VerificationCode=@pVerificationCode,@CreatedByID=@pCreatedByID,@ModifiedByID=@pModifiedByID," +
                    "@LoginMasterIDreturn=@pLoginMasterIDreturn OUTPUT", parms.ToArray());
                return Convert.ToInt64(pLoginMasterIDreturn.Value);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public LoginModel ChangePassword(long LoginMasterID, string OldPassword, string NewPassword)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@pLoginMasterID", LoginMasterID));
            parms.Add(new SqlParameter("@pOldPassword", OldPassword));
            parms.Add(new SqlParameter("@pNewPassword", NewPassword));


            return context.Database.SqlQuery<LoginModel>("EXEC dbo.[ChangePassword] @LoginMasterID=@pLoginMasterID," +
                  "@OldPassword=@pOldPassword, @NewPassword=@pNewPassword", parms.ToArray()).SingleOrDefault();
        }
        public long CheckIsAuthenticate(string Email)

        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@pEmail", Email));
            SqlParameter pLoginMasterID = new SqlParameter("@pLoginMasterID", SqlDbType.BigInt) { Value = Convert.ToInt64(0), Direction = ParameterDirection.InputOutput };
            parms.Add(pLoginMasterID);
            context.Database.ExecuteSqlCommand("dbo.[CheckIsAuthenticate] @Email=@pEmail, @LoginMasterID = @pLoginMasterID OUTPUT", parms.ToArray());
            return Convert.ToInt64(pLoginMasterID.Value);
        }
        public int CheckIsEmailExistForgotPassword(string Email)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@pEmail", UtilityFunctions.DBNullToDB(Email)));
            SqlParameter pIsExistreturn = new SqlParameter("@pLoginMasterID", SqlDbType.Int) { Value = Convert.ToInt32(0), Direction = ParameterDirection.InputOutput };
            parms.Add(pIsExistreturn);
            context.Database.ExecuteSqlCommand("EXEC CheckIsEmailExistForgotPassword @Email=@pEmail,@LoginMasterID=@pLoginMasterID OUTPUT", parms.ToArray());
            return Convert.ToInt32(pIsExistreturn.Value);
        }
        public async Task<LoginModel> GetLoginMasterByID(long LoginMasterID)
        {
            LoginModel objLoginModel = new LoginModel();
            objLoginModel = context.Database.SqlQuery<LoginModel>("EXEC GetLoginMasterByID {0}", LoginMasterID).ToList().FirstOrDefault();
            if (objLoginModel == null)
            {

            }
            return objLoginModel;
        }
        public LoginModel GetWebAppLoginMasterByID(long LoginMasterID)
        {
            LoginModel objLoginModel = new LoginModel();
            objLoginModel = context.Database.SqlQuery<LoginModel>("EXEC GetWebAppLoginMasterByID {0}", LoginMasterID).ToList().FirstOrDefault();
            return objLoginModel;
        }
        public void UpdateIsProfileComplete(long LoginMasterID)
        {
            var pLoginMasterID = new SqlParameter("@pLoginMasterID", SqlDbType.BigInt)
            {
                Value = LoginMasterID
            };
            context.Database.ExecuteSqlCommand("EXEC upd_Isprofilecomplete @pLoginMasterID", pLoginMasterID);
        }
        public async Task<LoginModel> GetLoginMasterByEmailID(string Email)
        {
            LoginModel objLoginModel = new LoginModel();
            objLoginModel = context.Database.SqlQuery<LoginModel>("EXEC GetLoginMasterByEmailID {0}", Email).ToList().FirstOrDefault();
            return objLoginModel;
        }
        public async Task UpdateDeviceInfoAsync(AuthModel objLogin)
        {
            try
            {
                var parameters = new List<SqlParameter>
            {
                //new SqlParameter("@DeviceID", objLogin.DeviceID),
                //new SqlParameter("@DeviceToken", objLogin.DeviceToken),
                //new SqlParameter("@DeviceType", objLogin.DeviceType),
                new SqlParameter("@Email", objLogin.Email),
            };
                context.Database.ExecuteSqlCommand(
                                   "EXEC dbo.UpdateDeviceInfo @DeviceID, @DeviceToken, @DeviceType ,@Email",
                                   parameters.ToArray());
            }
            catch (Exception ex)
            {
                // Handle exception
                throw;
            }
        }
        public async Task<UserProfile> GetProfile(long UserID)
        {
            UserProfile objUserProfile = new UserProfile();
            using (var db = context)
            {
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "[dbo].[sel_ProfileByID]";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@UserID", UserID));
                // execute your command
                db.Database.Connection.Open();
                var reader = cmd.ExecuteReader();
                // Read second model --> Bar
                objUserProfile.objUser = ((IObjectContextAdapter)db)
                        .ObjectContext
                        .Translate<UserModel>(reader).FirstOrDefault();
                reader.NextResult();

                //this commit Harshad
                //objUserProfile.LstPaymentTransaction = ((IObjectContextAdapter)db)
                //           .ObjectContext
                //           .Translate<PaymentTransactionModel>(reader).ToList();

                // move to next result set
            }
            return objUserProfile;
            //return context.Database.SqlQuery<UserModel>("EXEC [sel_ProfileByID] {0}", UserID).ToList().FirstOrDefault();
        }
        public bool ResetPassword(long LoginMasterID, string Password)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();

                parms.Add(new SqlParameter("@pLoginMasterID", UtilityFunctions.DBNullToDB(LoginMasterID)));
                parms.Add(new SqlParameter("@pPassword", UtilityFunctions.DBNullToDB(Password)));
                context.Database.ExecuteSqlCommand("EXEC [upd_ResetPassword] @LoginMasterID=@pLoginMasterID,@Password=@pPassword", parms.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public long UpdateProfile(UserModel userModel)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();

                parms.Add(new SqlParameter("@pUserID", UtilityFunctions.SafeDbObject(userModel.UserID)));
                parms.Add(new SqlParameter("@pLoginMasterID", UtilityFunctions.SafeDbObject(userModel.LoginMasterID)));
                parms.Add(new SqlParameter("@pFirstName", UtilityFunctions.SafeDbObject(userModel.FirstName)));
                parms.Add(new SqlParameter("@pLastName", UtilityFunctions.SafeDbObject(userModel.LastName)));
                parms.Add(new SqlParameter("@pPhone", UtilityFunctions.SafeDbObject(userModel.Phone)));
                parms.Add(new SqlParameter("@pEmail", UtilityFunctions.SafeDbObject(userModel.Email)));
                parms.Add(new SqlParameter("@pUserPhotoTitle", UtilityFunctions.SafeDbObject(userModel.UserPhotoTitle)));
                parms.Add(new SqlParameter("@pUserPhotoName", UtilityFunctions.SafeDbObject(userModel.UserPhotoName)));
                var pUserPhotoImage = new SqlParameter("@pUserPhotoImage", UtilityFunctions.SafeDbObject(userModel.UserPhotoImage))
                {
                    SqlDbType = SqlDbType.Image
                };
                parms.Add(pUserPhotoImage);
                parms.Add(new SqlParameter("@pIsShowPicture", UtilityFunctions.SafeDbObject(userModel.IsShowPicture)));
                parms.Add(new SqlParameter("@pWebsite", UtilityFunctions.SafeDbObject(userModel.Website)));
                parms.Add(new SqlParameter("@pNationality", UtilityFunctions.SafeDbObject(userModel.Nationality)));
                parms.Add(new SqlParameter("@pBirthDate", UtilityFunctions.SafeDbObject(userModel.BirthDate)));
                parms.Add(new SqlParameter("@pProfileSummary", UtilityFunctions.SafeDbObject(userModel.ProfileSummary)));
                parms.Add(new SqlParameter("@pIsChangeImage", UtilityFunctions.SafeDbObject(userModel.IsChangeImage)));
                parms.Add(new SqlParameter("@pAddress", UtilityFunctions.SafeDbObject(userModel.Address)));
                SqlParameter pUserIDreturn = new SqlParameter("@pUserIDreturn", SqlDbType.BigInt) { Value = Convert.ToInt64(0), Direction = ParameterDirection.InputOutput };
                parms.Add(pUserIDreturn);

                context.Database.ExecuteSqlCommand("EXEC dbo.[upd_Profile] @UserID=@pUserID,@LoginMasterID=@pLoginMasterID," +
                    "@FirstName=@pFirstName, @LastName=@pLastName, @Phone=@pPhone ,@Email=@pEmail," +
                    "@UserPhotoTitle=@pUserPhotoTitle,@UserPhotoName=@pUserPhotoName,@UserPhotoImage=@pUserPhotoImage,@IsShowPicture=@pIsShowPicture,@Website=@pWebsite,@Nationality=@pNationality,@BirthDate=@pBirthDate,@ProfileSummary=@pProfileSummary," +
                    "@IsChangeImage=@pIsChangeImage,@Address=@pAddress,@UserIDreturn=@pUserIDreturn OUTPUT", parms.ToArray());
                return Convert.ToInt64(pUserIDreturn.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public long UpdateVerificationCode(string Email, string VerificationCode)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter pEmail = new SqlParameter("@pEmail", UtilityFunctions.DBNullToDB(Email));
            SqlParameter pVerificationCode = new SqlParameter("@pVerificationCode", UtilityFunctions.DBNullToDB(VerificationCode));
            SqlParameter pLoginMasterID = new SqlParameter("@pLoginMasterID", SqlDbType.BigInt) { Value = Convert.ToInt32(0), Direction = ParameterDirection.InputOutput };
            parms.Add(pEmail);
            parms.Add(pVerificationCode);
            parms.Add(pLoginMasterID);
            context.Database.ExecuteSqlCommand("EXEC [upd_VerificationCode] @Email=@pEmail,@VerificationCode=@pVerificationCode,@LoginMasterID=@pLoginMasterID OUTPUT", parms.ToArray());
            return Convert.ToInt32(pLoginMasterID.Value);
        }
        public void UpdateOTP(long LoginMasterID, string VerificationCode)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter pLoginMasterID = new SqlParameter("@pLoginMasterID", UtilityFunctions.DBNullToDB(LoginMasterID));
            SqlParameter pVerificationCode = new SqlParameter("@pVerificationCode", UtilityFunctions.DBNullToDB(VerificationCode));
            parms.Add(pLoginMasterID);
            parms.Add(pVerificationCode);
            context.Database.ExecuteSqlCommand("EXEC [UpdateOTP] @LoginMasterID=@pLoginMasterID,@VerificationCode=@pVerificationCode", parms.ToArray());
        }
        public void UpdateVerificationCodeByRegister(string Email, string VerificationCode)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter pEmail = new SqlParameter("@pEmail", UtilityFunctions.DBNullToDB(Email));
            SqlParameter pVerificationCode = new SqlParameter("@pVerificationCode", UtilityFunctions.DBNullToDB(VerificationCode));
            parms.Add(pEmail);
            parms.Add(pVerificationCode);
            context.Database.ExecuteSqlCommand("EXEC [VerifyAccount] @Email=@pEmail,@VerificationCode=@pVerificationCode", parms.ToArray());
        }
        public void UpdateEmail(long LoginMasterID, string EMail)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter pLoginMasterID = new SqlParameter("@pLoginMasterID", UtilityFunctions.DBNullToDB(LoginMasterID));
            SqlParameter pEMail = new SqlParameter("@pEMail", UtilityFunctions.DBNullToDB(EMail));
            parms.Add(pLoginMasterID);
            parms.Add(pEMail);
            context.Database.ExecuteSqlCommand("EXEC [upd_Email] @LoginMasterID=@pLoginMasterID,@EMail=@pEMail", parms.ToArray());
        }
        public void UpdateVerificationCodeAndDateTime(string Email, string VerificationCode, DateTime verificationCodeDateTime)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter pEmail = new SqlParameter("@pEmail", UtilityFunctions.DBNullToDB(Email));
            SqlParameter pVerificationCode = new SqlParameter("@pVerificationCode", UtilityFunctions.DBNullToDB(VerificationCode));
            SqlParameter pVerificationCodeDateTime = new SqlParameter("@pVerificationCodeDateTime", UtilityFunctions.DBNullToDB(verificationCodeDateTime));
            parms.Add(pEmail);
            parms.Add(pVerificationCode);
            parms.Add(pVerificationCodeDateTime);
            context.Database.ExecuteSqlCommand("EXEC [upd_VerificationCodeandDatetime] @Email=@pEmail,@VerificationCode=@pVerificationCode,@VerificationCodeDateTime=@pVerificationCodeDateTime", parms.ToArray());
        }
        public long VerifyAccountByEmail(long LognMasterID, string VerificationCode)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                int type = 0;
                SqlParameter pType = new SqlParameter("@pType", type);
                SqlParameter pLoginMasterID = new SqlParameter("@pLoginMasterID", LognMasterID);
                SqlParameter pVerificationCode = new SqlParameter("@pVerificationCode", VerificationCode);

                SqlParameter preturnStatus = new SqlParameter("@pReturnStatus", SqlDbType.BigInt) { Value = Convert.ToInt64(0), Direction = ParameterDirection.InputOutput };

                parms.Add(pType);
                parms.Add(pLoginMasterID);
                parms.Add(pVerificationCode);
                parms.Add(preturnStatus);


                context.Database.ExecuteSqlCommand("EXEC dbo.[upd_VerificationCodeByID] @Type=@pType, @LoginMasterID=@pLoginMasterID, " +
                    "@VerificationCode=@pVerificationCode, @ReturnStatus=@pReturnStatus OUTPUT", parms.ToArray());
                return Convert.ToInt32(preturnStatus.Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public long UpdateSocialLogin(LoginModel objLogin)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            SqlParameter pLoginMasterID = new SqlParameter("@pLoginMasterID", objLogin.LoginMasterID);
            //SqlParameter pLoginProvider = new SqlParameter("@pLoginProvider", objLogin.AuthenticationType);
            //SqlParameter pProviderKey = new SqlParameter("@pProviderKey", objLogin.AuthenticationProviderKey);
            //SqlParameter pProviderDisplayName = new SqlParameter("@pProviderDisplayName", objLogin.FirstName + objLogin.LastName);
            parms.Add(pLoginMasterID);
            //parms.Add(pLoginProvider);
            //parms.Add(pProviderKey);
            //parms.Add(pProviderDisplayName);

            return context.Database.ExecuteSqlCommand("EXEC dbo.[ins_upd_LoginMasterLogins] @LoginMasterID=@pLoginMasterID,@LoginProvider=@pLoginProvider,@ProviderKey=@pProviderKey,@ProviderDisplayName=@pProviderDisplayName", parms.ToArray());
        }
        public async Task<List<ConfigurationModel>> GetConfiguration()
        {
            return await context.Database.SqlQuery<ConfigurationModel>("EXEC [Sel_Configuration]").ToListAsync();
        }
        public long DeleteAccount(long? LoginMasterID)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@pLoginMasterID", LoginMasterID));
            SqlParameter pIsExistreturn = new SqlParameter("@pIsExistreturn", SqlDbType.BigInt) { Value = Convert.ToInt64(0), Direction = ParameterDirection.InputOutput };
            parms.Add(pIsExistreturn);
            context.Database.ExecuteSqlCommand("EXEC del_Account @LoginMasterID = @pLoginMasterID, @pIsExistreturn = @pIsExistreturn OUTPUT", parms.ToArray());
            return (long)pIsExistreturn.Value;
        }
        public long LogoutMobile(long? LoginMasterID)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@pLoginMasterID", LoginMasterID));
            SqlParameter IsExist = new SqlParameter("@IsExist", SqlDbType.BigInt) { Value = Convert.ToInt64(0), Direction = ParameterDirection.InputOutput };
            parms.Add(IsExist);
            context.Database.ExecuteSqlCommand("EXEC upd_deviceToken @LoginMasterID = @pLoginMasterID,@IsExist= @IsExist OUTPUT", parms.ToArray());
            return (long)IsExist.Value;
        }
        public void UpdateCulture(long LoginMasterID, int CultureID)
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>();
                SqlParameter pLoginMasterID = new SqlParameter("@pLoginMasterID", UtilityFunctions.DBNullToDB(LoginMasterID));
                SqlParameter pCultureID = new SqlParameter("@pCultureID", UtilityFunctions.DBNullToDB(CultureID));
                parms.Add(pLoginMasterID);
                parms.Add(pCultureID);
                context.Database.ExecuteSqlCommand("EXEC [upd_Culture] @LoginMasterID=@pLoginMasterID,@CultureID=@pCultureID", parms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void InsertLoginDetails(long LoginMasterID, bool IsSuccessful)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@pLoginMasterID", LoginMasterID));
            parms.Add(new SqlParameter("@pIsSuccessful", IsSuccessful));
            context.Database.ExecuteSqlCommand("dbo.[ins_LoginHistory] @LoginMasterID=@pLoginMasterID,@IsSuccessful=@pIsSuccessful", parms.ToArray());
            //return Convert.ToInt64(pLoginMasterID.Value);
        }
        public List<LoginHistoryModel> GetLoginHistoryDetails(long LoginMasterID)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@pLoginMasterID", LoginMasterID));
            return context.Database.SqlQuery<LoginHistoryModel>("dbo.[sel_LoginHistory] @LoginMasterID=@pLoginMasterID", parms.ToArray()).ToList();
        }

        public List<LoginModel> GetAllInternalUserList(int Type, long EntityTypeID)
        {
            try
            {
                return context.Database.SqlQuery<LoginModel>("EXEC dbo.[sel_LoginMaster] @Type={0},@EntityTypeID={1}", Type, EntityTypeID).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

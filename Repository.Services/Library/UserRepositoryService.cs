using Repository.Services.Context;
using Repository.Services.Contract;
using Entity;
using System.Data.SqlClient;
using System.Data;
using Repository.Services.Infrastructure;
//using Microsoft.Data.SqlClient;

namespace Repository.Services.Library
{
    public class UserRepositoryService : IUserRepositoryService
    {
        private readonly AppDbContext context;
        public UserRepositoryService(AppDbContext context)
        {
            this.context = context;
        }
        public long AddUpdateLoginMaster(RegisterModel objLogin)
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
                parms.Add(new SqlParameter("@pCreatedByID", UtilityFunctions.DBNullToDB(1)));
                parms.Add(new SqlParameter("@pModifiedByID", UtilityFunctions.DBNullToDB(1)));
                SqlParameter pLoginMasterIDreturn = new SqlParameter("@pLoginMasterIDreturn", SqlDbType.BigInt) { Value = Convert.ToInt64(0), Direction = ParameterDirection.InputOutput };
                parms.Add(pLoginMasterIDreturn);
                context.Database.ExecuteSqlCommand("EXEC dbo.[ins_upd_LoginMaster] @LoginMasterID=@pLoginMasterID, @EntityTypeID = @pEntityTypeID, @EntityID=@pEntityID,@FirstName=@pFirstName, " +
                    "@Email=@pEmail, @Password=@pPassword, " +
                    " @VerificationCode=@pVerificationCode,@CreatedByID=@pCreatedByID,@ModifiedByID=@pModifiedByID," +
                    "@LoginMasterIDreturn=@pLoginMasterIDreturn OUTPUT", parms.ToArray());
                long res = Convert.ToInt64(pLoginMasterIDreturn.Value);
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

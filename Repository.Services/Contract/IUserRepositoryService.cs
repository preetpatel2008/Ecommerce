using Entity;

namespace Repository.Services.Contract
{
    public interface IUserRepositoryService
    {
        long AddUpdateLoginMaster(RegisterModel objLogin);

    }
}

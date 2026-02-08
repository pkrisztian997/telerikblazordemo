
namespace SHD.UserCatalog.BL.DataAccess
{
    internal interface IUserRepository
    {
        Task<IEnumerable<IUser>> GetAllUsersAsync();
    }
}

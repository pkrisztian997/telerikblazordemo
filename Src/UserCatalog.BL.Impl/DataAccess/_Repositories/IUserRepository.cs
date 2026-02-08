

namespace SHD.UserCatalog.BL.DataAccess
{
    internal interface IUserRepository
    {
        Task<IEnumerable<IUser>> GetAllUsersAsync();
        Task<IUser?> GetAuthenticatedUserAsync(string username, string password);
        Task<IUser> GetUserDetailsAsync(Guid userId);
        Task<IUser> UpdateUserAsync(IUser userToUpdate);
    }
}

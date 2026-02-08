using System.Security;

namespace SHD.UserCatalog.BL
{
    public interface IUser
    {
        Guid Id { get; }
        string Username { get; }
        SecureString Password { get; }
        string FirstName { get; }
        string LastName { get; }
        DateTime DateOfBirth { get; }
        string BirthPlace { get; }
        string Residence { get; }
    }
}

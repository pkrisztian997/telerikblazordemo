using SHD.UserCatalog.BL;
using System.Security;

namespace UserCatalog.Web.Components.Pages.ViewModels
{
    public class UserProxy : IUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string BirthPlace { get; set; } = string.Empty;
        public string Residence { get; set; } = string.Empty;
        public SecureString Password { get; set; } = new SecureString();
    }
}

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
        public string PlainPassword { get; set; } = string.Empty;

        public UserProxy(
            Guid id, 
            string username, 
            string firstName, 
            string lastName, 
            DateTime dateOfBirth, 
            string birthPlace, 
            string residence, 
            SecureString password)
        {
            Id = id;
            Username = username ?? throw new ArgumentNullException(nameof(username));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            DateOfBirth = dateOfBirth;
            BirthPlace = birthPlace ?? throw new ArgumentNullException(nameof(birthPlace));
            Residence = residence ?? throw new ArgumentNullException(nameof(residence));
            Password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public UserProxy(
            Guid id,
            string username,
            string firstName,
            string lastName,
            DateTime dateOfBirth,
            string birthPlace,
            string residence,
            string plainPassword)
        {
            _ = string.IsNullOrWhiteSpace(plainPassword) ? throw new ArgumentException("Plain password cannot be null or whitespace.", nameof(plainPassword)) : string.Empty;

            Id = id;
            Username = username ?? throw new ArgumentNullException(nameof(username));
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));
            DateOfBirth = dateOfBirth;
            BirthPlace = birthPlace ?? throw new ArgumentNullException(nameof(birthPlace));
            Residence = residence ?? throw new ArgumentNullException(nameof(residence));
            Password = ConvertPasswordToSecureString(plainPassword);
        }

        private static SecureString ConvertPasswordToSecureString(string passwordPlain)
        {
            var securePwd = new SecureString();
            foreach (var c in passwordPlain)
            {
                securePwd.AppendChar(c);
            }
            securePwd.MakeReadOnly();
            return securePwd;
        }

        // For serialiaztion purposes
        public UserProxy()
        {
        }
    }
}

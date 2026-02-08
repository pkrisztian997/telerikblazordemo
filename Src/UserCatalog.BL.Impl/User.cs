using System.Security;

namespace SHD.UserCatalog.BL
{
    internal class User : IUser
    {
        public Guid Id { get; }
        public string Username { get; }
        public SecureString Password { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateTime DateOfBirth { get; }
        public string BirthPlace { get; }
        public string Residence { get; }

        public User(Guid id, string username, SecureString password, string firstName, string lastName, DateTime dateOfBirth, string birthPlace, string residence)
        {
            Id = id;
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            BirthPlace = birthPlace;
            Residence = residence;
        }

        override public string ToString()
        {
            return $"Id: {Id}, Username: {Username}, FirstName: {FirstName}, LastName: {LastName}, DateOfBirth: {DateOfBirth}, BirthPlace: {BirthPlace}, Residence: {Residence}";
        }
    }
}

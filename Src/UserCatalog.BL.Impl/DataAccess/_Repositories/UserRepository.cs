using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;

namespace SHD.UserCatalog.BL.DataAccess
{
    internal class UserRepository : IUserRepository
    {
        private const int EXPECTED_NUMBER_OF_RECORDS = 8;
        private const string HEADER_LINE = "Id;Username;Password;LastName;FirstName;DateOfBirth;BirthPlace;Residence";
        private readonly string _dataFilePath;

        public UserRepository(string dataFilePath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(dataFilePath);

            _dataFilePath = dataFilePath;
        }

        public async Task<IEnumerable<IUser>> GetAllUsersAsync()
        {
            if (!File.Exists(_dataFilePath))
            {
                return [];
            }

            var users = new List<IUser>();

            using (var sr = new StreamReader(_dataFilePath))
            {
                string? line;
                while ((line = await sr.ReadLineAsync().ConfigureAwait(false)) != null)
                {
                    line = line.Trim();
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    if (line.Equals(HEADER_LINE, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var parts = line.Split(';');
                    if (parts.Length < EXPECTED_NUMBER_OF_RECORDS)
                    {
                        continue;
                    }

                    var idText = parts[0].Trim();
                    if (!Guid.TryParse(idText, out var id))
                    {
                        continue;
                    }

                    ExtractLine(users, parts, id);
                }
            }

            return users;
        }

        private static void ExtractLine(List<IUser> users, string[] parts, Guid id)
        {
            var username = parts[1].Trim();
            var passwordPlain = parts[2].Trim();
            var lastName = parts[3].Trim();
            var firstName = parts[4].Trim();
            var dobText = parts[5].Trim();
            var birthPlace = parts[6].Trim();
            var residence = parts[7].Trim();
            DateTime dateOfBirth = ExtractDateOfBirth(dobText);
            SecureString securePwd = ExtractPassword(passwordPlain);

            users.Add(new User(id, username, securePwd, firstName, lastName, dateOfBirth, birthPlace, residence));
        }

        private static DateTime ExtractDateOfBirth(string dobText)
        {
            DateTime dateOfBirth;
            if (!DateTime.TryParse(dobText, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth)
                && !DateTime.TryParse(dobText, out dateOfBirth))
            {
                dateOfBirth = DateTime.MinValue;
            }

            return dateOfBirth;
        }

        private static SecureString ExtractPassword(string passwordPlain)
        {
            var securePwd = new SecureString();
            foreach (var c in passwordPlain)
            {
                securePwd.AppendChar(c);
            }
            securePwd.MakeReadOnly();
            return securePwd;
        }

        public async Task<IUser?> GetAuthenticatedUserAsync(string username, string password)
        {
            var allUsers = await GetAllUsersAsync();
            var user = allUsers.FirstOrDefault(u => u.Username.Equals(username, StringComparison.Ordinal));

            if (user != null && AreSecureStringsEqual(user.Password, ExtractPassword(password)))
            {
                return user;
            }

            return null;
        }

        private static bool AreSecureStringsEqual(SecureString password, SecureString securedInputPassword)
        {
            if (password == null || securedInputPassword == null)
            {
                return false;
            }

            if (password.Length != securedInputPassword.Length)
            {
                return false;
            }

            var bstr1 = IntPtr.Zero;
            var bstr2 = IntPtr.Zero;

            try
            {
                bstr1 = Marshal.SecureStringToBSTR(password);
                bstr2 = Marshal.SecureStringToBSTR(securedInputPassword);

                for (int i = 0; i < password.Length; i++)
                {
                    char char1 = (char)Marshal.ReadInt16(bstr1, i * 2);
                    char char2 = (char)Marshal.ReadInt16(bstr2, i * 2);

                    if (char1 != char2)
                    {
                        return false;
                    }
                }

                return true;
            }
            finally
            {
                if (bstr1 != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(bstr1);
                }

                if (bstr2 != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(bstr2);
                }
            }
        }

        public async Task<IUser> GetUserDetailsAsync(Guid userId)
        {
            var allUsers = await GetAllUsersAsync();
            return allUsers.FirstOrDefault(u => u.Id == userId) ?? throw new InvalidOperationException($"User with id {userId} not found.");
        }

        public async Task<IUser> UpdateUserAsync(IUser userToUpdate)
        {
            ArgumentNullException.ThrowIfNull(userToUpdate);

            if (!File.Exists(_dataFilePath))
            {
                throw new InvalidOperationException("Data file does not exist.");
            }

            var lines = await File.ReadAllLinesAsync(_dataFilePath);
            var updated = false;

            var output = new List<string>(lines.Length)
            {
                HEADER_LINE
            };

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var parts = line.Split(';');
                if (parts.Length < EXPECTED_NUMBER_OF_RECORDS)
                {
                    continue;
                }

                if (!Guid.TryParse(parts[0], out var id))
                {
                    continue;
                }

                if (id == userToUpdate.Id)
                {
                    output.Add(BuildLine(userToUpdate));
                    updated = true;
                }
                else
                {
                    output.Add(line);
                }
            }

            if (!updated)
            {
                throw new InvalidOperationException($"User with id {userToUpdate.Id} not found.");
            }

            var tempFile = Path.GetTempFileName();
            await File.WriteAllLinesAsync(tempFile, output).ConfigureAwait(false);

            File.Copy(tempFile, GetAbsolutePath(_dataFilePath), overwrite: true);
            File.Delete(tempFile);

            return userToUpdate;
        }

        public static string GetAbsolutePath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentException("Path cannot be null or empty.", nameof(relativePath));

            return Path.GetFullPath(relativePath);
        }


        private static string BuildLine(IUser user)
        {
            return string.Join(";",
                user.Id,
                user.Username,
                ConvertToPlainText(user.Password),
                user.LastName,
                user.FirstName,
                user.DateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                user.BirthPlace,
                user.Residence
            );
        }

        private static string ConvertToPlainText(SecureString secureString)
        {
            if (secureString == null)
            {
                return string.Empty;
            }

            IntPtr bstr = IntPtr.Zero;
            try
            {
                bstr = Marshal.SecureStringToBSTR(secureString);
                return Marshal.PtrToStringBSTR(bstr) ?? string.Empty;
            }
            finally
            {
                if (bstr != IntPtr.Zero)
                {
                    Marshal.ZeroFreeBSTR(bstr);
                }
            }
        }
    }
}

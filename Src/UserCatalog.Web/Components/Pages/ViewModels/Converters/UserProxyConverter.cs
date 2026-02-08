using SHD.UserCatalog.BL;
using System.Security;
using System.Text.Json;

namespace UserCatalog.Web.Components.Pages.ViewModels.Converters
{
    public class UserProxyConverter
    {
        public IEnumerable<IUser> ConvertUserListToUserProxy(string json)
        {
            var doc = JsonDocument.Parse(json);
            var list = new List<IUser>();

            foreach (var element in doc.RootElement.EnumerateArray())
            {
                list.Add(ConvertUserToUserProxy(element.ToString()));
            }

            return list;
        }

        public IUser ConvertUserToUserProxy(string json)
        {
            using var doc = JsonDocument.Parse(json);
            var element = doc.RootElement;

            var user = new UserProxy()
            {
                Id = element.GetProperty("id").GetGuid(),
                Username = element.GetProperty("username").GetString()!,
                FirstName = element.GetProperty("firstName").GetString()!,
                LastName = element.GetProperty("lastName").GetString()!,
                DateOfBirth = element.GetProperty("dateOfBirth").GetDateTime(),
                BirthPlace = element.GetProperty("birthPlace").GetString()!,
                Residence = element.GetProperty("residence").GetString()!,
                Password = new SecureString()
            };

            return user;
        }
    }
}

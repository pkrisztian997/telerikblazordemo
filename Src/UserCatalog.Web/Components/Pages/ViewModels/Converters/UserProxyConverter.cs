using SHD.UserCatalog.BL;
using System.Security;
using System.Text.Json;

namespace UserCatalog.Web.Components.Pages.ViewModels.Converters
{
    public class UserProxyConverter : IUserProxyConverter
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

            var user = new UserProxy(
                element.GetProperty("id").GetGuid(),
                element.GetProperty("username").GetString()!,
                element.GetProperty("firstName").GetString()!,
                element.GetProperty("lastName").GetString()!,
                element.GetProperty("dateOfBirth").GetDateTime(),
                element.GetProperty("birthPlace").GetString()!,
                element.GetProperty("residence").GetString()!,
                new SecureString());

            return user;
        }

        public IUser ConvertUserDetailFormModelToDomainModel(UserDetailFormModel userDetailFormModel)
        {
            var user = new UserProxy(
                userDetailFormModel.Id,
                userDetailFormModel.Username,
                userDetailFormModel.FirstName,
                userDetailFormModel.LastName,
                userDetailFormModel.DateOfBirth,
                userDetailFormModel.BirthPlace,
                userDetailFormModel.Residence,
                userDetailFormModel.NewPassword
                );

            return user;
        }
    }
}

using SHD.UserCatalog.BL;

namespace UserCatalog.Web.Components.Pages.ViewModels.Converters
{
    public interface IUserProxyConverter
    {
        public IEnumerable<IUser> ConvertUserListToUserProxy(string json);
        public IUser ConvertUserToUserProxy(string json);
        public IUser ConvertUserDetailFormModelToDomainModel(UserDetailFormModel userDetailFormModel);
    }
}

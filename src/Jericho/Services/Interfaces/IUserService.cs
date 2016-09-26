namespace Jericho.Services.Interfaces
{
    using System.Threading.Tasks;

    using Jericho.Models.v1.Entities;

    public interface IUserService
    {
        Task CreateUserAsync(UserEntity user);

        bool LoginUserAsync(UserEntity user);

        bool IsUserNameExists(string userName);

        bool IsEmailAddressExists(string email);
    }
}
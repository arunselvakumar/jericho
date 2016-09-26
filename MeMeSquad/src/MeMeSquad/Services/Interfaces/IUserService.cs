namespace MeMeSquad.Services.Interfaces
{
    using System.Threading.Tasks;

    using MeMeSquad.Models.v1.Entities;

    public interface IUserService
    {
        Task CreateUserAsync(UserEntity user);

        bool LoginUserAsync(UserEntity user);

        bool IsUserNameExists(string userName);

        bool IsEmailAddressExists(string email);
    }
}
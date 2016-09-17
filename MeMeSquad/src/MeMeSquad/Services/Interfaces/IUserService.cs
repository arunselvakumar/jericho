namespace MeMeSquad.Services.Interfaces
{
    using System.Threading.Tasks;

    using MeMeSquad.Models.Entities;

    public interface IUserService
    {
        Task CreateUserAsync(UserEntity user);

        Task LoginUserAsync(UserEntity user);

        Task<bool> IsUserNameExistsAsync(string userName);

        Task<bool> IsEmailAddressExistsAsync(string email);
    }
}
namespace Jericho.Services.Interfaces
{
    using System.Threading.Tasks;

    using Jericho.Identity;
    using Jericho.Models.v1.DTOs.User;
    using Jericho.Models.v1.Entities;

    public interface IUserService
    {
        Task<string> SaveUserAsync(SaveApplicationUserDto applicationUser);

        Task<bool> UpdateUserAsync(SaveApplicationUserDto applicationUser);

        Task<ApplicationUser> GetUserById(string id);

        Task<ApplicationUser> GetUserByUserName(string username);

        Task<string> LoginUserAsync(AuthUserRequestDto user);
    }
}
namespace Jericho.Services.Interfaces
{
    using System.Threading.Tasks;

    using Jericho.Identity;
    using Jericho.Models.v1.DTOs.User;
    using Jericho.Models.v1.Entities;
    using Microsoft.Extensions.Configuration;
    using Models.v1;

    public interface IUserService
    { 
        Task<AuthTokenModel> SaveUserAsync(SaveApplicationUserDto applicationUser);

        Task<bool> UpdateUserAsync(SaveApplicationUserDto applicationUser);

        Task<ApplicationUser> GetUserById(string id);

        Task<ApplicationUser> GetUserByUserName(string username);

        Task<AuthTokenModel> LoginUserAsync(AuthUserRequestDto user);
    }
}
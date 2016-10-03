namespace Jericho.Services.Interfaces
{
    using System.Threading.Tasks;

    using Jericho.Identity;
    using Jericho.Models.v1.DTOs.User;
    using Jericho.Models.v1.Entities;
    using Microsoft.Extensions.Configuration;
    using Models.v1;
    using Providers;

    public interface IUserService
    { 
        Task<ServiceResult<AuthTokenModel>> SaveUserAsync(SaveApplicationUserDto applicationUser);

        Task<bool> UpdateUserAsync(SaveApplicationUserDto applicationUser);

        Task<ServiceResult<ApplicationUser>> GetUserById(string id);

        Task<ServiceResult<ApplicationUser>> GetUserByUserName(string username);

        Task<ServiceResult<AuthTokenModel>> LoginUserAsync(AuthUserRequestDto user);
    }
}
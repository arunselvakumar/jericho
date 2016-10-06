namespace Jericho.Services.Interfaces
{
    using System.Threading.Tasks;

    using Jericho.Identity;
    using Jericho.Models.v1.DTOs.User;
    using Models.v1;
    using Providers;

    public interface IUserService
    { 
        Task<ServiceResult<AuthTokenModel>> SaveUserAsync(ApplicationUser user, string password);

        Task<ServiceResult<AuthTokenModel>> AuthorizeUserAsync(string username, string password);

        Task<ServiceResult<object>> ConfirmEmailAsync(string id, string token);

        Task<ServiceResult<ApplicationUser>> GetUserByIdAsync(string id);

        Task<ServiceResult<ApplicationUser>> GetUserByUserNameAsync(string username);

        Task<ServiceResult<object>> ChangePasswordAsync(string userId, string oldPassword, string newPassword);

        Task<ServiceResult<object>> ChangeEmailAddressAsync(string newEmailAddress);

        Task<bool> UpdateUserAsync(SaveUserRequestDto applicationUser);

    }
}
namespace Jericho.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Identity;
    using Jericho.Models.v1.DTOs.User;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using Options;
    using Microsoft.Extensions.Options;
    using Models.v1;
    using Providers;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Http;

    public class UserService : IUserService
    {
        #region Fields

        private const string Issuer = "Jericho";

        private readonly IMapper mapper;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly AuthenticationOptions authenticationOptions;

        private readonly IHttpContextAccessor httpContextAccessor;

        #endregion

        #region Constructor

        public UserService(
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IOptions<AuthenticationOptions> authenticationOptions, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager)
        {
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            this.authenticationOptions = authenticationOptions.Value;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        #endregion

        public async Task<ServiceResult<AuthTokenModel>> SaveUserAsync(ApplicationUser user, string password)
        {
            var saveUserResult = await this.userManager.CreateAsync(user, password);

            if (!saveUserResult.Succeeded)
            {
                return new ServiceResult<AuthTokenModel>(false, null, saveUserResult.Errors);
            }

            return new ServiceResult<AuthTokenModel>(true, await this.GenerateJwtSecurityToken(user.UserName));
        }

        public async Task<ServiceResult<AuthTokenModel>> AuthorizeUserAsync(string username, string password)
        {
            var loginUserResult = await this.signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

            if (!loginUserResult.Succeeded)
            {
                return new ServiceResult<AuthTokenModel>(false, null, "Invalid Username or Password");
            }

            return new ServiceResult<AuthTokenModel>(true, await this.GenerateJwtSecurityToken(username));
        }

        public async Task<ServiceResult<ApplicationUser>> GetUserByIdAsync(string id)
        {
            var applicationUser = await this.FindByIdAsync(id);
            if (applicationUser == null)
            {
                return new ServiceResult<ApplicationUser>(false, null);
            }

            return new ServiceResult<ApplicationUser>(true, applicationUser);
        }

        public async Task<ServiceResult<ApplicationUser>> GetUserByUserNameAsync(string username)
        {
            var applicationUser = await this.FindByNameAsync(username);
            if (applicationUser == null)
            {
                return new ServiceResult<ApplicationUser>(false, null);
            }

            return new ServiceResult<ApplicationUser>(true, applicationUser);
        }

        public async Task<ServiceResult<object>> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var applicationUser = await this.FindByIdAsync(userId);
            var changePasswordResult = await this.userManager.ChangePasswordAsync(applicationUser, oldPassword, newPassword);

            if (!changePasswordResult.Succeeded)
            {
                return new ServiceResult<object>(false, null, changePasswordResult.Errors);
            }

            return new ServiceResult<object>(true);
        }

        public async Task<ServiceResult<object>> ChangeEmailAddressAsync(string newEmailAddress)
        {
            var userId = this.httpContextAccessor.HttpContext.User.FindFirst(JwtRegisteredClaimNames.Sid).Value;
            var applicationUser = await this.FindByIdAsync(userId);
            var changePasswordResult = await this.userManager.ChangeEmailAsync(applicationUser, newEmailAddress, null);

            if (!changePasswordResult.Succeeded)
            {
                return new ServiceResult<object>(false, null, changePasswordResult.Errors);
            }

            return new ServiceResult<object>(true);
        }

        public async Task<bool> UpdateUserAsync(SaveUserRequestDto user)
        {
            var applicationUser = new ApplicationUser(user.UserName, user.EMail)
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            var updateUserResult = await this.userManager.UpdateAsync(applicationUser);

            return updateUserResult.Succeeded;
        }

        private async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await this.userManager.FindByIdAsync(userId);
        }

        private async Task<ApplicationUser> FindByNameAsync(string username)
        {
            return await this.userManager.FindByNameAsync(username);
        }

        private async Task<bool> IsEmailConfirmedAsync(string userId)
        {
            var user = await this.FindByIdAsync(userId);
            if (user != null)
            {
                return await this.userManager.IsEmailConfirmedAsync(user);
            }

            return false;
        }

        private async Task<string> GenerateResetPasswordToken(string userId)
        {
            var user = await this.FindByIdAsync(userId);
            if (user != null)
            {
                return await this.userManager.GeneratePasswordResetTokenAsync(user);
            }

            return null;
        }

        private async Task<AuthTokenModel> GenerateJwtSecurityToken(string username)
        {
            var loggedInUser = await this.FindByNameAsync(username);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, loggedInUser.UserName),
                new Claim(JwtRegisteredClaimNames.Sid, loggedInUser.Id)
            };

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.authenticationOptions.SecretKey));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(claims: claims, issuer: Issuer, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddDays(7), signingCredentials: signingCredentials);

            return await Task.FromResult(new AuthTokenModel(jwt));
        }
    }
}
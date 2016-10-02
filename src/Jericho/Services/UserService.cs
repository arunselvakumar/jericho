namespace Jericho.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Identity;
    using Jericho.Models.v1.DTOs.User;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using Options;
    using Microsoft.Extensions.Options;
    using Models.v1;

    public class UserService : IUserService
    {
        #region Fields

        private const string Issuer = "Jericho";

        private readonly IMapper mapper;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly AuthenticationOptions authenticationOptions;

        #endregion

        #region Constructor

        public UserService(IOptions<AuthenticationOptions> authenticationOptions, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            this.mapper = mapper;

            this.authenticationOptions = authenticationOptions.Value;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        #endregion

        public async Task<AuthTokenModel> SaveUserAsync(SaveApplicationUserDto user)
        {
            var applicationUser = new ApplicationUser(user.UserName, user.EMail)
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            var saveUserResult = await this.userManager.CreateAsync(applicationUser, user.Password);

            if (!saveUserResult.Succeeded)
            {
                return null;
            }

            return await this.GenerateToken();
        }

        public async Task<AuthTokenModel> LoginUserAsync(AuthUserRequestDto user)
        {
            var isLoginSucceeded = await this.signInManager.PasswordSignInAsync(user.UserName, user.Password, isPersistent: false, lockoutOnFailure: false);

            if (!isLoginSucceeded.Succeeded)
            {
                return null;
            }

            return await this.GenerateToken();
        }

        public async Task<bool> UpdateUserAsync(SaveApplicationUserDto user)
        {
            var applicationUser = new ApplicationUser(user.UserName, user.EMail)
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };

            var updateUserResult = await this.userManager.UpdateAsync(applicationUser);

            return updateUserResult.Succeeded;
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await this.userManager.FindByIdAsync(id);
        }

        public async Task<ApplicationUser> GetUserByUserName(string username)
        {
            return await this.userManager.FindByNameAsync(username);
        }

        private async Task<AuthTokenModel> GenerateToken()
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.authenticationOptions.SecretKey));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(issuer: Issuer, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddDays(7), signingCredentials: signingCredentials);

            return await Task.FromResult(new AuthTokenModel(jwt));
        }
    }
}
namespace Jericho.Services
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Identity;
    using Jericho.Models.v1.DTOs.User;
    using Jericho.Models.v1.Entities;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Identity;

    public class UserService : IUserService
    {
        #region Fields

        private const string Issuer = "Jericho";

        private readonly IMapper mapper;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        #endregion

        #region Constructor

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            this.mapper = mapper;

            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        #endregion

        public async Task<string> SaveUserAsync(SaveUserRequestDto user)
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

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToLongDateString(), ClaimValueTypes.Integer64)
            };

            var jwt = new JwtSecurityToken(claims: claims, issuer: Issuer, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddDays(7));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public async Task<string> LoginUserAsync(LoginUserRequestDto user)
        {
            var isLoginSucceeded = await this.signInManager.PasswordSignInAsync(user.UserName, user.Password, isPersistent: false, lockoutOnFailure: false);

            if (isLoginSucceeded.Succeeded)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToLongDateString(), ClaimValueTypes.Integer64)
                };

                var jwt = new JwtSecurityToken(claims: claims, issuer: Issuer, notBefore: DateTime.UtcNow, expires: DateTime.UtcNow.AddDays(7));

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                return encodedJwt;
            }

            return null;
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

        public async Task<ApplicationUser> GetUserById(string id)
        {
            return await this.userManager.FindByIdAsync(id);
        }

        public async Task<ApplicationUser> GetUserByUserName(string username)
        {
            return await this.userManager.FindByNameAsync(username);
        }
    }
}
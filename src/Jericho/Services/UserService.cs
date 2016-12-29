namespace Jericho.Services
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Common;
    using Jericho.Identity;
    using Jericho.Providers.ServiceResultProvider;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Tokens;

    using Models.v1;

    using Options;

    public class UserService : IUserService
    {
        private const string Issuer = "Jericho";

        private readonly IMapper mapper;

        private readonly IEmailService emailService;

        private readonly UserManager<ApplicationUser> userManager;

        private readonly SignInManager<ApplicationUser> signInManager;

        private readonly AuthenticationOptions authenticationOptions;

        public UserService(
            IMapper mapper,
            IEmailService emailService,
            IOptions<AuthenticationOptions> authenticationOptions, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.mapper = mapper;
            this.emailService = emailService;
            this.authenticationOptions = authenticationOptions.Value;
            this.userManager = userManager;
            this.signInManager = signInManager;

            this.ApplyUserManagerPresets();
        }

        public async Task<ServiceResult<AuthTokenModel>> SaveUserAsync(ApplicationUser user, string password)
        {
            Ensure.NotNull(user);
            Ensure.NotNullOrEmpty(password);

            var saveUserResult = await this.userManager.CreateAsync(user, password);

            if (!saveUserResult.Succeeded)
            {
                var errors = this.mapper.Map<IEnumerable<Error>>(saveUserResult.Errors);
                return new ServiceResult<AuthTokenModel>(false, errors);
            }

            this.SendConfirmationEmail(await this.FindUserByNameAsync(user.UserName));

            return new ServiceResult<AuthTokenModel>(true, await this.GenerateJwtSecurityToken(user.UserName));
        }

        public async Task<ServiceResult<AuthTokenModel>> AuthorizeUserAsync(string username, string password)
        {
            Ensure.NotNullOrEmpty(username);
            Ensure.NotNullOrEmpty(password);

            var loginUserResult = await this.signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

            if (!loginUserResult.Succeeded)
            {
                var error = new Error("unauthorized", "Invalid Username or Password");
                return new ServiceResult<AuthTokenModel>(false, new List<Error> { error });
            }

            return new ServiceResult<AuthTokenModel>(true, await this.GenerateJwtSecurityToken(username));
        }

        public async Task<ServiceResult<object>> ActivateEmailAsync(string id, string token)
        {
            Ensure.NotNullOrEmpty(id);
            Ensure.NotNullOrEmpty(token);

            var user = await this.FindUserByIdAsync(id);
            var confirmEmailResult = await this.userManager.ConfirmEmailAsync(user, token);

            if (!confirmEmailResult.Succeeded)
            {
                var errors = this.mapper.Map<IEnumerable<Error>>(confirmEmailResult.Errors);
                return new ServiceResult<object>(false, errors);
            }

            return new ServiceResult<object>(true);
        }

        public async Task<ServiceResult<ApplicationUser>> GetUserByIdAsync(string id)
        {
            Ensure.NotNullOrEmpty(id);

            var applicationUser = await this.FindUserByIdAsync(id);
            if (applicationUser == null)
            {
                var error = new Error("notfound", "User Not Found");
                return new ServiceResult<ApplicationUser>(false, new List<Error> { error });
            }

            return new ServiceResult<ApplicationUser>(true, applicationUser);
        }

        public async Task<ServiceResult<ApplicationUser>> GetUserByUserNameAsync(string username)
        {
            Ensure.NotNullOrEmpty(username);

            var applicationUser = await this.FindUserByNameAsync(username);
            if (applicationUser == null)
            {
                var error = new Error("notfound", "User Not Found");
                return new ServiceResult<ApplicationUser>(false, new List<Error> { error });
            }

            return new ServiceResult<ApplicationUser>(true, applicationUser);
        }

        public async Task<ServiceResult<object>> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            Ensure.NotNullOrEmpty(userId);
            Ensure.NotNullOrEmpty(oldPassword);
            Ensure.NotNullOrEmpty(newPassword);

            var applicationUser = await this.FindUserByIdAsync(userId);
            var changePasswordResult = await this.userManager.ChangePasswordAsync(applicationUser, oldPassword, newPassword);

            if (!changePasswordResult.Succeeded)
            {
                var errors = this.mapper.Map<IEnumerable<Error>>(changePasswordResult.Errors);
                return new ServiceResult<object>(false, errors);
            }

            return new ServiceResult<object>(true);
        }

        public async Task<ServiceResult<object>> ResetPasswordAsync(string username)
        {
            Ensure.NotNullOrEmpty(username);

            this.SendPasswordResetTokenEmail(username);
            
            return new ServiceResult<object>(true);
        }

        public async Task<ServiceResult<object>> ChangeEmailAddressAsync(string newEmailAddress)
        {
            Ensure.NotNullOrEmpty(newEmailAddress);

            var applicationUser = await this.FindUserByIdAsync(string.Empty);
            var changePasswordResult = await this.userManager.ChangeEmailAsync(applicationUser, newEmailAddress, null);

            if (!changePasswordResult.Succeeded)
            {
                var errors = this.mapper.Map<IEnumerable<Error>>(changePasswordResult.Errors);
                return new ServiceResult<object>(false, errors);
            }

            return new ServiceResult<object>(true);
        }

        public async Task<ServiceResult<object>> ForgotPasswordAsync(string username)
        {
            Ensure.NotNullOrEmpty(username);

            var applicationUser = await this.FindUserByNameAsync(username);
            if (applicationUser != null)
            {
                this.SendResetPasswordEmail(applicationUser);
                return new ServiceResult<object>(true);
            }

            return new ServiceResult<object>(false);
        }

        public async Task<ServiceResult<object>> ResetPasswordAsync(string token, string username, string password)
        {
            Ensure.NotNullOrEmpty(token);
            Ensure.NotNullOrEmpty(username);
            Ensure.NotNullOrEmpty(password);

            var applicationUser = await this.FindUserByNameAsync(username);
            if (applicationUser != null)
            {
                var resetPasswordResult = await this.userManager.ResetPasswordAsync(applicationUser, token, password);
                if (!resetPasswordResult.Succeeded)
                {
                    var errors = this.mapper.Map<IEnumerable<Error>>(resetPasswordResult.Errors);
                    return new ServiceResult<object>(false, errors);
                }

                return new ServiceResult<object>(true);
            }

            return new ServiceResult<object>(false);
        }

        private void ApplyUserManagerPresets()
        {
            this.userManager.RegisterTokenProvider("Default", new EmailTokenProvider<ApplicationUser>());
        }

        private async Task<ApplicationUser> FindUserByIdAsync(string userId)
        {
            return await this.userManager.FindByIdAsync(userId);
        }

        private async Task<ApplicationUser> FindUserByNameAsync(string username)
        {
            var user = await this.userManager.FindByNameAsync(username);
            if (user == null)
            {
                return await this.userManager.FindByEmailAsync(username);    
            }

            return user;
        }

        private async Task<string> GenerateResetPasswordToken(ApplicationUser user)
        {
            return await this.userManager.GeneratePasswordResetTokenAsync(user);
        }

        private async Task<AuthTokenModel> GenerateJwtSecurityToken(string username)
        {
            var loggedInUser = await this.FindUserByNameAsync(username);

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

        private async void SendConfirmationEmail(ApplicationUser user)
        {
            var token = await this.userManager.GenerateEmailConfirmationTokenAsync(user);
            // await this.emailService.SendEmailAsync(user.Email.NormalizedValue, "Activate Account", token);
        }

        private async void SendResetPasswordEmail(ApplicationUser user)
        {
            var token = await this.userManager.GeneratePasswordResetTokenAsync(user);
        }

        private async void SendPasswordResetTokenEmail(string username)
        {
            var user = await this.FindUserByNameAsync(username);
            var token = await this.GenerateResetPasswordToken(user);
            await this.emailService.SendEmailAsync(user.Email.NormalizedValue, "Reset Password Account", token);
        }
    }
}
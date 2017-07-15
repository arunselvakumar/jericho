namespace Jericho.Controllers.APIs.V1
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Threading.Tasks;

    using AutoMapper;

    using Identity;

    using Jericho.Models.v1.DTOs.User;
    using Jericho.Models.V1.DTOs.User.Request;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public sealed class UsersController : Controller
    {
        private readonly IMapper mapper;

        private readonly IUserService userService;

        public UsersController(IMapper mapper, IUserService userService)
        {
            this.mapper = mapper;
            this.userService = userService;
        }

        [HttpPost, AllowAnonymous]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> SaveUserAsync([FromBody] SaveUserRequestDto saveUser)
        {
            var user = this.mapper.Map<ApplicationUser>(saveUser);
            var serviceResult = await this.userService.SaveUserAsync(user, saveUser.Password);

            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new OkObjectResult(serviceResult.Value);
        }

        [HttpPost, AllowAnonymous]
        [Route("api/v1/[controller]/authorize")]
        public async Task<IActionResult> AuthorizeUserAsync([FromBody] AuthUserRequestDto user)
        {
            var serviceResult = await this.userService.AuthorizeUserAsync(user.UserName, user.Password);

            if (!serviceResult.Succeeded)
            {
                return new StatusCodeResult(401);
            }

            return new OkObjectResult(serviceResult.Value);
        }

        [HttpGet]
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/v1/[controller]/activate/{token}")]
        public async Task<IActionResult> ActivateEmailAsync([FromRoute] string token)
        {
            var userId = this.User.FindFirst(JwtRegisteredClaimNames.Sid).Value;
            var serviceResult = await this.userService.ActivateEmailAsync(userId, token);

            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new StatusCodeResult(204);
        }

        [HttpGet, AllowAnonymous]
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> GetUserAsync([FromQuery] string id = null, [FromQuery] string username = null)
        {
            var serviceResult = id != null ? await this.userService.GetUserByIdAsync(id) : await this.userService.GetUserByUserNameAsync(username);

            if (!serviceResult.Succeeded)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(this.mapper.Map<GetUserResponseDto>(serviceResult.Value));
        }

        [HttpPatch]
        [Route("api/v1/[controller]/resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto passwordRequestDto)
        {
            var serviceResult = await this.userService.ResetPasswordAsync(passwordRequestDto.Token, passwordRequestDto.Username, passwordRequestDto.Password);

            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new StatusCodeResult(204);
        }

        [HttpPatch]
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/v1/[controller]/changepassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody]ChangePasswordRequestDto password)
        {
            var userId = this.User.FindFirst(JwtRegisteredClaimNames.Sid).Value;
            var serviceResult = await this.userService.ChangePasswordAsync(userId, password.OldPassword, password.NewPassword);

            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new StatusCodeResult(204);
        }

        [HttpGet]
        [Route("api/v1/[controller]/forgotpassword/{username}")]
        public async Task<IActionResult> ForgotPasswordAsync([FromRoute]string username)
        {
            var serviceResult = await this.userService.ForgotPasswordAsync(username);

            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new StatusCodeResult(204);
        }

        [HttpPatch]
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/v1/[controller]/email")]
        public async Task<IActionResult> ChangeEmailAddressAsync([FromBody]string newEmailAddress)
        {
            var serviceResult = await this.userService.ChangeEmailAddressAsync(newEmailAddress);

            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new StatusCodeResult(204);
        }
    }
}
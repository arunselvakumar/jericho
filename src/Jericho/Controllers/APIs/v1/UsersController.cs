namespace Jericho.Controllers.APIs.v1
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Models.v1.DTOs.User;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Identity;
    using System.IdentityModel.Tokens.Jwt;

    public class UsersController : Controller
    {
        private readonly IMapper mapper;

        private readonly IUserService userService;

        public UsersController(IMapper mapper, IUserService userService)
        {
            this.userService = userService;

            this.mapper = mapper;
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
                return new UnauthorizedResult();
            }

            return new OkObjectResult(serviceResult.Value);
        }

        [HttpPost, AllowAnonymous]
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/v1/[controller]/confirmemail")]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] string token)
        {
            var userId = this.User.FindFirst(JwtRegisteredClaimNames.Sid).Value;
            var serviceResult = await this.userService.ConfirmEmailAsync(userId, token);

            if(!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new StatusCodeResult(204);
        }

        [HttpGet, AllowAnonymous]
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
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/v1/[controller]/resetpassword")]
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

        [HttpPatch]
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] SaveUserRequestDto updateApplicationUserDto)
        {
            var userId = this.User.FindFirst(JwtRegisteredClaimNames.Sid).Value;
            var isUpdated = await this.userService.UpdateUserAsync(updateApplicationUserDto);
            return isUpdated ? new StatusCodeResult(204) : new BadRequestResult();
        }
    }
}
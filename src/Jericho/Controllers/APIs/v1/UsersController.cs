namespace Jericho.Controllers.APIs.v1
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Models.v1.DTOs.User;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authentication.JwtBearer;

    public class UsersController : Controller
    {
        #region Fields

        private readonly IMapper mapper;

        private readonly IUserService userService;

        #endregion

        #region Constructor

        public UsersController(IMapper mapper, IUserService userService)
        {
            this.userService = userService;

            this.mapper = mapper;
        }

        #endregion

        [HttpPost, AllowAnonymous]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> SaveUserAsync([FromBody] SaveApplicationUserDto saveApplicationUserDto)
        {
            var serviceResult = await this.userService.SaveUserAsync(saveApplicationUserDto);

            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new OkObjectResult(serviceResult.Value);
        }

        [HttpGet, AllowAnonymous]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> GetUserAsync([FromQuery] string id = null, [FromQuery] string username = null)
        {
            var serviceResult = id != null ? await this.userService.GetUserById(id) : await this.userService.GetUserByUserName(username);

            if (!serviceResult.Succeeded)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(this.mapper.Map<UserDto>(serviceResult.Value));
        }

        [HttpPatch]
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/v1/[controller]/password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody]ChangePasswordRequestDto changePasswordRequest)
        {
            var serviceResult = await this.userService.ChangePasswordAsync(changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);

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
        public async Task<IActionResult> UpdateUserAsync([FromBody] SaveApplicationUserDto updateApplicationUserDto)
        {
            var isUpdated = await this.userService.UpdateUserAsync(updateApplicationUserDto);
            return isUpdated ? new StatusCodeResult(204) : new BadRequestResult();
        }

        [HttpPost, AllowAnonymous]
        [Route("api/v1/[controller]/authorize")]
        public async Task<IActionResult> AuthorizeUserAsync([FromBody] AuthUserRequestDto authUserRequestDto)
        {
            var serviceResult = await this.userService.LoginUserAsync(authUserRequestDto);

            if (!serviceResult.Succeeded)
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(serviceResult.Value);
        }
    }
}
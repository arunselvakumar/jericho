namespace Jericho.Controllers.APIs.v1
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Models.v1.DTOs.User;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        #region Fields

        private readonly IMapper mapper;

        private readonly IUserService userService;

        #endregion

        #region Constructor

        public UsersController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;

            this.mapper = mapper;
        }

        #endregion

        [HttpPost, AllowAnonymous]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> SaveUserAsync([FromBody] SaveApplicationUserDto saveApplicationUserDto)
        {
            var jwtToken = await this.userService.SaveUserAsync(saveApplicationUserDto);

            if (jwtToken != null)
            {
                return new CreatedResult(string.Empty, jwtToken);
            }

            return new BadRequestResult();
        }

        [HttpGet]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> GetUserAsync([FromQuery] string id = null, [FromQuery] string username = null)
        {
            var applicationUser = id != null ? await this.userService.GetUserById(id) : await this.userService.GetUserByUserName(username);

            if (applicationUser == null)
            {
                return new NotFoundResult();
            }

            var user = this.mapper.Map<UserDto>(applicationUser);
            return new OkObjectResult(user);
        }

        [HttpPatch]
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
            var user = await this.userService.LoginUserAsync(authUserRequestDto);

            if (string.IsNullOrEmpty(user))
            {
                return new UnauthorizedResult();
            }

            return new OkObjectResult(user);
        }
    }
}
 // ReSharper disable once StyleCop.SA1300
namespace MeMeSquad.Controllers.APIs.v1
{
    using System.Threading.Tasks;

    using AutoMapper;

    using MeMeSquad.Models.DTOs;
    using MeMeSquad.Models.DTOs.User;
    using MeMeSquad.Models.Entities;
    using MeMeSquad.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/v1/[controller]")]
    public class UsersController : Controller
    {
        #region Fields

        private readonly IUserService userService;

        private readonly IMapper mapper;

        #endregion

        #region Constructor

        public UsersController(IUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        #endregion

        #region Public Method

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto userDto)
        {
            if (!this.ModelState.IsValid)
            {
                return new BadRequestObjectResult(this.ModelState.Values);
            }

            var userEntity = this.mapper.Map<UserEntity>(userDto);
            await this.userService.CreateUserAsync(userEntity);

            return new CreatedResult(string.Empty, null);
        }

        [HttpPatch]
        public async Task<IActionResult> LoginUserAsync([FromBody] UserDto userDto)
        {
            if (!this.ModelState.IsValid)
            {
                return new BadRequestObjectResult(this.ModelState.Values);
            }

            var userEntity = this.mapper.Map<UserEntity>(userDto);
            await this.userService.CreateUserAsync(userEntity);

            return new CreatedResult(string.Empty, null);
        }
        #endregion
    }
}
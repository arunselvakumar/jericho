 // ReSharper disable once StyleCop.SA1300
namespace MeMeSquad.Controllers.APIs.v1
{
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using MeMeSquad.Models.DTOs;
    using MeMeSquad.Models.DTOs.User;
    using MeMeSquad.Models.Entities;
    using MeMeSquad.Services.Interfaces;
    using MeMeSquad.Validations.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    public class UsersController : Controller
    {
        #region Fields

        private readonly IMapper mapper;

        private readonly IUserService userService;

        private ICreateUserValidationService createUserValidationService;

        #endregion

        #region Constructor

        public UsersController(IUserService userService, ICreateUserValidationService createUserValidationService, IMapper mapper)
        {
            this.userService = userService;
            this.createUserValidationService = createUserValidationService;
            this.mapper = mapper;
        }

        #endregion

        #region Public Method

        [HttpPost]
        [Route("api/v1/[controller]/createuser")]
        public async Task<IActionResult> SaveUserAsync([FromBody] SaveUserRequestDto saveUserRequestDto)
        {
            if (!this.ModelState.IsValid)
            {
                return new BadRequestObjectResult(this.ModelState.Values);
            }

            var createUserValidationErrors = this.createUserValidationService.Validate(saveUserRequestDto);
            if (createUserValidationErrors.Any())
            {
                return new BadRequestObjectResult(createUserValidationErrors);
            }

            var userModel = this.mapper.Map<UserEntity>(saveUserRequestDto);
            await this.userService.CreateUserAsync(userModel);

            return new CreatedResult(string.Empty, null);
        }

        [HttpPost]
        [Route("api/v1/[controller]/loginuser")]
        public IActionResult LoginUserAsync([FromBody] LoginUserRequestDto loginUserRequestDto)
        {
            if (!this.ModelState.IsValid)
            {
                return new BadRequestObjectResult(this.ModelState.Values);
            }

            var userModel = this.mapper.Map<UserEntity>(loginUserRequestDto);

            var isLoginSuccessful = this.userService.LoginUserAsync(userModel);

            if (isLoginSuccessful)
            {
                return new OkResult();
            }

            return new BadRequestResult();
        }
        #endregion
    }
}
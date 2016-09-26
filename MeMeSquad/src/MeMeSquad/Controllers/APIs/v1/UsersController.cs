 // ReSharper disable once StyleCop.SA1300

using MeMeSquad.Identity;
using Microsoft.AspNetCore.Identity;

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

        private readonly UserManager<MongoIdentityUser> userManager;

        private readonly SignInManager<MongoIdentityUser> signInManager;

        private ICreateUserValidationService createUserValidationService;

        #endregion

        #region Constructor

        public UsersController(
            UserManager<MongoIdentityUser> userManager,
            SignInManager<MongoIdentityUser> signInManager,
            ICreateUserValidationService createUserValidationService, 
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;

            this.createUserValidationService = createUserValidationService;
            this.mapper = mapper;
        }

        #endregion

        #region Public Method

        [HttpPost]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> SaveUserAsync([FromBody] SaveUserRequestDto saveUserRequestDto)
        {
            var userModel = this.mapper.Map<UserEntity>(saveUserRequestDto);
            var user = new MongoIdentityUser(userModel.UserName, userModel.EMail);
            var result = await this.userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
            {
                return new OkResult();
            }
            else
            {
                return new BadRequestObjectResult(null);
            }
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

            return new BadRequestResult();
        }
        #endregion
    }
}
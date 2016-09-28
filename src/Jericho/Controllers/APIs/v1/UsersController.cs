namespace Jericho.Controllers.APIs.v1
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Identity;
    using Jericho.Models.v1.DTOs.User;
    using Jericho.Models.v1.Entities;
    using Jericho.Services.Interfaces;
    using Jericho.Validations.Interfaces;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
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

        [HttpPost, AllowAnonymous]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> SaveUserAsync([FromBody] SaveUserRequestDto saveUserRequestDto)
        {
            var user = this.mapper.Map<UserEntity>(saveUserRequestDto);
            var userIdentity = new MongoIdentityUser(user.UserName, user.EMail);
            userIdentity.Age = 26;

            var userManagerResult = await this.userManager.CreateAsync(userIdentity, user.Password);

            if (userManagerResult.Succeeded)
            {
                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToLongDateString(), ClaimValueTypes.Integer64)
                };

                var jwt = new JwtSecurityToken(claims: claims);

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                return new OkObjectResult(encodedJwt);
            }

            return new BadRequestObjectResult(userManagerResult.Errors);
        }

        [HttpGet, Authorize]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> GetUserAsync([FromQuery] string id = null, [FromQuery] string username = null)
        {
            var userIdentity = id != null ? await this.userManager.FindByIdAsync(id) : await this.userManager.FindByNameAsync(username);

            if(userIdentity != null)
            {
                return new OkObjectResult(userIdentity);
            }

            return new NotFoundResult();
        }

        [HttpPatch]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] SaveUserRequestDto updateUserRequestDto)
        {
            var user = this.mapper.Map<UserEntity>(updateUserRequestDto);
            var userIdentity = new MongoIdentityUser(user.UserName, user.EMail);

            var userManagerResult = await this.userManager.UpdateAsync(userIdentity);

            if (userManagerResult.Succeeded)
            {
                return new OkResult();
            }

            return new BadRequestObjectResult(userManagerResult.Errors);
        }

        [HttpPost, AllowAnonymous]
        [Route("api/v1/[controller]/authorize")]
        public async Task<IActionResult> AuthorizeUserAsync([FromBody] LoginUserRequestDto loginUserRequestDto)
        {
            var user = this.mapper.Map<UserEntity>(loginUserRequestDto);
            
            var signInManagerResult = await this.signInManager.PasswordSignInAsync(user.UserName, user.Password, isPersistent: false, lockoutOnFailure: false);

            if (signInManagerResult.Succeeded)
            {
                var claims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToLongDateString(), ClaimValueTypes.Integer64)
                };

                var jwt = new JwtSecurityToken(claims: claims);

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

                return new OkObjectResult(encodedJwt);
            }

            return new UnauthorizedResult();
        }
    }
}
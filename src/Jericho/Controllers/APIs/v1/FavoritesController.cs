namespace Jericho.Controllers.APIs.V1
{
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Models.v1.DTOs.Favorite;
    using Jericho.Models.v1.Entities;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class FavoritesController : Controller
    {
        private readonly IMapper mapper;

        private readonly IFavoritesService favoritesService;

        public FavoritesController(IMapper mapper, IFavoritesService favoritesService)
        {
            this.mapper = mapper;
            this.favoritesService = favoritesService;
        }

        [HttpGet]
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> GetAllFavoritesDirectoryAsync()
        {
            var userId = this.User.FindFirst(JwtRegisteredClaimNames.Sid).Value;
            var serviceResult = await this.favoritesService.GetAllFavoritesDirectoryAsync(userId);
            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new OkObjectResult(this.mapper.Map<IEnumerable<GetFavoriteResponseDto>>(serviceResult.Value));
        }

        [HttpPost]
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> SaveFavoritesDirectoryAsync([FromBody]SaveFavoriteDirectoryDto favoriteDto)
        {
            var userId = this.User.FindFirst(JwtRegisteredClaimNames.Sid).Value;
            var serviceResult = await this.favoritesService.SaveFavoritesDirectoryAsync(this.mapper.Map<FavoriteEntity>(favoriteDto), userId);
            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new CreatedResult(string.Empty, serviceResult.Value);
        }

        [HttpDelete]
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/v1/[controller]/{id}")]
        public async Task<IActionResult> DeleteFavoritesDirectoryAsync([FromRoute]string id)
        {
            var serviceResult = await this.favoritesService.DeleteFavoritesDirectoryAsync(id);
            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);                
            }

            return new OkResult();
        }

        [HttpGet]
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/v1/[controller]/{id}")]
        public async Task<IActionResult> GetPostsFromFavoritesDirectoryAsync([FromRoute]string id)
        {
            var serviceResult = await this.favoritesService.GetPostsFromFavoritesDirectoryAsync(id);
            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new OkObjectResult(this.mapper.Map<IEnumerable<GetFavoriteResponseDto>>(serviceResult.Value));
        }

        [HttpPost]
        [Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("api/v1/[controller]/{id}")]
        public async Task<IActionResult> AddPostToFavoritesDirectoryAsync([FromRoute]string id, [FromBody] SaveFavoritePostDto favoritePostDto)
        {
            var serviceResult = await this.favoritesService.AddPostToFavoritesDirectoryAsync(id, this.mapper.Map<FavoriteEntity>(favoritePostDto));
            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new CreatedResult(string.Empty, serviceResult.Value);
        }
    }
}
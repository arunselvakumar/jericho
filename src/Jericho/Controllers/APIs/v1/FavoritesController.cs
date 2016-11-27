namespace Jericho.Controllers.APIs.v1
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Models.v1.DTOs.Favorite;
    using Jericho.Models.v1.Entities;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    public class FavoritesController : Controller
    {
        private readonly IMapper mapper;

        private readonly IFavoritesService favoritesService;

        public FavoritesController (IMapper mapper, IFavoritesService favoritesService)
        {
            this.mapper = mapper;
            this.favoritesService = favoritesService;
        }

        [HttpGet]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> GetAllFavoritesDirectoryAsync()
        {
            return new StatusCodeResult(200);
        }

        [HttpPost]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> SaveFavoritesDirectoryAsync([FromBody]SaveFavoriteDirectoryDto favoriteDto)
        {
            var serviceResult = await this.favoritesService.SaveFavoritesDirectoryAsync(this.mapper.Map<FavoriteEntity>(favoriteDto));
            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new CreatedResult(string.Empty, serviceResult.Value);
        }

        [HttpDelete]
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
        [Route("api/v1/[controller]/{id}")]
        public async Task<IActionResult> GetPostsFromFavoritesDirectoryAsync()
        {
            return new StatusCodeResult(200);
        }

        [HttpPost]
        [Route("api/v1/[controller]/{id}")]
        public async Task<IActionResult> AddPostToFavoritesDirectoryAsync()
        {
            return new StatusCodeResult(200);
        }

        [HttpDelete]
        [Route("api/v1/[controller]/{id}/{id2}")]
        public async Task<IActionResult> DeletePostFromFavoritesDirectoryAsync()
        {
            return new StatusCodeResult(200);
        }
    }
}
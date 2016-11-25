namespace Jericho.Controllers.APIs.v1
{
    using System.Threading.Tasks;

    using AutoMapper;

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
        public async Task<IActionResult> SaveFavoritesDirectoryAsync()
        {
            return new StatusCodeResult(200);
        }

        [HttpDelete]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> DeleteFavoritesDirectoryAsync()
        {
            return new StatusCodeResult(200);
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
        [Route("api/v1/[controller]/{id}")]
        public async Task<IActionResult> DeletePostFromFavoritesDirectoryAsync()
        {
            return new StatusCodeResult(200);
        }
    }
}
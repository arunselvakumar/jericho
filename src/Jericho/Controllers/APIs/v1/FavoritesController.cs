namespace Jericho.Controllers.APIs.v1
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

    public class FavoritesController : Controller
    {
        private readonly IMapper mapper;

        public FavoritesController (IMapper mapper)
        {
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> GetAllFavoritesDirectoryAsync()
        {
            return null;
        }

        [HttpPost]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> SaveFavoritesDirectoryAsync()
        {
            return null;
        }

        [HttpDelete]
        [Route("api/v1/[controller]")]
        public async Task<IActionResult> DeleteavoritesDirectoryAsync()
        {
            return null;
        }

        [HttpGet]
        [Route("api/v1/[controller]/{id}")]
        public async Task<IActionResult> GetPostsFromFavoritesDirectoryAsync()
        {
            return null;
        }

        [HttpPost]
        [Route("api/v1/[controller]/{id}")]
        public async Task<IActionResult> AddPostToFavoritesDirectoryAsync()
        {
            return null;
        }

        [HttpDelete]
        [Route("api/v1/[controller]/{id}")]
        public async Task<IActionResult> DeletePostFromFavoritesDirectoryAsync()
        {

        }
    }
}
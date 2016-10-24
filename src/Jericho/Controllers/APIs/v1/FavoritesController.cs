namespace Jericho.Controllers.APIs.v1
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;    
    using System.Threading.Tasks;

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
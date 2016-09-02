namespace MeMeSquad.Controllers.APIs.v1
{
    using System.Threading.Tasks;
    using AutoMapper;
    using MeMeSquad.Models.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using MeMeSquad.Services.Interfaces;
    using MeMeSquad.Models.Entities;

    [Route("api/v1/[controller]")]
    public class PostsController : Controller
    {
        #region Fields
        private readonly IPostService postService;
        private readonly IMapper mapper;
        #endregion

        #region Constructor

        public PostsController([FromServices]IPostService postService, IMapper mapper)
        {
            this.postService = postService;
            this.mapper = mapper;
        }
        #endregion

        #region Public Methods

        [HttpPost]
        public async Task<IActionResult> SavePostAsync([FromBody]PostDto postDto)
        {
            if(!ModelState.IsValid)
            {
                return new BadRequestObjectResult(this.ModelState.Values);
            }

            var postEntity = this.mapper.Map<PostEntity>(postDto);
            await this.postService.CreatePostAsync(postEntity, null);

            return new CreatedResult(string.Empty, null);
        }

        [HttpGet]
        public async Task<IActionResult> GetPostByIdAsync(string id)
        {
            var postEntity = await this.postService.GetPostAsync(id);

            var contentResult = new ContentResult();
            contentResult.StatusCode = 200;

            return contentResult;
        }
        #endregion
    }
}
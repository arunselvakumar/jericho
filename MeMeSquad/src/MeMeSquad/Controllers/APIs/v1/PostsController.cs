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

        /// <summary>
        /// Initiates a new Post Controller and exposes REST APIs for Posts.
        /// </summary>
        /// <param name="postService"></param>
        /// <param name="mapper"></param>
        public PostsController([FromServices]IPostService postService, IMapper mapper)
        {
            this.postService = postService;
            this.mapper = mapper;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Validates the Model States and Adds new posts to the Data Store.
        /// </summary>
        /// <param name="postDto"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get the post from the Data Store.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetPostByIdAsync(string id)
        {
            var postEntity = await this.postService.GetPostAsync(id);
            var postDto = this.mapper.Map<PostDto>(postEntity);

            var contentResult = new ContentResult
            {
                Content = postDto.ToString(),
                StatusCode = 200
            };

            return contentResult;
        }

        /// <summary>
        /// Updates the post from Data Store.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdatePostByIdAsync(string id)
        {
            var contentResult = new ContentResult
            {
                Content = string.Empty,
                StatusCode = 200
            };

            return contentResult;
        }
        #endregion
    }
}
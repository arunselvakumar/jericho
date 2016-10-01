 // ReSharper disable once StyleCop.SA1300
namespace Jericho.Controllers.APIs.v1
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Models.v1.DTOs;
    using Jericho.Models.v1.Entities;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Posts Controller.
    /// </summary>
    [Route("api/v1/[controller]")]
    public class PostsController : Controller
    {
        #region Fields

        private readonly IPostService postService;

        private readonly IMapper mapper;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PostsController"/> class. 
        /// </summary>
        /// <param name="postService">Post Service </param>
        /// <param name="mapper">Auto Mapper </param>
        public PostsController([FromServices]IPostService postService, IMapper mapper)
        {
            this.postService = postService;
            this.mapper = mapper;
        }
        #endregion

        /// <summary>
        /// Validates the Model States and Adds new posts to the Data Store.
        /// </summary>
        /// <param name="postDto">Post DTO</param>
        /// <returns>Service Response</returns>
        [HttpPost]
        public async Task<IActionResult> SavePostAsync([FromBody]PostDto postDto)
        {
            var postEntity = this.mapper.Map<PostEntity>(postDto);
            var result = await this.postService.CreatePostAsync(postEntity);

            return new CreatedResult(string.Empty, result);
        }

        /// <summary>
        /// Get the post from the Data Store.
        /// </summary>
        /// <param name="postId">Post ID</param>
        /// <returns>Post Document</returns>
        [HttpGet("{postId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetPostByIdAsync(string postId)
        {
            var postEntity = await this.postService.GetPostAsync(postId);
            var postDto = this.mapper.Map<PostDto>(postEntity);

            if(postDto!=null)
            {
                return new OkObjectResult(postDto);
            }

            return new NotFoundObjectResult(null);
        }

        [HttpGet]
        public IActionResult GetAllPosts()
        {
            var postEntities = this.postService.GetAllPosts();
            var postDtos = this.mapper.Map<IList<PostDto>>(postEntities);

            return new OkObjectResult(postDtos);
        }

        /// <summary>
        /// Updates the post from Data Store.
        /// </summary>
        /// <param name="id">Post ID</param>
        /// <returns>Post Document</returns>
        [HttpPatch("{postId}")]
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
    }
}
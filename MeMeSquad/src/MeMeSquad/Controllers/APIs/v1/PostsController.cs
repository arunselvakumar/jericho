using System.Net;
using System.Net.Http;
using MeMeSquad.Models.Entities;

namespace MeMeSquad.Controllers.APIs.v1
{
    using System.Threading.Tasks;
    using AutoMapper;
    using MeMeSquad.Models.DTOs;
    using Microsoft.AspNetCore.Mvc;
    using MeMeSquad.Services.Interfaces;

    [Route("api/v1/[controller]")]
    public class PostsController
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
            var postEntity = this.mapper.Map<PostEntity>(postDto);
            await this.postService.CreatePostAsync(postEntity, null);

            return new CreatedResult(string.Empty, null);
        }
        #endregion
    }
}
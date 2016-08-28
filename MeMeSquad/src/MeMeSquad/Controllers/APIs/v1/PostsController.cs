using System;
using System.Threading.Tasks;
using MeMeSquad.Models.DTOs;

namespace MeMeSquad.Controllers.APIs.v1
{
    using Microsoft.AspNetCore.Mvc;
    using MeMeSquad.Services.Interfaces;

    [Route("api/v1/[controller]")]
    public class PostsController
    {
        #region Fields

        private readonly IPostService postService;
        #endregion

        #region Constructor

        public PostsController([FromServices]IPostService postService)
        {
            this.postService = postService;
        }
        #endregion

        #region Public Methods

        [HttpPost]
        public async Task<string> SavePostAsync([FromBody]PostDto postDto)
        {
            //post.Id = new Guid();
            //await this.postService.CreatePostAsync(post, null);
            return "test 2";
        }
        #endregion
    }
}
using System;
using System.Threading.Tasks;

namespace MeMeSquad.Controllers.APIs.v1
{
    using MeMeSquad.Models;
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

        [HttpGet]
        public async Task<string> SavePostAsync()
        {
            var post = new Post();
            post.Id = new Guid();
            await this.postService.CreatePostAsync(post, null);
            return "test";
        }
        #endregion
    }
}
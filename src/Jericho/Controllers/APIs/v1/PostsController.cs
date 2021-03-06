﻿namespace Jericho.Controllers.APIs.V1
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Models.v1.DTOs;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;

    using Models.v1.BOs;
    using Models.v1.DTOs.Post;

    /// <summary>
    /// Posts Controller.
    /// </summary>
    [Route("api/v1/[controller]")]
    public class PostsController : Controller
    {
        private readonly IPostService postService;
        private readonly IMapper mapper;
        
        public PostsController([FromServices]IPostService postService, IMapper mapper)
        {
            this.postService = postService;
            this.mapper = mapper;
        }
        
        [HttpPost]
        ////[Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SavePostAsync([FromBody]CreatePostDto postDto)
        {          
            var postBo = this.mapper.Map<PostBo>(postDto);           
            var result = await this.postService.CreatePostAsync(postBo);

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(result.Errors);
            }

            var insertedDto = this.mapper.Map<UpdatePostDto>(result.Value);            
            return new CreatedResult(string.Empty, insertedDto);            
        }
        
        [HttpGet("{postId}")]
        [Produces("application/json")]
        public async Task<IActionResult> GetPostByIdAsync(string postId)
        {
            var result = await this.postService.GetPostAsync(postId);

            if (!result.Succeeded)
            {
                return new NotFoundResult();
            }

            var postDto = this.mapper.Map<UpdatePostDto>(result.Value);
            return new OkObjectResult(postDto);
        }

        [HttpGet]
        public async Task<IActionResult> GetPostsAsync([FromQuery]int page = 0, [FromQuery]int limit = 10)
        {           
            var result = await this.postService.GetPostsAsync(this.Request.Query, page, limit);
           
            if (!result.Value.Any())
            {
                return new OkResult();
            }

            var postDtos = this.mapper.Map<IEnumerable<UpdatePostDto>>(result.Value);
            return new OkObjectResult(postDtos);
        }
                
        [HttpPut]
        public async Task<IActionResult> UpdatePostByIdAsync([FromBody]UpdatePostDto postDto)
        {
            var postBo = this.mapper.Map<PostBo>(postDto);
            var result = await this.postService.UpdatePostAsync(postBo);

            if (!result.Succeeded)
            {
                return new NotFoundResult();
            }
            
            return new StatusCodeResult(204);            
        }

        [HttpDelete("{postId}")]
        public async Task<IActionResult> DeletePostByIdAsync(string postId)
        {
            var result = await this.postService.DeletePostAsync(postId);

            if (!result.Succeeded)
            {
                return new NotFoundResult();
            }

            return new StatusCodeResult(204);
        }
    }
}
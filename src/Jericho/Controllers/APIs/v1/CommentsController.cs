namespace Jericho.Controllers.APIs.V1
{
    using System.Threading.Tasks;

    using AutoMapper;

    using Jericho.Models.v1.DTOs;
    using Jericho.Models.v1.Entities;
    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Mvc;
    using Models.v1.BOs;

    /// <summary>
    /// Comments Controller.
    /// </summary>
    [Route("api/v1/[controller]")]
    public class CommentsController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IMapper mapper;
       
        public CommentsController([FromServices]ICommentService commentService, IMapper mapper)
        {
            this.commentService = commentService;
            this.mapper = mapper;
        }
        
        [HttpPost]        
        public async Task<IActionResult> SaveCommentAsync([FromBody]CommentDto commentDto)
        {
            var commentBo = this.mapper.Map<CommentBo>(commentDto);
            var result = await this.commentService.CreateCommentAsync(commentBo);

            if(!result.Succeeded)
            {
                return new BadRequestObjectResult(result.Errors);
            }

            var insertedDto = this.mapper.Map<CommentDto>(result.Value);
            return new CreatedResult(string.Empty, insertedDto);
        }
    }
}
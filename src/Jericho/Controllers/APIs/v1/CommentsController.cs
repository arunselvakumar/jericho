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

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentsController"/> class. 
        /// </summary>
        /// <param name="commentService">Comment Service </param>
        /// <param name="mapper">Auto Mapper </param>
        public CommentsController([FromServices]ICommentService commentService, IMapper mapper)
        {
            this.commentService = commentService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Validates the Model States and Adds new comments to the Data Store.
        /// </summary>
        /// <param name="commentDto">Comment DTO</param>
        /// <returns>Service Response</returns>
        [HttpPost]
        //[Authorize(ActiveAuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SaveCommentAsync([FromBody]CommentDto commentDto)
        {
            var commentBo = this.mapper.Map<CommentBo>(commentDto);
            var result = await this.commentService.CreateCommentAsync(commentBo);

            return new CreatedResult(string.Empty, result);
        }
    }
}
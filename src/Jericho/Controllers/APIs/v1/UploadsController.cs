namespace Jericho.Controllers.APIs.V1
{
    using System.Threading.Tasks;

    using Jericho.Services.Interfaces;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Comments Controller.
    /// </summary>
    [Route("api/v1/[controller]")]
    public class UploadsController : Controller
    {
        private readonly IUploadService uploadService;

        public UploadsController([FromServices]IUploadService uploadService)
        {
            this.uploadService = uploadService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormCollection collection)
        {
            var serviceResult = await this.uploadService.UploadAsync(collection);
            if (!serviceResult.Succeeded)
            {
                return new BadRequestObjectResult(serviceResult.Errors);
            }

            return new CreatedResult(serviceResult.Value, null);
        }
    }
}

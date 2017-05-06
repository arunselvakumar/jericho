namespace Jericho.Services.Interfaces
{
    using System.Threading.Tasks;

    using Jericho.Providers;
    using Microsoft.AspNetCore.Http;

    public interface IUploadService
    {
        Task<ServiceResult<string>> UploadAsync(IFormCollection collection);
    }
}

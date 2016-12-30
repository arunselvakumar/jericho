namespace Jericho.Services.Interfaces
{
    using System.Threading.Tasks;

    using Jericho.Identity;
    using Jericho.Providers.ServiceResultProvider;
    using Microsoft.AspNetCore.Http;

    public interface IUploadService
    {
        Task<ServiceResult<string>> UploadAsync(IFormCollection collection);
    }
}

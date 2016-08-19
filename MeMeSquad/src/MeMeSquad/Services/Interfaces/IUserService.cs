using System.Threading.Tasks;

namespace MeMeSquad.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> IsAuthorized();

        Task<bool> IsAuthorizedToPost();

        Task<bool> IsAuthorizedToDelete();

        Task<bool> IsAuthorizedToModified();
    }
}
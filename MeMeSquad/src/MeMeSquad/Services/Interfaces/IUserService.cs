using System.Threading.Tasks;
using MeMeSquad.Entity;

namespace MeMeSquad.Services.Interfaces
{
    public interface IUserService
    {
        Task CreateUserAsync(User user);

        Task LoginUserAsync(User user);
    }
}
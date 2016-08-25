using System.Threading.Tasks;
using MeMeSquad.Entity;

namespace MeMeSquad.Services
{
    using MeMeSquad.Services.Interfaces;

    public class UserService : IUserService
    {
        public Task CreateUserAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task LoginUserAsync(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
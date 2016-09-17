namespace MeMeSquad.Services
{
    using System.Threading.Tasks;

    using MeMeSquad.Models.Entities;
    using MeMeSquad.Services.Interfaces;

    public class UserService : IUserService
    {
        public Task CreateUserAsync(UserEntity user)
        {
            throw new System.NotImplementedException();
        }

        public Task LoginUserAsync(UserEntity user)
        {
            throw new System.NotImplementedException();
        }
    }
}
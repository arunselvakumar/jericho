namespace MeMeSquad.Services
{
    using System.Threading.Tasks;
    using MeMeSquad.Services.Interfaces;

    public class UserService : IUserService
    {
        public Task<bool> IsAuthorized()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsAuthorizedToPost()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsAuthorizedToDelete()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsAuthorizedToModified()
        {
            throw new System.NotImplementedException();
        }
    }
}
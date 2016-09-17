namespace MeMeSquad.Validations.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MeMeSquad.Models.DTOs.User;

    public interface ICreateUserValidationService
    {
        Task<IEnumerable<string>> Validate(CreateUserDto userDtoToValidate);
    }
}
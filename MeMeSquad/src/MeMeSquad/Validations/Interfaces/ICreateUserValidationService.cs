namespace MeMeSquad.Validations.Interfaces
{
    using System.Collections.Generic;

    using MeMeSquad.Models.v1.DTOs.User;

    public interface ICreateUserValidationService
    {
        IEnumerable<string> Validate(SaveUserRequestDto userRequestDtoToValidate);
    }
}
namespace MeMeSquad.Validations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using MeMeSquad.Models.DTOs.User;
    using MeMeSquad.Services.Interfaces;
    using MeMeSquad.Validations.Interfaces;

    public class CreateUserValidationService : ICreateUserValidationService
    {
        #region Fields

        private readonly IUserService userService;

        #endregion

        #region Constructor

        public CreateUserValidationService(IUserService userService)
        {
            this.userService = userService;
        }

        #endregion

        #region Public Methods

        public async Task<IEnumerable<string>> Validate(CreateUserDto userDtoToValidate)
        {
            var errors = new List<string>();

            var isUserNameValidError = this.IsUserNameValid(userDtoToValidate.UserName);
            var isEmailAddressValidError = this.IsEmailAddressValid(userDtoToValidate.EMail);
            var isUserNameExistsError = this.IsUserNameExistsAsync(userDtoToValidate.UserName);
            var isEmailExistsError = this.IsEmailAddressExistsAsync(userDtoToValidate.EMail);

            await Task.WhenAll(isUserNameExistsError, isEmailExistsError);

            errors.AddRange(isUserNameValidError);
            errors.Add(isEmailAddressValidError);
            errors.Add(isUserNameExistsError.Result);
            errors.Add(isEmailExistsError.Result);

            return errors;
        }

        #endregion

        #region Private Methods

        private IEnumerable<string> IsUserNameValid(string username)
        {
            var errors = new List<string>();
            if (username.Length >= 50)
            {
                errors.Add("Username Limit Exceeded 50 Characters");
            }

            if (username.Any(char.IsWhiteSpace))
            {
                errors.Add("Username should not contain White Spaces");
            }

            return errors;
        }

        private string IsEmailAddressValid(string email)
        {
            var isEmail = Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            return isEmail ? null : "Entered Email Format is Invalid";
        }

        private Task<string> IsUserNameExistsAsync(string userName)
        {
            throw new NotImplementedException();
        }

        private async Task<string> IsEmailAddressExistsAsync(string email)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
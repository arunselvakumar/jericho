namespace Jericho.Validations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Jericho.Models.v1.DTOs.User;
    using Jericho.Services.Interfaces;
    using Jericho.Validations.Interfaces;

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

        public IEnumerable<string> Validate(SaveUserRequestDto userRequestDtoToValidate)
        {
            var errors = new List<string>();

            errors.AddRange(this.IsUserNameValid(userRequestDtoToValidate.UserName));
            errors.Add(this.IsEmailAddressValid(userRequestDtoToValidate.EMail));
            errors.Add(this.IsUserNameExists(userRequestDtoToValidate.UserName));
            errors.Add(this.IsEmailAddressExists(userRequestDtoToValidate.EMail));
            errors.RemoveAll(string.IsNullOrEmpty);

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
            // Regex Snippet copied from http://emailregex.com/
            var isEmail = Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");

            return isEmail ? null : "Entered Email Format is Invalid";
        }

        private string IsUserNameExists(string userName)
        {
            return this.userService.IsUserNameExists(userName) ? "Username already exits" : null;
        }

        private string IsEmailAddressExists(string email)
        {
            return this.userService.IsEmailAddressExists(email) ? "EMail Address already exits" : null;
        }
        #endregion
    }
}
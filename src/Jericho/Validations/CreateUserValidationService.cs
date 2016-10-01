namespace Jericho.Validations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Jericho.Models.v1.DTOs.User;
    using Jericho.Services.Interfaces;
    using Jericho.Validations.Interfaces;

    public class CreateUserValidationServices
    {
        #region Fields

        private readonly IUserService userService;

        #endregion

        #region Constructor

        public CreateUserValidationServices(IUserService userService)
        {
            //this.userService = userService;
        }

        #endregion

        #region Public Methods

        public IEnumerable<string> Validate(SaveApplicationUserDto applicationUserDtoToValidate)
        {
            var errors = new List<string>();

            errors.AddRange(this.IsUserNameValid(applicationUserDtoToValidate.UserName));
            errors.Add(this.IsEmailAddressValid(applicationUserDtoToValidate.EMail));
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
        #endregion
    }
}
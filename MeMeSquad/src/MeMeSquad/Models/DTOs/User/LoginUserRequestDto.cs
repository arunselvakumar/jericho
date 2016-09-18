namespace MeMeSquad.Models.DTOs.User
{
    using System.ComponentModel.DataAnnotations;

    public class LoginUserRequestDto
    {
        #region Public Properties

        [Required, DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        #endregion
    }
}
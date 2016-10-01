namespace Jericho.Models.v1.DTOs.User
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class AuthUserRequestDto
    {
        #region Public Properties

        [Required, DataType(DataType.Text)]
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        #endregion
    }
}
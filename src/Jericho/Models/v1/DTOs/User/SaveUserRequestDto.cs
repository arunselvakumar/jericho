namespace Jericho.Models.v1.DTOs.User
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class SaveUserRequestDto
    {
        [Required, DataType(DataType.Text)]
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [Required, DataType(DataType.Text)]
        [JsonProperty(PropertyName = "firstname")]
        public string FirstName { get; set; }

        [Required, DataType(DataType.Text)]
        [JsonProperty(PropertyName = "lastname")]
        public string LastName { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        [JsonProperty(PropertyName = "email")]
        public string EMail { get; set; }

        [Required, DataType(DataType.Password)]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}
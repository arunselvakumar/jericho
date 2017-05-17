namespace Jericho.Models.V1.DTOs.User.Request
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public sealed class ChangePasswordRequestDto
    {
        [Required, DataType(DataType.Text)]
        [JsonProperty(PropertyName = "oldpassword")]
        public string OldPassword { get; set; }

        [Required, DataType(DataType.Password)]
        [JsonProperty(PropertyName = "newpassword")]
        public string NewPassword { get; set; }
    }
}

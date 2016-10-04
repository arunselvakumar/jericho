using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Jericho.Models.v1.DTOs.User
{
    public sealed class ChangePasswordRequestDto
    {
        #region Public Properties

        [Required, DataType(DataType.Text)]
        [JsonProperty(PropertyName = "oldpassword")]
        public string OldPassword { get; set; }

        [Required, DataType(DataType.Password)]
        [JsonProperty(PropertyName = "newpassword")]
        public string NewPassword { get; set; }

        #endregion
    }
}

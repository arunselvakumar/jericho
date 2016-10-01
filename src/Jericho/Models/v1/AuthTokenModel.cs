namespace Jericho.Models.v1
{
    using System;

    using Newtonsoft.Json;

    public sealed class AuthTokenModel
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "expires")]
        public DateTime Expires { get; set; }

        [JsonProperty(PropertyName = "issuedat")]
        public DateTime Issued { get; set; }
    }
}
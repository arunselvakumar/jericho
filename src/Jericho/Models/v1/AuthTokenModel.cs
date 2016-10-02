namespace Jericho.Models.v1
{
    using System;

    using Newtonsoft.Json;
    using System.IdentityModel.Tokens.Jwt;

    public sealed class AuthTokenModel
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "validto")]
        public DateTime ValidTo { get; set; }

        [JsonProperty(PropertyName = "validfrom")]
        public DateTime ValidFrom { get; set; }

        public AuthTokenModel(JwtSecurityToken token)
        {
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

            this.Token = encodedJwt;
            this.ValidTo = token.ValidTo;
            this.ValidFrom = token.ValidFrom;
        }
    }
}
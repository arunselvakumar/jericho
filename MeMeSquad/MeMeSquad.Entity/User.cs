namespace MeMeSquad.Entity
{
    using System;
    using Newtonsoft.Json;

    public class User
    {
        #region Properties

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EMail { get; set; }

        public string Password { get; set; }

        public long PhoneNumber { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.UserName;
        }
        #endregion
    }
}
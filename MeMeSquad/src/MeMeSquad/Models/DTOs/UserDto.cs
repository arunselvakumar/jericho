namespace MeMeSquad.Models.DTOs
{
    public class UserDto
    {
        #region Properties

        public string Id { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EMail { get; set; }

        public string Password { get; set; }

        #endregion

        #region Methods

        public override string ToString()
        {
            return this.UserName;
        }

        #endregion
    }
}
namespace Jericho.Providers
{
    public sealed class Error
    {
        public Error(string code, string description)
        {
            this.Code = code;
            this.Description = description;
        }

        public string Code { get; set; }

        public string Description { get; set; }
    }
}
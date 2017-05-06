namespace Jericho.Providers
{
    using System.Collections.Generic;

    using Jericho.Providers.Interfaces;

    public sealed class ServiceResult<T> : IServiceResult<T>
    {
        public ServiceResult(bool isSuccess)
        {
            this.Succeeded = isSuccess;
        }

        public ServiceResult(bool isSuccess, T value)
        {
            this.Succeeded = isSuccess;
            this.Value = value;
        }

        public ServiceResult(bool isSuccess, IEnumerable<Error> errors)
        {
            this.Succeeded = isSuccess;
            this.Errors = errors;
        }

        public IEnumerable<Error> Errors { get; set; }

        public bool Succeeded { get; set; }

        public T Value { get; set; }

        public string Message { get; set; }
    }
}

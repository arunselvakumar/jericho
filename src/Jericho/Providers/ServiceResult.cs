using System.Collections.Generic;

namespace Jericho.Providers
{
    using System;
    using Jericho.Providers.Interfaces;

    public sealed class ServiceResult<T> : IServiceResult<T>
    {
        public Dictionary<string, string> Errors { get; set; }

        public bool Succeeded { get; set; }

        public T Value { get; set; }

        public String ErrorMessage { get; set; }

        public ServiceResult(bool isSuccess)
        {
            this.Succeeded = isSuccess;
        }

        public ServiceResult(bool isSuccess, T value)
        {
            this.Succeeded = isSuccess;
            this.Value = value;
        }

        public ServiceResult(bool isSuccess, Dictionary<string,string> errors)
        {
            this.Succeeded = isSuccess;
            this.Errors = errors;
        }
    }
}

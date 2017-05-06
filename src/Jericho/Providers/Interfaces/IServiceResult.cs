namespace Jericho.Providers.Interfaces
{
    using System.Collections.Generic;

    public interface IServiceResult<T>
    {
        string Message { get; set; }

        bool Succeeded { get; set; }    

        T Value { get; set; }

        IEnumerable<Error> Errors { get; set; }
    }
}

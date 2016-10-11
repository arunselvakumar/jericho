using System.Collections.Generic;

namespace Jericho.Providers.Interfaces
{
    interface IServiceResult<T>
    {
        bool Succeeded { get; set; }    

        T Value { get; set; }

        Dictionary<string, string> Errors { get; set; }
    }
}

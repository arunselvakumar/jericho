using System.Collections.Generic;
using Jericho.Providers.ServiceResultProvider;

namespace Jericho.Validations.Interfaces
{
    public interface IValidatableEntity 
    {
         IEnumerable<Error> Validate();
    }
}
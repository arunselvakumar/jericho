namespace Jericho.Validations.Interfaces
{
    using System.Collections.Generic;

    using Jericho.Providers;

    public interface IValidatableEntity 
    {
         IEnumerable<Error> Validate();
    }
}
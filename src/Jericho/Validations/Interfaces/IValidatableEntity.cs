using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using Jericho.Models.v1.DTOs.User;

namespace Jericho.Validations.Interfaces
{
    public interface IValidatableEntity 
    {
         Dictionary<string, string> Validate();
    }
}
using Jericho.Providers.ServiceResultProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jericho.Validations
{
    public static class ObjectValidationExtensions
    {
        public static void RequiredValidationRule(this object validateObject, string propertyName, IList<Error> errors)
        {
            var value = validateObject.GetType().GetProperty(propertyName).GetValue(validateObject);

            if (value == null || Convert.ToString(value) == string.Empty)
            {
                errors.Add(new Error($"{propertyName}Required", $"The {propertyName} is Required"));                
            }
        }
    }
}

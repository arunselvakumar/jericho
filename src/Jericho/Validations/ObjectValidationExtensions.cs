using Jericho.Providers.ServiceResultProvider;
using System;
using System.Collections.Generic;

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

        public static void RequiredEnumValidationRule(this object validateObject, string propertyName, IList<Error> errors)
        {
            var value = validateObject.GetType().GetProperty(propertyName).GetValue(validateObject);

            if (Convert.ToString(value) == "None")
            {
                errors.Add(new Error($"{propertyName}Required", $"The {propertyName} is Required"));
            }
        }
    }
}

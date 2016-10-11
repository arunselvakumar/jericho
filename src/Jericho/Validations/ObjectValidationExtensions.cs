using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jericho.Validations
{
    public static class ObjectValidationExtensions
    {
        public static void RequiredValidationRule(this object validateObject, string propertyName, Dictionary<string, string> results)
        {
            var value = validateObject.GetType().GetProperty(propertyName).GetValue(validateObject);

            if (value == null || Convert.ToString(value) == string.Empty)
            {
                results.Add(propertyName, $"The {propertyName} is Required");
            }
        }
    }
}

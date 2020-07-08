using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDiscussion.Controllers
{
    public static class ApiUtils
    {
        public static bool IsModelValid(object model)
        {
            if (model == null)
                return false;

            var context = new ValidationContext(model, null, null);
            //var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(model, context, null, true);

            return isValid;
        }
    }
}

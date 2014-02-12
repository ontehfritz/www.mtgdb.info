using System;
using System.Collections.Generic;
using Nancy.Validation;

namespace MtgDb.Info
{
    public class ErrorUtility
    {
        public static List<string> GetValidationErrors(ModelValidationResult result)
        {
            List<string> errors = new List<string>();

            foreach(var e in result.Errors)
            {
                foreach(var message in e.Value)
                {
                    errors.Add (message.ErrorMessage);
                }
            }

            return errors;
        }
    }
}


using System;
using System.Collections.Generic;
using Nancy.Validation;

namespace mtgdb.info
{
    public class ErrorUtility
    {
        public static List<string> GetValidationErrors(ModelValidationResult result)
        {
            List<string> errors = new List<string>();

            foreach(var e in result.Errors)
            {
                foreach(var member in e.MemberNames)
                {
                    errors.Add(e.GetMessage(member));
                }
            }

            return errors;
        }
    }
}


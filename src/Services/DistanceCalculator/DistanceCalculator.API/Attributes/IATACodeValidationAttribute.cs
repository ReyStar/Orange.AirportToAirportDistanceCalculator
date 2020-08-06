using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace DistanceCalculator.API.Attributes
{
    /// <summary>
    /// Attribute for IATA code validation 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class IATACodeValidationAttribute : ValidationAttribute
    {
        private static readonly Regex IATARegex;
        private const string MatchPattern = "(?=^.{3}$)^([A-Z])+$";
        private const string ErrorMessagePattern = "The {0} must has IATA format: 3 letter in uppercase"; //good practice to put in resources

        static IATACodeValidationAttribute()
        {
            IATARegex = new Regex(MatchPattern);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if(value is string inputString
               && IATARegex.IsMatch(inputString))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.Format(ErrorMessagePattern, validationContext.DisplayName));
        }
    }
}

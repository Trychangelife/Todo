using System;
using System.ComponentModel.DataAnnotations;

namespace Todo.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class NotEmptyStringAttribute : ValidationAttribute
    {
        public NotEmptyStringAttribute()
        {
            ErrorMessage = "The {0} field must not be empty or contain only whitespace.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            if (value is string str)
            {
                str = str.Trim(); // Удаляем пробелы в начале и конце строки

                if (string.IsNullOrWhiteSpace(str))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}

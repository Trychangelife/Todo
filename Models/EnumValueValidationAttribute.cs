using System.ComponentModel.DataAnnotations;

namespace Todo.Models.Validation {
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EnumValueValidationAttribute : ValidationAttribute
    {
        private readonly Type _enumType;

        public EnumValueValidationAttribute(Type enumType)
        {
            _enumType = enumType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Enum.IsDefined(_enumType, value))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"Invalid value for {_enumType.Name}.");
        }
    }
}


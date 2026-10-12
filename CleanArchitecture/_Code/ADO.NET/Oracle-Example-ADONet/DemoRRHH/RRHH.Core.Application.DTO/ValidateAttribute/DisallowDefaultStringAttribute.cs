using System.ComponentModel.DataAnnotations;

namespace RRHH.Core.Application.DTO.ValidateAttribute
{

    public class DisallowDefaultStringAttribute : ValidationAttribute
    {
        private readonly string _defaultValue;

        public DisallowDefaultStringAttribute(string defaultValue)
        {
            _defaultValue = defaultValue;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"El campo no puede tener el valor por defecto '{_defaultValue}'."
                : ErrorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Si el valor es nulo, la validación pasa (delegamos la obligatoriedad al atributo [Required])
            if (value is null) return ValidationResult.Success;

            // Comprobación directa y limpia usando pattern matching
            if (value is string strValue && string.Equals(strValue, _defaultValue, StringComparison.OrdinalIgnoreCase))
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }

}

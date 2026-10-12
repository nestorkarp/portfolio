using ServicesSwF.Helper.Validator;
using System.ComponentModel.DataAnnotations;

namespace RRHH.Core.Application.DTO.ValidateAttribute
{

    public class MinAgeAttribute : ValidationAttribute
    {
        private readonly int _minAge;

        // 1. El constructor solo guarda el parámetro de negocio
        public MinAgeAttribute(int minAge)
        {
            _minAge = minAge;
        }

        // 2. Este método nativo define el mensaje por defecto si el usuario no provee uno en el DTO
        public override string FormatErrorMessage(string name)
        {
            return string.IsNullOrEmpty(ErrorMessage)
                ? $"La persona debe tener al menos {_minAge} años."
                : ErrorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Soporte para nulos o tipos incorrectos (deja que [Required] maneje si es obligatorio)
            if (value == null) return ValidationResult.Success;

            if (value is not DateOnly birthDate)
            {
                return new ValidationResult("[ Fecha de Nacimiento ] Inválido.");
            }

            if  (! (Date.IsEarlierThanSystemDate(birthDate)) )
            {
                return new ValidationResult("[ Fecha de Nacimiento ] NO puede ser del futuro.");
            }

            return (Age.IsMaximumAgeLimit(birthDate,_minAge))
                   ? ValidationResult.Success
                   : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }

}


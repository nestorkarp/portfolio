using System.ComponentModel.DataAnnotations;
using ServicesSwF.Helper.Validator;

namespace RRHH.Core.Application.DTO.ValidateAttribute
{
    public class CuitOrCuilAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (!Tax.IsValidCuitOrCuil(value))
            {
                return new ValidationResult("[ CUIT/CUIL ] Inválido.");
            }

            return ValidationResult.Success;
        }
    }
}
 
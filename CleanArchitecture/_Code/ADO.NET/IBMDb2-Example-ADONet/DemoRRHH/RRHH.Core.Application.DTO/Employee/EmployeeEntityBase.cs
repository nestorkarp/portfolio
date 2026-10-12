using RRHH.Core.Application.DTO.ValidateAttribute;
using System.ComponentModel.DataAnnotations;

namespace RRHH.Core.Application.DTO.Employee
{
    public  class EmployeeEntityBase
    {

        [Required(ErrorMessage = "Requerido [ Nombre ]")]
        [StringLength(30, ErrorMessage = "Nombre [Máximo= 30] caracteres")]
        [DisallowDefaultString("string", ErrorMessage = "[ Nombre ] Valor Inválido.")]
        public string Firstname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Requerido [ Apellido ]")]
        [StringLength(30, ErrorMessage = "Apellido [Máximo= 30] caracters")]
        [DisallowDefaultString("string", ErrorMessage = "[ Apellido ] Valor Inválido.")]
        public string Lastname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Requerido [ Fecha de Nacimiento ]")]
        [DataType(DataType.Date)]
        [MinAge(15, ErrorMessage = "[ Edad ] Mínimo 15 años.")]
        public DateOnly Birthdate { get; set; }

        public int Active { get; set; } = 1;

    }
}

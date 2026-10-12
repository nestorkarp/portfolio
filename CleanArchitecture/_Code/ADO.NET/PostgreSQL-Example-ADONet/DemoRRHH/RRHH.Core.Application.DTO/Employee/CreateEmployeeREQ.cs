using RRHH.Core.Application.DTO.ValidateAttribute;
using System.ComponentModel.DataAnnotations;

namespace RRHH.Core.Application.DTO.Employee
{
    public class CreateEmployeeDTObyREQ : EmployeeEntityBase
    {

        [Required(ErrorMessage = "Requerido [ CUIL|CUIT ]")]
        [CuitOrCuilAttribute(ErrorMessage = "CUIT/CUIL ingresado Incorrecto.")]
        public string IdentityCard { get; set; } = null!;
    }
    

    public class CreateEmployeeREQ
    {
        public CreateEmployeeDTObyREQ Employee { get; set; }
    }
}

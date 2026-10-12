namespace RRHH.Core.Application.DTO.Employee
{
    public class UpdateEmployeeDTObyREQ : EmployeeEntityBase
    {
           public int Id { get; set; }
    }


    public class UpdateEmployeeREQ
    {
        public required UpdateEmployeeDTObyREQ Employee { get; set; }
    }
}

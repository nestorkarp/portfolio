namespace RRHH.Core.Application.DTO.Employee
{
    public class EmployeeDTO 
    {
        public int employeeId { get; set; }
        public string identitycard { get; set; }  
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string birthdate { get; set; }
        public bool active { get; set; }
        public string auditdatetime { get; set; }

    }
 

    public class ReadEmployeeRESP : ResponseDTO
    {
    }

}

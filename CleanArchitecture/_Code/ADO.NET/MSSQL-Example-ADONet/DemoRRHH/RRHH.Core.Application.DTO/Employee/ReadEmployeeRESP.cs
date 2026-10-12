namespace RRHH.Core.Application.DTO.Employee
{
    public class EmployeeDTO
    {
        public int EmployeeId { get; set; }
        public string Identitycard { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Birthdate { get; set; }
        public bool Active { get; set; }
        public string Audit_Datetime { get; set; }

    }


    public class ReadEmployeeRESP : ResponseDTO
    {
    }

}
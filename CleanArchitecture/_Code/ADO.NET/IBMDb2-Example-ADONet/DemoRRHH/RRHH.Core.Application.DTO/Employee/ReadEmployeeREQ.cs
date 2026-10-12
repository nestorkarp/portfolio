namespace RRHH.Core.Application.DTO.Employee
{
    public class ReadEmployeeDTObyREQ
    {
        public int Id { get; set; } = 0;
        public string IdentityCard { get; set; } = string.Empty;
        public int? Active { get; set; } = null;
    }


    public class ReadEmployeeREQ
    {
        public required ReadEmployeeDTObyREQ Employee { get; set; }
    }
}

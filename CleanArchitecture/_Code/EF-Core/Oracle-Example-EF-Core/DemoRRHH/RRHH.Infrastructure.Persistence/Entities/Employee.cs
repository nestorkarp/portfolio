namespace RRHH.Infrastructure.Persistence.Entities;

public class Employee
{
    public int EleId { get; set; }

    public string EleIdentitycard { get; set; } = null!;

    public string EleFirstname { get; set; } = null!;

    public string EleLastname { get; set; } = null!;

    public DateTime EleBirthdate { get; set; }

    public bool EleActive { get; set; }
    
    public DateTime EleAuditDatetime { get; set; }
}

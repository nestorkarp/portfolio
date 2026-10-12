using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RRHH.Infrastructure.Persistence.Entities;


namespace RRHH.Infrastructure.Persistence.Configuration.Tables
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {

        public void Configure(EntityTypeBuilder<Employee> entity)
        {
            entity.HasKey(e => e.EleId).HasName("EMPLOYEE_PK");

            entity.ToTable("EMPLOYEE");

            entity.Property(e => e.EleId)
               .ValueGeneratedOnAdd()
               .HasColumnType("NUMBER(38)")
               .HasColumnName("ELE_ID");

            entity.Property(e => e.EleIdentitycard)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ELE_IDENTITYCARD");
            
            entity.Property(e => e.EleFirstname)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ELE_FIRSTNAME");
           
            entity.Property(e => e.EleLastname)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("ELE_LASTNAME");

            entity.Property(e => e.EleBirthdate)
                .HasColumnType("DATE")
                .HasColumnName("ELE_BIRTHDATE");

            entity.Property(e => e.EleActive)
             .HasColumnType("NUMBER(1,0)")
             .HasColumnName("ELE_ACTIVE");

            entity.Property(e => e.EleAuditDatetime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP ")
                .HasColumnType("DATE")
                .HasColumnName("ELE_AUDIT_DATETIME");

        }
    }
}
using RRHH.Core.Application.DTO.Employee;

namespace RRHH.Core.Application.Interfaces.Repository
{
    public interface IEmployeeRepository
    {
        Task<CreateEmployeeRESP> AddAsync(CreateEmployeeREQ entity);
        Task<UpdateEmployeeRESP> UpdateAsync(UpdateEmployeeREQ entity);
        Task<ReadEmployeeRESP> GetAsync(ReadEmployeeREQ entity);
    }
}

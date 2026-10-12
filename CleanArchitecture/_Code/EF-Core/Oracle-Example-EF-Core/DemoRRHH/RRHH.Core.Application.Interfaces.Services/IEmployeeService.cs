using RRHH.Core.Application.DTO.Employee;

namespace RRHH.Core.Application.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<CreateEmployeeRESP> Create(CreateEmployeeREQ entity);
        Task<UpdateEmployeeRESP> Update(UpdateEmployeeREQ entity);
        Task<ReadEmployeeRESP> Read(ReadEmployeeREQ entity);
    }
}


using RRHH.Core.Application.DTO.Employee;
using RRHH.Core.Application.Interfaces.Repository;
using RRHH.Core.Application.Interfaces.Services;

namespace RRHH.Core.Domain.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _EmployeeRepository;

        public EmployeeService(IEmployeeRepository EmployeeRepository)
        {
            _EmployeeRepository = EmployeeRepository;
        }


        #region Public method
        public async Task<CreateEmployeeRESP> Create(CreateEmployeeREQ request)
        {
            CreateEmployeeRESP response = new CreateEmployeeRESP();

            response = await _EmployeeRepository.AddAsync(request);

            return response;
        }


        public async Task<UpdateEmployeeRESP> Update(UpdateEmployeeREQ request)
        {
            UpdateEmployeeRESP response = new UpdateEmployeeRESP();

            response = await _EmployeeRepository.UpdateAsync(request);

            return response;
        }


 

        public async Task<ReadEmployeeRESP> Read(ReadEmployeeREQ request)
        {
            ReadEmployeeRESP response = new ReadEmployeeRESP();

            response = await _EmployeeRepository.GetAsync(request);

            return response;
        }

        #endregion



    }

}


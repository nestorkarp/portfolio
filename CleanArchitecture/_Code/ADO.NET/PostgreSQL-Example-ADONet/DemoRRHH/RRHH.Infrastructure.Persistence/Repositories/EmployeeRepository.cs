using RRHH.Core.Application.DTO.Employee;
using RRHH.Core.Application.Interfaces.Repository;
using ServicesSwF.Helper.Application.DTO;
using System.Data.Common;
using System.Reflection;
using SIDR = ServicesSwF.Infrastructure.Data.Repositories;


namespace RRHH.Infrastructure.Persistence.Repositories
{

    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly string _connectionString;

        public EmployeeRepository(DbProviderFactory providerFactory, string connectionString)
        {
            _dbProviderFactory = providerFactory;
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

       

        #region Public method

        public async Task<CreateEmployeeRESP> AddAsync(CreateEmployeeREQ request)
        {
            //1..Validar existencia null
            if (request?.Employee == null)
            {
                return new CreateEmployeeRESP
                {
                    Success = false,
                    ErrorMessageByLog= string.Empty,
                    ErrorMessageByUser = "Datos NO Validos."
                };
            }

            //2..Parametros
            CreateEmployeeDTObyREQ _entityRequest = request.Employee;
            IList<RequestGenericDTO> _request = new List<RequestGenericDTO>();
            _request = getParameters<CreateEmployeeDTObyREQ>(_entityRequest);

            //3..Ejecución Stored Procedure
            string _storedProcedure = "sp_employee_c";
            SIDR.IRepository _repository = new SIDR.Repository(_dbProviderFactory, _connectionString);
            var _response = new ResponseGenericDTO();
            _response = await _repository.InsertAsync(_request, _storedProcedure);

            //4..Cfg respuesta
            var response = new CreateEmployeeRESP();
            response.Success = _response.Success;
            if (!_response.Success)
            {
                response.ErrorMessageByLog = _response.ErrorMessageByLog;
                response.ErrorMessageByUser = _response.ErrorMessageByUser;
            }
            else
            {
                response.Data = _response.Data;
            }

            return response;
        }

     
        public async Task<UpdateEmployeeRESP> UpdateAsync(UpdateEmployeeREQ request)
        {
            //1..Validar existencia null
            if (request?.Employee == null)
            {
                return new UpdateEmployeeRESP
                {
                    Success = false,
                    ErrorMessageByLog = string.Empty,
                    ErrorMessageByUser = "Datos NO Validos."
                };
            }

            //2..Parametros
            UpdateEmployeeDTObyREQ _entityRequest = request.Employee;
            IList<RequestGenericDTO> _request = new List<RequestGenericDTO>();
            _request = getParameters<UpdateEmployeeDTObyREQ>(_entityRequest);

            //3..Ejecución Stored Procedure
            string _storedProcedure = "sp_employee_u";
            SIDR.IRepository _repository = new SIDR.Repository(_dbProviderFactory, _connectionString);
            var _response = new ResponseGenericDTO();
            _response = await _repository.UpdateAsync(_request, _storedProcedure);

            //4..Cfg respuesta
            var response = new UpdateEmployeeRESP();
            response.Success = _response.Success;
            if (!_response.Success)
            {
                response.ErrorMessageByLog = _response.ErrorMessageByLog;
                response.ErrorMessageByUser = _response.ErrorMessageByUser;
            }
           
            return response;
        }


        public async Task<ReadEmployeeRESP> GetAsync(ReadEmployeeREQ request)
        {
            //1..Validar existencia null
            if (request?.Employee == null)
            {
                return new ReadEmployeeRESP
                {
                    Success = false,
                    ErrorMessageByLog = string.Empty,
                    ErrorMessageByUser = "Datos NO Validos."
                };
            }

            //2..Parametros
            ReadEmployeeDTObyREQ _entityRequest = request.Employee;
            IList<RequestGenericDTO> _request = new List<RequestGenericDTO>();
            _request = getParameters<ReadEmployeeDTObyREQ>(_entityRequest);

            //3..Ejecución Stored Procedure
            string _query = "SELECT * FROM function_employee_r(@p_id, @p_identitycard, @p_active)";
            SIDR.IRepository _repository = new SIDR.Repository(_dbProviderFactory, _connectionString);
            var _response = new ResponseGenericDTO();
            _response = await _repository.ReadAsyncByQuery<EmployeeDTO>(_request, _query);

            //4..Cfg respuesta
            var response = new ReadEmployeeRESP();
            response.Success = _response.Success;
            if (!_response.Success)
            {
                response.ErrorMessageByLog = _response.ErrorMessageByLog;
                response.ErrorMessageByUser = _response.ErrorMessageByUser;      
            }
            else
            {
                response.Data = _response.Data;
            }

            return response;
        }


        #endregion



        #region Private method

        private IList<RequestGenericDTO> getParameters<T>(T dto)
        {
            IList<RequestGenericDTO> output = new List<RequestGenericDTO>();

            PropertyInfo[] properties = typeof(T).GetProperties();
      
            foreach (var property in properties)
            {
                string _name = property.Name.ToLower();
                var _value = property.GetValue(dto);
                output.Add(new RequestGenericDTO() { PrmtName = _name, PrmtData = _value });
            }

            return output;
        }

        #endregion


    }


}

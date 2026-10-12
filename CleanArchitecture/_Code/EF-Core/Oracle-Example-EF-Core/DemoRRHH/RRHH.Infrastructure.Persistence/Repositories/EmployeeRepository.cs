using Microsoft.EntityFrameworkCore;
using RRHH.Core.Application.DTO.Employee;
using RRHH.Core.Application.Interfaces.Repository;
using RRHH.Infrastructure.Persistence.Configuration;
using RRHH.Infrastructure.Persistence.Entities;


namespace RRHH.Infrastructure.Persistence.Repositories
{

    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;

        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
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

            var response = new CreateEmployeeRESP();

            try
            {
                //2..Validar  existencia de registro
                var identityCardClean = request.Employee.IdentityCard.Trim();

                int _count = await _context.Employee
                                    .CountAsync(x => x.EleIdentitycard == identityCardClean);
                bool exists = _count > 0;
                if (exists)
                {
                    response.Success = false;
                    response.ErrorMessageByLog = string.Empty;
                    response.ErrorMessageByUser = "Registro Existente";
                    return response;
                }

                // 3. Crear objeto 
                string? _formattedDate = request.Employee.Birthdate.ToString("yyyy-MM-dd");
                DateTime _birthdate = Convert.ToDateTime(_formattedDate);
                var newEmployee = new Employee
                {
                    EleIdentitycard = identityCardClean,
                    EleFirstname = request.Employee.Firstname,
                    EleLastname = request.Employee.Lastname,
                    EleBirthdate = _birthdate,
                    EleActive = request.Employee.Active
                };
 

                // 4. Registrar
                await _context.Employee.AddAsync(newEmployee);
                await _context.SaveChangesAsync();

                // 5. Asignar Id generado
                response.Data = newEmployee.EleId;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessageByLog = $"Origen: {ex.Source}. Detalle: {ex.Message}";
                response.ErrorMessageByUser= "Se ha producido un error en el proceso de Registración";
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
                    ErrorMessageByLog= string.Empty,
                    ErrorMessageByUser = "Datos NO Validos."
                };
            }
             
            var response = new UpdateEmployeeRESP();

            try
            {
                // 2. Buscar registro
                var employee = await _context.Employee.FindAsync(request.Employee.Id);
                if (employee == null)
                {
                    response.Success = false;
                    response.ErrorMessageByLog = string.Empty;
                    response.ErrorMessageByUser = "Registro Inexistente";
                    return response;
                }

                // 3. Mapear cambios 
                string? _formattedDate = request.Employee.Birthdate.ToString("yyyy-MM-dd");
                DateTime _birthdate = Convert.ToDateTime(_formattedDate);
                employee.EleFirstname = request.Employee.Firstname;
                employee.EleLastname = request.Employee.Lastname;
                employee.EleBirthdate = _birthdate;
                employee.EleActive = request.Employee.Active;

                // 4. Registrar 
                await _context.SaveChangesAsync();

                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorMessageByLog = $"Origen: {ex.Source}. Detalle: {ex.Message}";
                response.ErrorMessageByUser = "Se ha producido un error en el proceso de Actualización";
            }
           
            return response;
        }


        public async Task<ReadEmployeeRESP> GetAsync(ReadEmployeeREQ request)
        {
            var response = new ReadEmployeeRESP();

            //1..Validar existencia null
            if (request?.Employee == null)
            {
                return new ReadEmployeeRESP
                {
                    Success = false,
                    ErrorMessageByLog= string.Empty,
                    ErrorMessageByUser = "Datos NO Validos."
                };
            }


            try
            {
                // 2. Crear la base de la consulta (IQueryable)
                var query = _context.Employee.AsNoTracking(); // Evita que EF rastree las entidades en memoria

                // 3. Filtros
                if (request.Employee.Id > 0)
                {
                    query = query.Where(x => x.EleId == request.Employee.Id);
                }

                if (!string.IsNullOrWhiteSpace(request.Employee.IdentityCard))
                {
                    var idCard = request.Employee.IdentityCard.Trim();
                    query = query.Where(x => x.EleIdentitycard.Trim() == idCard);
                }

                if (request.Employee.Active.HasValue)
                {
                    query = query.Where(x => x.EleActive == request.Employee.Active.Value);
                }

                // 4. Ejecución consulta
                var result = await query
                    .OrderBy(x => x.EleId)
                    .Select(u => new EmployeeDTO
                    {
                        EmployeeId = u.EleId,
                        Identitycard = u.EleIdentitycard,
                        Firstname = u.EleFirstname,
                        Lastname = u.EleLastname,
                        Birthdate = u.EleBirthdate.ToString("dd-MM-yyyy"),
                        Active = u.EleActive,
                        Audit_Datetime = u.EleAuditDatetime.ToString("dd-MM-yyyy HH:mm:ss")
                    })
                    .ToListAsync();


                // 5. Evaluar resultado
                if (result == null || result.Count == 0)
                {
                    response.Success = false;
                    response.ErrorMessageByLog = string.Empty;
                    response.ErrorMessageByUser = "Registro/s Inexistentes";
                    return response;
                }

                response.Data = result;
                response.Success = true;
            }
            catch (Exception ex)
            {
  		response.Success = false;
                response.ErrorMessageByLog = $"Origen: {ex.Source}. Detalle: {ex.Message}";
                response.ErrorMessageByUser = "Se ha producido un error en el proceso de Consulta";
            }

            return response;
        }


        #endregion
    }


}

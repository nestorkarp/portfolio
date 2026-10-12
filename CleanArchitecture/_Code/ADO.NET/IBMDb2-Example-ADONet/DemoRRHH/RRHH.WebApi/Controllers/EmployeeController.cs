using Microsoft.AspNetCore.Mvc;
using RRHH.Core.Application.DTO.Employee;
using RRHH.Core.Application.Interfaces.Services;
using RRHH.WebApi.ModelState;
using ServicesSwF.Helper.API.DTO;


namespace RRHH.WebApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;

        public readonly IEmployeeService _employeeService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }



        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiCreatedRESP), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiErrorRESP), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeREQ request)
        {
            // 1. Validación del modelo
            if (!ModelState.IsValid)
            {
                var modelStateError = new ModelStateError();
                var errors = modelStateError.Get(ModelState);
                return BadRequest(new ApiErrorRESP(errors));
            }

            // 2. Ejecución del servicio
            var response = await _employeeService.Create(request);

            // 3. Control de errores del servicio
            if (!response.Success)
            {
                // Uso de string.IsNullOrWhiteSpace para evitar excepciones o errores de espacio en blanco
                if (!string.IsNullOrWhiteSpace(response.ErrorMessageByLog))
                {
                    _logger.LogError("Error técnico en Create: {TechMessage}", response.ErrorMessageByLog);
                }

                return BadRequest(new ApiErrorRESP(response.ErrorMessageByUser));
            }

            // 4. Respuesta de creación exitosa
            return StatusCode(StatusCodes.Status201Created, new ApiCreatedRESP
            {
                Id = (int)response.Data
            });
        }



        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(ApiUpdatedRESP), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorRESP), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeREQ request)
        {
            // 1. Validación de concordancia de ID
            if (id != request.Employee.Id)
            {
                return BadRequest(new ApiErrorRESP("El ID de la URL no coincide con el del cuerpo de la solicitud."));
            }

            // 2. Ejecución del servicio
            UpdateEmployeeRESP response = await _employeeService.Update(request);

            // 3. Manejo de errores de negocio o técnicos
            if (!response.Success)
            {
                if (!string.IsNullOrWhiteSpace(response.ErrorMessageByLog))
                {
                    _logger.LogError("Error técnico en Update {TechMessage}", response.ErrorMessageByLog);
                }

                return BadRequest(new ApiErrorRESP(response.ErrorMessageByUser));
            }

            // 4. Respuesta exitosa 
            return Ok(new ApiUpdatedRESP() { });
        }



        [HttpGet("get")]
        [ProducesResponseType(typeof(IEnumerable<EmployeeDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorRESP), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiErrorRESP), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get([FromQuery] ReadEmployeeREQ request)
        {
            // 1. Ejecución del servicio
            ReadEmployeeRESP response = await _employeeService.Read(request);

            // 2. Manejo de errores de negocio o técnicos
            if (!response.Success)
            {
                if (!string.IsNullOrWhiteSpace(response.ErrorMessageByLog))
                {
                    _logger.LogError("Error técnico en Read: {TechMessage}", response.ErrorMessageByLog);
                }
                return BadRequest(new ApiErrorRESP(response.ErrorMessageByUser));
            }

            // 3. Respuesta exitosa
            if (!string.IsNullOrWhiteSpace(response.ErrorMessageByUser))
            {
                return NotFound(response.ErrorMessageByUser);
            }
            else
            {
                return Ok(response.Data);
            }
        }

    }
}

namespace ServicesSwF.Helper.API.DTO
{ 
    public class ApiErrorRESP
    {
        public List<string> Errors { get; set; } = new List<string>();

        // Constructor vacío por defecto
        public ApiErrorRESP() { }

        // Constructor  para un solo error
        public ApiErrorRESP(string error)
        {
            Errors.Add(error);
        }

        // Constructor  para múltiples errores
        public ApiErrorRESP(IEnumerable<string> errors)
        {
            Errors.AddRange(errors);
        }
    }
}

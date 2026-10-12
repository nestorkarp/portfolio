namespace ServicesSwF.Helper.Application.DTO
{
    public class ResponseGenericDTO
    {
        public enum ErrorType
        {
            Technical,
            Validation,
            Nothing
        }

        public bool Success { get; set; }
        public object? Data { get; set; }
        public ErrorType ErrorTypeMessage { get; set; }
        public string? ErrorMessageByLog { get; set; }
        public string? ErrorMessageByUser { get; set; }

    }
}

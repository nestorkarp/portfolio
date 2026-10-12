namespace ServicesSwF.Helper.Validator
{
    public class Date
    {
       
        public static bool IsEarlierThanSystemDate(DateOnly value)
        {
            // Retorna true si 'value' es anterior o igual a la fecha del sistema, 
            // y false si 'value' es mayor (futura).
            return value < DateOnly.FromDateTime(DateTime.Today);
        }


    }
}

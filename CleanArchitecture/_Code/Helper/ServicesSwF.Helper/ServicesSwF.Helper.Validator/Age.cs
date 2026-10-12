namespace ServicesSwF.Helper.Validator
{
    public class Age
    {
        public static bool IsMaximumAgeLimit(DateOnly value, int  minAge)
        {
            bool result = false;
  
            DateOnly _today = DateOnly.FromDateTime(DateTime.Today);
            int _age = _today.Year - value.Year;
            if (value > _today.AddYears(-_age))
            {
                _age--;
            }

            result = (_age >= minAge);

            return result;
        }
    }
}

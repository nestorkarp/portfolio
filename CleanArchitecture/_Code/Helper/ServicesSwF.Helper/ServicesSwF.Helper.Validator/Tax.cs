using System.Text.RegularExpressions;

namespace ServicesSwF.Helper.Validator
{
    public class Tax
    {
        public static bool IsValidCuitOrCuil(object? identificador)
        {
            if (identificador is not string _value || string.IsNullOrWhiteSpace(_value))
                return false;

            // Quitar guiones u otros caracteres no numéricos
            string _identificadorSoloNros = Regex.Replace(_value, @"[^\d]", "");

            if (_identificadorSoloNros.Length != 11)
                return false;

            int[] multiplicadores = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
            int suma = 0;

            for (int i = 0; i < 10; i++)
            {
                suma += (_identificadorSoloNros[i] - '0') * multiplicadores[i];
            }

            int resto = suma % 11;
            int digitoCalculado = resto == 0 ? 0 : resto == 1 ? 9 : 11 - resto;
            int digitoReal = _identificadorSoloNros[10] - '0';

            return (digitoCalculado == digitoReal);
        }
    }
}

using System.Data.Common;

namespace ServicesSwF.Infrastructure.Data.Repositories.Helper
{
    public static class DbCommandExtensions
    {
        public static T? GetOutputValue<T>(this DbCommand command, string paramName)
        {
            string commandTypeName = command.GetType().Name;
          
            if (!command.Parameters.Contains(paramName)) return default;

            var value = command.Parameters[paramName].Value;
            if (value == DBNull.Value || value == null) return default;

            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}

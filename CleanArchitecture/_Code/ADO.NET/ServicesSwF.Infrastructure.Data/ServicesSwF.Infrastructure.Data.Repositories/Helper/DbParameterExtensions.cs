using System.Data.Common;

namespace ServicesSwF.Infrastructure.Data.Repositories.Helper
{
    public static class DbParameterExtensions
    {
        public static string GetParameterPrefix(DbCommand command)
        {
            string commandTypeName = command.GetType().Name;
            return commandTypeName switch
            {
                "SqlCommand" => "@",
                _ => "p_"
            };
        }

        public static string GetParameterOutId()
        {
            return "Id";
        }
    }
}

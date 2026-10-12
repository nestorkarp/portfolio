using System.Data;
using System.Data.Common;
using System.Reflection;

namespace ServicesSwF.Infrastructure.Data.Repositories.Helper
{

    public class DbCommandBuilder 
    {
        private readonly DbCommand _command;
        private readonly string _paramPrefix;


        #region Public method

        public static DbCommandBuilder Create(DbCommand command, string storedProcedure, bool isCommandTypeStoredProcedure = true)
        {
            return new DbCommandBuilder(command, storedProcedure, isCommandTypeStoredProcedure);
        }

        public DbCommandBuilder WithInputParam(string name, object? value)
        {
            var parameter = _command.CreateParameter();
            parameter.ParameterName= _paramPrefix + name;
            parameter.Value = value ?? DBNull.Value;
            parameter.Direction = ParameterDirection.Input;
            _command.Parameters.Add(parameter);
            return this;
        }



        public DbCommandBuilder WithOutputParam(string name, DbType dbType, int size = 0)
        {
            var parameter = _command.CreateParameter();
            parameter.ParameterName = _paramPrefix + name;
            parameter.DbType = dbType;
            parameter.Direction = ParameterDirection.Output;
            if (size == 0)
            {
                if (_command.GetType().Name.Trim() == "SqlCommand")
                { 
                    if (dbType == DbType.String)
                    {
                        size = 255;
                    }
                };
            }

            parameter.Size = size;
            _command.Parameters.Add(parameter);
            return this;
        }



        /// <summary>
        /// Agrega un parámetro específico de un proveedor (ej: Oracle RefCursor) utilizando Reflexión
        /// para evitar el acoplamiento rígido en tiempo de compilación.
        /// </summary>
        public DbCommandBuilder WithProviderSpecificOutputParam(string name, string providerDbTypeName)
        {       
            var parameter = _command.CreateParameter();
            parameter.ParameterName = name;
            parameter.Direction = ParameterDirection.Output;
             
            // Buscamos si el parámetro nativo tiene una propiedad específica del motor (ej: OracleDbType)
            PropertyInfo? providerTypeProp = parameter.GetType().GetProperty("OracleDbType")
                                          ?? parameter.GetType().GetProperty("MySqlDbType")
                                          ?? parameter.GetType().GetProperty("NpgsqlDbType")
                                          ?? parameter.GetType().GetProperty("SqlDbType")
                                          ?? parameter.GetType().GetProperty("DB2Type");

            if (providerTypeProp != null)
            {
                try
                {
                    // Obtenemos el Enum específico del motor (ej: OracleDbType.RefCursor)
                    var enumType = providerTypeProp.PropertyType;
                    var enumValue = Enum.Parse(enumType, providerDbTypeName, ignoreCase: true);
                    providerTypeProp.SetValue(parameter, enumValue);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"No se pudo mapear el tipo específico '{providerDbTypeName}' para el parámetro '{name}'.", ex);
                }
            }
            else
            {
                // Fallback si el motor no requiere tipos especiales
                parameter.DbType = DbType.Object;
            }

            _command.Parameters.Add(parameter);
            return this;
        }


        public DbCommandBuilder WithErrorOutputs(string commandType, bool isConvertToLowercase)
        {
            return this
                .WithOutputParam(getParameterErrorMessage(isConvertToLowercase), DbType.String)
                .WithOutputParam(getParameterErrorSource(isConvertToLowercase), DbType.String);
        }

        public string getParameterErrorMessage(bool isConvertToLowercase)
        {
            string output = (!isConvertToLowercase) ? "ErrorMessage" : "errormessage";
            return output; 
        }

        public string getParameterErrorSource(bool isConvertToLowercase)
        {
            string output = (!isConvertToLowercase) ? "ErrorSource" : "errorsource";
            return output;
        }


        public DbCommandBuilder WithOutputIdentity(string commandType, string parameter)
        {
            if ((commandType == "NpgsqlCommand") || (commandType == "DB2Command"))
            {
                return this.WithProviderSpecificOutputParam(_paramPrefix + parameter, "Integer");
            }
            else
            {
                 return this.WithOutputParam(parameter, DbType.Decimal);
            }

        }

        public string GetIdOutput()
        {
            return DbParameterExtensions.GetParameterOutId();
        }

        public DbCommand Build()
        {
            return _command;
        }

        #endregion



        #region Private method
        private DbCommandBuilder(DbCommand command, string storedProcedure, bool isCommandTypeStoredProcedure)
        {
            _command = command ?? throw new ArgumentNullException(nameof(command));
            _command.CommandText = storedProcedure;
            _command.CommandType = (isCommandTypeStoredProcedure) ? CommandType.StoredProcedure: CommandType.Text;
            _paramPrefix = DbParameterExtensions.GetParameterPrefix(command);
        }


        #endregion
    }

}

using ServicesSwF.Helper.Application.DTO;
using ServicesSwF.Infrastructure.Data.Repositories.Helper;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace ServicesSwF.Infrastructure.Data.Repositories
{
    public class Repository : BaseRepository, IRepository
    {
        private const string PrefixErrorTypeValidation = "V";
        private const string MessageByUserStd= "Se ha producido un error inesperado en el sistema.";


        #region Public method
        public Repository(DbProviderFactory providerFactory, string connectionString) : base(providerFactory, connectionString)
        {

        }

        
        public async Task<ResponseGenericDTO> InsertAsync(IList<RequestGenericDTO> request, string storedProcedure)
        {
            ResponseGenericDTO response = new ResponseGenericDTO();

            try
            {
                using (var connection = await GetOpenConnectionAsync())
                {
                    using var rawCommand = CreateCommand(connection);
                    string _commandTypeName = rawCommand.GetType().Name;
                    bool isConvertToLowercase = (_commandTypeName != "NpgsqlCommand") ? false : true;

                    var builder = Helper.DbCommandBuilder.Create(rawCommand, storedProcedure);

                    foreach (var item in request)
                    {
                        builder.WithInputParam(item.PrmtName, item.PrmtData);
                    }

                    string _prmtIdentity = (!isConvertToLowercase) ? builder.GetIdOutput() : builder.GetIdOutput().ToLower();

                    if (_prmtIdentity.Trim().Length > 0)
                    {
                        builder.WithOutputIdentity(_commandTypeName, _prmtIdentity);
                    }

                    using var command = builder
                        .WithErrorOutputs(_commandTypeName, isConvertToLowercase)
                        .Build();
                     
                    await command.ExecuteNonQueryAsync();
                  
                    string? _errorSource = command.GetOutputValue<string?>(builder.getParameterErrorSource(isConvertToLowercase));
                    if (_errorSource != null)
                    {
                        string _messsage= command.GetOutputValue<string?>(builder.getParameterErrorMessage(isConvertToLowercase));
                        response.Success = false;
                        response.ErrorTypeMessage = ((_errorSource.Substring(0, 1) == PrefixErrorTypeValidation) ? ResponseGenericDTO.ErrorType.Validation : ResponseGenericDTO.ErrorType.Technical);
                        if (response.ErrorTypeMessage == ResponseGenericDTO.ErrorType.Technical)
                        {
                            response.ErrorMessageByLog = _messsage;
                            response.ErrorMessageByUser = MessageByUserStd;
                        }
                        else
                        {
                            response.ErrorMessageByUser = _messsage;
                        }

                    }
                    else
                    {

                        response.Success = true;
                        response.ErrorTypeMessage = ResponseGenericDTO.ErrorType.Nothing;
                        string _parameterOutIdentity = DbParameterExtensions.GetParameterPrefix(command) + _prmtIdentity;
                        response.Data = command.GetOutputValue<int>(_parameterOutIdentity);

                    } 
                }
            }

            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorTypeMessage = ResponseGenericDTO.ErrorType.Technical;
                response.ErrorMessageByLog = "Origen: " + ex.Source + " - Detalle: " + ex.Message;
                response.ErrorMessageByUser = MessageByUserStd;
            }

            return response;
        }


        public async Task<ResponseGenericDTO> UpdateAsync(IList<RequestGenericDTO> request, string storedProcedure)
        {
            ResponseGenericDTO response = new ResponseGenericDTO();
          
            try
            {
                using (var connection = await GetOpenConnectionAsync())
                {
                    using var rawCommand = CreateCommand(connection);
                    string _commandTypeName = rawCommand.GetType().Name;
                    bool isConvertToLowercase = (_commandTypeName != "NpgsqlCommand") ? false : true;

                    var builder = Helper.DbCommandBuilder.Create(rawCommand, storedProcedure);

                    foreach (var item in request)
                    {
                        builder.WithInputParam(item.PrmtName, item.PrmtData);
                    }

                    using var command = builder
                        .WithErrorOutputs(_commandTypeName, isConvertToLowercase)
                        .Build();

                    await command.ExecuteNonQueryAsync();

                    string? _errorSource = command.GetOutputValue<string?>(builder.getParameterErrorSource(isConvertToLowercase));
                    if (_errorSource != null)
                    {
                        string _messsage = command.GetOutputValue<string?>(builder.getParameterErrorMessage(isConvertToLowercase));
                        response.Success = false;
                        response.ErrorTypeMessage = ((_errorSource.Substring(0, 1) == PrefixErrorTypeValidation) ? ResponseGenericDTO.ErrorType.Validation : ResponseGenericDTO.ErrorType.Technical);
                        if (response.ErrorTypeMessage == ResponseGenericDTO.ErrorType.Technical)
                        {
                            response.ErrorMessageByLog = _messsage;
                            response.ErrorMessageByUser = MessageByUserStd;
                        }
                        else
                        {
                            response.ErrorMessageByUser = _messsage;
                        }
                    }
                    else
                    {
                        response.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorTypeMessage = ResponseGenericDTO.ErrorType.Technical;
                response.ErrorMessageByLog = "Origen: " + ex.Source + " - Detalle: " + ex.Message;
                response.ErrorMessageByUser = MessageByUserStd;
            }


            return response;
        }


 
        public async Task<ResponseGenericDTO> ReadAsync<T>(IList<RequestGenericDTO> request, string storedProcedure)
        {
            ResponseGenericDTO response = new ResponseGenericDTO();

            try
            {
                using (var connection = await GetOpenConnectionAsync())
                {
                    using var rawCommand = CreateCommand(connection);
                    string _commandTypeName = rawCommand.GetType().Name;
                    bool isConvertToLowercase = (_commandTypeName != "NpgsqlCommand") ? false : true;

                    var builder = Helper.DbCommandBuilder.Create(rawCommand, storedProcedure, true);

                    foreach (var item in request)
                    {
                        builder.WithInputParam(item.PrmtName, item.PrmtData);
                    }


                    // Oracle: Se añade dinámicamente el cursor de salida  
                    if (_commandTypeName == "OracleCommand") 
                    {
                        // "RefCursor" es mapeado internamente a OracleDbType.RefCursor mediante reflexión
                        builder.WithProviderSpecificOutputParam("p_cursor", "RefCursor");
                    }

                    using var command = builder
                        .WithErrorOutputs(_commandTypeName, isConvertToLowercase)
                        .Build();

                    //Buid Datatable
                    response = getDataTable<T>();
                    if (!response.Success) return response;

                    DataTable _dt = (DataTable)response.Data;
                    //..

                    using (var dr = await command.ExecuteReaderAsync())
                    {
                        _dt.Load(dr);

                        string? _errorSource = command.GetOutputValue<string?>(builder.getParameterErrorSource(isConvertToLowercase));
                        if (_errorSource != null)
                        {
                            string _messsage = command.GetOutputValue<string?>(builder.getParameterErrorMessage(isConvertToLowercase));
                            response.Success = false;
                            response.ErrorTypeMessage = ((_errorSource.Substring(0, 1) == PrefixErrorTypeValidation) ? ResponseGenericDTO.ErrorType.Validation : ResponseGenericDTO.ErrorType.Technical);
                            if (response.ErrorTypeMessage == ResponseGenericDTO.ErrorType.Technical)
                            {
                                response.ErrorMessageByLog = _messsage;
                                response.ErrorMessageByUser = MessageByUserStd;
                            }
                            else
                            {
                                response.ErrorMessageByUser = _messsage;
                            }
                        }
                        else
                        {
                            string errorMsg = string.Empty;
                            IList<T> output = getDataTableMapToList<T>(_dt, ref errorMsg);
                            if (errorMsg.Trim().Length > 0)
                            {
                                string _messsage = command.GetOutputValue<string?>(builder.getParameterErrorMessage(isConvertToLowercase));
                                response.Success = false;
                                response.ErrorTypeMessage = ((_errorSource.Substring(0, 1) == PrefixErrorTypeValidation) ? ResponseGenericDTO.ErrorType.Validation : ResponseGenericDTO.ErrorType.Technical);
                                if (response.ErrorTypeMessage == ResponseGenericDTO.ErrorType.Technical)
                                {
                                    response.ErrorMessageByLog = _messsage;
                                    response.ErrorMessageByUser = MessageByUserStd;
                                }
                                else
                                {
                                    response.ErrorMessageByUser = _messsage;
                                }
                            }
                            else
                            {
                                response.Success = true;
                                response.Data = output;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorTypeMessage = ResponseGenericDTO.ErrorType.Technical;
                response.ErrorMessageByLog = "Origen: " + ex.Source + " - Detalle: " + ex.Message;
                response.ErrorMessageByUser = MessageByUserStd;
            }


            return response;
        }



        public async Task<ResponseGenericDTO> ReadAsyncByQuery<T>(IList<RequestGenericDTO> request, string query)
        {
            ResponseGenericDTO response = new ResponseGenericDTO();

            try
            {
                using (var connection = await GetOpenConnectionAsync())
                {
                    using var rawCommand = CreateCommand(connection);

                    var builder = Helper.DbCommandBuilder.Create(rawCommand, query, false);

                    foreach (var item in request)
                    {
                        builder.WithInputParam(item.PrmtName, item.PrmtData);
                    }

                    using var command = builder.Build();

                    //Buid Datatable
                    response = getDataTable<T>();
                    if (!response.Success) return response;

                    DataTable _dt = (DataTable)response.Data;
                    //..

                    using (var dr = await command.ExecuteReaderAsync())
                    {
                        _dt.Load(dr);

                        bool _returnData = false;

                        if (_dt.Rows.Count > 0)
                        {
                            _returnData = true;
                        }
                        else
                        {
                            response.Success = false;
                            response.ErrorMessageByUser = "No se han encontrado registros con los parametro/s requerido/s.";
                            response.ErrorTypeMessage = ResponseGenericDTO.ErrorType.Validation;
                        }

                        if (_returnData)
                        {
                            string errorMsg = string.Empty;
                            IList<T> output = getDataTableMapToList<T>(_dt, ref errorMsg);
                            if (errorMsg.Trim().Length > 0)
                            {
                                response.Success = false;
                                response.ErrorMessageByLog = errorMsg;
                                response.ErrorTypeMessage = ResponseGenericDTO.ErrorType.Technical;
                                response.ErrorMessageByUser = MessageByUserStd;
                            }
                            else
                            {
                                response.Success = true;
                                response.Data = output;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.ErrorTypeMessage = ResponseGenericDTO.ErrorType.Technical;
                response.ErrorMessageByLog = "Origen: " + ex.Source + " - Detalle: " + ex.Message;
                response.ErrorMessageByUser = MessageByUserStd;
            }

            return response;
        }



        #endregion


        #region Private method

        #region AutoMapper

        private ResponseGenericDTO getDataTable<T>()
        {
            ResponseGenericDTO response = new ResponseGenericDTO();
            response.Success = true;

            try
            {
                DataTable _dt = new DataTable();

                PropertyInfo[] properties = typeof(T).GetProperties();

                foreach (var property in properties)
                {
                    string _name = property.Name;
                    Type _dataType = property.PropertyType;
                    _dt.Columns.Add(_name, _dataType);
                }
                response.Data = _dt;
            }
            catch (Exception ex)
            {
                response = new ResponseGenericDTO() { Success = false, ErrorMessageByLog = ex.Source + "-" + ex.Message, ErrorTypeMessage = ResponseGenericDTO.ErrorType.Technical };
            }

            return response;
        }


        private IList<T> getDataTableMapToList<T>(DataTable dt, ref string errorMsg)
        {
            IList<T> output = new List<T>();

            T obj = default(T);

            try
            {

                foreach (DataRow row in dt.Rows)
                {
                    obj = Activator.CreateInstance<T>();

                    foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    {
                        if (Equals(row[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, null, null);
                        }
                        else
                        {
                            prop.SetValue(obj, row[prop.Name], null);
                        }
                    }

                    output.Add(obj);
                }

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            return output;
        }

        #endregion

        #endregion

    }
}

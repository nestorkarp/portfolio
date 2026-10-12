using System.Data.Common;

namespace ServicesSwF.Infrastructure.Data.Repositories
{
    public abstract class BaseRepository
    {
        private readonly DbProviderFactory _providerFactory;
        private readonly string _connectionString;

        // Se inyecta la fábrica del motor (SQL, Oracle, MySQL, Postgres, Db2) y el ConnectionString
        protected BaseRepository(DbProviderFactory providerFactory, string connectionString)
        {
            _providerFactory = providerFactory ?? throw new ArgumentNullException(nameof(providerFactory));
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }
 
        protected async Task<DbConnection> GetOpenConnectionAsync()
        {
            var connection = _providerFactory.CreateConnection();
            if (connection == null)
                throw new InvalidOperationException("No se pudo crear la conexión para el proveedor especificado.");

            connection.ConnectionString = _connectionString;
            await connection.OpenAsync();
            return connection;
        }

        
        protected DbCommand CreateCommand(DbConnection connection)
        {
            var command = connection.CreateCommand();
            if (command == null)
                throw new InvalidOperationException("No se pudo crear el comando para el proveedor especificado.");

            return command;
        }
    }

}
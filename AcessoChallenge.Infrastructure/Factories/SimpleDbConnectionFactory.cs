using System.Data;

namespace AcessoChallenge.Infrastructure.Factories
{
    public class SimpleDbConnectionFactory : IDbConnectionFactory
    {
        public string ConnectionString { get; }

        public string ReadConnectionString { get; }

        public SimpleDbConnectionFactory(string connectionString)
        {
            ConnectionString = connectionString;
            ReadConnectionString = connectionString;
        }

        public SimpleDbConnectionFactory(
            string connectionString,
            string readConnectionString)
            : this(connectionString)
        {
            ReadConnectionString = readConnectionString;
        }

        public IDbConnection GetConnection()
        {
            return new Microsoft.Data.SqlClient.SqlConnection(ConnectionString);
        }

        public IDbConnection GetReadConnection()
        {
            return new Microsoft.Data.SqlClient.SqlConnection(ReadConnectionString);
        }
    }
}